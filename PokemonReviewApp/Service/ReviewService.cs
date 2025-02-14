using AutoMapper;
using FluentValidation;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Interfaces.Services;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ReviewInputModel> _reviewInputValidator;
        private readonly IValidator<ReviewUpdateModel> _reviewUpdateValidator;

        public ReviewService
        (   IReviewRepository reviewRepository, 
            IPokemonRepository pokemonRepository, 
            IReviewerRepository reviewerRepository, 
            IMapper mapper,
            IValidator<ReviewInputModel> reviewInputValidator,
            IValidator<ReviewUpdateModel> reviewUpdateValidator
        )
        {
            this._reviewRepository = reviewRepository;
            this._pokemonRepository = pokemonRepository;
            this._reviewerRepository = reviewerRepository;
            this._mapper = mapper;
            this._reviewInputValidator = reviewInputValidator;
            this._reviewUpdateValidator = reviewUpdateValidator;
        }

        public async Task<ICollection<ReviewOutputModel>> GetReviews()
        {
            var reviews = await _reviewRepository.GetReviews();
            return _mapper.Map<ICollection<ReviewOutputModel>>(reviews);
        }

        public async Task<ReviewOutputModel> GetReview(int id)
        {
            var review = await _reviewRepository.GetReview(id);
            if (review == null) return null; 
            return _mapper.Map<ReviewOutputModel>(review);
        }

        public async Task<ICollection<ReviewOutputModel>> GetReviewsByPokemon(int pokemonId)
        {
            var reviews = await _reviewRepository.GetReviewsByPokemon(pokemonId);
            return _mapper.Map<ICollection<ReviewOutputModel>>(reviews);
        }

        public async Task<bool> ReviewExists(int pokemonId)
        {
            return await _reviewRepository.ReviewExists(pokemonId);
        }

        public async Task<ReviewSummaryWithReviewerNameOutputModel> CreateReview(ReviewInputModel reviewInputModel)
        {
            reviewInputModel.Title = reviewInputModel.Title.Trim();
            reviewInputModel.Text = reviewInputModel.Text.Trim();

            var validationResult = await _reviewInputValidator.ValidateAsync(reviewInputModel);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Pokemon data is not valid: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var pokemon = await _pokemonRepository.GetPokemonById(reviewInputModel.PokemonId);
            if (pokemon == null)
            {
                throw new ArgumentException("Pokemon not found.");
            }

            // Verificar se o Reviewer existe
            var reviewer = await _reviewerRepository.GetReviewerById(reviewInputModel.ReviewerId);
            if (reviewer == null)
            {
                throw new ArgumentException("Reviewer not found.");
            }

            // Mapear e Adicionar pokemon e Reviewer
            var review = _mapper.Map<Review>(reviewInputModel);
            review.Pokemon = pokemon;
            review.Reviewer = reviewer;

            var createdReview = await _reviewRepository.CreateReview(review);

            return _mapper.Map<ReviewSummaryWithReviewerNameOutputModel>(createdReview);
        }

        public async Task<ReviewOutputModel> UpdateReview(int id, ReviewUpdateModel reviewUpdateModel)
        {
            var existingReview = await _reviewRepository.GetReview(id);
            if (existingReview == null)
            {
                throw new ArgumentException("Review not found.");
            }

            // Normaliza os valores para evitar espaços desnecessários
            reviewUpdateModel.Title = reviewUpdateModel.Title.Trim();
            reviewUpdateModel.Text = reviewUpdateModel.Text.Trim();

            // Validação dos dados
            var validationResult = await _reviewUpdateValidator.ValidateAsync(reviewUpdateModel);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Review data is not valid: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            existingReview.Title = reviewUpdateModel.Title;
            existingReview.Text = reviewUpdateModel.Text;
            existingReview.Rating = reviewUpdateModel.Rating;

            var updatedReview = await _reviewRepository.UpdateReview(existingReview);
            return _mapper.Map<ReviewOutputModel>(updatedReview);
        }
    }
}