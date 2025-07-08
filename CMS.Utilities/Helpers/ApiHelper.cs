using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Utilities.Helpers
{
    public class ApiHelper<T>
    {
        public async static Task<T> post(string baseUrl, string apiEnpoint, string json)
        {
            using (var client = new HttpClient())
            {

                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(contentType);

                var contentData = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiEnpoint, contentData);

                if (response.IsSuccessStatusCode)
                {
                    var stringData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(stringData);
                }
            }

            return default;
        }

        public async static Task<T> get(string baseUrl, string apiEnpoint, string queryString = null)
        {
            using (var client = new HttpClient())
            {

                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(contentType);

                if (!string.IsNullOrEmpty(queryString))
                {
                    apiEnpoint += "?" + queryString.Trim();
                }

                var response = await client.GetAsync(apiEnpoint);

                if (response.IsSuccessStatusCode)
                {
                    var stringData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(stringData);
                }
            }

            return default;
        }
    }
}
