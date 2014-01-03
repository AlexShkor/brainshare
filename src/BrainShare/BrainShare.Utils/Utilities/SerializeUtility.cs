using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BrainShare.Utils.Utilities
{
    public class SerializeUtility
    {
        public static byte[] Serialize(object objectToSerialize)
        {
            byte[] serializedObject;

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, objectToSerialize);
                serializedObject = stream.ToArray();
            }

            return serializedObject;
        }

        public static T Deserialize<T>(byte[] serializedType)
        {
            T deserializedObject;
            using (var memoryStream = new MemoryStream(serializedType))
            {
                var deserializer = new BinaryFormatter();
                deserializedObject = (T)deserializer.Deserialize(memoryStream);
            }

            return deserializedObject;
        }
    }
}
