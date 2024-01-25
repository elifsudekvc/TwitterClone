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
    public class UsersApiController : ApiController
    {
        private readonly string _connectionString;

        public UsersApiController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        }

        [HttpGet]
        [Route("api/users/{userId:int}")]
        public IHttpActionResult UserIndex(int userId)
        {
            using (SqlConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                // Fetch user information
                var userQuery = @"
            SELECT UserId, UserName, UserLink, UserDescription, UserImg, UserEmail
            FROM [User]
            WHERE UserId = @UserId
        ";

                User user = dbConnection.QueryFirstOrDefault<User>(userQuery, new { UserId = userId });

                if (user == null)
                {
                    return NotFound(); // User not found
                }

                // Fetch tweets for the user
                var tweetsQuery = @"
            SELECT TweetId, TweetContent, TweetImg, TweetTime
            FROM Tweet
            WHERE UserId = @UserId
        ";

                var tweets = dbConnection.Query<Tweet>(tweetsQuery, new { UserId = userId });

                user.Tweets = tweets.ToList();

                return Ok(user);
            }
        }
    }
}
