using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace web_clima.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetWeather(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                ViewBag.Error = "Por favor, insira uma cidade válida.";
                return View("Index");
            }

            var client = _httpClientFactory.CreateClient();
            var apiKey = "b719f314b22e648c40b319b0a1c8e7a5";
            var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric");

            if (response.IsSuccessStatusCode)
            {
                var weatherData = await response.Content.ReadAsStringAsync();
                var weather = JsonConvert.DeserializeObject<WeatherResponse>(weatherData);

                // Aplicar o arredondamento na temperatura
                if (weather.Main.Temp % 1 >= 0.5)
                {
                    weather.Main.Temp = (float)Math.Ceiling(weather.Main.Temp); // Arredonda para cima
                }
                else
                {
                    weather.Main.Temp = (float)Math.Floor(weather.Main.Temp); // Arredonda para baixo
                }

                return View("Index", weather);
            }
            else
            {
                ViewBag.Error = "Cidade não encontrada.";
                return View("Index");
            }
        }
    }

    public class WeatherResponse
    {
        public Main Main { get; set; }
        public Wind Wind { get; set; }
        public string Name { get; set; }
    }

    public class Main
    {
        public float Temp { get; set; }
        public float Humidity { get; set; }
    }

    public class Wind
    {
        public float Speed { get; set; }
    }
}