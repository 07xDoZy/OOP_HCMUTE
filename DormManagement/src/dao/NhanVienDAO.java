package dao;
import model.NhanVien;
import java.util.List;
public class NhanVienDAO extends BaseDAO<NhanVien> {
    public NhanVienDAO(List<NhanVien> data) { super(data, NhanVien::getMaNV); }
}