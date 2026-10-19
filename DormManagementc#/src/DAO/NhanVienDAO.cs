using DormManagement.Models;

namespace DormManagement.DAO;

public class NhanVienDAO : BaseDAO<NhanVien>
{
	public NhanVienDAO(List<NhanVien> data) : base(data, item => item.MaNV) { }
}
