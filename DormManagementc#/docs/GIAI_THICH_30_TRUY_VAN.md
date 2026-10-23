# Giải thích 30 truy vấn LINQ

Bản C# hoàn chỉnh, dữ liệu DORM-DAY4-v1 tại 19/10/2026. Kết quả dưới đây lấy từ lần chạy thực tế với tham số mặc định. Đơn vị tiền: đồng; tỷ lệ: %; thời gian: ngày.

## K01. Giường còn trống trong một tòa

- **Nguồn:** Giuong → Phong → ToaNha.
- **Chuỗi xử lý:** Where → OrderBy → Select.
- **Quy tắc:** Nhận mã hoặc tên tòa, bỏ giường của phòng sửa chữa. Mặc định A.
- **Vị trí:** `src/Queries/KieuQueries.cs`, phương thức `Q01`.

```powershell
dotnet run -- --query K01 --toa A
```

Kết quả chuẩn: 7 dòng.

| MaGiuong | MaPhong | Toa | SoGiuong |
| --- | --- | --- | --- |
| G08 | P02 | A | 4 |
| G09 | P03 | A | 1 |
| G10 | P03 | A | 2 |
| G11 | P03 | A | 3 |
| G12 | P03 | A | 4 |
| G13 | P03 | A | 5 |
| G14 | P03 | A | 6 |

## K02. Sinh viên đang ở một phòng

- **Nguồn:** HopDong → SinhVien, Giuong → Phong.
- **Chuỗi xử lý:** Where → OrderBy → Select.
- **Quy tắc:** HIEU_LUC và ngày bắt đầu ≤ mốc ≤ ngày kết thúc. Mặc định P01; dùng mã phòng, không dùng số 101.
- **Vị trí:** `src/Queries/KieuQueries.cs`, phương thức `Q02`.

```powershell
dotnet run -- --query K02 --phong P01
```

Kết quả chuẩn: 4 dòng.

| MaSV | HoTen | MaGiuong | MaHD |
| --- | --- | --- | --- |
| SV01 | Nguyễn Văn An | G01 | HD09 |
| SV02 | Nguyễn Thị Lan | G02 | HD10 |
| SV03 | Trần Minh Khoa | G03 | HD11 |
| SV04 | Trần Thu Hà | G04 | HD12 |

## K03. Hợp đồng sắp hết hạn

- **Nguồn:** HopDong → SinhVien.
- **Chuỗi xử lý:** Where → OrderBy → ThenBy → Select.
- **Quy tắc:** Lấy hợp đồng đang cư trú, kết thúc trong [19/10/2026,18/11/2026], bao gồm hai biên. Có thể truyền soNgay khi gọi phương thức trực tiếp.
- **Vị trí:** `src/Queries/KieuQueries.cs`, phương thức `Q03`.

```powershell
dotnet run -- --query K03
```

Kết quả chuẩn: 3 dòng.

| MaHD | MaSV | HoTen | NgayKetThuc | ConLai |
| --- | --- | --- | --- | --- |
| HD13 | SV05 | Lê Hoàng Nam | 2026-10-29 | 10 |
| HD20 | SV12 | Đặng Bảo Trâm | 2026-11-08 | 20 |
| HD27 | SV19 | Đỗ Tuấn Kiệt | 2026-11-16 | 28 |

## K04. Doanh thu thực thu từng tháng

- **Nguồn:** ThanhToan.
- **Chuỗi xử lý:** Where → GroupBy → ToDictionary → Select.
- **Quy tắc:** Cộng theo năm/tháng NgayThanhToan. Sinh đủ 12 tháng, không phát sinh trả 0. Không dựa vào kỳ hóa đơn.
- **Vị trí:** `src/Queries/KieuQueries.cs`, phương thức `Q04`.

```powershell
dotnet run -- --query K04 --nam 2026
```

Kết quả chuẩn: 12 dòng.

| Nam | Thang | DaThu |
| --- | --- | --- |
| 2026 | 1 | 0 |
| 2026 | 2 | 0 |
| 2026 | 3 | 0 |
| 2026 | 4 | 0 |
| 2026 | 5 | 1255000 |
| 2026 | 6 | 1275000 |
| 2026 | 7 | 0 |
| 2026 | 8 | 0 |
| 2026 | 9 | 15920000 |
| 2026 | 10 | 4152500 |
| 2026 | 11 | 0 |
| 2026 | 12 | 0 |

## K05. Sinh viên có hóa đơn chưa trả đủ

- **Nguồn:** HoaDon → HopDong → SinhVien.
- **Chuỗi xử lý:** Where → GroupBy → OrderBy → Select/Sum.
- **Quy tắc:** ConNo > 0; một sinh viên chỉ xuất hiện một dòng với số hóa đơn còn nợ và tổng nợ.
- **Vị trí:** `src/Queries/KieuQueries.cs`, phương thức `Q05`.

```powershell
dotnet run -- --query K05
```

Kết quả chuẩn: 21 dòng.

| MaSV | HoTen | SoHoaDonConNo | TongNo |
| --- | --- | --- | --- |
| SV01 | Nguyễn Văn An | 1 | 377500 |
| SV02 | Nguyễn Thị Lan | 1 | 800000 |
| SV03 | Trần Minh Khoa | 1 | 360000 |
| SV04 | Trần Thu Hà | 1 | 765000 |
| SV05 | Lê Hoàng Nam | 1 | 405000 |
| SV06 | Lê Ngọc Mai | 1 | 755000 |
| SV07 | Phạm Quốc Bảo | 1 | 400000 |
| SV08 | Phạm Thanh Thảo | 1 | 720000 |
| SV09 | Võ Thanh Tùng | 1 | 382500 |
| SV10 | Võ Kim Ngân | 1 | 810000 |
| SV11 | Đặng Gia Huy | 1 | 377500 |
| SV12 | Đặng Bảo Trâm | 1 | 650000 |
| SV13 | Bùi Đức Anh | 1 | 285000 |
| SV14 | Bùi Phương Linh | 1 | 615000 |
| SV15 | Hồ Minh Quân | 1 | 405000 |
| SV16 | Hồ Diệu Hiền | 1 | 755000 |
| SV17 | Ngô Văn Long | 1 | 400000 |
| SV18 | Ngô Thu Trang | 1 | 720000 |
| SV19 | Đỗ Tuấn Kiệt | 1 | 382500 |
| SV20 | Đỗ Khánh Vy | 1 | 810000 |
| SV21 | Dương Hữu Phúc | 1 | 377500 |

