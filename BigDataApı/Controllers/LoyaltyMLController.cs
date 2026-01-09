using BigDataApı.ML.NET.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BigDataApı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoyaltyMLController : ControllerBase
    {
        private readonly ILoyaltyService _loyaltyService;

        public LoyaltyMLController(ILoyaltyService loyaltyService)
        {
            _loyaltyService = loyaltyService;
        }

        [HttpGet("GetItalyLoyaltyScores")]
        public async Task<IActionResult> GetItalyLoyaltyScores()
        {
            // Servis içindeki Task<object> dönen metodu çağırıyoruz
            var result = await _loyaltyService.GetItalyLoyaltyScoreItalyWithML();

            if (result == null)
            {
                return NotFound("Veri bulunamadı veya model eğitilemedi.");
            }

            return Ok(result);
        }
    }
}