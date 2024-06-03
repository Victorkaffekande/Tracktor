using LocationRetrievalApi.repo;
using SharedModels;

namespace LocationRetrievalApi.Services;

public class LocationRetrievalApiService : ILocationRetrievalApiService
{

    private readonly ILocationRetrievalApiRepo _locationRetrievalApiRepo;
    public LocationRetrievalApiService(ILocationRetrievalApiRepo locationRetrievalApiRepo)
    {
        _locationRetrievalApiRepo = locationRetrievalApiRepo;
    }

    public List<Location> GetLocationFromFleet(Guid fleetId, DateTime fromTimestamp, DateTime toTimestamp,string hourDate)
    {
        return _locationRetrievalApiRepo.GetLocationFromFleet(fleetId, fromTimestamp, toTimestamp, hourDate);
    }

    public List<Location> GetLocationFromVehicle(Guid vehicleId, string weekYear, DateTime fromTimestamp,
        DateTime toTimestamp)
    {
        return _locationRetrievalApiRepo.GetLocationFromVehicle(vehicleId, weekYear, fromTimestamp, toTimestamp);
    }

    public List<Location> TestVehicle()
    {
        return _locationRetrievalApiRepo.TestVehicle();
    }
    
    public List<Location> TestFleet()
    {
        return _locationRetrievalApiRepo.TestFleet();
    }


}