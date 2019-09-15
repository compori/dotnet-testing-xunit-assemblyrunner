using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit.Abstractions;
using Xunit.Runners;

namespace Compori.Testing.Xunit.AssemblyRunner
{
    /// <summary>
    /// Class Runner.
    /// </summary>
    public class Runner
    {
        /// <summary>
        /// The lock object
        /// </summary>
        private readonly object lockObj = new object();

        /// <summary>
        /// Gets the current runner state.
        /// </summary>
        /// <value>The state.</value>
        public RunnerState State { get; private set; } = RunnerState.Idle;

        /// <summary>
        /// Tries the set runner state running.
        /// </summary>
        /// <returns><c>true</c> if state could be set to running, <c>false</c> otherwise.</returns>
        private bool TrySetRunnerStateRunning()
        {
            lock (lockObj)
            {
                if(this.State != RunnerState.Idle)
                {
                    return false;
                }
                this.State = RunnerState.Running;
                return true;
            }
        }

        /// <summary>
        /// The test results.
        /// </summary>
        public IResultSummary Summary { get => this.summary; }

        /// <summary>
        /// Gets the exclude.
        /// </summary>
        /// <value>The exclude.</value>
        public List<Filter> Exclude { get; private set; } = new List<Filter>();

        /// <summary>
        /// Gets the include.
        /// </summary>
        /// <value>The include.</value>
        public List<Filter> Include { get; private set; } = new List<Filter>();

        /// <summary>
        /// The result
        /// </summary>
        private ResultSummary summary = new ResultSummary();

        /// <summary>
        /// Synchronisation object in order to complete tests.
        /// </summary>
        private readonly ManualResetEvent finished = new ManualResetEvent(false);

        /// <summary>
        /// The assembly locations
        /// </summary>
        private readonly List<string> assemblyLocations;

        /// <summary>
        /// Gets the assembly locations.
        /// </summary>
        /// <value>The assembly locations.</value>
        public IList<string> AssemblyLocations { get => assemblyLocations.AsReadOnly(); }

        /// <summary>
        /// The factory
        /// </summary>
        protected IXunitAssemblyRunnerFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Runner"/> class.
        /// </summary>
        public Runner(IXunitAssemblyRunnerFactory factory, string[] assemblyLocations)
        {
            this.assemblyLocations = new List<string>(assemblyLocations);
            this.factory = factory;
        }

        /// <summary>
        /// Set to get notification of when tests start running.
        /// </summary>
        /// <value>The on test starting.</value>
        public Action<IResult, TestStartingInfo> OnTestStarting { get; set; }

        /// <summary>
        /// Set to get notification of when test discovery for an assembly is complete.
        /// </summary>
        /// <value>The on discovery assembly complete.</value>
        public Action<IResult, DiscoveryCompleteInfo> OnAssemblyDiscoveryComplete { get; set; }

        /// <summary>
        /// Set to get notification of finished tests (regardless of outcome).
        /// </summary>
        /// <value>The on test finished.</value>
        public Action<IResult, TestFinishedInfo> OnTestFinished { get; set; }

        /// <summary>
        /// Set to get notification of when test execution for an assembly is complete.
        /// </summary>
        /// <value>The on complete assembly.</value>
        public Action<IResult, ExecutionCompleteInfo> OnAssemblyComplete { get; set; }

        /// <summary>
        /// Set to get notification of when test execution for all assemblies is complete.
        /// </summary>
        /// <value>The on complete.</value>
        public Action<IResultSummary> OnComplete { get; set; }

        /// <summary>
        /// Set to get notification of skipped tests.
        /// </summary>
        /// <value>The on test skipped.</value>
        public Action<IResult, TestSkippedInfo> OnTestSkipped { get; set; }

        /// <summary>
        /// Set to get notification of passing tests.
        /// </summary>
        /// <value>The on test passed.</value>
        public Action<IResult, TestPassedInfo> OnTestPassed { get; set; }

        /// <summary>
        /// Set to get notification of failed tests.
        /// </summary>
        /// <value>The on test failed.</value>
        public Action<IResult, TestFailedInfo> OnTestFailed { get; set; }

        /// <summary>
        /// Called when from runner if discovery stage is complete.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="info">The discovery complete information.</param>
        private void OnDiscoveryCompleteInternal(Result result, DiscoveryCompleteInfo info)
        {
            result.SetDiscoveryCompleteInfo(info.TestCasesDiscovered, info.TestCasesToRun);

            this.OnAssemblyDiscoveryComplete?.Invoke(result, info);
        }

