using SharedModels;

namespace LocationConsumer.Repo;

public interface ILocationRepo
{
    public Task BulkInsert(List<Location> locations);
    public Task BulkWriteLocationsByVehicle(List<Location> locations);
    public Task BulkWriteLocationsByFleet(List<Location> locations);
}