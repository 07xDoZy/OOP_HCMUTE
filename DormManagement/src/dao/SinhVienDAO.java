package dao;
import model.SinhVien;
import java.util.List;
public class SinhVienDAO extends BaseDAO<SinhVien> {
    public SinhVienDAO(List<SinhVien> data) { super(data, SinhVien::getMaSV); }
}