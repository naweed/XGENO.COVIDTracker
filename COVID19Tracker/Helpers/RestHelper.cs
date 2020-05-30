using System;
using System.Net.Http;
using System.Threading.Tasks;
using COVID19Tracker.Exceptions;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace COVID19Tracker.Helpers
{
    public static class RestHelper
    {
        public static async Task<T> GetAsync<T>(string resource, string parameters = "", string apiKey = "")
        {
            //Construct the URL
            string uri = resource + (!String.IsNullOrEmpty(apiKey) ? "?api_key=" + apiKey : "");

            //Add Parameters
            if (!string.IsNullOrEmpty(parameters))
                uri += (!String.IsNullOrEmpty(apiKey) ? "&" : "") + parameters;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                throw new InternetConnectionException();
            }

            //Make Http Call
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(new Uri(uri));

            //Ensure that 2XX status code is returned
            response.EnsureSuccessStatusCode();

            //Get json result
            string json = await response.Content.ReadAsStringAsync();

            //Convert and return the result
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
