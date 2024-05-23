using SharedModels;

namespace LocationAPI.Services;

public interface ILocationApiService
{
    public List<Location> GetLocationFromCompany(Guid fleetId, DateTime fromTimestamp, DateTime toTimestamp,
        string hourDate);

    public List<Location> GetLocationFromTractor(Guid vehicleId, string weekYear, DateTime fromTimestamp,
        DateTime toTimestamp);

    public List<Location> TestTractor();

    public List<Location> TestCompany();
}