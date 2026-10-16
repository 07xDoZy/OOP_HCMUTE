package model;
import enums.GioiTinh;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

public class SinhVien {
    private String maSV;
    private String hoTen;
    private LocalDate ngaySinh;
    private GioiTinh gioiTinh;
    private String sdt;
    private String email;
    private String nganh;
    private List<HopDong> danhSachHopDong = new ArrayList<>();

    public SinhVien(String maSV, String hoTen, LocalDate ngaySinh, GioiTinh gioiTinh,
                    String sdt, String email, String nganh) {
        this.maSV = maSV; this.hoTen = hoTen; this.ngaySinh = ngaySinh;
        this.gioiTinh = gioiTinh; this.sdt = sdt; this.email = email; this.nganh = nganh;
    }
    // getters, setters, toString

    public String getMaSV() {
        return maSV;
    }

    public void setMaSV(String maSV) {
        this.maSV = maSV;
    }

    public String getHoTen() {
        return hoTen;
    }

    public void setHoTen(String hoTen) {
        this.hoTen = hoTen;
    }

    public LocalDate getNgaySinh() {
        return ngaySinh;
    }

    public void setNgaySinh(LocalDate ngaySinh) {
        this.ngaySinh = ngaySinh;
    }

    public GioiTinh getGioiTinh() {
        return gioiTinh;
    }

    public void setGioiTinh(GioiTinh gioiTinh) {
        this.gioiTinh = gioiTinh;
    }

    public String getSdt() {
        return sdt;
    }

    public void setSdt(String sdt) {
        this.sdt = sdt;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getNganh() {
        return nganh;
    }

    public void setNganh(String nganh) {
        this.nganh = nganh;
    }

    public List<HopDong> getDanhSachHopDong() {
        return danhSachHopDong;
    }

    public void setDanhSachHopDong(List<HopDong> danhSachHopDong) {
        this.danhSachHopDong = danhSachHopDong;
    }

    @Override
    public String toString() {
        return "SinhVien{" +
                "maSV='" + maSV + '\'' +
                ", hoTen='" + hoTen + '\'' +
                ", ngaySinh=" + ngaySinh +
                ", gioiTinh=" + gioiTinh +
                ", sdt='" + sdt + '\'' +
                ", email='" + email + '\'' +
                ", nganh='" + nganh + '\'' +
                '}';
    }
}

