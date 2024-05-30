using LocationAPI.repo;
using SharedModels;

namespace LocationAPI.Services;

public class LocationApiService : ILocationApiService
{

    private readonly ILocationApiRepo _locationApiRepo;
    public LocationApiService(ILocationApiRepo locationApiRepo)
    {
        _locationApiRepo = locationApiRepo;
    }

    public List<Location> GetLocationFromFleet(Guid fleetId, DateTime fromTimestamp, DateTime toTimestamp,string hourDate)
    {
        return _locationApiRepo.GetLocationFromFleet(fleetId, fromTimestamp, toTimestamp, hourDate);
    }

    public List<Location> GetLocationFromVehicle(Guid vehicleId, string weekYear, DateTime fromTimestamp,
        DateTime toTimestamp)
    {
        return _locationApiRepo.GetLocationFromVehicle(vehicleId, weekYear, fromTimestamp, toTimestamp);
    }

    public List<Location> TestVehicle()
    {
        return _locationApiRepo.TestVehicle();
    }
    
    public List<Location> TestFleet()
    {
        return _locationApiRepo.TestFleet();
    }


}