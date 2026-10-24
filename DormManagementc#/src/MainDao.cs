using DormManagement.DAO;
using DormManagement.Models;
using DormManagement.Enums;
namespace DormManagement;
public static class MainDao {
 public static void Run(){var ctx=new DormContext();
 Console.WriteLine("So sinh vien: "+ctx.SinhVienDAO.FindAll().Count);
 Console.WriteLine("So hop dong: "+ctx.HopDongDAO.FindAll().Count);
 Console.WriteLine(ctx.SinhVienDAO.FindById("SV01"));
 Console.WriteLine(ctx.PhongDAO.FindById("P99") is not null);
 int truoc=ctx.NhanVienDAO.FindAll().Count;
 ctx.NhanVienDAO.Add(new NhanVien("NV99","Test","000",ChucVu.LE_TAN));
 Console.WriteLine($"Sau add: {ctx.NhanVienDAO.FindAll().Count} (truoc: {truoc})");
 Console.WriteLine("Delete NV99: "+ctx.NhanVienDAO.Delete("NV99"));
 Console.WriteLine("Sau delete: "+ctx.NhanVienDAO.FindAll().Count);
 }
}