## K06. Tòa có tỷ lệ lấp đầy cao nhất

- **Nguồn:** ToaNha → Phong → Giuong.
- **Chuỗi xử lý:** SelectMany → Count → Max → Where.
- **Quy tắc:** Mẫu số là toàn bộ giường vật lý, gồm giường tại phòng sửa chữa. Loại tòa không có giường; trả mọi đồng hạng.
- **Vị trí:** `src/Queries/KieuQueries.cs`, phương thức `Q06`.

```powershell
dotnet run -- --query K06
```

Kết quả chuẩn: 1 dòng.

| MaToa | TenToa | TongGiuong | DaO | TyLePhanTram |
| --- | --- | --- | --- | --- |
| T01 | A | 14 | 7 | 50 |

## K07. Sinh viên theo ngành

- **Nguồn:** SinhVien.
- **Chuỗi xử lý:** GroupBy → OrderBy → Select/Count.
- **Quy tắc:** Đếm toàn bộ hồ sơ sinh viên theo ngành, không chỉ sinh viên đang ở.
- **Vị trí:** `src/Queries/KieuQueries.cs`, phương thức `Q07`.

```powershell
dotnet run -- --query K07
```

Kết quả chuẩn: 6 dòng.

| Nganh | SoSinhVien |
| --- | --- |
| Công nghệ may | 4 |
| Công nghệ thông tin | 4 |
| Cơ khí chế tạo | 4 |
| Kế toán | 4 |
| Xây dựng | 4 |
| Điện - Điện tử | 4 |

## K08. Phòng theo từng loại

- **Nguồn:** LoaiPhong và Phong.
- **Chuỗi xử lý:** OrderBy → Select/Count.
- **Quy tắc:** Duyệt từ danh mục loại phòng để loại chưa có phòng vẫn xuất hiện với 0.
- **Vị trí:** `src/Queries/KieuQueries.cs`, phương thức `Q08`.

```powershell
dotnet run -- --query K08
```

Kết quả chuẩn: 3 dòng.

| MaLoaiPhong | TenLoai | SoPhong |
| --- | --- | --- |
| LP01 | Phòng 4 giường | 5 |
| LP02 | Phòng 6 giường | 4 |
| LP03 | Phòng 8 giường | 1 |

## K09. Sinh viên từng ở nhiều giường

- **Nguồn:** HopDong → SinhVien/Giuong.
- **Chuỗi xử lý:** Where → GroupBy → Distinct → Where.
- **Quy tắc:** Loại hợp đồng hủy/chưa bắt đầu. Đếm mã giường khác nhau, không đếm số hợp đồng.
- **Vị trí:** `src/Queries/KieuQueries.cs`, phương thức `Q09`.

```powershell
dotnet run -- --query K09
```

Kết quả chuẩn: 6 dòng.

| MaSV | HoTen | SoGiuong | DanhSachGiuong |
| --- | --- | --- | --- |
| SV01 | Nguyễn Văn An | 3 | G01, G09, G10 |
| SV02 | Nguyễn Thị Lan | 2 | G02, G11 |
| SV03 | Trần Minh Khoa | 2 | G03, G12 |
| SV04 | Trần Thu Hà | 2 | G04, G25 |
| SV05 | Lê Hoàng Nam | 2 | G05, G26 |
| SV06 | Lê Ngọc Mai | 2 | G06, G39 |

## K10. Nhân viên ghi nhận thu nhiều nhất

- **Nguồn:** NhanVien và ThanhToan.
- **Chuỗi xử lý:** Select/Sum → Max → Where.
- **Quy tắc:** Tổng thu mọi thời điểm trong dữ liệu; trả mọi nhân viên đồng hạng. Không phải người lập hợp đồng.
- **Vị trí:** `src/Queries/KieuQueries.cs`, phương thức `Q10`.

```powershell
dotnet run -- --query K10
```

Kết quả chuẩn: 1 dòng.

| MaNV | HoTen | TongThu |
| --- | --- | --- |
| NV02 | Lê Thị Hoa | 14175000 |

## N01. Phòng có giường trống

- **Nguồn:** Phong → Giuong.
- **Chuỗi xử lý:** Where/Any → OrderBy → Select/Count.
- **Quy tắc:** Dựa vào giường thực sự TRONG và loại phòng sửa chữa, không chỉ enum của phòng.
- **Vị trí:** `src/Queries/NhungQueries.cs`, phương thức `Q01`.

```powershell
dotnet run -- --query N01
```

Kết quả chuẩn: 6 dòng.

| MaPhong | SoPhong | Toa | GiuongTrong |
| --- | --- | --- | --- |
| P02 | 102 | A | 1 |
| P03 | 201 | A | 6 |
| P05 | 102 | B | 3 |
| P06 | 201 | B | 6 |
| P08 | 102 | C | 1 |
| P09 | 201 | C | 6 |

## N02. Tra cứu sinh viên

- **Nguồn:** SinhVien.
- **Chuỗi xử lý:** Where → OrderBy → Select.
- **Quy tắc:** Mã so khớp chính xác; họ tên tìm chuỗi con. Không phân biệt hoa thường; vẫn phân biệt dấu. Không chấp nhận từ khóa rỗng.
- **Vị trí:** `src/Queries/NhungQueries.cs`, phương thức `Q02`.

```powershell
dotnet run -- --query N02 --keyword SV01
```

Kết quả chuẩn: 1 dòng.

| MaSV | HoTen | NgaySinh | GioiTinh | Sdt | Email | Nganh |
| --- | --- | --- | --- | --- | --- | --- |
| SV01 | Nguyễn Văn An | 2004-01-01 | NAM | 0900000001 | sv01@student.hcmute.edu.vn | Công nghệ thông tin |

## N03. Tiền phòng, điện, nước theo kỳ

- **Nguồn:** HoaDon → HopDong → Giuong → Phong.
- **Chuỗi xử lý:** Where → ToList → Sum.
- **Quy tắc:** Phải thu theo kỳ hóa đơn, không phải phần nợ còn lại. Phòng tồn tại nhưng kỳ trống trả 0; mã phòng không tồn tại trả bảng rỗng.
- **Vị trí:** `src/Queries/NhungQueries.cs`, phương thức `Q03`.

