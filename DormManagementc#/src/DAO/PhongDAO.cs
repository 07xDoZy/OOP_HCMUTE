using DormManagement.Models;

namespace DormManagement.DAO;

public class PhongDAO : BaseDAO<Phong>
{
	public PhongDAO(List<Phong> data) : base(data, item => item.MaPhong) { }
}
