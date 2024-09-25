using Microsoft.AspNetCore.Mvc;
using ConsumeWalkApi.Models;
using System.Text.Json;
using System.Text;
using System.Reflection;


namespace ConsumeWalkApi.Controllers
{
    public class RegionController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {

            List<RegoinDTO> response = new List<RegoinDTO>();

            var Client = httpClientFactory.CreateClient();
            var httpResponseMessage = await Client.GetAsync("https://localhost:7165/api/Regions");

            httpResponseMessage.EnsureSuccessStatusCode();

            response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegoinDTO>>());

            //var stringResponseBody = await httpResponseMessage.Content.ReadAsStringAsync();
            //ViewBag.Response = stringResponseBody;

            return View(response);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddRegionModel model)
        {
            var Client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7165/api/Regions"),
                Content = new StringContent(JsonSerializer.Serialize(model),Encoding.UTF8,"application/Json"),
            };

            var httpResponseMessage = await Client.SendAsync(httpRequestMessage);
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegoinDTO>();

            if(response is not null )
            {
                return RedirectToAction("Index","Region");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var Client = httpClientFactory.CreateClient();

            var response = await Client.GetFromJsonAsync<RegoinDTO>($"https://localhost:7165/api/Regions/{id.ToString()}");

            if(response is not null)
            {
                return View(response);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, AddRegionModel model)
        {
            var Client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7165/api/Regions/{id.ToString()}"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/Json"),
            };

            var httpResponseMessage = await Client.SendAsync(httpRequestMessage);
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegoinDTO>();

            if (response is not null)
            {
                return RedirectToAction("Index", "Region");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegoinDTO dto)
        {
            var Client = httpClientFactory.CreateClient();
            var httpresponse = await Client.DeleteAsync($"https://localhost:7165/api/Regions/{dto.Id.ToString()}");
            httpresponse.EnsureSuccessStatusCode();
            return RedirectToAction("Index", "Region");

           
        }
    }
}
