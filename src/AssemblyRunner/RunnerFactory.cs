using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compori.Testing.Xunit.AssemblyRunner
{
    /// <summary>
    /// Class RunnerFactory.
    /// </summary>
    public class RunnerFactory
    {
        /// <summary>
        /// The factory
        /// </summary>
        protected IXunitAssemblyRunnerFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunnerFactory"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public RunnerFactory(IXunitAssemblyRunnerFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Creates a runner for specified assembly location.
        /// </summary>
        /// <param name="assemblyLocation">The assembly location.</param>
        /// <returns>Runner.</returns>
        public Runner Create(string assemblyLocation)
        {
            return this.Create(new string[] { assemblyLocation });
        }

        /// <summary>
        /// Creates a runner for all specified assembly locations.
        /// </summary>
        /// <param name="assemblyLocations">The assembly locations.</param>
        /// <returns>Runner.</returns>
        public Runner Create(string[] assemblyLocations)
        {
            return new Runner(this.factory, assemblyLocations);
        }
    }
}
