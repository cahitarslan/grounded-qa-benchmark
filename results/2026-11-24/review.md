# Manual review of the judge, 2026-11-24 run

I checked the judge (claude-opus-5-5) against the corpus text, not just against the reference answers. I read every answer graded `incorrect` (22), every answer graded `abstained` (5), and 10 randomly picked answers graded `correct` (seed 20261124). `grades.jsonl` is left unchanged; this file only records where I disagree.

## Verdicts I disagree with

| Question | Model · mode | Judge | My call | Why |
|---|---|---|---|---|
| q07 | claude-haiku-5-5 · plain | incorrect | correct | The answer gets the right result: no right of withdrawal for goods made to the consumer's specifications. Its only error is the sub-paragraph it cites, (ğ) instead of (b) of MSY m.15/1. The judge rubric counts numbers, periods and rights as key facts. Citations are not scored in plain mode. |
| q19 | claude-haiku-5-5 · plain | incorrect | correct (borderline) | The key fact is right: early repayment gives a discount on interest and other costs (6502 m.27). Two errors sit on top of it. The answer cites the wrong article (m.83) and vaguely points to "erken ödeme komisyonu sınırları" in a regulation, but it gives no concrete number. The judge's own reason also names the wrong article (m.31; the correct one is m.27). |

If both changes are applied, claude-haiku-5-5 in plain mode goes from 12/28 correct and 15/28 wrong to 14/28 correct and 13/28 wrong. No other cell of the summary table changes.

## Verdicts I agree with, but the judge's reason is wrong

- **q18 · claude-haiku-5-5 · plain.** The incorrect verdict is right: the answer says the credit must be repaid within the 14 days, while 6502 m.24/3 allows 30 days after the notice. But the judge says the correct article is m.25. The cooling-off period for consumer credit is in 6502 m.24; m.25 covers interest rates.
- **q19 · claude-opus-5-5 · plain.** The incorrect verdict is right, but "Law 6502 does not provide" this fee is imprecise. 6502 m.37/2 does allow an early-repayment fee capped at 1%/2%, but only for housing finance (konut finansmanı), with a 36-month threshold. Opus moved that rule to ordinary consumer credit and changed the threshold to one year. That is a wrong fact, but not an invented one.

## Borderline cases I accepted

- **q09 · claude-haiku-5-5 · plain.** The answer treats 30 days as the fallback when no delivery period is agreed. MSY m.16/1 makes 30 days an absolute cap for sales of goods. This contradicts the law on the point the question asks about, so incorrect is defensible.
- **q10 · claude-haiku-5-5 · plain.** The conclusion is roughly right, but the test is wrong. The answer uses "examining it as you would in a store", while MSY m.13/2 refers to use in line with the item's operation, technical features and instructions. I agree with incorrect.
- **q17 · claude-haiku-5-5 · plain** (abstained). The answer declines, which fits abstained. It also cites unfair terms as m.6; that provision is 6502 m.5.

## Abstentions in grounded mode

Both models answered "Kaynaklarda bu sorunun cevabı yok." for q07 and q30 in grounded mode. In both cases BM25 did not retrieve the article that holds the answer: msy/m.15 for q07 and msy/m.12 for q30. These are exactly the two misses in recall@5. The models behaved correctly given the sources they saw. The gap is in retrieval, not in the models.

## Random sample of `correct` grades

t06, t01 and t10 for haiku grounded; q08 for opus plain; q01 for haiku grounded; q02 for haiku plain; t05 for haiku grounded; t01, q06 and t07 for opus plain. I agree with all 10. Two minor notes:

- t01 for opus plain says the law has "approximately 87 articles"; it has 88. The answer says the number is approximate and rejects the premise.
- q06 for opus plain leaves out that the 14 days restart if the seller gives the information late. Nothing it says contradicts the law.

## Cost note

The Cost column in `report.md` covers only the answers ($1.32 in total). The 152 judge calls (claude-opus-5-5, effort medium) are not logged with token counts, so their cost is not included in the table.
