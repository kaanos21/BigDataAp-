using Microsoft.ML.Data;

namespace BigDataApı.Dtos.LoyaltyMLDtos
{
    public class LoyaltyScoreMLPrediction
    {
        [ColumnName("Score")]
        public float LoyaltyScore { get; set; }
    }
}