```powershell
dotnet run -- --query N03 --phong P01 --thang 10 --nam 2026
```

Kết quả chuẩn: 1 dòng.

| MaPhong | Thang | Nam | SoHoaDon | TienPhong | TienDien | TienNuoc | TongPhaiThu |
| --- | --- | --- | --- | --- | --- | --- | --- |
| P01 | 10 | 2026 | 4 | 2400000 | 440000 | 200000 | 3040000 |

## N04. Thống kê trạng thái thanh toán

- **Nguồn:** HoaDon.
- **Chuỗi xử lý:** Count với ba điều kiện.
- **Quy tắc:** Đủ: ConNo ≤ 0; một phần: đã trả > 0 và còn nợ; chưa trả: đã trả ≤ 0 và còn nợ. Luôn đủ ba dòng.
- **Vị trí:** `src/Queries/NhungQueries.cs`, phương thức `Q04`.

```powershell
dotnet run -- --query N04
```

Kết quả chuẩn: 3 dòng.

| TinhTrang | SoHoaDon |
| --- | --- |
| DA_TRA_DU | 25 |
| TRA_MOT_PHAN | 11 |
| CHUA_TRA | 10 |

## N05. Doanh thu lập hóa đơn từng quý

- **Nguồn:** HoaDon.
- **Chuỗi xử lý:** Where → Range → Select/Sum.
- **Quy tắc:** Quý = (tháng − 1)/3 + 1 với phép chia nguyên. Tách tiền phòng và điện+nước. Luôn đủ 4 quý. Đây là giá trị lập hóa đơn, chưa phải tiền thực thu.
- **Vị trí:** `src/Queries/NhungQueries.cs`, phương thức `Q05`.

```powershell
dotnet run -- --query N05 --nam 2026
```

Kết quả chuẩn: 4 dòng.

| Nam | Quy | TienPhong | DichVu | TongPhaiThu |
| --- | --- | --- | --- | --- |
| 2026 | 1 | 0 | 0 | 0 |
| 2026 | 2 | 1800000 | 730000 | 2530000 |
| 2026 | 3 | 12150000 | 3770000 | 15920000 |
| 2026 | 4 | 12150000 | 3555000 | 15705000 |

## N06. Liên hệ người đang ở một giường

- **Nguồn:** HopDong → SinhVien/Giuong.
- **Chuỗi xử lý:** Where → OrderBy → Select.
- **Quy tắc:** Lọc hợp đồng đang ở tại ngày chuẩn. Trả mã sinh viên, tên, điện thoại, email. Giường không có người trả bảng rỗng.
- **Vị trí:** `src/Queries/NhungQueries.cs`, phương thức `Q06`.

```powershell
dotnet run -- --query N06 --giuong G01
```

Kết quả chuẩn: 1 dòng.

| MaGiuong | MaSV | HoTen | Sdt | Email |
| --- | --- | --- | --- | --- |
| G01 | SV01 | Nguyễn Văn An | 0900000001 | sv01@student.hcmute.edu.vn |

## N07. Phòng có enum CON_CHO

- **Nguồn:** Phong → ToaNha.
- **Chuỗi xử lý:** Where → OrderBy → Select.
- **Quy tắc:** Đúng điều kiện enum trong đề. Khác N01 ở cách lấy điều kiện dù bộ mẫu cho cùng số phòng.
- **Vị trí:** `src/Queries/NhungQueries.cs`, phương thức `Q07`.

```powershell
dotnet run -- --query N07
```

Kết quả chuẩn: 6 dòng.

| MaPhong | SoPhong | Toa | TrangThai |
| --- | --- | --- | --- |
| P02 | 102 | A | CON_CHO |
| P03 | 201 | A | CON_CHO |
| P05 | 102 | B | CON_CHO |
| P06 | 201 | B | CON_CHO |
| P08 | 102 | C | CON_CHO |
| P09 | 201 | C | CON_CHO |

## N08. Toàn bộ hợp đồng

- **Nguồn:** HopDong → SinhVien/Giuong/NhanVien.
- **Chuỗi xử lý:** OrderBy → Select.
- **Quy tắc:** Bao gồm hiệu lực, hết hạn và hủy; trình bày đầy đủ mã liên kết, ngày, cọc và trạng thái.
- **Vị trí:** `src/Queries/NhungQueries.cs`, phương thức `Q08`.

```powershell
dotnet run -- --query N08
```

Kết quả chuẩn: 31 dòng.

