using Compori.Testing.Xunit.AssemblyRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AssemblyRunnerTests
{
    public class ResultSummaryTest
    {
        [Fact()]
        public void TestAddResult()
        {
            var sut = new ResultSummary();

            Assert.Empty(sut.Results);
            Assert.Equal(TimeSpan.Zero, sut.ExecutionTime);
            Assert.Equal(0, sut.Failed);
            Assert.Equal(0, sut.Skipped);
            Assert.Equal(0, sut.Total);

            var result = new Result("abc.dll");
            result.SetExecutionCompleteInfo(new Xunit.Runners.ExecutionCompleteInfo(10, 5, 2, 1.5M));
            sut.AddResult(result);
#if NET35
            Assert.Equal(1, sut.Results.Count);
#else
            Assert.Single(sut.Results);
#endif
            Assert.Equal(new TimeSpan(0, 0, 0, 1, 500), sut.ExecutionTime);
            Assert.Equal(10, sut.Total);
            Assert.Equal(5, sut.Failed);
            Assert.Equal(2, sut.Skipped);

            result = new Result("abc.dll");
            result.SetExecutionCompleteInfo(new Xunit.Runners.ExecutionCompleteInfo(15, 8, 3, 3.512M));
            sut.AddResult(result);
#if NET35
            Assert.Equal(1, sut.Results.Count);
#else
            Assert.Single(sut.Results);
#endif
            Assert.Equal(new TimeSpan(0, 0, 0, 3, 512), sut.ExecutionTime);
            Assert.Equal(15, sut.Total);
            Assert.Equal(8, sut.Failed);
            Assert.Equal(3, sut.Skipped);

            result = new Result("xyz.dll");
            result.SetExecutionCompleteInfo(new Xunit.Runners.ExecutionCompleteInfo(3, 1, 2, 1M));
            sut.AddResult(result);
            Assert.Equal(2, sut.Results.Count);
            Assert.Equal(new TimeSpan(0, 0, 0, 4, 512), sut.ExecutionTime);
            Assert.Equal(18, sut.Total);
            Assert.Equal(9, sut.Failed);
            Assert.Equal(5, sut.Skipped);
        }
    }
}
