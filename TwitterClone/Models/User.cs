using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TwitterClone.Models
{

    public class User
    {
        public int UserId { get; set; }

        [DisplayName("İsim")]
        public string UserName { get; set; }
        [DisplayName("Kullanıcı Adı")]
        public string UserLink { get; set; }
        [DisplayName("Kişisel Bilgiler")]
        public string UserDescription { get; set; }
        [DisplayName("Fotoğraf")]
        public string UserImg { get; set; }
        [DisplayName("Email")]
        public string UserEmail { get; set; }
        [DisplayName("Parola")]
        public string UserPassword { get; set; }
        public virtual Tweet tweet { get; set; }

        public void HashPassword()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(UserPassword));
                UserPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

    }

}