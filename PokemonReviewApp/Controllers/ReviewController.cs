using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Services;

namespace PokemonReviewApp.Controllers
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

        // GET: api/review
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewOutputModel>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetReviews()
        {
            try
            {
                var reviews = await _reviewService.GetReviews();
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/review/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ReviewOutputModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetReview(int id)
        {
            try
            {
                var review = await _reviewService.GetReview(id);
                return Ok(review);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/review/pokemon/{pokemonId}
        [HttpGet("pokemon/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewOutputModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetReviewsByPokemon(int pokemonId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByPokemon(pokemonId);
                return Ok(reviews);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        // POST: api/review
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ReviewSummaryWithReviewerNameOutputModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateReview([FromBody] ReviewInputModel reviewInputModel)
        {
            try
            {
                var createdReview = await _reviewService.CreateReview(reviewInputModel);
                return CreatedAtAction(nameof(GetReview), new { id = createdReview.Id }, createdReview);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error: " + ex.Message });
            }
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(ReviewOutputModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ReviewOutputModel>> UpdateReview([FromQuery] int id, [FromBody] ReviewUpdateModel reviewUpdateModel)
        {
            try
            {
                var updatedReview = await _reviewService.UpdateReview(id, reviewUpdateModel);
                return Ok(updatedReview);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(204)] 
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            try
            {
                var deleted = await _reviewService.DeleteReview(reviewId);
                if (!deleted)
                {
                    return NotFound(new { error = "Review not found." });
                }

                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

    }
}
