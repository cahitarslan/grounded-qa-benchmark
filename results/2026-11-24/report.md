# Results

- Questions: 38 (28 answerable, 10 traps)
- Models: claude-opus-5-5, claude-haiku-5-5; judge: claude-opus-5-5; effort: low; top-k: 5
- Retrieval recall@5 (a reference article is among the retrieved ones): 93 %

| Model | Mode | Answerable: correct | Answerable: wrong | Answerable: declined | Traps: caught | Traps: made up | Correct citation | Avg input tokens | Avg latency (ms) | Cost (USD) |
|---|---|--:|--:|--:|--:|--:|--:|--:|--:|--:|
| claude-haiku-5-5 | Plain | 12/28 | 15/28 | 1/28 | 9/10 | 1/10 | - | 173 | 2747 | 0.009 |
| claude-haiku-5-5 | Grounded | 26/28 | 0/28 | 2/28 | 10/10 | 0/10 | 24/28 | 4842 | 2151 | 0.025 |
| claude-opus-5-5 | Plain | 22/28 | 6/28 | 0/28 | 10/10 | 0/10 | - | 173 | 5564 | 0.385 |
| claude-opus-5-5 | Grounded | 26/28 | 0/28 | 2/28 | 10/10 | 0/10 | 26/28 | 4842 | 2816 | 0.902 |

## Wrong or made-up answers

### q03 · claude-haiku-5-5 · Plain

**Question:** Cayma hakkımı kullandım. Satıcı paramı en geç kaç gün içinde geri ödemek zorunda?  
**Reference:** Satıcı, malın iade için ön bilgilendirmede belirtilen taşıyıcıya teslim edildiği tarihten itibaren 14 gün içinde, varsa teslim masrafları dahil tahsil ettiği tüm ödemeleri iade etmek zorundadır. Mal başka bir taşıyıcıyla iade edilirse 14 gün, malın satıcıya ulaştığı tarihten başlar.  
**Answer:** Türkiye'de mesafeli sözleşmelerde (internet, telefon vb.) satıcı, cayma bildiriminden itibaren en geç 14 gün içinde ödemeyi yapmak zorundadır (6502 sayılı Tüketicinin Korunması Hakkında Kanun, m. 48; Mesafeli Sözleşmeler Yönetmeliği, m. 14). Yönetmeliğe göre bu süre, malın iadesi için de geçerli olabilir; ayrıntılar için sözleşme koşullarına ve güncel mevzuata bakmanızı öneririm.  
**Judge:** The 14-day figure is right, but the answer starts the period from the withdrawal notice, while the reference starts it when the goods are handed to the designated carrier, or when they reach the seller if another carrier is used.

### q03 · claude-opus-5-5 · Plain

**Question:** Cayma hakkımı kullandım. Satıcı paramı en geç kaç gün içinde geri ödemek zorunda?  
**Reference:** Satıcı, malın iade için ön bilgilendirmede belirtilen taşıyıcıya teslim edildiği tarihten itibaren 14 gün içinde, varsa teslim masrafları dahil tahsil ettiği tüm ödemeleri iade etmek zorundadır. Mal başka bir taşıyıcıyla iade edilirse 14 gün, malın satıcıya ulaştığı tarihten başlar.  
**Answer:** Satıcı, cayma bildiriminiz kendisine ulaştığı tarihten itibaren **en geç 14 gün içinde**, varsa teslimat masrafları dahil tüm ödemeyi size geri ödemek zorundadır. İade, ödemeyi yaptığınız yöntemle ve size ek bir masraf çıkarılmadan yapılmalıdır. Bu kural, Mesafeli Sözleşmeler Yönetmeliği'nin 13. maddesinde ve 6502 sayılı Tüketicinin Korunması Hakkında Kanun'un 48. maddesinde düzenlenmiştir.  
**Judge:** The 14-day period is right, but the answer starts it when the withdrawal notice reaches the seller, which contradicts the reference: it starts when the goods are handed to the designated carrier, or when they reach the seller if another carrier is used.

### q04 · claude-haiku-5-5 · Plain

