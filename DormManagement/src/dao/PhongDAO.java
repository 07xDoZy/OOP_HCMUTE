package dao;
import model.Phong;
import java.util.List;
public class PhongDAO extends BaseDAO<Phong> {
    public PhongDAO(List<Phong> data) { super(data, Phong::getMaPhong); }
}