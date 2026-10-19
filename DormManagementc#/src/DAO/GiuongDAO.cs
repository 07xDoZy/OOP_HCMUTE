using DormManagement.Models;

namespace DormManagement.DAO;

public class GiuongDAO : BaseDAO<Giuong>
{
    public GiuongDAO(List<Giuong> data) : base(data, item => item.MaGiuong) { }
}
