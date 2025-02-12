using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Interfaces.Services;

namespace PokemonReviewApp.Service
{
    public class ReviewerService : IReviewerService
    {

        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerService(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<ReviewerOutputModel>> GetReviewers()
        {
            var reviewers = await _reviewerRepository.GetReviewers();
            return _mapper.Map<ICollection<ReviewerOutputModel>>(reviewers);
        }

        public async Task<ReviewerOutputModel> GetReviewerById(int reviewerId)
        {
            var reviewer = await _reviewerRepository.GetReviewerById(reviewerId);
            if (reviewer == null)
                return null;
            return _mapper.Map<ReviewerOutputModel>(reviewer);
        }

        public async Task<ICollection<ReviewSummaryWithReviewerNameOutputModel>> 
            GetReviewsByReviewer(int reviewerId)
        {
            var reviews = await _reviewerRepository.GetReviewsByReviewer(reviewerId);
            return _mapper.Map<ICollection<ReviewSummaryWithReviewerNameOutputModel>>(reviews);
        }

        public async Task<bool> ReviewerExists(int reviewerId)
        {
            return await _reviewerRepository.ReviewerExists(reviewerId);
        }
        }
}