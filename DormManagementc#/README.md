# Dorm Management System — C# hoàn chỉnh theo README nhóm 36

## Đưa bản này lên GitHub

Gói này đã chọn đủ các file cần bàn giao. Giữ nguyên toàn bộ thư mục `CSharp`.
Giữ cả `KieuQueries.cs`, `NhungQueries.cs`, `LocQueries.cs`: bản đầy đủ cần đủ 30 câu;
`QueryCatalog` và `QueryTests` sử dụng cả ba lớp.

### Repository nhóm có cả Java và C#

1. Clone repository nhóm về máy, mở thư mục đó bằng VS Code.
2. Chép thư mục `CSharp` trong gói này vào gốc repository, cùng cấp với thư mục Java.
   Nếu đã có bản C# cũ, sao lưu ngoài repository rồi thay nội dung thư mục C# bằng bản này.
   Không đặt hai bản project C# lồng trong nhau.
3. Mở Terminal tại gốc repository, chạy:

```powershell
dotnet build CSharp/DormManagement.csproj
dotnet run --project CSharp/DormManagement.csproj -- --self-test
git status
git add CSharp/
git diff --cached --stat
git commit -m "Complete C# dorm management with 30 LINQ queries"
git push
```

Kiểm tra danh sách trước khi commit: chỉ chứa phần bạn muốn bàn giao; không có bin/obj/.vs.
Các lệnh trên giả định repository đã clone và branch đã có upstream. Nếu push báo nhánh remote
có thay đổi mới, đồng bộ theo quy trình của nhóm; không dùng force-push để ghi đè việc của người khác.
README chung và phần Java của nhóm vẫn ở ngoài thư mục CSharp.

### Repository chỉ dành riêng cho C#

Chép **nội dung bên trong** thư mục CSharp vào gốc repository đã clone, gồm src, csproj, README,
.gitignore, docs, scripts và verification. Lúc đó chạy `dotnet build`, `dotnet run` tại gốc.
Có thể giữ thư mục CSharp làm thư mục con nếu muốn; lệnh chạy dùng `--project CSharp/DormManagement.csproj`.

### Những gì có trong gói

- `src/` + `DormManagement.csproj`: mã nguồn và cấu hình bắt buộc để build.
- `.gitignore`: bỏ qua file build/IDE và output sinh mới.
- `README.md`: hướng dẫn chạy và bàn giao.
- `docs/`: giải thích code, 30 câu truy vấn, nội dung hỗ trợ báo cáo/demo.
- `scripts/`: kiểm chứng bổ sung bằng Python/SQL; không cần để chạy ứng dụng C#.
- `verification/`: kết quả mẫu và log kiểm tra từ bản hoàn chỉnh.

Đưa các file đã giải nén lên repository để GitHub hiển thị mã nguồn; không chỉ đưa một file ZIP.
Gói này chuẩn bị nội dung để bạn upload, chưa tạo commit hoặc đẩy lên repository của nhóm.

---


Bản bàn giao ngày 22/10/2026. Nền: DORM-DAY4-v1, mốc dữ liệu **19/10/2026**.
Đã hoàn thành 30 truy vấn LINQ (10 Kiều + 10 Nhung + 10 Lộc), menu Console, tham số truy vấn,
xuất kết quả, kiểm thử và tài liệu học/trình bày. Dự án sử dụng dữ liệu in-memory đúng mục 9 README.

## 1. Mở và chạy

Giải nén vào thư mục riêng, ví dụ `D:\OOP_HCMUTE\DormManagement-CSharp-HoanChinh`.
Không giải nén vào bên trong dự án cũ: SDK sẽ quét các file .cs và dễ phát sinh lỗi trùng lớp/entry point.

### Visual Studio

1. Mở `DormManagement.csproj` tại thư mục gốc.
2. Dự án nhắm tới .NET 8. Máy cần có .NET 8 SDK và môi trường C# hỗ trợ target này.
3. Build Solution rồi Ctrl+F5. Menu sẽ hiện danh sách K01–K10, N01–N10, L01–L10.
4. Gõ `K01`, Enter; gõ `A` hoặc Enter để chọn mặc định. Gõ `0` để thoát.

### Visual Studio Code / Terminal

Open Folder đúng thư mục chứa `.csproj`. Mở Terminal trong thư mục đó:

```powershell
dotnet --list-sdks
dotnet build
dotnet run
```

Nếu `dotnet` không được nhận diện, máy chưa có SDK trong PATH: hoàn tất cài .NET SDK rồi mở lại Terminal.
Nếu “Couldn't find a project”, chuyển Terminal về đúng thư mục chứa `.csproj`.
Không chạy riêng `Program.cs` bằng Code Runner; chương trình cần biên dịch toàn bộ project.

## 2. Các lệnh sử dụng

```powershell
dotnet run -- --summary
dotnet run -- --all
dotnet run -- --query K01 --toa B
dotnet run -- --query K02 --phong P04
dotnet run -- --query K04 --nam 2026
dotnet run -- --query N02 --keyword "Nguyễn"
dotnet run -- --query N03 --phong P01 --thang 10 --nam 2026
dotnet run -- --query N06 --giuong G01
dotnet run -- --query L01 --toa C --tang 2
dotnet run -- --self-test
dotnet run -- --export output/data
dotnet run -- --export-queries output/queries
dotnet run -- --help
```

