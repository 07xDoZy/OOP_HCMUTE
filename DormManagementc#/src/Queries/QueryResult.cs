using System.Globalization;
using System.Text;

namespace DormManagement.Queries;

// Chỉ chứa các giá trị đơn để in/xuất, không đưa đồ thị model hai chiều vào JSON.
public sealed record QueryResult(string Id, string Title, string[] Columns, object?[][] Rows)
{
    public static QueryResult From<T>(string id, string title, IEnumerable<T> data)
    {
        var properties = typeof(T).GetProperties();
        return new(id, title, properties.Select(p => p.Name).ToArray(),
            data.Select(item => properties.Select(p => p.GetValue(item)).ToArray()).ToArray());
    }
    public static string Format(object? value) => value switch
    {
        null => "—",
        DateTime d => d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        double n => n.ToString("0.########", CultureInfo.InvariantCulture),
        _ => Convert.ToString(value, CultureInfo.InvariantCulture) ?? ""
    };
    private static string Clean(string value) => value.Replace("\t", " ").Replace("\r", " ").Replace("\n", " ");
    public string ToTsv() => string.Join("\t", Columns) + "\n" +
        string.Join("\n", Rows.Select(r => string.Join("\t", r.Select(v => Clean(Format(v)))))) + "\n";
    public void Print()
    {
        Console.WriteLine($"\n[{Id}] {Title}");
        Console.Write(ToTsv());
        Console.WriteLine($"({Rows.Length} dòng)");
    }
}
