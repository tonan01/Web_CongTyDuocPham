using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CongTyDuocPham.Models;

namespace Web_CongTyDuocPham.Controllers
{
    public class AdminController : Controller
    {
        Data_CongTyDuocPhamDataContext data = new Data_CongTyDuocPhamDataContext();

        // Trang chủ ------------------------------------------------------------------------------------------------
        public ActionResult HomeAdmin()
        {
            return View();
        }

        // Hiển thị danh sách tất cả dược phẩm ----------------------------------------------------------------------
        public ActionResult ProductsAdmin()
        {
            List<DUOCPHAM> dsDP = data.DUOCPHAMs.ToList();
            return View(dsDP);
        }

        // Hiển thị danh sách tất cả khách hàng ---------------------------------------------------------------------
        public ActionResult Customer()
        {
            List<TAIKHOAN> dskh = data.TAIKHOANs.ToList();
            return View(dskh);
        }

        // Hiển thị danh sách tất cả hóa đơn ------------------------------------------------------------------------
        public ActionResult Bill()
        {
            List<HOADON> dshd = data.HOADONs.ToList();
            return View(dshd);
        }

        // Thống kê -------------------------------------------------------------------------------------------------
        public ActionResult Statistical()
        {
            return View();
        }
    }
}