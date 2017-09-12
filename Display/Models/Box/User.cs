using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Display.Models.Box
{
    public class User : BoxObject
    {
        [JsonProperty("login")]
        public string Login { get; set; }
    }
}