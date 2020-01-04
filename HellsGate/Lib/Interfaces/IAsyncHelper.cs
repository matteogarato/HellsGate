using System;
using System.Threading.Tasks;

namespace HellsGate.Lib.Interfaces
{
    public interface IAsyncHelper
    {
        public TResult RunSync<TResult>(Func<Task<TResult>> func);

        public void RunSync(Func<Task> func);
    }
}