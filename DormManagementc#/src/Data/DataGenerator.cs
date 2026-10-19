using DormManagement.Enums;
using DormManagement.Models;

namespace DormManagement.Data;

public class DataGenerator {
    public List<ToaNha> ToaNhas { get; } = new();
    public List<LoaiPhong> LoaiPhongs { get; } = new();
    public List<Phong> Phongs { get; } = new();
    public List<Giuong> Giuongs { get; } = new();
    public List<SinhVien> SinhViens { get; } = new();
    public List<NhanVien> NhanViens { get; } = new();
    public List<HopDong> HopDongs { get; } = new();
    public List<HoaDon> HoaDons { get; } = new();
    public List<ThanhToan> ThanhToans { get; } = new();

    // Được gán trong TaoHopDong trước khi TaoHoaDonVaThanhToan sử dụng.
    private HopDong hdCu1 = null!, hdCu2 = null!; // 2 hợp đồng hết hạn có hóa đơn

    private readonly DateTime ngayThamChieu;

    public DataGenerator(DateTime? ngayThamChieu = null) {
        this.ngayThamChieu = (ngayThamChieu ?? DateTime.Today).Date;
        TaoNhanVien();
        TaoToaPhongGiuong();
        TaoSinhVien();
        TaoHopDong();
        TaoHoaDonVaThanhToan();
        CapNhatTrangThai();
    }

    // ===== 1. Nhân viên =====
    private void TaoNhanVien() {
        NhanViens.Add(new NhanVien("NV01", "Trần Quốc Hùng", "0901000001", ChucVu.QUAN_LY));
        NhanViens.Add(new NhanVien("NV02", "Lê Thị Hoa", "0901000002", ChucVu.KE_TOAN));
        NhanViens.Add(new NhanVien("NV03", "Phạm Văn Dũng", "0901000003", ChucVu.LE_TAN));
        NhanViens.Add(new NhanVien("NV04", "Nguyễn Thị Mai", "0901000004", ChucVu.LE_TAN));
    }

    // ===== 2. Tòa, loại phòng, phòng, giường =====
    private void TaoToaPhongGiuong() {
        ToaNha a = new ToaNha("T01", "A", 5);
        ToaNha b = new ToaNha("T02", "B", 5);
        ToaNha c = new ToaNha("T03", "C", 4);
        ToaNhas.Add(a); ToaNhas.Add(b); ToaNhas.Add(c);

        LoaiPhong lp4 = new LoaiPhong("LP01", "Phòng 4 giường", 4, 600000);
        LoaiPhong lp6 = new LoaiPhong("LP02", "Phòng 6 giường", 6, 450000);
        LoaiPhong lp8 = new LoaiPhong("LP03", "Phòng 8 giường", 8, 350000);
        LoaiPhongs.Add(lp4); LoaiPhongs.Add(lp6); LoaiPhongs.Add(lp8);

        // Thứ tự tạo quyết định mã giường:
        // P01 -> G01-04, P02 -> G05-08, P03 -> G09-14, P04 -> G15-18, P05 -> G19-24,
        // P06 -> G25-30, P07 -> G31-34, P08 -> G35-38, P09 -> G39-44, P10 -> G45-52
        TaoPhong("P01", "101", 1, a, lp4);
        TaoPhong("P02", "102", 1, a, lp4);
        TaoPhong("P03", "201", 2, a, lp6);
        TaoPhong("P04", "101", 1, b, lp4);
        TaoPhong("P05", "102", 1, b, lp6);
        TaoPhong("P06", "201", 2, b, lp6);
        TaoPhong("P07", "101", 1, c, lp4);
        TaoPhong("P08", "102", 1, c, lp4);
        TaoPhong("P09", "201", 2, c, lp6);
        TaoPhong("P10", "202", 2, c, lp8);
    }

    private void TaoPhong(string ma, string so, int tang, ToaNha toa, LoaiPhong lp) {
        Phong p = new Phong(ma, so, tang, TrangThaiPhong.CON_CHO, lp);
        toa.ThemPhong(p);
        for (int i = 1; i <= lp.SoGiuong; i++) {
            Giuong g = new Giuong($"G{Giuongs.Count + 1:D2}", i, TrangThaiGiuong.TRONG);
            p.ThemGiuong(g);
            Giuongs.Add(g);
        }
        Phongs.Add(p);
    }

