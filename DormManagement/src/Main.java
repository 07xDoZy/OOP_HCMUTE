import data.DataGenerator;
import enums.*;
import model.*;
import java.time.LocalDate;

public class Main {
    public static void main(String[] args) {
        DataGenerator d = new DataGenerator();
        LocalDate homNay = LocalDate.now();

        System.out.println("ToaNha:    " + d.toaNhas.size());
        System.out.println("LoaiPhong: " + d.loaiPhongs.size());
        System.out.println("Phong:     " + d.phongs.size());
        System.out.println("Giuong:    " + d.giuongs.size());
        System.out.println("SinhVien:  " + d.sinhViens.size());
        System.out.println("NhanVien:  " + d.nhanViens.size());
        System.out.println("HopDong:   " + d.hopDongs.size());
        System.out.println("HoaDon:    " + d.hoaDons.size());
        System.out.println("ThanhToan: " + d.thanhToans.size());
        System.out.println("--- Các ca đặc biệt ---");

        long giuongTrong = d.giuongs.stream().filter(g -> g.getTrangThai() == TrangThaiGiuong.TRONG).count();
        long sapHetHan = d.hopDongs.stream()
                .filter(h -> h.getTrangThai() == TrangThaiHopDong.HIEU_LUC
                        && !h.getNgayKetThuc().isBefore(homNay)
                        && !h.getNgayKetThuc().isAfter(homNay.plusDays(30))).count();
        long chuaTra = d.hoaDons.stream().filter(h -> h.getTongDaTra() == 0).count();
        long traMotPhan = d.hoaDons.stream().filter(h -> h.getTongDaTra() > 0 && h.getConNo() > 0.01).count();
        long conDung1Giuong = d.phongs.stream()
                .filter(p -> p.getTrangThai() == TrangThaiPhong.CON_CHO
                        && p.getDanhSachGiuong().stream().filter(g -> g.getTrangThai() == TrangThaiGiuong.TRONG).count() == 1)
                .count();
        long svNhieuGiuong = d.sinhViens.stream()
                .filter(s -> s.getDanhSachHopDong().stream().map(HopDong::getGiuong).distinct().count() > 1).count();

        System.out.println("Giường trống:                 " + giuongTrong);
        System.out.println("HĐ sắp hết hạn (30 ngày):     " + sapHetHan);
        System.out.println("Hóa đơn chưa trả:             " + chuaTra);
        System.out.println("Hóa đơn trả một phần:         " + traMotPhan);
        System.out.println("Phòng còn đúng 1 giường trống:" + conDung1Giuong);
        System.out.println("SV từng ở nhiều hơn 1 giường: " + svNhieuGiuong);
    }
}