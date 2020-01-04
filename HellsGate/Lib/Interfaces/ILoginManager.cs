using System.Threading.Tasks;

namespace HellsGate.Lib.Interfaces
{
    public interface ILoginManager
    {
        public Task<string> GetUserByInputAsync(string UserInput);
    }
}