using System.Globalization;
using System.Text;
using DormManagement.DAO;
using DormManagement.Data;
using DormManagement.Enums;
namespace DormManagement;

// TSV chuẩn hóa chỉ phục vụ nghiệm thu, không thay đổi dữ liệu model.
internal static class Snapshot
{
    private static string F(object value) => value switch
    {
        DateTime d => d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        double x => x.ToString("0.########", CultureInfo.InvariantCulture),
        _ => Convert.ToString(value, CultureInfo.InvariantCulture) ?? ""
    };
    private static string Escape(string x) => x.Replace("\\", "\\\\").Replace("\t", "\\t").Replace("\r", "\\r").Replace("\n", "\\n");
    private static string Row(params object[] values) => string.Join("\t", values.Select(v => Escape(F(v))));
    private static void Save(string folder, string name, IEnumerable<string> lines)
        => File.WriteAllText(Path.Combine(folder, name + ".tsv"), string.Join("\n", lines) + "\n", new UTF8Encoding(false));
    public static void Write(DormContext db, string folder)
    {
        Directory.CreateDirectory(folder);
        Save(folder, "ToaNha", new[] { Row("MaToa","TenToa","SoTang") }.Concat(db.ToaNhaDAO.FindAll().OrderBy(x => x.MaToa, StringComparer.Ordinal).Select(x => Row(x.MaToa, x.TenToa, x.SoTang))));
        Save(folder, "LoaiPhong", new[] { Row("MaLoaiPhong","TenLoai","SoGiuong","GiaThang") }.Concat(db.LoaiPhongDAO.FindAll().OrderBy(x => x.MaLoaiPhong, StringComparer.Ordinal).Select(x => Row(x.MaLoaiPhong, x.TenLoai, x.SoGiuong, x.GiaThang))));
        Save(folder, "Phong", new[] { Row("MaPhong","SoPhong","Tang","TrangThai","ToaNha_MaToa","LoaiPhong_MaLoaiPhong") }.Concat(db.PhongDAO.FindAll().OrderBy(x => x.MaPhong, StringComparer.Ordinal).Select(x => Row(x.MaPhong, x.SoPhong, x.Tang, x.TrangThai, x.ToaNha!.MaToa, x.LoaiPhong!.MaLoaiPhong))));
        Save(folder, "Giuong", new[] { Row("MaGiuong","SoGiuong","TrangThai","Phong_MaPhong") }.Concat(db.GiuongDAO.FindAll().OrderBy(x => x.MaGiuong, StringComparer.Ordinal).Select(x => Row(x.MaGiuong, x.SoGiuong, x.TrangThai, x.Phong!.MaPhong))));
        Save(folder, "SinhVien", new[] { Row("MaSV","HoTen","NgaySinh","GioiTinh","Sdt","Email","Nganh") }.Concat(db.SinhVienDAO.FindAll().OrderBy(x => x.MaSV, StringComparer.Ordinal).Select(x => Row(x.MaSV, x.HoTen, x.NgaySinh, x.GioiTinh, x.Sdt, x.Email, x.Nganh))));
        Save(folder, "NhanVien", new[] { Row("MaNV","HoTen","Sdt","ChucVu") }.Concat(db.NhanVienDAO.FindAll().OrderBy(x => x.MaNV, StringComparer.Ordinal).Select(x => Row(x.MaNV, x.HoTen, x.Sdt, x.ChucVu))));
        Save(folder, "HopDong", new[] { Row("MaHD","NgayBatDau","NgayKetThuc","TienCoc","TrangThai","SinhVien_MaSV","Giuong_MaGiuong","NhanVienLap_MaNV") }.Concat(db.HopDongDAO.FindAll().OrderBy(x => x.MaHD, StringComparer.Ordinal).Select(x => Row(x.MaHD, x.NgayBatDau, x.NgayKetThuc, x.TienCoc, x.TrangThai, x.SinhVien.MaSV, x.Giuong.MaGiuong, x.NhanVienLap.MaNV))));
        Save(folder, "HoaDon", new[] { Row("MaHoaDon","Thang","Nam","TienPhong","TienDien","TienNuoc","HopDong_MaHD","TongTien","TongDaTra","ConNo") }.Concat(db.HoaDonDAO.FindAll().OrderBy(x => x.MaHoaDon, StringComparer.Ordinal).Select(x => Row(x.MaHoaDon, x.Thang, x.Nam, x.TienPhong, x.TienDien, x.TienNuoc, x.HopDong.MaHD, x.TongTien, x.TongDaTra, x.ConNo))));
        Save(folder, "ThanhToan", new[] { Row("MaThanhToan","NgayThanhToan","SoTien","PhuongThuc","HoaDon_MaHoaDon","NhanVien_MaNV") }.Concat(db.ThanhToanDAO.FindAll().OrderBy(x => x.MaThanhToan, StringComparer.Ordinal).Select(x => Row(x.MaThanhToan, x.NgayThanhToan, x.SoTien, x.PhuongThuc, x.HoaDon.MaHoaDon, x.NhanVien.MaNV))));
        Save(folder, "Summary", Summary(db));
        Save(folder, "Metadata", new[] { Row("Key", "Value"), Row("DatasetVersion", DatasetConfig.Version), Row("ReferenceDate", DatasetConfig.ReferenceDate) });
    }
    public static List<string> Summary(DormContext db)
    {
        var rows = new List<string> { Row("Metric", "Value") };
        rows.Add(Row("ToaNha.Count", db.ToaNhaDAO.FindAll().Count));
        rows.Add(Row("LoaiPhong.Count", db.LoaiPhongDAO.FindAll().Count));
        rows.Add(Row("Phong.Count", db.PhongDAO.FindAll().Count));
        rows.Add(Row("Giuong.Count", db.GiuongDAO.FindAll().Count));
        rows.Add(Row("SinhVien.Count", db.SinhVienDAO.FindAll().Count));
        rows.Add(Row("NhanVien.Count", db.NhanVienDAO.FindAll().Count));
        rows.Add(Row("HopDong.Count", db.HopDongDAO.FindAll().Count));
        rows.Add(Row("HoaDon.Count", db.HoaDonDAO.FindAll().Count));
        rows.Add(Row("ThanhToan.Count", db.ThanhToanDAO.FindAll().Count));
        rows.Add(Row("HoaDon.TongTien", db.HoaDonDAO.FindAll().Sum(x => x.TongTien)));
        rows.Add(Row("HoaDon.TongDaTra", db.HoaDonDAO.FindAll().Sum(x => x.TongDaTra)));
        rows.Add(Row("HoaDon.ConNo", db.HoaDonDAO.FindAll().Sum(x => x.ConNo)));
        rows.Add(Row("HopDong.HIEU_LUC", db.HopDongDAO.FindAll().Count(x => x.TrangThai == TrangThaiHopDong.HIEU_LUC)));
        rows.Add(Row("HopDong.HET_HAN", db.HopDongDAO.FindAll().Count(x => x.TrangThai == TrangThaiHopDong.HET_HAN)));
        rows.Add(Row("HopDong.DA_HUY", db.HopDongDAO.FindAll().Count(x => x.TrangThai == TrangThaiHopDong.DA_HUY)));
        rows.Add(Row("Phong.DAY", db.PhongDAO.FindAll().Count(x => x.TrangThai == TrangThaiPhong.DAY)));
        rows.Add(Row("Phong.CON_CHO", db.PhongDAO.FindAll().Count(x => x.TrangThai == TrangThaiPhong.CON_CHO)));
        rows.Add(Row("Phong.DANG_SUA_CHUA", db.PhongDAO.FindAll().Count(x => x.TrangThai == TrangThaiPhong.DANG_SUA_CHUA)));
        rows.Add(Row("Giuong.TRONG", db.GiuongDAO.FindAll().Count(x => x.TrangThai == TrangThaiGiuong.TRONG)));
        rows.Add(Row("Giuong.DA_CO_NGUOI", db.GiuongDAO.FindAll().Count(x => x.TrangThai == TrangThaiGiuong.DA_CO_NGUOI)));
        rows.Add(Row("SinhVien.NAM", db.SinhVienDAO.FindAll().Count(x => x.GioiTinh == GioiTinh.NAM)));
        rows.Add(Row("SinhVien.NU", db.SinhVienDAO.FindAll().Count(x => x.GioiTinh == GioiTinh.NU)));
        rows.Add(Row("HoaDon.DaTraDu", db.HoaDonDAO.FindAll().Count(x => x.ConNo == 0)));
        rows.Add(Row("HoaDon.TraMotPhan", db.HoaDonDAO.FindAll().Count(x => x.TongDaTra > 0 && x.ConNo > 0)));
        rows.Add(Row("HoaDon.ChuaTra", db.HoaDonDAO.FindAll().Count(x => x.TongDaTra == 0)));
        return rows;
    }
}
