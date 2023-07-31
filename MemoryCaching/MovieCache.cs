using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using DataAccess.Models;

namespace MemoryCachingLibrary
{
    public class MovieMemoryCache
    {
        private readonly IMemoryCache _memoryCache;
        private const string WatchlistCacheKey = "Watchlist";

        public MovieMemoryCache(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

        public Watchlist? GetOrCreateWatchlist()
        {
            if (!_memoryCache.TryGetValue(WatchlistCacheKey, out Watchlist? watchlist))
            {
                watchlist = new Watchlist
                {
                    Movies = new List<Movie>()
                };
                _memoryCache.Set(WatchlistCacheKey, watchlist, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Adjust the expiration time as needed
                });
            }

            return watchlist;
        }

        public void SetWatchlist(Watchlist watchlist)
        {
            _memoryCache.Set(WatchlistCacheKey, watchlist, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Adjust the expiration time as needed
            });
        }

    }

    public class Watchlist
    {
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
