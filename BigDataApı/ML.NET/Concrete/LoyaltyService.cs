using BigDataApı.Context;
using BigDataApı.Dtos.LoyaltyMLDtos;
using BigDataApı.ML.NET.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;

namespace BigDataApı.ML.NET.Concrete
{
    public class LoyaltyService : ILoyaltyService
    {
        private readonly BigDataOrdersDbContext _context;
        private readonly MLContext _mlContext;
        private readonly string modelPath = "MLModels/LoyaltyModels/LoyaltyScoreModel.zip";

        public LoyaltyService(BigDataOrdersDbContext context, MLContext mlContext)
        {
            _context = context;
            _mlContext = mlContext;
        }

        public Task<object> GetItalyLoyaltyScoreItalyWithML()
        {
            var data = _context.Customers
                .Include(o => o.Orders)
                .ThenInclude(o => o.Product)
                .Where(c => c.CustomerCountry == "İtalya")
                .AsEnumerable()
                .Select(c => 
                {
                    //Son sipariş tarihini bul
                    var lastOrderDate = c.Orders.Max(o => (DateTime?)o.OrderDate);
                    //Son siparişten bu yana geçen gün sayısı
                    var daySince = lastOrderDate.HasValue ? Math.Round( (DateTime.Now - lastOrderDate.Value).TotalDays) : 999;

                    double recency = daySince;
                    double frequency = c.Orders.Count;
                    double monetary = c.Orders.Sum(o => o.Quantity * o.Product.UnitPrice);

                    //Loyalty ağırlıklı ortalama bulma
                    double loyalty=(RecencyScore(recency) * 0.4) + (FrequencyScore(frequency) * 0.3) + (MonetaryScore(monetary) * 0.3);

                    //ML nete gönderilecek veri seti
                    return new LoyaltyScoreMLData
                    {
                        CustomerName = c.CustomerName + " " + c.CustomerSurname,
                        Recency = (float)recency,
                        Frequency = (float) frequency,
                        Monetary = (float) monetary,
                        LoyaltyScore =(float)loyalty
                    };
                }).ToList();

            IDataView dataView=_mlContext.Data.LoadFromEnumerable(data);

            var pipeline = _mlContext.Transforms.Concatenate("Features", "Recency", "Frequency", "Monetary" )
                .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "LoyaltyScore", maximumNumberOfIterations:100));

            //Modeli eğit
            var model = pipeline.Fit(dataView);

            //Modeli kaydet
            _mlContext.Model.Save(model, dataView.Schema, modelPath);

            //Tahmin Metodu
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<LoyaltyScoreMLData, LoyaltyScoreMLPrediction>(model);

            //
            var results = data.Select(x =>
            {
                var prediction = predictionEngine.Predict(new LoyaltyScoreMLData
                {
                    CustomerName = x.CustomerName,
                    Recency = x.Recency,
                    Frequency = x.Frequency,
                    Monetary = x.Monetary
                });

                return new 
                {
                    CustomerName = x.CustomerName,
                    Recency=x.Recency, //Son sipariş günü
                    Frequency =x.Frequency, //Toplam sipariş sayısı
                    Monetary =x.Monetary, //Toplam harcama
                    ActualLoyaltyScore =Math.Round(x.LoyaltyScore,2), //Manuel hesaplanan sadakat puanı
                    PredictedLoyaltyScore =Math.Round(prediction.LoyaltyScore,2) //Ml Net ile tahmin edilen sadakat puanı
                };
            }).OrderByDescending(r => r.PredictedLoyaltyScore).ToList();
            return Task.FromResult((object)results);
        }

        private static double RecencyScore(double days)
        {
            switch (days)
            {
                case <= 30:
                    return 100;
                case <= 90:
                    return 75;
                case <= 180:
                    return 50;
                case <= 365:
                    return 25;
            }
            return 10;
        }

        private static double FrequencyScore(double orders)
        {
            if (orders >= 20) return 100;
            if (orders >= 10) return 80;
            if (orders >= 5) return 60;
            if (orders >= 2) return 40;
            if (orders >= 1) return 20;
            return 10;
        }
        private static double MonetaryScore(double spent)
        {
            if (spent >= 5000) return 100;
            if (spent >= 3000) return 80;
            if (spent >= 1000) return 60;
            if (spent >= 500) return 40;
            if (spent >= 100) return 20;
            return 10;
        }
    }
}
