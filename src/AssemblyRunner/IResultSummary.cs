using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compori.Testing.Xunit.AssemblyRunner
{
    public interface IResultSummary
    {
        /// <summary>
        /// Gets the results for each Assembly.
        /// </summary>
        /// <value>The results.</value>
        IList<IResult> Results { get; }

        /// <summary>
        /// Gets the total tests count.
        /// </summary>
        /// <value>The total.</value>
        int Total { get; }

        /// <summary>
        /// Gets the failed tests count.
        /// </summary>
        /// <value>The failed.</value>
        int Failed { get; }

        /// <summary>
        /// Gets the skipped tests count.
        /// </summary>
        /// <value>The tests skipped.</value>
        int Skipped { get; }

        /// <summary>
        /// Gets the execution time.
        /// </summary>
        /// <value>The execution time.</value>
        TimeSpan ExecutionTime { get; }
    }
}
