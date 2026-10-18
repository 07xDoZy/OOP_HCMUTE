package dao;
import model.LoaiPhong;
import java.util.List;
public class LoaiPhongDAO extends BaseDAO<LoaiPhong> {
    public LoaiPhongDAO(List<LoaiPhong> data) { super(data, LoaiPhong::getMaLoaiPhong); }
}