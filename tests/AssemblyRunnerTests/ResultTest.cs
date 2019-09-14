using Compori.Testing.Xunit.AssemblyRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Runners;

namespace AssemblyRunnerTests
{
    public class ResultTest
    {

        [Fact()]
        public void TestSetDiscoveryCompleteInfo()
        {
            var assemblyLocation = "abc.dll";
            var discovered = 3;
            var runnable = 2;
            var sut = new Result(assemblyLocation);
            sut.SetDiscoveryCompleteInfo(discovered, runnable);
            Assert.Equal(discovered, sut.Discovered);
            Assert.Equal(runnable, sut.Runnable);
        }

        public static IEnumerable<object[]> GetSetExecutionCompleteInfoData()
        {
            yield return new object[] { 0, 0, 0, TimeSpan.Zero, new ExecutionCompleteInfo(0, 0, 0, 0) };
            yield return new object[] { 5, 2, 1, new TimeSpan(0, 0, 0,4, 500), new ExecutionCompleteInfo(5, 2, 1, 4.5M) };
        }

#if NET35

        [Fact()]
        public void TestSetExecutionCompleteInfo()
        {
            foreach (object[] data in GetSetExecutionCompleteInfoData())
            {
                var total = (int)data[0];
                var failed = (int) data[1];
                var skipped = (int) data[2];
                var executionTime = (TimeSpan)data[3];
                var info = data[4] as ExecutionCompleteInfo;
                this.AssertsTestSetExecutionCompleteInfo(total, failed, skipped, executionTime, info);
            }           
        }

#else

        [Theory(), MemberData(nameof(GetSetExecutionCompleteInfoData))]
        public void TestSetExecutionCompleteInfo(
            int total, int failed, int skipped, TimeSpan executionTime, 
            ExecutionCompleteInfo info)
        {
            this.AssertsTestSetExecutionCompleteInfo(total, failed, skipped, executionTime, info);
        }

#endif
        protected void AssertsTestSetExecutionCompleteInfo(
             int total, int failed, int skipped, TimeSpan executionTime, 
            ExecutionCompleteInfo info)
        {
            var assemblyLocation = "abc.dll";
            var sut = new Result(assemblyLocation);
            sut.SetExecutionCompleteInfo(info);

            Assert.Equal(total, sut.Total);
            Assert.Equal(failed, sut.Failed);
            Assert.Equal(skipped, sut.Skipped);
            Assert.Equal(executionTime, sut.ExecutionTime);
        }

        public static IEnumerable<object[]> GetAddFinishedTestData()
        {
            yield return new object[] { TimeSpan.Zero, new TestFinishedInfo("", "", new Dictionary<string, List<string>>(), "", "", 0M, "") };
            yield return new object[] { new TimeSpan(0, 0, 0, 4, 456), new TestFinishedInfo("", "", new Dictionary<string, List<string>>(), "", "", 4.456M, "") };
        }

#if NET35

        [Fact()]
        public void TestAddFinishedTest()
        {
            foreach (object[] data in GetAddFinishedTestData())
            {
                var executionTime = (TimeSpan)data[0];
                var info = data[1] as TestFinishedInfo;
                this.AssertsTestAddFinishedTest(executionTime, info);
            }
        }

#else

        [Theory(), MemberData(nameof(GetAddFinishedTestData))]
        public void TestAddFinishedTest(TimeSpan executionTime, TestFinishedInfo info)
        {
            this.AssertsTestAddFinishedTest(executionTime, info);
        }

#endif
        protected void AssertsTestAddFinishedTest(TimeSpan executionTime, TestFinishedInfo info)
        {
            var assemblyLocation = "abc.dll";
            var sut = new Result(assemblyLocation);

            Assert.Empty(sut.FinishedTests);

            sut.AddFinishedTest(info);
            Assert.Equal(1, sut.Total);
#if NET35
            Assert.Equal(1, sut.FinishedTests.Count);
#else
            Assert.Single(sut.FinishedTests);
#endif
            Assert.Contains(info, sut.FinishedTests);
            Assert.Equal(executionTime, sut.ExecutionTime);

            var actual = sut.FinishedTests.First();
            Assert.Equal(Convert.ToDecimal(executionTime.TotalSeconds), actual.ExecutionTime);
            Assert.Equal(info.MethodName, actual.MethodName);
            Assert.Equal(info.Output, actual.Output);
            Assert.Equal(info.TestCollectionDisplayName, actual.TestCollectionDisplayName);
            Assert.Equal(info.TestDisplayName, actual.TestDisplayName);
            Assert.Equal(info.TypeName, actual.TypeName);
        }


        public static IEnumerable<object[]> GetAddFailedTestData()
        {
            yield return new object[] { 
                new TestFailedInfo("", "", new Dictionary<string, List<string>>(), "", "", 0M, "", "exType", "exMessage", "exStackTrace") };
            yield return new object[] { 
                new TestFailedInfo("", "", new Dictionary<string, List<string>>(), "", "", 4.456M, "", "exType", "exMessage", "exStackTrace") };
        }

#if NET35

