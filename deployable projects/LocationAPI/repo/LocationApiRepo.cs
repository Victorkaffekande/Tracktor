
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
    
    public List<Location> GetLocationFromCompany(Guid fleetId, DateTime fromTimestamp, DateTime toTimestamp,string hourDate)
    {
        var selectCql = @"
        SELECT * 
        FROM Locations_By_Company 
        WHERE fleet_id = ?
        AND hour_date = ?
        AND timestamp >= ? 
        AND timestamp <= ?";
        
        var statement = _cassandraSession.Prepare(selectCql).Bind(fleetId, hourDate,fromTimestamp, toTimestamp);
        var rs = _cassandraSession.Execute(statement);
        var results = rs.Select(row => new Location
        {
            FleetId = row.GetValue<Guid>("fleet_id"),
            VehicleId = row.GetValue<Guid>("vehicle_id"),
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
        WHERE vehicle_id = ? 
        AND week_year = ? 
        AND timestamp >= ? 
        AND timestamp <= ?";
        
        var statement = _cassandraSession.Prepare(selectCql).Bind(tractorId, monthYear, fromTimestamp, toTimestamp);
        var rs = _cassandraSession.Execute(statement);
        var results = rs.Select(row => new Location
        {
            VehicleId = row.GetValue<Guid>("vehicle_id"),
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
            VehicleId = row.GetValue<Guid>("vehicle_id"),
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
            VehicleId = row.GetValue<Guid>("vehicle_id"),
            Timestamp = row.GetValue<DateTime>("timestamp"),
            Latitude = row.GetValue<double>("latitude"),
            Longitude = row.GetValue<double>("longitude")
        }).ToList();

        return results;
    }
    
}