    private Giuong Giuong(int n) { return Giuongs[n - 1]; }   // Giuong(9) = G09

    // ===== 3. Sinh viên (24) =====
    private void TaoSinhVien() {
        string[] nam = {"Nguyễn Văn An", "Trần Minh Khoa", "Lê Hoàng Nam", "Phạm Quốc Bảo",
                "Võ Thanh Tùng", "Đặng Gia Huy", "Bùi Đức Anh", "Hồ Minh Quân",
                "Ngô Văn Long", "Đỗ Tuấn Kiệt", "Dương Hữu Phúc", "Lý Thành Đạt"};
        string[] nu = {"Nguyễn Thị Lan", "Trần Thu Hà", "Lê Ngọc Mai", "Phạm Thanh Thảo",
                "Võ Kim Ngân", "Đặng Bảo Trâm", "Bùi Phương Linh", "Hồ Diệu Hiền",
                "Ngô Thu Trang", "Đỗ Khánh Vy", "Dương Mỹ Duyên", "Lý Hồng Nhung"};
        string[] nganh = {"Công nghệ thông tin", "Cơ khí chế tạo", "Điện - Điện tử",
                "Công nghệ may", "Kế toán", "Xây dựng"};

        for (int i = 0; i < 24; i++) {
            bool laNam = (i % 2 == 0);
            string ten = laNam ? nam[i / 2] : nu[i / 2];
            SinhViens.Add(new SinhVien(
                    $"SV{i + 1:D2}", ten,
                    new DateTime(2004 + i % 3, 1 + i % 12, 1 + (i * 3) % 28),
                    laNam ? GioiTinh.NAM : GioiTinh.NU,
                    "0900000" + $"{i + 1:D3}",
                    $"sv{i + 1:D2}@student.hcmute.edu.vn",
                    nganh[i % nganh.Length]));
        }
    }

    private SinhVien Sv(int n) { return SinhViens[n - 1]; }

    // ===== 4. Hợp đồng (31) =====
    private void TaoHopDong() {
        DateTime homNay = ngayThamChieu;
        DateTime d = new DateTime(2025, 9, 1);
        DateTime c = new DateTime(2026, 6, 30);

        // --- 8 hợp đồng HET_HAN (năm học trước) ---
        TaoHD(Sv(1), Giuong(9), new DateTime(2025, 2, 1), new DateTime(2025, 6, 30), TrangThaiHopDong.HET_HAN);
        hdCu1 = TaoHD(Sv(1), Giuong(10), d, c, TrangThaiHopDong.HET_HAN);   // SV01 có 3 hợp đồng, 3 giường
        hdCu2 = TaoHD(Sv(2), Giuong(11), d, c, TrangThaiHopDong.HET_HAN);
        TaoHD(Sv(3), Giuong(12), d, c, TrangThaiHopDong.HET_HAN);
        TaoHD(Sv(4), Giuong(25), d, c, TrangThaiHopDong.HET_HAN);
        TaoHD(Sv(5), Giuong(26), d, c, TrangThaiHopDong.HET_HAN);
        TaoHD(Sv(6), Giuong(39), d, c, TrangThaiHopDong.HET_HAN);
        TaoHD(Sv(7), Giuong(7), d, c, TrangThaiHopDong.HET_HAN);            // cùng giường với hợp đồng hiện tại

        // --- 21 hợp đồng HIEU_LUC ---
        int[] giuongSV = {1, 2, 3, 4, 5, 6, 7, 15, 16, 17, 18, 19, 20, 21, 31, 32, 33, 34, 35, 36, 37};
        for (int i = 0; i < giuongSV.Length; i++) {
            DateTime ketThuc = new DateTime(2027, 6, 30);
            if (i == 4)  ketThuc = homNay.AddDays(10);   // SV05 sắp hết hạn
            if (i == 11) ketThuc = homNay.AddDays(20);   // SV12 sắp hết hạn
            if (i == 18) ketThuc = homNay.AddDays(28);   // SV19 sắp hết hạn
            TaoHD(Sv(i + 1), Giuong(giuongSV[i]), new DateTime(2026, 9, 1), ketThuc, TrangThaiHopDong.HIEU_LUC);
        }

        // --- 2 hợp đồng DA_HUY ---
        TaoHD(Sv(22), Giuong(9), new DateTime(2026, 9, 5), new DateTime(2027, 6, 30), TrangThaiHopDong.DA_HUY);
        TaoHD(Sv(23), Giuong(10), new DateTime(2026, 9, 5), new DateTime(2027, 6, 30), TrangThaiHopDong.DA_HUY);
        // SV24: chưa có hợp đồng nào
    }

