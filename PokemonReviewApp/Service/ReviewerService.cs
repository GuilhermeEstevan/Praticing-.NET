using System.Text.RegularExpressions;
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
        private readonly IValidator<ReviewerInputModel> _reviewerInputValidator;
        private readonly IValidator<ReviewerUpdateModel> _reviewerUpdateValidator;


        public ReviewerService
        (
            IReviewerRepository reviewerRepository,
            IMapper mapper,
            IValidator<ReviewerInputModel> reviewerInputValidator,
            IValidator<ReviewerUpdateModel> reviewerUpdateValidator
            )
        {

            this._reviewerRepository = reviewerRepository;
            this._mapper = mapper;
            this._reviewerInputValidator = reviewerInputValidator;
            this._reviewerUpdateValidator = reviewerUpdateValidator;
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
            {
                throw new KeyNotFoundException($"Reviewer with ID {reviewerId} not found.");
            }

            return _mapper.Map<ReviewerOutputModel>(reviewer);
        }

        public async Task<ICollection<ReviewSummaryWithReviewerNameOutputModel>> 
            GetReviewsByReviewer(int reviewerId)
        {
            var reviews = await _reviewerRepository.GetReviewsByReviewer(reviewerId);

            if (reviews == null || !reviews.Any())
            {
                throw new KeyNotFoundException($"No reviews found for Reviewer with ID {reviewerId}.");
            }

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

            var validationResult = await _reviewerInputValidator.ValidateAsync(reviewerInputModel);

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

        public async Task<ReviewerOutputModel> UpdateReviewer(int id, ReviewerUpdateModel reviewerUpdateModel)
        {
            var existingReviewer = await _reviewerRepository.GetReviewerById(id);
            if (existingReviewer == null)
            {
                throw new ArgumentException("Reviewer not found.");
            }

            // Normaliza os valores
            reviewerUpdateModel.FirstName = Regex.Replace(reviewerUpdateModel.FirstName.Trim(), @"\s+", " ").ToLower();
            reviewerUpdateModel.LastName = Regex.Replace(reviewerUpdateModel.LastName.Trim(), @"\s+", " ").ToLower();

            // Validação dos dados
            var validationResult = await _reviewerUpdateValidator.ValidateAsync(reviewerUpdateModel);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Reviewer data is not valid: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            existingReviewer.FirstName = reviewerUpdateModel.FirstName;
            existingReviewer.LastName = reviewerUpdateModel.LastName;

            var updatedReviewer = await _reviewerRepository.UpdateReviewer(existingReviewer);
            return _mapper.Map<ReviewerOutputModel>(updatedReviewer);
        }

        public async Task<bool> DeleteReviewer(int reviewerId)
        {
            var reviewerExists = await _reviewerRepository.ReviewerExists(reviewerId);
            if (!reviewerExists)
            {
                throw new KeyNotFoundException($"Reviewer with ID {reviewerId} not found.");
            }

            return await _reviewerRepository.DeleteReviewer(reviewerId);
        }

    }
}