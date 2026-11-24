namespace GroundedQa;

/// <summary>
/// Okapi BM25 over articles. Small, in-memory and deterministic, so anyone can re-run the benchmark
/// without an embedding service. k1/b are the textbook defaults.
/// </summary>
public sealed class Bm25Index
{
    private const double K1 = 1.2;
    private const double B = 0.75;

    private readonly IReadOnlyList<Article> _articles;
    private readonly List<Dictionary<string, int>> _termFreqs = [];
    private readonly Dictionary<string, int> _docFreq = new(StringComparer.Ordinal);
    private readonly int[] _lengths;
    private readonly double _avgLength;

    public Bm25Index(IReadOnlyList<Article> articles)
    {
        ArgumentNullException.ThrowIfNull(articles);
        if (articles.Count == 0) throw new ArgumentException("Corpus is empty.", nameof(articles));
        _articles = articles;
        _lengths = new int[articles.Count];

        for (var i = 0; i < articles.Count; i++)
        {
            var tf = new Dictionary<string, int>(StringComparer.Ordinal);
            // The heading is indexed twice: it is a short, dense summary of the article ("Cayma hakkı").
            var a = articles[i];
            foreach (var term in TurkishText.Terms($"{a.Heading} {a.Heading} {a.Text}"))
            {
                tf[term] = tf.GetValueOrDefault(term) + 1;
                _lengths[i]++;
            }
            foreach (var term in tf.Keys) _docFreq[term] = _docFreq.GetValueOrDefault(term) + 1;
            _termFreqs.Add(tf);
        }
        _avgLength = _lengths.Average();
    }

    public IReadOnlyList<(Article Article, double Score)> Search(string query, int k)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(k, 1);
        var terms = TurkishText.Terms(query).Distinct().ToArray();
        var n = _articles.Count;
        var scores = new double[n];

        foreach (var term in terms)
        {
            if (!_docFreq.TryGetValue(term, out var df)) continue;
            var idf = Math.Log(1 + (n - df + 0.5) / (df + 0.5));
            for (var i = 0; i < n; i++)
            {
                if (!_termFreqs[i].TryGetValue(term, out var f)) continue;
                scores[i] += idf * f * (K1 + 1) / (f + K1 * (1 - B + B * _lengths[i] / _avgLength));
            }
        }

        return Enumerable.Range(0, n)
            .Where(i => scores[i] > 0)
            .OrderByDescending(i => scores[i])
            .ThenBy(i => i) // stable order for ties keeps runs reproducible
            .Take(k)
            .Select(i => (_articles[i], scores[i]))
            .ToList();
    }
}
