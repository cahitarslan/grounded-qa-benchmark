**Türkçe** | [English](README.en.md)

# grounded-qa-benchmark

Yapay zeka modeli, cevabı kaynak metinden vermek zorunda kalınca daha az mı uyduruyor?

Bu repo bunu herkesin karşılaştığı bir konuda ölçüyor: tüketici hakları (iade, ayıplı ürün, garanti, abonelik, kredi).
Aynı 38 soru aynı modele iki kez soruluyor:

- **Düz:** Model bildiği kadarıyla cevaplıyor.
- **Kaynaklı:** Küçük bir arama motoru kanundan en ilgili 5 maddeyi buluyor. Model yalnızca bunları kullanabiliyor, hangi maddeye dayandığını yazıyor, cevap yoksa "yok" diyor.

28 sorunun cevabı kanunda açıkça var. 10 soru ise **tuzak**: olmayan bir madde, yanlış bir varsayım ("indirimli üründe iade süresi 7 gün, değil mi?") ya da kanunda hiç geçmeyen bir rakam.
Cevap böyle bir kural olmadığını ya da kaynakta yer almadığını söylerse tuzak yakalanmış sayılıyor.

İki moda da aynı "emin değilsen söyle" talimatı veriliyor. Yani aradaki fark daha sıkı bir talimattan değil, kaynağı görmekten geliyor.

## Sonuçlar

[`results/`](results/) klasöründe. Her cevap, her not ve hakemin gerekçesi JSON Lines olarak duruyor; rapordaki her rakam elle kontrol edilebilir.

## Çalıştırma

.NET 10 SDK ve bir Anthropic API anahtarı gerekir.

```sh
dotnet test GroundedQa.slnx
dotnet run --project src/GroundedQa -- recall                 # arama kontrolü, API çağrısı yok
dotnet run --project src/GroundedQa -- search "iade süresi"   # aramanın ne bulduğunu gösterir
set ANTHROPIC_API_KEY=...
dotnet run --project src/GroundedQa -- run --models claude-opus-5-5,claude-haiku-5-5
```

Tam çalıştırma 160 cevap ve 160 nottan oluşuyor; liste fiyatlarıyla 2 doların altında tutuyor.

## Sınırlar

- Tek konu, 38 soru, yalnız Türkçe. Bir yön gösteriyor, her alana geçerli bir oran değil.
- Notları bir model veriyor. Şüpheli olanlar elle kontrol edildi; ayrıntı raporda.
- Mevzuat değişir. Metinler `corpus/README.md` içindeki tarihteki hâliyle duruyor. Hukuki tavsiye değildir.

## Lisans

Kod MIT. `corpus/` altındaki mevzuat metinleri resmî metin olduğu için 5846 sayılı Kanun'un 31. maddesine göre telif korumasına tabi değildir.
