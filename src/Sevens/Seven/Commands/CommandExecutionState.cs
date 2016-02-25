using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Commands
{
    /// <summary>
    /// 命令执行状态
    /// </summary>
    public enum CommandExecutionState
    {
        None,
        Resolved,
        Called
    }
}
