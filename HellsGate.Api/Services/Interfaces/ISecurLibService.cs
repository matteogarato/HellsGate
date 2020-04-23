using System.Threading.Tasks;

namespace HellsGate.Services.Interfaces
{
    public interface ISecurLibService
    {
        public Task<byte[]> EncriptLineAsync(string p_textToEncrypt);

        public Task<bool> CompareHashAsync(byte[] hashBase, byte[] hashToVerify);

        public Task<string> EncryptLineToStringAsync(string p_textToEncrypt);
    }
}