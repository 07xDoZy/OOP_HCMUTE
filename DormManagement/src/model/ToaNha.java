package model;
import java.util.ArrayList;
import java.util.List;

public class ToaNha {
    private String maToa;
    private String tenToa;
    private int soTang;
    private List<Phong> danhSachPhong = new ArrayList<>();

    public ToaNha(String maToa, String tenToa, int soTang) {
        this.maToa = maToa;
        this.tenToa = tenToa;
        this.soTang = soTang;
    }

    public void themPhong(Phong p) {   // gán cả hai chiều
        danhSachPhong.add(p);
        p.setToaNha(this);
    }
    // getters, setters, toString
    public String getMaToa() {
        return maToa;
    }

    public void setMaToa(String maToa) {
        this.maToa = maToa;
    }

    public String getTenToa() {
        return tenToa;
    }

    public void setTenToa(String tenToa) {
        this.tenToa = tenToa;
    }

    public int getSoTang() {
        return soTang;
    }

    public void setSoTang(int soTang) {
        this.soTang = soTang;
    }

    public List<Phong> getDanhSachPhong() {
        return danhSachPhong;
    }

    public void setDanhSachPhong(List<Phong> danhSachPhong) {
        this.danhSachPhong = danhSachPhong;
    }

    @Override
    public String toString() {
        return "ToaNha{" +
                "maToa='" + maToa + '\'' +
                ", soTang=" + soTang +
                ", tenToa='" + tenToa + '\'' +
                '}';
    }
}