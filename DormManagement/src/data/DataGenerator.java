package data;

import enums.*;
import model.*;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

public class DataGenerator {
    public final List<ToaNha> toaNhas = new ArrayList<>();
    public final List<LoaiPhong> loaiPhongs = new ArrayList<>();
    public final List<Phong> phongs = new ArrayList<>();
    public final List<Giuong> giuongs = new ArrayList<>();
    public final List<SinhVien> sinhViens = new ArrayList<>();
    public final List<NhanVien> nhanViens = new ArrayList<>();
    public final List<HopDong> hopDongs = new ArrayList<>();
    public final List<HoaDon> hoaDons = new ArrayList<>();
    public final List<ThanhToan> thanhToans = new ArrayList<>();

    private HopDong hdCu1, hdCu2; // 2 hợp đồng hết hạn có hóa đơn

    public DataGenerator() {
        taoNhanVien();
        taoToaPhongGiuong();
        taoSinhVien();
        taoHopDong();
        taoHoaDonVaThanhToan();
        capNhatTrangThai();
    }

    // ===== 1. Nhân viên =====
    private void taoNhanVien() {
        nhanViens.add(new NhanVien("NV01", "Trần Quốc Hùng", "0901000001", ChucVu.QUAN_LY));
        nhanViens.add(new NhanVien("NV02", "Lê Thị Hoa", "0901000002", ChucVu.KE_TOAN));
        nhanViens.add(new NhanVien("NV03", "Phạm Văn Dũng", "0901000003", ChucVu.LE_TAN));
        nhanViens.add(new NhanVien("NV04", "Nguyễn Thị Mai", "0901000004", ChucVu.LE_TAN));
    }

    // ===== 2. Tòa, loại phòng, phòng, giường =====
    private void taoToaPhongGiuong() {
        ToaNha a = new ToaNha("T01", "A", 5);
        ToaNha b = new ToaNha("T02", "B", 5);
        ToaNha c = new ToaNha("T03", "C", 4);
        toaNhas.add(a); toaNhas.add(b); toaNhas.add(c);

        LoaiPhong lp4 = new LoaiPhong("LP01", "Phòng 4 giường", 4, 600000);
        LoaiPhong lp6 = new LoaiPhong("LP02", "Phòng 6 giường", 6, 450000);
        LoaiPhong lp8 = new LoaiPhong("LP03", "Phòng 8 giường", 8, 350000);
        loaiPhongs.add(lp4); loaiPhongs.add(lp6); loaiPhongs.add(lp8);

        // Thứ tự tạo quyết định mã giường:
        // P01 -> G01-04, P02 -> G05-08, P03 -> G09-14, P04 -> G15-18, P05 -> G19-24,
        // P06 -> G25-30, P07 -> G31-34, P08 -> G35-38, P09 -> G39-44, P10 -> G45-52
        taoPhong("P01", "101", 1, a, lp4);
        taoPhong("P02", "102", 1, a, lp4);
        taoPhong("P03", "201", 2, a, lp6);
        taoPhong("P04", "101", 1, b, lp4);
        taoPhong("P05", "102", 1, b, lp6);
        taoPhong("P06", "201", 2, b, lp6);
        taoPhong("P07", "101", 1, c, lp4);
        taoPhong("P08", "102", 1, c, lp4);
        taoPhong("P09", "201", 2, c, lp6);
        taoPhong("P10", "202", 2, c, lp8);
    }

    private void taoPhong(String ma, String so, int tang, ToaNha toa, LoaiPhong lp) {
        Phong p = new Phong(ma, so, tang, TrangThaiPhong.CON_CHO, lp);
        toa.themPhong(p);
        for (int i = 1; i <= lp.getSoGiuong(); i++) {
            Giuong g = new Giuong(String.format("G%02d", giuongs.size() + 1), i, TrangThaiGiuong.TRONG);
            p.themGiuong(g);
            giuongs.add(g);
        }
        phongs.add(p);
    }

    private Giuong giuong(int n) { return giuongs.get(n - 1); }   // giuong(9) = G09

