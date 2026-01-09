using BigDataApı.Context;
using BigDataApı.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BigDataApı.Repositories.Concrete
{
    public class ReviewService : IReviewService
    {
        private readonly BigDataOrdersDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public ReviewService(BigDataOrdersDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetCustomerReviewWithOpenAIAnalysis()
        {
            int id = 657;

            var reviews = await _context.Reviews
                .Where(r => r.CustomerId == id)
                .OrderByDescending(r => r.ReviewDate)
                .Take(10)
                .Select(r => new
                {
                    r.Rating,
                    r.Sentiment,
                    r.ReviewText,
                    r.ReviewDate
                })
                .ToListAsync();

            if (reviews == null || !reviews.Any())
            {
                return "<h4>Veri bulunamadı</h4><p>Bu müşterinin yorumu yok.</p>";
            }

            var jsonData = JsonSerializer.Serialize(reviews);

            string prompt = $@"
⚠️ Sadece saf HTML üret. Kod bloğu üretme. Markdown verme.

Sen bir müşteri davranış analisti + psikoloji destekli yorum analiz uzmanısın.

Aşağıdaki müşteriye ait son yorumları analiz et ve HTML oluştur.

Kullanacağın başlıklar ve format:

<h4>👤 Müşteri Yorum Profili</h4>
<p><b>Genel tutum:</b> ...</p>
<p><b>Yorum tarzı:</b> ...</p>
<p><b>Ortalama Rating:</b> ...</p>

<h4>📊 Duygu & Ton Analizi</h4>
<p><b>Olumlu:</b> ...</p>
<p><b>Olumsuz:</b> ...</p>
<p><b>Nötr:</b> ...</p>
<p><b>Dil ve ton:</b> ...</p>

<h4>🧠 Karakter Analizi (Review Temelli)</h4>
<ul>
<li>Memnuniyet eşiği: ...</li>
<li>Şikayet hassasiyeti: ...</li>
<li>Beklenti seviyesi: ...</li>
<li>Kişilik tipi: ...</li>
</ul>

<h4>🔥 Şikayet & Övgü Temaları</h4>
<p><b>Şikayet ettiği konular:</b></p>
<ul><li>...</li></ul>

<p><b>Övdüğü konular:</b></p>
<ul><li>...</li></ul>

<h4>📈 Davranış Trendi</h4>
<p><b>Duygu değişimi:</b> ...</p>
<p><b>Memnuniyet eğilim:</b> ...</p>
<p><b>Risk analizi:</b> ...</p>

<h4>🚀 Aksiyon & İletişim Stratejisi</h4>
<ul>
<li>Müşteriye yaklaşım:</li>
<li>Destek iletişim dili:</li>
<li>Müşteri bağlılığını artırma önerisi:</li>
</ul>

Veri: {jsonData}
";

            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YOUR-API-KEY");

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
            new { role = "system", content = "You are an expert customer sentiment and behavior analyst." },
            new { role = "user", content = prompt }
        },
                temperature = 0.5
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var doc = JsonDocument.Parse(responseString);
            var completion = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

            string[] sections = completion.Split(new[] { "<h4>" }, StringSplitOptions.None);


            return completion;
        }
    }
}
