"""Independent SQL oracle over exported Day-4 TSV. Python stdlib only.
Usage: python scripts/verify_queries.py [data_directory] [queries_directory]
This checks default parameters A/P01/G01/SV01/1/10/2026, date 2026-10-19.
"""
import csv
import math
import sqlite3
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
data_dir = Path(sys.argv[1]) if len(sys.argv) > 1 else ROOT / 'verification/data'
query_dir = Path(sys.argv[2]) if len(sys.argv) > 2 else ROOT / 'verification/queries'
con = sqlite3.connect(':memory:')
for name in ['ToaNha','LoaiPhong','Phong','Giuong','SinhVien','NhanVien','HopDong','HoaDon','ThanhToan']:
    with (data_dir / (name + '.tsv')).open(encoding='utf-8-sig', newline='') as f:
        rows = list(csv.reader(f, delimiter='\t'))
    columns = rows[0]
    con.execute(f'CREATE TABLE "{name}" (' + ','.join('"'+c+'" TEXT' for c in columns) + ')')
    con.executemany(f'INSERT INTO "{name}" VALUES (' + ','.join('?' for c in columns) + ')', rows[1:])
con.executescript('''
CREATE VIEW residence AS SELECT h.*, s.HoTen, s.Sdt, s.Email, g.Phong_MaPhong AS MaPhong,
 p.ToaNha_MaToa AS MaToa FROM HopDong h JOIN SinhVien s ON s.MaSV=h.SinhVien_MaSV
 JOIN Giuong g ON g.MaGiuong=h.Giuong_MaGiuong JOIN Phong p ON p.MaPhong=g.Phong_MaPhong;
CREATE VIEW bills AS SELECT b.*, r.SinhVien_MaSV AS MaSV, r.HoTen, r.MaPhong, r.MaToa
 FROM HoaDon b JOIN residence r ON r.MaHD=b.HopDong_MaHD;
CREATE VIEW receipts AS SELECT t.*, b.MaToa FROM ThanhToan t JOIN bills b ON b.MaHoaDon=t.HoaDon_MaHoaDon;
CREATE VIEW available AS SELECT g.*, p.ToaNha_MaToa AS MaToa FROM Giuong g
 JOIN Phong p ON p.MaPhong=g.Phong_MaPhong WHERE g.TrangThai='TRONG' AND p.TrangThai<>'DANG_SUA_CHUA';
CREATE VIEW active AS SELECT * FROM residence WHERE TrangThai='HIEU_LUC'
 AND NgayBatDau<='2026-10-19' AND NgayKetThuc>='2026-10-19';
''')
SQL = {
'K01': "SELECT a.MaGiuong,a.Phong_MaPhong,t.TenToa,a.SoGiuong FROM available a JOIN ToaNha t ON a.MaToa=t.MaToa WHERE t.TenToa='A' ORDER BY a.MaGiuong",
'K02': "SELECT SinhVien_MaSV,HoTen,Giuong_MaGiuong,MaHD FROM active WHERE MaPhong='P01' ORDER BY SinhVien_MaSV",
'K03': "SELECT MaHD,SinhVien_MaSV,HoTen,NgayKetThuc,julianday(NgayKetThuc)-julianday('2026-10-19') FROM active WHERE NgayKetThuc<='2026-11-18' ORDER BY NgayKetThuc,MaHD",
'K04': "WITH RECURSIVE m(n) AS (SELECT 1 UNION ALL SELECT n+1 FROM m WHERE n<12) SELECT 2026,n,coalesce((SELECT sum(SoTien) FROM ThanhToan WHERE substr(NgayThanhToan,1,4)='2026' AND cast(substr(NgayThanhToan,6,2) AS INT)=n),0) FROM m ORDER BY n",
'K05': "SELECT MaSV,HoTen,count(*),sum(ConNo) FROM bills WHERE cast(ConNo AS REAL)>0 GROUP BY MaSV,HoTen ORDER BY MaSV",
'K06': "WITH x AS (SELECT t.MaToa,t.TenToa,count(g.MaGiuong) n,sum(CASE WHEN g.TrangThai='DA_CO_NGUOI' THEN 1 ELSE 0 END) a FROM ToaNha t JOIN Phong p ON p.ToaNha_MaToa=t.MaToa JOIN Giuong g ON g.Phong_MaPhong=p.MaPhong GROUP BY t.MaToa,t.TenToa), y AS (SELECT *,100.0*a/n rate FROM x WHERE n>0) SELECT * FROM y WHERE rate=(SELECT max(rate) FROM y) ORDER BY MaToa",
'K07': "SELECT Nganh,count(*) FROM SinhVien GROUP BY Nganh ORDER BY Nganh",
'K08': "SELECT l.MaLoaiPhong,l.TenLoai,count(p.MaPhong) FROM LoaiPhong l LEFT JOIN Phong p ON p.LoaiPhong_MaLoaiPhong=l.MaLoaiPhong GROUP BY l.MaLoaiPhong,l.TenLoai ORDER BY l.MaLoaiPhong",
'K09': "WITH x AS (SELECT DISTINCT SinhVien_MaSV,HoTen,Giuong_MaGiuong FROM residence WHERE TrangThai<>'DA_HUY' AND NgayBatDau<='2026-10-19' ORDER BY SinhVien_MaSV,Giuong_MaGiuong) SELECT SinhVien_MaSV,HoTen,count(*),group_concat(Giuong_MaGiuong,', ') FROM x GROUP BY SinhVien_MaSV,HoTen HAVING count(*)>1 ORDER BY SinhVien_MaSV",
'K10': "WITH x AS (SELECT n.MaNV,n.HoTen,coalesce(sum(t.SoTien),0) total FROM NhanVien n LEFT JOIN ThanhToan t ON t.NhanVien_MaNV=n.MaNV GROUP BY n.MaNV,n.HoTen) SELECT * FROM x WHERE total=(SELECT max(total) FROM x) ORDER BY MaNV",
'N01': "SELECT p.MaPhong,p.SoPhong,t.TenToa,count(a.MaGiuong) FROM Phong p JOIN ToaNha t ON t.MaToa=p.ToaNha_MaToa JOIN available a ON a.Phong_MaPhong=p.MaPhong GROUP BY p.MaPhong,p.SoPhong,t.TenToa ORDER BY p.MaPhong",
'N02': "SELECT * FROM SinhVien WHERE MaSV='SV01' ORDER BY MaSV",
'N03': "SELECT 'P01',10,2026,count(*),coalesce(sum(TienPhong),0),coalesce(sum(TienDien),0),coalesce(sum(TienNuoc),0),coalesce(sum(TongTien),0) FROM bills WHERE MaPhong='P01' AND Thang='10' AND Nam='2026'",
'N04': "SELECT 'DA_TRA_DU',count(*) FROM HoaDon WHERE cast(ConNo AS REAL)<=0 UNION ALL SELECT 'TRA_MOT_PHAN',count(*) FROM HoaDon WHERE cast(ConNo AS REAL)>0 AND cast(TongDaTra AS REAL)>0 UNION ALL SELECT 'CHUA_TRA',count(*) FROM HoaDon WHERE cast(ConNo AS REAL)>0 AND cast(TongDaTra AS REAL)<=0",
'N05': "WITH q(n) AS (VALUES(1),(2),(3),(4)) SELECT 2026,q.n,coalesce(sum(h.TienPhong),0),coalesce(sum(cast(h.TienDien AS REAL)+cast(h.TienNuoc AS REAL)),0),coalesce(sum(h.TongTien),0) FROM q LEFT JOIN HoaDon h ON (cast(h.Thang AS INT)-1)/3+1=q.n AND h.Nam='2026' GROUP BY q.n ORDER BY q.n",
'N06': "SELECT Giuong_MaGiuong,SinhVien_MaSV,HoTen,Sdt,Email FROM active WHERE Giuong_MaGiuong='G01' ORDER BY SinhVien_MaSV",
'N07': "SELECT p.MaPhong,p.SoPhong,t.TenToa,p.TrangThai FROM Phong p JOIN ToaNha t ON p.ToaNha_MaToa=t.MaToa WHERE p.TrangThai='CON_CHO' ORDER BY p.MaPhong",
'N08': "SELECT MaHD,SinhVien_MaSV,HoTen,Giuong_MaGiuong,NgayBatDau,NgayKetThuc,TienCoc,TrangThai,NhanVienLap_MaNV FROM residence ORDER BY MaHD",
'N09': "SELECT MaHoaDon,MaSV,Thang,Nam,TongTien,TongDaTra,ConNo FROM bills WHERE cast(TongDaTra AS REAL)<=0 AND cast(ConNo AS REAL)>0 ORDER BY MaHoaDon",
'N10': "SELECT p.MaPhong,t.TenToa,coalesce(sum(b.TienDien),0) total FROM Phong p JOIN ToaNha t ON p.ToaNha_MaToa=t.MaToa LEFT JOIN bills b ON b.MaPhong=p.MaPhong GROUP BY p.MaPhong,t.TenToa ORDER BY total DESC,p.MaPhong LIMIT 3",
'L01': "SELECT p.MaPhong,p.SoPhong,p.Tang,t.TenToa,p.TrangThai FROM Phong p JOIN ToaNha t ON p.ToaNha_MaToa=t.MaToa WHERE t.TenToa='A' AND p.Tang='1' ORDER BY p.MaPhong",
'L02': "SELECT MaHD,SinhVien_MaSV,Giuong_MaGiuong,NgayBatDau,NgayKetThuc,TrangThai FROM HopDong WHERE TrangThai='DA_HUY' ORDER BY MaHD",
'L03': "WITH x AS (SELECT s.MaSV,s.HoTen,count(h.MaHD) n FROM SinhVien s LEFT JOIN HopDong h ON h.SinhVien_MaSV=s.MaSV GROUP BY s.MaSV,s.HoTen) SELECT * FROM x WHERE n=(SELECT max(n) FROM x) ORDER BY MaSV",
'L04': "SELECT p.MaPhong,p.SoPhong,t.TenToa,p.TrangThai FROM Phong p JOIN ToaNha t ON p.ToaNha_MaToa=t.MaToa WHERE p.TrangThai='DANG_SUA_CHUA' ORDER BY p.MaPhong",
'L05': "SELECT t.MaToa,t.TenToa,2026,coalesce(sum(r.SoTien),0) total FROM ToaNha t LEFT JOIN receipts r ON r.MaToa=t.MaToa AND substr(r.NgayThanhToan,1,4)='2026' GROUP BY t.MaToa,t.TenToa ORDER BY total DESC,t.MaToa",
'L06': "SELECT n.MaNV,n.HoTen,count(h.MaHD) total FROM NhanVien n LEFT JOIN HopDong h ON h.NhanVienLap_MaNV=n.MaNV GROUP BY n.MaNV,n.HoTen ORDER BY total DESC,n.MaNV",
'L07': "WITH x AS (SELECT * FROM NhanVien ORDER BY MaNV) SELECT ChucVu,count(*),group_concat(MaNV || ' - ' || HoTen,'; ') FROM x GROUP BY ChucVu ORDER BY CASE ChucVu WHEN 'QUAN_LY' THEN 0 WHEN 'KE_TOAN' THEN 1 ELSE 2 END",
'L08': "SELECT p.MaPhong,t.TenToa,min(a.MaGiuong) FROM Phong p JOIN ToaNha t ON p.ToaNha_MaToa=t.MaToa JOIN available a ON a.Phong_MaPhong=p.MaPhong GROUP BY p.MaPhong,t.TenToa HAVING count(*)=1 ORDER BY p.MaPhong",
'L09': "SELECT GioiTinh,count(*) FROM SinhVien GROUP BY GioiTinh ORDER BY GioiTinh",
'L10': "SELECT count(*),avg(julianday(NgayKetThuc)-julianday(NgayBatDau)+1) FROM HopDong WHERE TrangThai='HET_HAN' AND NgayKetThuc<'2026-10-19'"
}
def equal(a, b):
    if a is None: return b == '—'
    if str(a) == b: return True
    try: return math.isclose(float(a), float(b), rel_tol=1e-10, abs_tol=1e-7)
    except (ValueError, TypeError): return False

failures = []
for id, sql in SQL.items():
    expected = con.execute(sql).fetchall()
    with (query_dir / (id+'.tsv')).open(encoding='utf-8-sig', newline='') as f:
        actual = list(csv.reader(f, delimiter='\t'))[1:]
    # TSV deliberately emits a trailing blank row for zero-result output.
    actual = [row for row in actual if row]
    ok = len(expected)==len(actual) and all(len(e)==len(a) and all(equal(x,y) for x,y in zip(e,a)) for e,a in zip(expected,actual))
    print(f'{"PASS" if ok else "FAIL"} {id}: {len(actual)} rows vs {len(expected)} SQL rows')
    if not ok: failures.append(id)
print(f'SQL comparison: {30-len(failures)}/30 passed; failed={failures}')
sys.exit(bool(failures))
