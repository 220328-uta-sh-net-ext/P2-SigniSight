<<<<<<< HEAD
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SigniSightAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OCRController : ControllerBase
    {
    }
}
||||||| 0b55246
=======
﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using static SigniSightBL.Logic;

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
>>>>>>> bf69d9f84122b825694fd86869befed8c1aa27ba
