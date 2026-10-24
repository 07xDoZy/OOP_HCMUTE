using DormManagement.DAO;
using DormManagement.Models;
using DormManagement.Enums;
namespace DormManagement.Queries;
public static class LocQueries
{
    public static List<Phong> Cau1PhongTheoTang(DormContext ctx,string tenToa,int tang) => ctx.PhongDAO.FindAll().Where(p=>p.ToaNha!.TenToa==tenToa&&p.Tang==tang).ToList();
    public static List<HopDong> Cau2HopDongDaHuy(DormContext ctx) => ctx.HopDongDAO.FindAll().Where(h=>h.TrangThai==TrangThaiHopDong.DA_HUY).ToList();
    public static SinhVien? Cau3SinhVienNhieuHopDongNhat(DormContext ctx) => ctx.SinhVienDAO.FindAll().OrderByDescending(s=>s.DanhSachHopDong.Count).FirstOrDefault();
    public static List<Phong> Cau4PhongDangSuaChua(DormContext ctx) => ctx.PhongDAO.FindAll().Where(p=>p.TrangThai==TrangThaiPhong.DANG_SUA_CHUA).ToList();
    public static SortedDictionary<string,double> Cau5DoanhThuTheoToa(DormContext ctx,int nam) => new(ctx.ThanhToanDAO.FindAll().Where(t=>t.NgayThanhToan.Year==nam).GroupBy(t=>t.HoaDon.HopDong.Giuong.Phong!.ToaNha!.TenToa).ToDictionary(g=>g.Key,g=>g.Sum(t=>t.SoTien)),StringComparer.Ordinal);
    public static List<KeyValuePair<NhanVien,long>> Cau6SoHopDongTheoNhanVien(DormContext ctx) => ctx.HopDongDAO.FindAll().GroupBy(h=>h.NhanVienLap).Select(g=>new KeyValuePair<NhanVien,long>(g.Key,g.LongCount())).OrderByDescending(e=>e.Value).ThenBy(e=>e.Key.MaNV,StringComparer.Ordinal).ToList();
    public static SortedDictionary<ChucVu,List<NhanVien>> Cau7NhanVienTheoChucVu(DormContext ctx) => new(ctx.NhanVienDAO.FindAll().GroupBy(n=>n.ChucVu).ToDictionary(g=>g.Key,g=>g.ToList()));
    public static List<Phong> Cau8PhongConDung1Giuong(DormContext ctx) => ctx.PhongDAO.FindAll().Where(p=>p.DanhSachGiuong.Count(g=>g.TrangThai==TrangThaiGiuong.TRONG)==1).ToList();
    public static SortedDictionary<GioiTinh,long> Cau9SoSinhVienTheoGioiTinh(DormContext ctx) => new(ctx.SinhVienDAO.FindAll().GroupBy(s=>s.GioiTinh).ToDictionary(g=>g.Key,g=>g.LongCount()));
    public static double Cau10CuTruTrungBinh(DormContext ctx) => ctx.HopDongDAO.FindAll().Where(h=>h.TrangThai==TrangThaiHopDong.HET_HAN).Select(h=>(double)(h.NgayKetThuc-h.NgayBatDau).Days).DefaultIfEmpty(0).Average();
}
