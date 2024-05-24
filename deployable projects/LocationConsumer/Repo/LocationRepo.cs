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
    
    
    public string BatchInsert(List<Location> locations)
    {
        //doesnt adjust for amount of inserts into the database.
        //TODO limit the amount of locations getting writing per batch write.
        var batch = new BatchStatement();
    
        //setting up for batch write into both tables
        var insertTractorCql = "INSERT INTO Locations_By_Tractor (vehicle_id, week_year, timestamp, latitude, longitude) VALUES (?, ?, ?, ?, ?)";
        var insertCompanyCql = "INSERT INTO Locations_By_Company (fleet_id, vehicle_id, hour_date, timestamp, latitude, longitude) VALUES (?, ?, ?, ?, ?, ?)";
    
        var preparedStatementTractor = _cassandraSession.Prepare(insertTractorCql);
        var preparedStatementCompany = _cassandraSession.Prepare(insertCompanyCql);

        foreach (var location in locations)
        {
            var weekYear = BucketWeekYear(location.Timestamp);  
            
            var boundStatementTractor = preparedStatementTractor.Bind(
                location.VehicleId,
                weekYear,
                location.Timestamp,
                location.Latitude,
                location.Longitude
            );
            batch.Add(boundStatementTractor);

            var hourDate = BucketHourDate(location.Timestamp);  

            
            var boundStatementCompany = preparedStatementCompany.Bind(
                location.FleetId,
                location.VehicleId,
                hourDate,
                location.Timestamp,
                location.Latitude,
                location.Longitude
            );
            batch.Add(boundStatementCompany);
        }

        _cassandraSession.Execute(batch);

        return "Inserted locations successfully";
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
        var hourDate = timestamp.ToString("hh-dd-mm-yyyy");
        
        return hourDate;
    }
}