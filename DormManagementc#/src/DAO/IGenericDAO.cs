namespace DormManagement.DAO;

/// <summary>Hợp đồng thao tác dữ liệu dùng chung. T là lớp model.</summary>
public interface IGenericDAO<T> where T : class
{
	void Add(T item);
	bool Delete(string ma);
	T? FindById(string ma);
	IReadOnlyList<T> FindAll();
}
