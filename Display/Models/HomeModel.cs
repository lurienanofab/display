using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Configuration;
using Newtonsoft.Json;

namespace Display.Models
{
    public class HomeModel
    {
        public int ID { get; set; }

        public async Task<string[]> ListDocuments()
        {
            using (HttpClient hc = new HttpClient())
            {
                hc.BaseAddress = new Uri("https://view-api.box.com/1/");
                hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token " + GetApiKey());

                var msg = await hc.GetAsync("documents");
                var content = await msg.Content.ReadAsStringAsync();

                string[] result = JsonConvert.DeserializeObject<string[]>(content);

                return result;
            }
        }

        private string GetApiKey()
        {
            return ConfigurationManager.AppSettings["BoxApiKey"];
        }
    }
}