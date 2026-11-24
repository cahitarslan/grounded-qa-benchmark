using System.Text.Json;
using System.Text.RegularExpressions;

namespace GroundedQa;

public sealed record BenchmarkOptions(
    IReadOnlyList<string> Models,
    string JudgeModel,
    string Effort,
    int TopK,
    int Parallelism,
    string OutputDirectory);

/// <summary>Asks every question in both modes for every model, then grades each answer with a judge model.</summary>
public sealed partial class Benchmark(IChatModel chat, Bm25Index index, IReadOnlyList<Question> questions, BenchmarkOptions options)
{
    private string AnswersPath => Path.Combine(options.OutputDirectory, "answers.jsonl");
    private string GradesPath => Path.Combine(options.OutputDirectory, "grades.jsonl");

    public async Task<(List<Answer> Answers, List<Grade> Grades)> RunAsync(CancellationToken ct)
    {
        Directory.CreateDirectory(options.OutputDirectory);
        var answers = JsonLines.Read<Answer>(AnswersPath);
        var done = answers.Select(a => (a.QuestionId, a.Model, a.Mode)).ToHashSet();

        var jobs = from model in options.Models
                   from mode in new[] { Mode.Plain, Mode.Grounded }
                   from q in questions
                   where !done.Contains((q.Id, model, mode))
                   select (q, model, mode);

        await ForEachAsync(jobs, async (job, token) =>
        {
            var answer = await AskAsync(job.q, job.model, job.mode, token);
            await JsonLines.AppendAsync(AnswersPath, answer, token);
            lock (answers) answers.Add(answer);
            Console.WriteLine($"answer  {job.model,-20} {job.mode,-8} {job.q.Id}");
        }, ct);

        var grades = JsonLines.Read<Grade>(GradesPath);
        var graded = grades.Select(g => (g.QuestionId, g.Model, g.Mode)).ToHashSet();
        var byId = questions.ToDictionary(q => q.Id);

        await ForEachAsync(answers.Where(a => !graded.Contains((a.QuestionId, a.Model, a.Mode))), async (a, token) =>
        {
            var grade = await JudgeAsync(byId[a.QuestionId], a, token);
            await JsonLines.AppendAsync(GradesPath, grade, token);
            lock (grades) grades.Add(grade);
            Console.WriteLine($"grade   {a.Model,-20} {a.Mode,-8} {a.QuestionId} {grade.Verdict}");
        }, ct);

        return (answers, grades);
    }

    private async Task<Answer> AskAsync(Question q, string model, Mode mode, CancellationToken ct)
    {
        string[] retrieved = [];
        string system = Prompts.PlainSystem, user = q.Text;
        if (mode == Mode.Grounded)
        {
            var hits = index.Search(q.Text, options.TopK);
            retrieved = hits.Select(h => h.Article.Id).ToArray();
            system = Prompts.GroundedSystem;
            user = Prompts.GroundedUser(q.Text, hits.Select(h => h.Article));
        }
        var reply = await chat.CompleteAsync(model, system, user, options.Effort, 2048, ct);
        return new Answer(q.Id, model, mode, reply.Text, retrieved, reply.InputTokens, reply.OutputTokens, reply.LatencyMs);
    }

    private async Task<Grade> JudgeAsync(Question q, Answer a, CancellationToken ct)
    {
        var reply = await chat.CompleteAsync(options.JudgeModel, Prompts.JudgeSystem, Prompts.JudgeUser(q, a.Text), "medium", 1024, ct);
        var (verdict, reason) = ParseVerdict(reply.Text);
        // Trap questions have no "abstained" bucket: declining is the right behaviour there.
        if (!q.Answerable && verdict == Verdict.Abstained) verdict = Verdict.Correct;
        return new Grade(q.Id, a.Model, a.Mode, verdict, reason);
    }

    [GeneratedRegex(@"\{.*\}", RegexOptions.Singleline)]
    private static partial Regex JsonObject();

    internal static (Verdict Verdict, string Reason) ParseVerdict(string text)
    {
        var match = JsonObject().Match(text);
        if (!match.Success) throw new FormatException($"Judge did not return JSON: {text}");
        using var doc = JsonDocument.Parse(match.Value);
        var raw = doc.RootElement.GetProperty("verdict").GetString();
        var reason = doc.RootElement.TryGetProperty("reason", out var r) ? r.GetString() ?? "" : "";
        return raw?.Trim().ToLowerInvariant() switch
        {
            "correct" => (Verdict.Correct, reason),
            "incorrect" => (Verdict.Incorrect, reason),
            "abstained" => (Verdict.Abstained, reason),
            _ => throw new FormatException($"Unknown verdict '{raw}'.")
        };
    }

    private Task ForEachAsync<T>(IEnumerable<T> items, Func<T, CancellationToken, Task> body, CancellationToken ct) =>
        Parallel.ForEachAsync(items, new ParallelOptions { MaxDegreeOfParallelism = options.Parallelism, CancellationToken = ct },
            async (item, token) => await body(item, token));
}
