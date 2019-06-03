using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CVMSanpham.Models;
using PagedList;
using PagedList.Mvc;

namespace CVMSanpham.Controllers
{
    public class LaptopstoreController : Controller
    {
        // GET: Laptopstore
        dbQLstoreDataContext data= new dbQLstoreDataContext();
        private List<SanPham> laylapmoi(int count)
        {
            return data.SanPhams.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        public ActionResult Index()
        {
            var lapmoi = laylapmoi(7);
            return View(lapmoi);
        }
        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }

        public ActionResult Hang()
        {
            var hang = from h in data.Hangs select h;
            return PartialView(hang);
        }
        [ChildActionOnly]
        public ActionResult _pSanpham(int? page)

        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            var Sanpham = from sp in data.SanPhams select sp;
            return PartialView(Sanpham.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Sptheochdue(int id)
        {
            var Sanpham = from s in data.SanPhams where s.MaCD == id select s;
            return View(Sanpham);
        }

        public ActionResult Sptheohang(int id)
        {
            var Sanpham = from s in data.SanPhams where s.MaHang == id select s;
            return View(Sanpham);
        }

        public ActionResult Slider()
        {
            var Slider = from sl in data.Sliders select sl;
            return PartialView(Slider);
        }

        public ActionResult Details(int id)
        {
            var Sanpham = from s in data.SanPhams
                where s.MaSP == id
                select s;
            return View(Sanpham.Single());
        }

        public ActionResult SpPartial (int id)
        {
            var Sanpham = from s in data.SanPhams
                where s.MaSP == id
                select s;
            return View(Sanpham.Single());

        }
        
    }
}