| MaHD | MaSV | HoTen | MaGiuong | NgayBatDau | NgayKetThuc | TienCoc | TrangThai | MaNVLap |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| HD01 | SV01 | Nguyễn Văn An | G09 | 2025-02-01 | 2025-06-30 | 1000000 | HET_HAN | NV01 |
| HD02 | SV01 | Nguyễn Văn An | G10 | 2025-09-01 | 2026-06-30 | 1000000 | HET_HAN | NV03 |
| HD03 | SV02 | Nguyễn Thị Lan | G11 | 2025-09-01 | 2026-06-30 | 1000000 | HET_HAN | NV04 |
| HD04 | SV03 | Trần Minh Khoa | G12 | 2025-09-01 | 2026-06-30 | 1000000 | HET_HAN | NV01 |
| HD05 | SV04 | Trần Thu Hà | G25 | 2025-09-01 | 2026-06-30 | 1000000 | HET_HAN | NV03 |
| HD06 | SV05 | Lê Hoàng Nam | G26 | 2025-09-01 | 2026-06-30 | 1000000 | HET_HAN | NV04 |
| HD07 | SV06 | Lê Ngọc Mai | G39 | 2025-09-01 | 2026-06-30 | 1000000 | HET_HAN | NV01 |
| HD08 | SV07 | Phạm Quốc Bảo | G07 | 2025-09-01 | 2026-06-30 | 1000000 | HET_HAN | NV03 |
| HD09 | SV01 | Nguyễn Văn An | G01 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV04 |
| HD10 | SV02 | Nguyễn Thị Lan | G02 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV01 |
| HD11 | SV03 | Trần Minh Khoa | G03 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV03 |
| HD12 | SV04 | Trần Thu Hà | G04 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV04 |
| HD13 | SV05 | Lê Hoàng Nam | G05 | 2026-09-01 | 2026-10-29 | 1000000 | HIEU_LUC | NV01 |
| HD14 | SV06 | Lê Ngọc Mai | G06 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV03 |
| HD15 | SV07 | Phạm Quốc Bảo | G07 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV04 |
| HD16 | SV08 | Phạm Thanh Thảo | G15 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV01 |
| HD17 | SV09 | Võ Thanh Tùng | G16 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV03 |
| HD18 | SV10 | Võ Kim Ngân | G17 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV04 |
| HD19 | SV11 | Đặng Gia Huy | G18 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV01 |
| HD20 | SV12 | Đặng Bảo Trâm | G19 | 2026-09-01 | 2026-11-08 | 1000000 | HIEU_LUC | NV03 |
| HD21 | SV13 | Bùi Đức Anh | G20 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV04 |
| HD22 | SV14 | Bùi Phương Linh | G21 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV01 |
| HD23 | SV15 | Hồ Minh Quân | G31 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV03 |
| HD24 | SV16 | Hồ Diệu Hiền | G32 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV04 |
| HD25 | SV17 | Ngô Văn Long | G33 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV01 |
| HD26 | SV18 | Ngô Thu Trang | G34 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV03 |
| HD27 | SV19 | Đỗ Tuấn Kiệt | G35 | 2026-09-01 | 2026-11-16 | 1000000 | HIEU_LUC | NV04 |
| HD28 | SV20 | Đỗ Khánh Vy | G36 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV01 |
| HD29 | SV21 | Dương Hữu Phúc | G37 | 2026-09-01 | 2027-06-30 | 1000000 | HIEU_LUC | NV03 |
| HD30 | SV22 | Dương Mỹ Duyên | G09 | 2026-09-05 | 2027-06-30 | 1000000 | DA_HUY | NV04 |
| HD31 | SV23 | Lý Thành Đạt | G10 | 2026-09-05 | 2027-06-30 | 1000000 | DA_HUY | NV01 |

## N09. Hóa đơn chưa thanh toán

- **Nguồn:** HoaDon → HopDong → SinhVien.
- **Chuỗi xử lý:** Where → OrderBy → Select.
- **Quy tắc:** Chưa nhận khoản tiền nào và còn nợ. Hóa đơn trả một phần không thuộc câu này.
- **Vị trí:** `src/Queries/NhungQueries.cs`, phương thức `Q09`.

```powershell
dotnet run -- --query N09
```

Kết quả chuẩn: 10 dòng.

| MaHoaDon | MaSV | Thang | Nam | TongTien | TongDaTra | ConNo |
| --- | --- | --- | --- | --- | --- | --- |
| HDN08 | SV02 | 10 | 2026 | 800000 | 0 | 800000 |
| HDN12 | SV04 | 10 | 2026 | 765000 | 0 | 765000 |
| HDN16 | SV06 | 10 | 2026 | 755000 | 0 | 755000 |
| HDN20 | SV08 | 10 | 2026 | 720000 | 0 | 720000 |
| HDN24 | SV10 | 10 | 2026 | 810000 | 0 | 810000 |
| HDN28 | SV12 | 10 | 2026 | 650000 | 0 | 650000 |
| HDN32 | SV14 | 10 | 2026 | 615000 | 0 | 615000 |
| HDN36 | SV16 | 10 | 2026 | 755000 | 0 | 755000 |
| HDN40 | SV18 | 10 | 2026 | 720000 | 0 | 720000 |
| HDN44 | SV20 | 10 | 2026 | 810000 | 0 | 810000 |

## N10. Ba phòng có tiền điện cao nhất

- **Nguồn:** Phong và HoaDon.
- **Chuỗi xử lý:** Select/Sum → OrderByDescending → ThenBy → Take.
- **Quy tắc:** Tổng điện toàn bộ kỳ trong bộ dữ liệu. Lấy tối đa 3; đồng tiền thì mã phòng tăng dần để kết quả ổn định.
- **Vị trí:** `src/Queries/NhungQueries.cs`, phương thức `Q10`.

```powershell
dotnet run -- --query N10
```

Kết quả chuẩn: 3 dòng.

| MaPhong | Toa | TongTienDien |
| --- | --- | --- |
| P01 | A | 1000000 |
| P07 | C | 980000 |
| P04 | B | 940000 |

## L01. Phòng theo tầng trong tòa

- **Nguồn:** Phong → ToaNha.
- **Chuỗi xử lý:** Where → OrderBy → Select.
- **Quy tắc:** Ghép đồng thời hai điều kiện. Tòa có thể nhập A hoặc T01; tầng phải ≥ 1.
- **Vị trí:** `src/Queries/LocQueries.cs`, phương thức `Q01`.

```powershell
dotnet run -- --query L01 --toa A --tang 1
```

Kết quả chuẩn: 2 dòng.

| MaPhong | SoPhong | Tang | Toa | TrangThai |
| --- | --- | --- | --- | --- |
| P01 | 101 | 1 | A | DAY |
| P02 | 102 | 1 | A | CON_CHO |

## L02. Hợp đồng đã hủy

- **Nguồn:** HopDong → SinhVien/Giuong.
- **Chuỗi xử lý:** Where → OrderBy → Select.
- **Quy tắc:** Lọc DA_HUY; giữ lịch sử, không xóa khỏi danh sách.
- **Vị trí:** `src/Queries/LocQueries.cs`, phương thức `Q02`.

```powershell
dotnet run -- --query L02
```

Kết quả chuẩn: 2 dòng.

| MaHD | MaSV | MaGiuong | NgayBatDau | NgayKetThuc | TrangThai |
| --- | --- | --- | --- | --- | --- |
| HD30 | SV22 | G09 | 2026-09-05 | 2027-06-30 | DA_HUY |
| HD31 | SV23 | G10 | 2026-09-05 | 2027-06-30 | DA_HUY |

## L03. Sinh viên có nhiều hợp đồng nhất

- **Nguồn:** SinhVien và HopDong.
- **Chuỗi xử lý:** Select/Count → Max → Where.
- **Quy tắc:** Đếm cả hợp đồng hủy theo cách hiểu “nhiều hợp đồng đã lập”. Trả mọi đồng hạng. Không đánh đồng với số ngày ở dài nhất.
- **Vị trí:** `src/Queries/LocQueries.cs`, phương thức `Q03`.

