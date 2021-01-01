using Microsoft.AspNetCore.Mvc;
using OdeToFood.Data;

namespace OdeToFood.ViewComponents
{
    public class RestaurantCountViewComponent : ViewComponent
    {
        private readonly IReadOnlyRestaurantData _restaurantData;
        
        public RestaurantCountViewComponent(IReadOnlyRestaurantData restaurantData)
        {
            _restaurantData = restaurantData;
        }

        public IViewComponentResult Invoke()
        {
            // ViewComponent does not care what it gets as long as it implements the interface and gives the answer
            var count = _restaurantData.GetCountOfRestaurants();
            return View(count);
        }
    }
}
