using OdeToFood.Core;


namespace OdeToFood.Data
{
    public interface IRestaurantData : IReadOnlyRestaurantData
    {
        Restaurant Update(Restaurant updatedRestaurant);
        Restaurant Add(Restaurant newRestaurant);
        Restaurant Delete(int id);
        int Commit();
    }
}
