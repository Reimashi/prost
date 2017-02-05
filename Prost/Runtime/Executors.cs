using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prost.Runtime
{
    class Executors
    {
        public static bool ExecuteWithTimeLimit(TimeSpan timeSpan, Action routine)
        {
            try
            {
                Task task = Task.Factory.StartNew(() => routine());
                task.Wait(timeSpan);
                return task.IsCompleted;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerExceptions[0];
            }
        }
    }
}
