using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Services;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerService _reviewerService;

        public ReviewerController(IReviewerService reviewerService)
        {
            _reviewerService = reviewerService;
        }

        // GET: api/Reviewer
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<ReviewerOutputModel>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetReviewers()
        {
            try
            {
                var reviewers = await _reviewerService.GetReviewers();
                return Ok(reviewers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Reviewer/1
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(typeof(ReviewerOutputModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetReviewerById(int reviewerId)
        {
            try
            {
                var reviewer = await _reviewerService.GetReviewerById(reviewerId);
                if (reviewer == null)
                {
                    return NotFound("Reviewer not found.");
                }
                return Ok(reviewer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Reviewer/1/reviews
        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(typeof(ICollection<ReviewOutputModel>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetReviewsByReviewer(int reviewerId)
        {
            try
            {
                var reviews = await _reviewerService.GetReviewsByReviewer(reviewerId);
                if (reviews == null || !reviews.Any())
                {
                    return NotFound("No reviews found for this reviewer.");
                }
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
