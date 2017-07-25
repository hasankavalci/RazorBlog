using BlogLayer.CF;
using BlogLayer.Models.DataModel;
using BlogLayer.Models.Helper;
using System;
using System.Collections.Generic;
using System.IO;
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
        public ActionResult KategoriSil(Category c)
        {
            RazerBlogContext _db = new RazerBlogContext();
            Category ToDel = _db.Categories.First(x => x.CategoryID == c.CategoryID);
            _db.Categories.Remove(ToDel);
            _db.SaveChanges();
            return RedirectToAction("KategoriListele");
        }
        public ActionResult KategoriGuncelle(int KategoriID)
        {
            RazerBlogContext _db = new RazerBlogContext();
            Category ToEdit = _db.Categories.First(x => x.CategoryID == KategoriID);
            return View(ToEdit);
        }
        [HttpPost]
        public ActionResult KategoriGuncelle(Category c)
        {
            RazerBlogContext _db = new RazerBlogContext();
            Category ToEdit = _db.Categories.First(x => x.CategoryID == c.CategoryID);
            ToEdit.Name = c.Name;
            ToEdit.UrlName = c.UrlName;
            _db.SaveChanges();
            return RedirectToAction("KategoriListele");
        }
        public List<Category> TumKategoriler()
        {
            RazerBlogContext _db = new RazerBlogContext();
            List<Category> KategorilerList = _db.Categories.ToList();            
            ViewBag.kategoriler = KategorilerList;
            return KategorilerList;
        }
        public List<Admin> TumAdmin()
        {
            RazerBlogContext _db = new RazerBlogContext();
            List<Admin> AdminList = _db.Admins.ToList();
            ViewBag.kategoriler = AdminList;
            return AdminList;
        }
        public ActionResult MakaleEkle()
        {
            TumKategoriler();
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult MakaleEkle(FormCollection frm, HttpPostedFileBase file)
        {            
            string ViewTitle = frm.Get("Title");
            string ViewDescription = frm.Get("Description");
            int ViewCategoryID =Convert.ToInt32(frm.Get("Category"));
            string ViewText = frm.Get("Text");
            string ViewKeywords = frm.Get("Keywords");
            int ViewYazarID = Convert.ToInt32(Session["adminid"]);

            Random rnd = new Random();
            string sayi = rnd.Next(111111, 999999).ToString();
            var FileName = Path.GetFileName(file.FileName);
            var File = Path.Combine(Server.MapPath("~/MakaleResimler/"), sayi + FileName);
            file.SaveAs(File);
            string FilePath = "~/MakaleResimler/" + sayi + FileName;

            Makale ToAdd = new Makale();
            ToAdd.AuthorName = TumAdmin().FirstOrDefault(x => x.AdminID == ViewYazarID);
            ToAdd.Category = TumKategoriler().FirstOrDefault(x => x.CategoryID == ViewCategoryID);
            ToAdd.Description = ViewDescription;
            ToAdd.Image = FilePath;
            ToAdd.Keywords = ViewKeywords;
            ToAdd.text = ViewText;
            ToAdd.Title = ViewTitle;

            RazerBlogContext _db = new RazerBlogContext();
            _db.Makales.Add(ToAdd);
            if( _db.SaveChanges()>0)
            {
                ViewBag.Mesaj = "Makale Ekleme Başarılı";
            }
            else
            {
                ViewBag.Mesaj = "";
            }
            return View();
        }
        public ActionResult MakaleListele()
        {
            RazerBlogContext _db = new RazerBlogContext();           
            return View(_db.Makales.ToList());
        }
        public ActionResult MakaleSil(int GelenID)
        {
            RazerBlogContext _db = new RazerBlogContext();
            Makale ToDel = _db.Makales.FirstOrDefault(x => x.MakaleID == GelenID);
            return View(ToDel);
        }
        [HttpPost]
        public ActionResult MakaleSil(Makale m)
        {
            RazerBlogContext _db = new RazerBlogContext();
            Makale ToDel = _db.Makales.FirstOrDefault(x => x.MakaleID == m.MakaleID);
            _db.Makales.Remove(ToDel);
            _db.SaveChanges();
            return RedirectToAction("MakaleListele");
        }
        public void yakalaıd(int yakalananıd)
        {
            RazerBlogContext _db = new RazerBlogContext();
            List<SelectListItem> katlist = new List<SelectListItem>();
            var katsorgu = _db.Categories.ToList();
            foreach (var item in katsorgu)
            {
                if (yakalananıd == item.CategoryID)
                {
                    katlist.Add(new SelectListItem { Text = item.Name, Value = item.CategoryID.ToString(), Selected = true });
                }
                else
                {
                    katlist.Add(new SelectListItem { Text = item.Name, Value = item.CategoryID.ToString() });
                }
                               
            }
            ViewBag.kat = katlist;
        }
        public ActionResult MakaleGuncelle(int GelenMakaleID,FormCollection frm)
        {
            
            RazerBlogContext _db = new RazerBlogContext();
            Makale ToEdit = _db.Makales.FirstOrDefault(x => x.MakaleID == GelenMakaleID);
            yakalaıd(Convert.ToInt32(ToEdit.Category.CategoryID));
            return View(ToEdit);
        }       
    }
}