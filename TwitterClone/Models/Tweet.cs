using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterClone.Models
{
    public class Tweet
    {
        public int TweetId { get; set; }
        public string TweetContent { get; set; }
        public string TweetImg { get; set;}
        public DateTime TweetTime { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }

}