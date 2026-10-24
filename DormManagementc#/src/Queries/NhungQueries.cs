using DormManagement.DAO;
using DormManagement.Models;
using DormManagement.Enums;
namespace DormManagement.Queries;
public static class NhungQueries
{
    public static long SoGiuongTrong(Phong p) => p.DanhSachGiuong.LongCount(g=>g.TrangThai==TrangThaiGiuong.TRONG);
    public static List<Phong> Cau1PhongConGiuongTrong(DormContext ctx) => ctx.PhongDAO.FindAll().Where(p=>SoGiuongTrong(p)>0).ToList();
    public static List<SinhVien> Cau2TimSinhVien(DormContext ctx,string tuKhoa) => ctx.SinhVienDAO.FindAll().Where(s=>s.MaSV.Equals(tuKhoa,StringComparison.OrdinalIgnoreCase)||s.HoTen.Contains(tuKhoa,StringComparison.OrdinalIgnoreCase)).ToList();
    public static Dictionary<string,double> Cau3TongTienPhongTrongThang(DormContext ctx,string maPhong,int thang,int nam) {
        var ds=ctx.HoaDonDAO.FindAll().Where(h=>h.HopDong.Giuong.Phong!.MaPhong==maPhong&&h.Thang==thang&&h.Nam==nam).ToList();
        return new(){["Tien phong"]=ds.Sum(h=>h.TienPhong),["Tien dien"]=ds.Sum(h=>h.TienDien),["Tien nuoc"]=ds.Sum(h=>h.TienNuoc),["Tong"]=ds.Sum(h=>h.TongTien)};
    }
    public static SortedDictionary<string,long> Cau4ThongKeTinhTrangHoaDon(DormContext ctx) => new(ctx.HoaDonDAO.FindAll().GroupBy(h=>h.ConNo<=0.01?"Da tra du":h.TongDaTra>0?"Tra mot phan":"Chua tra").ToDictionary(g=>g.Key,g=>g.LongCount()),StringComparer.Ordinal);
    public static SortedDictionary<int,double> Cau5DoanhThuTheoQuy(DormContext ctx,int nam) => new(ctx.ThanhToanDAO.FindAll().Where(t=>t.NgayThanhToan.Year==nam).GroupBy(t=>(t.NgayThanhToan.Month-1)/3+1).ToDictionary(g=>g.Key,g=>g.Sum(t=>t.SoTien)));
    public static SinhVien? Cau6SinhVienOGiuong(DormContext ctx,string maGiuong) => ctx.HopDongDAO.FindAll().Where(h=>h.TrangThai==TrangThaiHopDong.HIEU_LUC&&h.Giuong.MaGiuong==maGiuong).Select(h=>h.SinhVien).FirstOrDefault();
    public static List<Phong> Cau7PhongConCho(DormContext ctx) => ctx.PhongDAO.FindAll().Where(p=>p.TrangThai==TrangThaiPhong.CON_CHO).ToList();
    public static List<HopDong> Cau8TatCaHopDong(DormContext ctx) => ctx.HopDongDAO.FindAll().OrderBy(h=>h.MaHD,StringComparer.Ordinal).ToList();
    public static List<HoaDon> Cau9HoaDonChuaThanhToan(DormContext ctx) => ctx.HoaDonDAO.FindAll().Where(h=>h.TongDaTra==0).ToList();
    public static List<KeyValuePair<Phong,double>> Cau10Top3PhongTienDien(DormContext ctx) => ctx.HoaDonDAO.FindAll().GroupBy(h=>h.HopDong.Giuong.Phong!).Select(g=>new KeyValuePair<Phong,double>(g.Key,g.Sum(h=>h.TienDien))).OrderByDescending(e=>e.Value).ThenBy(e=>e.Key.MaPhong,StringComparer.Ordinal).Take(3).ToList();
}
