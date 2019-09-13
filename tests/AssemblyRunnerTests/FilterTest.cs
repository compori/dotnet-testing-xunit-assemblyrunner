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
            yield return new object[] { false, new Filter { }, new Filter { AssemblyLocation = "abc.dll" } };
            yield return new object[] { false, new Filter { AssemblyLocation = "abc.dll" }, new Filter { AssemblyLocation = "xyz.dll" } };
            yield return new object[] { true, new Filter { AssemblyLocation = "abc.dll" }, new Filter { AssemblyLocation = "abc.dll" } };

            yield return new object[] { false, new Filter { Case = "Tests.TestCase" }, new Filter { } };
            yield return new object[] { false, new Filter { }, new Filter { Case = "Tests.TestCase" } };
            yield return new object[] { false, new Filter { Case = "Tests.TestCase" }, new Filter { Case = "Tests.OtherTestCase" } };
            yield return new object[] { true, new Filter { Case = "Tests.TestCase" }, new Filter { Case = "Tests.TestCase" } };

            yield return new object[] { false, new Filter { ClassName = "MyTest" }, new Filter { } };
            yield return new object[] { false, new Filter { }, new Filter { ClassName = "MyTest" } };
            yield return new object[] { false, new Filter { ClassName = "MyTest" }, new Filter { ClassName = "YourTest" } };
            yield return new object[] { true, new Filter { ClassName = "MyTest" }, new Filter { ClassName = "MyTest" } };

            yield return new object[] { false, new Filter { TraitName = "TraitName1" }, new Filter { } };
            yield return new object[] { false, new Filter { }, new Filter { TraitName = "TraitName1" } };
            yield return new object[] { false, new Filter { TraitName = "TraitName1" }, new Filter { TraitName = "OtherTraitName" } };
            yield return new object[] { true, new Filter { TraitName = "TraitName1" }, new Filter { TraitName = "TraitName1" } };

            yield return new object[] { false, new Filter { TraitValue = "TraitValu1" }, new Filter { } };
            yield return new object[] { false, new Filter { }, new Filter { TraitValue = "TraitValu1" } };
            yield return new object[] { false, new Filter { TraitValue = "TraitValu1" }, new Filter { TraitValue = "OtherTraitValu" } };
            yield return new object[] { true, new Filter { TraitValue = "TraitValu1" }, new Filter { TraitValue = "TraitValu1" } };
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

        public static IEnumerable<object[]> GetGetHashCodeTestData()
        {
            yield return new object[] { "null:null:null:null:null".GetHashCode(), new Filter { } };
            yield return new object[] { "abc.dll:null:null:null:null".GetHashCode(), new Filter { AssemblyLocation = "abc.dll" } };
            yield return new object[] { "null:TestCase:null:null:null".GetHashCode(), new Filter { Case = "TestCase" } };
            yield return new object[] { "null:null:TestClass:null:null".GetHashCode(), new Filter { ClassName = "TestClass" } };
            yield return new object[] { "null:null:null:TraitName:null".GetHashCode(), new Filter { TraitName = "TraitName" } };
            yield return new object[] { "null:null:null:null:TraitValue".GetHashCode(), new Filter { TraitValue = "TraitValue" } };
        }

#if NET35


        [Fact()]
        public void TestGetHashCode()
        {
            foreach (object[] data in GetGetHashCodeTestData())
            {
                this.AssertsTestGetHashCode((int)data[0], data[1] as Filter);
            }
        }

#else

        [Theory(), MemberData(nameof(GetGetHashCodeTestData))]
        public void TestGetHashCode(int expect, Filter value)
        {
            this.AssertsTestGetHashCode(expect, value);
        }

#endif

        protected void AssertsTestGetHashCode(int expect, Filter value)
        {
            Assert.Equal(expect, value.GetHashCode());
        }
    }
}
