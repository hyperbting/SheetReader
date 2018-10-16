using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SheetReader.Models
{
    [Serializable]
    public class GeneralResponse
    {
        public List<Dictionary<string, string>> characters;
    }
}
