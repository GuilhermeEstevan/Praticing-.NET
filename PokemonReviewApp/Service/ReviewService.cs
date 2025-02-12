using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Interfaces.Services;

namespace PokemonReviewApp.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
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
    }
}