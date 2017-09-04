using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using UnityEngine;

namespace Unidux.Util
{
    public class Vector4SerializationSurrogateTest
    {
        [Test]
        public void SerializationTest()
        {
            SurrogateSelector selector = new SurrogateSelector();
            selector.AddSurrogate(
                typeof(Vector4),
                new StreamingContext(StreamingContextStates.All),
                new Vector4SerializationSurrogate()
            );
            var formatter = new BinaryFormatter();
            formatter.SurrogateSelector = selector;

            var clonee = new Vector4(1, 2, 3, 4);
            Vector4 cloned;
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, clonee);
                stream.Position = 0;
                cloned = (Vector4) formatter.Deserialize(stream);
            }

            Assert.AreEqual(clonee, cloned);
        }
    }
}