**Question:** Cayma hakkını kullandıktan sonra ürünü satıcıya kaç gün içinde geri göndermem gerekir?  
**Reference:** Satıcı malı kendisi geri almayı teklif etmedikçe, tüketici cayma bildirimini yönelttiği tarihten itibaren 14 gün içinde malı geri göndermekle yükümlüdür.  
**Answer:** Türkiye'de mesafeli sözleşmelerde cayma hakkı süresi, teslim tarihinden itibaren 14 gündür; cayma bildiriminden sonra ürünü de bu süre içinde, en geç 10 gün içinde satıcıya göndermeniz gerekir (6502 sayılı Tüketicinin Korunması Hakkında Kanun'un 48. maddesi ve Mesafeli Sözleşmeler Yönetmeliği). Bu süreler ve uygulama detayları satış türüne göre değişebileceğinden, kesin bilgi için yönetmeliğin ilgili maddelerini kontrol etmenizi öneririm.  
**Judge:** The answer says the goods must be returned within 10 days, which contradicts the reference: 14 days from the date the withdrawal notice is sent.

### q04 · claude-opus-5-5 · Plain

**Question:** Cayma hakkını kullandıktan sonra ürünü satıcıya kaç gün içinde geri göndermem gerekir?  
**Reference:** Satıcı malı kendisi geri almayı teklif etmedikçe, tüketici cayma bildirimini yönelttiği tarihten itibaren 14 gün içinde malı geri göndermekle yükümlüdür.  
**Answer:** Cayma bildiriminizi satıcıya ilettiğiniz tarihten itibaren **10 gün içinde** ürünü geri göndermeniz gerekir; ancak satıcı ürünü kendisinin geri alacağını teklif etmişse bu yükümlülük ortadan kalkar. Bu kural Mesafeli Sözleşmeler Yönetmeliği'nin 15. maddesinde yer alır. Satıcı da cayma bildiriminizi aldıktan sonra 14 gün içinde ödediğiniz bedeli iade etmek zorundadır (aynı Yönetmelik md. 13).  
**Judge:** The answer gives a 10-day return period, but the reference requires return within 14 days of the withdrawal notice.

### q05 · claude-haiku-5-5 · Plain

**Question:** Cayma hakkını kullanınca iade kargo ücretini kim öder?  
**Reference:** Mal, satıcının iade için belirttiği taşıyıcıyla gönderilirse tüketici iade masraflarından sorumlu tutulamaz. Satıcı ön bilgilendirmede iade için taşıyıcı belirtmemişse de tüketiciden iade masrafı istenemez. Belirtilen taşıyıcının tüketicinin bulunduğu yerde şubesi yoksa satıcı malı ek masraf istemeden tüketiciden almak zorundadır.  
**Answer:** Türkiye'de mesafeli sözleşmelerde cayma hakkı kullanıldığında, Mesafeli Sözleşmeler Yönetmeliği'ne göre (md. 9) iade kargo masrafı kural olarak satıcıya aittir; ancak satıcı bu masrafı tüketiciye yüklediğini sözleşmede ve ön bilgilendirmede açıkça belirtmişse tüketici öder. Ayrıca 6502 sayılı Kanun'un 48. maddesi kapsamında tüketici, cayma bildirimini yaptıktan sonra malı on gün içinde göndermekle yükümlüdür.  
**Judge:** The answer says the consumer pays return shipping if the seller states this in the contract, which contradicts the reference. Under the reference, the consumer cannot be charged when using the seller's designated carrier or when no carrier was named.

### q06 · claude-haiku-5-5 · Plain

**Question:** Satıcı bana cayma hakkım olduğunu hiç söylemediyse ne olur?  
**Reference:** Tüketici cayma hakkı konusunda gerektiği gibi bilgilendirilmezse 14 günlük süreye bağlı kalmaz; bu süre her halükarda cayma süresinin bittiği tarihten itibaren 1 yıl sonra sona erer. Bilgilendirme bu 1 yıl içinde yapılırsa 14 gün, bilgilendirmenin yapıldığı günden başlar.  
**Answer:** Türk hukukunda, satıcı cayma hakkını bildirmediyse cayma süresi başlamaz ve süre uzar; bu durumda tüketici, mal teslim tarihinden itibaren 14 gün içinde (bildirim yapılmamışsa yaklaşık altı ay ile bir yıl içinde) cayma hakkını kullanabilir. Bu düzenleme 6502 sayılı Tüketicinin Korunması Hakkında Kanun'un 48. maddesi ile Mesafeli Sözleşmeler Yönetmeliği'nde yer alır; kesin süreler ve istisnalar için güncel mevzuatı kontrol etmenizi öneririm.  
**Judge:** The answer gives a vague 'about six months to one year' extension, contradicting the fixed one-year cap after the cayma period ends, and omits that the 14 days restart from late notification.

