package dao;
import model.ToaNha;
import java.util.List;
public class ToaNhaDAO extends BaseDAO<ToaNha> {
    public ToaNhaDAO(List<ToaNha> data) { super(data, ToaNha::getMaToa); }
}
