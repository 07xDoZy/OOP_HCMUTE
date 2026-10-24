using DormManagement.DAO;
using DormManagement.Models;
using DormManagement.Enums;
namespace DormManagement.Queries;

public static class KieuQueries
{
    public static List<Giuong> Cau1GiuongTrongToaA(DormContext ctx) => ctx.GiuongDAO.FindAll().Where(g => g.TrangThai == TrangThaiGiuong.TRONG && g.Phong!.ToaNha!.TenToa == "A").ToList();
    public static List<SinhVien> Cau2SinhVienTrongPhong(DormContext ctx, string maPhong) => ctx.HopDongDAO.FindAll().Where(h => h.TrangThai == TrangThaiHopDong.HIEU_LUC && h.Giuong.Phong!.MaPhong == maPhong).Select(h => h.SinhVien).ToList();
    public static List<HopDong> Cau3HopDongSapHetHan(DormContext ctx) {
        var homNay = DateTime.Today; var han = homNay.AddDays(30);
        return ctx.HopDongDAO.FindAll().Where(h => h.TrangThai == TrangThaiHopDong.HIEU_LUC && h.NgayKetThuc >= homNay && h.NgayKetThuc <= han).OrderBy(h => h.NgayKetThuc).ToList();
    }
    public static SortedDictionary<int, double> Cau4DoanhThuTheoThang(DormContext ctx, int nam) => new(ctx.ThanhToanDAO.FindAll().Where(t => t.NgayThanhToan.Year == nam).GroupBy(t => t.NgayThanhToan.Month).ToDictionary(g => g.Key, g => g.Sum(t => t.SoTien)));
    public static List<SinhVien> Cau5SinhVienConNo(DormContext ctx) => ctx.HoaDonDAO.FindAll().Where(h => h.ConNo > 0.01).Select(h => h.HopDong.SinhVien).Distinct().ToList();
    public static double TyLeLapDay(ToaNha toa) { var gs=toa.DanhSachPhong.SelectMany(p => p.DanhSachGiuong).ToList(); return gs.Count == 0 ? 0 : (double)gs.Count(g => g.TrangThai == TrangThaiGiuong.DA_CO_NGUOI)/gs.Count; }
    // Java Stream.max trả một đối tượng; dùng OrderByDescending ổn định, chọn đầu nếu đồng hạng.
    public static ToaNha? Cau6ToaLapDayCaoNhat(DormContext ctx) => ctx.ToaNhaDAO.FindAll().OrderByDescending(TyLeLapDay).FirstOrDefault();
    public static SortedDictionary<string,long> Cau7SoSinhVienTheoNganh(DormContext ctx) => new(ctx.SinhVienDAO.FindAll().GroupBy(s=>s.Nganh).ToDictionary(g=>g.Key,g=>g.LongCount()),StringComparer.Ordinal);
    public static SortedDictionary<string,long> Cau8SoPhongTheoLoai(DormContext ctx) => new(ctx.PhongDAO.FindAll().GroupBy(p=>p.LoaiPhong.TenLoai).ToDictionary(g=>g.Key,g=>g.LongCount()),StringComparer.Ordinal);
    public static List<SinhVien> Cau9SinhVienNhieuGiuong(DormContext ctx) => ctx.SinhVienDAO.FindAll().Where(s=>s.DanhSachHopDong.Where(h=>h.TrangThai!=TrangThaiHopDong.DA_HUY).Select(h=>h.Giuong).Distinct().Count()>1).ToList();
    public static KeyValuePair<NhanVien,double>? Cau10NhanVienThuNhieuNhat(DormContext ctx) => ctx.ThanhToanDAO.FindAll().GroupBy(t=>t.NhanVien).Select(g=>new KeyValuePair<NhanVien,double>(g.Key,g.Sum(t=>t.SoTien))).OrderByDescending(e=>e.Value).Select(e=>(KeyValuePair<NhanVien,double>?)e).FirstOrDefault();
}
