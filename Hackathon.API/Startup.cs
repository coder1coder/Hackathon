using System;
using System.Linq;
using Hackathon.API.Extensions;
using Hackathon.BL;
using Hackathon.Common.Configuration;
using Hackathon.DAL;
using Hackathon.DAL.Mappings;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hackathon.API
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appConfig = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));

            var config = new TypeAdapterConfig();

            config.Apply(new EventEntityMapping(), new TeamEntityMapping(), new UserEntityMapping());

            services.AddSingleton(config);
            services.AddSingleton<IMapper, ServiceMapper>();

            // services.AddDbContext<HangFireDbContext>(options =>
            // {
            //     options.UseNpgsql(Configuration.GetConnectionString("JobsDatabaseConnectionString"));
            // });

            bool? isEnableSensitiveDataLogging = Configuration.GetValue<bool>("EnableSensitiveDataLogging");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnectionString"));

                if (appConfig.EnableSensitiveDataLogging == true)
                    options.EnableSensitiveDataLogging();
            });

            services
                .AddDalDependencies()
                .AddBlDependencies()
                .AddApiDependencies();
                // .AddJobsDependencies();

            services.AddCors(options =>
            {
                options.AddPolicy("default", builder =>
                    builder
                        .WithOrigins(appConfig.OriginsOptions.AllowUrls)
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST", "PUT", "OPTIONS"));
            });

            services.AddControllers(options =>
            {
                options.Filters.Add(new MainActionFilter());
            });

            // services.AddJobs(Configuration);
            services.AddSignalR();

            services.AddAuthentication(appConfig);
            services.AddAuthorization();
            services.AddSwagger();
        }

        public void Configure(
            IApplicationBuilder app,
            IServiceProvider serviceProvider,
            ApplicationDbContext dbContext,
            ILogger<Startup> logger,
            IOptions<AppSettings> appSettings)
        {
            var appConfig = appSettings.Value;

            if (!string.IsNullOrWhiteSpace(appConfig.PathBase))
                app.UsePathBase(appConfig.PathBase);

            if (_environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{appConfig?.PathBase?.Trim()}/swagger/v1/swagger.json", "Hackathon.API v1");
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // var messageHubPrefix = Configuration.GetValue<string>("MessageHubPrefix");

            // app.UseCors("default");
            app.UseCors(x =>
            {
                x
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });

            //DbInitializer.Seed(dbContext, logger, administratorDefaultsOptions.Value);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapHub<EventMessageHub>(messageHubPrefix);
            });
        }
    }
}