        /// <summary>
        /// Called when runner's execution is completed.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="info">The execution complete information.</param>
        private void OnExecutionCompleteInternal(Result result, ExecutionCompleteInfo info)
        {
            // Update result values
            result.SetExecutionCompleteInfo(info);

            this.OnAssemblyComplete?.Invoke(result, info);

            //
            // trigger finished event.
            //
            this.finished.Set();
        }

        /// <summary>
        /// Called when the runner starts a new test.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="info">The information.</param>
        private void OnTestStartingInternal(Result result, TestStartingInfo info)
        {
            this.OnTestStarting?.Invoke(result, info);
        }

        /// <summary>
        /// Called when runner finished a test.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="info">The information.</param>
        private void OnTestFinishedInternal(Result result, TestFinishedInfo info)
        {
            result.AddFinishedTest(info);

            this.OnTestFinished?.Invoke(result, info);
        }

        /// <summary>
        /// Called when the test passed.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="info">The information.</param>
        private void OnTestPassedInternal(Result result, TestPassedInfo info)
        {
            result.AddPassedTest(info);

            this.OnTestPassed?.Invoke(result, info);
        }

        /// <summary>
        /// Called when the test failed.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="info">The information.</param>
        private void OnTestFailedInternal(Result result, TestFailedInfo info)
        {
            result.AddFailedTest(info);

            this.OnTestFailed?.Invoke(result, info);
        }

        /// <summary>
        /// Called when the test skipped.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="info">The information.</param>
        private void OnTestSkippedInternal(Result result, TestSkippedInfo info)
        {
            result.AddSkippedTest(info);

            this.OnTestSkipped?.Invoke(result, info);
        }

        /// <summary>
        /// Filters the specified test case.
        /// </summary>
        /// <param name="assemblyLocation">The assembly location.</param>
        /// <param name="testCase">The test case.</param>
        /// <returns><c>true</c> if test should be executed, <c>false</c> otherwise.</returns>
        private bool Filter(string assemblyLocation, ITestCase testCase)
        {
            var testCaseName = testCase.DisplayName;
            var testClassName = testCase.TestMethod.TestClass.Class.Name;
            var traits = testCase.Traits;

            //
            // No filtering active. The default behavior is to execute all discovered filters.
            //
            if (this.Exclude.Count == 0 && this.Include.Count == 0)
            {
                return true;
            }

            //
            // if explicit exclude...
            //
            var excluded = this.Exclude.FirstOrDefault(filter => filter.Match(assemblyLocation, testCaseName, testClassName, traits)) != null;
            if (excluded)
            {
                return false;
            }

            //
            // None explicit included, include all!
            //
            if(this.Include.Count == 0)
            {
                return true;
            }

            return this.Include.FirstOrDefault(filter => filter.Match(assemblyLocation, testCaseName, testClassName, traits)) != null;
        }

        /// <summary>
        /// Executes the test runner.
        /// </summary>
        public void Execute()
        {
            if (!this.TrySetRunnerStateRunning())
            {
                return;
            }

            this.summary = new ResultSummary();

            try
            {

                foreach (var location in this.assemblyLocations)
                {
                    this.finished.Reset();

                    var result = new Result(location);

                    using (var runner = this.factory.Create(result.AssemblyLocation))
                    {
                        runner.TestCaseFilter = testCase => this.Filter(result.AssemblyLocation, testCase);

                        runner.OnDiscoveryComplete = info => this.OnDiscoveryCompleteInternal(result, info); 
                        runner.OnExecutionComplete = info => this.OnExecutionCompleteInternal(result, info);

                        runner.OnTestStarting = info => this.OnTestStartingInternal(result, info);
                        runner.OnTestFinished = info => this.OnTestFinishedInternal(result, info);
                        runner.OnTestFailed = info => this.OnTestFailedInternal(result, info);
                        runner.OnTestSkipped = info => this.OnTestSkippedInternal(result, info);
                        runner.OnTestPassed = info => this.OnTestPassedInternal(result, info);

                        this.summary.AddResult(result);

                        //
                        // - test the whole assembly
                        //
                        runner.Start(null, null, null, null, null, false);

                        //
                        // Wait until all tests are finished.
                        //
                        this.finished.WaitOne();
                        while (runner.Status != AssemblyRunnerStatus.Idle)
                        {
                            Thread.Sleep(100);
                        }
                    }
                }
                this.State = RunnerState.Complete;
            }
            finally
            {
                this.OnComplete?.Invoke(this.summary);
                this.State = RunnerState.Idle;
            }
        }
    }
}
