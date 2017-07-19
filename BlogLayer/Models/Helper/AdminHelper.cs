using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogLayer.Models.Helper
{
    public class AdminHelper
    {
        [
            Required(ErrorMessage ="Kullanıcı Adi Giriniz"),
            DisplayName("Kullanıcı Adi")
        ]
        public string UserName { get; set; }
        [
            Required(ErrorMessage = "Şifre Adi Giriniz"),
            DisplayName("Şifre")
        ]
        public string Password { get; set; }


    }
}