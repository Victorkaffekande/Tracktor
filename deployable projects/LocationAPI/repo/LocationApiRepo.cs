
using SharedModels;
using ISession = Cassandra.ISession;

namespace LocationAPI.repo;

public class LocationApiRepo : ILocationApiRepo
{
    private readonly ISession _cassandraSession;
    
    public LocationApiRepo(ISession cassandraSession)
    {
        _cassandraSession = cassandraSession;
    }
    
    public List<Location> GetLocationFromCompany(Guid companyId, DateTime fromTimestamp, DateTime toTimestamp,string monthYear)
    {
        var selectCql = @"
        SELECT * 
        FROM Locations_By_Company 
        WHERE company_id = ?
        AND month_year = ?
        AND timestamp >= ? 
        AND timestamp <= ?";
        
        var statement = _cassandraSession.Prepare(selectCql).Bind(companyId, monthYear,fromTimestamp, toTimestamp);
        var rs = _cassandraSession.Execute(statement);
        var results = rs.Select(row => new Location
        {
            CompanyId = row.GetValue<Guid>("company_id"),
            TractorId = row.GetValue<Guid>("tractor_id"),
            Timestamp = row.GetValue<DateTime>("timestamp"),
            Latitude = row.GetValue<double>("latitude"),
            Longitude = row.GetValue<double>("longitude")
        }).ToList();

        return results;
    }
    
    public List<Location> GetLocationFromTractor(Guid tractorId, string monthYear, DateTime fromTimestamp, DateTime toTimestamp)
    {
        var selectCql = @"
        SELECT * 
        FROM Locations_By_Tractor 
        WHERE tractor_id = ? 
        AND month_year = ? 
        AND timestamp >= ? 
        AND timestamp <= ?";
        
        var statement = _cassandraSession.Prepare(selectCql).Bind(tractorId, monthYear, fromTimestamp, toTimestamp);
        var rs = _cassandraSession.Execute(statement);
        var results = rs.Select(row => new Location
        {
            TractorId = row.GetValue<Guid>("tractor_id"),
            Timestamp = row.GetValue<DateTime>("timestamp"),
            Latitude = row.GetValue<double>("latitude"),
            Longitude = row.GetValue<double>("longitude")
        }).ToList();

        return results;
    }
    
    public List<Location> TestTractor()
    {
        var selectCql = @"
        SELECT * 
        FROM Locations_By_Tractor 
        ";
        
        var rs = _cassandraSession.Execute(selectCql);
        var results = rs.Select(row => new Location
        {
            TractorId = row.GetValue<Guid>("tractor_id"),
            Timestamp = row.GetValue<DateTime>("timestamp"),
            Latitude = row.GetValue<double>("latitude"),
            Longitude = row.GetValue<double>("longitude")
        }).ToList();

        return results;
    }
    public List<Location> TestCompany()
    {
        var selectCql = @"
        SELECT * 
        FROM Locations_By_Company
        ";
        
        var rs = _cassandraSession.Execute(selectCql);
        var results = rs.Select(row => new Location
        {
            TractorId = row.GetValue<Guid>("tractor_id"),
            Timestamp = row.GetValue<DateTime>("timestamp"),
            Latitude = row.GetValue<double>("latitude"),
            Longitude = row.GetValue<double>("longitude")
        }).ToList();

        return results;
    }
    
}