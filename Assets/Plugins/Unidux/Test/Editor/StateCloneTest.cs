using NUnit.Framework;
namespace Unidux
{
    public class StateCloneTest
    {
        class Hoge : IStateClone
        {
            private bool Flag = false;

            public void SetFlag(bool flg)
            {
                this.Flag = flg;
            }

            public bool GetFlag()
            {
                return this.Flag;
            }

            public T Clone<T>() where T : IStateClone
            {
                return (T) MemberwiseClone();
            }
        }

        class HogeHoge : Hoge
        {
            private int Count = 1;

            public void SetCount(int count)
            {
                this.Count = count;
            }

            public int GetCount()
            {
                return this.Count;
            }
        }

        [Test]
        public void CloneTest()
        {
            var hoge1 = new HogeHoge();
            hoge1.SetFlag(true);
            hoge1.SetCount(100);
            Assert.AreEqual(true, hoge1.GetFlag());
            Assert.AreEqual(100, hoge1.GetCount());

            var hoge2 = hoge1.Clone<HogeHoge>();
            Assert.AreEqual(true, hoge2.GetFlag());
            Assert.AreEqual(100, hoge2.GetCount());
        }
    }
}