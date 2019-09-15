using System;
using Xunit;

namespace SampleTest
{
    public class MySampleTest
    {
        public static string Context;

        [Fact()]
        public void TestTrue()
        {
            Assert.True(true);
        }

        [Fact(), Trait("MyTrait", "SomeValue")]
        public void TestContextSet()
        {
            Assert.NotNull(Context);
        }

        [Fact(Skip = "Skip this test")]
        public void TestSkipped()
        {
            Assert.True(true);
        }
    }
}
