# Hướng dẫn đọc code và trình bày phần C#

## 1. Luồng chạy từ đầu đến cuối

1. `Program.Run` đọc tham số dòng lệnh; không có tham số thì chạy menu.
2. `new DormContext()` gọi `DataGenerator` với mốc `DatasetConfig.ReferenceDate`.
3. Generator tạo nhân viên, tòa/phòng/giường, sinh viên, hợp đồng rồi hóa đơn/thanh toán,
   sau đó cập nhật trạng thái. Thứ tự này đảm bảo đối tượng cha tồn tại trước đối tượng con.
4. DormContext đưa các danh sách vào DAO tương ứng. `FindAll()` cung cấp view chỉ đọc về thao tác thêm/xóa.
5. `QueryCatalog` tạo ba lớp truy vấn cùng dùng một context; chọn mã câu gọi đúng phương thức.
6. LINQ lọc, nhóm, tính toán và chiếu các trường cần hiển thị.
7. `QueryResult.From` duyệt kết quả ngay thành mảng dòng; phản chiếu thuộc tính của đối tượng vô danh
   chỉ dùng để dựng bảng. Phần nghiệp vụ vẫn nằm trong các biểu thức LINQ.
8. Console in bảng TSV; `--export-queries` lưu TSV và JSON phẳng.

Ví dụ K02 đi theo đường: HopDong → SinhVien, HopDong → Giuong → Phong.
Điều kiện: trạng thái HIEU_LUC, ngày nằm trong khoảng cư trú, mã phòng đúng tham số.
Kết quả gồm MaSV, HoTen, MaGiuong, MaHD. Không dùng riêng danh sách hợp đồng của sinh viên
vì có cả hợp đồng lịch sử và đã hủy.

## 2. Những kiến thức OOP thực sự có trong chương trình

| Kiến thức | Vị trí | Giải thích khi bảo vệ |
|---|---|---|
| Lớp/đối tượng | Models | Một SinhVien cụ thể là đối tượng có trạng thái và liên kết |
| Constructor | HopDong | Kiểm tra ngày/trùng lịch trước khi gắn vào ba danh sách liên kết |
| Đóng gói | BaseDAO | Danh sách data là private; truy cập qua Add/Delete/FindById/FindAll |
| Interface | IGenericDAO<T> | Hợp đồng thao tác chung của DAO |
| Generic | BaseDAO<T> | Dùng chung logic cho nhiều kiểu thực thể |
| Kế thừa | SinhVienDAO : BaseDAO<SinhVien> | Chuyên biệt hóa kiểu thực thể và cách lấy khóa |
| Lớp trừu tượng | QueryBase | Chia sẻ kiểm tra năm, tháng, trạng thái đang ở |
| Đa hình | Gọi DAO qua IGenericDAO<SinhVien> | Lời gọi theo interface được thực hiện bởi đối tượng DAO thực tế |
| Liên kết 1–N | ToaNha–Phong, HoaDon–ThanhToan | Cha giữ List con; con giữ tham chiếu cha |
| Enum | TrangThaiHopDong, ChucVu… | Giới hạn các giá trị trạng thái hợp lệ |
| Exception | HopDong, Parse, QueryBase | Báo khoảng ngày/truy vấn/tham số không hợp lệ |
| Record | QueryParameters, QueryResult | Đóng gói tham số và kết quả; biểu thức with tạo bản sao tham số |

Không gọi public setter là bảo vệ tuyệt đối dữ liệu. Model bản hiện tại vẫn cho phép sửa trực tiếp;
constructor bảo vệ khi tạo hợp đồng nhưng không phải mọi thay đổi sau đó. ReadOnlyCollection chỉ
ngăn sửa cấu trúc danh sách qua view, không làm từng phần tử thành bất biến.

## 3. Từ Java Stream sang LINQ

