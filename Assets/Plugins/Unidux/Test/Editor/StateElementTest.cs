using System;
using NUnit.Framework;

namespace Unidux
{
    [Serializable]
    public class StateElementTest
    {
        [Test]
        public void Equals()
        {
            var element1 = new SampleStateElement();
            var element2 = new SampleStateElement();
            
            Assert.AreEqual(element1, element2);

            element1.Value = "Hello";
            Assert.AreNotEqual(element1, element2);
            element2.Value = "Hello";
            Assert.AreEqual(element1, element2);
        }

        class SampleStateElement : StateElement
        {
            public string Value;
        }
    }
}
