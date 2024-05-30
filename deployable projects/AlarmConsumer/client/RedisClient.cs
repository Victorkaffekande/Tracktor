using StackExchange.Redis;

namespace AlarmConsumer.client;

public class RedisClient
{
    private readonly string _hostName;
    private readonly int _port;
    private readonly string _password;

    private ConnectionMultiplexer _redis;
    
    public RedisClient(string hostName, int port, string password)
    {
        _hostName = hostName;
        _port = port;
        _password = password;
    }

    public void Connect()
    {
        var op = ConfigurationOptions.Parse(_hostName);
        op.Password = _password;
        
        var connectionString = $"{_hostName},password={_password}";
        _redis = ConnectionMultiplexer.Connect(op);
        
    }

    private IDatabase GetDatabase()
    {
        return _redis.GetDatabase();
    }

    public void StoreString(string key, string value)
    {
        var db = GetDatabase();
        
        db.StringSet(key, value);
    }

    public string? GetString(string key)
    {
        var db = GetDatabase();
        return db.StringGet(key);
    }

    public void RemoveString(string key)
    {
        var db = GetDatabase();
        db.KeyDelete(key);
    }
    
}