| Ý định | Java Stream | C# LINQ trong bài |
|---|---|---|
| Lọc | filter | Where |
| Chọn trường | map | Select |
| Trải danh sách con | flatMap | SelectMany |
| Có ít nhất một | anyMatch | Any |
| Loại trùng | distinct | Distinct |
| Sắp xếp | sorted | OrderBy / OrderByDescending / ThenBy |
| Nhóm | groupingBy | GroupBy |
| Cộng | mapToDouble().sum() | Sum |
| Đếm | count | Count |
| Trung bình | average | Average |
| Giới hạn | limit | Take |
| Vật chất hóa | collect/toList | ToList / ToArray |

`Where` chọn dòng; `Select` chọn cột. `GroupBy` chia các dòng thành nhóm có cùng khóa.
Ví dụ K05: lọc hóa đơn còn nợ → nhóm theo sinh viên → mỗi nhóm tính số hóa đơn và tổng nợ.
Không cộng trực tiếp mọi hóa đơn của sinh viên sau khi chỉ kiểm tra có một hóa đơn nợ: phải chỉ cộng
những hóa đơn còn nợ để tránh kéo cả khoản trả dư vào kết quả.

LINQ thường thực thi trì hoãn: biểu thức chỉ được duyệt khi ToList, ToArray, Sum, Count… được gọi.
Ở bài này `QueryResult.From` vật chất hóa dữ liệu thành mảng trước khi in, nên không truy vấn lại
mỗi lần in một ô. `ToList` trong các câu tìm cực đại giúp không thực hiện lại phép tính nhiều lần.

`Average()` trên tập rỗng gây lỗi: L10 kiểm tra Count trước và trả null nếu chưa có hợp đồng kết thúc.
`Sum()` trên tập số rỗng trả 0: phù hợp tháng hoặc phòng không có phát sinh.
`FirstOrDefault` có thể trả null: DAO dùng `T?`, người gọi phải kiểm tra.

## 4. Cơ sở dữ liệu cơ bản và quan hệ trong bộ nhớ

- Khóa chính là MaSV, MaHD, MaHoaDon…; số phòng 101 không phải khóa toàn cục vì nhiều tòa cùng có phòng 101.
- Trong model, khóa ngoại được biểu diễn bằng tham chiếu đối tượng, ví dụ HoaDon.HopDong.
- Trong TSV, liên kết được xuất thành mã, ví dụ HopDong_MaHD. Các mã cho phép JOIN bảng độc lập.
- Tòa 1–N phòng; phòng 1–N giường; sinh viên 1–N hợp đồng; giường 1–N hợp đồng theo thời gian.
- Hóa đơn 1–N thanh toán: một hóa đơn có thể được trả nhiều đợt. Nếu JOIN thẳng hóa đơn với thanh toán
  rồi SUM(TongTien), tiền hóa đơn có thể bị nhân lên theo số lượt thanh toán. Bài này tính hai phía riêng.
- TongTien/TongDaTra/ConNo là thuộc tính suy diễn, tránh cập nhật đồng thời nhiều bản sao tổng tiền.
- `scripts/verify_queries.py` nạp TSV vào SQLite trong RAM và chạy SELECT/JOIN/GROUP BY độc lập.
  Đây là công cụ kiểm chứng và học SQL; ứng dụng C# chính không kết nối SQLite hoặc SQL Server.

Ví dụ minh họa truy vấn nợ theo SQL: JOIN HoaDon với HopDong để xác định sinh viên, lọc số nợ > 0,
GROUP BY MaSV rồi SUM số nợ. LINQ dùng navigation property thay cho thao tác JOIN viết tường minh.
Trong SQL thông thường không có cột ConNo lưu sẵn: phải tính từ tổng hóa đơn trừ SUM thanh toán.
Oracle dùng snapshot đã xuất các cột suy diễn; các công thức model có kiểm tra riêng trong SelfTests.

## 5. Những con số cần hiểu

- 3 tòa, 3 loại phòng, 10 phòng, 52 giường.
- 24 hồ sơ sinh viên nhưng 21 hợp đồng hiệu lực; không phải tất cả hồ sơ đều đang ở.
- 31 hợp đồng: 21 hiệu lực, 8 hết hạn, 2 hủy.
- 31 giường mang enum TRONG, trong đó 8 thuộc P10 đang sửa chữa: chỉ 23 giường có thể xếp mới.
- Tổng lập hóa đơn 34.155.000; đã thu 22.602.500; còn nợ 11.552.500.
- K06: tòa A, 7/14 giường = 50%, cao nhất theo cách tính toàn bộ giường vật lý.
- K10: NV02 Lê Thị Hoa ghi nhận 14.175.000.
- L03: SV01 Nguyễn Văn An, 3 hợp đồng. Có nhiều hợp đồng không đồng nghĩa chắc chắn ở lâu nhất.
- L10: 8 hợp đồng hết hạn, trung bình 283.875 ngày khi tính cả hai đầu.

