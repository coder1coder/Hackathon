using System.Threading.Tasks;
using Hackathon.Common.Abstraction.User;

namespace Hackathon.BL.Users;

public class PasswordHashService: IPasswordHashService
{
    public Task<bool> VerifyAsync(string password, string passwordHash)
        => Task.FromResult(BCrypt.Net.BCrypt.Verify(password, passwordHash));

    public Task<string> HashPasswordAsync(string password)
        => Task.FromResult(BCrypt.Net.BCrypt.HashPassword(password));
}