        [Fact()]
        public void TestAddFailedTest()
        {
            foreach (object[] data in GetAddFailedTestData())
            {
                var info = data[0] as TestFailedInfo;
                this.AssertsTestAddFailedTest(info);
            }
        }

#else

        [Theory(), MemberData(nameof(GetAddFailedTestData))]
        public void TestAddFailedTest(TestFailedInfo  info)
        {
            this.AssertsTestAddFailedTest( info);
        }

#endif
        protected void AssertsTestAddFailedTest(TestFailedInfo info)
        {
            var assemblyLocation = "abc.dll";
            var sut = new Result(assemblyLocation);

            Assert.Empty(sut.FailedTests);

            sut.AddFailedTest(info);
            Assert.Equal(1, sut.Failed);
#if NET35
            Assert.Equal(1, sut.FailedTests.Count);
#else
            Assert.Single(sut.FailedTests);
#endif
            Assert.Contains(info, sut.FailedTests);

            var actual = sut.FailedTests.First();
            Assert.Equal(info.MethodName, actual.MethodName);
            Assert.Equal(info.Output, actual.Output);
            Assert.Equal(info.TestCollectionDisplayName, actual.TestCollectionDisplayName);
            Assert.Equal(info.TestDisplayName, actual.TestDisplayName);
            Assert.Equal(info.TypeName, actual.TypeName);

            Assert.Equal(info.ExceptionMessage, actual.ExceptionMessage);
            Assert.Equal(info.ExceptionStackTrace, actual.ExceptionStackTrace);
            Assert.Equal(info.ExceptionType, actual.ExceptionType);
        }


        public static IEnumerable<object[]> GetAddSkippedTestData()
        {
            yield return new object[] {new TestSkippedInfo ("", "", new Dictionary<string, List<string>>(), "", "", "skipReason") };
        }

#if NET35

        [Fact()]
        public void TestAddSkippedTest()
        {
            foreach (object[] data in GetAddSkippedTestData())
            {
                var info = data[0] as TestSkippedInfo;
                this.AssertsTestAddSkippedTest(info);
            }
        }

#else

        [Theory(), MemberData(nameof(GetAddSkippedTestData))]
        public void TestAddSkippedTest(TestSkippedInfo  info)
        {
            this.AssertsTestAddSkippedTest( info);
        }

#endif
        protected void AssertsTestAddSkippedTest(TestSkippedInfo info)
        {
            var assemblyLocation = "abc.dll";
            var sut = new Result(assemblyLocation);

            Assert.Empty(sut.SkippedTests);

            sut.AddSkippedTest(info);
            Assert.Equal(1, sut.Skipped);
#if NET35
            Assert.Equal(1, sut.SkippedTests.Count);
#else
            Assert.Single(sut.SkippedTests);
#endif
            Assert.Contains(info, sut.SkippedTests);

            var actual = sut.SkippedTests.First();
            Assert.Equal(info.MethodName, actual.MethodName);
            Assert.Equal(info.TestCollectionDisplayName, actual.TestCollectionDisplayName);
            Assert.Equal(info.TestDisplayName, actual.TestDisplayName);
            Assert.Equal(info.TypeName, actual.TypeName);

            Assert.Equal(info.SkipReason, actual.SkipReason);
        }

        public static IEnumerable<object[]> GetAddPassedTestData()
        {
            yield return new object[] { new TestPassedInfo("", "", new Dictionary<string, List<string>>(), "", "", 1.5M, "output") };
        }

#if NET35

        [Fact()]
        public void TestAddPassedTest()
        {
            foreach (object[] data in GetAddPassedTestData())
            {
                var info = data[0] as TestPassedInfo;
                this.AssertsTestAddPassedTest(info);
            }
        }

#else

        [Theory(), MemberData(nameof(GetAddPassedTestData))]
        public void TestAddPassedTest(TestPassedInfo info)
        {
            this.AssertsTestAddPassedTest( info);
        }

#endif
        protected void AssertsTestAddPassedTest(TestPassedInfo info)
        {
            var assemblyLocation = "abc.dll";
            var sut = new Result(assemblyLocation);

            Assert.Empty(sut.PassedTests);

            sut.AddPassedTest(info);
#if NET35
            Assert.Equal(1, sut.PassedTests.Count);
#else
            Assert.Single(sut.PassedTests);
#endif
            Assert.Contains(info, sut.PassedTests);

            var actual = sut.PassedTests.First();
            Assert.Equal(info.MethodName, actual.MethodName);
            Assert.Equal(info.TestCollectionDisplayName, actual.TestCollectionDisplayName);
            Assert.Equal(info.TestDisplayName, actual.TestDisplayName);
            Assert.Equal(info.TypeName, actual.TypeName);
            Assert.Equal(info.ExecutionTime, actual.ExecutionTime);
        }
    }
}
