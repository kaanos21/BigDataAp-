using Microsoft.AspNetCore.Mvc;
using BigDataApı.Repositories.Abstract;

namespace BigDataApı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("GetAnalysis")]
        public async Task<IActionResult> GetAnalysis()
        {
            string completion = await _reviewService.GetCustomerReviewWithOpenAIAnalysis();

            string[] sections = completion.Split(new[] { "<h4>" }, StringSplitOptions.None);

            var result = new
            {
                Profile = sections.Length > 1 ? "<h4>" + sections[1] : null,
                Sentiment = sections.Length > 2 ? "<h4>" + sections[2] : null,
                Character = sections.Length > 3 ? "<h4>" + sections[3] : null,
                Themes = sections.Length > 4 ? "<h4>" + sections[4] : null,
                Trend = sections.Length > 5 ? "<h4>" + sections[5] : null,
                Strategy = sections.Length > 6 ? "<h4>" + sections[6] : null,
                FullContent = completion 
            };

            return Ok(result);
        }
    }
}