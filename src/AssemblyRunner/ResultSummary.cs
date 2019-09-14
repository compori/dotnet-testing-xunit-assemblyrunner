using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compori.Testing.Xunit.AssemblyRunner
{
    /// <summary>
    /// Class ResultSummary.
    /// Implements the <see cref="IResultSummary" />
    /// </summary>
    /// <seealso cref="IResultSummary" />
    public class ResultSummary : IResultSummary
    {
        /// <summary>
        /// The results
        /// </summary>
        private readonly Dictionary<string, IResult> results;
        
        /// <summary>
        /// The lock object
        /// </summary>
        private readonly object lockObj = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultSummary"/> class.
        /// </summary>
        public ResultSummary()
        {
            this.results = new Dictionary<string, IResult>();
        }

        /// <summary>
        /// Adds the result.
        /// </summary>
        /// <param name="result">The result.</param>
        public void AddResult(IResult result)
        {
            lock (this.lockObj)
            {
                if (this.results.ContainsKey(result.AssemblyLocation))
                {
                    this.results[result.AssemblyLocation] = result;
                    return;
                }
                this.results.Add(result.AssemblyLocation, result);
            }
        }

        /// <summary>
        /// Adds the specified result.
        /// </summary>
        /// <value>The results.</value>
        public IList<IResult> Results
        {
            get
            {
                lock (this.lockObj)
                {
                    return new List<IResult>(this.results.Values).AsReadOnly();
                }
            }
        }

        /// <summary>
        /// Gets the total tests count.
        /// </summary>
        /// <value>The total.</value>
        public int Total
        {
            get
            {
                lock (this.lockObj)
                {
                    return this.results.Values.Sum(v => v.Total); ;
                }
            }
        }

        /// <summary>
        /// Gets the failed tests count.
        /// </summary>
        /// <value>The failed.</value>
        public int Failed
        {
            get
            {
                lock (this.lockObj)
                {
                    return this.results.Values.Sum(v => v.Failed); ;
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
                lock (this.lockObj)
                {
                    return this.results.Values.Sum(v => v.Skipped); ;
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
                lock (this.lockObj)
                {
                    return new TimeSpan(this.results.Values.Sum(v => v.ExecutionTime.Ticks));
                }
            }
        }
    }
}
