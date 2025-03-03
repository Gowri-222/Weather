using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Weather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : Controller
    {
        private const string API_KEY = "72cf8fe946ad54c62f814a3b58a1f1d4";  // Replace with your OpenWeather API key
        private const string CITY = "Bangalore";  // Change this to your preferred city

        // ✅ GET API to fetch date, temperature & season
        [HttpGet]
        public async Task<IActionResult> GetWeather()
        {
            string temperature = await GetTemperatureAsync(CITY);
            string season = GetSeason(DateTime.Now);

            var result = new
            {
                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                Temperature = temperature,
                Season = season
            };

            return Ok(result);
        }

        // ✅ Fetch current temperature from OpenWeatherMap API
        private async Task<string> GetTemperatureAsync(string city)
        {
            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q=Bangalore&appid=72cf8fe946ad54c62f814a3b58a1f1d4&units=metric";

            using HttpClient client = new HttpClient();
            var response = await client.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
                return "Error fetching temperature";

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            double temp = doc.RootElement.GetProperty("main").GetProperty("temp").GetDouble();

            return $"{temp} °C";
        }

        // ✅ Determine the season based on the month
        private string GetSeason(DateTime date)
        {
            int month = date.Month;
            return month switch
            {
                12 or 1 or 2 => "Winter",
                3 or 4 or 5 => "Spring",
                6 or 7 or 8 => "Summer",
                9 or 10 or 11 => "Autumn",
                _ => "Unknown"
            };
        }

    }
}
