using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compori.Testing.Xunit.AssemblyRunner
{
    /// <summary>
    /// Enum RunnerState
    /// </summary>
    public enum RunnerState : int
    {
        /// <summary>
        /// The runner is in idle state.
        /// </summary>
        Idle = 0,

        /// <summary>
        /// The runner is in running state.
        /// </summary>
        Running = 1,

        /// <summary>
        /// The runner is completed execution.
        /// </summary>
        Complete = 2
    }
}