    // ===== 3. Sinh viên (24) =====
    private void taoSinhVien() {
        String[] nam = {"Nguyễn Văn An", "Trần Minh Khoa", "Lê Hoàng Nam", "Phạm Quốc Bảo",
                "Võ Thanh Tùng", "Đặng Gia Huy", "Bùi Đức Anh", "Hồ Minh Quân",
                "Ngô Văn Long", "Đỗ Tuấn Kiệt", "Dương Hữu Phúc", "Lý Thành Đạt"};
        String[] nu = {"Nguyễn Thị Lan", "Trần Thu Hà", "Lê Ngọc Mai", "Phạm Thanh Thảo",
                "Võ Kim Ngân", "Đặng Bảo Trâm", "Bùi Phương Linh", "Hồ Diệu Hiền",
                "Ngô Thu Trang", "Đỗ Khánh Vy", "Dương Mỹ Duyên", "Lý Hồng Nhung"};
        String[] nganh = {"Công nghệ thông tin", "Cơ khí chế tạo", "Điện - Điện tử",
                "Công nghệ may", "Kế toán", "Xây dựng"};

        for (int i = 0; i < 24; i++) {
            boolean laNam = (i % 2 == 0);
            String ten = laNam ? nam[i / 2] : nu[i / 2];
            sinhViens.add(new SinhVien(
                    String.format("SV%02d", i + 1), ten,
                    LocalDate.of(2004 + i % 3, 1 + i % 12, 1 + (i * 3) % 28),
                    laNam ? GioiTinh.NAM : GioiTinh.NU,
                    "0900000" + String.format("%03d", i + 1),
                    String.format("sv%02d@student.hcmute.edu.vn", i + 1),
                    nganh[i % nganh.length]));
        }
    }

    private SinhVien sv(int n) { return sinhViens.get(n - 1); }

    // ===== 4. Hợp đồng (31) =====
    private void taoHopDong() {
        LocalDate homNay = LocalDate.now();
        LocalDate d = LocalDate.of(2025, 9, 1);
        LocalDate c = LocalDate.of(2026, 6, 30);

        // --- 8 hợp đồng HET_HAN (năm học trước) ---
        taoHD(sv(1), giuong(9), LocalDate.of(2025, 2, 1), LocalDate.of(2025, 6, 30), TrangThaiHopDong.HET_HAN);
        hdCu1 = taoHD(sv(1), giuong(10), d, c, TrangThaiHopDong.HET_HAN);   // SV01 có 3 hợp đồng, 3 giường
        hdCu2 = taoHD(sv(2), giuong(11), d, c, TrangThaiHopDong.HET_HAN);
        taoHD(sv(3), giuong(12), d, c, TrangThaiHopDong.HET_HAN);
        taoHD(sv(4), giuong(25), d, c, TrangThaiHopDong.HET_HAN);
        taoHD(sv(5), giuong(26), d, c, TrangThaiHopDong.HET_HAN);
        taoHD(sv(6), giuong(39), d, c, TrangThaiHopDong.HET_HAN);
        taoHD(sv(7), giuong(7), d, c, TrangThaiHopDong.HET_HAN);            // cùng giường với hợp đồng hiện tại

        // --- 21 hợp đồng HIEU_LUC ---
        int[] giuongSV = {1, 2, 3, 4, 5, 6, 7, 15, 16, 17, 18, 19, 20, 21, 31, 32, 33, 34, 35, 36, 37};
        for (int i = 0; i < giuongSV.length; i++) {
            LocalDate ketThuc = LocalDate.of(2027, 6, 30);
            if (i == 4)  ketThuc = homNay.plusDays(10);   // SV05 sắp hết hạn
            if (i == 11) ketThuc = homNay.plusDays(20);   // SV12 sắp hết hạn
            if (i == 18) ketThuc = homNay.plusDays(28);   // SV19 sắp hết hạn
            taoHD(sv(i + 1), giuong(giuongSV[i]), LocalDate.of(2026, 9, 1), ketThuc, TrangThaiHopDong.HIEU_LUC);
        }

        // --- 2 hợp đồng DA_HUY ---
        taoHD(sv(22), giuong(9), LocalDate.of(2026, 9, 5), LocalDate.of(2027, 6, 30), TrangThaiHopDong.DA_HUY);
        taoHD(sv(23), giuong(10), LocalDate.of(2026, 9, 5), LocalDate.of(2027, 6, 30), TrangThaiHopDong.DA_HUY);
        // SV24: chưa có hợp đồng nào
    }

