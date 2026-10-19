using DormManagement.Enums;

namespace DormManagement.Models;

/// <summary>Bản chuyển đổi Ngày 1 từ SinhVien.java; giữ nguyên quy tắc nghiệp vụ.</summary>
public class SinhVien
{
    public string MaSV { get; set; }
    public string HoTen { get; set; }
    private DateTime ngaySinh;
    public DateTime NgaySinh
    {
        get => ngaySinh;
        set => ngaySinh = value.Date;
    }
    public GioiTinh GioiTinh { get; set; }
    public string Sdt { get; set; }
    public string Email { get; set; }
    public string Nganh { get; set; }
    public List<HopDong> DanhSachHopDong { get; set; } = new();

    public SinhVien(string maSV, string hoTen, DateTime ngaySinh, GioiTinh gioiTinh, string sdt, string email, string nganh)
    {
        MaSV = maSV;
        HoTen = hoTen;
        NgaySinh = ngaySinh;
        GioiTinh = gioiTinh;
        Sdt = sdt;
        Email = email;
        Nganh = nganh;
    }


    // Chỉ in trường đơn; không duyệt đồ thị liên kết hai chiều.
    public override string ToString()
        => $"SinhVien{{maSV='{MaSV}', hoTen='{HoTen}', ngaySinh={NgaySinh:yyyy-MM-dd}, gioiTinh={GioiTinh}, sdt='{Sdt}', email='{Email}', nganh='{Nganh}'}}";
}
