namespace GroundedQa;

/// <summary>List prices in USD per million tokens (input, output), Claude API, checked 24 Nov 2026.</summary>
public static class Pricing
{
    private static readonly Dictionary<string, (decimal In, decimal Out)> PerMillion = new(StringComparer.Ordinal)
    {
        ["claude-opus-5-5"] = (4.00m, 20.00m),
        ["claude-sonnet-5-5"] = (2.00m, 10.00m),
        ["claude-haiku-5-5"] = (0.10m, 0.50m),
    };

    public static decimal? Cost(string model, long input, long output) =>
        PerMillion.TryGetValue(model, out var p) ? (input * p.In + output * p.Out) / 1_000_000m : null;
}
