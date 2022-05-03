using System.Linq;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models.User;
using Hackathon.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hackathon.DAL;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context, ILogger logger, AdministratorDefaults administratorDefaults)
    {
        try
        {
            if (!context.Users.Any(x=> x.UserName == administratorDefaults.Login))
            {
                var user = new UserEntity
                {
                    UserName = administratorDefaults.Login,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(administratorDefaults.Password),
                    FullName = administratorDefaults.Login,
                    Email = "administrator@administrator.ru",
                    Role = UserRole.Administrator
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