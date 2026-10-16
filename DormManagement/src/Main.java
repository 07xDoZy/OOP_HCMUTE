import enums.*;
import model.*;
import java.time.LocalDate;

public class Main {
    public static void main(String[] args) {
        NhanVien nv = new NhanVien("NV01", "Tran Van A", "0901234567", ChucVu.LE_TAN);
        ToaNha toa = new ToaNha("T01", "A", 5);
        LoaiPhong lp = new LoaiPhong("LP01", "Phong 4 giuong", 4, 500000);
        Phong p = new Phong("P101", "101", 1, TrangThaiPhong.CON_CHO, lp);
        toa.themPhong(p);
        Giuong g = new Giuong("G01", 1, TrangThaiGiuong.DA_CO_NGUOI);
        p.themGiuong(g);
        SinhVien sv = new SinhVien("SV01", "Nguyen Van B", LocalDate.of(2005, 3, 1),
                GioiTinh.NAM, "0912345678", "b@mail.com", "CNTT");
        HopDong hd = new HopDong("HD01", LocalDate.of(2026, 9, 1), LocalDate.of(2027, 1, 31),
                1000000, TrangThaiHopDong.HIEU_LUC, sv, g, nv);
        HoaDon hoaDon = new HoaDon("HDN01", 10, 2026, 500000, 120000, 60000, hd);
        new ThanhToan("TT01", LocalDate.of(2026, 10, 5), 300000,
                PhuongThucThanhToan.TIEN_MAT, hoaDon, nv);

        System.out.println("Tong tien: " + hoaDon.getTongTien());
        System.out.println("Da tra: " + hoaDon.getTongDaTra());
        System.out.println("Con no: " + hoaDon.getConNo());
        System.out.println("Toa " + toa.getTenToa() + " co " + toa.getDanhSachPhong().size() + " phong");
        System.out.println(toa);
        System.out.println(p);
        System.out.println(hd);
    }
}