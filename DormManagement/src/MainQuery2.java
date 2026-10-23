import dao.DormContext;
import model.*;
import query.*;

import java.util.Locale;

public class MainQuery2 {
    static String tien(double v) { return String.format(Locale.US, "%,.0f", v); }

    public static void main(String[] args) {
        DormContext ctx = new DormContext();

        System.out.println("##### NHUNG #####");
        System.out.println("-- N1: phong con giuong trong");
        NhungQueries.cau1_phongConGiuongTrong(ctx).forEach(p ->
                System.out.println(p.getMaPhong() + " (" + p.getToaNha().getTenToa() + p.getSoPhong()
                        + ") trong " + NhungQueries.soGiuongTrong(p)));
        System.out.println("-- N2: tim 'SV01' va 'Nguyễn'");
        NhungQueries.cau2_timSinhVien(ctx, "SV01").forEach(s -> System.out.println(s.getMaSV() + " " + s.getHoTen()));
        System.out.println("So SV họ Nguyễn: " + NhungQueries.cau2_timSinhVien(ctx, "Nguyễn").size());
        System.out.println("-- N3: phong P01 thang 10/2026");
        NhungQueries.cau3_tongTienPhongTrongThang(ctx, "P01", 10, 2026)
                .forEach((k, v) -> System.out.println(k + ": " + tien(v)));
        System.out.println("-- N4: tinh trang hoa don");
        NhungQueries.cau4_thongKeTinhTrangHoaDon(ctx).forEach((k, v) -> System.out.println(k + ": " + v));
        System.out.println("-- N5: doanh thu theo quy 2026");
        NhungQueries.cau5_doanhThuTheoQuy(ctx, 2026).forEach((q, v) -> System.out.println("Quy " + q + ": " + tien(v)));
        System.out.println("-- N6: SV o giuong G01");
        NhungQueries.cau6_sinhVienOGiuong(ctx, "G01").ifPresent(s ->
                System.out.println(s.getMaSV() + " " + s.getHoTen() + " - " + s.getSdt() + " - " + s.getEmail()));
        System.out.println("-- N7: phong CON_CHO: " + NhungQueries.cau7_phongConCho(ctx).size());
        System.out.println("-- N8: tong hop dong: " + NhungQueries.cau8_tatCaHopDong(ctx).size());
        System.out.println("-- N9: hoa don chua thanh toan: " + NhungQueries.cau9_hoaDonChuaThanhToan(ctx).size());
        System.out.println("-- N10: top 3 phong tien dien");
        NhungQueries.cau10_top3PhongTienDien(ctx).forEach(e ->
                System.out.println(e.getKey().getMaPhong() + ": " + tien(e.getValue())));

        System.out.println("\n##### LOC #####");
        System.out.println("-- L1: toa C tang 2");
        LocQueries.cau1_phongTheoTang(ctx, "C", 2).forEach(p -> System.out.println(p.getMaPhong()));
        System.out.println("-- L2: hop dong da huy");
        LocQueries.cau2_hopDongDaHuy(ctx).forEach(h -> System.out.println(h.getMaHD()));
        System.out.println("-- L3: SV nhieu hop dong nhat");
        LocQueries.cau3_sinhVienNhieuHopDongNhat(ctx).ifPresent(s ->
                System.out.println(s.getMaSV() + " - " + s.getDanhSachHopDong().size() + " hop dong"));
        System.out.println("-- L4: phong dang sua chua");
        LocQueries.cau4_phongDangSuaChua(ctx).forEach(p -> System.out.println(p.getMaPhong()));
        System.out.println("-- L5: doanh thu theo toa 2026");
        LocQueries.cau5_doanhThuTheoToa(ctx, 2026).forEach((k, v) -> System.out.println("Toa " + k + ": " + tien(v)));
        System.out.println("-- L6: so hop dong theo nhan vien");
        LocQueries.cau6_soHopDongTheoNhanVien(ctx).forEach(e ->
                System.out.println(e.getKey().getMaNV() + ": " + e.getValue()));
        System.out.println("-- L7: nhan vien theo chuc vu");
        LocQueries.cau7_nhanVienTheoChucVu(ctx).forEach((cv, ds) -> System.out.println(cv + ": " + ds.size()));
        System.out.println("-- L8: phong con dung 1 giuong trong");
        LocQueries.cau8_phongConDung1Giuong(ctx).forEach(p -> System.out.println(p.getMaPhong()));
        System.out.println("-- L9: SV theo gioi tinh");
        LocQueries.cau9_soSinhVienTheoGioiTinh(ctx).forEach((k, v) -> System.out.println(k + ": " + v));
        System.out.println("-- L10: cu tru trung binh (ngay): "
                + String.format(Locale.US, "%.1f", LocQueries.cau10_cuTruTrungBinh(ctx)));
    }
}