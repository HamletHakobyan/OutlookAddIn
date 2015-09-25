using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtTask.OutlookAddIn.Utilities
{
    public interface IPeriodicRunner
    {
        void Run();
        void Stop();
    }
}