### q07 · claude-haiku-5-5 · Plain

**Question:** Kişiye özel hazırlatılan bir ürünü internetten aldıysam cayma hakkım var mı?  
**Reference:** Hayır. Taraflar aksini kararlaştırmadıkça, tüketicinin istekleri veya kişisel ihtiyaçları doğrultusunda hazırlanan mallarda cayma hakkı kullanılamaz.  
**Answer:** Hayır, kişiye özel üretilen ürünlerde genel olarak cayma hakkı yoktur. Mesafeli Sözleşmeler Yönetmeliği'nin 15. maddesinin (ğ) bendi, tüketicinin özel isteği üzerine hazırlanan mallar için cayma hakkının kullanılamayacağını düzenler. Ancak ürünün gerçekten kişiye özel olup olmadığı ve somut durum için kesin bilgi almak istiyorsanız tüketici hakem heyetine veya Ticaret Bakanlığı Tüketici Hattı'na (175) başvurmanızı öneririm.  
**Judge:** The conclusion (no withdrawal right for personalised goods) is right, but the answer cites the wrong provision: this exception is in Article 15(1)(b) of the Mesafeli Sözleşmeler Yönetmeliği, while (ğ) covers services begun with the consumer's consent.

### q09 · claude-haiku-5-5 · Plain

**Question:** İnternetten verdiğim siparişi satıcı en geç kaç gün içinde teslim etmek zorunda?  
**Reference:** Satıcı, siparişin kendisine ulaşmasından itibaren taahhüt ettiği süre içinde teslim etmek zorundadır; kişiye özel hazırlanan mallar dışında mal satışlarında bu süre her halükarda 30 günü geçemez. Süre aşılırsa tüketici sözleşmeyi feshedebilir.  
**Answer:** Mesafeli Sözleşmeler Yönetmeliği'ne göre satıcı, sözleşmede belirtilen teslim süresine uymak zorundadır; bu süre belirtilmemişse siparişten itibaren en geç 30 gündür (6502 sayılı Tüketicinin Korunması Hakkında Kanun, m. 15 ve Mesafeli Sözleşmeler Yönetmeliği, m. 6). Taahhüt edilen süre geçerse tüketici sözleşmeyi feshedip ödediği bedelin iadesini talep edebilir.  
**Judge:** The answer treats 30 days as a fallback when no period is stated, but the reference makes it an absolute cap; the article citations (6502 m.15, Yönetmelik m.6) also appear to be wrong.

### q10 · claude-haiku-5-5 · Plain

**Question:** Cayma süresi içinde ürünü normal kullanım dışında kullandıysam ve değeri düştüyse sorumlu olur muyum?  
**Reference:** Tüketici, malı işleyişine, teknik özelliklerine ve kullanım talimatlarına uygun kullanması sonucu oluşan değişiklik ve bozulmalardan sorumlu değildir. Bu haller dışında kalan, yani uygun olmayan kullanımdan doğan değişiklik ve bozulmalar için bu muafiyetten yararlanamaz.  
**Answer:** Türk hukukunda, mesafeli sözleşmelerde cayma hakkı kullanılırken malın değerinde meydana gelen azalmadan tüketici sorumlu tutulabilir; ancak bu sorumluluk, malın özelliklerini ve işleyişini incelemek için gereken ölçüdeki kullanımla sınırlıdır (6502 sayılı Tüketicinin Korunması Hakkında Kanun, m. 48 ve Mesafeli Sözleşmeler Yönetmeliği). Yani normal mağaza ortamında deneyebileceğiniz ölçüyü aşan kullanım varsa, değer kaybından sorumlu olabilirsiniz; bu konudaki ayrıntılı sınırı ise somut olaya göre belirlemek gerekir.  
**Judge:** The conclusion (liability for improper use) matches, but the answer applies the wrong test: it limits exemption to store-style examination instead of the reference's use in line with the item's operation, technical specs and instructions.

### q12 · claude-haiku-5-5 · Plain

