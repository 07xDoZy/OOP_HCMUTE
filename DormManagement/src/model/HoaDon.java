package model;
import java.util.ArrayList;
import java.util.List;

public class HoaDon {
    private String maHoaDon;
    private int thang;
    private int nam;
    private double tienPhong;
    private double tienDien;
    private double tienNuoc;
    private HopDong hopDong;
    private List<ThanhToan> danhSachThanhToan = new ArrayList<>();

    public HoaDon(String maHoaDon, int thang, int nam, double tienPhong,
                  double tienDien, double tienNuoc, HopDong hopDong) {
        this.maHoaDon = maHoaDon; this.thang = thang; this.nam = nam;
        this.tienPhong = tienPhong; this.tienDien = tienDien; this.tienNuoc = tienNuoc;
        this.hopDong = hopDong;
        hopDong.getDanhSachHoaDon().add(this);
    }

    public double getTongTien() { return tienPhong + tienDien + tienNuoc; }

    public double getTongDaTra() {
        return danhSachThanhToan.stream().mapToDouble(ThanhToan::getSoTien).sum();
    }

    public double getConNo() { return getTongTien() - getTongDaTra(); }
    // getters, setters, toString

    public String getMaHoaDon() {
        return maHoaDon;
    }

    public void setMaHoaDon(String maHoaDon) {
        this.maHoaDon = maHoaDon;
    }

    public int getThang() {
        return thang;
    }

    public void setThang(int thang) {
        this.thang = thang;
    }

    public int getNam() {
        return nam;
    }

    public void setNam(int nam) {
        this.nam = nam;
    }

    public double getTienPhong() {
        return tienPhong;
    }

    public void setTienPhong(double tienPhong) {
        this.tienPhong = tienPhong;
    }

    public double getTienDien() {
        return tienDien;
    }

    public void setTienDien(double tienDien) {
        this.tienDien = tienDien;
    }

    public double getTienNuoc() {
        return tienNuoc;
    }

    public void setTienNuoc(double tienNuoc) {
        this.tienNuoc = tienNuoc;
    }

    public HopDong getHopDong() {
        return hopDong;
    }

    public void setHopDong(HopDong hopDong) {
        this.hopDong = hopDong;
    }

    public List<ThanhToan> getDanhSachThanhToan() {
        return danhSachThanhToan;
    }

    public void setDanhSachThanhToan(List<ThanhToan> danhSachThanhToan) {
        this.danhSachThanhToan = danhSachThanhToan;
    }

    @Override
    public String toString() {
        return "HoaDon{" +
                "maHoaDon='" + maHoaDon + '\'' +
                ", thang=" + thang +
                ", nam=" + nam +
                ", tienPhong=" + tienPhong +
                ", tienDien=" + tienDien +
                ", tienNuoc=" + tienNuoc +
                '}';
    }
}