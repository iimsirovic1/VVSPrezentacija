
using System;
using System.Security.Cryptography;
using System.Text;

namespace LosKod
{
    public static class SHA1Example
    {
        public static void Run()
        {
            // CWE-328: Koristenje slabog hash algoritma
            // SHA-1 nije siguran za zastitu integriteta.

            string podatak = "Vazni podaci";

            byte[] data = Encoding.UTF8.GetBytes(podatak);
            byte[] hash = SHA1.HashData(data);

            Console.WriteLine("SHA-1 hash:");
            Console.WriteLine(Convert.ToHexString(hash));
        }
    }
}
