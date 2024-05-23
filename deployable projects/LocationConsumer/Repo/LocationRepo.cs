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
        var batch = new BatchStatement();
    
        //setting up for batch write into both tables
        var insertTractorCql = "INSERT INTO Locations_By_Tractor (vehicle_id, week_year, timestamp, latitude, longitude) VALUES (?, ?, ?, ?, ?)";
        var insertCompanyCql = "INSERT INTO Locations_By_Company (fleet_id, vehicle_id, hour_date, timestamp, latitude, longitude) VALUES (?, ?, ?, ?, ?, ?)";
    
        var preparedStatementTractor = _cassandraSession.Prepare(insertTractorCql);
        var preparedStatementCompany = _cassandraSession.Prepare(insertCompanyCql);

        foreach (var location in locations)
        {
            //TODO Actually calculate the different bucket times
            var tempbucket = location.Timestamp.ToString("yyyy-MM");

            var boundStatementTractor = preparedStatementTractor.Bind(
                location.VehicleId,
                tempbucket,
                location.Timestamp,
                location.Latitude,
                location.Longitude
            );
            batch.Add(boundStatementTractor);

            var boundStatementCompany = preparedStatementCompany.Bind(
                location.FleetId,
                location.VehicleId,
                tempbucket,
                location.Timestamp,
                location.Latitude,
                location.Longitude
            );
            batch.Add(boundStatementCompany);
        }

        _cassandraSession.Execute(batch);

        return "Inserted locations successfully";
    }
}