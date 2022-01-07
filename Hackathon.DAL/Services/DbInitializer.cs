using System.Linq;
using Hackathon.DAL;
using Hackathon.DAL.Entities;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Hackathon.DAL.Services;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context, ILogger logger)
    {
        try
        {
            if (!context.Users.Any())
            {
                var user = new UserEntity
                {
                    UserName = "User_123",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("qwerty123"),
                    Email = "email123@email.com",
                    FullName = "UserFullName123"
                };
                context.Add(user);
                context.SaveChanges();
            }
        }
        catch (NpgsqlException e)
        {
            logger.LogError(e, "Can't seed data");
        }
    }
}