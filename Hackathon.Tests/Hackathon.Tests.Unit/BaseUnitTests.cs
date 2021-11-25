using System;
using Hackathon.DAL;
using Hackathon.DAL.Mappings;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hackathon.Tests.Unit
{
    public class BaseUnitTests: IDisposable
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly IMapper Mapper;

        public BaseUnitTests()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder
                .AddJsonFile($"appsettings.Tests.Unit.json", true, false);

            var configurationRoot = configurationBuilder.Build();
            var connectionString = configurationRoot.GetConnectionString("DefaultConnectionString");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(connectionString)
                .LogTo(Console.WriteLine)
                .Options;

            DbContext = new ApplicationDbContext(options);

            try
            {
                //DbContext.Database.EnsureDeleted();
            }
            finally
            {
                DbContext.Database.EnsureCreated();
            }

            var mapperConfig = new TypeAdapterConfig();
            mapperConfig.Apply(new IRegister[]
            {
                new EventEntityMapping(),
                new TeamEntityMapping(),
                new UserEntityMapping()
            });

            Mapper = new Mapper(mapperConfig);
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}