using DormManagement.Models;

namespace DormManagement.DAO;

public class SinhVienDAO : BaseDAO<SinhVien>
{
	public SinhVienDAO(List<SinhVien> data) : base(data, item => item.MaSV) { }
}
