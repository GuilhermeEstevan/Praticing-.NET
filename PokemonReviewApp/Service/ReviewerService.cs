using AutoMapper;
using FluentValidation;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Interfaces.Services;
using PokemonReviewApp.Models;


namespace PokemonReviewApp.Service
{
    public class ReviewerService : IReviewerService
    {

        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ReviewerInputModel> _reviewerValidator;


        public ReviewerService
        (
            IReviewerRepository reviewerRepository,
            IMapper mapper,
            IValidator<ReviewerInputModel> reviewerValidator)
        {

            this._reviewerRepository = reviewerRepository;
            this._mapper = mapper;
            this._reviewerValidator = reviewerValidator;
       
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

        public async Task<ReviewerOutputModel> CreateReviewer(ReviewerInputModel reviewerInputModel)
        {

            reviewerInputModel.FirstName = Utils.Utils.RemoveAccents(reviewerInputModel.FirstName.Trim());
            reviewerInputModel.LastName = Utils.Utils.RemoveAccents(reviewerInputModel.LastName.Trim());

            var validationResult = await _reviewerValidator.ValidateAsync(reviewerInputModel);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Reviewer data is not valid: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var reviewerNameAlreadyExists = await _reviewerRepository.ReviewerNameAlreadyExists(reviewerInputModel.FirstName, reviewerInputModel.LastName);
            if (reviewerNameAlreadyExists)
            {
                throw new ArgumentException("A Reviewer with the same FirstName and LastName already exists.");
            }

            var reviewer = _mapper.Map<Reviewer>(reviewerInputModel);
            var createdReviewer = await _reviewerRepository.CreateReviewer(reviewer);
            return _mapper.Map<ReviewerOutputModel>(createdReviewer);
        }
    }
}