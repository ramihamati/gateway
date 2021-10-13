using Digitteck.Gateway.Service.Common;
using NUnit.Framework;

namespace HelperTests
{
    public class StringHelperTests
    {
        [Test]
        public void RemoveImmediateDuplicateTest()
        {
            string syntax = "api//movies///test\\\\";

            string reconstructed = syntax.RemoveImmediateDuplicate('/');

            Assert.AreEqual("api/movies/test\\\\", reconstructed);
        }
    }
}
