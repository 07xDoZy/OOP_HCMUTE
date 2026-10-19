using DormManagement.Enums;

namespace DormManagement.Models;

/// <summary>Bản chuyển đổi Ngày 1 từ Phong.java; giữ nguyên quy tắc nghiệp vụ.</summary>
public class Phong
{
    public string MaPhong { get; set; }
    public string SoPhong { get; set; }
    public int Tang { get; set; }
    public TrangThaiPhong TrangThai { get; set; }
    public ToaNha? ToaNha { get; set; }
    public LoaiPhong LoaiPhong { get; set; }
    public List<Giuong> DanhSachGiuong { get; set; } = new();

    public Phong(string maPhong, string soPhong, int tang, TrangThaiPhong trangThai, LoaiPhong loaiPhong)
    {
        MaPhong = maPhong;
        SoPhong = soPhong;
        Tang = tang;
        TrangThai = trangThai;
        LoaiPhong = loaiPhong;
        loaiPhong.DanhSachPhong.Add(this);
    }

    public void ThemGiuong(Giuong g)
    {
        DanhSachGiuong.Add(g);
        g.Phong = this;
    }

    // Chỉ in trường đơn; không duyệt đồ thị liên kết hai chiều.
    public override string ToString()
        => $"Phong{{maPhong='{MaPhong}', soPhong='{SoPhong}', tang={Tang}, trangThai={TrangThai}}}";
}
