using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Compori.Testing.Xunit.AssemblyRunner
{
    /// <summary>
    /// Interface IXunitAssemblyRunnerFactory
    /// </summary>
    public interface IXunitAssemblyRunnerFactory
    {
        /// <summary>
        /// Creates the real assembly runner for the specified assembly.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>XunitRunners.AssemblyRunner.</returns>
        global::Xunit.Runners.AssemblyRunner Create(string assemblyFileName);
    }
}
