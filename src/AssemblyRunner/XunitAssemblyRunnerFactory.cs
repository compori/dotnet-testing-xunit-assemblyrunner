using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Runners;

namespace Compori.Testing.Xunit.AssemblyRunner
{
    /// <summary>
    /// Class XunitAssemblyRunnerFactory.
    /// Implements the <see cref="IXunitAssemblyRunnerFactory" />
    /// </summary>
    /// <seealso cref="IXunitAssemblyRunnerFactory" />
    public class XunitAssemblyRunnerFactory : IXunitAssemblyRunnerFactory
    {
        /// <summary>
        /// Creates the real assembly runner for the specified assembly.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>XunitRunners.AssemblyRunner.</returns>
        global::Xunit.Runners.AssemblyRunner IXunitAssemblyRunnerFactory.Create(string assemblyFileName)
        {
            return global::Xunit.Runners.AssemblyRunner.WithoutAppDomain(assemblyFileName);
        }
    }
}
