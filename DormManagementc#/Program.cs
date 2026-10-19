using System.Globalization;
using System.Text;
using DormManagement;
using DormManagement.DAO;
using DormManagement.Enums;

Console.OutputEncoding = Encoding.UTF8;
if (args.Length == 1 && args[0] == "--self-test")
{
    Environment.ExitCode = SelfTests.Run();
    return;
}
// Mốc mẫu cố định để chạy lại vẫn ra cùng ngày hết hạn; không phải ngày hệ thống.
DateTime ngay = new DateTime(2026, 10, 19);
if (args.Length > 0)
{
    if (args.Length != 2 || args[0] != "--date" ||
        !DateTime.TryParseExact(args[1], "yyyy-MM-dd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out ngay))
    {
        Console.Error.WriteLine("Cách chạy: dotnet run [-- --date yyyy-MM-dd | -- --self-test]");
        Environment.ExitCode = 1;
        return;
    }
}
try
{
    var db = new DormContext(ngay);
    Console.WriteLine("DORM MANAGEMENT SYSTEM — KIỂM TRA NGÀY 3");
    Console.WriteLine($"Ngày tham chiếu mẫu: {ngay:yyyy-MM-dd}");
    Console.WriteLine("Thực thể       | Số lượng");
    Console.WriteLine($"ToaNha         | {db.ToaNhaDAO.FindAll().Count}");
    Console.WriteLine($"LoaiPhong      | {db.LoaiPhongDAO.FindAll().Count}");
    Console.WriteLine($"Phong          | {db.PhongDAO.FindAll().Count}");
    Console.WriteLine($"Giuong         | {db.GiuongDAO.FindAll().Count}");
    Console.WriteLine($"SinhVien       | {db.SinhVienDAO.FindAll().Count}");
    Console.WriteLine($"NhanVien       | {db.NhanVienDAO.FindAll().Count}");
    Console.WriteLine($"HopDong        | {db.HopDongDAO.FindAll().Count}");
    Console.WriteLine($"HoaDon         | {db.HoaDonDAO.FindAll().Count}");
    Console.WriteLine($"ThanhToan      | {db.ThanhToanDAO.FindAll().Count}");
    var bills = db.HoaDonDAO.FindAll();
    Console.WriteLine($"\nTổng phải thu: {bills.Sum(h => h.TongTien).ToString("0", CultureInfo.InvariantCulture)}");
    Console.WriteLine($"Tổng đã trả: {bills.Sum(h => h.TongDaTra).ToString("0", CultureInfo.InvariantCulture)}");
    Console.WriteLine($"Tổng còn nợ: {bills.Sum(h => h.ConNo).ToString("0", CultureInfo.InvariantCulture)}");
    Console.WriteLine($"Hợp đồng hiệu lực / hết hạn / đã hủy: {db.HopDongDAO.FindAll().Count(h => h.TrangThai == TrangThaiHopDong.HIEU_LUC)} / {db.HopDongDAO.FindAll().Count(h => h.TrangThai == TrangThaiHopDong.HET_HAN)} / {db.HopDongDAO.FindAll().Count(h => h.TrangThai == TrangThaiHopDong.DA_HUY)}");
    var sv = db.SinhVienDAO.FindById("SV01");
    Console.WriteLine($"\nFindById(SV01): {sv?.HoTen}; số hợp đồng: {sv?.DanhSachHopDong.Count}");
    Console.WriteLine("Chạy dotnet run -- --self-test để kiểm tra tự động.");
}
catch (ArgumentException ex)
{
    Console.Error.WriteLine($"Dữ liệu/ngày tham chiếu không hợp lệ: {ex.Message}");
    Environment.ExitCode = 1;
}
catch (InvalidOperationException ex)
{
    Console.Error.WriteLine($"Lỗi nghiệp vụ: {ex.Message}");
    Environment.ExitCode = 1;
}
