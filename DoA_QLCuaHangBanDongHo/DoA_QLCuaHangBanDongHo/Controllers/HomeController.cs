using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using DoA_QLCuaHangBanDongHo.Models;
namespace DoA_QLCuaHangBanDongHo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        QuanLyDongHoDataContext dh = new QuanLyDongHoDataContext();
        public ActionResult Index(int?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            return View(dh.SANPHAMs.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
            //List<SANPHAM> lst = dh.SANPHAMs.ToList();
            //return View(lst);
        }
        public ActionResult ChiTietDongHo(int mh)
        {
            List<SANPHAM> lst = dh.SANPHAMs.ToList();
            SANPHAM a = lst.FirstOrDefault(t => t.MaSP == mh);
            return View(a);
        }
        [HttpPost]
        public ActionResult TimKiem(FormCollection c)
        {
            var tukhoa = c["txtSearch"];
            List<SANPHAM> lst = dh.SANPHAMs.ToList();
            List<SANPHAM> lst2 = lst.Where(t => t.TenSP.Contains(tukhoa) == true).ToList(); 
            return View(lst2);
        }
        public ActionResult HienThiLoai(int maloai)
        {
            List<SANPHAM> lst = dh.SANPHAMs.ToList();
            List<SANPHAM> lst2 = lst.Where(t => t.MaLoai==maloai).ToList();
            return View(lst2);
        }
        public ActionResult Menu1()
        {
            List<LOAI> lst = dh.LOAIs.ToList();
            return PartialView(lst);
        }
        [HttpPost]
        public ActionResult TinTuc(FormCollection c)
        {
            var f = c["txtFirstName"];
            var l = c["txtEMAIL"];

            User i = new User();

            i.HoTen = f;
            i.Email = l;
            return RedirectToAction("News", i);
        }
        public ActionResult News(User sv)
        {
            return View(sv);
        }
        public ActionResult MenuNgang()
        {
            List<LOAI> lst = dh.LOAIs.ToList();
            return PartialView(lst);
        }
        public ActionResult LichSu()
        {
            return View();
        }
    }
}
