using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HellsGate.Lib.Interfaces
{
    public interface IAsyncHelper
    {
        public TResult RunSync<TResult>(Func<Task<TResult>> func);

        public void RunSync(Func<Task> func);
    }
}