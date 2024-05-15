namespace AlarmService.Schema;

public class FenceRequest
{
    public string TractorId { get; set; }
    public List<GeoFence> Fences { get; set; }
}