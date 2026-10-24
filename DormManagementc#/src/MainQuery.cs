using DormManagement.DAO;
using DormManagement.Queries;
namespace DormManagement;
public static class MainQuery {
 public static void Run() {
  QueryRunner.Run("Kieu");
  var ctx=new DormContext();
  foreach(var t in ctx.ToaNhaDAO.FindAll()) Console.WriteLine($"Toa {t.TenToa}: {KieuQueries.TyLeLapDay(t)*100:F1}%");
 }
}
