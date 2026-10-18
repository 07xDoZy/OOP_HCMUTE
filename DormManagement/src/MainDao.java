import dao.DormContext;

public class MainDao {
    public static void main(String[] args) {
        DormContext ctx = new DormContext();

        System.out.println("So sinh vien: " + ctx.sinhVienDAO.findAll().size());
        System.out.println("So hop dong:  " + ctx.hopDongDAO.findAll().size());

        System.out.println(ctx.sinhVienDAO.findById("SV01").orElse(null));
        System.out.println(ctx.phongDAO.findById("P99").isPresent());   // false

        int truoc = ctx.nhanVienDAO.findAll().size();
        ctx.nhanVienDAO.add(new model.NhanVien("NV99", "Test", "000", enums.ChucVu.LE_TAN));
        System.out.println("Sau add:    " + ctx.nhanVienDAO.findAll().size() + " (truoc: " + truoc + ")");
        System.out.println("Delete NV99: " + ctx.nhanVienDAO.delete("NV99"));
        System.out.println("Sau delete: " + ctx.nhanVienDAO.findAll().size());
    }
}