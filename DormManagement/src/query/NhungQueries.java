package query;

import dao.DormContext;
import enums.*;
import model.*;

import java.util.*;
import java.util.stream.Collectors;

public class NhungQueries {

    public static long soGiuongTrong(Phong p) {
        return p.getDanhSachGiuong().stream()
                .filter(g -> g.getTrangThai() == TrangThaiGiuong.TRONG).count();
    }

    // Câu 1: Các phòng còn ít nhất 1 giường trống
    public static List<Phong> cau1_phongConGiuongTrong(DormContext ctx) {
        return ctx.phongDAO.findAll().stream()
                .filter(p -> soGiuongTrong(p) > 0)
                .collect(Collectors.toList());
    }

    // Câu 2: Tìm sinh viên theo mã hoặc họ tên (chứa từ khóa, không phân biệt hoa thường)
    public static List<SinhVien> cau2_timSinhVien(DormContext ctx, String tuKhoa) {
        String k = tuKhoa.toLowerCase();
        return ctx.sinhVienDAO.findAll().stream()
                .filter(s -> s.getMaSV().equalsIgnoreCase(tuKhoa) || s.getHoTen().toLowerCase().contains(k))
                .collect(Collectors.toList());
    }

    // Câu 3: Tổng tiền phòng, điện, nước phải thu của một phòng trong tháng
    public static Map<String, Double> cau3_tongTienPhongTrongThang(DormContext ctx, String maPhong, int thang, int nam) {
        List<HoaDon> ds = ctx.hoaDonDAO.findAll().stream()
                .filter(h -> h.getHopDong().getGiuong().getPhong().getMaPhong().equals(maPhong))
                .filter(h -> h.getThang() == thang && h.getNam() == nam)
                .collect(Collectors.toList());
        Map<String, Double> kq = new LinkedHashMap<>();
        kq.put("Tien phong", ds.stream().mapToDouble(HoaDon::getTienPhong).sum());
        kq.put("Tien dien", ds.stream().mapToDouble(HoaDon::getTienDien).sum());
        kq.put("Tien nuoc", ds.stream().mapToDouble(HoaDon::getTienNuoc).sum());
        kq.put("Tong", ds.stream().mapToDouble(HoaDon::getTongTien).sum());
        return kq;
    }

    // Câu 4: Thống kê số hóa đơn: đã trả đủ / trả một phần / chưa trả
    public static Map<String, Long> cau4_thongKeTinhTrangHoaDon(DormContext ctx) {
        return ctx.hoaDonDAO.findAll().stream()
                .collect(Collectors.groupingBy(
                        h -> h.getConNo() <= 0.01 ? "Da tra du"
                                : h.getTongDaTra() > 0 ? "Tra mot phan" : "Chua tra",
                        TreeMap::new, Collectors.counting()));
    }

    // Câu 5: Doanh thu theo từng quý của một năm
    public static Map<Integer, Double> cau5_doanhThuTheoQuy(DormContext ctx, int nam) {
        return ctx.thanhToanDAO.findAll().stream()
                .filter(t -> t.getNgayThanhToan().getYear() == nam)
                .collect(Collectors.groupingBy(
                        t -> (t.getNgayThanhToan().getMonthValue() - 1) / 3 + 1,
                        TreeMap::new, Collectors.summingDouble(ThanhToan::getSoTien)));
    }

    // Câu 6: Sinh viên đang ở giường có mã cụ thể (kèm thông tin liên hệ)
    public static Optional<SinhVien> cau6_sinhVienOGiuong(DormContext ctx, String maGiuong) {
        return ctx.hopDongDAO.findAll().stream()
                .filter(h -> h.getTrangThai() == TrangThaiHopDong.HIEU_LUC)
                .filter(h -> h.getGiuong().getMaGiuong().equals(maGiuong))
                .map(HopDong::getSinhVien)
                .findFirst();
    }

    // Câu 7: Các phòng có trangThai = CON_CHO
    public static List<Phong> cau7_phongConCho(DormContext ctx) {
        return ctx.phongDAO.findAll().stream()
                .filter(p -> p.getTrangThai() == TrangThaiPhong.CON_CHO)
                .collect(Collectors.toList());
    }

    // Câu 8: Toàn bộ hợp đồng đã lập
    public static List<HopDong> cau8_tatCaHopDong(DormContext ctx) {
        return ctx.hopDongDAO.findAll().stream()
                .sorted(Comparator.comparing(HopDong::getMaHD))
                .collect(Collectors.toList());
    }

    // Câu 9: Hóa đơn chưa thanh toán (chưa trả đồng nào)
    public static List<HoaDon> cau9_hoaDonChuaThanhToan(DormContext ctx) {
        return ctx.hoaDonDAO.findAll().stream()
                .filter(h -> h.getTongDaTra() == 0)
                .collect(Collectors.toList());
    }

    // Câu 10: 3 phòng có tổng tiền điện cao nhất
    public static List<Map.Entry<Phong, Double>> cau10_top3PhongTienDien(DormContext ctx) {
        return ctx.hoaDonDAO.findAll().stream()
                .collect(Collectors.groupingBy(h -> h.getHopDong().getGiuong().getPhong(),
                        Collectors.summingDouble(HoaDon::getTienDien)))
                .entrySet().stream()
                .sorted(Map.Entry.<Phong, Double>comparingByValue().reversed()
                        .thenComparing(e -> e.getKey().getMaPhong()))
                .limit(3)
                .collect(Collectors.toList());
    }
}