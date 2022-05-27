using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SigniSightModel;
using SigniSightBL;
using SigniSightDL;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace SigniSightBL
{
    public class Logic : ILogic
    {
        private readonly IRepo _repo;
        public Logic(IRepo _repo)
        {
            this._repo = _repo;
        }

        public User AddUser(User userToAdd)
        {
            return _repo.AddUser(userToAdd);
        }


        public bool Authenticate(User user)
        {
            List<User> users = _repo.GetAllUsers();
            if (users.Exists(auth => auth.Username == user.Username && auth.Password == user.Password))
                return true;
            else
                return false;
        }

        public List<User> GetUserAccount(string Username, string Password)
        {
            List<User> users = _repo.GetAllUsers();
            var filteredUsernames = users.Where(user => user.Username.ToLower().Equals(Username)
            && user.Password.ToLower().Equals(Password)).ToList();
            return filteredUsernames;
        }
        public class OCRProcessor
        {
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
                    foreach (Line line in page.Lines)
                    {
                        list += line.Text;
                    }
                }
                return list;
            }
        }
    }
}