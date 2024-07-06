using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mapper.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mapper.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private string GetApiKeyMaps()
        {
            return _configuration["GoogleMaps:ApiKey"];
        }
        private string GetApiKeySheets()
        {
            return _configuration["GoogleSheets:ApiKey"];
        }

        [HttpGet("api/maps")]
        public async Task<IActionResult> GetMapScript()
        {
            string apiKeyMaps = GetApiKeyMaps();
            string apiUrl = $"https://maps.googleapis.com/maps/api/js?key={apiKeyMaps}&callback=initMap&libraries=&v=weekly";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(apiUrl);
                return Content(response, "application/javascript");
            }
        }
        public async Task<ActionResult> Index()
        {
            var markerData = await GetMarkerDataFromGoogleSheets();
            return View(markerData);
        }

        private async Task<List<MarkerViewModel>> GetMarkerDataFromGoogleSheets()
        {
            string apiKeyMaps = GetApiKeyMaps();
            string apiKeySheets = GetApiKeySheets();
            string apiUrl = $"https://sheets.googleapis.com/v4/spreadsheets/{apiKeySheets}/values/Sheet1!A:E?key={apiKeyMaps}";

            List<MarkerViewModel> markers = new List<MarkerViewModel>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                Console.WriteLine(response.Content);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API response data: " + responseData);

                    var data = JsonConvert.DeserializeObject<GoogleSheetResponse>(responseData);

                    if (data != null && data.Values != null)
                    {
                        for (int i = 1; i < data.Values.Count; i++) 
                        {
                            var row = data.Values[i];
                            if (row.Count >= 5) 
                            {
                                string website = (string)row[0];
                                string title = (string)row[1];
                                string location = (string)row[2]; 
                                double lat = Convert.ToDouble(row[3]); 
                                double lng = Convert.ToDouble(row[4]); 

                                var marker = new MarkerViewModel
                                {
                                    Title = title,
                                    Website = website,
                                    Location = location, 
                                    Lat = lat,
                                    Lng = lng
                                };

                                markers.Add(marker);
                            }
                            else
                            {
                                Console.WriteLine("Error: Insufficient data columns in a row.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Empty or invalid data received from Google Sheets API.");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            return markers;
        }
    }
}
