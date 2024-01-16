using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TwitterClone.Models;
using Dapper;

namespace TwitterClone.Controllers
{
    public class LoginController : Controller
    {
        private readonly string _connectionString;

        public LoginController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(User user)
        {
            using (SqlConnection dbConnection = new SqlConnection(_connectionString))
            {
                await dbConnection.OpenAsync();

                user.HashPassword();

                await dbConnection.QueryAsync<User>("INSERT INTO [User] (UserName, UserLink, UserEmail, UserPassword) VALUES (@UserName, @UserLink, @UserEmail, @UserPassword)", user);
                ModelState.Clear();
            }
            FormsAuthentication.SetAuthCookie(user.UserEmail, true);


            Session["UserId"] = user.UserId;
            Session["UserName"] = user.UserName;
            Session["UserLink"] = user.UserLink;
            Session["UserPassword"] = user.UserPassword;
            return RedirectToAction("Index", "Home");
        }

        public class NoCacheAttribute : ActionFilterAttribute
        {
            public override void OnResultExecuting(ResultExecutingContext filterContext)
            {
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetNoStore();
                base.OnResultExecuting(filterContext);
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(Login login)
        {
            using (SqlConnection dbConnection = new SqlConnection(_connectionString))
            {
                await dbConnection.OpenAsync();

                var user = await dbConnection.QueryFirstOrDefaultAsync<User>("SELECT * FROM [User] WHERE UserEmail = @UserEmail", login);

                if (user != null)
                {
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(login.UserPassword));
                        string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                        if (user.UserPassword == hashedPassword)
                        {
                            FormsAuthentication.SetAuthCookie(user.UserEmail, true);

                            Session["UserId"] = user.UserId;
                            Session["UserName"] = user.UserName;
                            Session["UserLink"] = user.UserLink;
                            Session["UserDescription"] = user.UserDescription;
                            Session["UserEmail"] = user.UserEmail;
                            Session["tweet"] = user.tweet;
                            Session["UserPassword"] = login.UserPassword;
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }

            return View();
        }


        [NoCache]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            this.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Response.Cache.SetNoStore();
            return RedirectToAction("Login", "Login");
        }
    }
}