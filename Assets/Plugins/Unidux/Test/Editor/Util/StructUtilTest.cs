using NUnit.Framework;

namespace Unidux.Util
{
    public class StructUtilTest
    {
        [Test]
        public void IntTest()
        {
            Assert.AreEqual(1, "1".ParseInt());
            Assert.AreEqual(null, "".ParseInt());
            Assert.AreEqual(null, "abc".ParseInt());
        }

        [Test]
        public void LongTest()
        {
            Assert.AreEqual(1, "1".ParseLong());
            Assert.AreEqual(null, "".ParseLong());
            Assert.AreEqual(null, "abc".ParseLong());
        }

        [Test]
        public void UIntTest()
        {
            Assert.AreEqual(1, "1".ParseUInt());
            Assert.AreEqual(null, "".ParseUInt());
            Assert.AreEqual(null, "abc".ParseUInt());
        }

        [Test]
        public void ULongTest()
        {
            Assert.AreEqual(1, "1".ParseULong());
            Assert.AreEqual(null, "".ParseULong());
            Assert.AreEqual(null, "abc".ParseULong());
        }

        [Test]
        public void FloatTest()
        {
            Assert.AreEqual(1, "1".ParseFloat());
            Assert.AreEqual(null, "".ParseFloat());
            Assert.AreEqual(null, "abc".ParseFloat());
        }

        [Test]
        public void DoubleTest()
        {
            Assert.AreEqual(1, "1".ParseDouble());
            Assert.AreEqual(null, "".ParseDouble());
            Assert.AreEqual(null, "abc".ParseDouble());
        }

        [Test]
        public void BoolTest()
        {
            Assert.AreEqual(true, "True".ParseBool());
            Assert.AreEqual(false, "False".ParseBool());
            Assert.AreEqual(true, "true".ParseBool());
            Assert.AreEqual(false, "false".ParseBool());
            Assert.AreEqual(null, "".ParseBool());
            Assert.AreEqual(null, "abc".ParseBool());
        }
    }
}