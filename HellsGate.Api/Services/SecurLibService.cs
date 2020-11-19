using HellsGate.Models.DatabaseModel;
using HellsGate.Services.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HellsGate.Services
{
    public class SecurLibService : ISecurLibService
    {
        private static int IterationCount = 10000;

        public Task<bool> CompareHash(string hash, string toVerify)
        {
            var parts = hash.Split('.', 2);

            if (parts.Length != 2)
            {
                throw new FormatException("Unexpected hash format");
            }

            var salt = Convert.FromBase64String(parts[0]);
            var oldHashed = parts[1];
            var newHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: toVerify,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: IterationCount,
                numBytesRequested: 256 / 8));

            return Task.FromResult(oldHashed == newHashed);
        }

        public Task<string> EncriptLine(string p_textToEncrypt)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: p_textToEncrypt,
               salt: salt,
               prf: KeyDerivationPrf.HMACSHA512,
               iterationCount: IterationCount,
               numBytesRequested: 256 / 8));
            return Task.FromResult($"{Convert.ToBase64String(salt)}.{hashed}");
        }

        public Task<string> EncryptEntityRelation(PersonModel p_UserModel, AutorizationLevelModel p_AuthModel)
        {
            var md5Anagraphic = CreateMD5($"{p_UserModel.Name}{p_UserModel.Email}{p_UserModel.CardNumber?.CardNumber}{p_UserModel.AutorizationLevel.AuthValue}{p_UserModel.AutorizationLevel.Id}");
            var md5Autorization = CreateMD5($"{p_AuthModel.AuthValue}{p_AuthModel.AuthName}{p_AuthModel.Id}");
            var encripted = $"{md5Anagraphic}{md5Autorization}";
            return Task.FromResult(encripted);
        }

        private static Task<string> CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return Task.FromResult(sb.ToString());
            }
        }
    }
}