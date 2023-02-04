using System.Net;
using System.Text.Json;
using System.Text;

namespace astralSafeClientMaui
{
    internal class JsonRequest
    {
        public string UID { get; set; }
        public string License { get; set; }
        public string Key { get; set; }
        public bool Valid { get; set; }

        private readonly HttpClient HttpClient;

        private readonly string BaseUrl;

        public JsonRequest()
        {
            BaseUrl = "http://localhost:8080/api/";
            
            HttpClient = new();

            UID = null;
            License = null;
            Key = null;
            Valid = false;
        }

        public JsonRequest(string UID) : this()
        {
            this.UID = UID;
        }

        public JsonRequest(string UID, string license) : this()
        {
            this.UID = UID;
            License = license;
        }

        public void SendRequest(string option)
        {
            HttpClient.BaseAddress = new(uriString: BaseUrl + option);

            string jsonReturned = "";
            string jsonSent;

            switch (option)
            {
                case "keygen":

                    jsonSent = "{\"uid\":\"" + UID + "\"}";
                    jsonReturned = Send(json: jsonSent, endpoint: option);                  
                    break;
                    
                case "validate-license":

                    jsonSent = "{\"uid\":\"" + UID + "\",\"license\":\"" + License + "\"}";
                    jsonReturned = Send(json: jsonSent, endpoint: option);
                    break;
                    
                case null:
                    throw new(message: "Invalid option");
            }

            if (jsonReturned.Equals(""))
            {
                throw new(message: "Invalid json");
            }

            JsonRequest jr = JsonSerializer.Deserialize<JsonRequest>(json: jsonReturned);

            Key = jr.Key;
            License = jr.License;
        }

        private string Send(string json, string endpoint)
        {
            var response = HttpClient.PostAsync(requestUri: endpoint, content: new StringContent(content: json, Encoding.UTF8,mediaType: "application/json")).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent responseContent = response.Content;
                string responseString = responseContent.ReadAsStringAsync().Result;
                return responseString;
            }
            else
            {
                throw new(message: "Error in request");
            }
        }
    }
}
