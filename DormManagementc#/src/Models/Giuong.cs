using DormManagement.Enums;

namespace DormManagement.Models;

/// <summary>Bản chuyển đổi Ngày 1 từ Giuong.java; giữ nguyên quy tắc nghiệp vụ.</summary>
public class Giuong
{
    public string MaGiuong { get; set; }
    public int SoGiuong { get; set; }
    public TrangThaiGiuong TrangThai { get; set; }
    public Phong? Phong { get; set; }
    public List<HopDong> DanhSachHopDong { get; set; } = new();

    public Giuong(string maGiuong, int soGiuong, TrangThaiGiuong trangThai)
    {
        MaGiuong = maGiuong;
        SoGiuong = soGiuong;
        TrangThai = trangThai;
    }


    // Chỉ in trường đơn; không duyệt đồ thị liên kết hai chiều.
    public override string ToString()
        => $"Giuong{{maGiuong='{MaGiuong}', soGiuong={SoGiuong}, trangThai={TrangThai}}}";
}
