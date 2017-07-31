using BlogLayer.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BlogLayer.CF
{
    public class RazerBlogContext:DbContext
    {
        public RazerBlogContext()
        {
            Database.Connection.ConnectionString = "Server=.;Database=MyBlog;User Id=sa;Password=123456789?";
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Makale> Makales { get; set; }
        public DbSet<Yorum> Yorums { get; set; }
        public DbSet<SistemMail> SistemMails { get; set; }
    }
   
    
}