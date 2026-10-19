using DormManagement.Models;

namespace DormManagement.DAO;

public class ToaNhaDAO : BaseDAO<ToaNha>
{
    public ToaNhaDAO(List<ToaNha> data) : base(data, item => item.MaToa) { }
}
