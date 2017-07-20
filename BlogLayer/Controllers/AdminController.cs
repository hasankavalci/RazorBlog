﻿using BlogLayer.CF;
using BlogLayer.Models.DataModel;
using BlogLayer.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogLayer.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            bool giris = Convert.ToBoolean(Session["giris"]);
            if (giris == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }            
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Login(AdminHelper model)
        {
            if (ModelState.IsValid)
            {
                using (RazerBlogContext _db=new RazerBlogContext())
                {
                    var admin = _db.Admins.FirstOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);
                    if (admin != null)
                    {
                        Session.Add("giris", true);
                        Session.Add("adminid", admin.AdminID);
                        return RedirectToAction("Index", "Admin");
                    }
                }
            }
            ViewBag.mesaj = "Kullanıcı Adı veya Şifre Yanlış";
            return View();
        }
        public ActionResult KategoriEkleOto()
        {
            return View();
        } 
        [HttpPost]
        public ActionResult KategoriEkleOto(FormCollection frm)
        {
            return Ozhakikikategoriekle(frm);
        }      
        public ActionResult KategoriEkle()
        {
            return View();
        }
        public ActionResult Ozhakikikategoriekle(FormCollection frm)
        {
            string KategoriAdi = frm.Get("CategoryName");
            if (KategoriAdi == "")
            {
                ViewBag.hatavar = true;
                ViewBag.hata = "Kategori Adi Boş Geçilemez";
            }
            else
            {
                ViewBag.hatavar = false;
                RazerBlogContext _db = new RazerBlogContext();
                List<Category> CakisanKategoriler = _db.Categories.Where(x => x.Name == KategoriAdi).ToList();
                if (CakisanKategoriler.Count > 0)
                {
                    ViewBag.hatavar = true;
                    ViewBag.hata = "Bu Kategori Zaten Mevcut";
                }
                else
                {
                    ViewBag.hatavar = false;
                    CategoryHelper chelp = new CategoryHelper();
                    chelp.kategoriolustur(KategoriAdi);
                    Category c = new Category();
                    c.Name = chelp.CategoryName;
                    c.UrlName = chelp.UrlName;
                    _db.Categories.Add(c);
                    if (_db.SaveChanges() == 0)
                    {
                        ViewBag.hatavar = true;
                        ViewBag.hata = "Veritabanı Bağlantısında Sorun var";
                    }
                    else
                    {
                        ViewBag.hatavar = false;
                    }
                }
            }
            return View();
        }       
        [HttpPost]
        public ActionResult KategoriEkle(FormCollection frm)
        {
           return Ozhakikikategoriekle(frm);
        }
        public ActionResult KategoriListele()
        {
            RazerBlogContext _db = new RazerBlogContext();
            return View(_db.Categories.ToList());
        }
        public ActionResult KategoriSil(int KategoriID)
        {
            RazerBlogContext _db = new RazerBlogContext();
            Category CategoryToDel = _db.Categories.First(x => x.CategoryID == KategoriID);
            return View(CategoryToDel);
        }
        [HttpPost]
        public ActionResult KategoriSil(FormCollection frm)
        {
            int gelenid = Convert.ToInt32(frm.Get("IDCarrier"));
            RazerBlogContext _db = new RazerBlogContext();
            Category toDel = _db.Categories.First(x => x.CategoryID == gelenid);
            _db.Categories.Remove(toDel);
            _db.SaveChanges();
            return RedirectToAction("KategoriListele");
        }
    }
}