    private HopDong TaoHD(SinhVien sv, Giuong g, DateTime bd, DateTime kt, TrangThaiHopDong tt) {
        // Constructor HopDong kiểm tra ngày và trùng lịch trước khi gắn quan hệ.
        int[] nvLap = {0, 2, 3};   // NV01, NV03, NV04 lập hợp đồng
        NhanVien nv = NhanViens[nvLap[HopDongs.Count % 3]];
        HopDong hd = new HopDong($"HD{HopDongs.Count + 1:D2}",
                bd, kt, 1000000, tt, sv, g, nv);
        HopDongs.Add(hd);
        if (tt == TrangThaiHopDong.HIEU_LUC) g.TrangThai = TrangThaiGiuong.DA_CO_NGUOI;
        return hd;
    }

    // ===== 5. Hóa đơn + thanh toán =====
    private void TaoHoaDonVaThanhToan() {
        // 4 hóa đơn của 2 hợp đồng hết hạn: trả đủ
        foreach (HopDong hd in new HopDong[]{hdCu1, hdCu2}) {
            for (int thang = 5; thang <= 6; thang++) {
                TaoThanhToan(TaoHoaDon(hd, thang, 2026), 1);
            }
        }
        // 42 hóa đơn (tháng 9, 10/2026) của 21 hợp đồng hiệu lực
        int k = 0;
        foreach (HopDong hd in HopDongs) {
            if (hd.TrangThai != TrangThaiHopDong.HIEU_LUC) continue;
            for (int thang = 9; thang <= 10; thang++) {
                k++;
                // k%4: 0 chưa trả | 1 trả đủ | 2 trả một phần | 3 trả đủ bằng 2 lần
                TaoThanhToan(TaoHoaDon(hd, thang, 2026), k % 4);
            }
        }
    }

    private HoaDon TaoHoaDon(HopDong hd, int thang, int nam) {
        int idx = HoaDons.Count + 1;
        double tienPhong = (hd.Giuong.Phong ?? throw new InvalidOperationException("Giường chưa được gắn vào phòng.")).LoaiPhong.GiaThang;
        double tienDien = 80000 + (idx * 7 % 10) * 10000;
        double tienNuoc = 40000 + (idx * 3 % 5) * 5000;
        HoaDon h = new HoaDon($"HDN{idx:D2}", thang, nam, tienPhong, tienDien, tienNuoc, hd);
        HoaDons.Add(h);
        return h;
    }

    private void TaoThanhToan(HoaDon h, int kieu) {
        DateTime ngay = new DateTime(h.Nam, h.Thang, 5);
        double tong = h.TongTien;
        switch (kieu) {
            case 1: ThemTT(h, ngay, tong); break;
            case 2: ThemTT(h, ngay, tong / 2); break;
            case 3: {
                // Java Math.round làm tròn .5 lên; Math.Round C# mặc định chọn số chẵn.
                double lan1 = Math.Floor(tong * 0.6 / 1000 + 0.5) * 1000.0;
                ThemTT(h, ngay, lan1);
                ThemTT(h, ngay.AddDays(10), tong - lan1);
                break;
            }
            default: break;   // chưa trả
        }
    }

    private void ThemTT(HoaDon h, DateTime ngay, double soTien) {
        NhanVien nv = NhanViens[1 + ThanhToans.Count % 2];   // NV02 hoặc NV03 thu tiền
        PhuongThucThanhToan pt = Enum.GetValues<PhuongThucThanhToan>()[ThanhToans.Count % 3];
        ThanhToans.Add(new ThanhToan($"TT{ThanhToans.Count + 1:D2}", ngay, soTien, pt, h, nv));
    }

    // ===== 6. Cập nhật trạng thái phòng =====
    private void CapNhatTrangThai() {
        foreach (Phong p in Phongs) {
            int trong = p.DanhSachGiuong.Count(g => g.TrangThai == TrangThaiGiuong.TRONG);
            p.TrangThai = trong == 0 ? TrangThaiPhong.DAY : TrangThaiPhong.CON_CHO;
        }
        Phongs[9].TrangThai = TrangThaiPhong.DANG_SUA_CHUA;   // P10 đang sửa chữa
    }
}