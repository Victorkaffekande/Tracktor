using SharedModels;

namespace LocationConsumer.Repo;

public interface ILocationRepo
{
    public Task<string> BatchInsert(List<Location> locations);

}