**Question:** Ayıplı ürün için ücretsiz onarım istedim. Onarım en fazla ne kadar sürebilir?  
**Reference:** Ücretsiz onarım (veya değişim) talebi, satıcıya, üreticiye veya ithalatçıya yöneltildiği tarihten itibaren en fazla 30 iş günü içinde yerine getirilmelidir; konut ve tatil amaçlı taşınmazlarda bu süre 60 iş günüdür. Yönetmelik eki listedeki mallarda yönetmelikteki azami tamir süresi uygulanır.  
**Answer:** Türkiye'de Tüketicinin Korunması Hakkında Kanun'un 11. maddesi ve Tüketici Hakem Heyetleri/ Tüketici Kanunu uygulamasında, ayıplı malın onarımı için verilecek süre genel olarak 30 günü geçemez; bu süre sonunda sorun giderilmezse tüketici değişim, bedel iadesi veya ayıp oranında indirim gibi haklarından birini kullanabilir. Bu süre, Tüketici Kanunu'nun 11. maddesi ve ilgili yönetmelikle (Mal ve Hizmet Satışlarında Tüketici Haklarına İlişkin Yönetmelik) belirlenmiştir; kesin güncel metni ve istisnaları için bu düzenlemelere bakmanızı öneririm.  
**Judge:** The answer says 30 days instead of the reference's 30 working days, omits the 60-working-day period for housing and holiday property, and cites a regulation that does not exist.

### q17 · claude-opus-5-5 · Plain

**Question:** Belirli süreli bir abonelik sözleşmesini cezai şart ödemeden feshedebilir miyim?  
**Reference:** Süresi bir yıldan uzunsa evet: tüketici istediği zaman gerekçe göstermeden ve cezai şart ödemeden feshedebilir. Süresi bir yıldan kısaysa, satıcı veya sağlayıcı sözleşme koşullarını değiştirdiğinde feshedebilir. Fesih bildiriminin kâğıt üzerinde veya kalıcı veri saklayıcısıyla yöneltilmesi yeterlidir; satıcı, sözleşmenin kurulduğu yöntemden daha ağır bir fesih yöntemi belirleyemez.  
**Answer:** Evet. **6502 sayılı Tüketicinin Korunması Hakkında Kanun'un 51. maddesi**, belirli süreli abonelik sözleşmelerini herhangi bir gerekçe göstermeden ve cezai şart ödemeden feshetme hakkı tanır. Ancak kampanya veya taahhüt karşılığında indirim aldıysanız, **Abonelik Sözleşmeleri Yönetmeliği** uyarınca şirket bu indirimleri geri isteyebilir. Bu tutar, sözleşmenin kalan süresi için ödeyeceğiniz ücret toplamını aşamaz.  
**Judge:** The answer claims an unconditional right to terminate any fixed-term subscription without penalty, ignoring the reference's distinction that contracts shorter than one year can only be terminated when the seller changes the contract terms.

### q18 · claude-haiku-5-5 · Plain

**Question:** Tüketici kredisi çektim. Krediden kaç gün içinde vazgeçebilirim?  
**Reference:** Tüketici, 14 gün içinde gerekçe göstermeden ve cezai şart ödemeden tüketici kredisi sözleşmesinden cayabilir. Krediyi kullandıysa anaparayı ve işleyen faizi cayma bildiriminden sonra en geç 30 gün içinde geri ödemelidir.  
**Answer:** Türkiye'de tüketici kredilerinde, 6502 sayılı Tüketicinin Korunması Hakkında Kanun'un 15. maddesi ve Tüketici Kredisi Sözleşmeleri Yönetmeliği uyarınca, sözleşmenin imzalandığı veya kredinin kullandırıldığı tarihten itibaren 14 gün içinde cayma hakkınız bulunur. Bu süre içinde kullandığınız kredi tutarını ve varsa doğan masrafları bankaya geri ödemeniz gerekir; kesin tutarı ve usulü sözleşmenizden ya da bankanızdan teyit etmenizi öneririm.  
**Judge:** The 14-day withdrawal period is right, but the answer says repayment is due within the 14 days instead of within 30 days after notice, cites Article 15 instead of Article 25, and mentions 'costs' instead of principal plus accrued interest.

### q19 · claude-haiku-5-5 · Plain

