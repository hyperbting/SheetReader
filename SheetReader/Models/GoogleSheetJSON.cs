using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SheetReader.Models
{
    //load from https://spreadsheets.google.com/feeds/list/1ls8K6YQyDVqaOmTJUVXKwGnbg--urIWlzF3aKStNadY/od6/public/values?alt=json
    //https://spreadsheets.google.com/feeds/list/1ls8K6YQyDVqaOmTJUVXKwGnbg--urIWlzF3aKStNadY/od6/public/basic?alt=json
    [Serializable]
    public class GoogleSheetJSON
    {
        public string version;
        public string encoding;
        public FeedContent feed;
    }

    [Serializable]
    public class FeedContent
    {
        public string xmlns;
        public List<EntryContent> entry;
    }

    [Serializable]
    public class EntryContent
    {
        public Content content;
    }

    [Serializable]
    public class Content
    {
        public string type;
        [JsonProperty(PropertyName = "$t")]
        public string wholeContent;

        public Dictionary<string, string> GetDict()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            var comaSplit = wholeContent.Split(',');
            foreach (var kvpair in comaSplit)
            {
                var kvSplit = kvpair.Split(':');
                result[kvSplit[0].TrimStart(' ')] = kvSplit[1].TrimStart(' ');
            }

            return result;
        }
    }

}
