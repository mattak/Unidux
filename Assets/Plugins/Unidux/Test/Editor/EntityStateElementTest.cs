using System;
using NUnit.Framework;

namespace Unidux
{
    public class EntityStateElementTest
    {
        [Test]
        public void EqualsTest()
        {
            var state1 = new SampleStateElement();
            var state2 = new SampleStateElement();
            Assert.AreEqual(state1, state2);

            state1.Entity = new SampleEntity();
            state2.Entity = new SampleEntity();
            state1.Entity.Id = 1;
            state2.Entity.Id = 2;

            Assert.AreNotEqual(state1, state2);
        }

        [Test]
        public void CloneTest()
        {
            var state1 = new SampleStateElement();
            state1.Entity = new SampleEntity();

            var state2 = (SampleStateElement) state1.Clone();
            Assert.AreEqual(state1, state2);

            state1.Entity.Id = 1;
            state2.Entity.Id = 2;

            Assert.AreNotEqual(state1, state2);
        }

        class SampleEntity : ICloneable
        {
            public int Id;

            public override bool Equals(object obj)
            {
                if (obj is SampleEntity)
                {
                    var target = (SampleEntity) obj;
                    return this.Id == target.Id;
                }
                return false;
            }

            public object Clone()
            {
                var target = new SampleEntity();
                target.Id = this.Id;
                return target;
            }
            
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        class SampleStateElement : EntityStateElement<SampleEntity>
        {
        }
    }
}