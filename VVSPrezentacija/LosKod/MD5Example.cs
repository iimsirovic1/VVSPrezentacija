
using System;
using System.Security.Cryptography;
using System.Text;

namespace LosKod
{
    public static class MD5Example
    {
        public static void Run()
        {
            // CWE-916: Nesigurno hashiranje lozinke
            // MD5 je prebrz i nije pogodan za lozinke.

            string password = "Student123";

            byte[] data = Encoding.UTF8.GetBytes(password);
            byte[] hash = MD5.HashData(data);

            Console.WriteLine("MD5 hash:");
            Console.WriteLine(Convert.ToHexString(hash));
        }
    }
}
