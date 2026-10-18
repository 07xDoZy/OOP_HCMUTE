using DormManagement.Enums;

namespace DormManagement.Models;

/// <summary>Bản chuyển đổi Ngày 1 từ HopDong.java; giữ nguyên quy tắc nghiệp vụ.</summary>
public class HopDong
{
    public string MaHD { get; set; }
    public DateOnly NgayBatDau { get; set; }
    public DateOnly NgayKetThuc { get; set; }
    public double TienCoc { get; set; }
    public TrangThaiHopDong TrangThai { get; set; }
    public SinhVien SinhVien { get; set; }
    public Giuong Giuong { get; set; }
    public NhanVien NhanVienLap { get; set; }
    public List<HoaDon> DanhSachHoaDon { get; set; } = new();

    public HopDong(string maHD, DateOnly ngayBatDau, DateOnly ngayKetThuc, double tienCoc, TrangThaiHopDong trangThai, SinhVien sinhVien, Giuong giuong, NhanVien nhanVienLap)
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
    // Chỉ kiểm tra thời gian; không kiểm tra trạng thái hoặc tự chặn khi tạo hợp đồng.
    public bool TrungThoiGian(DateOnly batDau, DateOnly ketThuc)
        => NgayBatDau <= ketThuc && NgayKetThuc >= batDau;

    // Chỉ in trường đơn; không duyệt đồ thị liên kết hai chiều.
    public override string ToString()
        => $"HopDong{{maHD='{MaHD}', ngayBatDau={NgayBatDau:yyyy-MM-dd}, ngayKetThuc={NgayKetThuc:yyyy-MM-dd}, tienCoc={TienCoc}, trangThai={TrangThai}}}";
}
