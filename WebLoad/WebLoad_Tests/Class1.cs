
using NUnit.Framework;

namespace WebLoad_Tests
{
    //[TestFixture]
    public class Class1
    {
        [Test]
        public void aaa()
        {
            // https://github.com/nunit/nunit/issues/2223
            //var context = new NUnit.Framework.Internal.TestExecutionContext();
            //context.EstablishExecutionEnvironment();

            Assert.AreEqual("aaa", "aaa");

            Assert.That(0, Is.EqualTo(0));
        }
    }
}
