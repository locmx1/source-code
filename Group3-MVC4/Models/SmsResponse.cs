using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Group3_MVC4.Models
{
    public class SmsResponse
    {
        [JsonProperty(PropertyName = "date")]
        public TimeSpan Date { get; set; }

        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "read")]
        public bool IsRead { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "requestMethod")]
        public string RequestMethod { get; set; }

        [JsonProperty(PropertyName = "isSuccessful")]
        public bool IsSuccessful { get; set; }
    }
}