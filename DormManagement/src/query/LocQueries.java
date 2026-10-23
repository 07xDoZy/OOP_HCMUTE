package query;

import dao.DormContext;
import enums.*;
import model.*;

import java.time.temporal.ChronoUnit;
import java.util.*;
import java.util.stream.Collectors;

public class LocQueries {

    // Câu 1: Các phòng theo một tầng cụ thể trong một tòa nhà
    public static List<Phong> cau1_phongTheoTang(DormContext ctx, String tenToa, int tang) {
        return ctx.phongDAO.findAll().stream()
                .filter(p -> p.getToaNha().getTenToa().equals(tenToa) && p.getTang() == tang)
                .collect(Collectors.toList());
    }

    // Câu 2: Các hợp đồng đã bị hủy
    public static List<HopDong> cau2_hopDongDaHuy(DormContext ctx) {
        return ctx.hopDongDAO.findAll().stream()
                .filter(h -> h.getTrangThai() == TrangThaiHopDong.DA_HUY)
                .collect(Collectors.toList());
    }

    // Câu 3: Sinh viên có nhiều hợp đồng nhất
    public static Optional<SinhVien> cau3_sinhVienNhieuHopDongNhat(DormContext ctx) {
        return ctx.sinhVienDAO.findAll().stream()
                .max(Comparator.comparingInt(s -> s.getDanhSachHopDong().size()));
    }

    // Câu 4: Các phòng đang sửa chữa
    public static List<Phong> cau4_phongDangSuaChua(DormContext ctx) {
        return ctx.phongDAO.findAll().stream()
                .filter(p -> p.getTrangThai() == TrangThaiPhong.DANG_SUA_CHUA)
                .collect(Collectors.toList());
    }

    // Câu 5: So sánh tổng doanh thu giữa các tòa trong cùng một năm
    public static Map<String, Double> cau5_doanhThuTheoToa(DormContext ctx, int nam) {
        return ctx.thanhToanDAO.findAll().stream()
                .filter(t -> t.getNgayThanhToan().getYear() == nam)
                .collect(Collectors.groupingBy(
                        t -> t.getHoaDon().getHopDong().getGiuong().getPhong().getToaNha().getTenToa(),
                        TreeMap::new, Collectors.summingDouble(ThanhToan::getSoTien)));
    }

    // Câu 6: Số hợp đồng do mỗi nhân viên lập, giảm dần
    public static List<Map.Entry<NhanVien, Long>> cau6_soHopDongTheoNhanVien(DormContext ctx) {
        return ctx.hopDongDAO.findAll().stream()
                .collect(Collectors.groupingBy(HopDong::getNhanVienLap, Collectors.counting()))
                .entrySet().stream()
                .sorted(Map.Entry.<NhanVien, Long>comparingByValue().reversed()
                        .thenComparing(e -> e.getKey().getMaNV()))
                .collect(Collectors.toList());
    }

    // Câu 7: Nhân viên theo từng chức vụ
    public static Map<ChucVu, List<NhanVien>> cau7_nhanVienTheoChucVu(DormContext ctx) {
        return ctx.nhanVienDAO.findAll().stream()
                .collect(Collectors.groupingBy(NhanVien::getChucVu, TreeMap::new, Collectors.toList()));
    }

    // Câu 8: Phòng chỉ còn đúng 1 giường trống
    public static List<Phong> cau8_phongConDung1Giuong(DormContext ctx) {
        return ctx.phongDAO.findAll().stream()
                .filter(p -> p.getDanhSachGiuong().stream()
                        .filter(g -> g.getTrangThai() == TrangThaiGiuong.TRONG).count() == 1)
                .collect(Collectors.toList());
    }

    // Câu 9: Số sinh viên theo giới tính
    public static Map<GioiTinh, Long> cau9_soSinhVienTheoGioiTinh(DormContext ctx) {
        return ctx.sinhVienDAO.findAll().stream()
                .collect(Collectors.groupingBy(SinhVien::getGioiTinh, TreeMap::new, Collectors.counting()));
    }

    // Câu 10: Thời gian cư trú trung bình (ngày) của hợp đồng đã kết thúc (HET_HAN)
    public static double cau10_cuTruTrungBinh(DormContext ctx) {
        return ctx.hopDongDAO.findAll().stream()
                .filter(h -> h.getTrangThai() == TrangThaiHopDong.HET_HAN)
                .mapToLong(h -> ChronoUnit.DAYS.between(h.getNgayBatDau(), h.getNgayKetThuc()))
                .average().orElse(0);
    }
}