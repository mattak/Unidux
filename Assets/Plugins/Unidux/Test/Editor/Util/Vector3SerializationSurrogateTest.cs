using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using UnityEngine;

namespace Unidux.Util
{
    public class Vector3SerializationSurrogateTest
    {
        [Test]
        public void SerializationTest()
        {
            SurrogateSelector selector = new SurrogateSelector();
            selector.AddSurrogate(
                typeof(Vector3),
                new StreamingContext(StreamingContextStates.All),
                new Vector3SerializationSurrogate()
            );
            var formatter = new BinaryFormatter();
            formatter.SurrogateSelector = selector;

            var clonee = new Vector3(1, 2, 3);
            Vector3 cloned;
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, clonee);
                stream.Position = 0;
                cloned = (Vector3) formatter.Deserialize(stream);
            }

            Assert.AreEqual(clonee, cloned);
        }
    }
}