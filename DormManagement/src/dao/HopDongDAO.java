package dao;
import model.HopDong;
import java.util.List;
public class HopDongDAO extends BaseDAO<HopDong> {
    public HopDongDAO(List<HopDong> data) { super(data, HopDong::getMaHD); }
}
