using DormManagement.DAO;
using DormManagement.Enums;
using DormManagement.Models;

namespace DormManagement.Queries;

public abstract class QueryBase
{
    protected readonly DormContext Db;
    protected QueryBase(DormContext db) => Db = db ?? throw new ArgumentNullException(nameof(db));
    protected static bool DangO(HopDong h, DateTime ngay) => h.TrangThai == TrangThaiHopDong.HIEU_LUC &&
        h.NgayBatDau <= ngay.Date && h.NgayKetThuc >= ngay.Date;
    protected static bool ConTrong(Giuong g) => g.TrangThai == TrangThaiGiuong.TRONG &&
        g.Phong is { TrangThai: not TrangThaiPhong.DANG_SUA_CHUA };
    protected static bool Bang(string? a, string b) => string.Equals(a, b.Trim(), StringComparison.OrdinalIgnoreCase);
    protected static void NamHopLe(int nam)
    {
        if (nam < 1 || nam > 9999) throw new ArgumentOutOfRangeException(nameof(nam), "Năm từ 1 đến 9999.");
    }
    protected static void KyHopLe(int thang, int nam)
    {
        NamHopLe(nam);
        if (thang < 1 || thang > 12) throw new ArgumentOutOfRangeException(nameof(thang), "Tháng từ 1 đến 12.");
    }
}
