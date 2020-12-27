using Microsoft.AspNetCore.Mvc;
using OdeToFood.Data;
using System;
using Microsoft.Extensions.Caching.Distributed;

namespace OdeToFood.ViewComponents
{
    public class RestaurantCountViewComponent : ViewComponent
    {
        private readonly IRestaurantData _restaurantData;
        private readonly IDistributedCache _cache;
        private readonly RestaurantCountCacheSettings _settings;

        public RestaurantCountViewComponent(IRestaurantData restaurantData, IDistributedCache cache, RestaurantCountCacheSettings settings)
        {
            _restaurantData = restaurantData;
            _cache = cache;
            _settings = settings;
        }

        public IViewComponentResult Invoke()
        {
            byte[] countBytes = _cache.Get("Count");
            int count;
            if (countBytes == null)
            {
                count = _restaurantData.GetCountOfRestaurants();
                var options = new DistributedCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(_settings.Seconds) };
                _cache.Set("Count", BitConverter.GetBytes(count), options);
            }
            else
            {
                count = BitConverter.ToInt32(countBytes);
            }
            return View(count);
        }
    }
}
