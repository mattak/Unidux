using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Unidux
{
    public class StateBaseTest
    {
        [Test]
        public void CloneTest()
        {
            var sample1 = new SampleState();
            sample1.List = new List<string>() {"a", "b", "c"};

            var sample2 = (SampleState)sample1.Clone();
            sample2.List[1] = "bb";
            Assert.AreEqual(new List<string>() {"a", "b", "c"}, sample1.List);
            Assert.AreEqual(new List<string>() {"a", "bb", "c"}, sample2.List);
        }
    }

    [Serializable]
    public class SampleState : StateBase
    {
        public List<string> List = new List<string>();
    }
}