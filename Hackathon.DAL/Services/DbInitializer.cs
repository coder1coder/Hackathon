using System.Linq;
using Hackathon.Common.Configuration;
using Hackathon.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hackathon.DAL.Services;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context, ILogger logger, AdministratorDefaults administratorDefaults)
    {
        try
        {
            if (!context.Users.Any())
            {
                var user = new UserEntity
                {
                    UserName = administratorDefaults.Login,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(administratorDefaults.Password),
                    FullName = administratorDefaults.Login
                };
                context.Add(user);
                context.SaveChanges();
            }
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, "Невозможно выполнить первоначальную инициализацию данных");
            throw;
        }
    }
}