using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TwitterClone.Models;

namespace TwitterClone.Controllers
{
    [Route("api/tweet")]
    public class TweetApiController : ApiController
    {
        private readonly string _connectionString;

        public TweetApiController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        }
        [HttpGet]
        public IHttpActionResult GetTweetsWithUser()
        {
            using (SqlConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                    
                var query = @"
                        SELECT 
                        t.TweetId, t.TweetContent, t.TweetImg, t.TweetTime,
                        u.UserId AS UserId, u.UserName, u.UserLink, u.UserDescription, u.UserImg
                        FROM Tweet t
                        JOIN [User] u ON t.UserId = u.UserId
                    ";

                var tweetsWithUser = dbConnection.Query<Tweet, User, Tweet>(
                    query,
                    (tweet, user) =>
                    {
                        tweet.User = user;
                        return tweet;
                    },
                    splitOn: "UserId"
                );

                return Ok(tweetsWithUser);
            }
        }


    }
}
