namespace AlarmConsumer.Models;

public class AlarmMessage
{
    public Guid VehicleId { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public DateTime Timestamp { get; set; }
}