using SharedModels;

namespace LocationAPI.repo;

public interface ILocationApiRepo
{
    public List<Location> GetLocationFromCompany(Guid companyId, DateTime fromTimestamp, DateTime toTimestamp,
        string monthYear);

    public List<Location> GetLocationFromTractor(Guid tractorId, string monthYear, DateTime fromTimestamp,
        DateTime toTimestamp);

    public List<Location> TestTractor();

    public List<Location> TestCompany();
}