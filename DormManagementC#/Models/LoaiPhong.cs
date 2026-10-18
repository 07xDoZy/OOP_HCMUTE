using DormManagement.Enums;

namespace DormManagement.Models;

/// <summary>Bản chuyển đổi Ngày 1 từ LoaiPhong.java; giữ nguyên quy tắc nghiệp vụ.</summary>
public class LoaiPhong
{
    public string MaLoaiPhong { get; set; }
    public string TenLoai { get; set; }
    public int SoGiuong { get; set; }
    public double GiaThang { get; set; }
    public List<Phong> DanhSachPhong { get; set; } = new();

    public LoaiPhong(string maLoaiPhong, string tenLoai, int soGiuong, double giaThang)
    {
        MaLoaiPhong = maLoaiPhong;
        TenLoai = tenLoai;
        SoGiuong = soGiuong;
        GiaThang = giaThang;
    }


    // Chỉ in trường đơn; không duyệt đồ thị liên kết hai chiều.
    public override string ToString()
        => $"LoaiPhong{{giaThang={GiaThang}, soGiuong={SoGiuong}, tenLoai='{TenLoai}', maLoaiPhong='{MaLoaiPhong}'}}";
}
