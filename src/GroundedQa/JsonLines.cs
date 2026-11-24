using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GroundedQa;

/// <summary>Results are stored as JSON Lines: append-only, so an interrupted run resumes where it stopped.</summary>
public static class JsonLines
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // keep Turkish letters readable in the files
        WriteIndented = false,
    };

    public static List<T> Read<T>(string path) =>
        File.Exists(path)
            ? File.ReadLines(path).Where(l => l.Length > 0).Select(l => JsonSerializer.Deserialize<T>(l, Options)!).ToList()
            : [];

    private static readonly SemaphoreSlim WriteLock = new(1, 1);

    public static async Task AppendAsync<T>(string path, T item, CancellationToken ct)
    {
        var line = JsonSerializer.Serialize(item, Options) + "\n";
        await WriteLock.WaitAsync(ct);
        try { await File.AppendAllTextAsync(path, line, ct); }
        finally { WriteLock.Release(); }
    }
}
