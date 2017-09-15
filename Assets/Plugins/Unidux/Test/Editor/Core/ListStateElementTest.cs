using System;
using NUnit.Framework;

namespace Unidux
{
    public class ListStateElementTest
    {
        [Test]
        public void EqualsTest()
        {
            var state1 = new SampleState();
            var state2 = new SampleState();

            Assert.AreEqual(state1, state2);

            state1.Entities.Add(new SampleEntity() {Value = 1});
            state2.Entities.Add(new SampleEntity() {Value = 2});

            Assert.AreNotEqual(state1, state2);

            state2.Entities[0].Value = 1;
        }

        [Test]
        public void CloneTest()
        {
            var state1 = new SampleState();
            state1.Entities.Add(new SampleEntity() {Value = 1});
            var state2 = (SampleState) state1.Clone();
            Assert.AreEqual(state1, state2);

            state2.Entities[0].Value = 2;
            Assert.AreNotEqual(state1, state2);
        }

        class SampleState : ListStateElement<SampleEntity>
        {
        }

        [Serializable]
        class SampleEntity : ICloneable
        {
            public int Value;

            public SampleEntity()
            {
            }

            public SampleEntity(SampleEntity entity)
            {
                this.Value = entity.Value;
            }

            public object Clone()
            {
                return new SampleEntity(this);
            }

            public override bool Equals(object obj)
            {
                if (obj is SampleEntity)
                {
                    var target = (SampleEntity) obj;
                    return this.Value == target.Value;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}