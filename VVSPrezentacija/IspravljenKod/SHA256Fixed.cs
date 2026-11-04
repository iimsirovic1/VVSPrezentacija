
using System;
using System.Security.Cryptography;
using System.Text;

namespace IspravljenKod
{
    public static class SHA256Fixed
    {
        public static void Run()
        {
            // ISPRAVKA CWE-328:
            // SHA-256 umjesto zastarjelog SHA-1.
            // Primjer provjere integriteta podataka.

            string podatak = "Vazni podaci";

            byte[] data = Encoding.UTF8.GetBytes(podatak);
            byte[] hash = SHA256.HashData(data);

            Console.WriteLine("SHA-256 hash:");
            Console.WriteLine(Convert.ToHexString(hash));
        }
    }
}
