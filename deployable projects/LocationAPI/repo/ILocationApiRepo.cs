using SharedModels;

namespace LocationAPI.repo;

public interface ILocationApiRepo
{
    public List<Location> GetLocationFromFleet(Guid fleetId, DateTime fromTimestamp, DateTime toTimestamp,
        string hourDate);

    public List<Location> GetLocationFromVehicle(Guid vehicleId, string weekYear, DateTime fromTimestamp,
        DateTime toTimestamp);

    public List<Location> TestVehicle();

    public List<Location> TestFleet();
}