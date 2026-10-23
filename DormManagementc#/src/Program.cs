using System.Text;
using System.Text.Json;
using DormManagement;
using DormManagement.DAO;
using DormManagement.Data;
using DormManagement.Queries;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
try { Environment.ExitCode = Run(args); }
catch (Exception ex) when (ex is ArgumentException or IOException or UnauthorizedAccessException)
{
    Console.Error.WriteLine("Lỗi: " + ex.Message);
    Environment.ExitCode = 1;
}

static int Run(string[] args)
{
    if (args.Length == 1 && args[0] == "--self-test")
    {
        int core = SelfTests.Run();
        int queries = QueryTests.Run();
        return core == 0 && queries == 0 ? 0 : 1;
    }
    if (args.Length == 1 && args[0] == "--help") { Help(); return 0; }
    var db = new DormContext();
    var catalog = new QueryCatalog(db);
    if (args.Length == 0) { Menu(db, catalog); return 0; }
    if (args.Length == 1 && args[0] == "--summary") { Summary(db); return 0; }
    if (args.Length == 2 && args[0] == "--export") { Snapshot.Write(db, args[1]); return 0; }
    if (args.Length == 2 && args[0] == "--export-queries")
    {
        var results = catalog.RunAll(new());
        Directory.CreateDirectory(args[1]);
        foreach (var r in results) File.WriteAllText(Path.Combine(args[1], r.Id + ".tsv"), r.ToTsv(), new UTF8Encoding(false));
        File.WriteAllText(Path.Combine(args[1], "queries.json"), JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true, Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() } }), new UTF8Encoding(false));
        Console.WriteLine($"Đã xuất 30 truy vấn với tham số mặc định vào {Path.GetFullPath(args[1])}");
        return 0;
    }
    if (args[0] is "--all" or "--query")
    {
        bool all = args[0] == "--all";
        if (!all && args.Length < 2) throw new ArgumentException("Thiếu mã truy vấn sau --query.");
        var p = Parse(args.Skip(all ? 1 : 2).ToArray());
        foreach (var r in all ? catalog.RunAll(p) : new[] { catalog.Run(args[1], p) }) r.Print();
        return 0;
    }
    throw new ArgumentException("Lệnh không hợp lệ. Dùng --help để xem hướng dẫn.");
}

static QueryParameters Parse(string[] args)
{
    var p = new QueryParameters();
    if (args.Length % 2 != 0) throw new ArgumentException("Mỗi tùy chọn cần một giá trị.");
    var seen = new HashSet<string>();
    for (int i = 0; i < args.Length; i += 2)
    {
        string key = args[i], value = args[i + 1].Trim();
        if (!seen.Add(key)) throw new ArgumentException("Tùy chọn bị lặp: " + key);
        if (value.Length == 0) throw new ArgumentException("Giá trị không được rỗng: " + key);
        int Number(int min, int max) => int.TryParse(value, out int x) && x >= min && x <= max
            ? x : throw new ArgumentException($"{key} phải là số nguyên từ {min} đến {max}.");
        p = key switch {
            "--toa" => p with { Toa = value }, "--phong" => p with { Phong = value },
            "--giuong" => p with { Giuong = value }, "--keyword" => p with { TuKhoa = value },
            "--tang" => p with { Tang = Number(1, 100) }, "--thang" => p with { Thang = Number(1, 12) },
            "--nam" => p with { Nam = Number(1, 9999) },
            _ => throw new ArgumentException("Tùy chọn không hợp lệ: " + key)
        };
    }
    return p;
}

static void Summary(DormContext db)
{
    Console.WriteLine($"DORM MANAGEMENT | {DatasetConfig.Version} | Mốc dữ liệu {DatasetConfig.ReferenceDate:yyyy-MM-dd}");
    foreach (var line in Snapshot.Summary(db)) Console.WriteLine(line);
}
static void Help() => Console.WriteLine("""
Dorm Management System — C# / LINQ
  dotnet run                              Menu tương tác
  dotnet run -- --summary                  Thống kê dữ liệu
  dotnet run -- --all                      Chạy 30 câu với tham số mặc định
  dotnet run -- --query K01 --toa A         Một câu truy vấn
  dotnet run -- --query N03 --phong P01 --thang 10 --nam 2026
  dotnet run -- --query N02 --keyword "Nguyễn"
  dotnet run -- --self-test                Kiểm tra model/DAO/dữ liệu và LINQ
  dotnet run -- --export output/data       Xuất dữ liệu gốc TSV
  dotnet run -- --export-queries output/queries  Xuất 30 kết quả TSV + JSON
Tham số dùng với --query/--all: --toa --phong --giuong --keyword --tang --thang --nam.
Mặc định: A, P01, G01, SV01, tầng 1, tháng 10, năm 2026.
Ngày tham chiếu cố định 19/10/2026 để so sánh với Java.
""");

static void Menu(DormContext db, QueryCatalog catalog)
{
    Summary(db);
    Console.WriteLine("\nK = Kiều, N = Nhung, L = Lộc. Các truy vấn:");
    foreach (var entry in catalog.Entries) Console.WriteLine($"  {entry.Id}: {entry.Description}");
    while (true)
    {
        Console.Write("\nNhập mã câu / ALL / S (thống kê) / 0 (thoát): ");
        var input = Console.ReadLine()?.Trim().ToUpperInvariant();
        if (input is null or "0") return;
        try
        {
            if (input == "S") { Summary(db); continue; }
            if (input == "ALL") { foreach (var r in catalog.RunAll(new())) r.Print(); continue; }
            if (!catalog.Entries.Any(e => e.Id == input)) { Console.WriteLine("Mã không hợp lệ."); continue; }
            string Ask(string label, string fallback)
            {
                Console.Write($"{label} [{fallback}]: ");
                var value = Console.ReadLine();
                return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
            }
            var options = new List<string>();
            void Option(string key, string label, string fallback) { options.Add(key); options.Add(Ask(label, fallback)); }
            if (input is "K01" or "L01") Option("--toa", "Mã/tên tòa", "A");
            if (input is "K02" or "N03") Option("--phong", "Mã phòng", "P01");
            if (input == "N06") Option("--giuong", "Mã giường", "G01");
            if (input == "N02") Option("--keyword", "Mã hoặc một phần họ tên (có dấu)", "SV01");
            if (input == "L01") Option("--tang", "Tầng", "1");
            if (input == "N03") Option("--thang", "Tháng", "10");
            if (input is "K04" or "N03" or "N05" or "L05") Option("--nam", "Năm", "2026");
            catalog.Run(input, Parse(options.ToArray())).Print();
        }
        catch (ArgumentException ex) { Console.WriteLine("Lỗi nhập liệu: " + ex.Message); }
    }
}
