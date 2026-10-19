using DormManagement.Models;

namespace DormManagement.DAO;

public class ThanhToanDAO : BaseDAO<ThanhToan>
{
    public ThanhToanDAO(List<ThanhToan> data) : base(data, item => item.MaThanhToan) { }
}
