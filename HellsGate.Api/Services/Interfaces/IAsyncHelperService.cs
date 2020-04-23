using System;
using System.Threading.Tasks;

namespace HellsGate.Services.Interfaces
{
    public interface IAsyncHelperService
    {
        public TResult RunSync<TResult>(Func<Task<TResult>> func);

        public void RunSync(Func<Task> func);
    }
}