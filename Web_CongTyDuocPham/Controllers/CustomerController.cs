using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CongTyDuocPham.Models;

namespace Web_CongTyDuocPham.Controllers
{
    public class CustomerController : Controller
    {
        Data_CongTyDuocPhamDataContext data = new Data_CongTyDuocPhamDataContext();

        // Hiển thị danh sách 12 dược phẩm ---------------------------------------------------------------------------------------
        public ActionResult Home()
        {
            List<DUOCPHAM> dsSP = data.DUOCPHAMs.Take(12).ToList();
            return View(dsSP);
        }

        // Hiển thị trang giới thiệu ---------------------------------------------------------------------------------------------
        public ActionResult About()
        {
            return View();
        }

        // Lỗi -------------------------------------------------------------------------------------------------------------------
        public ActionResult Error()
        {
            return View();
        }

        //// Hiển thị loại sản phẩm ------------------------------------------------------------------------------------------------
        //public ActionResult HTLoai()
        //{
        //    List<DANHMUC> dsLoai = data.DANHMUCs.ToList(); // Truyền sang view
        //    return PartialView(dsLoai);
        //}

        //// Hiển thị sản phẩm theo loại -------------------------------------------------------------------------------------------
        //public ActionResult HTSPTheoLoai(string maL)
        //{
        //    List<DANHMUC> dsSP = data.DANHMUCs.Where(t => t.ID_DANHMUC == int.Parse(maL)).ToList();

        //    // Trả về cùng 1 view với view Home
        //    return View("Products", dsSP);
        //}

        // Hiển thị danh sách tất cả dược phẩm -----------------------------------------------------------------------------------
        public ActionResult Products()
        {
            List<DUOCPHAM> dsDP = data.DUOCPHAMs.ToList();
            return View(dsDP);
        }

        // Đăng nhập -------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        // Xử lý đăng nhập 
        [HttpPost]
        public ActionResult XuLyDangNhap(FormCollection col)
        {
            string ten = col["txtTenDN"];
            string mk = col["txtPassword"];

            TAIKHOAN kh = data.TAIKHOANs.SingleOrDefault(k => k.ID_USER == ten && k.PASS == mk);
            if (kh == null) // Đăng nhập không thành công, hiển thị thông báo
                return RedirectToAction("Error", "Customer");

            Session["KhachHang"] = kh; // Kiểu đối tượng khách hàng
            return RedirectToAction("Home", "Customer");
        }

        // Đăng xuất -------------------------------------------------------------------------------------------------------------
        public ActionResult DangXuat()
        {
            Session["KhachHang"] = null;
            Session["gio"] = null;
            return RedirectToAction("Home", "Customer");
        }

        // Đăng ký ---------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        // Xử lý đăng ký
        [HttpPost]
        public ActionResult XLDangKy(TAIKHOAN kh)
        {
            data.TAIKHOANs.InsertOnSubmit(kh);
            data.SubmitChanges();
            return RedirectToAction("DangNhap", "Customer");
        }

        // Trả về chi tiết 1 dược phẩm theo mã danh mục -------------------------------------------------------------------------
        public ActionResult ChiTietSP(int id)
        {
            // Trả về 1 sản phẩm
            DUOCPHAM sp = data.DUOCPHAMs.FirstOrDefault(t => t.ID_DUOCPHAM == id);

            // Lấy ds dược phẩm cùng mã loại với dược phẩm có mã id
            List<DUOCPHAM> dsDPCCD = data.DUOCPHAMs.Where(i => i.ID_DANHMUC == sp.ID_DANHMUC).ToList();

            // Truyền qua view bag
            ViewBag.dsSCCD = dsDPCCD;

            return View(sp);
        }

        // Tìm kiếm -------------------------------------------------------------------------------------------------------------
        public ActionResult TimKiem()
        {
            ViewBag.maLoai = new SelectList(data.DANHMUCs.ToList(), "ID_DANHMUC", "TEN_DANHMUC");
            return View();
        }

        // Xử lý tìm kiếm
        [HttpPost]
        public ActionResult XuLyTimKiem(FormCollection fct)
        {
            string tuKhoa = fct["txtTuKhoa"].ToString();
            string maLoai = fct["maLoai"].ToString();

            List<DUOCPHAM> dsS = data.DUOCPHAMs.Where(t => t.TEN_DUOCPHAM.Contains(tuKhoa) 
                == true && t.ID_DANHMUC == int.Parse(maLoai)).ToList();

            return View("Products", dsS);
        }

