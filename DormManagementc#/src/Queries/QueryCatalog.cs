using DormManagement.DAO;
using DormManagement.Data;

namespace DormManagement.Queries;

public sealed record QueryParameters(string Toa = "A", string Phong = "P01", string Giuong = "G01",
    string TuKhoa = "SV01", int Tang = 1, int Thang = 10, int Nam = 2026);
public sealed record QueryEntry(string Id, string Description, Func<QueryParameters, QueryResult> Execute);

public sealed class QueryCatalog
{
    public IReadOnlyList<QueryEntry> Entries { get; }
    public QueryCatalog(DormContext db)
    {
        var k = new KieuQueries(db); var n = new NhungQueries(db); var l = new LocQueries(db);
        var date = DatasetConfig.ReferenceDate;
        Entries = new List<QueryEntry> {
            new("K01", "Giường trống theo tòa", p => k.Q01(p.Toa)),
            new("K02", "Sinh viên đang ở phòng", p => k.Q02(p.Phong, date)),
            new("K03", "Hợp đồng hết hạn trong 30 ngày", p => k.Q03(date)),
            new("K04", "Doanh thu đã thu từng tháng", p => k.Q04(p.Nam)),
            new("K05", "Sinh viên còn công nợ", p => k.Q05()),
            new("K06", "Tòa có tỷ lệ lấp đầy cao nhất", p => k.Q06()),
            new("K07", "Sinh viên theo ngành", p => k.Q07()),
            new("K08", "Phòng theo loại", p => k.Q08()),
            new("K09", "Sinh viên từng ở nhiều giường", p => k.Q09(date)),
            new("K10", "Nhân viên ghi nhận thu nhiều nhất", p => k.Q10()),
            new("N01", "Phòng có giường trống", p => n.Q01()),
            new("N02", "Tra cứu sinh viên", p => n.Q02(p.TuKhoa)),
            new("N03", "Tiền phòng, điện, nước theo kỳ", p => n.Q03(p.Phong, p.Thang, p.Nam)),
            new("N04", "Hóa đơn theo tình trạng thanh toán", p => n.Q04()),
            new("N05", "Doanh thu lập hóa đơn từng quý", p => n.Q05(p.Nam)),
            new("N06", "Sinh viên đang ở một giường", p => n.Q06(p.Giuong, date)),
            new("N07", "Phòng có trạng thái CON_CHO", p => n.Q07()),
            new("N08", "Toàn bộ hợp đồng", p => n.Q08()),
            new("N09", "Hóa đơn chưa thanh toán", p => n.Q09()),
            new("N10", "Top 3 phòng có tiền điện cao nhất", p => n.Q10()),
            new("L01", "Phòng theo tầng và tòa", p => l.Q01(p.Toa, p.Tang)),
            new("L02", "Hợp đồng đã hủy", p => l.Q02()),
            new("L03", "Sinh viên có nhiều hợp đồng nhất", p => l.Q03()),
            new("L04", "Phòng đang sửa chữa", p => l.Q04()),
            new("L05", "Doanh thu đã thu theo tòa", p => l.Q05(p.Nam)),
            new("L06", "Số hợp đồng nhân viên lập", p => l.Q06()),
            new("L07", "Nhân viên theo chức vụ", p => l.Q07()),
            new("L08", "Phòng chỉ còn một giường trống", p => l.Q08()),
            new("L09", "Sinh viên theo giới tính", p => l.Q09()),
            new("L10", "Số ngày cư trú trung bình", p => l.Q10(date))
        }.AsReadOnly();
    }
    public QueryResult Run(string id, QueryParameters p) =>
        (Entries.FirstOrDefault(e => e.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException("Mã truy vấn hợp lệ: K01–K10, N01–N10, L01–L10.")).Execute(p);
    public QueryResult[] RunAll(QueryParameters p) => Entries.Select(e => e.Execute(p)).ToArray();
}
