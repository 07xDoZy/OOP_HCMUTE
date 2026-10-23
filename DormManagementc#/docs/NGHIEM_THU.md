# Nghiệm thu C# — 22/10/2026

## Kết quả đã chạy thực tế

| Hạng mục | Kết quả | Bằng chứng |
|---|---|---|
| Build .NET SDK 8.0.408, target net8.0, warnings-as-errors | Thành công, 0 warning, 0 error | verification/build.txt |
| Kiểm tra nền model/DAO/dữ liệu | 38/38 | verification/tests.txt |
| Kiểm tra truy vấn và tình huống biên | 68/68 | verification/tests.txt |
| SQL độc lập đối chiếu cả 30 bảng kết quả mặc định | 30/30 | verification/sql-comparison.txt |
| CLI, tham số sai, menu quay lại sau lỗi, EOF | Đạt | verification/cli-and-baseline.txt |
| Model/Enums/DAO/Data và 11 TSV dữ liệu | Byte-identical với Ngày 4 | verification/cli-and-baseline.txt |

Tổng 106 kiểm tra C# đạt. Kiểm tra SQL dùng Python chuẩn và SQLite trong RAM, lấy các bảng TSV
rồi JOIN qua mã; không gọi lại phương thức LINQ để tạo kết quả kỳ vọng. Kiểm tra toàn bộ hàng,
thứ tự và giá trị; số thực được so với sai số 1e-7 tuyệt đối hoặc 1e-10 tương đối.

Các tình huống biên gồm: tìm không thấy, kỳ không phát sinh, tháng/năm/tầng sai, mã câu sai,
ngày biên 30/31, hợp đồng hủy, đồng hạng, trung bình trên tập rỗng, hợp đồng một ngày và DAO rỗng.

## Môi trường và giới hạn bằng chứng

Build MSBuild thực tế đã thành công. Một số lần gọi wrapper `dotnet run` trong môi trường kiểm tra
bị lỗi đọc thông tin process của môi trường; đã chạy assembly .NET đã build trực tiếp để hoàn tất
các phép kiểm tra/xuất/CLI. Không có kết quả kiểm tra giao diện Visual Studio Windows tại đây.
Trên máy nhóm, chạy `dotnet build` và `dotnet run -- --self-test` để xác nhận môi trường cá nhân.

Không khẳng định 30 kết quả Java mới đã khớp: bản Java trước đây chỉ đến Ngày 4. Dữ liệu gốc và
các nguồn cốt lõi C# không đổi; khi nhóm thêm Java Stream cần áp dụng đúng quy ước và đối chiếu TSV.

## Quy trình nghiệm thu tại máy nhóm

1. Giải nén vào thư mục riêng; build dự án.
2. Chạy self-test: phải có `38/38 checks passed.` và `Query tests: 68 passed, 0 failed.`.
3. Chạy `--all`; hoặc menu chọn K03, K05, N04, N05, L05, L10 để demo.
4. Xuất dữ liệu và truy vấn vào `output/data`, `output/queries`.
5. Nếu có Python 3: `python scripts/verify_queries.py output/data output/queries` → 30/30.
6. Ghi nhận phiên bản trong Git của nhóm và chụp kết quả chạy trên máy thực tế.

## Thay đổi so với Ngày 4

Thêm 3 lớp truy vấn, lớp cơ sở chung, catalog và kết quả phẳng; thay Program bằng menu/CLI;
thêm 68 kiểm tra LINQ; thêm oracle SQL và tài liệu. Giữ nguyên code Models/Enums/DAO/Data/Snapshot/SelfTests.

## Giới hạn đã biết

Ứng dụng đọc dữ liệu mẫu trong RAM. DAO hỗ trợ thêm/xóa danh sách nhưng chưa xử lý toàn bộ ràng buộc
CRUD và liên kết ngược; menu không cung cấp các thao tác cập nhật đó. Model có setter công khai.
K06/N01/L08 đọc trạng thái đã sinh sẵn; không tự tính lịch trống cho một yêu cầu đặt giường trong tương lai.
Q03 và các câu đang ở xét ngày truyền vào; catalog dùng ngày chuẩn 19/10/2026. Không đổi ngày tùy ý
trong catalog mà quên đồng bộ generator/trạng thái. TSV tiền hiển thị tối đa 8 chữ số phần thập phân,
phù hợp bộ mẫu, không phải cam kết độ chính xác cho mọi dữ liệu tài chính.
