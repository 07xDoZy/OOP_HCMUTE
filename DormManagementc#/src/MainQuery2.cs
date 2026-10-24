using DormManagement.DAO;
using DormManagement.Queries;
namespace DormManagement;
public static class MainQuery2 {
 public static void Run() {
  QueryRunner.Run("Nhung");
  Console.WriteLine("So SV họ Nguyễn: "+NhungQueries.Cau2TimSinhVien(new DormContext(),"Nguyễn").Count);
  QueryRunner.Run("Loc");
 }
}
