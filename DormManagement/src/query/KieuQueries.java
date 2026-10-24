package query;

import dao.DormContext;
import enums.TrangThaiGiuong;
import enums.TrangThaiHopDong;
import model.*;

import java.time.LocalDate;
import java.util.*;
import java.util.stream.Collectors;

public class KieuQueries {

    // Câu 1: Các giường còn trống trong tòa nhà A
    public static List<Giuong> cau1_giuongTrongToaA(DormContext ctx) {
        return ctx.giuongDAO.findAll().stream()
                .filter(g -> g.getTrangThai() == TrangThaiGiuong.TRONG)
                .filter(g -> g.getPhong().getToaNha().getTenToa().equals("A"))
                .collect(Collectors.toList());
    }

    // Câu 2: Sinh viên đang ở trong một phòng cụ thể
    public static List<SinhVien> cau2_sinhVienTrongPhong(DormContext ctx, String maPhong) {
        return ctx.hopDongDAO.findAll().stream()
                .filter(h -> h.getTrangThai() == TrangThaiHopDong.HIEU_LUC)
                .filter(h -> h.getGiuong().getPhong().getMaPhong().equals(maPhong))
                .map(HopDong::getSinhVien)
                .collect(Collectors.toList());
    }

    // Câu 3: Hợp đồng hết hạn trong 30 ngày tới
    public static List<HopDong> cau3_hopDongSapHetHan(DormContext ctx) {
        LocalDate homNay = LocalDate.now();
        LocalDate han = homNay.plusDays(30);
        return ctx.hopDongDAO.findAll().stream()
                .filter(h -> h.getTrangThai() == TrangThaiHopDong.HIEU_LUC)
                .filter(h -> !h.getNgayKetThuc().isBefore(homNay) && !h.getNgayKetThuc().isAfter(han))
                .sorted(Comparator.comparing(HopDong::getNgayKetThuc))
                .collect(Collectors.toList());
    }

    // Câu 4: Tổng doanh thu đã thu theo từng tháng của một năm
    public static Map<Integer, Double> cau4_doanhThuTheoThang(DormContext ctx, int nam) {
        return ctx.thanhToanDAO.findAll().stream()
                .filter(t -> t.getNgayThanhToan().getYear() == nam)
                .collect(Collectors.groupingBy(
                        t -> t.getNgayThanhToan().getMonthValue(),
                        TreeMap::new,
                        Collectors.summingDouble(ThanhToan::getSoTien)));
    }

    // Câu 5: Sinh viên có hóa đơn chưa thanh toán đủ
    public static List<SinhVien> cau5_sinhVienConNo(DormContext ctx) {
        return ctx.hoaDonDAO.findAll().stream()
                .filter(h -> h.getConNo() > 0.01)
                .map(h -> h.getHopDong().getSinhVien())
                .distinct()
                .collect(Collectors.toList());
    }

    // Hàm phụ cho câu 6: tỷ lệ giường có người của một tòa (0..1)
    public static double tyLeLapDay(ToaNha toa) {
        List<Giuong> gs = toa.getDanhSachPhong().stream()
                .flatMap(p -> p.getDanhSachGiuong().stream())
                .collect(Collectors.toList());
        long coNguoi = gs.stream().filter(g -> g.getTrangThai() == TrangThaiGiuong.DA_CO_NGUOI).count();
        return gs.isEmpty() ? 0 : (double) coNguoi / gs.size();
    }

    // Câu 6: Tòa nhà có tỷ lệ lấp đầy giường cao nhất
    public static Optional<ToaNha> cau6_toaLapDayCaoNhat(DormContext ctx) {
        return ctx.toaNhaDAO.findAll().stream()
                .max(Comparator.comparingDouble(KieuQueries::tyLeLapDay));
    }

    // Câu 7: Số sinh viên theo ngành
    public static Map<String, Long> cau7_soSinhVienTheoNganh(DormContext ctx) {
        return ctx.sinhVienDAO.findAll().stream()
                .collect(Collectors.groupingBy(SinhVien::getNganh, TreeMap::new, Collectors.counting()));
    }

    // Câu 8: Số phòng theo từng loại phòng
    public static Map<String, Long> cau8_soPhongTheoLoai(DormContext ctx) {
        return ctx.phongDAO.findAll().stream()
                .collect(Collectors.groupingBy(p -> p.getLoaiPhong().getTenLoai(), TreeMap::new, Collectors.counting()));
    }

    // Câu 9: Sinh viên từng ở nhiều hơn một giường (không tính hợp đồng đã hủy)
    public static List<SinhVien> cau9_sinhVienNhieuGiuong(DormContext ctx) {
        return ctx.sinhVienDAO.findAll().stream()
                .filter(s -> s.getDanhSachHopDong().stream()
                        .filter(h -> h.getTrangThai() != TrangThaiHopDong.DA_HUY)
                        .map(HopDong::getGiuong)
                        .distinct()
                        .count() > 1)
                .collect(Collectors.toList());
    }

    // Câu 10: Nhân viên ghi nhận tổng số tiền thu nhiều nhất
    public static Optional<Map.Entry<NhanVien, Double>> cau10_nhanVienThuNhieuNhat(DormContext ctx) {
        return ctx.thanhToanDAO.findAll().stream()
                .collect(Collectors.groupingBy(ThanhToan::getNhanVien,
                        Collectors.summingDouble(ThanhToan::getSoTien)))
                .entrySet().stream()
                .max(Map.Entry.comparingByValue());
    }
}