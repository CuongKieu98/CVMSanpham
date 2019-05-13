using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVMSanpham.Models
{
    public class Giohang
    {
        //Tao doi tuong data chua dữ liệu từ model dbBansach đã tạo. 
        dbQLstoreDataContext data = new dbQLstoreDataContext();
        public int iMaSP { set; get; }
        public string sTenSP { set; get; }
        public string sAnhbia { set; get; }
        public Double dGiaHienTai { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dGiaHienTai; }

        }
        //Khoi tao gio hàng theo Masach duoc truyen vao voi Soluong mac dinh la 1
        public Giohang(int MaSP)
        {
            iMaSP = MaSP;
            SanPham sanpham = data.SanPhams.Single(n => n.MaSP == iMaSP);
            sTenSP = sanpham.TenSP;
            sAnhbia = sanpham.Anhbia;
            dGiaHienTai = double.Parse(sanpham.GiaHienTai.ToString());
            iSoluong = 1;
        }
    }
}