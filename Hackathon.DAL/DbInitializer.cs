using System.Linq;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.User;
using Hackathon.Configuration;
using Hackathon.DAL.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hackathon.DAL;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context, ILogger logger, 
        AdministratorDefaults administratorDefaults,
        IPasswordHashService passwordHashService)
    {
        try
        {
            if (context.Users.Any(x => x.UserName == administratorDefaults.Login)) 
                return;
            
            var user = new UserEntity
            {
                UserName = administratorDefaults.Login,
                PasswordHash = passwordHashService.HashPassword(administratorDefaults.Password),
                FullName = administratorDefaults.Login,
                Email = "administrator@administrator.ru",
                Role = UserRole.Administrator
            };
            context.Add(user);
            context.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, "{Source}. Невозможно выполнить первоначальную инициализацию данных", nameof(DbInitializer));
            throw;
        }
    }
}
