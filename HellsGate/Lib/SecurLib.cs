using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HellsGate.Lib
{
    public static class SecurLib
    {
        public static Task<byte[]> EncriptLineAsync(string p_textToEncrypt)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var salted = new Rfc2898DeriveBytes(p_textToEncrypt, salt, 1000);
            byte[] hash = salted.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Task.FromResult(hashBytes);
        }

        public static Task<bool> CompareHashAsync(byte[] hashBase, byte[] hashToVerify)
        {
            for (int i = 16; i < 20; i++)
            {
                if (hashBase[i] != hashToVerify[i])
                {
                    return Task.FromResult(false);
                }
            }
            return Task.FromResult(true);
        }

        public static async Task<string> EncryptLineToStringAsync(string p_textToEncrypt) => Convert.ToBase64String(await EncriptLineAsync(p_textToEncrypt).ConfigureAwait(false));
    }
}