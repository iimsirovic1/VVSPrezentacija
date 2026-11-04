
using System;
using System.Security.Cryptography;
using System.Text;

namespace LosKod
{
    public static class DESExample
    {
        public static void Run()
        {
            // CWE-327: Koristenje nesigurnog algoritma
            // DES ima kljuc od samo 56 bita.

            string podatak = "Povjerljivi podaci";
            byte[] data = Encoding.UTF8.GetBytes(podatak);

            using DES des = DES.Create();

            des.GenerateKey();
            des.GenerateIV();

            using var encryptor = des.CreateEncryptor();

            byte[] encrypted = encryptor.TransformFinalBlock(
                data, 0, data.Length);

            Console.WriteLine("DES sifrirani podaci:");
            Console.WriteLine(Convert.ToHexString(encrypted));
        }
    }
}
