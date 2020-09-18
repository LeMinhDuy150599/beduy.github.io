using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoA_QLCuaHangBanDongHo.Models;
namespace DoA_QLCuaHangBanDongHo.Controllers
{
    public class GioHangController : Controller
    {
        //
        // GET: /GioHang/
        QuanLyDongHoDataContext dh = new QuanLyDongHoDataContext();
        public ActionResult XemGioHang()
        {
            List<item> lst = Session["gh"] as List<item>;
            int tsl = Tongsoluong();
            int ttt = Tongthanhtien();

            ViewBag.sl = tsl;
            ViewBag.tt = ttt;
            return View(lst);
        }
        public List<item> LayGioHang()
        {
            List<item> lstGH = Session["gh"] as List<item>;
            if (lstGH == null)
            {
                lstGH = new List<item>();
                Session["gh"] = lstGH;
            }
            return lstGH;
        }
        public int Tongsoluong()
        {
            List<item> GH = LayGioHang();
            int sumsl = 0;
            if (GH.Count != 0)
            {
                sumsl = GH.Sum(t => t.soluong);
            }
            return sumsl;
        }
        public int Tongthanhtien()
        {
            List<item> GH = LayGioHang();
            int sumtt = 0;
            if (GH.Count != 0)
            {
                sumtt = GH.Sum(t => t.thanhtien);
            }
            return sumtt;
        }
        public ActionResult ChiTietSGH(int mi)
        {
            List<item> lst = Session["gh"] as List<item>;
            item a = lst.FirstOrDefault(t => t.masanpham == mi);
            return View(a);
        }
        public ActionResult Xoa(int mi)
        {
            List<item> lst = Session["gh"] as List<item>;
            item a = lst.FirstOrDefault(t => t.masanpham == mi);
            if (a != null)
            {
                lst.Remove(a);
            }
            return RedirectToAction("XemGioHang");
        }
        public ActionResult ThemGioHang(int ms)
        {
            List<item> GH = LayGioHang();
            item s = GH.FirstOrDefault(t => t.masanpham == ms);
            if (s == null)
            {
                item a = new item(ms); 
                GH.Add(a);
            }
            else
            {
                s.soluong++;
            }
            return RedirectToAction("XemGioHang");
        }
        public ActionResult GioHangpartial()
        {

            ViewBag.sl = Tongsoluong();
            return PartialView();
        }


        public ActionResult DatHang()
        {
            if (Session["luu"] == null)
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }
            KHACHHANG x = Session["luu"] as KHACHHANG;
            return View(x);

        }
        [HttpPost]
        public ActionResult XLDatHang(FormCollection col)
        {
            var ng = col["txtng"];

            List<item> lstGH = Session["gh"] as List<item>;
            if (lstGH == null)
                return RedirectToAction("Index", "Home");

            KHACHHANG x = Session["luu"] as KHACHHANG;
            DONHANG dha = new DONHANG();
            dha.MaKH = x.MaKH;
            var ngayGiao = String.Format("{0:MM/dd/yyyy}", col["txtng"]);
            dha.NgayGiao = DateTime.Parse(ngayGiao);
            dha.NgayDat = DateTime.Now;
            dha.DaThanhToan = null;
            dha.TinhTrangGiaoHang = null;
            dh.DONHANGs.InsertOnSubmit(dha);
            dh.SubmitChanges();
            foreach (var i in lstGH)
            {
                CHITIETDONHANG ct = new CHITIETDONHANG();
                ct.MaDonHang = dha.MaDonHang;
                ct.MaSP = i.masanpham;
                ct.SoLuong = i.soluong;
                ct.DonGia = i.dongia;
                dh.CHITIETDONHANGs.InsertOnSubmit(ct);
            }
            dh.SubmitChanges();
            Session["gh"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }
        
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
        public ActionResult CapNhatGioHang(string mi, FormCollection c)
        {
            List<item> lst = LayGioHang();
            int m = int.Parse(mi);
            item a = lst.FirstOrDefault(t => t.masanpham == m);
            if (a != null)
            {
                a.soluong = int.Parse(c["txtSoLuong"].ToString());
            }
            return RedirectToAction("XemGioHang");
        }

    }
}



