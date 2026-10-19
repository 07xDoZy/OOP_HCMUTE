using System.Collections.ObjectModel;

namespace DormManagement.DAO;

public abstract class BaseDAO<T> : IGenericDAO<T> where T : class
{
    private readonly List<T> data;
    private readonly Func<T, string> idGetter;
    private readonly ReadOnlyCollection<T> readOnlyData;

    protected BaseDAO(List<T> data, Func<T, string> idGetter)
    {
        this.data = data;
        this.idGetter = idGetter;
        // View sống của cùng danh sách, không phải bản sao.
        readOnlyData = data.AsReadOnly();
    }

    // Giữ hành vi Java: chưa chặn mã trùng, chưa cập nhật quan hệ nghiệp vụ.
    public void Add(T item) => data.Add(item);

    // Java removeIf xóa TẤT CẢ phần tử khớp mã, không chỉ phần tử đầu.
    public bool Delete(string ma)
        => data.RemoveAll(item => idGetter(item).Equals(ma, StringComparison.Ordinal)) > 0;

    // Java Optional.empty() được biểu diễn bằng null.
    public T? FindById(string ma)
        => data.FirstOrDefault(item => idGetter(item).Equals(ma, StringComparison.Ordinal));

    // Chỉ khóa thao tác thêm/xóa qua view; các model bên trong vẫn sửa được.
    public IReadOnlyList<T> FindAll() => readOnlyData;
}
