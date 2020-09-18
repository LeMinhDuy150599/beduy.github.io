using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using DoA_QLCuaHangBanDongHo.Models;
namespace DoA_QLCuaHangBanDongHo.Controllers
{
    public class QuanLyLoaiSPController : Controller
    {
        //
        // GET: /QuanLyLoaiSP/

        QuanLyDongHoDataContext dt = new QuanLyDongHoDataContext();
        public ActionResult QuanLyLoai(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(dt.LOAIs.ToList().OrderBy(n => n.MaLoai).ToPagedList(pageNumber, pageSize));
        }
        //Hien thi san pham
        public ActionResult ChiTietLoai(string id)
        {
            int m = int.Parse(id);
            LOAI loai = dt.LOAIs.SingleOrDefault(n => n.MaLoai == m);
            ViewBag.Masach = loai.MaLoai;
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loai);
        }
        //Xoa
        [HttpGet]
        public ActionResult XoaLoai(string id)
        {
            int m = int.Parse(id);
            LOAI loai = dt.LOAIs.SingleOrDefault(n => n.MaLoai == m);
            ViewBag.Masach = loai.MaLoai;
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loai);
        }
        [HttpPost, ActionName("XoaLoai")]
        public ActionResult XacLoai(string id)
        {
            int m = int.Parse(id);
            LOAI loai = dt.LOAIs.SingleOrDefault(n => n.MaLoai == m);
            ViewBag.Masach = loai.MaLoai;
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            dt.LOAIs.DeleteOnSubmit(loai);
            dt.SubmitChanges();
            return RedirectToAction("QuanLyLoai");
        }

        [HttpGet]
        public ActionResult ThemMoiLoai()
        {
            ViewBag.MaNhom = new SelectList(dt.NHOMs.ToList().OrderBy(n => n.TenNhom), "MaNhom", "TenNhom");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiLoai(LOAI loai)
        {
            //Dua du lieu vao dropdownlist
            ViewBag.MaNhom = new SelectList(dt.NHOMs.ToList().OrderBy(n => n.TenNhom), "MaNhom", "TenNhom");
            dt.LOAIs.InsertOnSubmit(loai);
            dt.SubmitChanges();
            return RedirectToAction("QuanLyLoai");

        }

        [HttpGet]
        public ActionResult ChinhSuaLoai(int id)
        {
            var loai = dt.LOAIs.First(m => m.MaLoai == id);
            ViewBag.MaNhom = new SelectList(dt.NHOMs.ToList().OrderBy(n => n.TenNhom), "MaNhom", "TenNhom", loai.MaNhom);

            return View(loai);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSuaLoai(int id, FormCollection collection)
        {
            ViewBag.MaNhom = new SelectList(dt.NHOMs.ToList().OrderBy(n => n.TenNhom), "MaNhom", "TenNhom");
            var loai = dt.LOAIs.First(m => m.MaLoai == id);
            var loai_Edit = collection["tenloai"];
            //var nhom_Edit = collection["MaNhom"];
            loai.MaNhom = id;

            if (string.IsNullOrEmpty(loai_Edit))
            {
                ViewData["Loi"] = "Tên không được để trống";
            }

            else
            {
                loai.TenLoai = loai_Edit;
                //loai.MaNhom = ViewBag.ma;
                UpdateModel(loai);
                dt.SubmitChanges();
                return RedirectToAction("QuanLyLoai");
            }
            return this.ChinhSuaLoai(id);
        }

    }
}