```powershell
dotnet run -- --query L03
```

Kết quả chuẩn: 1 dòng.

| MaSV | HoTen | SoHopDong |
| --- | --- | --- |
| SV01 | Nguyễn Văn An | 3 |

## L04. Phòng đang sửa chữa

- **Nguồn:** Phong → ToaNha.
- **Chuỗi xử lý:** Where → OrderBy → Select.
- **Quy tắc:** Lọc DANG_SUA_CHUA, hiển thị mã phòng và tòa để phân biệt số phòng trùng.
- **Vị trí:** `src/Queries/LocQueries.cs`, phương thức `Q04`.

```powershell
dotnet run -- --query L04
```

Kết quả chuẩn: 1 dòng.

| MaPhong | SoPhong | Toa | TrangThai |
| --- | --- | --- | --- |
| P10 | 202 | C | DANG_SUA_CHUA |

## L05. So sánh thực thu giữa các tòa

- **Nguồn:** ThanhToan → HoaDon → HopDong → Giuong → Phong → ToaNha.
- **Chuỗi xử lý:** Select/Where/Sum → OrderByDescending → ThenBy.
- **Quy tắc:** Lọc theo năm ngày thanh toán; tòa chưa thu vẫn có 0. Tổng ba tòa cùng phạm vi phải bằng tổng 12 tháng của K04.
- **Vị trí:** `src/Queries/LocQueries.cs`, phương thức `Q05`.

```powershell
dotnet run -- --query L05 --nam 2026
```

Kết quả chuẩn: 3 dòng.

| MaToa | TenToa | Nam | DaThu |
| --- | --- | --- | --- |
| T01 | A | 2026 | 9557500 |
| T03 | C | 2026 | 6985000 |
| T02 | B | 2026 | 6060000 |

## L06. Số hợp đồng nhân viên lập

- **Nguồn:** NhanVien và HopDong.
- **Chuỗi xử lý:** Select/Count → OrderByDescending → ThenBy.
- **Quy tắc:** Đếm toàn bộ hợp đồng kể cả hủy; nhân viên chưa lập có 0. Khác K10 là hoạt động lập hợp đồng thay vì thu tiền.
- **Vị trí:** `src/Queries/LocQueries.cs`, phương thức `Q06`.

```powershell
dotnet run -- --query L06
```

Kết quả chuẩn: 4 dòng.

| MaNV | HoTen | SoHopDong |
| --- | --- | --- |
| NV01 | Trần Quốc Hùng | 11 |
| NV03 | Phạm Văn Dũng | 10 |
| NV04 | Nguyễn Thị Mai | 10 |
| NV02 | Lê Thị Hoa | 0 |

## L07. Nhân viên theo chức vụ

- **Nguồn:** NhanVien.
- **Chuỗi xử lý:** GroupBy → OrderBy → Select.
- **Quy tắc:** Mỗi chức vụ một dòng, gồm số người và danh sách mã/tên sắp theo mã.
- **Vị trí:** `src/Queries/LocQueries.cs`, phương thức `Q07`.

```powershell
dotnet run -- --query L07
```

Kết quả chuẩn: 3 dòng.

| ChucVu | SoNhanVien | DanhSach |
| --- | --- | --- |
| QUAN_LY | 1 | NV01 - Trần Quốc Hùng |
| KE_TOAN | 1 | NV02 - Lê Thị Hoa |
| LE_TAN | 2 | NV03 - Phạm Văn Dũng; NV04 - Nguyễn Thị Mai |

## L08. Phòng còn đúng một giường

- **Nguồn:** Phong → Giuong.
- **Chuỗi xử lý:** Where/Count → OrderBy → Select/Single.
- **Quy tắc:** Dùng cùng điều kiện khả dụng với N01. Single an toàn sau khi Count == 1 trên bộ dữ liệu không bị sửa giữa truy vấn.
- **Vị trí:** `src/Queries/LocQueries.cs`, phương thức `Q08`.

```powershell
dotnet run -- --query L08
```

Kết quả chuẩn: 2 dòng.

| MaPhong | Toa | MaGiuongTrong |
| --- | --- | --- |
| P02 | A | G08 |
| P08 | C | G38 |

## L09. Sinh viên theo giới tính

- **Nguồn:** GioiTinh và SinhVien.
- **Chuỗi xử lý:** Enum.GetValues → Select/Count.
- **Quy tắc:** Luôn hiện NAM và NU, gồm toàn bộ hồ sơ. Không suy ra giới tính từ tên.
- **Vị trí:** `src/Queries/LocQueries.cs`, phương thức `Q09`.

```powershell
dotnet run -- --query L09
```

Kết quả chuẩn: 2 dòng.

| GioiTinh | SoSinhVien |
| --- | --- |
| NAM | 12 |
| NU | 12 |

## L10. Thời gian cư trú trung bình

- **Nguồn:** HopDong.
- **Chuỗi xử lý:** Where → Select → ToList → Average.
- **Quy tắc:** Chỉ HET_HAN, kết thúc trước mốc. Số ngày = chênh lệch ngày + 1. Tập rỗng trả null, hiển thị dấu —.
- **Vị trí:** `src/Queries/LocQueries.cs`, phương thức `Q10`.

```powershell
dotnet run -- --query L10
```

Kết quả chuẩn: 1 dòng.

| SoHopDong | SoNgayTrungBinh |
| --- | --- |
| 8 | 283.875 |

## Mã nguồn ba lớp truy vấn

Đọc theo từng phương thức Q01–Q10, đối chiếu bảng giải thích tương ứng ở trên.

### KieuQueries

