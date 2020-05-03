using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotDefteriVeritabani.KimlikDogrulamaIslemleri
{
    public class GoogleVerisi
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("verified_email")]
        public bool Verified_Email { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }
    }

}
