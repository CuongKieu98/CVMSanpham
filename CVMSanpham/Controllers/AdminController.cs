using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CVMSanpham.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace CVMSanpham.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbQLstoreDataContext db= new dbQLstoreDataContext();
        public ActionResult Index()
        {
            var Khachhang = from kh in db.KhachHangs select kh;
            return View(Khachhang);
        }
        public ActionResult Laptop(int? page)
        {
           /* if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }*/
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(db.SanPhams.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult hangsanxuat()
        {
            /*if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }*/
            return View(db.Hangs.ToList());
        }
        public ActionResult chudesp()
        {
            /*
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }*/
            return View(db.CHUDEs.ToList());
        }

        public ActionResult thongtin()
        {
            /*
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }*/
            return View(db.ThongTins.ToList());

        }

        public ActionResult quangcao()
        {
            return View(db.QuangCaos.ToList());
        }

        public ActionResult slide()
        {
            return View(db.Sliders.ToList());
        }

        public ActionResult tintuc()
        {
            return View(db.TinTucs.ToList());
        }
       
        [HttpGet]
       
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến 
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (ad)        

                Admin ad = db.Admins.SingleOrDefault(n => n.Username == tendn && n.Password == matkhau);
                if (ad != null)
                {
                    // ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad;
                    Session["Tenadmin"] = collection["username"];

                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiSanpham()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaHang = new SelectList(db.Hangs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSanpham(SanPham laptop, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.Hangs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }

                    laptop.Anhbia = fileName;
                    //Luu vao CSDL
                    db.SanPhams.InsertOnSubmit(laptop);
                    db.SubmitChanges();
                }
                return RedirectToAction("Laptop");
            }
        }
        [HttpGet]
        public ActionResult Themmoihang()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");


            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoihang(Hang hang, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload

            //Kiem tra duong dan file

            //Them vao CSDL

            if (ModelState.IsValid)
            {


                //Luu vao CSDL
                db.Hangs.InsertOnSubmit(hang);
                db.SubmitChanges();
            }
            return RedirectToAction("hangsanxuat");

        }
        [HttpGet]
        public ActionResult Suahang(int id)
        {
            Hang hang = db.Hangs.SingleOrDefault(n => n.MaHang == id);
            ViewBag.MaHang = hang.MaHang;
            if (hang == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(hang);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suahang(Hang hang, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload

            //Kiem tra duong dan file

            //Them vao CSDL

            if (ModelState.IsValid)
            {



                Hang h = db.Hangs.SingleOrDefault(n => n.MaHang == hang.MaHang);
                h.MaHang = hang.MaHang;
                h.TenHang = hang.TenHang;

                //Luu vao CSDL   
                UpdateModel(hang);
                db.SubmitChanges();

            }
            return RedirectToAction("hangsanxuat");

        }
        [HttpGet]
        public ActionResult Xoahang(int id)
        {
            return View();
        }
        [HttpPost, ActionName("Xoahang")]
        public ActionResult Banmuonxoa(int id)
        {

            Hang hang = db.Hangs.SingleOrDefault(n => n.MaHang == id);
            ViewBag.MaHang = hang.MaHang;
            if (hang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Hangs.DeleteOnSubmit(hang);
            db.SubmitChanges();
            return RedirectToAction("hangsanxuat");
        }
        [HttpGet]
        public ActionResult Themmoicdsp()
        {
            ViewBag.MaHang = new SelectList(db.Hangs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");


            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoicdsp(CHUDE chude, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload

            //Kiem tra duong dan file

            //Them vao CSDL

            if (ModelState.IsValid)
            {


                //Luu vao CSDL
                db.CHUDEs.InsertOnSubmit(chude);
                db.SubmitChanges();
            }
            return RedirectToAction("chudesp");

        }
        [HttpGet]
        public ActionResult SuaCD(int id)
        {
            CHUDE chude = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            ViewBag.MaCD = chude.MaCD;
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(chude);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaCD(CHUDE chude, HttpPostedFileBase fileUpload)
        {


            //Them vao CSDL

            if (ModelState.IsValid)
            {


                CHUDE cd = db.CHUDEs.SingleOrDefault(n => n.MaCD == chude.MaCD);
                cd.MaCD = chude.MaCD;
                cd.TenChuDe = chude.TenChuDe;

                //Luu vao CSDL   
                UpdateModel(chude); 
                db.SubmitChanges();

            }
            return RedirectToAction("chudesp");

        }
        [HttpGet]
        public ActionResult XoaCD(int id)
        {
            return View();
        }
        [HttpPost, ActionName("XoaCD")]
        public ActionResult Chacchanxoa(int id)
        {

            CHUDE chude = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            ViewBag.MaCD = chude.MaCD;
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.CHUDEs.DeleteOnSubmit(chude);
            db.SubmitChanges();
            return RedirectToAction("chudesp");
        }
        public ActionResult ChitietSanpham(int id)
        {
            //Lay ra doi tuong sach theo ma
            SanPham laptop = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.Malaptop = laptop.MaSP;
            if (laptop == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(laptop);
        }

        //Xóa sản phẩm
        [HttpGet]
        public ActionResult Xoasanpham(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            SanPham laptop = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.Malaptop = laptop.MaSP;
            if (laptop == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(laptop);
        }

        [HttpPost, ActionName("Xoasanpham")]
        public ActionResult Xacnhanxoa(int id)
        {

            SanPham laptop = db.SanPhams.SingleOrDefault(n => n.MaSP== id);
            ViewBag.Masach = laptop.MaSP;
            if (laptop == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SanPhams.DeleteOnSubmit(laptop);
            db.SubmitChanges();
            return RedirectToAction("Laptop");
        }




        [HttpGet]
        public ActionResult Suasanpham(int id)
        {
            //Lay ra doi tuong sach theo ma
            SanPham laptop = db.SanPhams.SingleOrDefault(n => n.MaSP== id);
            ViewBag.Malaptop = laptop.MaSP;
            if (laptop == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude", laptop.MaCD);
            ViewBag.MaHang = new SelectList(db.Hangs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang", laptop.MaHang);
            return View(laptop);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasanpham(SanPham laptop, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaHang = new SelectList(db.Hangs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {

                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);

                    fileUpload.SaveAs(path);

                    SanPham lp = db.SanPhams.SingleOrDefault(n => n.MaSP == laptop.MaSP);
                    lp.TenSP = laptop.TenSP;
                    lp.MoTa = laptop.MoTa;
                    lp.Anhbia = fileName;
                    lp.GiaCu = laptop.GiaCu;
                    lp.GiaHienTai = laptop.GiaHienTai;

                    UpdateModel(laptop);
                    db.SubmitChanges();

                }
                return RedirectToAction("Laptop");
            }
        }
        
        


        //
        [HttpGet]
        public ActionResult ThemmoiSlide()
        {
            
      
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSlide(Slider slider, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }

                    slider.UrlHinh = fileName;
                    //Luu vao CSDL
                    db.Sliders.InsertOnSubmit(slider);
                    db.SubmitChanges();
                }
                return RedirectToAction("slide");
            }
        }
        //Xóa silde
        [HttpGet]
        public ActionResult Xoaslide(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            Slider slider = db.Sliders.SingleOrDefault(n => n.MaSlider == id);
            ViewBag.MaSlider = slider.MaSlider;
            if (slider == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(slider);
        }

        [HttpPost, ActionName("Xoaslide")]
        public ActionResult Xoaslider(int id)
        {

            Slider slider = db.Sliders.SingleOrDefault(n => n.MaSlider == id);
            ViewBag.MaSlider = slider.MaSlider;
            if (slider == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Sliders.DeleteOnSubmit(slider);
            db.SubmitChanges();
            return RedirectToAction("slide");
        }
        [HttpGet]
        public ActionResult Suaslide(int id)
        {
            //Lay ra doi tuong sach theo ma
            Slider slider = db.Sliders.SingleOrDefault(n => n.MaSlider == id);
            ViewBag.MaSlider = slider.MaSlider;
            if (slider == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua du lieu vao dropdownList
            
            
          
            return View(slider);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suaslide(Slider slider, HttpPostedFileBase fileUpload)
        {
           //Dua du lieu vao dropdownload
           
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {

                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/img/slider/slider-2/"), fileName);

                    fileUpload.SaveAs(path);

                    Slider sl = db.Sliders.SingleOrDefault(n => n.MaSlider == slider.MaSlider);
                    sl.Tenhinh = slider.Tenhinh;
                    sl.LinkUrl = slider.LinkUrl;
                    sl.UrlHinh = fileName;
                    sl.Title = slider.Tenhinh;
                    sl.review = slider.review;
                    

                    UpdateModel(slider );
                    db.SubmitChanges();

                }
                return RedirectToAction("Laptop");
            }
        }
        [HttpGet]
        public ActionResult ThemmoiQuangcao()
        {
            

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiQuangcao(QuangCao quangCao, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
         
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/img"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }

                    quangCao.AnhQC = fileName;
                    //Luu vao CSDL
                    db.QuangCaos.InsertOnSubmit(quangCao);
                    db.SubmitChanges();
                }
                return RedirectToAction("quangcao");
            }
        }




    }
}