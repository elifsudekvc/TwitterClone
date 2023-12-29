using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterClone.Models
{
    public class UserTweetsView
    {
        public int TweetId { get; set; }
        public string TweetContent { get; set; }
        public string TweetImg { get; set; }
        public DateTime TweetTime { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserLink { get; set; }
        public string UserDescription { get; set; }
        public string UserImg { get; set; }
    }
}