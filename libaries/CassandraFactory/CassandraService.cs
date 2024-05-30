using Cassandra;

namespace CassandraFactory;

public class CassandraService
{

    private ISession _cassandraSession;
    public CassandraService(ISession cassandraSession)
    {
        _cassandraSession = cassandraSession;
    }
    
    public ISession Start()
    {
        return _cassandraSession;
    }

}