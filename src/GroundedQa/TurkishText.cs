using System.Globalization;
using System.Text;

namespace GroundedQa;

/// <summary>
/// Turkish-aware text normalisation for retrieval.
/// Turkish is agglutinative ("iade" -> "iadesi", "iadesini", "iadesinden"), so exact word matching misses most hits.
/// Truncating every word to its first five letters ("fixed prefix stemming") is a well-known, dependency-free
/// approach that performs close to a full morphological analyser for Turkish search (Can et al., 2008).
/// </summary>
public static class TurkishText
{
    private static readonly CultureInfo Tr = CultureInfo.GetCultureInfo("tr-TR");
    public const int PrefixLength = 5;

    private static readonly HashSet<string> StopWords = new(StringComparer.Ordinal)
    {
        "ve", "veya", "ile", "bir", "bu", "şu", "o", "da", "de", "ki", "mi", "mı", "mu", "mü", "için", "gibi",
        "olan", "olarak", "ise", "ya", "ne", "kaç", "nasıl", "hangi", "mı?", "ben", "benim", "var", "yok",
        "her", "çok", "daha", "en", "kadar", "sonra", "önce", "göre", "üzere", "dair", "ilişkin",
        // "ürün" is how people say "product" in questions; the law says "mal" and uses "ürün" mostly in
        // product-safety articles, so the word pulls those articles up for everyday shopping questions.
        "ürün", "ürünü", "ürünün", "ürünler", "ürünlerde"
    };

    /// <summary>Lower-cases with Turkish rules (I -> ı, İ -> i), splits on non-letters/digits, drops stop words and stems.</summary>
    public static IEnumerable<string> Terms(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        var sb = new StringBuilder();
        foreach (var ch in text.ToLower(Tr))
        {
            if (char.IsLetterOrDigit(ch)) { sb.Append(ch); continue; }
            if (Flush(sb) is { } term) yield return term;
        }
        if (Flush(sb) is { } last) yield return last;
    }

    private static string? Flush(StringBuilder sb)
    {
        if (sb.Length == 0) return null;
        var word = sb.ToString();
        sb.Clear();
        if (word.Length < 2 || StopWords.Contains(word)) return null;
        return word.Length > PrefixLength && !char.IsDigit(word[0]) ? word[..PrefixLength] : word;
    }
}
