using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace GroundedQa;

/// <summary>Turns answers and grades into a Markdown report (tables only; the interpretation goes in the blog post).</summary>
public static partial class Report
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    [GeneratedRegex(@"\[(?<id>[\w-]+/m\.[\w/]+)\]")]
    private static partial Regex CitationTag();

    public static string Build(IReadOnlyList<Question> questions, IReadOnlyList<Answer> answers, IReadOnlyList<Grade> grades, BenchmarkOptions o, double recallAtK)
    {
        var q = questions.ToDictionary(x => x.Id);
        var sb = new StringBuilder();
        var answerable = questions.Count(x => x.Answerable);
        sb.AppendLine("# Results").AppendLine()
          .AppendLine(Inv, $"- Questions: {questions.Count} ({answerable} answerable, {questions.Count - answerable} traps)")
          .AppendLine(Inv, $"- Models: {string.Join(", ", o.Models)}; judge: {o.JudgeModel}; effort: {o.Effort}; top-k: {o.TopK}")
          .AppendLine(Inv, $"- Retrieval recall@{o.TopK} (a reference article is among the retrieved ones): {recallAtK:P0}")
          .AppendLine();

        sb.AppendLine("| Model | Mode | Answerable: correct | Answerable: wrong | Answerable: declined | Traps: caught | Traps: made up | Correct citation | Avg input tokens | Avg latency (ms) | Cost (USD) |")
          .AppendLine("|---|---|--:|--:|--:|--:|--:|--:|--:|--:|--:|");

        foreach (var group in grades.GroupBy(g => (g.Model, g.Mode)).OrderBy(g => g.Key.Model).ThenBy(g => g.Key.Mode))
        {
            var ans = group.Where(g => q[g.QuestionId].Answerable).ToList();
            var traps = group.Where(g => !q[g.QuestionId].Answerable).ToList();
            var runs = answers.Where(a => a.Model == group.Key.Model && a.Mode == group.Key.Mode).ToList();
            var cost = runs.Sum(a => Pricing.Cost(a.Model, a.InputTokens, a.OutputTokens) ?? 0m);
            sb.AppendLine(Inv,
                $"| {group.Key.Model} | {group.Key.Mode} | {Count(ans, Verdict.Correct)} | {Count(ans, Verdict.Incorrect)} | {Count(ans, Verdict.Abstained)} " +
                $"| {Count(traps, Verdict.Correct)} | {Count(traps, Verdict.Incorrect)} | {CitationAccuracy(runs, q)} " +
                $"| {runs.Average(a => a.InputTokens):F0} | {runs.Average(a => a.LatencyMs):F0} | {cost:F3} |");
        }

        sb.AppendLine().AppendLine("## Wrong or made-up answers").AppendLine();
        foreach (var g in grades.Where(g => g.Verdict == Verdict.Incorrect).OrderBy(g => g.QuestionId).ThenBy(g => g.Model).ThenBy(g => g.Mode))
        {
            var a = answers.First(x => x.QuestionId == g.QuestionId && x.Model == g.Model && x.Mode == g.Mode);
            sb.AppendLine(Inv, $"### {g.QuestionId} · {g.Model} · {g.Mode}").AppendLine()
              .AppendLine(Inv, $"**Question:** {q[g.QuestionId].Text}  ").AppendLine(Inv, $"**Reference:** {q[g.QuestionId].Gold}  ")
              .AppendLine(Inv, $"**Answer:** {a.Text.ReplaceLineEndings(" ")}  ").AppendLine(Inv, $"**Judge:** {g.Reason}").AppendLine();
        }
        return sb.ToString();
    }

    private static string Count(List<Grade> grades, Verdict v) => $"{grades.Count(g => g.Verdict == v)}/{grades.Count}";

    /// <summary>Share of grounded answers to answerable questions whose citations include at least one reference article.</summary>
    private static string CitationAccuracy(List<Answer> runs, Dictionary<string, Question> q)
    {
        var relevant = runs.Where(a => a.Mode == Mode.Grounded && q[a.QuestionId].Answerable).ToList();
        if (relevant.Count == 0) return "-";
        var ok = relevant.Count(a => CitationTag().Matches(a.Text).Select(m => m.Groups["id"].Value).Intersect(q[a.QuestionId].Refs).Any());
        return $"{ok}/{relevant.Count}";
    }

    public static double RecallAtK(Bm25Index index, IEnumerable<Question> questions, int k)
    {
        var withRefs = questions.Where(x => x.Answerable && x.Refs.Length > 0).ToList();
        if (withRefs.Count == 0) return 0;
        return withRefs.Count(x => index.Search(x.Text, k).Any(h => x.Refs.Contains(h.Article.Id))) / (double)withRefs.Count;
    }
}
