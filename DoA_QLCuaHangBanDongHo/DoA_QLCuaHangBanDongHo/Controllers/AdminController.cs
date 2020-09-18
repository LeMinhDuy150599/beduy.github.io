using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoA_QLCuaHangBanDongHo.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
namespace DoA_QLCuaHangBanDongHo.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        QuanLyDongHoDataContext ql = new QuanLyDongHoDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DongHo(int?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(ql.SANPHAMs.ToList().OrderBy(n=>n.MaSP).ToPagedList(pageNumber,pageSize));
        }
        [HttpGet]
        
        public ActionResult Themmoidongho()
        {
            ViewBag.MaLoai = new SelectList(ql.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNhom = new SelectList(ql.NHOMs.ToList().OrderBy(n => n.TenNhom), "MaNhom", "TenNhom");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoidongho(SANPHAM sp, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaLoai = new SelectList(ql.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNhom = new SelectList(ql.NHOMs.ToList().OrderBy(n => n.TenNhom), "MaNhom", "TenNhom");
            
            if (fileUpload == null)
            {
                @ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/imgWEB/"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    sp.AnhBia = fileName;
                    ql.SANPHAMs.InsertOnSubmit(sp);
                    ql.SubmitChanges();
                }
                return RedirectToAction("Themmoidongho","Admin");
            }
        }
        public ActionResult Chitietdongho(int id)
        {
            SANPHAM sp = ql.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaDongHo = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpGet]
        public ActionResult Xoadongho(int id)
        {
            SANPHAM sp = ql.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost,ActionName("Xoadongho")]
        public ActionResult Xacnhanxoa(int id)
        {
            SANPHAM sp = ql.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ql.SANPHAMs.DeleteOnSubmit(sp);
            ql.SubmitChanges();
            return RedirectToAction("DongHo","Admin");
        }
        [HttpGet]
        public ActionResult Suadongho(int id)
        {
            var sp = ql.SANPHAMs.First(m => m.MaSP == id);
            ViewBag.MaLoai = new SelectList(ql.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", sp.MaLoai);
            ViewBag.MaNhom = new SelectList(ql.NHOMs.ToList().OrderBy(n => n.TenNhom), "MaNhom", "TenNhom", sp.MaNhom);

            return View(sp);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suadongho(int id, FormCollection collection, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(ql.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNhom = new SelectList(ql.NHOMs.ToList().OrderBy(n => n.TenNhom), "MaNhom", "TenNhom");
            var sp = ql.SANPHAMs.First(m => m.MaSP == id);
            var ten_Edit = collection["tensp"];
            int gia_Edit = int.Parse(collection["giaban"]);
            var mota_Edit = collection["mota"];
            int sl_Edit = int.Parse(collection["soluong"]);

            sp.MaNhom = id;

            if (fileupload == null)
            {
                ViewBag.ThongBao = "Chưa chọn Ảnh bìa";
                return View(sp);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var FileName = Path.GetFileName(fileupload.FileName);
                    var DuongDan = Path.Combine(Server.MapPath("~/Content/imgWEB/"), FileName);
                    if (System.IO.File.Exists(DuongDan))
                        ViewBag.ThongBao = "hinh da ton tai";
                    else
                    {
                        fileupload.SaveAs(DuongDan);
                    }
                    sp.TenSP = ten_Edit;
                    sp.GiaBan = gia_Edit;
                    sp.MoTa = mota_Edit;
                    sp.SoLuongTon = sl_Edit;
                    sp.NgayCapNhat = DateTime.Now;
                    sp.AnhBia = FileName;
                    UpdateModel(sp);
                    ql.SubmitChanges();
                    return RedirectToAction("DongHo");
                }


            }
            return this.Suadongho(id);
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
      
        public ActionResult XLDangNhap(FormCollection c)
        {

            var a = c["txttendn"];
            var b = c["txtmatkhau"];

            QUANLYADMIN k = ql.QUANLYADMINs.SingleOrDefault(t => t.TenDangNhap == a && t.MatKhau == b);
            if (k != null)
            {
                return RedirectToAction("DongHo", "Admin");
            }

            return RedirectToAction("DangNhap", "Admin");

        }
       

        
       
    }
}
