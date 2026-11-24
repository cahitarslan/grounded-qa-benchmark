namespace GroundedQa;

/// <summary>One article (madde) of a legal text. Retrieval works at article level because that is how the law is cited.</summary>
public sealed record Article(string Id, string Document, string Heading, string Text)
{
    public string Citation => Id; // e.g. "6502/m.48"
}

/// <summary>
/// A benchmark question. <see cref="Answerable"/> is false for trap questions: a non-existent article,
/// a false premise, or a fact that is not in the corpus. For those the only good answer is "the source does not say this".
/// </summary>
public sealed record Question(string Id, string Text, bool Answerable, string Gold, string[] Refs, string Category);

public enum Mode { Plain, Grounded }

public sealed record Answer(
    string QuestionId,
    string Model,
    Mode Mode,
    string Text,
    string[] RetrievedIds,
    long InputTokens,
    long OutputTokens,
    long LatencyMs);

public enum Verdict { Correct, Incorrect, Abstained }

public sealed record Grade(string QuestionId, string Model, Mode Mode, Verdict Verdict, string Reason);
