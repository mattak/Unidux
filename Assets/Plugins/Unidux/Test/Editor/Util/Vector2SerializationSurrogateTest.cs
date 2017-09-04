using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using UnityEngine;

namespace Unidux.Util
{
    public class Vector2SerializationSurrogateTest
    {
        [Test]
        public void SerializationTest()
        {
            SurrogateSelector selector = new SurrogateSelector();
            selector.AddSurrogate(
                typeof(Vector2),
                new StreamingContext(StreamingContextStates.All),
                new Vector2SerializationSurrogate()
            );
            var formatter = new BinaryFormatter();
            formatter.SurrogateSelector = selector;

            var clonee = new Vector2(1, 2);
            Vector2 cloned;
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, clonee);
                stream.Position = 0;
                cloned = (Vector2) formatter.Deserialize(stream);
            }

            Assert.AreEqual(clonee, cloned);
        }
    }
}