using System;
using NUnit.Framework;

namespace Unidux
{
    public class StructStateElementTest
    {
        [Test]
        public void EqualsTest()
        {
            var state1 = new SampleStateElement();
            var state2 = new SampleStateElement();
            Assert.AreEqual(state1, state2);

            state1.Value = AlphabetNumber.One;
            state2.Value = AlphabetNumber.Two;

            Assert.AreNotEqual(state1, state2);
        }

        [Test]
        public void CloneTest()
        {
            var state1 = new SampleStateElement();
            state1.Value = AlphabetNumber.Three;

            var state2 = (SampleStateElement) state1.Clone();
            Assert.AreEqual(state1, state2);

            state2.Value = AlphabetNumber.Two;
            Assert.AreNotEqual(state1, state2);
        }

        class SampleStateElement : StructStateElement<AlphabetNumber>
        {
        }

        enum AlphabetNumber
        {
            One,
            Two,
            Three,
        }
    }
}