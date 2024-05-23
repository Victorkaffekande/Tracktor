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

    public List<Location> GetLocationFromCompany(Guid fleetId, DateTime fromTimestamp, DateTime toTimestamp,string hourDate)
    {
        return _locationApiRepo.GetLocationFromCompany(fleetId, fromTimestamp, toTimestamp, hourDate);
    }

    public List<Location> GetLocationFromTractor(Guid vehicleId, string weekYear, DateTime fromTimestamp,
        DateTime toTimestamp)
    {
        return _locationApiRepo.GetLocationFromTractor(vehicleId, weekYear, fromTimestamp, toTimestamp);
    }

    public List<Location> TestTractor()
    {
        return _locationApiRepo.TestTractor();
    }
    
    public List<Location> TestCompany()
    {
        return _locationApiRepo.TestCompany();
    }


}