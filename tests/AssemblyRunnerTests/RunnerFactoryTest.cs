using Compori.Testing.Xunit.AssemblyRunner;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AssemblyRunnerTests
{
    public class RunnerFactoryTest 
    {
        [Fact()]
        public void TestCreate()
        {
            Mock<IXunitAssemblyRunnerFactory> moq;
            RunnerFactory sut;
            Runner actual;

            moq = new Mock<IXunitAssemblyRunnerFactory>();
            sut = new RunnerFactory(moq.Object);
            actual = sut.Create("abc.dll");

#if NET35
            Assert.Equal(1, actual.AssemblyLocations.Count);
#else
            Assert.Single(actual.AssemblyLocations);
#endif
            Assert.Contains("abc.dll", actual.AssemblyLocations);

            moq = new Mock<IXunitAssemblyRunnerFactory>();
            sut = new RunnerFactory(moq.Object);
            actual = sut.Create(new string[] { "abc.dll", "xyz.dll" });

            Assert.Equal(2, actual.AssemblyLocations.Count);
            Assert.Contains("abc.dll", actual.AssemblyLocations);
            Assert.Contains("xyz.dll", actual.AssemblyLocations);

        }
    }
}
