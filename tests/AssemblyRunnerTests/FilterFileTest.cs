using Compori.Testing.Xunit.AssemblyRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AssemblyRunnerTests
{
    public class FilterFileTest
    {

        public static IEnumerable<object[]> GetParseTestData()
        {
            yield return new object[] { null, null };
            yield return new object[] { null, "  " };
            yield return new object[] { null, "Mary had a litte lamb." };
            yield return new object[] { null, "  #class:TestClass" };
            yield return new object[] { new Filter { ClassName = "TestClass" }, "class:TestClass" };
            yield return new object[] { new Filter { ClassName = "TestClass" }, "class:TestClass   " };
            yield return new object[] { new Filter { ClassName = "TestClass" }, "class:  TestClass" };
            yield return new object[] { new Filter { Case = "TestCase" }, "case:TestCase" };
            yield return new object[] { new Filter { Case = "TestCase" }, "  case:  TestCase   " };
            yield return new object[] { new Filter { TraitName = "TraitName" }, "trait:TraitName" };
            yield return new object[] { new Filter { TraitName = "TraitName" }, "trait:TraitName=" };
            yield return new object[] { new Filter { TraitName = "TraitName", TraitValue = "Value" }, "trait:TraitName=Value" };
            yield return new object[] { new Filter { TraitName = "TraitName", TraitValue="Value" }, "  trait:  TraitName = Value  " };
            yield return new object[] { new Filter { TraitValue = "Value" }, "  trait:= Value  " };
            yield return new object[] { new Filter { TraitValue = "Value" }, "trait:=Value" };
            yield return new object[] { null, "trait:=" };
        }


#if NET35

        [Fact()]
        public void TestParse()
        {
            foreach (object[] data in GetParseTestData())
            {
                this.AssertsTestParse(data[0] as Filter, data[1] as string);
            }
        }

#else

        [Theory(), MemberData(nameof(GetParseTestData))]
        public void TestParse(Filter expect, string line)
        {
            this.AssertsTestParse(expect, line);
        }
        
#endif

        protected void AssertsTestParse(Filter expect, string line)
        {
            var sut = new FilterFile();
            var actual = sut.Parse(line);
            Assert.Equal(expect, actual);
        }

        public static IEnumerable<object[]> GetLoadTestData()
        {
            yield return new object[] { new List<Filter>(), "data/filter-data-01.txt" };
            yield return new object[] { new List<Filter>() {
                new Filter { ClassName = "MyClass" },
                new Filter { Case = "MyCase" },
            }, "data/filter-data-02.txt" };
        }

#if NET35

        [Fact()]
        public void TestLoad()
        {
            foreach (object[] data in GetLoadTestData())
            {
                this.AssertsTestLoad(data[0] as List<Filter>, data[1] as string);
            }
        }

#else

        [Theory(), MemberData(nameof(GetLoadTestData))]
        public void TestLoad(List<Filter> expect, string line)
        {
            this.AssertsTestLoad(expect, line);
        }
        
#endif

        protected void AssertsTestLoad(List<Filter> expect, string file)
        {
            var sut = new FilterFile();
            var actual = sut.Load(file);
            Assert.Equal(expect, actual);
        }

        [Fact()]
        public void TestParseFailed()
        {
            var sut = new FilterFile();
            Assert.Throws<InvalidOperationException>(() => sut.Load("not-existing-file.txt"));
        }
    }
}