`--export-queries` xuất 30 TSV và một JSON theo tham số mặc định. Chạy lại sẽ ghi đè các tên đó.
`--query`/`--all` in kết quả, có thể chuyển hướng `> ketqua.txt` nếu muốn lưu một lần chạy có tham số khác.
Menu hỏi tham số đúng với câu được chọn; nhập sai tháng/năm sẽ báo lỗi và quay về menu.
`ALL` chạy 30 câu với A/P01/G01/SV01/tầng 1/tháng 10/năm 2026.
Mã và tên tòa được chấp nhận; phòng phải dùng mã toàn cục P01, không dùng số phòng 101.
Tìm tên không phân biệt hoa/thường nhưng **có phân biệt dấu tiếng Việt**.

## 3. Tài liệu nên đọc theo thứ tự

1. `docs/HUONG_DAN_HOC_VA_DEMO.md`: kiến trúc, OOP, LINQ, dữ liệu, demo và các câu hỏi bảo vệ.
2. `docs/GIAI_THICH_30_TRUY_VAN.html`: mở trong trình duyệt; đủ 30 câu, thuật toán, quy ước,
   kết quả chuẩn và toàn bộ mã nguồn ba lớp truy vấn. Có mục lục và tìm kiếm.
3. `docs/GIAI_THICH_30_TRUY_VAN.md`: cùng nội dung dạng Markdown, thuận tiện đưa vào Word.
4. `docs/NGHIEM_THU.md`: bằng chứng build/test, những gì đã kiểm tra và giới hạn.
5. `docs/DE_BAI_GOC.md`: README gốc của nhóm, giữ nguyên để đối chiếu phạm vi.

## 4. Cấu trúc và trách nhiệm

| Vị trí | Vai trò |
|---|---|
| `src/Models`, `src/Enums` | 9 thực thể và 6 enum, liên kết hai chiều |
| `src/Data` | Tạo dữ liệu mẫu; mốc cố định |
| `src/DAO` | Interface generic, lớp cơ sở, 9 DAO, DormContext |
| `src/Queries/KieuQueries.cs` | K01–K10 |
| `src/Queries/NhungQueries.cs` | N01–N10 |
| `src/Queries/LocQueries.cs` | L01–L10 |
| `src/Queries/QueryBase.cs` | Quy tắc lọc đang ở, giường trống và kiểm tra tham số |
| `src/Queries/QueryCatalog.cs` | Ánh xạ mã câu → phương thức, tham số mặc định |
| `src/Queries/QueryResult.cs` | Kết quả phẳng, in Console và xuất TSV |
| `src/Program.cs` | Menu, lệnh CLI và xử lý nhập liệu |
| `src/SelfTests.cs`, `src/QueryTests.cs` | 38 kiểm tra nền + 68 kiểm tra truy vấn |
| `src/Snapshot.cs` | Xuất 9 bảng và thống kê để đối chiếu |
| `scripts/verify_queries.py` | Oracle SQL độc lập kiểm tra cả 30 kết quả mặc định |
| `verification` | Log và kết quả thực tế từ bản bàn giao |

Dùng `python scripts/verify_queries.py output/data output/queries` sau hai lệnh xuất để kiểm tra lại.
Python chỉ cần cho phép đối chiếu bổ sung; chạy ứng dụng C# không cần Python.

## 5. Các quy ước phải thống nhất khi đối chiếu Java

- Ngày hiện tại trong bài demo là 19/10/2026; không tự thay bằng DateTime.Today.
- K04, K10, L05: doanh thu **đã thu**, dựa vào ThanhToan. K04/L05 lọc năm theo ngày thanh toán.
- N03, N05: tổng **lập hóa đơn/phải thu**, dựa vào tháng/năm hóa đơn; N05 tách phòng và dịch vụ.
- N09 chưa trả đồng nào; K05 còn nợ gồm cả chưa trả và trả một phần.
- Giường có thể xếp mới: TRONG và phòng không sửa chữa. N07 chỉ lọc enum CON_CHO.
- K06 tỷ lệ = giường đang có người / toàn bộ giường vật lý, kể cả ở phòng sửa chữa.
- K06, K10, L03 trả mọi đồng hạng; N10 lấy đúng tối đa 3 phòng, nếu bằng tiền thì mã phòng tăng dần.
- L03 và L06 đếm toàn bộ hợp đồng, kể cả hủy. K09 loại hủy và hợp đồng chưa bắt đầu.
- L10 chỉ lấy HET_HAN có ngày kết thúc trước mốc, tính `(ketThuc - batDau).Days + 1`.
- K07/L09 thống kê toàn bộ 24 hồ sơ, không chỉ 21 sinh viên đang cư trú.

Đây là các lựa chọn cụ thể cho những từ ngữ chưa rõ trong README. Khi làm Java, dùng cùng quy ước.

## 6. Phạm vi hoàn thành

Đủ phần C# được mô tả trong README: model/enums, DataGenerator, DAO, 30 LINQ và chương trình demo.
Không có lưu dữ liệu qua lần chạy; mỗi lần mở chương trình tạo lại bộ mẫu. Menu tập trung vào truy vấn,
không cung cấp quy trình CRUD nghiệp vụ. DAO hiện có vẫn là thao tác danh sách từ bản Ngày 4;
xóa qua DAO không tự gỡ liên kết ngược, setter của model vẫn có thể phá vỡ ràng buộc.
Không dùng các thao tác đó để chỉnh dữ liệu chuẩn rồi kỳ vọng mọi quan hệ tự đồng bộ.

Bản này chỉ hoàn thiện C#. Chưa có bằng chứng đối chiếu 30 câu với code Java mới của nhóm;
SQL oracle và snapshot Ngày 4 được dùng để kiểm tra độc lập. Nhung có thể lấy bảng kết quả,
quy ước và kịch bản demo trong docs để hoàn thiện báo cáo/video hiện có.
