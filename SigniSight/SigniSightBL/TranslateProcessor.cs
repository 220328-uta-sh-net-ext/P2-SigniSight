using System.Text;
using Newtonsoft.Json;

namespace SigniSightBL
{
    public class TranslateProcessor
    {
        private static readonly string key = "?????????????????????";
        private static readonly string endpointTranslate = "https://api.cognitive.microsofttranslator.com/";
        private static readonly string location = "????????";
        private static string route = "/translate?api-version=3.0&to=ru&to=en&to=pl"; //change it to variable
        public static async Task<string> TranslateText(string textToTranslate)
        {
            object[] body = new object[] { new { Text = textToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);
            var client2 = new HttpClient();
            var request = new HttpRequestMessage();

            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(endpointTranslate + route);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
            request.Headers.Add("Ocp-Apim-Subscription-Region", location);

            HttpResponseMessage response = await client2.SendAsync(request).ConfigureAwait(false);
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
