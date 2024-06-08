using System.Globalization;
using Cassandra;
using SharedModels;

namespace LocationConsumer.Repo;

public class LocationRepo : ILocationRepo
{

    private readonly ISession _cassandraSession;
    
    public LocationRepo(ISession cassandraSession)
    {
        _cassandraSession = cassandraSession;
    }

    public async Task BulkInsert(List<Location> locations)
    {
        var insertVehicleTask = BulkWriteLocationsByVehicle(locations);  
        var insertFleetTask = BulkWriteLocationsByFleet(locations);  
  
        await Task.WhenAll(insertVehicleTask, insertFleetTask);  
    }

    public async Task BulkWriteLocationsByVehicle(List<Location> locations)
    {
        var insertVehicleCql = "INSERT INTO Locations_By_Vehicle (vehicle_id, week_year, timestamp, latitude, longitude) VALUES (?, ?, ?, ?, ?)";
        var preparedStatementVehicle = _cassandraSession.Prepare(insertVehicleCql);
        
        var tasks = new List<Task>();
        
        foreach (var location in locations)
        {
            var weekYear = BucketWeekYear(location.Timestamp);  
            var boundStatementVehicle = preparedStatementVehicle.Bind(
                location.VehicleId,
                weekYear,
                location.Timestamp,
                location.Latitude,
                location.Longitude
            );
            tasks.Add(_cassandraSession.ExecuteAsync(boundStatementVehicle));
        }
        await Task.WhenAll(tasks);
    }
    
    public async Task BulkWriteLocationsByFleet(List<Location> locations)
    {
        var insertFleetCql =
            "INSERT INTO Locations_By_Fleet (fleet_id, vehicle_id, hour_date, timestamp, latitude, longitude) VALUES (?, ?, ?, ?, ?, ?)";
        var preparedStatementFleet = _cassandraSession.Prepare(insertFleetCql);        
        var tasks = new List<Task>();
        
        foreach (var location in locations)
        {
            var hourDate = BucketHourDate(location.Timestamp);  
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
    }

    //follows the rules of ISO 8601 
    public string BucketWeekYear(DateTime timestamp)
    {
        var week = ISOWeek.GetWeekOfYear(timestamp);
        
        var year = timestamp.ToString("yyyy");
        
        //examples 1-2024 or 12-2025
        var result = week + "-" + year;

        return result;
    }
    
    public string BucketHourDate(DateTime timestamp)
    {
        //examples 11-30-00-2024 ie "hh-dd-mm-yyyy"
        //CHANGING the capital in formating for HH to hh would make it 12formating instead of 24
        var hourDate = timestamp.ToString("HH-dd-MM-yyyy");
        
        return hourDate;
    }
}