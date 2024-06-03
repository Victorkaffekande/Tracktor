using SharedModels;

namespace LocationRetrievalApi.repo;

public interface ILocationRetrievalApiRepo
{
    public List<Location> GetLocationFromFleet(Guid fleetId, DateTime fromTimestamp, DateTime toTimestamp,
        string hourDate);

    public List<Location> GetLocationFromVehicle(Guid vehicleId, string weekYear, DateTime fromTimestamp,
        DateTime toTimestamp);

    public List<Location> TestVehicle();

    public List<Location> TestFleet();
}