    private HopDong taoHD(SinhVien sv, Giuong g, LocalDate bd, LocalDate kt, TrangThaiHopDong tt) {
        // Quy tắc nghiệp vụ: không trùng thời gian cư trú (hợp đồng đã hủy không tính)
        if (tt != TrangThaiHopDong.DA_HUY) {
            for (HopDong h : sv.getDanhSachHopDong()) {
                if (h.getTrangThai() != TrangThaiHopDong.DA_HUY && h.trungThoiGian(bd, kt))
                    throw new IllegalStateException("Trùng thời gian cư trú của sinh viên " + sv.getMaSV());
            }
            for (HopDong h : g.getDanhSachHopDong()) {
                if (h.getTrangThai() != TrangThaiHopDong.DA_HUY && h.trungThoiGian(bd, kt))
                    throw new IllegalStateException("Trùng thời gian của giường " + g.getMaGiuong());
            }
        }
        int[] nvLap = {0, 2, 3};   // NV01, NV03, NV04 lập hợp đồng
        NhanVien nv = nhanViens.get(nvLap[hopDongs.size() % 3]);
        HopDong hd = new HopDong(String.format("HD%02d", hopDongs.size() + 1),
                bd, kt, 1000000, tt, sv, g, nv);
        hopDongs.add(hd);
        if (tt == TrangThaiHopDong.HIEU_LUC) g.setTrangThai(TrangThaiGiuong.DA_CO_NGUOI);
        return hd;
    }

    // ===== 5. Hóa đơn + thanh toán =====
    private void taoHoaDonVaThanhToan() {
        // 4 hóa đơn của 2 hợp đồng hết hạn: trả đủ
        for (HopDong hd : new HopDong[]{hdCu1, hdCu2}) {
            for (int thang = 5; thang <= 6; thang++) {
                taoThanhToan(taoHoaDon(hd, thang, 2026), 1);
            }
        }
        // 42 hóa đơn (tháng 9, 10/2026) của 21 hợp đồng hiệu lực
        int k = 0;
        for (HopDong hd : hopDongs) {
            if (hd.getTrangThai() != TrangThaiHopDong.HIEU_LUC) continue;
            for (int thang = 9; thang <= 10; thang++) {
                k++;
                // k%4: 0 chưa trả | 1 trả đủ | 2 trả một phần | 3 trả đủ bằng 2 lần
                taoThanhToan(taoHoaDon(hd, thang, 2026), k % 4);
            }
        }
    }

    private HoaDon taoHoaDon(HopDong hd, int thang, int nam) {
        int idx = hoaDons.size() + 1;
        double tienPhong = hd.getGiuong().getPhong().getLoaiPhong().getGiaThang();
        double tienDien = 80000 + (idx * 7 % 10) * 10000;
        double tienNuoc = 40000 + (idx * 3 % 5) * 5000;
        HoaDon h = new HoaDon(String.format("HDN%02d", idx), thang, nam, tienPhong, tienDien, tienNuoc, hd);
        hoaDons.add(h);
        return h;
    }

    private void taoThanhToan(HoaDon h, int kieu) {
        LocalDate ngay = LocalDate.of(h.getNam(), h.getThang(), 5);
        double tong = h.getTongTien();
        switch (kieu) {
            case 1 -> themTT(h, ngay, tong);
            case 2 -> themTT(h, ngay, tong / 2);
            case 3 -> {
                double lan1 = Math.round(tong * 0.6 / 1000) * 1000.0;
                themTT(h, ngay, lan1);
                themTT(h, ngay.plusDays(10), tong - lan1);
            }
            default -> { }   // chưa trả
        }
    }

    private void themTT(HoaDon h, LocalDate ngay, double soTien) {
        NhanVien nv = nhanViens.get(1 + thanhToans.size() % 2);   // NV02 hoặc NV03 thu tiền
        PhuongThucThanhToan pt = PhuongThucThanhToan.values()[thanhToans.size() % 3];
        thanhToans.add(new ThanhToan(String.format("TT%02d", thanhToans.size() + 1), ngay, soTien, pt, h, nv));
    }

    // ===== 6. Cập nhật trạng thái phòng =====
    private void capNhatTrangThai() {
        for (Phong p : phongs) {
            long trong = p.getDanhSachGiuong().stream()
                    .filter(g -> g.getTrangThai() == TrangThaiGiuong.TRONG).count();
            p.setTrangThai(trong == 0 ? TrangThaiPhong.DAY : TrangThaiPhong.CON_CHO);
        }
        phongs.get(9).setTrangThai(TrangThaiPhong.DANG_SUA_CHUA);   // P10 đang sửa chữa
    }
}