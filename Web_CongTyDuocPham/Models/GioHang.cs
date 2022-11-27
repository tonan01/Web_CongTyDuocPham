using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_CongTyDuocPham.Models
{
    public class GioHang
    {
        public List<DanhMucGioHang> lst;

        // Hàm tạo cho giỏ hàng ---------------------------------------------------------------------------
        public GioHang()
        {
            lst = new List<DanhMucGioHang>();
        }

        public GioHang(List<DanhMucGioHang> lstGH)
        {
            if (lstGH == null)
                lstGH = new List<DanhMucGioHang>();
            else
                lst = lstGH;
        }

        // Tính số mặt hàng -------------------------------------------------------------------------------
        public int SoMatHang()
        {
            if (lst == null)
                return 0;
            return lst.Count();
        }

        // Tính tổng số lượng mặt hàng --------------------------------------------------------------------
        public int TongSLHang()
        {
            int iTongSoLuong = 0;

            if (lst != null)
                iTongSoLuong = lst.Sum(n => n.iSoLuong);
            return iTongSoLuong;
        }

        // Tính tổng thành tiền ---------------------------------------------------------------------------
        public double TongThanhTien()
        {
            double iTongTien = 0;

            if (lst != null)
                iTongTien = lst.Sum(n => n.ThanhTien);
            return iTongTien;
        }

        // Thêm sản phẩm ----------------------------------------------------------------------------------
        public int Them(string iMaSp)
        {
            // Kiểm tra sản phẩm có trong ds giỏ hàng chưa
            DanhMucGioHang sp = lst.Find(n => n.iMaSP == iMaSp);

            if (sp == null) // Chưa có
            {
                DanhMucGioHang sach = new DanhMucGioHang(iMaSp); // Tạo mới
                if (sach == null)
                    return -1;
                lst.Add(sach);
            }
            else
                sp.iSoLuong++; // Tăng sl lên 1
            return 1;
        }

        // Sửa sản phẩm -----------------------------------------------------------------------------------
        public int Sua(string MaSp, int SoLuong)
        {
            // Kiểm tra sản phẩm có trong ds giỏ hàng chưa
            DanhMucGioHang sp = lst.Find(n => n.iMaSP == MaSp);

            if (sp != null) // Có sản phẩm
            {
                sp.iSoLuong = SoLuong;
                return 1;
            }
            return -1;
        }

        // Xóa giỏ hàng -----------------------------------------------------------------------------------
        public void XoaGioHang()
        {
            lst.Clear();
        }

        public int Xoa(string iMaSp)
        {
            // Kiểm tra sản phẩm có trong ds giỏ hàng chưa
            DanhMucGioHang sp = lst.Find(n => n.iMaSP == iMaSp);

            if (sp != null) // Có sản phẩm
            {
                lst.Remove(sp);
                return 1;
            }
            return -1; // Không có sản phẩm
        }
    }
}