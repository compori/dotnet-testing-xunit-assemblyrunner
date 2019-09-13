using Compori.Testing.Xunit.AssemblyRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AssemblyRunnerTests
{
    public class FilterTest
    {

        public static IEnumerable<object[]> GetEqualsTestData()
        {
            yield return new object[] { false, new Filter { }, null };
            yield return new object[] { true, new Filter { }, new Filter { } };
            yield return new object[] { false, new Filter { AssemblyLocation = "abc.dll" }, new Filter { } };
            yield return new object[] { false, new Filter { AssemblyLocation = "abc.dll" }, new Filter { AssemblyLocation = "xyz.dll" } };
            yield return new object[] { true, new Filter { AssemblyLocation = "abc.dll" }, new Filter { AssemblyLocation = "abc.dll" } };
            yield return new object[] { false, new Filter { Case = "Tests.TestCase" }, new Filter { } };
            yield return new object[] { false, new Filter { AssemblyLocation = "Tests.TestCase" }, new Filter { AssemblyLocation = "Tests.OtherTestCase" } };
            yield return new object[] { true, new Filter { AssemblyLocation = "Tests.TestCase" }, new Filter { AssemblyLocation = "Tests.TestCase" } };
            yield return new object[] { false, new Filter { ClassName = "MyTest" }, new Filter { } };
            yield return new object[] { false, new Filter { ClassName = "MyTest" }, new Filter { ClassName = "YourTest" } };
            yield return new object[] { true, new Filter { ClassName = "MyTest" }, new Filter { ClassName = "MyTest" } };
        }

#if NET35

        [Fact()]
        public void TestEquals()
        {
            foreach(object[] data in GetEqualsTestData())
            {
                this.AssertsTestEquals((bool)data[0], data[1] as Filter, data[2] as Filter);
            }
        }

#else

        [Theory(), MemberData(nameof(GetEqualsTestData))]
        public void TestEquals(bool expect, Filter one, Filter two)
        {
            this.AssertsTestEquals(expect, one, two);
        }

#endif

        protected void AssertsTestEquals(bool expect, Filter one, Filter two)
        {
            Assert.Equal(expect, one.Equals(two));
        }
    }
}
