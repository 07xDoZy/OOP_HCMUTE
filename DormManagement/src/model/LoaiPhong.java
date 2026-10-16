package model;
import java.util.ArrayList;
import java.util.List;

public class LoaiPhong {
    private String maLoaiPhong;
    private String tenLoai;
    private int soGiuong;
    private double giaThang;
    private List<Phong> danhSachPhong = new ArrayList<>();

    public LoaiPhong(String maLoaiPhong, String tenLoai, int soGiuong, double giaThang) {
        this.maLoaiPhong = maLoaiPhong;
        this.tenLoai = tenLoai;
        this.soGiuong = soGiuong;
        this.giaThang = giaThang;
    }
    // getters, setters, toString

    public String getMaLoaiPhong() {
        return maLoaiPhong;
    }

    public void setMaLoaiPhong(String maLoaiPhong) {
        this.maLoaiPhong = maLoaiPhong;
    }

    public String getTenLoai() {
        return tenLoai;
    }

    public void setTenLoai(String tenLoai) {
        this.tenLoai = tenLoai;
    }

    public int getSoGiuong() {
        return soGiuong;
    }

    public void setSoGiuong(int soGiuong) {
        this.soGiuong = soGiuong;
    }

    public double getGiaThang() {
        return giaThang;
    }

    public void setGiaThang(double giaThang) {
        this.giaThang = giaThang;
    }

    public List<Phong> getDanhSachPhong() {
        return danhSachPhong;
    }

    public void setDanhSachPhong(List<Phong> danhSachPhong) {
        this.danhSachPhong = danhSachPhong;
    }

    @Override
    public String toString() {
        return "LoaiPhong{" +
                "giaThang=" + giaThang +
                ", soGiuong=" + soGiuong +
                ", tenLoai='" + tenLoai + '\'' +
                ", maLoaiPhong='" + maLoaiPhong + '\'' +
                '}';
    }
}


