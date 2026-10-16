package model;
import enums.TrangThaiHopDong;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

public class HopDong {
    private String maHD;
    private LocalDate ngayBatDau;
    private LocalDate ngayKetThuc;
    private double tienCoc;
    private TrangThaiHopDong trangThai;
    private SinhVien sinhVien;
    private Giuong giuong;
    private NhanVien nhanVienLap;
    private List<HoaDon> danhSachHoaDon = new ArrayList<>();

    public HopDong(String maHD, LocalDate ngayBatDau, LocalDate ngayKetThuc, double tienCoc,
                   TrangThaiHopDong trangThai, SinhVien sinhVien, Giuong giuong, NhanVien nhanVienLap) {
        this.maHD = maHD; this.ngayBatDau = ngayBatDau; this.ngayKetThuc = ngayKetThuc;
        this.tienCoc = tienCoc; this.trangThai = trangThai;
        this.sinhVien = sinhVien; this.giuong = giuong; this.nhanVienLap = nhanVienLap;
        // gán ngược hai chiều
        sinhVien.getDanhSachHopDong().add(this);
        giuong.getDanhSachHopDong().add(this);
        nhanVienLap.getDanhSachHopDongLap().add(this);
    }

    // true nếu khoảng thời gian của hợp đồng này giao với [batDau, ketThuc]
    public boolean trungThoiGian(LocalDate batDau, LocalDate ketThuc) {
        return !ngayBatDau.isAfter(ketThuc) && !ngayKetThuc.isBefore(batDau);
    }
    // getters, setters, toString

    public String getMaHD() {
        return maHD;
    }

    public void setMaHD(String maHD) {
        this.maHD = maHD;
    }

    public LocalDate getNgayBatDau() {
        return ngayBatDau;
    }

    public void setNgayBatDau(LocalDate ngayBatDau) {
        this.ngayBatDau = ngayBatDau;
    }

    public LocalDate getNgayKetThuc() {
        return ngayKetThuc;
    }

    public void setNgayKetThuc(LocalDate ngayKetThuc) {
        this.ngayKetThuc = ngayKetThuc;
    }

    public double getTienCoc() {
        return tienCoc;
    }

    public void setTienCoc(double tienCoc) {
        this.tienCoc = tienCoc;
    }

    public TrangThaiHopDong getTrangThai() {
        return trangThai;
    }

    public void setTrangThai(TrangThaiHopDong trangThai) {
        this.trangThai = trangThai;
    }

    public SinhVien getSinhVien() {
        return sinhVien;
    }

    public void setSinhVien(SinhVien sinhVien) {
        this.sinhVien = sinhVien;
    }

    public Giuong getGiuong() {
        return giuong;
    }

    public void setGiuong(Giuong giuong) {
        this.giuong = giuong;
    }

    public NhanVien getNhanVienLap() {
        return nhanVienLap;
    }

    public void setNhanVienLap(NhanVien nhanVienLap) {
        this.nhanVienLap = nhanVienLap;
    }

    public List<HoaDon> getDanhSachHoaDon() {
        return danhSachHoaDon;
    }

    public void setDanhSachHoaDon(List<HoaDon> danhSachHoaDon) {
        this.danhSachHoaDon = danhSachHoaDon;
    }

    @Override
    public String toString() {
        return "HopDong{" +
                "maHD='" + maHD + '\'' +
                ", ngayBatDau=" + ngayBatDau +
                ", ngayKetThuc=" + ngayKetThuc +
                ", tienCoc=" + tienCoc +
                ", trangThai=" + trangThai +
                '}';
    }
}
