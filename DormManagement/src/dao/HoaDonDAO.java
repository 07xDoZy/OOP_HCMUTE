package dao;
import model.HoaDon;
import java.util.List;
public class HoaDonDAO extends BaseDAO<HoaDon> {
    public HoaDonDAO(List<HoaDon> data) { super(data, HoaDon::getMaHoaDon); }
}