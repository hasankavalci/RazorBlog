using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogLayer.Models.DataModel;
using BlogLayer.CF;

namespace BlogLayer.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            RazerBlogContext _db = new RazerBlogContext();
            List<Makale> MakalelerList = _db.Makales.ToList();
            ViewBag.MakalelerList = MakalelerList;
                      
            return View();
        }
    }
}