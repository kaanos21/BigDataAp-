using BigDataApı.Context;
using BigDataApı.Entities;
using BigDataApı.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using OpenAI.Chat;
using System.Text;
using System.Text.Json;


namespace BigDataApı.Repositories.Concrete
{
    public class CustomerAnalyticsService : ICustomerAnalyticsService
    {
        private readonly BigDataOrdersDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerAnalyticsService(BigDataOrdersDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }


        public async Task<double> GetAveragelOrdersCountPerCustomer()
        {
            var value = await _context.Orders
                .GroupBy(o => o.CustomerId)
                .Select(g => g.Count())
                .AverageAsync();
            return (double)value;
        }


        public async Task<string> GetCustomerDetailAlAnalysisByLastOrderModernMethod(int id)
    {
        var customer = await _context.Customers
            .Where(x => x.CustomerId == id)
            .Select(c => new
            {
                c.CustomerName,
                c.CustomerSurname,
                Orders = c.Orders
                    .OrderByDescending(o => o.OrderDate)
                    .Take(20)
                    .Select(o => new
                    {
                        o.OrderDate,
                        Product = o.Product.ProductName,
                        Category = o.Product.Category.CategoryName,
                        o.Quantity,
                        UnitPrice = o.Product.UnitPrice,
                        TotalPrice = o.Quantity * o.Product.UnitPrice
                    }).ToList()
            }).FirstOrDefaultAsync();

        if (customer == null) return "Müşteri bulunamadı.";


        ChatClient client = new(model: "gpt-4o-mini", "YOUR-API-KEY");

        string jsonData = JsonSerializer.Serialize(customer);

        string prompt = $"""
        Sen bir veri analisti ve müşteri davranış uzmanısın.
        Aşağıda bir müşterinin son 20 siparişine ait JSON verisi bulunmaktadır.
        Bu veriyi analiz ederek şu başlıklarla detaylı bir rapor oluştur:
        1. Müşteri profili
        2. Ürün tercihleri
        3. Zaman bazlı alışveriş davranışı
        4. Ortalama ve tekrar harcama eğilimi
        5. Sadakat düzeyi
        6. Pazarlama önerileri
        
        Veri: {jsonData}
        """;

        // 3. Mesaj gönderme (HttpClient ve manuel JSON parse işlemleri bitti)
        ChatCompletion completion = await client.CompleteChatAsync(new List<ChatMessage>
    {
        new SystemChatMessage("You are a senior marketing data analyst"),
        new UserChatMessage(prompt)
    }, new ChatCompletionOptions { Temperature = 0.5f });

        return completion.Content[0].Text;
    }

        public async Task<string> GetCustomerDetailAlAnalysisByLastOrderOldMethod(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Product)
                    .ThenInclude(p => p.Category)
                .Where(x => x.CustomerId == id)
                .Select(c => new
                {
                    c.CustomerName,
                    c.CustomerSurname,
                    Orders = c.Orders
                        .OrderByDescending(o => o.OrderDate)
                        .Take(20)
                        .Select(o => new
                        {
                            o.OrderDate,
                            Product = o.Product.ProductName,
                            Category = o.Product.Category.CategoryName,
                            o.Quantity,
                            UnitPrice = o.Product.UnitPrice,
                            TotalPrice = o.Quantity * o.Product.UnitPrice
                        }).ToList()
                }).FirstOrDefaultAsync();


            var jsonData = JsonSerializer.Serialize(customer);

            string prompt = $"Sen bir veri analitsi ve müşteri davranış uzmanısın." +
                $"Aşağıda bir müşterinin son 20 siparişine ait bir json verisi bulunmaktadır" +
                $"Bu veriyi analiz ederek şu başlıklarla detaylı bir müşteri analiz raporu oluştur" +
                $"1. Müşteri profili" +
                $"2. Ürün tercihleri" +
                $"3. Zaman bazlı Alışveris Davranışı" +
                $"4. Ortalama ve tekrar harcama eğilimi" +
                $"5. Sadakat ve tekrar harcaam eğilimi" +
                $"6. Pazarlama önerileri" +
                $"Veri: {jsonData}";

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "YOUR-API-KEY");

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "You are a senior marketing data analyst" },
                    new { role = "user", content = prompt },
                },
            temperature=0.5
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JsonElement>();

                string aiResponse = result.GetProperty("choices")[0]
                                          .GetProperty("message")
                                          .GetProperty("content")
                                          .GetString();

                return aiResponse;
            }

            return $"API Hatası: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
        }

        public async Task<int> GetTotalActiveCustomerIn3Month()
        {
            var value = await _context.Orders
                .Where(o => o.OrderDate >= DateTime.Now.AddMonths(-3))
                .Select(o => o.CustomerId)
                .Distinct()
                .CountAsync();
            return value;
        }

        public async Task<int> GetTotalCustomerCount()
        {
            return await _context.Customers.CountAsync();
        }

        public async Task<int> GetTotalDeactiveCustomerIn6Month()
        {
            var value = await _context.Customers
                .Where(c => !_context.Orders
                    .Any(o => o.CustomerId == c.CustomerId && o.OrderDate >= DateTime.Now.AddMonths(-6)))
                .CountAsync();
            return value;
        }
    }
}
