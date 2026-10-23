using DormManagement.DAO;
using DormManagement.Enums;

namespace DormManagement.Queries;

public sealed class LocQueries : QueryBase
{
    public LocQueries(DormContext db) : base(db) { }
    public QueryResult Q01(string toa, int tang)
    {
        if (tang < 1) throw new ArgumentOutOfRangeException(nameof(tang), "Tầng phải lớn hơn 0.");
        return QueryResult.From("L01", $"Phòng tầng {tang}, tòa {toa}",
            Db.PhongDAO.FindAll().Where(p => p.Tang == tang && (Bang(p.ToaNha?.MaToa, toa) || Bang(p.ToaNha?.TenToa, toa)))
            .OrderBy(p => p.MaPhong, StringComparer.Ordinal).Select(p => new { p.MaPhong, p.SoPhong, p.Tang, Toa = p.ToaNha?.TenToa, p.TrangThai }));
    }
    public QueryResult Q02() => QueryResult.From("L02", "Hợp đồng đã hủy",
        Db.HopDongDAO.FindAll().Where(h => h.TrangThai == TrangThaiHopDong.DA_HUY).OrderBy(h => h.MaHD, StringComparer.Ordinal)
        .Select(h => new { h.MaHD, h.SinhVien.MaSV, h.Giuong.MaGiuong, h.NgayBatDau, h.NgayKetThuc, h.TrangThai }));

    public QueryResult Q03()
    {
        // Đếm mọi hợp đồng đã lập, kể cả hủy; không suy diễn số hợp đồng thành số ngày ở.
        var rows = Db.SinhVienDAO.FindAll().Select(s => new { s.MaSV, s.HoTen,
            SoHopDong = Db.HopDongDAO.FindAll().Count(h => h.SinhVien.MaSV == s.MaSV) }).ToList();
        var max = rows.Count == 0 ? 0 : rows.Max(s => s.SoHopDong);
        return QueryResult.From("L03", "Sinh viên có nhiều hợp đồng nhất (kể cả hợp đồng hủy)",
            rows.Where(s => s.SoHopDong == max).OrderBy(s => s.MaSV, StringComparer.Ordinal));
    }
    public QueryResult Q04() => QueryResult.From("L04", "Phòng đang sửa chữa",
        Db.PhongDAO.FindAll().Where(p => p.TrangThai == TrangThaiPhong.DANG_SUA_CHUA).OrderBy(p => p.MaPhong, StringComparer.Ordinal)
        .Select(p => new { p.MaPhong, p.SoPhong, Toa = p.ToaNha?.TenToa, p.TrangThai }));

    public QueryResult Q05(int nam)
    {
        NamHopLe(nam);
        return QueryResult.From("L05", $"So sánh doanh thu đã thu giữa các tòa năm {nam}",
            Db.ToaNhaDAO.FindAll().Select(t => new { t.MaToa, t.TenToa, Nam = nam,
                DaThu = Db.ThanhToanDAO.FindAll().Where(tt => tt.NgayThanhToan.Year == nam && tt.HoaDon.HopDong.Giuong.Phong?.ToaNha?.MaToa == t.MaToa).Sum(tt => tt.SoTien) })
            .OrderByDescending(t => t.DaThu).ThenBy(t => t.MaToa, StringComparer.Ordinal));
    }
    public QueryResult Q06() => QueryResult.From("L06", "Số hợp đồng mỗi nhân viên lập, giảm dần",
        Db.NhanVienDAO.FindAll().Select(n => new { n.MaNV, n.HoTen,
            SoHopDong = Db.HopDongDAO.FindAll().Count(h => h.NhanVienLap.MaNV == n.MaNV) })
        .OrderByDescending(n => n.SoHopDong).ThenBy(n => n.MaNV, StringComparer.Ordinal));

    public QueryResult Q07() => QueryResult.From("L07", "Nhân viên theo từng chức vụ",
        Db.NhanVienDAO.FindAll().GroupBy(n => n.ChucVu).OrderBy(g => g.Key)
        .Select(g => new { ChucVu = g.Key, SoNhanVien = g.Count(),
            DanhSach = string.Join("; ", g.OrderBy(n => n.MaNV, StringComparer.Ordinal).Select(n => $"{n.MaNV} - {n.HoTen}")) }));

    public QueryResult Q08() => QueryResult.From("L08", "Phòng còn đúng một giường có thể xếp mới",
        Db.PhongDAO.FindAll().Where(p => p.DanhSachGiuong.Count(ConTrong) == 1)
        .OrderBy(p => p.MaPhong, StringComparer.Ordinal)
        .Select(p => new { p.MaPhong, Toa = p.ToaNha?.TenToa, MaGiuongTrong = p.DanhSachGiuong.Single(ConTrong).MaGiuong }));

    public QueryResult Q09() => QueryResult.From("L09", "Số sinh viên theo giới tính (toàn bộ danh sách)",
        Enum.GetValues<GioiTinh>().Select(g => new { GioiTinh = g, SoSinhVien = Db.SinhVienDAO.FindAll().Count(s => s.GioiTinh == g) }));

    public QueryResult Q10(DateTime ngay)
    {
        // Khoảng đóng: ở từ ngày a đến ngày a là 1 ngày. Loại hợp đồng hủy và chưa kết thúc.
        var days = Db.HopDongDAO.FindAll().Where(h => h.TrangThai == TrangThaiHopDong.HET_HAN && h.NgayKetThuc < ngay.Date)
            .Select(h => (h.NgayKetThuc - h.NgayBatDau).Days + 1).ToList();
        return QueryResult.From("L10", "Thời gian cư trú trung bình của hợp đồng đã hết hạn (tính cả hai đầu)",
            new[] { new { SoHopDong = days.Count, SoNgayTrungBinh = days.Count == 0 ? (double?)null : days.Average() } });
    }
}
