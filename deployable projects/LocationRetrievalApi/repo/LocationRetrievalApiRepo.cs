using SharedModels;
using ISession = Cassandra.ISession;


namespace LocationRetrievalApi.repo;

public class LocationRetrievalApiRepo : ILocationRetrievalApiRepo
{
    private readonly ISession _cassandraSession;
    
    public LocationRetrievalApiRepo(ISession cassandraSession)
    {
        _cassandraSession = cassandraSession;
    }
    
    public List<Location> GetLocationFromFleet(Guid fleetId, DateTime fromTimestamp, DateTime toTimestamp,string hourDate)
    {
        var selectCql = @"
        SELECT * 
        FROM Locations_By_Fleet 
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
    
    public List<Location> GetLocationFromVehicle(Guid vehicleId, string monthYear, DateTime fromTimestamp, DateTime toTimestamp)
    {
        var selectCql = @"
        SELECT * 
        FROM Locations_By_Vehicle
        WHERE vehicle_id = ? 
        AND week_year = ? 
        AND timestamp >= ? 
        AND timestamp <= ?";
        
        var statement = _cassandraSession.Prepare(selectCql).Bind(vehicleId, monthYear, fromTimestamp, toTimestamp);
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
    
    public List<Location> TestVehicle()
    {
        var selectCql = @"
        SELECT * 
        FROM Locations_By_Vehicle 
        ";
        
        var rs = _cassandraSession.Execute(selectCql);
        var results = rs.Select(row => new Location
        {
            VehicleId = row.GetValue<Guid>("vehicle_id"),
            Timestamp = row.GetValue<DateTime>("timestamp"),
            Latitude = row.GetValue<double>("latitude"),
            Longitude = row.GetValue<double>("longitude"),
            Info = row.GetValue<string>("week_year")
            
        }).ToList();

        return results;
    }
    public List<Location> TestFleet()
    {
        var selectCql = @"
        SELECT * 
        FROM Locations_By_Fleet
        ";
        
        var rs = _cassandraSession.Execute(selectCql);
        var results = rs.Select(row => new Location
        {
            VehicleId = row.GetValue<Guid>("vehicle_id"),
            Timestamp = row.GetValue<DateTime>("timestamp"),
            Latitude = row.GetValue<double>("latitude"),
            Longitude = row.GetValue<double>("longitude"),
            Info = row.GetValue<string>("hour_date")
        }).ToList();

        return results;
    }
    
}