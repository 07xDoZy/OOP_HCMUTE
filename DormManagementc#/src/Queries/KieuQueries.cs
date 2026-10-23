using DormManagement.DAO;
using DormManagement.Enums;

namespace DormManagement.Queries;

public sealed class KieuQueries : QueryBase
{
    public KieuQueries(DormContext db) : base(db) { }

    // K01: giường có thể xếp mới, loại phòng đang sửa chữa; nhận mã hoặc tên tòa.
    public QueryResult Q01(string toa) => QueryResult.From("K01", $"Giường trống tại tòa {toa}",
        Db.GiuongDAO.FindAll().Where(g => ConTrong(g) &&
            (Bang(g.Phong?.ToaNha?.MaToa, toa) || Bang(g.Phong?.ToaNha?.TenToa, toa)))
        .OrderBy(g => g.MaGiuong, StringComparer.Ordinal)
        .Select(g => new { g.MaGiuong, MaPhong = g.Phong!.MaPhong, Toa = g.Phong.ToaNha!.TenToa, g.SoGiuong }));

    public QueryResult Q02(string maPhong, DateTime ngay) => QueryResult.From("K02", $"Sinh viên đang ở phòng {maPhong} tại {ngay:yyyy-MM-dd}",
        Db.HopDongDAO.FindAll().Where(h => DangO(h, ngay) && Bang(h.Giuong.Phong?.MaPhong, maPhong))
        .OrderBy(h => h.SinhVien.MaSV, StringComparer.Ordinal)
        .Select(h => new { h.SinhVien.MaSV, h.SinhVien.HoTen, h.Giuong.MaGiuong, h.MaHD }));

    public QueryResult Q03(DateTime ngay, int soNgay = 30)
    {
        if (soNgay < 0 || soNgay > (DateTime.MaxValue.Date - ngay.Date).Days)
            throw new ArgumentOutOfRangeException(nameof(soNgay));
        var den = ngay.Date.AddDays(soNgay);
        return QueryResult.From("K03", $"Hợp đồng hết hạn từ {ngay:yyyy-MM-dd} đến {den:yyyy-MM-dd}",
            Db.HopDongDAO.FindAll().Where(h => DangO(h, ngay) && h.NgayKetThuc <= den)
            .OrderBy(h => h.NgayKetThuc).ThenBy(h => h.MaHD, StringComparer.Ordinal)
            .Select(h => new { h.MaHD, h.SinhVien.MaSV, h.SinhVien.HoTen, h.NgayKetThuc, ConLai = (h.NgayKetThuc - ngay.Date).Days }));
    }

    public QueryResult Q04(int nam)
    {
        NamHopLe(nam);
        // GroupBy theo ngày THU TIỀN, không theo kỳ hóa đơn; đủ 12 tháng kể cả 0.
        var thu = Db.ThanhToanDAO.FindAll().Where(t => t.NgayThanhToan.Year == nam)
            .GroupBy(t => t.NgayThanhToan.Month).ToDictionary(g => g.Key, g => g.Sum(t => t.SoTien));
        return QueryResult.From("K04", $"Doanh thu đã thu từng tháng năm {nam}",
            Enumerable.Range(1, 12).Select(thang => new { Nam = nam, Thang = thang, DaThu = thu.GetValueOrDefault(thang) }));
    }

    public QueryResult Q05() => QueryResult.From("K05", "Sinh viên còn hóa đơn chưa trả đủ",
        Db.HoaDonDAO.FindAll().Where(h => h.ConNo > 0).GroupBy(h => h.HopDong.SinhVien)
        .OrderBy(g => g.Key.MaSV, StringComparer.Ordinal)
        .Select(g => new { g.Key.MaSV, g.Key.HoTen, SoHoaDonConNo = g.Count(), TongNo = g.Sum(h => h.ConNo) }));

    public QueryResult Q06()
    {
        // Mẫu số là toàn bộ giường vật lý, kể cả phòng sửa chữa. Trả mọi đồng hạng.
        var rows = Db.ToaNhaDAO.FindAll().Select(t => new {
            t.MaToa, t.TenToa,
            TongGiuong = t.DanhSachPhong.Sum(p => p.DanhSachGiuong.Count),
            DaO = t.DanhSachPhong.SelectMany(p => p.DanhSachGiuong).Count(g => g.TrangThai == TrangThaiGiuong.DA_CO_NGUOI)
        }).Where(t => t.TongGiuong > 0).Select(t => new { t.MaToa, t.TenToa, t.TongGiuong, t.DaO, TyLePhanTram = 100.0 * t.DaO / t.TongGiuong }).ToList();
        var max = rows.Count == 0 ? 0 : rows.Max(t => t.TyLePhanTram);
        return QueryResult.From("K06", "Tòa có tỷ lệ lấp đầy cao nhất", rows.Where(t => t.TyLePhanTram == max).OrderBy(t => t.MaToa, StringComparer.Ordinal));
    }

    public QueryResult Q07() => QueryResult.From("K07", "Số sinh viên theo ngành (toàn bộ danh sách)",
        Db.SinhVienDAO.FindAll().GroupBy(s => s.Nganh).OrderBy(g => g.Key, StringComparer.Ordinal)
        .Select(g => new { Nganh = g.Key, SoSinhVien = g.Count() }));

    public QueryResult Q08() => QueryResult.From("K08", "Số phòng theo loại phòng",
        Db.LoaiPhongDAO.FindAll().OrderBy(l => l.MaLoaiPhong, StringComparer.Ordinal)
        .Select(l => new { l.MaLoaiPhong, l.TenLoai, SoPhong = Db.PhongDAO.FindAll().Count(p => p.LoaiPhong.MaLoaiPhong == l.MaLoaiPhong) }));

    public QueryResult Q09(DateTime ngay) => QueryResult.From("K09", "Sinh viên từng ở nhiều hơn một giường",
        Db.HopDongDAO.FindAll().Where(h => h.TrangThai != TrangThaiHopDong.DA_HUY && h.NgayBatDau <= ngay.Date)
        .GroupBy(h => h.SinhVien).Select(g => new { g.Key.MaSV, g.Key.HoTen,
            Giuong = g.Select(h => h.Giuong.MaGiuong).Distinct().OrderBy(x => x, StringComparer.Ordinal).ToArray() })
        .Where(x => x.Giuong.Length > 1).OrderBy(x => x.MaSV, StringComparer.Ordinal)
        .Select(x => new { x.MaSV, x.HoTen, SoGiuong = x.Giuong.Length, DanhSachGiuong = string.Join(", ", x.Giuong) }));

    public QueryResult Q10()
    {
        var rows = Db.NhanVienDAO.FindAll().Select(n => new { n.MaNV, n.HoTen,
            TongThu = Db.ThanhToanDAO.FindAll().Where(t => t.NhanVien.MaNV == n.MaNV).Sum(t => t.SoTien) }).ToList();
        var max = rows.Count == 0 ? 0 : rows.Max(n => n.TongThu);
        return QueryResult.From("K10", "Nhân viên ghi nhận tổng thu cao nhất (mọi đồng hạng)",
            rows.Where(n => n.TongThu == max).OrderBy(n => n.MaNV, StringComparer.Ordinal));
    }
}