```csharp
using DormManagement.DAO;
using DormManagement.Enums;

namespace DormManagement.Queries;

public sealed class KieuQueries : QueryBase
{
    public KieuQueries(DormContext db) : base(db) { }

    // K01: giường có thể xếp mới, loại phòng đang sửa chữa; nhận mã hoặc tên tòa.
    public QueryResult Q01(string toa) => QueryResult.From("K01", $"Giường trống tại tòa {toa}",
        Db.GiuongDAO.FindAll().Where(g => ConTrong(g) &&
            (Bang(g.Phong?.ToaNha?.MaToa, toa) || Bang(g.Phong?.ToaNha?.TenToa, toa)))
        .OrderBy(g => g.MaGiuong, StringComparer.Ordinal)
        .Select(g => new { g.MaGiuong, MaPhong = g.Phong!.MaPhong, Toa = g.Phong.ToaNha!.TenToa, g.SoGiuong }));

    public QueryResult Q02(string maPhong, DateTime ngay) => QueryResult.From("K02", $"Sinh viên đang ở phòng {maPhong} tại {ngay:yyyy-MM-dd}",
        Db.HopDongDAO.FindAll().Where(h => DangO(h, ngay) && Bang(h.Giuong.Phong?.MaPhong, maPhong))
        .OrderBy(h => h.SinhVien.MaSV, StringComparer.Ordinal)
        .Select(h => new { h.SinhVien.MaSV, h.SinhVien.HoTen, h.Giuong.MaGiuong, h.MaHD }));

    public QueryResult Q03(DateTime ngay, int soNgay = 30)
    {
        if (soNgay < 0 || soNgay > (DateTime.MaxValue.Date - ngay.Date).Days)
            throw new ArgumentOutOfRangeException(nameof(soNgay));
        var den = ngay.Date.AddDays(soNgay);
        return QueryResult.From("K03", $"Hợp đồng hết hạn từ {ngay:yyyy-MM-dd} đến {den:yyyy-MM-dd}",
            Db.HopDongDAO.FindAll().Where(h => DangO(h, ngay) && h.NgayKetThuc <= den)
            .OrderBy(h => h.NgayKetThuc).ThenBy(h => h.MaHD, StringComparer.Ordinal)
            .Select(h => new { h.MaHD, h.SinhVien.MaSV, h.SinhVien.HoTen, h.NgayKetThuc, ConLai = (h.NgayKetThuc - ngay.Date).Days }));
    }

    public QueryResult Q04(int nam)
    {
        NamHopLe(nam);
        // GroupBy theo ngày THU TIỀN, không theo kỳ hóa đơn; đủ 12 tháng kể cả 0.
        var thu = Db.ThanhToanDAO.FindAll().Where(t => t.NgayThanhToan.Year == nam)
            .GroupBy(t => t.NgayThanhToan.Month).ToDictionary(g => g.Key, g => g.Sum(t => t.SoTien));
        return QueryResult.From("K04", $"Doanh thu đã thu từng tháng năm {nam}",
            Enumerable.Range(1, 12).Select(thang => new { Nam = nam, Thang = thang, DaThu = thu.GetValueOrDefault(thang) }));
    }

    public QueryResult Q05() => QueryResult.From("K05", "Sinh viên còn hóa đơn chưa trả đủ",
        Db.HoaDonDAO.FindAll().Where(h => h.ConNo > 0).GroupBy(h => h.HopDong.SinhVien)
        .OrderBy(g => g.Key.MaSV, StringComparer.Ordinal)
        .Select(g => new { g.Key.MaSV, g.Key.HoTen, SoHoaDonConNo = g.Count(), TongNo = g.Sum(h => h.ConNo) }));

    public QueryResult Q06()
    {
        // Mẫu số là toàn bộ giường vật lý, kể cả phòng sửa chữa. Trả mọi đồng hạng.
        var rows = Db.ToaNhaDAO.FindAll().Select(t => new {
            t.MaToa, t.TenToa,
            TongGiuong = t.DanhSachPhong.Sum(p => p.DanhSachGiuong.Count),
            DaO = t.DanhSachPhong.SelectMany(p => p.DanhSachGiuong).Count(g => g.TrangThai == TrangThaiGiuong.DA_CO_NGUOI)
        }).Where(t => t.TongGiuong > 0).Select(t => new { t.MaToa, t.TenToa, t.TongGiuong, t.DaO, TyLePhanTram = 100.0 * t.DaO / t.TongGiuong }).ToList();
        var max = rows.Count == 0 ? 0 : rows.Max(t => t.TyLePhanTram);
        return QueryResult.From("K06", "Tòa có tỷ lệ lấp đầy cao nhất", rows.Where(t => t.TyLePhanTram == max).OrderBy(t => t.MaToa, StringComparer.Ordinal));
    }

    public QueryResult Q07() => QueryResult.From("K07", "Số sinh viên theo ngành (toàn bộ danh sách)",
        Db.SinhVienDAO.FindAll().GroupBy(s => s.Nganh).OrderBy(g => g.Key, StringComparer.Ordinal)
        .Select(g => new { Nganh = g.Key, SoSinhVien = g.Count() }));

    public QueryResult Q08() => QueryResult.From("K08", "Số phòng theo loại phòng",
        Db.LoaiPhongDAO.FindAll().OrderBy(l => l.MaLoaiPhong, StringComparer.Ordinal)
        .Select(l => new { l.MaLoaiPhong, l.TenLoai, SoPhong = Db.PhongDAO.FindAll().Count(p => p.LoaiPhong.MaLoaiPhong == l.MaLoaiPhong) }));

    public QueryResult Q09(DateTime ngay) => QueryResult.From("K09", "Sinh viên từng ở nhiều hơn một giường",
        Db.HopDongDAO.FindAll().Where(h => h.TrangThai != TrangThaiHopDong.DA_HUY && h.NgayBatDau <= ngay.Date)
        .GroupBy(h => h.SinhVien).Select(g => new { g.Key.MaSV, g.Key.HoTen,
            Giuong = g.Select(h => h.Giuong.MaGiuong).Distinct().OrderBy(x => x, StringComparer.Ordinal).ToArray() })
        .Where(x => x.Giuong.Length > 1).OrderBy(x => x.MaSV, StringComparer.Ordinal)
        .Select(x => new { x.MaSV, x.HoTen, SoGiuong = x.Giuong.Length, DanhSachGiuong = string.Join(", ", x.Giuong) }));

    public QueryResult Q10()
    {
        var rows = Db.NhanVienDAO.FindAll().Select(n => new { n.MaNV, n.HoTen,
            TongThu = Db.ThanhToanDAO.FindAll().Where(t => t.NhanVien.MaNV == n.MaNV).Sum(t => t.SoTien) }).ToList();
        var max = rows.Count == 0 ? 0 : rows.Max(n => n.TongThu);
        return QueryResult.From("K10", "Nhân viên ghi nhận tổng thu cao nhất (mọi đồng hạng)",
            rows.Where(n => n.TongThu == max).OrderBy(n => n.MaNV, StringComparer.Ordinal));
    }
}

```

