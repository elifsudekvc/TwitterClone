using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TwitterClone.Models;
using Dapper;

namespace TwitterClone.Controllers
{
    [Route("api/user")]
    public class UserApiController : ApiController
    {
        //UserProfile
        //burada profil sayfası olacak userdaki resim isim açıklama kullanılacak
        //o kişinin attığı tweetler gelecek

        private readonly string _connectionString;

        public UserApiController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        }


        [HttpGet]
        [Route("api/user/{userId:int}")]
        public IHttpActionResult UserProfile(int userId)
        {
            using (SqlConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                var userNameQuery = @"
            SELECT TOP 1 UserName
            FROM [User]
            WHERE UserId = @UserId
        ";

                string userName = dbConnection.QueryFirstOrDefault<string>(userNameQuery, new { UserId = userId });

                var query = @"
            SELECT 
            t.TweetId, t.TweetContent, t.TweetImg, t.TweetTime,
            u.UserId AS UserId, u.UserName, u.UserLink, u.UserImg
            FROM Tweet t
            JOIN [User] u ON t.UserId = u.UserId
            WHERE u.UserId = @UserId
        ";

                var result = dbConnection.Query<Tweet, User, Tweet>(
                    query,
                    (tweet, user) =>
                    {
                        tweet.User = user;
                        return tweet;
                    },
                    new { UserId = userId },
                    splitOn: "UserId"
                );

                
                return Ok(result);
            }
        }


    }
}
