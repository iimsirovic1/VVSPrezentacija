
using System;
using System.Security.Cryptography;

namespace IspravljenKod
{
    public static class MD5Fixed
    {
        public static void Run()
        {
            // ISPRAVKA CWE-916:
            // PBKDF2 sa slucajnim saltom i 600000 iteracija.

            string password = "Student123";

            byte[] salt = RandomNumberGenerator.GetBytes(16);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                600000,
                HashAlgorithmName.SHA256,
                32
            );

            Console.WriteLine("PBKDF2 hash:");
            Console.WriteLine(Convert.ToHexString(hash));
        }
    }
}