Tổng lập hóa đơn và tổng thu có thể lệch năm nếu hóa đơn được trả vào năm sau. Không dùng K04 để
kết luận doanh thu lập hóa đơn N05 phải bằng nhau. Trong mẫu hiện tại tổng còn nợ bằng chênh lệch
hai tổng vì chưa có thanh toán vượt mức và các khoản đều thuộc phạm vi mẫu.

## 6. Kịch bản demo phần C# khoảng 5–7 phút

1. (40 giây) Mở README và project: giải thích C# Console .NET 8, dữ liệu in-memory, mốc cố định.
2. (50 giây) Mở HopDong constructor: chỉ ra kiểm tra ngày và trùng lịch trước khi gắn hai chiều.
3. (40 giây) Mở BaseDAO và DormContext: giải thích interface/generic và cách dữ liệu vào DAO.
4. (60 giây) Chạy K03: 3 hợp đồng sắp hết hạn. Đọc Where, OrderBy, Select trên code tương ứng.
5. (60 giây) Chạy K05 và N04: phân biệt còn nợ, trả một phần, chưa trả. Chỉ ra GroupBy/Sum.
6. (40 giây) Chạy N05 và L05: giải thích doanh thu lập hóa đơn so với thực thu, không đổi khái niệm.
7. (40 giây) Chạy L10: khoảng ngày đóng, loại hợp đồng hủy, xử lý tập rỗng.
8. (40 giây) Chạy `--self-test`, mở kết quả TSV hoặc HTML. Chốt đủ 30 câu và quy ước để đối chiếu Java.

Lộc cần nắm luồng tổng thể và L01–L10; Kiều/Nhung vẫn cần hiểu 10 câu mang tên mình theo README.
Nhung có thể đưa vào Word: kiến trúc, bảng thuật ngữ, điều kiện từng câu, ảnh console và bảng kết quả.
Chụp ảnh sau khi chạy trên máy nhóm, không trình bày log kiểm tra như ảnh đã chạy ở máy cá nhân.

## 7. Câu hỏi thường gặp khi bảo vệ

**Tại sao K02 không chỉ kiểm tra HIEU_LUC?** Vì trạng thái có thể chưa cập nhật; cần cả khoảng ngày.
**Tại sao không đổi DateTime.Today?** Để lần chạy lại và Java cho cùng kết quả chuẩn. Thay mốc cần
đồng thời sinh lại dữ liệu và truyền đúng ngày cho truy vấn; catalog hiện khóa vào mốc Ngày 4.
**Tại sao K06 tính cả giường sửa chữa?** Chỉ số đang dùng là công suất vật lý toàn tòa. Nếu tính công suất
khai thác, cần loại các giường sửa chữa khỏi mẫu số ở cả Java/C# và ghi lại định nghĩa.
**Tại sao N01 và N07 riêng nhau?** N01 kiểm tra giường thực tế; N07 lọc đúng enum như đề. Ở mẫu đồng bộ
có thể cùng kết quả, nhưng ý nghĩa và nguồn điều kiện khác nhau.
**Vì sao tiền vẫn là double?** Theo README và model Java đã chốt. Hệ thống tiền tệ thực tế thường cần
kiểu decimal và quy tắc làm tròn; đổi phải thực hiện nhất quán hai bản, không chỉ sửa một thuộc tính.
**Có dữ liệu thật hay đăng nhập không?** Đây là MIS demo truy vấn dữ liệu mẫu theo đặc tả hiện có.
**Đã khớp 30 câu Java chưa?** Chưa có code truy vấn Java mới trong gói; đã khớp 30 phép tính SQL độc lập.
