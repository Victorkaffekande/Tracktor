using System.Globalization;
using Cassandra;
using LocationConsumer.Helper;
using SharedModels;

namespace LocationConsumer.Repo;

public class LocationRepo : ILocationRepo
{

    private readonly ISession _cassandraSession;
    
    public LocationRepo(ISession cassandraSession)
    {
        _cassandraSession = cassandraSession;
    }
    
    
    public async Task<string> BatchInsert(List<Location> locations)
    {
        //doesnt adjust for amount of inserts into the database.
        //TODO limit the amount of locations getting writing per batch write.
    
        //setting up for batch write into both tables
        var insertVehicleCql = "INSERT INTO Locations_By_Vehicle (vehicle_id, week_year, timestamp, latitude, longitude) VALUES (?, ?, ?, ?, ?)";
        var insertFleetCql = "INSERT INTO Locations_By_Fleet (fleet_id, vehicle_id, hour_date, timestamp, latitude, longitude) VALUES (?, ?, ?, ?, ?, ?)";
    
        var preparedStatementVehicle = _cassandraSession.Prepare(insertVehicleCql);
        var preparedStatementFleet = _cassandraSession.Prepare(insertFleetCql);

        var tasks = new List<Task>();

        
        foreach (var location in locations)
        {
            var weekYear = BucketCalculation.BucketWeekYear(location.Timestamp);  
            
            var boundStatementVehicle = preparedStatementVehicle.Bind(
                location.VehicleId,
                weekYear,
                location.Timestamp,
                location.Latitude,
                location.Longitude
            );
            tasks.Add(_cassandraSession.ExecuteAsync(boundStatementVehicle));

            var hourDate = BucketCalculation.BucketHourDate(location.Timestamp);  

            
            var boundStatementFleet = preparedStatementFleet.Bind(
                location.FleetId,
                location.VehicleId,
                hourDate,
                location.Timestamp,
                location.Latitude,
                location.Longitude
            );
            tasks.Add(_cassandraSession.ExecuteAsync(boundStatementFleet));
        }
        
        await Task.WhenAll(tasks);
        
        return "Inserted locations successfully";
    }
}