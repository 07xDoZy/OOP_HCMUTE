package model;
import enums.TrangThaiPhong;
import java.util.ArrayList;
import java.util.List;

public class Phong {
    private String maPhong;
    private String soPhong;
    private int tang;
    private TrangThaiPhong trangThai;
    private ToaNha toaNha;
    private LoaiPhong loaiPhong;
    private List<Giuong> danhSachGiuong = new ArrayList<>();

    public String getMaPhong() {
        return maPhong;
    }

    public void setMaPhong(String maPhong) {
        this.maPhong = maPhong;
    }

    public String getSoPhong() {
        return soPhong;
    }

    public void setSoPhong(String soPhong) {
        this.soPhong = soPhong;
    }

    public int getTang() {
        return tang;
    }

    public void setTang(int tang) {
        this.tang = tang;
    }

    public TrangThaiPhong getTrangThai() {
        return trangThai;
    }

    public void setTrangThai(TrangThaiPhong trangThai) {
        this.trangThai = trangThai;
    }

    public ToaNha getToaNha() {
        return toaNha;
    }

    public void setToaNha(ToaNha toaNha) {
        this.toaNha = toaNha;
    }

    public LoaiPhong getLoaiPhong() {
        return loaiPhong;
    }

    public void setLoaiPhong(LoaiPhong loaiPhong) {
        this.loaiPhong = loaiPhong;
    }

    public List<Giuong> getDanhSachGiuong() {
        return danhSachGiuong;
    }

    public void setDanhSachGiuong(List<Giuong> danhSachGiuong) {
        this.danhSachGiuong = danhSachGiuong;
    }

    public Phong(String maPhong, String soPhong, int tang, TrangThaiPhong trangThai, LoaiPhong loaiPhong) {
        this.maPhong = maPhong;
        this.soPhong = soPhong;
        this.tang = tang;
        this.trangThai = trangThai;
        this.loaiPhong = loaiPhong;
        loaiPhong.getDanhSachPhong().add(this);
    }

    public void themGiuong(Giuong g) {
        danhSachGiuong.add(g);
        g.setPhong(this);
    }
    // getters, setters, toString (không in toaNha/danhSachGiuong để tránh lặp vô hạn)

    @Override
    public String toString() {
        return "Phong{" +
                "maPhong='" + maPhong + '\'' +
                ", soPhong='" + soPhong + '\'' +
                ", tang=" + tang +
                ", trangThai=" + trangThai +
                '}';
    }
}