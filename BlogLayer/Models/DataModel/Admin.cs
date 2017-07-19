﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BlogLayer.Models.DataModel
{
    public class Admin
    {
        public int AdminID { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string EMail { get; set; }        
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}