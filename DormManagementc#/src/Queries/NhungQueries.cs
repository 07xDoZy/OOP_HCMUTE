using DormManagement.DAO;
using DormManagement.Enums;

namespace DormManagement.Queries;

public sealed class NhungQueries : QueryBase
{
    public NhungQueries(DormContext db) : base(db) { }
    public QueryResult Q01() => QueryResult.From("N01", "Phòng có ít nhất một giường có thể xếp mới",
        Db.PhongDAO.FindAll().Where(p => p.DanhSachGiuong.Any(ConTrong)).OrderBy(p => p.MaPhong, StringComparer.Ordinal)
        .Select(p => new { p.MaPhong, p.SoPhong, Toa = p.ToaNha?.TenToa, GiuongTrong = p.DanhSachGiuong.Count(ConTrong) }));

    public QueryResult Q02(string tuKhoa)
    {
        if (string.IsNullOrWhiteSpace(tuKhoa)) throw new ArgumentException("Từ khóa không được để trống.");
        tuKhoa = tuKhoa.Trim();
        return QueryResult.From("N02", $"Tìm sinh viên: {tuKhoa}",
            Db.SinhVienDAO.FindAll().Where(s => Bang(s.MaSV, tuKhoa) || s.HoTen.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase))
            .OrderBy(s => s.MaSV, StringComparer.Ordinal)
            .Select(s => new { s.MaSV, s.HoTen, s.NgaySinh, s.GioiTinh, s.Sdt, s.Email, s.Nganh }));
    }

    public QueryResult Q03(string maPhong, int thang, int nam)
    {
        KyHopLe(thang, nam);
        var bills = Db.HoaDonDAO.FindAll().Where(h => Bang(h.HopDong.Giuong.Phong?.MaPhong, maPhong) && h.Thang == thang && h.Nam == nam).ToList();
        return QueryResult.From("N03", "Các khoản phải thu của phòng theo kỳ hóa đơn",
            Db.PhongDAO.FindAll().Where(p => Bang(p.MaPhong, maPhong)).Select(p => new {
                p.MaPhong, Thang = thang, Nam = nam, SoHoaDon = bills.Count,
                TienPhong = bills.Sum(h => h.TienPhong), TienDien = bills.Sum(h => h.TienDien),
                TienNuoc = bills.Sum(h => h.TienNuoc), TongPhaiThu = bills.Sum(h => h.TongTien) }));
    }

    public QueryResult Q04()
    {
        var bills = Db.HoaDonDAO.FindAll();
        return QueryResult.From("N04", "Số hóa đơn theo tình trạng thanh toán", new[] {
            new { TinhTrang = "DA_TRA_DU", SoHoaDon = bills.Count(h => h.ConNo <= 0) },
            new { TinhTrang = "TRA_MOT_PHAN", SoHoaDon = bills.Count(h => h.TongDaTra > 0 && h.ConNo > 0) },
            new { TinhTrang = "CHUA_TRA", SoHoaDon = bills.Count(h => h.TongDaTra <= 0 && h.ConNo > 0) }
        });
    }

    public QueryResult Q05(int nam)
    {
        NamHopLe(nam);
        // Doanh thu lập hóa đơn = tiền phòng + điện + nước. Không phân bổ khoản thu vào dịch vụ.
        var bills = Db.HoaDonDAO.FindAll().Where(h => h.Nam == nam).ToList();
        return QueryResult.From("N05", $"Doanh thu lập hóa đơn theo quý năm {nam}",
            Enumerable.Range(1, 4).Select(q => new { Nam = nam, Quy = q,
                TienPhong = bills.Where(h => (h.Thang - 1) / 3 + 1 == q).Sum(h => h.TienPhong),
                DichVu = bills.Where(h => (h.Thang - 1) / 3 + 1 == q).Sum(h => h.TienDien + h.TienNuoc),
                TongPhaiThu = bills.Where(h => (h.Thang - 1) / 3 + 1 == q).Sum(h => h.TongTien) }));
    }

    public QueryResult Q06(string maGiuong, DateTime ngay) => QueryResult.From("N06", $"Liên hệ sinh viên đang ở giường {maGiuong}",
        Db.HopDongDAO.FindAll().Where(h => Bang(h.Giuong.MaGiuong, maGiuong) && DangO(h, ngay))
        .OrderBy(h => h.SinhVien.MaSV, StringComparer.Ordinal)
        .Select(h => new { h.Giuong.MaGiuong, h.SinhVien.MaSV, h.SinhVien.HoTen, h.SinhVien.Sdt, h.SinhVien.Email }));

    public QueryResult Q07() => QueryResult.From("N07", "Phòng có trạng thái CON_CHO",
        Db.PhongDAO.FindAll().Where(p => p.TrangThai == TrangThaiPhong.CON_CHO).OrderBy(p => p.MaPhong, StringComparer.Ordinal)
        .Select(p => new { p.MaPhong, p.SoPhong, Toa = p.ToaNha?.TenToa, p.TrangThai }));

    public QueryResult Q08() => QueryResult.From("N08", "Toàn bộ hợp đồng đã lập, kể cả đã hủy",
        Db.HopDongDAO.FindAll().OrderBy(h => h.MaHD, StringComparer.Ordinal)
        .Select(h => new { h.MaHD, h.SinhVien.MaSV, h.SinhVien.HoTen, h.Giuong.MaGiuong,
            h.NgayBatDau, h.NgayKetThuc, h.TienCoc, h.TrangThai, MaNVLap = h.NhanVienLap.MaNV }));

    public QueryResult Q09() => QueryResult.From("N09", "Hóa đơn chưa thanh toán (chưa thu đồng nào và còn nợ)",
        Db.HoaDonDAO.FindAll().Where(h => h.TongDaTra <= 0 && h.ConNo > 0).OrderBy(h => h.MaHoaDon, StringComparer.Ordinal)
        .Select(h => new { h.MaHoaDon, h.HopDong.SinhVien.MaSV, h.Thang, h.Nam, h.TongTien, h.TongDaTra, h.ConNo }));

    public QueryResult Q10() => QueryResult.From("N10", "Ba phòng có tổng tiền điện cao nhất (toàn bộ kỳ)",
        Db.PhongDAO.FindAll().Select(p => new { p.MaPhong, Toa = p.ToaNha?.TenToa,
            TongTienDien = Db.HoaDonDAO.FindAll().Where(h => h.HopDong.Giuong.Phong?.MaPhong == p.MaPhong).Sum(h => h.TienDien) })
        .OrderByDescending(p => p.TongTienDien).ThenBy(p => p.MaPhong, StringComparer.Ordinal).Take(3));
}
