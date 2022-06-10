using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Serilog;
using SigniSightBL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
namespace SigniSightAPI.Controllers
{
    
    [ApiController]
    public class OCRController : ControllerBase
    {
        private readonly ILogger<OCRController> _logger;

        public OCRController(ILogger<OCRController> logger)
        {
            _logger = logger;
        }


        static string subscriptionKey = Environment.GetEnvironmentVariable("ocrkey");


        static string endpoint = Environment.GetEnvironmentVariable("ocrend");
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
            _logger.LogInformation("OCR Image Processing");
            var list = await OCRProcessor.ReadFileUrl(client, imageUrl);
            return Ok(list);
        }
        [HttpPost("OCRFile")]
        public async Task<ActionResult<string>> MakeOCRRequest2()
        {
            _logger.LogInformation("Making OCR Request");
            var list = await OCRProcessor.RecognizePrintedTextLocal(client);
            return Ok(list);
        }
    }
}

