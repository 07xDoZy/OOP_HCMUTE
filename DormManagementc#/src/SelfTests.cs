using DormManagement.DAO;
using DormManagement.Data;
using DormManagement.Models;
using DormManagement.Enums;
using System.Reflection;

namespace DormManagement;

internal static class SelfTests
{
    public static int Run()
    {
        int passed = 0;
        void Check(bool ok, string name)
        {
            if (!ok) throw new InvalidOperationException(name);
            Console.WriteLine($"PASS: {name}"); passed++;
        }
        try
        {
            var date = new DateTime(2026, 10, 19);
            var d = new DataGenerator(date);
            Check(d.ToaNhas.Count == 3 && d.LoaiPhongs.Count == 3 && d.Phongs.Count == 10 && d.Giuongs.Count == 52, "Counts: 3 toa, 3 loai, 10 phong, 52 giuong");
            Check(d.SinhViens.Count == 24 && d.NhanViens.Count == 4 && d.HopDongs.Count == 31 && d.HoaDons.Count == 46 && d.ThanhToans.Count == 46, "Counts: 24 SV, 4 NV, 31 HD, 46 hoa don, 46 thanh toan");
            Check(d.HopDongs.Count(h => h.TrangThai == TrangThaiHopDong.HIEU_LUC) == 21 && d.HopDongs.Count(h => h.TrangThai == TrangThaiHopDong.HET_HAN) == 8 && d.HopDongs.Count(h => h.TrangThai == TrangThaiHopDong.DA_HUY) == 2, "21 hieu luc, 8 het han, 2 da huy");
            Check(d.SinhViens[0].DanhSachHopDong.Count == 3 && d.SinhViens[23].DanhSachHopDong.Count == 0, "SV01 co 3 HD; SV24 chua co HD");
            Check(d.SinhViens.Count(s => s.GioiTinh == GioiTinh.NAM) == 12 && d.SinhViens.Select(s => s.Nganh).Distinct().Count() == 6, "12 nam, 12 nu, 6 nganh");
            Check(d.HopDongs.Single(h => h.MaHD == "HD13").NgayKetThuc == date.AddDays(10) && d.HopDongs.Single(h => h.MaHD == "HD20").NgayKetThuc == date.AddDays(20) && d.HopDongs.Single(h => h.MaHD == "HD27").NgayKetThuc == date.AddDays(28), "Ngay het han +10/+20/+28");
            Check(d.Phongs.Count(p => p.TrangThai == TrangThaiPhong.DAY) == 3 && d.Phongs.Count(p => p.TrangThai == TrangThaiPhong.CON_CHO) == 6 && d.Phongs[9].TrangThai == TrangThaiPhong.DANG_SUA_CHUA, "Trang thai phong: 3 day, 6 con cho, 1 sua chua");
            Check(d.Giuongs.Count(g => g.TrangThai == TrangThaiGiuong.DA_CO_NGUOI) == 21 && d.Giuongs.Count(g => g.TrangThai == TrangThaiGiuong.TRONG) == 31, "21 giuong co nguoi, 31 trong");
            Check(d.Giuongs.Count(g => g.TrangThai == TrangThaiGiuong.TRONG && g.Phong!.TrangThai != TrangThaiPhong.DANG_SUA_CHUA) == 23, "Chi 23 giuong trong ngoai phong sua chua");
            Check(d.Phongs.All(p => p.ToaNha!.DanhSachPhong.Count(x => x == p) == 1 && p.LoaiPhong.DanhSachPhong.Count(x => x == p) == 1 && p.DanhSachGiuong.Count == p.LoaiPhong.SoGiuong), "Quan he toa/loai/phong va suc chua");
            Check(d.Giuongs.All(g => g.Phong!.DanhSachGiuong.Count(x => x == g) == 1), "Quan he phong/giuong");
            Check(d.HopDongs.All(h => h.SinhVien.DanhSachHopDong.Count(x => x == h) == 1 && h.Giuong.DanhSachHopDong.Count(x => x == h) == 1 && h.NhanVienLap.DanhSachHopDongLap.Count(x => x == h) == 1), "Ba quan he nguoc cua hop dong");
            Check(d.HoaDons.All(h => h.HopDong.DanhSachHoaDon.Count(x => x == h) == 1) && d.ThanhToans.All(t => t.HoaDon.DanhSachThanhToan.Count(x => x == t) == 1 && t.NhanVien.DanhSachThanhToanGhiNhan.Count(x => x == t) == 1), "Quan he hoa don/thanh toan khong bi them lap");
            var valid = d.HopDongs.Where(h => h.TrangThai != TrangThaiHopDong.DA_HUY).ToArray();
            Check(valid.All(h => valid.All(other => h == other || (h.SinhVien != other.SinhVien && h.Giuong != other.Giuong) || !h.TrungThoiGian(other.NgayBatDau, other.NgayKetThuc))), "Khong trung lich sinh vien/giuong trong du lieu mau");
            Check(d.HoaDons.Count(h => h.TongDaTra == 0) == 10 && d.HoaDons.Count(h => h.TongDaTra > 0 && h.ConNo > 0) == 11 && d.HoaDons.Count(h => h.ConNo == 0) == 25, "10 chua tra, 11 tra mot phan, 25 tra du");
            Check(d.HoaDons.Count(h => h.DanhSachThanhToan.Count == 2) == 10 && d.HoaDons.All(h => h.ConNo >= 0), "10 hoa don tra hai lan, khong tra thua");
            Check(d.HoaDons.Sum(h => h.TongDaTra) == d.ThanhToans.Sum(t => t.SoTien), "Tong da tra khop danh sach thanh toan");
            Check(d.ThanhToans.Select(t => t.PhuongThuc).Distinct().Count() == 3 && d.ThanhToans.All(t => t.NhanVien.MaNV is "NV02" or "NV03"), "3 phuong thuc, NV02/NV03 thu tien");

            // Kiểm thử hợp đồng trùng qua đúng hàm private của generator; không mở API nghiệp vụ mới.
            var create = typeof(DataGenerator).GetMethod("TaoHD", BindingFlags.Instance | BindingFlags.NonPublic)!;
            bool Reject(SinhVien sv, Giuong g, DateTime start, DateTime end, string prefix)
            {
                int count = d.HopDongs.Count;
                try { create.Invoke(d, new object[] {sv, g, start, end, TrangThaiHopDong.HIEU_LUC}); return false; }
                catch (TargetInvocationException ex) when (ex.InnerException is InvalidOperationException e)
                { return e.Message.StartsWith(prefix) && d.HopDongs.Count == count; }
            }
            Check(Reject(d.SinhViens[0], d.Giuongs[40], date, date.AddDays(1), "Trùng thời gian cư trú"), "Chan trung lich sinh vien truoc khi them");
            Check(Reject(d.SinhViens[23], d.Giuongs[0], date, date.AddDays(1), "Trùng thời gian của giường"), "Chan trung lich giuong truoc khi them");
            Check(Reject(d.SinhViens[23], d.Giuongs[0], new(2027,6,30), new(2027,7,1), "Trùng thời gian của giường"), "Ngay bien cua hop dong van bi chan");
            var cancelled = (HopDong)create.Invoke(d, new object[] {d.SinhViens[0], d.Giuongs[0], date, date.AddDays(1), TrangThaiHopDong.DA_HUY})!;
            Check(cancelled.TrangThai == TrangThaiHopDong.DA_HUY, "Hop dong moi da huy bo qua chan trung lich");

            var list = new List<ToaNha>(); IGenericDAO<ToaNha> dao = new ToaNhaDAO(list);
            var view = dao.FindAll(); var a = new ToaNha("T01", "A", 5); dao.Add(a);
            Check(view.Count == 1 && ReferenceEquals(dao.FindById("T01"), a), "FindAll la view song; FindById tra cung tham chieu");
            Check(dao.FindById("missing") is null && dao.FindById("t01") is null, "Khong tim thay tra null; ma phan biet hoa thuong");
            bool readOnly = false;
            try { ((IList<ToaNha>)view).Add(new("X", "X", 1)); } catch (NotSupportedException) { readOnly = true; }
            Check(readOnly, "FindAll chan Add ke ca khi ep sang IList");
            a.TenToa = "Changed"; Check(view[0].TenToa == "Changed", "Read-only view khong dong bang model");
            dao.Add(new ToaNha("T01", "Duplicate", 1));
            Check(ReferenceEquals(dao.FindById("T01"), a) && dao.Delete("T01") && view.Count == 0 && !dao.Delete("T01"), "Ma trung duoc them; tim dau tien; Delete xoa tat ca va tra bool");
            var context = new DormContext(date); var second = new DormContext(date);
            Check(!ReferenceEquals(context.SinhVienDAO.FindById("SV01"), second.SinhVienDAO.FindById("SV01")), "Hai context co hai bo du lieu doc lap");
            var bill = context.HoaDonDAO.FindById("HDN01")!;
            var payment = bill.DanhSachThanhToan[0]; context.ThanhToanDAO.Delete(payment.MaThanhToan);
            Check(bill.DanhSachThanhToan.Contains(payment), "Ghi nhan gioi han Java: Delete DAO chua go quan he nguoc");
            Check(context.ToaNhaDAO.FindById("T01")?.TenToa == "A" &&
                context.LoaiPhongDAO.FindById("LP01")?.SoGiuong == 4 &&
                context.PhongDAO.FindById("P01")?.SoPhong == "101" &&
                context.GiuongDAO.FindById("G01")?.SoGiuong == 1 &&
                context.NhanVienDAO.FindById("NV01")?.ChucVu == ChucVu.QUAN_LY &&
                context.HopDongDAO.FindById("HD01")?.SinhVien.MaSV == "SV01" &&
                context.HoaDonDAO.FindById("HDN01")?.Thang == 5 &&
                context.ThanhToanDAO.FindById("TT02") is not null,
                "Cac DAO lay dung khoa, dac biet GiuongDAO da sua");
            Check(d.HoaDons.Sum(h => h.TongTien) == 34155000 &&
                d.HoaDons.Sum(h => h.TongDaTra) == 22602500 &&
                d.HoaDons.Sum(h => h.ConNo) == 11552500, "Tong tien mau khop moc nghiem thu");
            var fresh = new DataGenerator(date);
            var student = fresh.SinhViens[23];
            var bed = fresh.Giuongs[40];
            var employee = fresh.NhanViens[0];
            var direct = new HopDong("TEST", date.AddHours(14), date.AddDays(2).AddHours(17),
                0, TrangThaiHopDong.HIEU_LUC, student, bed, employee);
            Check(direct.NgayBatDau.TimeOfDay == TimeSpan.Zero && direct.NgayKetThuc.TimeOfDay == TimeSpan.Zero,
                "DateTime duoc chuan hoa ve ngay trong constructor");
            Check(direct.TrungThoiGian(date.AddDays(2).AddHours(23), date.AddDays(3)),
                "So sanh trung lich bo phan gio, bao gom ngay bien");
            int svCount = student.DanhSachHopDong.Count;
            int gCount = bed.DanhSachHopDong.Count;
            int nvCount = employee.DanhSachHopDongLap.Count;
            bool directRejected = false;
            try { _ = new HopDong("BAD", date, date.AddDays(1), 0,
                TrangThaiHopDong.HIEU_LUC, student, bed, employee); }
            catch (InvalidOperationException) { directRejected = true; }
            Check(directRejected && student.DanhSachHopDong.Count == svCount &&
                bed.DanhSachHopDong.Count == gCount && employee.DanhSachHopDongLap.Count == nvCount,
                "Tao truc tiep cung chan trung lich va khong them quan he do dang");
            bool reversed = false;
            try { _ = new HopDong("BADDATE", date.AddDays(2), date, 0,
                TrangThaiHopDong.DA_HUY, student, bed, employee); }
            catch (ArgumentException) { reversed = true; }
            Check(reversed && student.DanhSachHopDong.Count == svCount,
                "Ngay dao nguoc bi tu choi ke ca hop dong da huy");
            student.NgaySinh = new DateTime(2005, 1, 2, 13, 45, 0);
            direct.NgayKetThuc = date.AddDays(2).AddHours(22);
            fresh.ThanhToans[0].NgayThanhToan = date.AddHours(16);
            Check(student.NgaySinh.TimeOfDay == TimeSpan.Zero &&
                direct.NgayKetThuc.TimeOfDay == TimeSpan.Zero &&
                fresh.ThanhToans[0].NgayThanhToan.TimeOfDay == TimeSpan.Zero,
                "Setter ngay sinh/ket thuc/thanh toan bo phan gio");
            Check(new DataGenerator(date.AddHours(23)).HopDongs.Single(h => h.MaHD == "HD13").NgayKetThuc == date.AddDays(10),
                "Ngay tham chieu co gio khong lam lech ngay du lieu");
            Check(new DataGenerator().HopDongs.Single(h => h.MaHD == "HD13").NgayKetThuc == new DateTime(2026,10,29),
                "Mac dinh dung moc dong bang, khong phu thuoc ngay may");
            Console.WriteLine($"\n{passed}/{passed} checks passed.");
            return 0;
        }
        catch (Exception ex) { Console.Error.WriteLine($"FAIL after {passed}: {ex}"); return 1; }
    }
}
