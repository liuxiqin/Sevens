using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Messages
{
    public static class MessageUtils
    {
        public static string CurrentResponseRoutingKey = string.Format("{0}_{1}", Environment.MachineName, Process.GetCurrentProcess().Id);
    }
}