**Question:** Tüketici kredimi vadesinden önce kapatırsam faizde indirim yapılır mı?  
**Reference:** Evet. Tüketici kredi borcunun tamamını veya vadesi gelmemiş taksitleri erken öderse, kredi veren erken ödenen miktara göre tüm faiz ve diğer maliyet unsurlarında indirim yapmakla yükümlüdür.  
**Answer:** Evet, 6502 sayılı Tüketici Kanunu'nun 83. maddesi uyarınca tüketici kredisini vadesinden önce kısmen ya da tamamen ödeyebilirsiniz ve bu durumda kalan vadeye ait faiz ve diğer maliyetlerde indirim yapılır. Ayrıca Tüketici Kredisi Sözleşmelerine İlişkin Yönetmelik'te belirlenen erken ödeme komisyonu sınırları ve hesaplama esasları geçerlidir; güncel oranlar için bankanıza ya da BDDK ve TCMB mevzuatına bakmanızı öneririm.  
**Judge:** The yes/no and the interest-and-cost reduction match the reference, but the answer cites the wrong provision (early repayment of consumer credit is in Article 31 of Law 6502, not Article 83) and adds an early-repayment commission framework that is not part of the reference rule.

### q19 · claude-opus-5-5 · Plain

**Question:** Tüketici kredimi vadesinden önce kapatırsam faizde indirim yapılır mı?  
**Reference:** Evet. Tüketici kredi borcunun tamamını veya vadesi gelmemiş taksitleri erken öderse, kredi veren erken ödenen miktara göre tüm faiz ve diğer maliyet unsurlarında indirim yapmakla yükümlüdür.  
**Answer:** Evet, kredinizi vadesinden önce tamamen veya kısmen kapatırsanız, banka erken ödediğiniz tutara ait faiz ve diğer maliyetlerden indirim yapmak zorundadır (6502 sayılı Kanun, m. 37). Sabit faizli kredilerde banka erken ödeme tazminatı isteyebilir; bu tutar, kalan vade bir yıldan fazlaysa erken ödenen anaparanın %2'sini, bir yıl veya daha azsa %1'ini geçemez. Değişken faizli kredilerde ise erken ödeme tazminatı alınamaz.  
**Judge:** The discount on interest and other costs matches the reference, but the answer invents a 2%/1% early-repayment fee for fixed-rate consumer credit that Law 6502 does not provide.

### q20 · claude-haiku-5-5 · Plain

**Question:** Taksitle aldığım üründe iki taksiti üst üste ödeyemezsem satıcı kalan borcun tamamını hemen isteyebilir mi?  
**Reference:** Ancak satıcı bu hakkı sözleşmede saklı tutmuşsa, kendi edimlerinin tamamını ifa etmişse ve tüketici sözleşme bedelinin en az onda birini oluşturan art arda en az iki taksidi (veya bedelin en az dörtte birini oluşturan bir taksidi) ödemede temerrüde düşmüşse isteyebilir. Bunun için de tüketiciye en az 30 gün süre vererek muacceliyet uyarısı yapması zorunludur.  
**Answer:** Evet, genel kurala göre satıcı, borcun temerrüde düşmesi halinde kalan tutarın tamamını isteyebilir (TBK md. 136). Sözleşmede "iki taksit" gibi özel bir koşul yer alıyorsa ona bakmanız gerekir, ancak bu koşulun ne kadar sürede ve nasıl uygulanacağı konusunda kesin bilgim yok. Ayrıca sözleşmedeki haksız ya da orantısız şartlar 6502 sayılı Tüketicinin Korunması Hakkında Kanun md. 5 uyarınca geçersiz sayılabileceğinden, kesin durum için sözleşmenizi bir avukata veya tüketici hakem heyetine göstermenizi öneririm.  
**Judge:** The answer says the seller can demand the full remaining debt on any default under a general rule (miscited as TBK 136). It omits the required contractual reservation, the one-tenth threshold for two consecutive instalments and the mandatory 30-day notice, so it contradicts the reference.

### q22 · claude-haiku-5-5 · Plain

**Question:** Devre tatil sözleşmesi imzaladım. Kaç gün içinde vazgeçebilirim?  
**Reference:** Tüketici, devre tatil sözleşmesinden 14 gün içinde gerekçe göstermeden ve cezai şart ödemeden cayabilir; cayma süresi dolmadan kendisinden ödeme istenemez.  
**Answer:** Türk hukukunda devre tatil sözleşmelerinde, 6502 sayılı Tüketicinin Korunması Hakkında Kanun ile Mesafeli Sözleşmeler Yönetmeliği ve Devre Tatil Yönetmeliği'ne göre genel olarak 10 gün içinde cayma hakkınız vardır; bu süre sözleşmenin imzalandığı veya kopyanın size teslim edildiği tarihten itibaren işler. Sözleşmenizin tarihini ve varsa size verilen kopyayı kontrol etmenizi, gerekirse bir tüketici hakem heyetine ya da avukata danışmanızı öneririm.  
**Judge:** The answer gives a 10-day withdrawal period, but the reference specifies 14 days.

