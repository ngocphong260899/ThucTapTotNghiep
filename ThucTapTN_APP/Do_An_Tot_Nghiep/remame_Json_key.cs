using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft;
namespace Do_An_Tot_Nghiep
{
    class remame_Json_key
    {
        [JsonProperty("room_name")]
        public string room_name { get; set; }

        [JsonProperty("sw_wifi1")]
        public string sw_wifi1 { get; set; }

        [JsonProperty("sw_wifi2")]
        public string sw_wifi2 { get; set; }

        [JsonProperty("sw_wifi3")]
        public string sw_wifi3 { get; set; }
    }
}
