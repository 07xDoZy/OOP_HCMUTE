using DormManagement.Enums;

namespace DormManagement.Models;

/// <summary>Chuyển từ Java: constructor gắn liên kết; generator kiểm tra trùng lịch.</summary>
public class HopDong
{
    public string MaHD { get; set; }
    private DateTime ngayBatDau;
    public DateTime NgayBatDau
    {
        get => ngayBatDau;
        set => ngayBatDau = value.Date;
    }
    private DateTime ngayKetThuc;
    public DateTime NgayKetThuc
    {
        get => ngayKetThuc;
        set => ngayKetThuc = value.Date;
    }
    public double TienCoc { get; set; }
    public TrangThaiHopDong TrangThai { get; set; }
    public SinhVien SinhVien { get; set; }
    public Giuong Giuong { get; set; }
    public NhanVien NhanVienLap { get; set; }
    public List<HoaDon> DanhSachHoaDon { get; set; } = new();

    public HopDong(string maHD, DateTime ngayBatDau, DateTime ngayKetThuc, double tienCoc, TrangThaiHopDong trangThai, SinhVien sinhVien, Giuong giuong, NhanVien nhanVienLap)
    {
        MaHD = maHD;
        NgayBatDau = ngayBatDau;
        NgayKetThuc = ngayKetThuc;
        TienCoc = tienCoc;
        TrangThai = trangThai;
        SinhVien = sinhVien;
        Giuong = giuong;
        NhanVienLap = nhanVienLap;
        // gán ngược hai chiều
        sinhVien.DanhSachHopDong.Add(this);
        giuong.DanhSachHopDong.Add(this);
        nhanVienLap.DanhSachHopDongLap.Add(this);
    }

    // Hai khoảng đóng: trùng đúng ngày biên vẫn được tính là giao nhau.
    // Hàm chỉ xét ngày; constructor gọi hàm để bảo vệ thao tác tạo mới.
    public bool TrungThoiGian(DateTime batDau, DateTime ketThuc)
    {
        batDau = batDau.Date;
        ketThuc = ketThuc.Date;
        return NgayBatDau <= ketThuc && NgayKetThuc >= batDau;
    }

    // Chỉ in trường đơn; không duyệt đồ thị liên kết hai chiều.
    public override string ToString()
        => $"HopDong{{maHD='{MaHD}', ngayBatDau={NgayBatDau:yyyy-MM-dd}, ngayKetThuc={NgayKetThuc:yyyy-MM-dd}, tienCoc={TienCoc}, trangThai={TrangThai}}}";
}
