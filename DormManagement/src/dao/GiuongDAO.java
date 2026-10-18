package dao;
import model.Giuong;
import java.util.List;
public class GiuongDAO extends BaseDAO<Giuong> {
    public GiuongDAO(List<Giuong> data) { super(data, Giuong::getMaGiuong); }
}