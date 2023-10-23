using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tabemory.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Newtonsoft.Json;

namespace Tabemory.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Search()
        {
            Restaurant restaurant = new Restaurant();
            return View(restaurant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string[] values)
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    DotNetEnv.Env.Load(".env");
                    var key = DotNetEnv.Env.GetString("Recruit_API");
                    string keyword = values[0].Replace(" ", "+");
                    keyword = keyword.Replace("　", "+");
                    string url = webClient.DownloadString($"http://webservice.recruit.co.jp/hotpepper/gourmet/v1/?key={key}&keyword={keyword}&order=4&format=json");

                    var restaurant = JsonConvert.DeserializeObject<Restaurant>(url);

                    return View(restaurant);
                }
                catch (Exception ex)
                {
                    Restaurant restaurant = new Restaurant();
                    restaurant.ErrMsg = "検索エラーです。";
                    return View(restaurant);
                }

            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}