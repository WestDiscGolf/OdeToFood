using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using OdeToFood.Core;

namespace OdeToFood.Data
{
    public class CachedRestaurantData : IReadOnlyRestaurantData
    {
        private readonly IReadOnlyRestaurantData _repository;
        private readonly IDistributedCache _cache;
        private readonly RestaurantCacheSettings _settings;

        public CachedRestaurantData(IReadOnlyRestaurantData repository, IDistributedCache cache, RestaurantCacheSettings settings)
        {
            _repository = repository;
            _cache = cache;
            _settings = settings;
        }

        public IEnumerable<Restaurant> GetRestaurantsByName(string name)
        {
            return _repository.GetRestaurantsByName(name);
        }

        public Restaurant GetById(int id)
        {
            return _repository.GetById(id);
        }

        public int GetCountOfRestaurants()
        {
            byte[] countBytes = _cache.Get("Count");
            int count;
            if (countBytes == null)
            {
                count = _repository.GetCountOfRestaurants();
                var options = new DistributedCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(_settings.Seconds) };
                _cache.Set("Count", BitConverter.GetBytes(count), options);
            }
            else
            {
                count = BitConverter.ToInt32(countBytes);
            }

            return count; 
        }
    }
}