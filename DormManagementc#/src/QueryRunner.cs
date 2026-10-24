using System.Collections;
using System.Globalization;
using DormManagement.DAO;
namespace DormManagement;
public static class QueryRunner {
 public static string Canon(object? o) {
  if(o is null) return "null";
  var t=o.GetType();
  foreach(var key in new[]{"MaSV","MaGiuong","MaPhong","MaHD","MaHoaDon","MaNV","MaToa","MaThanhToan","MaLoaiPhong"}) {
   var p=t.GetProperty(key); if(p!=null) return (string)p.GetValue(o)!;
  }
  if(t.IsGenericType && t.GetGenericTypeDefinition()==typeof(KeyValuePair<,>)) return Canon(t.GetProperty("Key")!.GetValue(o))+":"+Canon(t.GetProperty("Value")!.GetValue(o));
  if(o is IDictionary d) {var a=new List<string>();foreach(DictionaryEntry e in d)a.Add(Canon(e.Key)+":"+Canon(e.Value));a.Sort(StringComparer.Ordinal);return "{"+string.Join(";",a)+"}";}
  if(o is IEnumerable seq && o is not string) {var a=new List<string>();foreach(var v in seq)a.Add(Canon(v));return "["+string.Join(";",a)+"]";}
  if(o is double n) return n.ToString("0.########",CultureInfo.InvariantCulture);
  return Convert.ToString(o,CultureInfo.InvariantCulture)??"";
 }
 public static void Run(string group="",bool verify=false) {
  var ctx=new DormContext();
  if(group=="" || group=="Kieu") {var result=Queries.KieuQueries.Cau1GiuongTrongToaA(ctx); Console.WriteLine("K01 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Kieu") {var result=Queries.KieuQueries.Cau2SinhVienTrongPhong(ctx, "P01"); Console.WriteLine("K02 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Kieu") {var result=Queries.KieuQueries.Cau3HopDongSapHetHan(ctx); Console.WriteLine("K03 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Kieu") {var result=Queries.KieuQueries.Cau4DoanhThuTheoThang(ctx, 2026); Console.WriteLine("K04 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Kieu") {var result=Queries.KieuQueries.Cau5SinhVienConNo(ctx); Console.WriteLine("K05 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Kieu") {var result=Queries.KieuQueries.Cau6ToaLapDayCaoNhat(ctx); Console.WriteLine("K06 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Kieu") {var result=Queries.KieuQueries.Cau7SoSinhVienTheoNganh(ctx); Console.WriteLine("K07 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Kieu") {var result=Queries.KieuQueries.Cau8SoPhongTheoLoai(ctx); Console.WriteLine("K08 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Kieu") {var result=Queries.KieuQueries.Cau9SinhVienNhieuGiuong(ctx); Console.WriteLine("K09 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Kieu") {var result=Queries.KieuQueries.Cau10NhanVienThuNhieuNhat(ctx); Console.WriteLine("K10 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Nhung") {var result=Queries.NhungQueries.Cau1PhongConGiuongTrong(ctx); Console.WriteLine("N01 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Nhung") {var result=Queries.NhungQueries.Cau2TimSinhVien(ctx, "SV01"); Console.WriteLine("N02 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Nhung") {var result=Queries.NhungQueries.Cau3TongTienPhongTrongThang(ctx, "P01", 10, 2026); Console.WriteLine("N03 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Nhung") {var result=Queries.NhungQueries.Cau4ThongKeTinhTrangHoaDon(ctx); Console.WriteLine("N04 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Nhung") {var result=Queries.NhungQueries.Cau5DoanhThuTheoQuy(ctx, 2026); Console.WriteLine("N05 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Nhung") {var result=Queries.NhungQueries.Cau6SinhVienOGiuong(ctx, "G01"); Console.WriteLine("N06 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Nhung") {var result=Queries.NhungQueries.Cau7PhongConCho(ctx); Console.WriteLine("N07 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Nhung") {var result=Queries.NhungQueries.Cau8TatCaHopDong(ctx); Console.WriteLine("N08 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Nhung") {var result=Queries.NhungQueries.Cau9HoaDonChuaThanhToan(ctx); Console.WriteLine("N09 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Nhung") {var result=Queries.NhungQueries.Cau10Top3PhongTienDien(ctx); Console.WriteLine("N10 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Loc") {var result=Queries.LocQueries.Cau1PhongTheoTang(ctx, "C", 2); Console.WriteLine("L01 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Loc") {var result=Queries.LocQueries.Cau2HopDongDaHuy(ctx); Console.WriteLine("L02 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Loc") {var result=Queries.LocQueries.Cau3SinhVienNhieuHopDongNhat(ctx); Console.WriteLine("L03 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Loc") {var result=Queries.LocQueries.Cau4PhongDangSuaChua(ctx); Console.WriteLine("L04 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Loc") {var result=Queries.LocQueries.Cau5DoanhThuTheoToa(ctx, 2026); Console.WriteLine("L05 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Loc") {var result=Queries.LocQueries.Cau6SoHopDongTheoNhanVien(ctx); Console.WriteLine("L06 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Loc") {var result=Queries.LocQueries.Cau7NhanVienTheoChucVu(ctx); Console.WriteLine("L07 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Loc") {var result=Queries.LocQueries.Cau8PhongConDung1Giuong(ctx); Console.WriteLine("L08 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Loc") {var result=Queries.LocQueries.Cau9SoSinhVienTheoGioiTinh(ctx); Console.WriteLine("L09 " + Canon(result)); if(!verify) Display(result);}
  if(group=="" || group=="Loc") {var result=Queries.LocQueries.Cau10CuTruTrungBinh(ctx); Console.WriteLine("L10 " + Canon(result)); if(!verify) Display(result);}
 }
 static void Display(object? value) {
  if(value is null){Console.WriteLine("  Không có kết quả.");return;}
  if(value is IDictionary d){foreach(DictionaryEntry e in d)Console.WriteLine($"  {e.Key}: {e.Value}");return;}
  if(value is IEnumerable list && value is not string){foreach(var x in list)Console.WriteLine("  "+x);return;}
  Console.WriteLine("  "+value);
 }
}