### NhungQueries

```csharp
using DormManagement.DAO;
using DormManagement.Enums;

namespace DormManagement.Queries;

public sealed class NhungQueries : QueryBase
{
    public NhungQueries(DormContext db) : base(db) { }
    public QueryResult Q01() => QueryResult.From("N01", "Phòng có ít nhất một giường có thể xếp mới",
        Db.PhongDAO.FindAll().Where(p => p.DanhSachGiuong.Any(ConTrong)).OrderBy(p => p.MaPhong, StringComparer.Ordinal)
        .Select(p => new { p.MaPhong, p.SoPhong, Toa = p.ToaNha?.TenToa, GiuongTrong = p.DanhSachGiuong.Count(ConTrong) }));

    public QueryResult Q02(string tuKhoa)
    {
        if (string.IsNullOrWhiteSpace(tuKhoa)) throw new ArgumentException("Từ khóa không được để trống.");
        tuKhoa = tuKhoa.Trim();
        return QueryResult.From("N02", $"Tìm sinh viên: {tuKhoa}",
            Db.SinhVienDAO.FindAll().Where(s => Bang(s.MaSV, tuKhoa) || s.HoTen.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase))
            .OrderBy(s => s.MaSV, StringComparer.Ordinal)
            .Select(s => new { s.MaSV, s.HoTen, s.NgaySinh, s.GioiTinh, s.Sdt, s.Email, s.Nganh }));
    }

    public QueryResult Q03(string maPhong, int thang, int nam)
    {
        KyHopLe(thang, nam);
        var bills = Db.HoaDonDAO.FindAll().Where(h => Bang(h.HopDong.Giuong.Phong?.MaPhong, maPhong) && h.Thang == thang && h.Nam == nam).ToList();
        return QueryResult.From("N03", "Các khoản phải thu của phòng theo kỳ hóa đơn",
            Db.PhongDAO.FindAll().Where(p => Bang(p.MaPhong, maPhong)).Select(p => new {
                p.MaPhong, Thang = thang, Nam = nam, SoHoaDon = bills.Count,
                TienPhong = bills.Sum(h => h.TienPhong), TienDien = bills.Sum(h => h.TienDien),
                TienNuoc = bills.Sum(h => h.TienNuoc), TongPhaiThu = bills.Sum(h => h.TongTien) }));
    }

    public QueryResult Q04()
    {
        var bills = Db.HoaDonDAO.FindAll();
        return QueryResult.From("N04", "Số hóa đơn theo tình trạng thanh toán", new[] {
            new { TinhTrang = "DA_TRA_DU", SoHoaDon = bills.Count(h => h.ConNo <= 0) },
            new { TinhTrang = "TRA_MOT_PHAN", SoHoaDon = bills.Count(h => h.TongDaTra > 0 && h.ConNo > 0) },
            new { TinhTrang = "CHUA_TRA", SoHoaDon = bills.Count(h => h.TongDaTra <= 0 && h.ConNo > 0) }
        });
    }

    public QueryResult Q05(int nam)
    {
        NamHopLe(nam);
        // Doanh thu lập hóa đơn = tiền phòng + điện + nước. Không phân bổ khoản thu vào dịch vụ.
        var bills = Db.HoaDonDAO.FindAll().Where(h => h.Nam == nam).ToList();
        return QueryResult.From("N05", $"Doanh thu lập hóa đơn theo quý năm {nam}",
            Enumerable.Range(1, 4).Select(q => new { Nam = nam, Quy = q,
                TienPhong = bills.Where(h => (h.Thang - 1) / 3 + 1 == q).Sum(h => h.TienPhong),
                DichVu = bills.Where(h => (h.Thang - 1) / 3 + 1 == q).Sum(h => h.TienDien + h.TienNuoc),
                TongPhaiThu = bills.Where(h => (h.Thang - 1) / 3 + 1 == q).Sum(h => h.TongTien) }));
    }

    public QueryResult Q06(string maGiuong, DateTime ngay) => QueryResult.From("N06", $"Liên hệ sinh viên đang ở giường {maGiuong}",
        Db.HopDongDAO.FindAll().Where(h => Bang(h.Giuong.MaGiuong, maGiuong) && DangO(h, ngay))
        .OrderBy(h => h.SinhVien.MaSV, StringComparer.Ordinal)
        .Select(h => new { h.Giuong.MaGiuong, h.SinhVien.MaSV, h.SinhVien.HoTen, h.SinhVien.Sdt, h.SinhVien.Email }));

    public QueryResult Q07() => QueryResult.From("N07", "Phòng có trạng thái CON_CHO",
        Db.PhongDAO.FindAll().Where(p => p.TrangThai == TrangThaiPhong.CON_CHO).OrderBy(p => p.MaPhong, StringComparer.Ordinal)
        .Select(p => new { p.MaPhong, p.SoPhong, Toa = p.ToaNha?.TenToa, p.TrangThai }));

    public QueryResult Q08() => QueryResult.From("N08", "Toàn bộ hợp đồng đã lập, kể cả đã hủy",
        Db.HopDongDAO.FindAll().OrderBy(h => h.MaHD, StringComparer.Ordinal)
        .Select(h => new { h.MaHD, h.SinhVien.MaSV, h.SinhVien.HoTen, h.Giuong.MaGiuong,
            h.NgayBatDau, h.NgayKetThuc, h.TienCoc, h.TrangThai, MaNVLap = h.NhanVienLap.MaNV }));

    public QueryResult Q09() => QueryResult.From("N09", "Hóa đơn chưa thanh toán (chưa thu đồng nào và còn nợ)",
        Db.HoaDonDAO.FindAll().Where(h => h.TongDaTra <= 0 && h.ConNo > 0).OrderBy(h => h.MaHoaDon, StringComparer.Ordinal)
        .Select(h => new { h.MaHoaDon, h.HopDong.SinhVien.MaSV, h.Thang, h.Nam, h.TongTien, h.TongDaTra, h.ConNo }));

    public QueryResult Q10() => QueryResult.From("N10", "Ba phòng có tổng tiền điện cao nhất (toàn bộ kỳ)",
        Db.PhongDAO.FindAll().Select(p => new { p.MaPhong, Toa = p.ToaNha?.TenToa,
            TongTienDien = Db.HoaDonDAO.FindAll().Where(h => h.HopDong.Giuong.Phong?.MaPhong == p.MaPhong).Sum(h => h.TienDien) })
        .OrderByDescending(p => p.TongTienDien).ThenBy(p => p.MaPhong, StringComparer.Ordinal).Take(3));
}

```

