using System.Text.RegularExpressions;

namespace GroundedQa;

/// <summary>
/// Loads the corpus: one Markdown file per legal text, where the first line is "# &lt;title&gt;" and a
/// "&lt;!-- id: 6502 --&gt;" comment gives the short code used in citations. Every article starts with
/// "## Madde N - Heading". Anything before the first article (preamble) is ignored.
/// </summary>
public static partial class Corpus
{
    [GeneratedRegex(@"^<!--\s*id:\s*(?<id>[\w-]+)\s*-->\s*$", RegexOptions.Multiline)]
    private static partial Regex IdComment();

    [GeneratedRegex(@"^##\s+Madde\s+(?<no>[\w/]+)\s*[-–—:]?\s*(?<heading>.*)$", RegexOptions.Multiline)]
    private static partial Regex ArticleHeading();

    public static IReadOnlyList<Article> Load(string directory)
    {
        if (!Directory.Exists(directory)) throw new DirectoryNotFoundException($"Corpus folder not found: {directory}");
        var articles = new List<Article>();
        foreach (var path in Directory.EnumerateFiles(directory, "*.md").Order(StringComparer.Ordinal))
            articles.AddRange(Parse(File.ReadAllText(path), Path.GetFileNameWithoutExtension(path)));
        if (articles.Count == 0) throw new InvalidDataException($"No articles found under {directory}.");
        var duplicates = articles.GroupBy(a => a.Id).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
        if (duplicates.Length > 0) throw new InvalidDataException($"Duplicate article ids: {string.Join(", ", duplicates)}");
        return articles;
    }

    internal static IEnumerable<Article> Parse(string markdown, string fallbackId)
    {
        var docId = IdComment().Match(markdown) is { Success: true } m ? m.Groups["id"].Value : fallbackId;
        var title = markdown.Split('\n', 2)[0].TrimStart('#', ' ').Trim();
        var heads = ArticleHeading().Matches(markdown);
        for (var i = 0; i < heads.Count; i++)
        {
            var start = heads[i].Index + heads[i].Length;
            var end = i + 1 < heads.Count ? heads[i + 1].Index : markdown.Length;
            var body = markdown[start..end].Trim();
            if (body.Length == 0) continue;
            var no = heads[i].Groups["no"].Value;
            yield return new Article($"{docId}/m.{no}", title, heads[i].Groups["heading"].Value.Trim(), body);
        }
    }
}
