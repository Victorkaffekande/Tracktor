namespace AlarmService.client;

public static class RedisClientFactory
{
    public static RedisClient CreateClient()
    {
        return new RedisClient("redis", 6379, "mypassword");
    }
}