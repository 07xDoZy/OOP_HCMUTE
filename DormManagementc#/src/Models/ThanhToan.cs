using DormManagement.Enums;

namespace DormManagement.Models;

/// <summary>Bản chuyển đổi Ngày 1 từ ThanhToan.java; giữ nguyên quy tắc nghiệp vụ.</summary>
public class ThanhToan
{
    public string MaThanhToan { get; set; }
    private DateTime ngayThanhToan;
    public DateTime NgayThanhToan
    {
        get => ngayThanhToan;
        set => ngayThanhToan = value.Date;
    }
    public double SoTien { get; set; }
    public PhuongThucThanhToan PhuongThuc { get; set; }
    public HoaDon HoaDon { get; set; }
    public NhanVien NhanVien { get; set; }

    public ThanhToan(string maThanhToan, DateTime ngayThanhToan, double soTien, PhuongThucThanhToan phuongThuc, HoaDon hoaDon, NhanVien nhanVien)
    {
        MaThanhToan = maThanhToan;
        NgayThanhToan = ngayThanhToan;
        SoTien = soTien;
        PhuongThuc = phuongThuc;
        HoaDon = hoaDon;
        NhanVien = nhanVien;
        hoaDon.DanhSachThanhToan.Add(this);
        nhanVien.DanhSachThanhToanGhiNhan.Add(this);
    }


    // Chỉ in trường đơn; không duyệt đồ thị liên kết hai chiều.
    public override string ToString()
        => $"ThanhToan{{maThanhToan='{MaThanhToan}', ngayThanhToan={NgayThanhToan:yyyy-MM-dd}, soTien={SoTien}, phuongThuc={PhuongThuc}}}";
}
