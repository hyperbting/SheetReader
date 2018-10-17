using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SheetReader.Models;

namespace SheetReader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultCharactersController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<GeneralResponse> Get()
        //public ActionResult<IEnumerable<Dictionary<string, string>>> Get()
        {
            GoogleSheetJSON jobject = null;
            using (WebClient wc = new WebClient())
            {
                string googleSheetJSONString = wc.DownloadString("https://spreadsheets.google.com/feeds/list/1ls8K6YQyDVqaOmTJUVXKwGnbg--urIWlzF3aKStNadY/od6/public/basic?alt=json");
                //lJson.Add(googleSheetJSONString);
                jobject = JsonConvert.DeserializeObject<GoogleSheetJSON>(googleSheetJSONString);
            }

            Dictionary<int, Dictionary<string,string>> chars = new Dictionary<int, Dictionary<string, string>>();
            ////process 
            foreach (var str in jobject.feed.entry)
            {
                var dict = str.content.GetDict();

                if (!dict.ContainsKey("key"))
                    continue;

                var targetField = dict["key"];
                foreach (var kvpair in dict)
                {
                    if (!kvpair.Key.Contains("row"))
                        continue;

                    if (!Int32.TryParse(kvpair.Key.Replace("row",""), out int idx))
                        continue;

                    if (!chars.ContainsKey(idx))
                        chars[idx] = new Dictionary<string, string>();

                    bool isAttributeOrBuff = false;
                    foreach (var prefixword in new List<string>() { "attribute", "buff"})
                    {
                        if (targetField.StartsWith(prefixword))
                        {
                            if (!chars[idx].ContainsKey(prefixword))
                                chars[idx][prefixword] = "";

                            chars[idx][prefixword] += targetField.Replace(prefixword, "") + ":" + kvpair.Value + ",";
                            isAttributeOrBuff = true;
                            continue;
                        }
                    }

                    if(!isAttributeOrBuff)
                        chars[idx].Add(targetField, kvpair.Value);
                }
            }

            return new GeneralResponse
            {
                characters = new List<Dictionary<string, string>>(chars.Values)
            };
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}