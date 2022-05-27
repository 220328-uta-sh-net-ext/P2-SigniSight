using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using SigniSightBL;

namespace SigniSightAPI.Controllers
{
    
    [ApiController]
    public class OCRController : ControllerBase
    {
        const string subscriptionKey = "????????????";
        const string endpoint = "https://*.cognitiveservices.azure.com/";
        ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);
        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }
        //[Authorize]
        [HttpPost("OCR")]
        public async Task<ActionResult<string>> ReadFileUrl(string imageUrl)
        {
            var list = await OCRProcessor.ReadFileUrl(client, imageUrl);
            return Ok(list);
        }
    }
}

