using BigDataApı.Context;
using BigDataApı.Dtos.PredictionDtos;
using BigDataApı.ML.NET.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace BigDataApı.ML.NET.Concrete
{
    public class PredictionService:IPredictionService
    {
        private readonly BigDataOrdersDbContext _context;
        private readonly MLContext _mLContext;

        public PredictionService(BigDataOrdersDbContext context, MLContext mLContext)
        {
            _context = context;
            _mLContext = mLContext;
        }

        public async  Task<object> GetGermanyCitiesSalesForecast()
        {
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2025, 12, 31);

            var germanyCityData=  _context.Orders
                .Include(o=>o.Customer)
                .Where(o=>o.OrderDate>=startDate && o.OrderDate<=endDate && o.Customer.CustomerCountry=="Almanya")
                .AsEnumerable()
                .GroupBy(o=> new
                {
                    o.Customer.CustomerCity,
                    year= o.OrderDate.Year,
                    Month=o.OrderDate.Month
                })
                .Select(g=> new
                {
                    City=g.Key.CustomerCity,
                    Year=g.Key.year,
                    Month=g.Key.Month,
                    Datekey =$"{g.Key.year}-{g.Key.Month:D2}",
                    OrderCount=g.Count()
                })
                .OrderBy(x=>x.City)
                .ThenBy(x=>x.Datekey)
                .ToList();

            var forecasts = new List<object>();

            foreach(var city in germanyCityData.Select(x => x.City).Distinct())
            {
                               var cityData = germanyCityData
                    .Where(x => x.City == city)
                    .Select((x, index) => new GermanyCitiesForecastData
                    {
                        City = x.City,
                        MonthIndex = index + 1,
                        OrderCount = x.OrderCount
                    }).ToList();

                if(cityData.Count < 4)
                    continue; 

                var dataView = _mLContext.Data.LoadFromEnumerable(cityData);
                var pipeline = _mLContext.Forecasting.ForecastBySsa(
                    outputColumnName: "ForecastedValues",
                    inputColumnName: nameof(GermanyCitiesForecastData.OrderCount),
                    windowSize: 12,
                    seriesLength: cityData.Count(),
                    trainSize: cityData.Count(),
                    horizon: 12,
                    confidenceLevel: 0.95f
                    );
                var model = pipeline.Fit(dataView);
                var engine = model.CreateTimeSeriesEngine<GermanyCitiesForecastData, GermanyCitiesForecastPrediction>(_mLContext);
                var prediction = engine.Predict();

                var yearlyForecast = (int)prediction.ForecastedValues.Sum();

                var year2024Count=germanyCityData
                    .Where(x=>x.City==city && x.Year==2024)
                    .Sum(x=>x.OrderCount);

                var year2025Count=germanyCityData
                    .Where(x=>x.City==city && x.Year==2025)
                    .Sum(x=>x.OrderCount);

                forecasts.Add(new
                {
                    City = city,
                    Year2024=year2024Count,
                    Year2025=year2025Count,
                    Month = "2026",
                    ForecastedCount = yearlyForecast,
                });

            }
            return forecasts;
        }
        public async Task<object> GetPaymentMethodForecastWith2025To2026Q1Async()
        {
            var startDate= new DateTime(2025, 1, 1);
            var endDate= new DateTime(2025, 12, 31);

            var monthlyPaymentData = _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .AsEnumerable()
                .GroupBy(o => new
                {
                    Month = new DateTime(o.OrderDate.Year, o.OrderDate.Month, 1),
                    o.PaymentMethod
                })
                .Select(g => new
                {
                    Month=g.Key.Month,
                    PaymentMethod=g.Key.PaymentMethod,
                    OrderCount = g.Count()
                })
                .OrderBy(x=>x.Month)
                .ToList();

            var forecasts=new List<object>();

            foreach(var method in monthlyPaymentData.Select(x => x.PaymentMethod).Distinct())
            {
                var methodData = monthlyPaymentData
                    .Where(x => x.PaymentMethod == method)
                    .Select((x, index) => new PaymentForecastData
                    {
                        PaymentMethod = x.PaymentMethod,
                        MonthIndex= index + 1,
                        OrderCount = x.OrderCount
                    });
                var dataView = _mLContext.Data.LoadFromEnumerable(methodData);


                var pipeline = _mLContext.Forecasting.ForecastBySsa(
                    outputColumnName: "ForecastedValues",
                    inputColumnName: nameof(PaymentForecastData.OrderCount),
                    windowSize: 4,
                    seriesLength: methodData.Count(),
                    trainSize: methodData.Count(),
                    horizon: 3,
                    confidenceLevel: 0.95f
                    );

                var model = pipeline.Fit(dataView);
                var engine=model.CreateTimeSeriesEngine<PaymentForecastData, PaymentForecastPrediction>(_mLContext);

                var prediction= engine.Predict();

                //2026 ocak şubat mart
                for(int i=0;i<prediction.ForecastedValues.Length;i++)
                {
                    forecasts.Add(new
                    {
                        PaymentMethod = method,
                        Month = new DateTime(2026, i + 1, 1).ToString("yyyy MMM"),
                        ForecastedCount = (int)prediction.ForecastedValues[i]
                    });
                }
            }

            return forecasts;
        }
    }
}
