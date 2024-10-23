using Hackathon.Common.Abstraction.User;

namespace Hackathon.BL.Users;

public class PasswordHashService: IPasswordHashService
{
    public bool Verify(string password, string passwordHash)
        => BCrypt.Net.BCrypt.Verify(password, passwordHash);

    public string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);
}
