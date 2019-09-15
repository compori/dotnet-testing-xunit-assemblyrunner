using Compori.Testing.Xunit.AssemblyRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AssemblyRunnerTests
{
    public class XunitAssemblyRunnerFactoryTest
    {
        [Fact()]
        public void TestCreate()
        {
            IXunitAssemblyRunnerFactory sut = new XunitAssemblyRunnerFactory();

            var actual = sut.Create(typeof(XunitAssemblyRunnerFactoryTest).Assembly.Location);

            Assert.NotNull(actual);
        }
    }
}
