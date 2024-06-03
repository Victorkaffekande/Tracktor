using SharedModels;

namespace LocationRetrievalApi.Services;

public interface ILocationRetrievalApiService
{
    public List<Location> GetLocationFromFleet(Guid fleetId, DateTime fromTimestamp, DateTime toTimestamp,
        string hourDate);

    public List<Location> GetLocationFromVehicle(Guid vehicleId, string weekYear, DateTime fromTimestamp,
        DateTime toTimestamp);

    public List<Location> TestVehicle();

    public List<Location> TestFleet();
}