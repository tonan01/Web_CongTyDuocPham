using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_CongTyDuocPham.Models
{
    public class DanhMucGioHang
    {
        public string iMaSP { get; set; }
        public string sTenSP { get; set; }
        public string sAnhBia { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public double ThanhTien
        {
            get
            {
                return iSoLuong * dDonGia;
            }
        }

        Data_CongTyDuocPhamDataContext data = new Data_CongTyDuocPhamDataContext();

        // Hàm tạo cho giỏ hàng ---------------------------------------------------------------------------
        public DanhMucGioHang(string maSP)
        {
            DUOCPHAM sp = data.DUOCPHAMs.Single(n => n.ID_DUOCPHAM == int.Parse(maSP));

            if (sp != null) // Tìm được sản phẩm
            {
                iMaSP = maSP;
                sTenSP = sp.TEN_DUOCPHAM;
                sAnhBia = sp.HINHANH;
                dDonGia = double.Parse(sp.GIA.ToString());
                iSoLuong = 1;
            }
        }
    }
}