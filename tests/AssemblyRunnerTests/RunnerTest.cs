using Compori.Testing.Xunit.AssemblyRunner;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Runners;

namespace AssemblyRunnerTests
{
    public class RunnerTest
    {
#if !NET461
        [Fact()]
        public void TestInvokeExecute()
        {
            var factory = new RunnerFactory();

            // var location = typeof(SampleTest.MySampleTest).Assembly.Location;
            var location = new Uri(typeof(SampleTest.MySampleTest).Assembly.CodeBase).LocalPath;
            using (var sut = factory.Create(location))
            {
                bool onCompleteInvoked = false;
                bool onTestStartingInvoked = false;
                bool onAssemblyDiscoveryCompleteInvoked = false;
                bool onAssemblyCompleteInvoked = false;
                bool onTestFinishedInvoked = false;
                bool onTestSkippedInvoked = false;
                bool onTestFailedInvoked = false;
                bool onTestPassedInvoked = false;

                sut.OnComplete = result => { onCompleteInvoked = true; };
                sut.OnTestStarting = (result, info) => { onTestStartingInvoked = true; };
                sut.OnAssemblyDiscoveryComplete = (result, info) => { onAssemblyDiscoveryCompleteInvoked = true; };
                sut.OnTestFinished = (result, info) => { onTestFinishedInvoked = true; };
                sut.OnAssemblyComplete = (result, info) => { onAssemblyCompleteInvoked = true; };
                sut.OnTestSkipped = (result, info) => { onTestSkippedInvoked = true; };
                sut.OnTestFailed = (result, info) => { onTestFailedInvoked = true; };
                sut.OnTestPassed = (result, info) => { onTestPassedInvoked = true; };

                SampleTest.MySampleTest.Context = null;
                sut.Execute();

                // Test invokation
                Assert.True(onTestPassedInvoked);
                Assert.True(onTestFailedInvoked);
                Assert.True(onCompleteInvoked);
                Assert.True(onTestStartingInvoked);
                Assert.True(onAssemblyDiscoveryCompleteInvoked);
                Assert.True(onTestFinishedInvoked);
                Assert.True(onAssemblyCompleteInvoked);
                Assert.True(onCompleteInvoked);
                Assert.True(onTestSkippedInvoked);
            }
        }

        [Fact()]
        public void TestExecuteWithInclude()
        {
            var factory = new RunnerFactory(new XunitAssemblyRunnerFactory());

            Runner sut;

            SampleTest.MySampleTest.Context = "A sample value";

            // 
            // var location = typeof(SampleTest.MySampleTest).Assembly.Location;
            var location = new Uri(typeof(SampleTest.MySampleTest).Assembly.CodeBase).LocalPath;
            sut = factory.Create(location);
            sut.Include.Add(new Filter { TraitName = "MyTrait" });
            SampleTest.MySampleTest.Context = null;
            sut.Execute();

            // 
            Assert.Equal(1, sut.Summary.Total);
        }

        [Fact()]
        public void TestExecuteWithExclude()
        {
            var factory = new RunnerFactory(new XunitAssemblyRunnerFactory());

            Runner sut;

            SampleTest.MySampleTest.Context = "A sample value";

            // 
            // var location = typeof(SampleTest.MySampleTest).Assembly.Location;
            var location = new Uri(typeof(SampleTest.MySampleTest).Assembly.CodeBase).LocalPath;
            sut = factory.Create(location);
            sut.Exclude.Add(new Filter { TraitName = "MyTrait" });
            SampleTest.MySampleTest.Context = null;
            sut.Execute();

            // 
            Assert.True(sut.Summary.Total > 0);
        }

        [Fact()]
        public void TestExecute()
        {
            var factory = new RunnerFactory(new XunitAssemblyRunnerFactory());

            Runner sut;

            // 
            // var location = typeof(SampleTest.MySampleTest).Assembly.Location;
            var location = new Uri(typeof(SampleTest.MySampleTest).Assembly.CodeBase).LocalPath;
            sut = factory.Create(location);
            SampleTest.MySampleTest.Context = "A sample value";
            sut.Execute();

            // 
            Assert.Equal(3, sut.Summary.Total);
        }
#endif
    }
}
