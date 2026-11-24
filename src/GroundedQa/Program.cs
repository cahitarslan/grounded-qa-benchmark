using System.Text.Json;
using GroundedQa;

// Usage:
//   dotnet run --project src/GroundedQa -- search "<question>"          retrieval only, no API calls
//   dotnet run --project src/GroundedQa -- recall                        retrieval recall@k over the question set
//   dotnet run --project src/GroundedQa -- run [--models a,b] [--judge m] [--effort low] [--k 5] [--parallel 4] [--out results/<date>]
var root = FindRoot();
var argsList = args.ToList();
var command = argsList.FirstOrDefault() ?? "help";
string Opt(string name, string fallback) => argsList.IndexOf($"--{name}") is var i and >= 0 && i + 1 < argsList.Count ? argsList[i + 1] : fallback;

var articles = Corpus.Load(Path.Combine(root, "corpus"));
var index = new Bm25Index(articles);
var questions = JsonSerializer.Deserialize<List<Question>>(File.ReadAllText(Path.Combine(root, "data", "questions.json")), JsonLines.Options)
                ?? throw new InvalidDataException("questions.json is empty.");
var k = int.Parse(Opt("k", "5"));

switch (command)
{
    case "search":
        foreach (var (a, score) in index.Search(argsList.ElementAtOrDefault(1) ?? "", k))
            Console.WriteLine($"{score,7:F2}  {a.Id,-14} {a.Heading}");
        break;

    case "recall":
        Console.WriteLine($"{articles.Count} articles, {questions.Count} questions, recall@{k} = {Report.RecallAtK(index, questions, k):P1}");
        foreach (var q in questions.Where(q => q.Answerable && !index.Search(q.Text, k).Any(h => q.Refs.Contains(h.Article.Id))))
            Console.WriteLine($"  miss: {q.Id} {q.Text}");
        break;

    case "run":
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY")))
        {
            Console.Error.WriteLine("Set ANTHROPIC_API_KEY first.");
            return 2;
        }
        var options = new BenchmarkOptions(
            Models: Opt("models", "claude-opus-5-5,claude-haiku-5-5").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
            JudgeModel: Opt("judge", "claude-opus-5-5"),
            Effort: Opt("effort", "low"),
            TopK: k,
            Parallelism: int.Parse(Opt("parallel", "4")),
            OutputDirectory: Path.Combine(root, Opt("out", Path.Combine("results", DateTime.UtcNow.ToString("yyyy-MM-dd")))));
        using (var cts = new CancellationTokenSource())
        {
            Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };
            var (answers, grades) = await new Benchmark(new AnthropicChatModel(), index, questions, options).RunAsync(cts.Token);
            var report = Report.Build(questions, answers, grades, options, Report.RecallAtK(index, questions, k));
            File.WriteAllText(Path.Combine(options.OutputDirectory, "report.md"), report);
            Console.WriteLine(report);
        }
        break;

    default:
        Console.WriteLine("Commands: search \"<question>\" | recall | run [--models a,b] [--judge m] [--effort low|medium|high] [--k 5] [--parallel 4] [--out dir]");
        break;
}
return 0;

// The repo root is the folder that holds corpus/ and data/; works from any working directory inside the repo.
static string FindRoot()
{
    for (var dir = new DirectoryInfo(Directory.GetCurrentDirectory()); dir is not null; dir = dir.Parent)
        if (Directory.Exists(Path.Combine(dir.FullName, "corpus")) && Directory.Exists(Path.Combine(dir.FullName, "data")))
            return dir.FullName;
    throw new DirectoryNotFoundException("Run this from inside the repository (corpus/ and data/ not found).");
}
