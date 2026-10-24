using System.Text;
using DormManagement;
Console.OutputEncoding=Encoding.UTF8;
switch(args.Length==0?"all":args[0].ToLowerInvariant()) {
 case "all": Console.WriteLine("DORM MANAGEMENT SYSTEM - 30 CAU TRUY VAN");MainQuery.Run();MainQuery2.Run();break;
 case "kieu":MainQuery.Run();break;
 case "nhung":QueryRunner.Run("Nhung");break;
 case "loc":QueryRunner.Run("Loc");break;
 case "dao":MainDao.Run();break;
 case "--verify":QueryRunner.Run(verify:true);break;
 default:Console.Error.WriteLine("Dùng: dotnet run -- [all|kieu|nhung|loc|dao|--verify]");Environment.ExitCode=1;break;
}
