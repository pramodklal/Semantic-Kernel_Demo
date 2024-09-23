using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SementicKernalTest
{
    public class GeneralPlugin
    {
        [KernelFunction]
        [Description("Gets the current time.")]
        public TimeSpan GetTime()
        {
            return TimeProvider.System.GetLocalNow().TimeOfDay;
        }

        [KernelFunction]
        [Description("Gets the current date.")]
        public DateTime GetDate()
        {
            return TimeProvider.System.GetLocalNow().Date;
        }
    }
}
