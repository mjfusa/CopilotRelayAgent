using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

class AzureAISearch
{
    private static readonly string searchServiceName = "azureaisearch101"; // Replace with your Azure Search Service name
    private static readonly string indexName = "hotels-sample-index";           // Replace with your index name
    private static readonly string apiKey = "";                 // Replace with your API key
    private static readonly string apiVersion = "2021-04-30-Preview";         // API version

    public static async Task<string> Search(string[] args)
    {
        string searchUrl = $"https://{searchServiceName}.search.windows.net/indexes/{indexName}/docs/search?api-version={apiVersion}";

        // Define the search request payload
        var payload = new
        {
            search = "hotels in new york with a pool",        // Search term
            filter = "Rating gt 3.0", // Optional filter
            top = 10                        // Number of results
        };

        // Convert the payload to JSON
        var jsonPayload = System.Text.Json.JsonSerializer.Serialize(payload);
        string result = string.Empty;
        string error = string.Empty;

        // Make the HTTP POST request
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("api-key", apiKey); // Add API key to headers

            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PostAsync(searchUrl, content).Result;

            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Search Results:");
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Details: {error}");
            }
        }
    return string.IsNullOrEmpty(result) ? error : result;
    }
}