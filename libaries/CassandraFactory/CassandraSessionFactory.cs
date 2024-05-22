using Cassandra;
using CassandraFactory;


public class CassandraSessionFactory
{

    public static CassandraService CreateCassandraService()
    {
        var cluster = Cluster.Builder()
            .AddContactPoints(new[] { "cassandra-1", "cassandra-2", "cassandra-3" })
            .Build();

        // Create keyspace if not exists
        using (var session = cluster.Connect())
        {
            var keyspaceCreationCql = $"CREATE KEYSPACE IF NOT EXISTS gps " +
                                      "WITH replication = {'class': 'NetworkTopologyStrategy', 'replication_factor': 3}";
            session.Execute(keyspaceCreationCql);
        }

        var keyspaceSession = cluster.Connect("gps");

       
        // Create table if not exists Locations_By_Tractor
        keyspaceSession.Execute(@"
    CREATE TABLE IF NOT EXISTS Locations_By_Tractor (
        tractor_id UUID,
        month_year  varchar,
        timestamp TIMESTAMP,
        latitude DOUBLE,
        longitude DOUBLE,
        PRIMARY KEY ((tractor_id, month_year), timestamp)
        )");
        
        // Create table if not exists Location_By_Company
        keyspaceSession.Execute(@"
    CREATE TABLE IF NOT EXISTS Locations_By_Company (
        company_id UUID,
        tractor_id UUID,
        month_year  varchar,
        timestamp TIMESTAMP,
        latitude DOUBLE,
        longitude DOUBLE,
        PRIMARY KEY ((company_id, month_year), timestamp, tractor_id)
        )");

        return new CassandraService(keyspaceSession);
    }
}