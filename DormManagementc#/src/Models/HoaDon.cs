using DormManagement.Enums;
using System.Linq;

namespace DormManagement.Models;

/// <summary>Bản chuyển đổi Ngày 1 từ HoaDon.java; giữ nguyên quy tắc nghiệp vụ.</summary>
public class HoaDon
{
    public string MaHoaDon { get; set; }
    public int Thang { get; set; }
    public int Nam { get; set; }
    public double TienPhong { get; set; }
    public double TienDien { get; set; }
    public double TienNuoc { get; set; }
    public HopDong HopDong { get; set; }
    public List<ThanhToan> DanhSachThanhToan { get; set; } = new();

    public HoaDon(string maHoaDon, int thang, int nam, double tienPhong, double tienDien, double tienNuoc, HopDong hopDong)
    {
        MaHoaDon = maHoaDon;
        Thang = thang;
        Nam = nam;
        TienPhong = tienPhong;
        TienDien = tienDien;
        TienNuoc = tienNuoc;
        HopDong = hopDong;
        hopDong.DanhSachHoaDon.Add(this);
    }

    public double TongTien => TienPhong + TienDien + TienNuoc;

    // Java Stream mapToDouble(...).sum() -> C# LINQ Sum(...).
    public double TongDaTra => DanhSachThanhToan.Sum(tt => tt.SoTien);

    // Giữ hành vi Java: có thể âm nếu thanh toán vượt tổng tiền.
    public double ConNo => TongTien - TongDaTra;

    // Chỉ in trường đơn; không duyệt đồ thị liên kết hai chiều.
    public override string ToString()
        => $"HoaDon{{maHoaDon='{MaHoaDon}', thang={Thang}, nam={Nam}, tienPhong={TienPhong}, tienDien={TienDien}, tienNuoc={TienNuoc}}}";
}
