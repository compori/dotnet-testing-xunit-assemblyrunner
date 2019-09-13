using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Runners;

namespace Compori.Testing.Xunit.AssemblyRunner
{
    /// <summary>
    /// Interface IResult
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Gets the assembly location.
        /// </summary>
        /// <value>The assembly location.</value>
        string AssemblyLocation { get; }

        /// <summary>
        /// Gets the skipped tests.
        /// </summary>
        /// <value>The skipped tests.</value>
        IList<TestSkippedInfo> SkippedTests { get; }

        /// <summary>
        /// Gets the passed tests.
        /// </summary>
        /// <value>The passed tests.</value>
        IList<TestPassedInfo> PassedTests { get; }

        /// <summary>
        /// Gets the failed tests.
        /// </summary>
        /// <value>The failed tests.</value>
        IList<TestFailedInfo> FailedTests { get; }

        /// <summary>
        /// Gets the finished tests.
        /// </summary>
        /// <value>The finished tests.</value>
        IList<TestFinishedInfo> FinishedTests { get; }

        /// <summary>
        /// Gets the count of discovered tests.
        /// </summary>
        /// <value>The discovered.</value>
        int Discovered { get; }

        /// <summary>
        /// Gets the count of runnable tests.
        /// </summary>
        /// <value>The runnable.</value>
        int Runnable { get; }

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
