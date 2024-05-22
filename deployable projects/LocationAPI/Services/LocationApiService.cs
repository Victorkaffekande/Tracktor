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

    public List<Location> GetLocationFromCompany(Guid companyId, DateTime fromTimestamp, DateTime toTimestamp,string monthYear)
    {
        return _locationApiRepo.GetLocationFromCompany(companyId, fromTimestamp, toTimestamp, monthYear);
    }

    public List<Location> GetLocationFromTractor(Guid tractorId, string monthYear, DateTime fromTimestamp,
        DateTime toTimestamp)
    {
        return _locationApiRepo.GetLocationFromTractor(tractorId, monthYear, fromTimestamp, toTimestamp);
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