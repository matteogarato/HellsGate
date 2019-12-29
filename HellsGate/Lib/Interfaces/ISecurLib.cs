using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HellsGate.Lib.Interfaces
{
    public interface ISecurLib
    {
        public Task<byte[]> EncriptLineAsync(string p_textToEncrypt);

        public Task<bool> CompareHashAsync(byte[] hashBase, byte[] hashToVerify);

        public Task<string> EncryptLineToStringAsync(string p_textToEncrypt);
    }
}