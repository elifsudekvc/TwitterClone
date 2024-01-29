using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public virtual User User { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }
    }

}