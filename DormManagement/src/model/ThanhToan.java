package model;
import enums.PhuongThucThanhToan;
import java.time.LocalDate;

public class ThanhToan {
    private String maThanhToan;
    private LocalDate ngayThanhToan;
    private double soTien;
    private PhuongThucThanhToan phuongThuc;
    private HoaDon hoaDon;
    private NhanVien nhanVien;

    public ThanhToan(String maThanhToan, LocalDate ngayThanhToan, double soTien,
                     PhuongThucThanhToan phuongThuc, HoaDon hoaDon, NhanVien nhanVien) {
        this.maThanhToan = maThanhToan; this.ngayThanhToan = ngayThanhToan;
        this.soTien = soTien; this.phuongThuc = phuongThuc;
        this.hoaDon = hoaDon; this.nhanVien = nhanVien;
        hoaDon.getDanhSachThanhToan().add(this);
        nhanVien.getDanhSachThanhToanGhiNhan().add(this);
    }
    // getters, setters, toString

    public String getMaThanhToan() {
        return maThanhToan;
    }

    public void setMaThanhToan(String maThanhToan) {
        this.maThanhToan = maThanhToan;
    }

    public LocalDate getNgayThanhToan() {
        return ngayThanhToan;
    }

    public void setNgayThanhToan(LocalDate ngayThanhToan) {
        this.ngayThanhToan = ngayThanhToan;
    }

    public double getSoTien() {
        return soTien;
    }

    public void setSoTien(double soTien) {
        this.soTien = soTien;
    }

    public PhuongThucThanhToan getPhuongThuc() {
        return phuongThuc;
    }

    public void setPhuongThuc(PhuongThucThanhToan phuongThuc) {
        this.phuongThuc = phuongThuc;
    }

    public HoaDon getHoaDon() {
        return hoaDon;
    }

    public void setHoaDon(HoaDon hoaDon) {
        this.hoaDon = hoaDon;
    }

    public NhanVien getNhanVien() {
        return nhanVien;
    }

    public void setNhanVien(NhanVien nhanVien) {
        this.nhanVien = nhanVien;
    }

    @Override
    public String toString() {
        return "ThanhToan{" +
                "maThanhToan='" + maThanhToan + '\'' +
                ", ngayThanhToan=" + ngayThanhToan +
                ", soTien=" + soTien +
                ", phuongThuc=" + phuongThuc +
                '}';
    }
}