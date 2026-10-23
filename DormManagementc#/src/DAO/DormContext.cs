using DormManagement.Data;

namespace DormManagement.DAO;

public class DormContext
{
	public ToaNhaDAO ToaNhaDAO { get; }
	public LoaiPhongDAO LoaiPhongDAO { get; }
	public PhongDAO PhongDAO { get; }
	public GiuongDAO GiuongDAO { get; }
	public SinhVienDAO SinhVienDAO { get; }
	public NhanVienDAO NhanVienDAO { get; }
	public HopDongDAO HopDongDAO { get; }
	public HoaDonDAO HoaDonDAO { get; }
	public ThanhToanDAO ThanhToanDAO { get; }

	// Không truyền ngày: dùng mốc dữ liệu chuẩn DatasetConfig.ReferenceDate.
	// Truyền ngày: phục vụ demo/kiểm tra có kết quả lặp lại được.
	public DormContext(DateTime? ngayThamChieu = null)
	{
		var d = new DataGenerator(ngayThamChieu);
		ToaNhaDAO = new ToaNhaDAO(d.ToaNhas);
		LoaiPhongDAO = new LoaiPhongDAO(d.LoaiPhongs);
		PhongDAO = new PhongDAO(d.Phongs);
		GiuongDAO = new GiuongDAO(d.Giuongs);
		SinhVienDAO = new SinhVienDAO(d.SinhViens);
		NhanVienDAO = new NhanVienDAO(d.NhanViens);
		HopDongDAO = new HopDongDAO(d.HopDongs);
		HoaDonDAO = new HoaDonDAO(d.HoaDons);
		ThanhToanDAO = new ThanhToanDAO(d.ThanhToans);
	}
}
