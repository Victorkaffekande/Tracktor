﻿using Cassandra;
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

       
        // Create table if not exists Locations_By_Vehicle
        keyspaceSession.Execute(@"
    CREATE TABLE IF NOT EXISTS Locations_By_Vehicle (
        vehicle_id UUID,
        week_year  varchar,
        timestamp TIMESTAMP,
        latitude DOUBLE,
        longitude DOUBLE,
        PRIMARY KEY ((vehicle_id, week_year), timestamp)
        )");
        
        // Create table if not exists Location_By_Fleet
        keyspaceSession.Execute(@"
    CREATE TABLE IF NOT EXISTS Locations_By_Fleet (
        fleet_id UUID,
        vehicle_id UUID,
        hour_date varchar,
        timestamp TIMESTAMP,
        latitude DOUBLE,
        longitude DOUBLE,
        PRIMARY KEY ((fleet_id, hour_date), timestamp, vehicle_id)
    ) WITH default_time_to_live = 86400
    AND compaction = {
        'class' : 'TimeWindowCompactionStrategy',
        'compaction_window_unit' : 'HOURS',
        'compaction_window_size' : 24
    };");

        return new CassandraService(keyspaceSession);
    }
}