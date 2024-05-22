using SharedModels;

namespace LocationConsumer.Repo;

public interface ILocationRepo
{
    public string BatchInsert(List<Location> locations);

}