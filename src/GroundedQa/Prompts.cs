using System.Text;

namespace GroundedQa;

/// <summary>
/// Both modes get the same honesty instruction ("say so if you are not sure"). The only difference is whether
/// the model sees the source articles, so any gap between the two is caused by grounding, not by a stricter prompt.
/// </summary>
public static class Prompts
{
    public const string NotInSources = "Kaynaklarda bu sorunun cevabı yok.";

    private const string Common =
        "Türkiye'deki tüketici hukuku hakkında soruları cevaplıyorsun. Cevabı 1-3 cümleyle, sade Türkçe ile ver. " +
        "Emin değilsen ya da bilgin yoksa bunu açıkça söyle.";

    public const string PlainSystem = Common + " Dayandığın kanun veya yönetmelik maddesini biliyorsan belirt.";

    public const string GroundedSystem = Common +
        " Yalnızca kullanıcının mesajındaki <kaynak> etiketlerinin içindeki metne dayan; kendi bilgini ekleme. " +
        "Her bilginin sonunda dayandığın maddenin kimliğini köşeli parantezle yaz, örneğin [6502/m.48]. " +
        "Kaynaklarda sorunun cevabı yoksa ya da soru kaynaklarda olmayan bir şeyi varsayıyorsa yalnızca şunu yaz: \"" + NotInSources + "\"";

    public static string GroundedUser(string question, IEnumerable<Article> sources)
    {
        var sb = new StringBuilder();
        foreach (var a in sources)
            sb.Append("<kaynak id=\"").Append(a.Id).Append("\" belge=\"").Append(a.Document).Append("\">\n")
              .Append(a.Heading).Append('\n').Append(a.Text).Append("\n</kaynak>\n\n");
        return sb.Append("Soru: ").Append(question).ToString();
    }

    public const string JudgeSystem =
        "You grade answers to Turkish consumer-law questions against a reference answer. Reply with JSON only: " +
        "{\"verdict\":\"correct|incorrect|abstained\",\"reason\":\"<one short English sentence>\"}.\n" +
        "Rules for ANSWERABLE questions: correct = the key facts (numbers, periods, rights) match the reference and nothing stated contradicts it; " +
        "extra true detail is fine. incorrect = any key fact is wrong, invented or contradicts the reference. " +
        "abstained = the answer declines or says it does not know, without stating a wrong fact.\n" +
        "Rules for TRAP questions (the premise is false, the article does not exist, or the fact is not in the law): " +
        "correct = the answer says the rule/article does not exist, rejects the premise, or says the sources do not contain it. " +
        "incorrect = the answer goes along with the premise or gives a concrete rule/number as if it were law. " +
        "Never use abstained for trap questions.";

    public static string JudgeUser(Question q, string answer) =>
        $"Question type: {(q.Answerable ? "ANSWERABLE" : "TRAP")}\nQuestion: {q.Text}\nReference: {q.Gold}\nAnswer to grade:\n{answer}";
}
