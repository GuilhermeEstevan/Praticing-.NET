﻿using Microsoft.AspNetCore.Mvc;
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
                return Ok(reviewer);
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


        // POST: api/Reviewer
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ReviewerOutputModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateReviewer([FromBody] ReviewerInputModel reviewerInputModel)
        {
            try
            { 
                var createdReviewer = await _reviewerService.CreateReviewer(reviewerInputModel);

                return CreatedAtAction(nameof(GetReviewerById), new { reviewerId = createdReviewer.Id }, createdReviewer);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Retorna erro genérico para outros tipos de exceções
                return StatusCode(500, new { error = "Internal server error: " + ex.Message });
            }
        }


        [HttpPut]
        [ProducesResponseType(200, Type = typeof(ReviewerOutputModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ReviewerOutputModel>> UpdateReviewer([FromQuery] int id, [FromBody] ReviewerUpdateModel reviewerUpdateModel)
        {
            try
            {
                var updatedReviewer = await _reviewerService.UpdateReviewer(id, reviewerUpdateModel);
                return Ok(updatedReviewer);
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

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)] 
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteReviewer(int reviewerId)
        {
            try
            {
                var deleted = await _reviewerService.DeleteReviewer(reviewerId);
                if (!deleted)
                {
                    return NotFound(new { error = "Reviewer not found." });
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
