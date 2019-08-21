using MicroServiceDemo.Api.Auth.Security;

namespace MicroServiceDemo.Api.Auth.Abstractions
{
    public interface IPasswordHashService
    {
        string ComputeHash(string password, string salt, int iterations = PasswordHashService.HashIterations, int hashByteSize = PasswordHashService.HashByteSize);
        string GenerateSalt(int saltByteSize = PasswordHashService.SaltByteSize);
        bool VerifyPassword(string password, string salt, string hash);
    }
}