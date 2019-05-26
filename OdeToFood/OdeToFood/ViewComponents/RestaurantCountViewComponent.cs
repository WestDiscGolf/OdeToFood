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

        public RestaurantCountViewComponent(IRestaurantData restaurantData, IDistributedCache cache)
        {
            _restaurantData = restaurantData;
            _cache = cache;
        }

        public IViewComponentResult Invoke()
        {
            byte[] countBytes = _cache.Get("Count");
            int count;
            if (countBytes == null)
            {
                count = _restaurantData.GetCountOfRestaurants();
                var options = new DistributedCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(15) };
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
