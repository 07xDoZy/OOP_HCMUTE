using DormManagement.Models;

namespace DormManagement.DAO;

public class LoaiPhongDAO : BaseDAO<LoaiPhong>
{
	public LoaiPhongDAO(List<LoaiPhong> data) : base(data, item => item.MaLoaiPhong) { }
}
