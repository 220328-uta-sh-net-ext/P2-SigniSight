using System.Net.Http.Headers;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;
using SigniSightModel;

namespace SigniSightBL
{
    public class OCRProcessor
    {
        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }
        public static async Task<string> ReadFileUrl(ComputerVisionClient client, string urlFile)
        {
            // Read text from URL
            var textHeaders = await client.ReadAsync(urlFile);
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);

            // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;

            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));

            // Display the found text.
            var list = "";
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models.Line line in page.Lines)
                {
                    list += line.Text + " ";
                }
            }
            return list.Trim();
        }
        public static async Task<string> RecognizePrintedTextLocal(ComputerVisionClient client)
        {
            var list = "";
            string localImage = @"C:\Users\shado\Desktop\Учло\albert-einstein-quotes-01-scaled.jpg";
            using (Stream stream = File.OpenRead(localImage))
            {
                // Get the recognized text
                OcrResult localFileOcrResult = await client.RecognizePrintedTextInStreamAsync(true, stream);

                foreach (var localRegion in localFileOcrResult.Regions)
                {
                    foreach (var line in localRegion.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            list += word.Text + " ";
                        }
                    }
                }
            }
            return list.Trim();
        }
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
        public static async Task<string> MakeOCRRequest()
        {
            string subscriptionKeyFilePath = "../SigniSightAPI/Controllers/OCRKey.txt";
            string subscriptionKey = File.ReadAllText(subscriptionKeyFilePath);
            string endpointFilePath = "../SigniSightAPI/Controllers/OCREndpoint.txt";
            string endpoint = File.ReadAllText(endpointFilePath);
            //string imageFilePath = @"C:\Users\rajeesh.raveendran\Desktop\bill.jpg";
            string localImage = @"C:/Users/shado/Desktop/Учло/colorful-chalk-drawing-asphalt-polish-words-good-morning-chalk-drawing-polish-words-good-morning-131362042.jpg";
            var errors = new List<string>();
            string extractedResult = "";
            ImageInfoViewModel responeData = new ImageInfoViewModel();

            try
            {
                HttpClient client = new HttpClient();

                //// Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameters.
                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API Call.
                string uri = endpoint + "?" + requestParameters;

                HttpResponseMessage response;

                // Request body. Posts a locally stored JPEG image.
                byte[] byteData = GetImageAsByteArray(localImage);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Make the REST API call.
                    response = await client.PostAsync(uri, content);
                }

                // Get the JSON response.
                string result = await response.Content.ReadAsStringAsync();

                //If it is success it will execute further process.
                if (response.IsSuccessStatusCode)
                {
                    // The JSON response mapped into respective view model.
                    responeData = JsonConvert.DeserializeObject<ImageInfoViewModel>(result,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Include,
                            Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs earg)
                            {
                                errors.Add(earg.ErrorContext.Member.ToString());
                                earg.ErrorContext.Handled = true;
                            }
                        }
                    );

                    var linesCount = responeData.regions[0].lines.Count;
                    for (int i = 0; i < linesCount; i++)
                    {
                        var wordsCount = responeData.regions[0].lines[i].words.Count;
                        for (int j = 0; j < wordsCount; j++)
                        {
                            //Appending all the lines content into one.
                            extractedResult += responeData.regions[0].lines[i].words[j].text + " ";
                        }
                        extractedResult += Environment.NewLine;
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
            return extractedResult;
        }
        public static async Task<string> MakeOCRRequest2()
        {
            string subscriptionKeyFilePath = "../SigniSightAPI/Controllers/OCRKey.txt";
            string subscriptionKey = File.ReadAllText(subscriptionKeyFilePath);
            string endpointFilePath = "../SigniSightAPI/Controllers/OCREndpoint.txt";
            string endpoint = File.ReadAllText(endpointFilePath);

            string imageFilePath = @"C:/Users/shado/Desktop/Учло/colorful-chalk-drawing-asphalt-polish-words-good-morning-chalk-drawing-polish-words-good-morning-131362042.jpg";
            var errors = new List<string>();
            string extractedResult = "";
            ImageInfoViewModel responeData = new ImageInfoViewModel();

            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameters.
                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API Call.
                string uri = endpoint + "?" + requestParameters;

                HttpResponseMessage response;

                // Request body. Posts a locally stored JPEG image.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Make the REST API call.
                    response = await client.PostAsync(uri, content);
                }

                // Get the JSON response.
                string result = await response.Content.ReadAsStringAsync();

                //If it is success it will execute further process.
                if (response.IsSuccessStatusCode)
                {
                    // The JSON response mapped into respective view model.
                    responeData = JsonConvert.DeserializeObject<ImageInfoViewModel>(result,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Include,
                            Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs earg)
                            {
                                errors.Add(earg.ErrorContext.Member.ToString());
                                earg.ErrorContext.Handled = true;
                            }
                        }
                    );

                    var linesCount = responeData.regions[0].lines.Count;
                    for (int i = 0; i < linesCount; i++)
                    {
                        var wordsCount = responeData.regions[0].lines[i].words.Count;
                        for (int j = 0; j < wordsCount; j++)
                        {
                            //Appending all the lines content into one.
                            extractedResult += responeData.regions[0].lines[i].words[j].text + " ";
                        }
                        extractedResult += Environment.NewLine;
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
            return extractedResult;
        }

    }
}
