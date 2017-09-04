using System.Collections.Generic;
using NUnit.Framework;

namespace Unidux.Util
{
    public class IListExtensionTest
    {
        [Test]
        public void RemoveLastTest()
        {
            var list = new List<int>(new int[] {1, 2});

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(2, list[list.Count - 1]);
            
            list.RemoveLast();
            
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(1, list[list.Count - 1]);
            
            list.RemoveLast();
            
            Assert.AreEqual(0, list.Count);
        }
    }
}