        // Button chọn mua --------------------------------------------------------------------------------
        public ActionResult ChonMua(string id)
        {
            GioHang gh = Session["gio"] as GioHang;
            if (gh == null)
                gh = new GioHang();

            // Thêm mặt hàng vào giỏ
            gh.Them(id);

            Session["gio"] = gh;

            return RedirectToAction("Products", "Customer");
        }

        // Hiển thị giỏ hàng ------------------------------------------------------------------------------
        public ActionResult HTGioHang()
        {
            GioHang gh = Session["gio"] as GioHang;

            return View(gh);
        }

        // View xác nhận thông tin ------------------------------------------------------------------------
        public ActionResult XacNhanThongTin()
        {
            TAIKHOAN khach = Session["KhachHang"] as TAIKHOAN;

            if (khach == null) // Chưa đăng nhập
                return RedirectToAction("DangNhap", "Customer");

            // Đã tồn tại khách hàng (Đăng nhập thành công)
            return View(khach);
        }

        // Lưu thông tin đặt hàng -------------------------------------------------------------------------
        [HttpPost]
        public ActionResult LuuDatHang(FormCollection col)
        {
            try
            {
                GioHang gh = (GioHang)Session["gio"];
                KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
                if (Session["KhachHang"] == null)
                    return RedirectToAction("DangNhap", "Customer");
                if (Session["gio"] == null || gh.lst.Count == 0) // Giỏ hàng rỗng
                    return RedirectToAction("HTGioHang", "Customer");

                // Lấy thông tin ngày giao
                string ngayGiao = col["txtNgay"];
                DateTime ngayGiao_dateTime = Convert.ToDateTime(ngayGiao);

                // Lưu vào bảng đặt hàng
                HOADON hd = new HOADON();
                hd.ID_KHACHHANG = kh.ID_KHACHHANG;
                hd.NGAYLAP = DateTime.Now;
                hd.NGAYGIAO = ngayGiao_dateTime;
                data.HOADONs.InsertOnSubmit(hd);
                data.SubmitChanges();

                // Lưu thông tin vào bản chi tiết đặt hàng
                foreach (DanhMucGioHang sp in gh.lst)
                {
                    CT_HOADON cthd = new CT_HOADON();
                    cthd.ID_HOADON = hd.ID_HOADON;
                    cthd.ID_DUOCPHAM = int.Parse(sp.iMaSP);
                    cthd.SOLUONG = sp.iSoLuong;
                    data.CT_HOADONs.InsertOnSubmit(cthd);
                }
                data.SubmitChanges();
                gh.XoaGioHang();
                ViewBag.tb = "Đặt hàng thành công!";
            }
            catch
            {
                ViewBag.tb = "Bạn chưa lưu được đơn hàng!";
            }
            return View();
        }

        // Lấy giỏ hàng 
        public List<DanhMucGioHang> layGioHang()
        {
            List<DanhMucGioHang> lstGH = Session["gio"] as List<DanhMucGioHang>;
            if (lstGH == null)
            {
                // Nếu lstGH chưa tồn tại thì khởi tạo
                lstGH = new List<DanhMucGioHang>();
                Session["gio"] = lstGH;
            }
            return lstGH;
        }

        // Xóa giỏ hàng -----------------------------------------------------------------------------------
        public ActionResult XoaGioHang(string maSP)
        {
            // Lấy giỏ hàng
            List<DanhMucGioHang> lstGH = layGioHang();
            // Kiểm tra xem sách cần xóa có trong giỏ hàng không?
            DanhMucGioHang sp = lstGH.Single(s => s.iMaSP == maSP);

            // Nếu có thì tiến hành xóa
            if (sp != null)
            {
                lstGH.RemoveAll(s => s.iMaSP == maSP);
                return RedirectToAction("gio", "gio");
            }
            // Nếu giỏ hàng rỗng
            if (lstGH.Count == 0)
                return RedirectToAction("Home", "Customer");
            return RedirectToAction("gio", "gio");
        }
    }
}