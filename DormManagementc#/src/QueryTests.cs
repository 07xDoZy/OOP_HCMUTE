using DormManagement.DAO;
using DormManagement.Data;
using DormManagement.Enums;
using DormManagement.Models;
using DormManagement.Queries;

namespace DormManagement;

public static class QueryTests
{
    public static int Run()
    {
        int passed = 0, failed = 0;
        void Check(string label, Func<bool> test)
        {
            try { if (!test()) throw new Exception("Kết quả không đúng"); passed++; Console.WriteLine("PASS " + label); }
            catch (Exception e) { failed++; Console.WriteLine("FAIL " + label + ": " + e.Message); }
        }
        bool Throws(Action action)
        {
            try { action(); return false; } catch (ArgumentException) { return true; }
        }
        object? Cell(QueryResult r, int row, string col) => r.Rows[row][Array.IndexOf(r.Columns, col)];
        double Num(QueryResult r, int row, string col) => Convert.ToDouble(Cell(r, row, col));
        double Sum(QueryResult r, string col) => Enumerable.Range(0, r.Rows.Length).Sum(i => Num(r, i, col));
        var db = new DormContext(); var cat = new QueryCatalog(db);
        var k = new KieuQueries(db); var n = new NhungQueries(db); var l = new LocQueries(db);
        var date = DatasetConfig.ReferenceDate;
        Check("30 mã truy vấn duy nhất", () => cat.Entries.Count == 30 && cat.Entries.Select(e => e.Id).Distinct().Count() == 30);
        foreach (var e in cat.Entries)
            Check(e.Id + " chạy và đúng số cột", () => { var r = e.Execute(new()); return r.Id == e.Id && r.Rows.All(row => row.Length == r.Columns.Length); });
        Check("K03 ba hợp đồng sắp hết hạn", () => string.Join(",", k.Q03(date).Rows.Select(r => r[0])) == "HD13,HD20,HD27");
        Check("K04 tổng thu 22602500", () => Sum(k.Q04(2026), "DaThu") == 22602500);
        Check("K05 tổng nợ 11552500", () => Sum(k.Q05(), "TongNo") == 11552500);
        Check("K07 đủ 24 sinh viên", () => Sum(k.Q07(), "SoSinhVien") == 24);
        Check("K08 đủ 10 phòng", () => Sum(k.Q08(), "SoPhong") == 10);
        Check("N01 loại phòng sửa chữa", () => n.Q01().Rows.Length == 6 && !n.Q01().Rows.Any(r => Equals(r[0], "P10")));
        Check("N04 đủ/partial/chưa trả = 25/11/10", () => string.Join(",", n.Q04().Rows.Select(r => r[1])) == "25,11,10");
        Check("N05 tổng phải thu 34155000", () => Sum(n.Q05(2026), "TongPhaiThu") == 34155000);
        Check("N08 đủ 31 hợp đồng", () => n.Q08().Rows.Length == 31);
        Check("N09 đúng 10 hóa đơn", () => n.Q09().Rows.Length == 10);
        Check("L02 hai hợp đồng hủy", () => l.Q02().Rows.Length == 2);
        Check("L04 chỉ P10", () => l.Q04().Rows.Length == 1 && Equals(l.Q04().Rows[0][0], "P10"));
        Check("L05 tổng khớp K04", () => Sum(l.Q05(2026), "DaThu") == Sum(k.Q04(2026), "DaThu"));
        Check("L06 đủ 31 hợp đồng", () => Sum(l.Q06(), "SoHopDong") == 31);
        Check("L09 nam/nữ 12/12", () => l.Q09().Rows.All(r => Convert.ToInt32(r[1]) == 12));
        Check("L10 tám hợp đồng đã kết thúc", () => Num(l.Q10(date), 0, "SoHopDong") == 8);
        Check("N02 mã không phân biệt hoa thường", () => n.Q02(" sv01 ").Rows.Length == 1);
        Check("N02 tìm tên", () => n.Q02("NGUYỄN").Rows.Length == 2);
        Check("Tìm không thấy trả rỗng", () => n.Q02("không-có").Rows.Length == 0 && k.Q02("P99", date).Rows.Length == 0 && n.Q06("G99", date).Rows.Length == 0);
        Check("N03 phòng không tồn tại", () => n.Q03("P99", 10, 2026).Rows.Length == 0);
        Check("N03 kỳ trống trả tổng 0", () => Num(n.Q03("P01", 1, 1900), 0, "TongPhaiThu") == 0);
        Check("Năm không phát sinh vẫn đủ 12 tháng và 4 quý", () => k.Q04(1900).Rows.Length == 12 && Sum(k.Q04(1900), "DaThu") == 0 && n.Q05(1900).Rows.Length == 4);
        Check("L10 không có hợp đồng: null, không lỗi Average", () => Cell(l.Q10(new DateTime(1900, 1, 1)), 0, "SoNgayTrungBinh") is null);
        Check("Từ khóa rỗng bị từ chối", () => Throws(() => n.Q02(" ")));
        Check("Tháng 13 bị từ chối", () => Throws(() => n.Q03("P01", 13, 2026)));
        Check("Năm 0 bị từ chối", () => Throws(() => k.Q04(0)));
        Check("Tầng 0 bị từ chối", () => Throws(() => l.Q01("A", 0)));
        Check("Mã truy vấn sai bị từ chối", () => Throws(() => cat.Run("X01", new())));
        Check("Số ngày âm bị từ chối", () => Throws(() => k.Q03(date, -1)));
        Check("Ngày kết thúc đúng biên 30 được lấy", () => {
            var d = new DormContext(); var h = d.HopDongDAO.FindById("HD13")!; h.NgayKetThuc = date.AddDays(30);
            return new KieuQueries(d).Q03(date).Rows.Any(r => Equals(r[0], "HD13")); });
        Check("Ngày kết thúc ngoài biên 31 bị loại", () => {
            var d = new DormContext(); d.HopDongDAO.FindById("HD13")!.NgayKetThuc = date.AddDays(31);
            return !new KieuQueries(d).Q03(date).Rows.Any(r => Equals(r[0], "HD13")); });
        Check("Hợp đồng hủy không tính đang ở", () => {
            var d = new DormContext(); d.HopDongDAO.FindById("HD09")!.TrangThai = TrangThaiHopDong.DA_HUY;
            return !new KieuQueries(d).Q02("P01", date).Rows.Any(r => Equals(r[0], "SV01")); });
        Check("K10 trả tất cả đồng hạng", () => {
            var d = new DormContext(); foreach (var t in d.ThanhToanDAO.FindAll()) t.SoTien = 0;
            return new KieuQueries(d).Q10().Rows.Length == 4; });
        Check("K06 trả tất cả đồng hạng", () => {
            var d = new DormContext(); foreach (var g in d.GiuongDAO.FindAll()) g.TrangThai = TrangThaiGiuong.TRONG;
            return new KieuQueries(d).Q06().Rows.Length == 3; });
        Check("L03 trả tất cả đồng hạng", () => {
            var d = new DormContext(); foreach (var h in d.HopDongDAO.FindAll().ToArray()) d.HopDongDAO.Delete(h.MaHD);
            return new LocQueries(d).Q03().Rows.Length == 24; });
        Check("L10 hợp đồng cùng ngày là 1 ngày", () => {
            var d = new DormContext(); foreach (var h in d.HopDongDAO.FindAll()) if(h.TrangThai == TrangThaiHopDong.HET_HAN) h.NgayKetThuc = h.NgayBatDau;
            return Num(new LocQueries(d).Q10(date), 0, "SoNgayTrungBinh") == 1; });
        Check("Cả 30 câu chạy khi các DAO rỗng", () => {
            var d = new DormContext();
            foreach(var x in d.ThanhToanDAO.FindAll().ToArray()) d.ThanhToanDAO.Delete(x.MaThanhToan);
            foreach(var x in d.HoaDonDAO.FindAll().ToArray()) d.HoaDonDAO.Delete(x.MaHoaDon);
            foreach(var x in d.HopDongDAO.FindAll().ToArray()) d.HopDongDAO.Delete(x.MaHD);
            foreach(var x in d.GiuongDAO.FindAll().ToArray()) d.GiuongDAO.Delete(x.MaGiuong);
            foreach(var x in d.PhongDAO.FindAll().ToArray()) d.PhongDAO.Delete(x.MaPhong);
            foreach(var x in d.ToaNhaDAO.FindAll().ToArray()) d.ToaNhaDAO.Delete(x.MaToa);
            foreach(var x in d.LoaiPhongDAO.FindAll().ToArray()) d.LoaiPhongDAO.Delete(x.MaLoaiPhong);
            foreach(var x in d.SinhVienDAO.FindAll().ToArray()) d.SinhVienDAO.Delete(x.MaSV);
            foreach(var x in d.NhanVienDAO.FindAll().ToArray()) d.NhanVienDAO.Delete(x.MaNV);
            return new QueryCatalog(d).RunAll(new()).Length == 30; });
        Console.WriteLine($"Query tests: {passed} passed, {failed} failed.");
        return failed == 0 ? 0 : 1;
    }
}
