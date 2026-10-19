using DormManagement.Enums;

namespace DormManagement.Models;

/// <summary>Bản chuyển đổi Ngày 1 từ ToaNha.java; giữ nguyên quy tắc nghiệp vụ.</summary>
public class ToaNha
{
    public string MaToa { get; set; }
    public string TenToa { get; set; }
    public int SoTang { get; set; }
    public List<Phong> DanhSachPhong { get; set; } = new();

    public ToaNha(string maToa, string tenToa, int soTang)
    {
        MaToa = maToa;
        TenToa = tenToa;
        SoTang = soTang;
    }

    // Gắn cả hai chiều, giống themPhong của Java.
    public void ThemPhong(Phong p)
    {
        DanhSachPhong.Add(p);
        p.ToaNha = this;
    }

    // Chỉ in trường đơn; không duyệt đồ thị liên kết hai chiều.
    public override string ToString()
        => $"ToaNha{{maToa='{MaToa}', soTang={SoTang}, tenToa='{TenToa}'}}";
}
