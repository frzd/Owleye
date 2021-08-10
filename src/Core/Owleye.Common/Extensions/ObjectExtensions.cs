using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Owleye.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static byte[] ObjectToByteArray(this Object obj)
        {
            if (obj == null)
                return null;

            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            //TODO fix this security vulnerabilities
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static T ByteArrayToObject<T>(this byte[] arrBytes)
        {
            var memStream = new MemoryStream();
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            //TODO fix this security vulnerabilities
            T obj = (T)binForm.Deserialize(memStream);

            return obj;
        }
    }
}
