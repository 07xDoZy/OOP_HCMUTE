package dao;

import data.DataGenerator;

public class DormContext {
    public final ToaNhaDAO toaNhaDAO;
    public final LoaiPhongDAO loaiPhongDAO;
    public final PhongDAO phongDAO;
    public final GiuongDAO giuongDAO;
    public final SinhVienDAO sinhVienDAO;
    public final NhanVienDAO nhanVienDAO;
    public final HopDongDAO hopDongDAO;
    public final HoaDonDAO hoaDonDAO;
    public final ThanhToanDAO thanhToanDAO;

    public DormContext() {
        DataGenerator d = new DataGenerator();
        toaNhaDAO = new ToaNhaDAO(d.toaNhas);
        loaiPhongDAO = new LoaiPhongDAO(d.loaiPhongs);
        phongDAO = new PhongDAO(d.phongs);
        giuongDAO = new GiuongDAO(d.giuongs);
        sinhVienDAO = new SinhVienDAO(d.sinhViens);
        nhanVienDAO = new NhanVienDAO(d.nhanViens);
        hopDongDAO = new HopDongDAO(d.hopDongs);
        hoaDonDAO = new HoaDonDAO(d.hoaDons);
        thanhToanDAO = new ThanhToanDAO(d.thanhToans);
    }
}