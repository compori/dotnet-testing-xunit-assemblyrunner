using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Runners;

namespace Compori.Testing.Xunit.AssemblyRunner
{
    /// <summary>
    /// Class Result.
    /// Implements the <see cref="IResult" />
    /// </summary>
    /// <seealso cref="IResult" />
    public class Result : IResult
    {
        /// <summary>
        /// The lock object
        /// </summary>
        private readonly object lockObj = new object();

        /// <summary>
        /// The failed tests count.
        /// </summary>
        private int total = 0;

        /// <summary>
        /// The failed tests count.
        /// </summary>
        private int failed = 0;

        /// <summary>
        /// The skipped tests count.
        /// </summary>
        private int skipped = 0;

        /// <summary>
        /// The skipped tests
        /// </summary>
        private readonly List<TestSkippedInfo> skippedTests = new List<TestSkippedInfo>();

        /// <summary>
        /// The passed tests
        /// </summary>
        private readonly List<TestPassedInfo> passedTests = new List<TestPassedInfo>();

        /// <summary>
        /// The failed tests
        /// </summary>
        private readonly List<TestFailedInfo> failedTests = new List<TestFailedInfo>();

        /// <summary>
        /// The finished tests
        /// </summary>
        private readonly List<TestFinishedInfo> finishedTests = new List<TestFinishedInfo>();

        /// <summary>
        /// The execution time.
        /// </summary>
        private TimeSpan executionTime = TimeSpan.Zero;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="assemblyLocation">The assembly location.</param>
        public Result(string assemblyLocation)
        {
            this.AssemblyLocation = assemblyLocation;
        }

        /// <summary>
        /// Gets the assembly location.
        /// </summary>
        /// <value>The assembly location.</value>
        public string AssemblyLocation { get; private set; }

        /// <summary>
        /// Gets or sets the discovered.
        /// </summary>
        /// <value>The discovered.</value>
        public int Discovered { get; private set; }

        /// <summary>
        /// Gets the count of runnable tests.
        /// </summary>
        /// <value>The count of runnable tests.</value>
        public int Runnable { get; private set; }

        /// <summary>
        /// Gets the total tests count.
        /// </summary>
        /// <value>The total tests.</value>
        public int Total
        {
            get
            {
                lock (lockObj)
                {
                    if(this.total == 0)
                    {
                        return this.FinishedTests.Count;
                    }
                    return total;
                }
            }
        }

        /// <summary>
        /// Gets the failed tests count.
        /// </summary>
        /// <value>The tests failed.</value>
        public int Failed
        {
            get
            {
                lock (lockObj)
                {
                    if (this.failed == 0)
                    {
                        return this.FailedTests.Count;
                    }
                    return failed;
                }
            }
        }

        /// <summary>
        /// Gets the skipped tests count.
        /// </summary>
        /// <value>The tests skipped.</value>
        public int Skipped
        {
            get
            {
                lock (lockObj)
                {
                    if (this.skipped == 0)
                    {
                        return this.SkippedTests.Count;
                    }
                    return skipped;
                }
            }
        }
        
        /// <summary>
        /// Gets the execution time.
        /// </summary>
        /// <value>The execution time.</value>
        public TimeSpan ExecutionTime
        {
            get
            {
                lock (lockObj)
                {
                    if (TimeSpan.Zero.Equals(this.executionTime))
                    {
                        return new TimeSpan(Convert.ToInt64(TimeSpan.TicksPerSecond * this.FinishedTests.Sum(t => t.ExecutionTime)));
                    }
                    return this.executionTime.Duration();
                }
            }
        }

        /// <summary>
        /// Gets the skipped tests.
        /// </summary>
        /// <value>The skipped tests.</value>
        public IList<TestSkippedInfo> SkippedTests { get => this.skippedTests.AsReadOnly(); }

        /// <summary>
        /// Gets the passed tests.
        /// </summary>
        /// <value>The passed tests.</value>
        public IList<TestPassedInfo> PassedTests { get => this.passedTests.AsReadOnly(); }

        /// <summary>
        /// Gets the failed tests.
        /// </summary>
        /// <value>The failed tests.</value>
        public IList<TestFailedInfo> FailedTests { get => this.failedTests.AsReadOnly(); }

        /// <summary>
        /// Gets the finished tests.
        /// </summary>
        /// <value>The finished tests.</value>
        public IList<TestFinishedInfo> FinishedTests { get => this.finishedTests.AsReadOnly(); }

        /// <summary>
        /// Sets the discovery complete information.
        /// </summary>
        /// <param name="testCasesDiscovered">The test cases discovered.</param>
        /// <param name="testCasesToRun">The test cases to run.</param>
        public void SetDiscoveryCompleteInfo(int testCasesDiscovered, int testCasesToRun)
        {
            lock (lockObj)
            {
                this.Runnable = testCasesToRun;
                this.Discovered = testCasesDiscovered;
            }
        }

        /// <summary>
        /// Sets the execution complete information.
        /// </summary>
        /// <param name="info">The information.</param>
        public void SetExecutionCompleteInfo(ExecutionCompleteInfo info)
        {
            lock (lockObj)
            {
                this.failed = info.TestsFailed;
                this.total = info.TotalTests;
                this.skipped = info.TestsSkipped;
                this.executionTime = new TimeSpan(Convert.ToInt64(TimeSpan.TicksPerSecond * info.ExecutionTime));
            }
        }

        /// <summary>
        /// Adds the failed test.
        /// </summary>
        /// <param name="info">The information.</param>
        public void AddFailedTest(TestFailedInfo info)
        {
            lock(this.lockObj)
            {
                this.failedTests.Add(info);
            }
        }

        /// <summary>
        /// Adds the skipped test.
        /// </summary>
        /// <param name="info">The information.</param>
        public void AddSkippedTest(TestSkippedInfo info)
        {
            lock (this.lockObj)
            {
                this.skippedTests.Add(info);
            }
        }

        /// <summary>
        /// Adds the passed test.
        /// </summary>
        /// <param name="info">The information.</param>
        public void AddPassedTest(TestPassedInfo info)
        {
            lock (this.lockObj)
            {
                this.passedTests.Add(info);
            }

        }

        /// <summary>
        /// Adds the finished.
        /// </summary>
        /// <param name="info">The information.</param>
        public void AddFinishedTest(TestFinishedInfo info)
        {
            lock (this.lockObj)
            {
                this.finishedTests.Add(info);
            }
        }
    }
}
