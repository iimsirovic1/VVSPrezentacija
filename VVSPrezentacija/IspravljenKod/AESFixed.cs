
using System;
using System.Security.Cryptography;
using System.Text;

namespace IspravljenKod
{
    public static class AESFixed
    {
        public static void Run()
        {
            // ISPRAVKA CWE-327:
            // AES-GCM umjesto nesigurnog DES algoritma.

            string podatak = "Povjerljivi podaci";
            byte[] data = Encoding.UTF8.GetBytes(podatak);

            // Generisanje sigurnog kljuca i nonce vrijednosti
            byte[] key = RandomNumberGenerator.GetBytes(32);
            byte[] nonce = RandomNumberGenerator.GetBytes(12);

            byte[] encrypted = new byte[data.Length];
            byte[] tag = new byte[16];

            using var aes = new AesGcm(key, 16);

            aes.Encrypt(nonce, data, encrypted, tag);

            Console.WriteLine("AES-GCM sifrirani podaci:");
            Console.WriteLine(Convert.ToHexString(encrypted));

            // Provjera i desifriranje podataka
            byte[] decrypted = new byte[data.Length];
            aes.Decrypt(nonce, encrypted, tag, decrypted);

            Console.WriteLine("Desifrirani podaci:");
            Console.WriteLine(Encoding.UTF8.GetString(decrypted));
        }
    }
}
