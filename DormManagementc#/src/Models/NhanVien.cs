using DormManagement.Enums;

namespace DormManagement.Models;

/// <summary>Bản chuyển đổi Ngày 1 từ NhanVien.java; giữ nguyên quy tắc nghiệp vụ.</summary>
public class NhanVien
{
    public string MaNV { get; set; }
    public string HoTen { get; set; }
    public string Sdt { get; set; }
    public ChucVu ChucVu { get; set; }
    public List<HopDong> DanhSachHopDongLap { get; set; } = new();
    public List<ThanhToan> DanhSachThanhToanGhiNhan { get; set; } = new();

    public NhanVien(string maNV, string hoTen, string sdt, ChucVu chucVu)
    {
        MaNV = maNV;
        HoTen = hoTen;
        Sdt = sdt;
        ChucVu = chucVu;
    }


    // Chỉ in trường đơn; không duyệt đồ thị liên kết hai chiều.
    public override string ToString()
        => $"NhanVien{{maNV='{MaNV}', hoTen='{HoTen}', sdt='{Sdt}', chucVu={ChucVu}}}";
}
