[Türkçe](README.md) | **English**

# grounded-qa-benchmark

Does an AI model make things up less when it has to answer from the source text?

This repo measures it on a topic everyone runs into: Turkish consumer law (returns, faulty products, warranties, subscriptions, loans).
The same 38 questions go to the same model twice:

- **Plain:** the model answers from what it knows.
- **Grounded:** a small search engine finds the 5 most relevant articles of the law, the model may only use those and must cite them, and it must say so when the answer is not there.

28 questions have a clear answer in the law. 10 are **traps**: an article that does not exist, a false premise ("discounted items have a 7-day return period, right?"), or a figure that is not in the law at all.
A trap is caught when the answer says the rule does not exist or is not in the sources.

Both modes get the same "say so if you are not sure" instruction, so any difference comes from grounding, not from a stricter prompt.

## Results

See [`results/`](results/). Every answer, every grade and the judge's reason are stored as JSON Lines, so each number in the report can be checked by hand.

## How it works

| Part | File | Notes |
|---|---|---|
| Corpus | `corpus/*.md` | Law No. 6502 and the Distance Contracts Regulation, one `## Madde N` heading per article |
| Search | `Bm25Index.cs`, `TurkishText.cs` | BM25 with Turkish lower-casing and 5-letter prefix stemming, no embedding service needed |
| Prompts | `Prompts.cs` | Same honesty instruction in both modes |
| Run + grade | `Benchmark.cs` | Answers and grades are appended to `.jsonl`, an interrupted run resumes |
| Report | `Report.cs` | Correct / wrong / declined, traps caught, citation accuracy, tokens, latency, cost |

Grading is done by a separate judge model against a reference answer written from the law text. Wrong answers are listed in full in the report so the judge can be checked too.

## Run it

Requires the .NET 10 SDK and an Anthropic API key.

```sh
dotnet test GroundedQa.slnx
dotnet run --project src/GroundedQa -- recall                 # retrieval check, no API calls
dotnet run --project src/GroundedQa -- search "iade süresi"   # see what the search returns
export ANTHROPIC_API_KEY=...
dotnet run --project src/GroundedQa -- run --models claude-opus-5-5,claude-haiku-5-5
```

A full run is 160 answers plus 160 grades and costs under 2 USD at list prices.

## Limits

- One domain, 38 questions, Turkish only. It shows a direction, not a universal rate.
- Grades come from a model. Disagreements were reviewed by hand; see the report.
- Laws change. The corpus is the text as of the date in `corpus/README.md`. This is not legal advice.

## License

Code: MIT. Legal texts in `corpus/` are official texts and not subject to copyright under Article 31 of Law No. 5846.