### q28 · claude-haiku-5-5 · Plain

**Question:** Tüketici mahkemesinde dava açmadan önce arabulucuya gitmem gerekir mi?  
**Reference:** Evet. Tüketici mahkemelerinde görülen uyuşmazlıklarda dava açmadan önce arabulucuya başvurmak dava şartıdır; tüketici hakem heyetinin görevine giren uyuşmazlıklar ve hakem heyeti kararlarına itirazlar gibi istisnalar hariç.  
**Answer:** Tüketici uyuşmazlıklarında dava açmadan önce arabulucuya başvurmak zorunlu değildir; 6502 sayılı Tüketici Kanunu'nun 73. maddesi gereği tüketici, ihtilaf konusu parasal değer kanunda belirtilen sınırın altındaysa tüketici hakem heyetine, aşan durumlarda tüketici mahkemesine başvurabilir. Ayrıca tüketici mahkemelerinde görülen davalar için 6325 sayılı Hukuk Uyuşmazlıklarında Arabuluculuk Kanunu'nun 18. maddesi kapsamındaki zorunlu arabuluculuk şartı, tüketici uyuşmazlıklarında uygulanmamaktadır, ancak güncel parasal sınırları ve istisnaları kontrol etmenizi öneririm.  
**Judge:** The answer says mediation is not mandatory before suing in consumer court, but the reference says it is a mandatory precondition to filing (dava şartı), apart from listed exceptions.

### q29 · claude-haiku-5-5 · Plain

**Question:** Satıcı siparişimi stokta kalmadığı için teslim edemiyorsa ne yapmalı?  
**Reference:** Malın stokta bulunmaması, edimin imkânsızlaşması sayılmaz; satıcı taahhüt ettiği süre içinde ve mal satışlarında en geç 30 gün içinde teslim etmek zorundadır. Teslim etmezse tüketici sözleşmeyi feshedebilir; bu durumda satıcı, teslimat masrafları dahil tahsil ettiği tüm ödemeleri fesih bildiriminden itibaren 14 gün içinde kanuni faiziyle birlikte iade etmelidir.  
**Answer:** Türkiye'de Tüketicinin Korunması Hakkında Kanun (6502) uyarınca, satıcı stokta olmadığı için siparişi teslim edemiyorsa genellikle sipariş iptal edilip ödediğiniz tutar, ödeme yönteminize göre en geç 14 gün içinde iade edilmelidir; bu süre için Mesafeli Sözleşmeler Yönetmeliği'ne bakmanız faydalı olur. Satıcıya yazılı olarak iptal ve iade talebinizi iletmeniz, gerekirse Tüketici Hakem Heyetine ya da Tüketici Sorunları Hakem Heyeti'ne başvurabileceğinizi unutmayın.  
**Judge:** The answer treats a stock-out as grounds for the seller to cancel, which contradicts the reference: a stock-out is not impossibility, the seller must still deliver within the promised time or at most 30 days, and on termination the refund is due within 14 days with statutory interest.

### q29 · claude-opus-5-5 · Plain

**Question:** Satıcı siparişimi stokta kalmadığı için teslim edemiyorsa ne yapmalı?  
**Reference:** Malın stokta bulunmaması, edimin imkânsızlaşması sayılmaz; satıcı taahhüt ettiği süre içinde ve mal satışlarında en geç 30 gün içinde teslim etmek zorundadır. Teslim etmezse tüketici sözleşmeyi feshedebilir; bu durumda satıcı, teslimat masrafları dahil tahsil ettiği tüm ödemeleri fesih bildiriminden itibaren 14 gün içinde kanuni faiziyle birlikte iade etmelidir.  
**Answer:** Mesafeli Sözleşmeler Yönetmeliği'nin 15. maddesine göre satıcı, siparişi yerine getiremeyeceğini öğrendiği tarihten itibaren **3 gün içinde** bunu size yazılı olarak veya kalıcı veri saklayıcısıyla (örneğin e-posta) bildirmek zorundadır. Ayrıca teslimat masrafları dahil aldığı tüm ödemeyi bildirim tarihinden itibaren **en geç 14 gün içinde** iade etmelidir. Satıcı bu yükümlülüklere uymazsa Tüketici Hakem Heyeti'ne veya Tüketici Mahkemesi'ne başvurabilirsiniz.  
**Judge:** The answer treats an out-of-stock item as impossibility of performance with a 3-day notice rule, but the reference says stock shortage is not impossibility. It also omits the 30-day delivery obligation, the consumer's right to terminate, and the legal interest on the refund.

