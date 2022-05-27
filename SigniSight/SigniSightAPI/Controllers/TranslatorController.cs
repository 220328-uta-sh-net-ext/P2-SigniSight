using Microsoft.AspNetCore.Mvc;
using SigniSightBL;

namespace SigniSightAPI.Controllers
{
    [ApiController]
    public class TranslatorController : ControllerBase
    {
        //[Authorize]
        [HttpPost("Translate")]
        public async Task<ActionResult<string>> TranslateText(string textToTranslate, string endLanguageCode)
        {
            endLanguageCode = endLanguageCode.Trim();
            var result = await TranslateProcessor.TranslateText(textToTranslate, endLanguageCode);
            return Ok(result);
        }
    }  
}
