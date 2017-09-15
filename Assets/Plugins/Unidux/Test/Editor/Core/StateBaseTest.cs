using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Unidux
{
    public class StateBaseTest
    {
        [Test]
        public void Clone()
        {
            var sample1 = new SampleState();
            sample1.List = new List<string>() {"a", "b", "c"};

            var sample2 = (SampleState)sample1.Clone();
            sample2.List[1] = "bb";
            Assert.AreEqual(new List<string>() {"a", "b", "c"}, sample1.List);
            Assert.AreEqual(new List<string>() {"a", "bb", "c"}, sample2.List);
        }

        [Test]
        public void Equals()
        {
            var sample1 = new SampleState();
            var sample2 = new SampleState();

            Assert.AreEqual(sample1, sample2);

            sample1.List = new List<string>() {"a", "b", "c"};
            Assert.AreNotEqual(sample1, sample2);
            sample2.List = new List<string>() {"a", "b", "c"};
            Assert.AreEqual(sample1, sample2);
        }
    }

    [Serializable]
    public class SampleState : StateBase
    {
        public List<string> List = new List<string>();
    }
}