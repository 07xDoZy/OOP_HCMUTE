import dao.DormContext;
import model.*;
import query.KieuQueries;

import java.util.List;
import java.util.Locale;

public class MainQuery {
    static String tien(double v) { return String.format(Locale.US, "%,.0f", v); }

    public static void main(String[] args) {
        DormContext ctx = new DormContext();

        System.out.println("=== Cau 1: Giuong trong o toa A ===");
        List<Giuong> c1 = KieuQueries.cau1_giuongTrongToaA(ctx);
        c1.forEach(g -> System.out.println(g.getMaGiuong() + " - phong " + g.getPhong().getMaPhong()));
        System.out.println("Tong: " + c1.size());

        System.out.println("\n=== Cau 2: Sinh vien dang o phong P01 ===");
        List<SinhVien> c2 = KieuQueries.cau2_sinhVienTrongPhong(ctx, "P01");
        c2.forEach(s -> System.out.println(s.getMaSV() + " - " + s.getHoTen()));
        System.out.println("Tong: " + c2.size());

        System.out.println("\n=== Cau 3: Hop dong het han trong 30 ngay ===");
        List<HopDong> c3 = KieuQueries.cau3_hopDongSapHetHan(ctx);
        c3.forEach(h -> System.out.println(h.getMaHD() + " - " + h.getSinhVien().getMaSV()
                + " - het han " + h.getNgayKetThuc()));
        System.out.println("Tong: " + c3.size());

        System.out.println("\n=== Cau 4: Doanh thu theo thang nam 2026 ===");
        KieuQueries.cau4_doanhThuTheoThang(ctx, 2026)
                .forEach((thang, tong) -> System.out.println("Thang " + thang + ": " + tien(tong)));

        System.out.println("\n=== Cau 5: Sinh vien con no hoa don ===");
        List<SinhVien> c5 = KieuQueries.cau5_sinhVienConNo(ctx);
        c5.forEach(s -> System.out.print(s.getMaSV() + " "));
        System.out.println("\nTong: " + c5.size());

        System.out.println("\n=== Cau 6: Toa lap day cao nhat ===");
        ctx.toaNhaDAO.findAll().forEach(t -> System.out.printf(Locale.US, "Toa %s: %.1f%%%n",
                t.getTenToa(), KieuQueries.tyLeLapDay(t) * 100));
        KieuQueries.cau6_toaLapDayCaoNhat(ctx)
                .ifPresent(t -> System.out.println("=> Cao nhat: Toa " + t.getTenToa()));

        System.out.println("\n=== Cau 7: So sinh vien theo nganh ===");
        KieuQueries.cau7_soSinhVienTheoNganh(ctx).forEach((k, v) -> System.out.println(k + ": " + v));

        System.out.println("\n=== Cau 8: So phong theo loai ===");
        KieuQueries.cau8_soPhongTheoLoai(ctx).forEach((k, v) -> System.out.println(k + ": " + v));

        System.out.println("\n=== Cau 9: Sinh vien tung o nhieu hon 1 giuong ===");
        List<SinhVien> c9 = KieuQueries.cau9_sinhVienNhieuGiuong(ctx);
        c9.forEach(s -> System.out.println(s.getMaSV() + " - " + s.getHoTen()));
        System.out.println("Tong: " + c9.size());

        System.out.println("\n=== Cau 10: Nhan vien thu nhieu tien nhat ===");
        KieuQueries.cau10_nhanVienThuNhieuNhat(ctx).ifPresent(e ->
                System.out.println(e.getKey().getMaNV() + " - " + e.getKey().getHoTen()
                        + ": " + tien(e.getValue())));
    }
}