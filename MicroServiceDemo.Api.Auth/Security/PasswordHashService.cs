using System;
using System.Security.Cryptography;
using MicroServiceDemo.Api.Auth.Abstractions;

namespace MicroServiceDemo.Api.Auth.Security
{
    public class PasswordHashService : IPasswordHashService
    {
        public const int SaltByteSize = 24;
        public const int HashByteSize = 24;
        public const int HashIterations = 1001;

        public string ComputeHash(string password, string salt, int iterations = HashIterations, int hashByteSize = HashByteSize)
        {
            var hash = ComputeHashBytes(password, salt, iterations, hashByteSize);
            return Convert.ToBase64String(hash);
        }

        public string GenerateSalt(int saltByteSize = SaltByteSize)
        {
            var salt = new byte[saltByteSize];
            using (var saltGenerator = new RNGCryptoServiceProvider())
            {
                saltGenerator.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        public bool VerifyPassword(string password, string salt, string hashString)
        {
            var hash = Convert.FromBase64String(hashString);
            var computedHash = ComputeHashBytes(password, salt);
            return AreHashesEqual(computedHash, hash);
        }

        //Length constant verification - prevents timing attack
        private bool AreHashesEqual(byte[] firstHash, byte[] secondHash)
        {
            var minLength = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (var i = 0; i < minLength; i++)
                xor |= firstHash[i] ^ secondHash[i];
            return 0 == xor;
        }

        private byte[] ComputeHashBytes(string password, string salt, int iterations = HashIterations, int hashByteSize = HashByteSize)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using (var hashGenerator = new Rfc2898DeriveBytes(password, saltBytes))
            {
                hashGenerator.IterationCount = iterations;
                return hashGenerator.GetBytes(hashByteSize);
            }
        }
    }
}
