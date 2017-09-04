using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using UnityEngine;

namespace Unidux.Util
{
    public class ColorSerializationSurrogateTest
    {
        [Test]
        public void SerializationTest()
        {
            SurrogateSelector selector = new SurrogateSelector();
            selector.AddSurrogate(
                typeof(Color),
                new StreamingContext(StreamingContextStates.All),
                new ColorSerializationSurrogate()
            );
            var formatter = new BinaryFormatter();
            formatter.SurrogateSelector = selector;

            var clonee = new Color(0.0f, 1f, 0.0f, 1.0f);
            Color cloned;
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, clonee);
                stream.Position = 0;
                cloned = (Color) formatter.Deserialize(stream);
            }

            Assert.AreEqual(clonee, cloned);
        }
    }
}