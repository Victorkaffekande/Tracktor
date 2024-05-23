using SharedModels;

namespace LocationAPI.repo;

public interface ILocationApiRepo
{
    public List<Location> GetLocationFromCompany(Guid fleetId, DateTime fromTimestamp, DateTime toTimestamp,
        string hourDate);

    public List<Location> GetLocationFromTractor(Guid vehicleId, string weekYear, DateTime fromTimestamp,
        DateTime toTimestamp);

    public List<Location> TestTractor();

    public List<Location> TestCompany();
}