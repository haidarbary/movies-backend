using Microsoft.AspNetCore.Mvc;
using BackendAPI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using DataAccess.Data;
using DataAccess.Models;
using MemoryCachingLibrary;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly MovieMemoryCache _memoryCache; // from MemoryCaching Class library
        private const string WatchlistCacheKey = "Watchlist";

        public MoviesController(AppDbContext dbContext, MovieMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        // Endpoint to add a movie to the watchlist
        [HttpPost("add")]
        public IActionResult AddToWatchlist([FromBody] Movie movieToAdd)
        {
            // Get or create the watchlist
            var watchlist = _memoryCache.GetOrCreateWatchlist();

            // Check if the movieToAdd is null
            if (movieToAdd != null)
            {
                // Check if the movie exists in the database
                var movie = _dbContext.Movies?.FirstOrDefault(m => m.Id == movieToAdd.Id);

                if (movie == null)
                {
                    movie = new Movie
                    {
                        Id = movieToAdd.Id,
                        Title = movieToAdd.Title,
                        Overview = movieToAdd.Overview,
                        Release_date = movieToAdd.Release_date,
                        Poster_path = movieToAdd.Poster_path
                    };
                    _dbContext.Movies.Add(movie);
                    _dbContext.SaveChanges();
                }

                // Add the movie to the watchlist
                watchlist.Movies.Add(movie);
                _memoryCache.SetWatchlist(watchlist); 
                return Ok("Movie added to watchlist");
            }

            return BadRequest("Invalid movie data");
        }

        // Endpoint to remove a movie from the watchlist
        [HttpDelete("remove/{movieId}")]
        public IActionResult RemoveMovieFromWatchlist(int movieId)
        {
            // Get the watchlist from memory cache
            var watchlist = _memoryCache.GetOrCreateWatchlist();

            // Find the movie in the watchlist
            var movie = watchlist?.Movies.FirstOrDefault(m => m.Id == movieId);
            if (movie == null)
            {
                return NotFound(); // Movie not found in the watchlist
            }

            watchlist.Movies.Remove(movie);
            _memoryCache.SetWatchlist(watchlist); 
            return Ok("Movie removed from watchlist");
        }

        [HttpGet("get-watchlist")]
        public IActionResult GetWatchlist()
        {
            // Get or create the watchlist
            var watchlist = _memoryCache.GetOrCreateWatchlist() ?? new Watchlist { Movies = new List<Movie>() };

            return Ok(watchlist.Movies);
        }
    }
}
