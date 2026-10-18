package dao;
import model.ThanhToan;
import java.util.List;
public class ThanhToanDAO extends BaseDAO<ThanhToan> {
    public ThanhToanDAO(List<ThanhToan> data) { super(data, ThanhToan::getMaThanhToan); }
}