using DormManagement.Models;

namespace DormManagement.DAO;

public class HopDongDAO : BaseDAO<HopDong>
{
    public HopDongDAO(List<HopDong> data) : base(data, item => item.MaHD) { }
}
