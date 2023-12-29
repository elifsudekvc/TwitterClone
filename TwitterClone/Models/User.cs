using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterClone.Models
{

    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserLink { get; set; }
        public string UserDescription { get; set; }
        public string UserImg { get; set; }
    }

}