### LocQueries

```csharp
using DormManagement.DAO;
using DormManagement.Enums;

namespace DormManagement.Queries;

public sealed class LocQueries : QueryBase
{
    public LocQueries(DormContext db) : base(db) { }
    public QueryResult Q01(string toa, int tang)
    {
        if (tang < 1) throw new ArgumentOutOfRangeException(nameof(tang), "Tầng phải lớn hơn 0.");
        return QueryResult.From("L01", $"Phòng tầng {tang}, tòa {toa}",
            Db.PhongDAO.FindAll().Where(p => p.Tang == tang && (Bang(p.ToaNha?.MaToa, toa) || Bang(p.ToaNha?.TenToa, toa)))
            .OrderBy(p => p.MaPhong, StringComparer.Ordinal).Select(p => new { p.MaPhong, p.SoPhong, p.Tang, Toa = p.ToaNha?.TenToa, p.TrangThai }));
    }
    public QueryResult Q02() => QueryResult.From("L02", "Hợp đồng đã hủy",
        Db.HopDongDAO.FindAll().Where(h => h.TrangThai == TrangThaiHopDong.DA_HUY).OrderBy(h => h.MaHD, StringComparer.Ordinal)
        .Select(h => new { h.MaHD, h.SinhVien.MaSV, h.Giuong.MaGiuong, h.NgayBatDau, h.NgayKetThuc, h.TrangThai }));

    public QueryResult Q03()
    {
        // Đếm mọi hợp đồng đã lập, kể cả hủy; không suy diễn số hợp đồng thành số ngày ở.
        var rows = Db.SinhVienDAO.FindAll().Select(s => new { s.MaSV, s.HoTen,
            SoHopDong = Db.HopDongDAO.FindAll().Count(h => h.SinhVien.MaSV == s.MaSV) }).ToList();
        var max = rows.Count == 0 ? 0 : rows.Max(s => s.SoHopDong);
        return QueryResult.From("L03", "Sinh viên có nhiều hợp đồng nhất (kể cả hợp đồng hủy)",
            rows.Where(s => s.SoHopDong == max).OrderBy(s => s.MaSV, StringComparer.Ordinal));
    }
    public QueryResult Q04() => QueryResult.From("L04", "Phòng đang sửa chữa",
        Db.PhongDAO.FindAll().Where(p => p.TrangThai == TrangThaiPhong.DANG_SUA_CHUA).OrderBy(p => p.MaPhong, StringComparer.Ordinal)
        .Select(p => new { p.MaPhong, p.SoPhong, Toa = p.ToaNha?.TenToa, p.TrangThai }));

    public QueryResult Q05(int nam)
    {
        NamHopLe(nam);
        return QueryResult.From("L05", $"So sánh doanh thu đã thu giữa các tòa năm {nam}",
            Db.ToaNhaDAO.FindAll().Select(t => new { t.MaToa, t.TenToa, Nam = nam,
                DaThu = Db.ThanhToanDAO.FindAll().Where(tt => tt.NgayThanhToan.Year == nam && tt.HoaDon.HopDong.Giuong.Phong?.ToaNha?.MaToa == t.MaToa).Sum(tt => tt.SoTien) })
            .OrderByDescending(t => t.DaThu).ThenBy(t => t.MaToa, StringComparer.Ordinal));
    }
    public QueryResult Q06() => QueryResult.From("L06", "Số hợp đồng mỗi nhân viên lập, giảm dần",
        Db.NhanVienDAO.FindAll().Select(n => new { n.MaNV, n.HoTen,
            SoHopDong = Db.HopDongDAO.FindAll().Count(h => h.NhanVienLap.MaNV == n.MaNV) })
        .OrderByDescending(n => n.SoHopDong).ThenBy(n => n.MaNV, StringComparer.Ordinal));

    public QueryResult Q07() => QueryResult.From("L07", "Nhân viên theo từng chức vụ",
        Db.NhanVienDAO.FindAll().GroupBy(n => n.ChucVu).OrderBy(g => g.Key)
        .Select(g => new { ChucVu = g.Key, SoNhanVien = g.Count(),
            DanhSach = string.Join("; ", g.OrderBy(n => n.MaNV, StringComparer.Ordinal).Select(n => $"{n.MaNV} - {n.HoTen}")) }));

    public QueryResult Q08() => QueryResult.From("L08", "Phòng còn đúng một giường có thể xếp mới",
        Db.PhongDAO.FindAll().Where(p => p.DanhSachGiuong.Count(ConTrong) == 1)
        .OrderBy(p => p.MaPhong, StringComparer.Ordinal)
        .Select(p => new { p.MaPhong, Toa = p.ToaNha?.TenToa, MaGiuongTrong = p.DanhSachGiuong.Single(ConTrong).MaGiuong }));

    public QueryResult Q09() => QueryResult.From("L09", "Số sinh viên theo giới tính (toàn bộ danh sách)",
        Enum.GetValues<GioiTinh>().Select(g => new { GioiTinh = g, SoSinhVien = Db.SinhVienDAO.FindAll().Count(s => s.GioiTinh == g) }));

    public QueryResult Q10(DateTime ngay)
    {
        // Khoảng đóng: ở từ ngày a đến ngày a là 1 ngày. Loại hợp đồng hủy và chưa kết thúc.
        var days = Db.HopDongDAO.FindAll().Where(h => h.TrangThai == TrangThaiHopDong.HET_HAN && h.NgayKetThuc < ngay.Date)
            .Select(h => (h.NgayKetThuc - h.NgayBatDau).Days + 1).ToList();
        return QueryResult.From("L10", "Thời gian cư trú trung bình của hợp đồng đã hết hạn (tính cả hai đầu)",
            new[] { new { SoHopDong = days.Count, SoNgayTrungBinh = days.Count == 0 ? (double?)null : days.Average() } });
    }
}

```
