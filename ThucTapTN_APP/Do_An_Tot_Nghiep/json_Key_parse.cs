using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft;
namespace Do_An_Tot_Nghiep
{
    class json_Key_parse

    {
        [JsonProperty("sw_wifi")]

        public string sw_wifi { get; set; }

        [JsonProperty("pos")]
        public string pos { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("ssid")]
        public string ssid { get; set; }


        [JsonProperty("streng")]
        public string streng { get; set; }



    
    }
}
