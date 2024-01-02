using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TwitterClone.Models;

namespace TwitterClone.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _apiBaseUrl = "https://localhost:44357/api/tweet";
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetAllTweet()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(_apiBaseUrl);
                response.EnsureSuccessStatusCode();
                var tweet = await response.Content.ReadAsAsync<IEnumerable<Tweet>>();
                return Json(tweet, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetTweet(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{_apiBaseUrl}/{id}");
                response.EnsureSuccessStatusCode();
                var tweet = await response.Content.ReadAsAsync<Tweet>();
                return Json(tweet, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddTweet(Tweet newTweet)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync(_apiBaseUrl, newTweet);
                response.EnsureSuccessStatusCode();
                var addedTweet = await response.Content.ReadAsAsync<Tweet>();
                return Json(addedTweet);
            }
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteTweet(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"{_apiBaseUrl}/{id}");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<string>();
                return Json(new { Message = result });
            }
        }
    }
}