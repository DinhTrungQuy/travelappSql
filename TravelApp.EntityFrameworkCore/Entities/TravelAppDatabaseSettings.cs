namespace TravelAppAPI.Models
{
    public class TravelAppDatabaseSettings
    {
        public string PlacesCollectionName { get; set; } = String.Empty;
        public string UsersCollectionName { get; set; } = String.Empty;
        public string BookingsCollectionName { get; set; } = String.Empty;
        public string WishlistsCollectionName { get; set; } = String.Empty;
        public string RatingsCollectionName { get; set; } = String.Empty;
        public string DashboardsCollectionName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;

    }
}
