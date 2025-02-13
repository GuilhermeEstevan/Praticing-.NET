using AutoMapper;
using FluentValidation;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Interfaces.Services;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<PokemonInputModel> _pokemonValidator;

        public PokemonService
        (
            IPokemonRepository pokemonRepository, 
            ICategoryRepository categoryRepository,
            IMapper mapper, 
            IValidator<PokemonInputModel> pokemonValidator
        )
        {
            this._pokemonRepository = pokemonRepository;
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
            this._pokemonValidator = pokemonValidator;
        }

        public async Task<ICollection<PokemonOutputModel>> GetPokemons()
        {
            var pokemons = await _pokemonRepository.GetPokemons();
            return _mapper.Map<List<PokemonOutputModel>>(pokemons);
        }

        public async Task<PokemonOutputModel> GetPokemonById(int id)
        {
            var pokemon = await _pokemonRepository.GetPokemonById(id);
            return _mapper.Map<PokemonOutputModel>(pokemon);
        }

        public async Task<decimal> GetPokemonRating(int pokemonId)
        {
            return await _pokemonRepository.GetPokemonRating(pokemonId);
        }

        public async Task<bool> PokemonExist(int id)
        {
            return await _pokemonRepository.PokemonExist(id);
        }

        public async Task<PokemonOutputModel> GetPokemonByName(string name)
        {
            var pokemon = await _pokemonRepository.GetPokemonByName(name);
            return _mapper.Map<PokemonOutputModel>(pokemon);
        }

        public async Task<PokemonOutputModel> CreatePokemon(PokemonInputModel pokemonInputModel)
        {
            var cleanedName = pokemonInputModel.Name.Trim().ToLower();
            pokemonInputModel.Name = cleanedName;

            var validationResult = await _pokemonValidator.ValidateAsync(pokemonInputModel);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Pokemon data is not valid: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }
            // Check Name
            var pokemonNameAlreadyExist = await _pokemonRepository.PokemonNameAlreadyExists(pokemonInputModel.Name);
            if (pokemonNameAlreadyExist)
            {
                throw new ArgumentException("A Pokemon with the same name already exists.");
            }
            // Get category
            var category = await _categoryRepository.GetCategoryById(pokemonInputModel.CategoryId);
            if (category == null)
            {
                throw new ArgumentException("The specified Category does not exists.");
            }

            var pokemon = _mapper.Map<Pokemon>(pokemonInputModel);
            // Add Category
            pokemon.PokemonCategories =
            [
                new PokemonCategory
                {
                    PokemonId = pokemon.Id,
                    CategoryId = pokemonInputModel.CategoryId
                }
            ];

            var createdPokemon = await _pokemonRepository.CreatePokemon(pokemon);
            return _mapper.Map<PokemonOutputModel>(createdPokemon);
        }
    }
}