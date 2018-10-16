using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace SheetReader.Models
{
    [Serializable]
    public class Character
    {
        public int id = -1;
        public string lastname = "UnKnOwN";
        public string firstname = "UnKnOwN";

        public Character()
        {
        }

        public override string ToString()
        {
            var json = new JsonMediaTypeFormatter();
            return Serialize(json, this);
        }

        string Serialize<T>(MediaTypeFormatter formatter, T value)
        {
            // Create a dummy HTTP Content.
            Stream stream = new MemoryStream();
            var content = new StreamContent(stream);
            /// Serialize the object.
            formatter.WriteToStreamAsync(typeof(T), value, stream, content, null).Wait();
            // Read the serialized string.
            stream.Position = 0;
            return content.ReadAsStringAsync().Result;
        }
    }


}
