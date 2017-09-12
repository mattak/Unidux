using System;
using NUnit.Framework;

namespace Unidux.Util
{
    public class StateUtilTest
    {
        [Test]
        public void ApplyStateChangedTest()
        {
            {
                var sample1 = new SampleEntity();
                var sample2 = new SampleEntity();

                Assert.IsFalse(StateUtil.ApplyStateChanged(sample1, sample2));
                Assert.IsFalse(sample1.IsStateChanged);
                Assert.IsFalse(sample2.IsStateChanged);

                sample2.Id = 2;

                Assert.IsTrue(StateUtil.ApplyStateChanged(sample1, sample2));
                Assert.IsFalse(sample1.IsStateChanged);
                Assert.IsTrue(sample2.IsStateChanged);

                sample2.SetStateChanged(false);
                sample2.Id = 1;
                sample2.Name = "Kuma";

                Assert.IsTrue(StateUtil.ApplyStateChanged(sample1, sample2));
                Assert.IsFalse(sample1.IsStateChanged);
                Assert.IsTrue(sample2.IsStateChanged);
            }

            {
                var root1 = new RootEntity();
                var root2 = new RootEntity();

                Assert.IsFalse(StateUtil.ApplyStateChanged(root1, root2));
                Assert.IsFalse(root1.IsStateChanged);
                Assert.IsFalse(root2.IsStateChanged);

                root2.Sample = new SampleEntity();
                Assert.IsTrue(StateUtil.ApplyStateChanged(root1, root2));
                Assert.IsFalse(root1.IsStateChanged);
                Assert.IsTrue(root2.IsStateChanged);

                root2.SetStateChanged(false);

                root1.Sample = new SampleEntity();
                Assert.IsFalse(StateUtil.ApplyStateChanged(root1, root2));
                Assert.IsFalse(root1.IsStateChanged);
                Assert.IsFalse(root2.IsStateChanged);

                root2.Sample.Id = root1.Sample.Id + 1;
                Assert.IsTrue(StateUtil.ApplyStateChanged(root1, root2));
                Assert.IsFalse(root1.IsStateChanged);
                Assert.IsTrue(root2.IsStateChanged);
            }
        }

        [Test]
        public void ResetStateChangedTest()
        {
            var sample = new SampleEntity();
            sample.SetStateChanged();
            Assert.IsTrue(sample.IsStateChanged);
            StateUtil.ResetStateChanged(sample);
            Assert.IsFalse(sample.IsStateChanged);

            var root = new RootEntity();
            root.SetStateChanged();
            root.Sample = new SampleEntity();
            root.Sample.SetStateChanged();
            Assert.IsTrue(root.IsStateChanged);
            Assert.IsTrue(root.Sample.IsStateChanged);
            StateUtil.ResetStateChanged(root);
            Assert.IsFalse(root.IsStateChanged);
            Assert.IsFalse(root.Sample.IsStateChanged);
        }
    }

    [Serializable]
    class SampleEntity : IStateChanged
    {
        public int Id = 1;
        public string Name = "";

        public bool IsStateChanged { get; private set; }

        public void SetStateChanged(bool state = true)
        {
            this.IsStateChanged = state;
        }
    }

    [Serializable]
    class RootEntity : IStateChanged
    {
        public SampleEntity Sample = null;

        public bool IsStateChanged { get; private set; }

        public void SetStateChanged(bool state = true)
        {
            this.IsStateChanged = state;
        }
    }
}