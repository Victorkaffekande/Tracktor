namespace LocationAPI.Domain;

public class CoordinateMessage
{
    public Guid VehicleId { get; set; }
    public Coordinate Coordinate { get; set; }
    public DateTime Timestamp { get; set; }
}