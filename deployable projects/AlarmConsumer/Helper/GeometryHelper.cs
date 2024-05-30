using System.Text.Json;
using AlarmConsumer.client;
using AlarmService.Schema;
using SharedModels;

namespace AlarmService.Helper;

public class GeometryHelper
{
    private static bool IsPointInQuadrilateral(GeoFence fence,  Coordinate p)
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

    private static double CrossProduct(Coordinate a, Coordinate b, Coordinate p)
    {
        return (p.Longitude - a.Longitude) * (b.Latitude - a.Latitude) - (p.Latitude - a.Latitude) * (b.Longitude - a.Longitude);
    }

    public static bool IsPointValid(List<GeoFence> fences, Coordinate point)
    {
        foreach (var fence in fences)
        {
            var val = IsPointInQuadrilateral(fence, point);
            if (!val) return val;
        }
        return true;
    }
}