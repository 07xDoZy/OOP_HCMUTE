package model;
import enums.ChucVu;
import java.util.ArrayList;
import java.util.List;

public class NhanVien {
    private String maNV;
    private String hoTen;
    private String sdt;
    private ChucVu chucVu;
    private List<HopDong> danhSachHopDongLap = new ArrayList<>();
    private List<ThanhToan> danhSachThanhToanGhiNhan = new ArrayList<>();

    public NhanVien(String maNV, String hoTen, String sdt, ChucVu chucVu) {
        this.maNV = maNV; this.hoTen = hoTen; this.sdt = sdt; this.chucVu = chucVu;
    }
    // getters, setters, toString

    public String getMaNV() {
        return maNV;
    }

    public void setMaNV(String maNV) {
        this.maNV = maNV;
    }

    public String getHoTen() {
        return hoTen;
    }

    public void setHoTen(String hoTen) {
        this.hoTen = hoTen;
    }

    public String getSdt() {
        return sdt;
    }

    public void setSdt(String sdt) {
        this.sdt = sdt;
    }

    public ChucVu getChucVu() {
        return chucVu;
    }

    public void setChucVu(ChucVu chucVu) {
        this.chucVu = chucVu;
    }

    public List<HopDong> getDanhSachHopDongLap() {
        return danhSachHopDongLap;
    }

    public void setDanhSachHopDongLap(List<HopDong> danhSachHopDongLap) {
        this.danhSachHopDongLap = danhSachHopDongLap;
    }

    public List<ThanhToan> getDanhSachThanhToanGhiNhan() {
        return danhSachThanhToanGhiNhan;
    }

    public void setDanhSachThanhToanGhiNhan(List<ThanhToan> danhSachThanhToanGhiNhan) {
        this.danhSachThanhToanGhiNhan = danhSachThanhToanGhiNhan;
    }

    @Override
    public String toString() {
        return "NhanVien{" +
                "maNV='" + maNV + '\'' +
                ", hoTen='" + hoTen + '\'' +
                ", sdt='" + sdt + '\'' +
                ", chucVu=" + chucVu +
                '}';
    }
}
