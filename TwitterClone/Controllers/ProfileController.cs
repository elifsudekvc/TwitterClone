using TwitterClone;
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
    public class ProfileController : Controller
    {
        private readonly string _apiBaseUrl = "https://localhost:44357/api/user";
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorize]
        public async Task<JsonResult> UserProfile(int userId)
        {
            using (var httpClient = new HttpClient())
            {
                var userResponse = await httpClient.GetAsync($"{_apiBaseUrl}/user/{userId}");
                userResponse.EnsureSuccessStatusCode();
                var user = await userResponse.Content.ReadAsAsync<User>();

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