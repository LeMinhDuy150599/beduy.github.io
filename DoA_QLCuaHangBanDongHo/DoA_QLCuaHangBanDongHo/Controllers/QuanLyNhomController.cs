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
    public class QuanLyNhomController : Controller
    {
        //
        // GET: /QuanLyNhom/

        QuanLyDongHoDataContext dt = new QuanLyDongHoDataContext();
        public ActionResult QuanLyNhom()
        {
            return View(dt.NHOMs.ToList());
        }
        public ActionResult ChiTietNhom(string id)
        {
            int m = int.Parse(id);
            NHOM nhom = dt.NHOMs.SingleOrDefault(n => n.MaNhom == m);
            ViewBag.Masach = nhom.MaNhom;
            if (nhom == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nhom);
        }

        //Xoa
        [HttpGet]
        public ActionResult XoaNhom(string id)
        {
            int m = int.Parse(id);
            NHOM nhom = dt.NHOMs.SingleOrDefault(n => n.MaNhom == m);
            ViewBag.Masach = nhom.MaNhom;
            if (nhom == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nhom);
        }
        [HttpPost, ActionName("XoaNhom")]
        public ActionResult XacNhom(string id)
        {
            int m = int.Parse(id);
            NHOM nhom = dt.NHOMs.SingleOrDefault(n => n.MaNhom == m);
            ViewBag.Masach = nhom.MaNhom;
            if (nhom == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            dt.NHOMs.DeleteOnSubmit(nhom);
            dt.SubmitChanges();
            return RedirectToAction("QuanLyNhom");
        }

        [HttpGet]
        public ActionResult ThemMoiNhom()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiNhom(NHOM nhom)
        {

            dt.NHOMs.InsertOnSubmit(nhom);
            dt.SubmitChanges();
            return RedirectToAction("QuanLyNhom");

        }
        public ActionResult Edit(int id)
        {
            var nhom = dt.NHOMs.First(m => m.MaNhom == id);
            return View(nhom);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var nhom = dt.NHOMs.First(m => m.MaNhom == id);
            var nhom_Edit = collection["tennhom"];

            nhom.MaNhom = id;

            if (string.IsNullOrEmpty(nhom_Edit))
            {
                ViewData["Loi"] = "Tên không được để trống";
            }

            else
            {
                nhom.TenNhom = nhom_Edit;
                UpdateModel(nhom);
                dt.SubmitChanges();
                return RedirectToAction("QuanLyNhom");
            }
            return this.Edit(id);
        }

    }
}
