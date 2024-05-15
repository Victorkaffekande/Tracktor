using System.Text.Json;
using AlarmService.client;
using AlarmService.Schema;

namespace AlarmService.Helper;

public class GeometryHelper
{
    private readonly RedisClient _redisClient;

    public GeometryHelper(RedisClient redisClient)
    {
        _redisClient = redisClient;
    }

    private static bool IsPointInQuadrilateral(GeoFence fence,  GeoPoint p)
    {
        var a = fence.a;
        var b = fence.b;
        var c = fence.c;
        var d = fence.d;
        
        
        var b1 = CrossProduct(a, b, p) < 0.0;
        var b2 = CrossProduct(b, c, p) < 0.0;
        var b3 = CrossProduct(c, d, p) < 0.0;
        var b4 = CrossProduct(d, a, p) < 0.0;

        return ((b1 == b2) && (b2 == b3) && (b3 == b4));
    }

    private static double CrossProduct(GeoPoint a, GeoPoint b, GeoPoint p)
    {
        return (p.X - a.X) * (b.Y - a.Y) - (p.Y - a.Y) * (b.X - a.X);
    }

    public  bool IsPointValid(string id, GeoPoint point)
    {
        var fencesString = _redisClient.GetString(id);
        if (fencesString == null) return false;
        var opt = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var fences = JsonSerializer.Deserialize<List<GeoFence>>(fencesString, opt);

        if (fences == null) return true;
        foreach (var fence in fences)
        {
            var val = IsPointInQuadrilateral(fence, point);
            if (!val) return val;
        }

        return true;
    }
}