using System.Text.RegularExpressions;
using AutoMapper;
using FluentValidation;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Helper.Validators;
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
        private readonly IValidator<PokemonInputModel> _pokemonInputValidator;
        private readonly IValidator<PokemonUpdateModel> _pokemonUpdateValidator;

        public PokemonService
        (
            IPokemonRepository pokemonRepository, 
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IValidator<PokemonInputModel> pokemonInputValidator,
            IValidator<PokemonUpdateModel> pokemonUpdateValidator   
        )
        {
            this._pokemonRepository = pokemonRepository;
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
            this._pokemonInputValidator = pokemonInputValidator;
            this._pokemonUpdateValidator = pokemonUpdateValidator;

        }

        public async Task<ICollection<PokemonOutputModel>> GetPokemons()
        {
            var pokemons = await _pokemonRepository.GetPokemons();
            return _mapper.Map<List<PokemonOutputModel>>(pokemons);
        }

        public async Task<PokemonOutputModel> GetPokemonById(int id)
        {
            var pokemon = await _pokemonRepository.GetPokemonById(id);
            if (pokemon == null)
            {
                throw new KeyNotFoundException($"Pokemon with ID {id} not found.");
            }


            var pokemonOutput = _mapper.Map<PokemonOutputModel>(pokemon);

     
            pokemonOutput.Categories = pokemon.PokemonCategories?
                .Select(pc => new CategoryOutputModel { Id = pc.Category.Id, Name = pc.Category.Name })
                .ToList() ?? new List<CategoryOutputModel>();

            return pokemonOutput;
        }

        public async Task<decimal> GetPokemonRating(int pokemonId)
        {
            if (!await _pokemonRepository.PokemonExist(pokemonId))
            {
                throw new KeyNotFoundException($"Pokemon with ID {pokemonId} not found.");
            }

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

            var validationResult = await _pokemonInputValidator.ValidateAsync(pokemonInputModel);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Pokemon data is not valid: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }
            // Verifica se o nome já existe
            var pokemonNameAlreadyExist = await _pokemonRepository.PokemonNameAlreadyExists(pokemonInputModel.Name);
            if (pokemonNameAlreadyExist)
            {
                throw new ArgumentException("A Pokemon with the same name already exists.");
            }
            // Verifica se todas as categorias existem
            var validCategories = await _categoryRepository.GetCategoriesByIds(pokemonInputModel.CategoryIds);
            if (validCategories.Count != pokemonInputModel.CategoryIds.Count)
            {
                throw new ArgumentException("One or more categories do not exist.");
            }

            var pokemon = _mapper.Map<Pokemon>(pokemonInputModel);

            // Adiciona todas as categorias
            pokemon.PokemonCategories = validCategories
                .Select(category => new PokemonCategory { PokemonId = pokemon.Id, CategoryId = category.Id })
                .ToList();

            var createdPokemon = await _pokemonRepository.CreatePokemon(pokemon);
            return _mapper.Map<PokemonOutputModel>(createdPokemon);
        }

        public async Task<PokemonOutputModel> UpdatePokemon(int id, PokemonUpdateModel pokemonUpdateModel)
        {

            var existingPokemon = await _pokemonRepository.GetPokemonById(id);
            if (existingPokemon == null)
            {
                throw new ArgumentException("Pokemon not found.");
            }

            // Normaliza o nome (remove espaços extras)
            pokemonUpdateModel.Name = pokemonUpdateModel.Name.Trim();

            // Validação do modelo
            var validationResult = await _pokemonUpdateValidator.ValidateAsync(pokemonUpdateModel);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Pokemon data is not valid: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            // Verifica se já existe outro Pokémon com o mesmo nome
            bool nameAlreadyExists = await _pokemonRepository.PokemonNameAlreadyExists(pokemonUpdateModel.Name);
            if (nameAlreadyExists && existingPokemon.Name != pokemonUpdateModel.Name)
            {
                throw new ArgumentException("A Pokemon with the same name already exists.");
            }

            existingPokemon.Name = pokemonUpdateModel.Name;

            // Atualiza as categorias do Pokémon, se fornecidas
            if (pokemonUpdateModel.CategoryIds != null && pokemonUpdateModel.CategoryIds.Any())
            {
                var validCategories = await _categoryRepository.GetCategoriesByIds(pokemonUpdateModel.CategoryIds);

                if (validCategories.Count != pokemonUpdateModel.CategoryIds.Count)
                {
                    throw new ArgumentException("One or more categories do not exist.");
                }

                existingPokemon.PokemonCategories = validCategories
                    .Select(category => new PokemonCategory { PokemonId = existingPokemon.Id, CategoryId = category.Id })
                    .ToList();
            }

            var updatedPokemon = await _pokemonRepository.UpdatePokemon(existingPokemon);
            return _mapper.Map<PokemonOutputModel>(updatedPokemon);
        }

        public async Task<bool> DeletePokemon(int id)
        {
            var pokemonExists = await _pokemonRepository.PokemonExist(id);
            if (!pokemonExists)
            {
                throw new ArgumentException("Pokemon not found.");
            }

            return await _pokemonRepository.DeletePokemon(id);
        }
    }
}