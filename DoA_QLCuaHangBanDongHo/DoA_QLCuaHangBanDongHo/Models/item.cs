using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoA_QLCuaHangBanDongHo.Models;
namespace DoA_QLCuaHangBanDongHo.Models
{
    public class item
    {
        QuanLyDongHoDataContext db = new QuanLyDongHoDataContext();
        public int masanpham { get; set; }
        public string tensanpham { get; set; }
        public string mota { get; set; }
        public string hinhanh { get; set; }
        public int dongia { get; set; }
        public int soluong { get; set; }
        public int thanhtien
        {
            get { return soluong * dongia; }
        }
        public item(int ms)
        {
            
            SANPHAM a = db.SANPHAMs.FirstOrDefault(t => t.MaSP == ms);

            masanpham = ms;
            tensanpham = a.TenSP;
            mota = a.MoTa;
            hinhanh = a.AnhBia;
            dongia = (int)a.GiaBan;
            soluong = 1;
        }
    }
}