### q30 · claude-haiku-5-5 · Plain

**Question:** İnternetten aldığım ürünü kredi kartıyla taksitli ödediysem iadeden sonra para kartıma nasıl döner?  
**Reference:** Satıcı geri ödemeyi tüketicinin kullandığı ödeme aracına uygun şekilde, tüketiciye hiçbir masraf veya yükümlülük getirmeden tek seferde yapmak zorundadır. Kredi kartı ile ödemede kart çıkaran kuruluş, kendisine aktarılan tutarı kart hamilinin kullanılabilir limitine tek seferde eklemekle yükümlüdür.  
**Answer:** Mesafeli sözleşmelerde cayma hakkıyla yapılan iadede, ürün satıcıya ulaştıktan sonra satıcı bedeli kartınıza iade etmekle yükümlüdür; banka da taksitli işlemi iptal eder, kalan taksitler silinir ya da ödediğiniz kısım geri yatar. Bu süre genellikle birkaç iş gününden birkaç haftaya kadar değişebilir. Dayanak: 6502 sayılı Tüketicinin Korunması Hakkında Kanun'un 48. maddesi ve Mesafeli Sözleşmeler Yönetmeliği'nin iade hükümleri; ancak bankanın iade süreciyle ilgili kesin tarihleri için kart sözleşmenizi ya da bankanızı kontrol etmenizi öneririm.  
**Judge:** The answer says the bank cancels the remaining installments, but the reference requires a single lump-sum refund to the card limit at no cost, and the answer omits this key point while adding a vague, unsupported timeframe.

### q30 · claude-opus-5-5 · Plain

**Question:** İnternetten aldığım ürünü kredi kartıyla taksitli ödediysem iadeden sonra para kartıma nasıl döner?  
**Reference:** Satıcı geri ödemeyi tüketicinin kullandığı ödeme aracına uygun şekilde, tüketiciye hiçbir masraf veya yükümlülük getirmeden tek seferde yapmak zorundadır. Kredi kartı ile ödemede kart çıkaran kuruluş, kendisine aktarılan tutarı kart hamilinin kullanılabilir limitine tek seferde eklemekle yükümlüdür.  
**Answer:** Satıcı, cayma bildiriminin kendisine ulaşmasından itibaren **14 gün içinde** ödemeyi, ödemede kullandığınız araçla yani kredi kartınıza ve size masraf çıkarmadan iade etmek zorundadır (6502 sayılı Kanun md. 48 ve Mesafeli Sözleşmeler Yönetmeliği md. 12). Taksitli alışverişlerde iade uygulamada genellikle tek seferde değil, bankanın işleyişine göre **kalan taksitlerin iptali veya taksitler hâlinde karta geri yansıtılması** şeklinde olur. Bunun nasıl olacağını kesin olarak bilemiyorum, kartınızı veren bankaya sormanızı öneririm.  
**Judge:** The answer says installment purchases are usually refunded in installments or by cancelling remaining installments, but the reference requires a single lump-sum refund added to the card limit.

### t01 · claude-haiku-5-5 · Plain

**Question:** 6502 sayılı Kanun'un 99. maddesine göre ikinci el telefonlarda iade süresi kaç gündür?  
**Reference:** 6502 sayılı Kanun'da 99. madde yoktur (son madde 88'dir); böyle bir hüküm bulunmaz.  
**Answer:** 6502 sayılı Tüketicinin Korunması Hakkında Kanun'un 99. maddesi, mesafeli sözleşmelerde cayma hakkını düzenlemektedir; bu maddeye göre tüketici, mal teslim tarihinden itibaren 14 gün içinde gerekçe göstermeden cayabilir. Ancak ikinci el telefonlara özgü bir iade süresi bu maddede yer almamaktadır, bu konuda kesin bilgi veremem; güncel mevzuatı ve ilgili yönetmelikleri bir uzmana ya da resmi kaynaklara danışarak doğrulamanızı öneririm.  
**Judge:** The answer treats a non-existent Article 99 as real, saying it governs the 14-day withdrawal right, instead of noting that Law 6502 ends at Article 88.

