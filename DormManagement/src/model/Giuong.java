package model;
import enums.TrangThaiGiuong;
import java.util.ArrayList;
import java.util.List;

public class Giuong {
    private String maGiuong;
    private int soGiuong;
    private TrangThaiGiuong trangThai;
    private Phong phong;
    private List<HopDong> danhSachHopDong = new ArrayList<>();

    public Giuong(String maGiuong, int soGiuong, TrangThaiGiuong trangThai) {
        this.maGiuong = maGiuong;
        this.soGiuong = soGiuong;
        this.trangThai = trangThai;
    }
    // getters, setters, toString

    public String getMaGiuong() {
        return maGiuong;
    }

    public void setMaGiuong(String maGiuong) {
        this.maGiuong = maGiuong;
    }

    public int getSoGiuong() {
        return soGiuong;
    }

    public void setSoGiuong(int soGiuong) {
        this.soGiuong = soGiuong;
    }

    public TrangThaiGiuong getTrangThai() {
        return trangThai;
    }

    public void setTrangThai(TrangThaiGiuong trangThai) {
        this.trangThai = trangThai;
    }

    public Phong getPhong() {
        return phong;
    }

    public void setPhong(Phong phong) {
        this.phong = phong;
    }

    public List<HopDong> getDanhSachHopDong() {
        return danhSachHopDong;
    }

    public void setDanhSachHopDong(List<HopDong> danhSachHopDong) {
        this.danhSachHopDong = danhSachHopDong;
    }

    @Override
    public String toString() {
        return "Giuong{" +
                "maGiuong='" + maGiuong + '\'' +
                ", soGiuong=" + soGiuong +
                ", trangThai=" + trangThai +
                '}';
    }
}
