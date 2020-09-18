using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoA_QLCuaHangBanDongHo.Models;
namespace DoA_QLCuaHangBanDongHo.Controllers
{
    public class KhachHangController : Controller
    {
        //
        // GET: /KhachHang/
        QuanLyDongHoDataContext dh = new QuanLyDongHoDataContext();
        public ActionResult DangKy()
        {
            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XLDangKy(FormCollection c, KHACHHANG kh)
        {
            kh.HoTen = c["txthoten"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", c["txtngaysinh"]);
            kh.NgaySinh = DateTime.Parse(ngaysinh);
            kh.GioiTinh = c["txtgioitinh"];
            kh.DienThoai = c["txtdienthoai"];
            kh.TaiKhoan = c["txttendn"];
            kh.MatKhau = c["txtmatkhau"];
            kh.Email = c["txtemail"];
            kh.DiaChi = c["txtdiachi"];
            
            
            dh.KHACHHANGs.InsertOnSubmit(kh);
            dh.SubmitChanges();
            return RedirectToAction("DangNhap");
        }
        public ActionResult XLDangNhap(FormCollection c)
        {
            
            var a = c["txttendn"];
            var b = c["txtmatkhau"];

            KHACHHANG k = dh.KHACHHANGs.SingleOrDefault(t => t.TaiKhoan == a && t.MatKhau == b);
            if (k != null)
            {
                Session["luu"] = k;
                Session["dn"] = k.HoTen;
                return RedirectToAction("Index", "Home");
            }
           
            return RedirectToAction("DangNhap","KhachHang");

        }
    }
}
