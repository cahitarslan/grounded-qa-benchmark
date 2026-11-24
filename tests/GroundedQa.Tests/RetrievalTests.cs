using GroundedQa;
using Xunit;

namespace GroundedQa.Tests;

public class TurkishTextTests
{
    [Fact]
    public void Lowercases_with_Turkish_rules_and_stems_to_five_letters()
    {
        Assert.Equal(["iades", "ıslak", "hakkı"], TurkishText.Terms("İADESİNDEN ISLAKLIK hakkı").ToArray());
    }

    [Fact]
    public void Drops_stop_words_and_keeps_numbers_whole()
    {
        Assert.Equal(["14", "günlü", "süre"], TurkishText.Terms("ve 14 günlük bir süre").ToArray());
    }
}

public class CorpusTests
{
    private const string Sample = """
        # Tüketicinin Korunması Hakkında Kanun
        <!-- id: 6502 -->
        Giriş metni.
        ## Madde 48 - Mesafeli sözleşmeler
        Tüketici on dört gün içinde cayma hakkına sahiptir.
        ## Madde 49 - Finansal hizmetler
        Finansal hizmetlere ilişkin hükümler.
        """;

    [Fact]
    public void Splits_markdown_into_articles_with_citation_ids()
    {
        var articles = Corpus.Parse(Sample, "fallback").ToList();
        Assert.Equal(["6502/m.48", "6502/m.49"], articles.Select(a => a.Id));
        Assert.Equal("Mesafeli sözleşmeler", articles[0].Heading);
        Assert.Contains("on dört gün", articles[0].Text);
    }

    [Fact]
    public void Bm25_ranks_the_article_that_matches_inflected_words_first()
    {
        var index = new Bm25Index(Corpus.Parse(Sample, "x").ToList());
        var top = index.Search("Cayma hakkımı kaç günde kullanırım?", 1).Single();
        Assert.Equal("6502/m.48", top.Article.Id);
    }

    [Fact]
    public void Bm25_returns_nothing_for_unrelated_query()
    {
        var index = new Bm25Index(Corpus.Parse(Sample, "x").ToList());
        Assert.Empty(index.Search("uzay gemisi", 3));
    }
}

public class JudgeParsingTests
{
    [Theory]
    [InlineData("{\"verdict\":\"correct\",\"reason\":\"ok\"}", Verdict.Correct)]
    [InlineData("Sure.\n{\"verdict\": \"INCORRECT\", \"reason\": \"wrong period\"}", Verdict.Incorrect)]
    [InlineData("{\"verdict\":\"abstained\"}", Verdict.Abstained)]
    public void Parses_judge_json(string text, Verdict expected)
    {
        Assert.Equal(expected, Benchmark.ParseVerdict(text).Verdict);
    }

    [Fact]
    public void Rejects_unknown_verdict()
    {
        Assert.Throws<FormatException>(() => Benchmark.ParseVerdict("{\"verdict\":\"maybe\"}"));
    }
}
