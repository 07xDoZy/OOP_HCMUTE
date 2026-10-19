using DormManagement.Models;

namespace DormManagement.DAO;

public class HoaDonDAO : BaseDAO<HoaDon>
{
	public HoaDonDAO(List<HoaDon> data) : base(data, item => item.MaHoaDon) { }
}
