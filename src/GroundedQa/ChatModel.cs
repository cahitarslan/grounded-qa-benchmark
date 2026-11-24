using System.Diagnostics;
using Anthropic;
using Anthropic.Models.Messages;

namespace GroundedQa;

public sealed record ModelReply(string Text, long InputTokens, long OutputTokens, long LatencyMs);

public interface IChatModel
{
    Task<ModelReply> CompleteAsync(string model, string system, string user, string effort, int maxTokens, CancellationToken ct);
}

/// <summary>
/// Thin wrapper over the official Anthropic SDK. The SDK already retries 429/5xx with backoff;
/// we only add a timing measurement and turn refusals into an explicit error instead of an empty answer.
/// </summary>
public sealed class AnthropicChatModel : IChatModel
{
    private readonly AnthropicClient _client = new(); // reads ANTHROPIC_API_KEY

    public async Task<ModelReply> CompleteAsync(string model, string system, string user, string effort, int maxTokens, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        var response = await _client.Messages.Create(new MessageCreateParams
        {
            Model = model,
            MaxTokens = maxTokens,
            System = system,
            OutputConfig = new OutputConfig { Effort = effort switch
            {
                "low" => Effort.Low,
                "medium" => Effort.Medium,
                "high" => Effort.High,
                _ => throw new ArgumentOutOfRangeException(nameof(effort), effort, "Use low, medium or high.")
            } },
            Messages = [new UserMessageParam { Content = user }],
        }, cancellationToken: ct);
        sw.Stop();

        if (response.StopReason == StopReason.Refusal)
            throw new InvalidOperationException($"Model {model} refused the request.");

        var text = string.Concat(response.Content.Select(b => b.Value).OfType<TextBlock>().Select(t => t.Text)).Trim();
        return new ModelReply(text, response.Usage.InputTokens, response.Usage.OutputTokens, sw.ElapsedMilliseconds);
    }
}
