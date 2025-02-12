﻿using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Services;


namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
        public async Task<IActionResult> GetPokemons()
        {
            try
            {
                var pokemons = await _pokemonService.GetPokemons();
                return Ok(pokemons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PokemonDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPokemonById(int id)
        {
            try
            {
                var pokemon = await _pokemonService.GetPokemonById(id);
                if (pokemon == null)
                {
                    return NotFound($"Pokemon with ID {id} not found.");
                }

                return Ok(pokemon);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPokemonRating(int id)
        {
            try
            {
                var pokemonExists = await _pokemonService.PokemonExist(id);
                if (!pokemonExists)
                    return NotFound($"Pokemon with ID {id} not found.");

                var rating = await _pokemonService.GetPokemonRating(id);
                return Ok(rating);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }
    }
}
