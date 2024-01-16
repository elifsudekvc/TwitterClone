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
                        u.UserId AS UserId, u.UserName, u.UserLink, u.UserImg
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

        [HttpGet]
        [Route("api/tweet/{id}")]
        public IHttpActionResult GetTweetById(int id)
        {
            using (SqlConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                var query = @"
            SELECT 
            t.TweetId, t.TweetContent, t.TweetImg, t.TweetTime,
            u.UserId AS UserId, u.UserName, u.UserLink, u.UserImg
            FROM Tweet t
            JOIN [User] u ON t.UserId = u.UserId
            WHERE t.TweetId = @TweetId
        ";

                var tweetWithUser = dbConnection.Query<Tweet, User, Tweet>(
                    query,
                    (tweet, user) =>
                    {
                        tweet.User = user;
                        return tweet;
                    },
                    new { TweetId = id },
                    splitOn: "UserId"
                ).FirstOrDefault();

                if (tweetWithUser != null)
                {
                    return Ok(tweetWithUser);
                }
                else
                {
                    return NotFound();
                }
            }
        }


        [HttpPost]
        public IHttpActionResult AddTweet(Tweet newTweet)
        {
            using (SqlConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                var query = @"
                INSERT INTO Tweet (TweetContent, TweetImg, TweetTime, UserId)
                VALUES (@TweetContent, @TweetImg, GETDATE(), (SELECT [UserId] FROM [dbo].[User] WHERE [UserEmail] = @UserEmail));
                SELECT CAST(SCOPE_IDENTITY() as int)
            ";

                var tweetId = dbConnection.Query<int>(query, new { TweetContent = newTweet.TweetContent, TweetImg = newTweet.TweetImg, UserEmail = User.Identity.Name }).Single();
                newTweet.TweetId = tweetId;

                return Ok(newTweet);
            }
        }

        [HttpDelete]
        [Route("api/tweet/{id}")]
        public IHttpActionResult DeleteTweet(int id)
        {
            try
            {
                using (SqlConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();

                    var affectedRows = dbConnection.Execute("DELETE FROM Tweet WHERE TweetId = @TweetId", new { TweetId = id });

                    if (affectedRows > 0)
                    {
                        return Ok("Tweet deleted successfully");
                    }
                    else
                    {
                        return BadRequest("Failed to delete Tweet");
                    }
                }
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }


    }
}
