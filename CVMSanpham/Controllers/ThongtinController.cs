using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CVMSanpham.Models;
namespace CVMSanpham.Controllers
{
    public class ThongtinController : Controller
    {
        // GET: Thongtin
        dbQLstoreDataContext data = new dbQLstoreDataContext();
        public ActionResult Thongtin()
        {
            var Thongtin = from tt in data.ThongTins  select tt;
            return View(Thongtin);
        }
    }
}