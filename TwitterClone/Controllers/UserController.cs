using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TwitterClone.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TwitterClone.Controllers
{
    public class UserController : Controller
    {
        private readonly string _apiBaseUrl = "https://localhost:44357/api/users";
        public ActionResult Index(int? id)
        {
            ViewBag.UserId = id;
            return View();
        }

        public async Task<JsonResult> getProfile(int userId)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{_apiBaseUrl}/{userId}");
                response.EnsureSuccessStatusCode();
                var user = await response.Content.ReadAsAsync<User>();

                var tweetsResponse = await httpClient.GetAsync($"{_apiBaseUrl}/user/{userId}/tweet");
                tweetsResponse.EnsureSuccessStatusCode();
                var tweets = await tweetsResponse.Content.ReadAsAsync<List<Tweet>>();
                var viewModel = new UserTweetsView
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    UserLink = user.UserLink,
                    UserDescription = user.UserDescription,
                    UserImg = user.UserImg,
                    UserTweets = tweets
                };

                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

        }
    }
}