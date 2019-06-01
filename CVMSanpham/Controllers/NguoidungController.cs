using CVMSanpham.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVMSanpham.Controllers
{
    public class NguoidungController : Controller
    {
        // GET: Nguoidung
        dbQLstoreDataContext db = new dbQLstoreDataContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KhachHang kh)
        {
            // Gán các giá tị người dùng nhập liệu cho các biến 
            var hoten = collection.Get("HotenKH");
            var tendn = collection.Get("TenDN");
            var matkhau = collection.Get("Matkhau");
            var matkhaunhaplai = collection.Get("Matkhaunhaplai");
            var diachi = collection.Get("Diachi");
            var email = collection.Get("Email");
            var dienthoai = collection.Get("Dienthoai");
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection.Get("Ngaysinh"));
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {

                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }

            if (string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi7"] = "Phải nhập địa chỉ";
            }

            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được bỏ trống";
            }

            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Phải nhập điện thoai";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)
                kh.HoTen = hoten;
                kh.Username = tendn;
                kh.Password = matkhau;
                kh.Email = email;
                kh.DiaChi = diachi;
                kh.DienThoai = dienthoai;
                // kh.Ngaysinh = DateTime.Parse(ngaysinh);
               
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến 
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
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
                //Gán giá trị cho đối tượng được tạo mới (kh)
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.Username == tendn && n.Password == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    Session["Tentaikhoan"] = collection["TenDN"].ToString();
                    return RedirectToAction("Index", "Laptopstore");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["Taikhoan"] = null;
            Session["Tentaikhoan"] = null;
            Session["Giohang"] = null;
            return RedirectToAction("Index", "Laptopstore");
        }

    }
}