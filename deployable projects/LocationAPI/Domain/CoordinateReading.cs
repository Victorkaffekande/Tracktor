namespace LocationAPI.Domain;

public class CoordinateReading
{
    public Guid VehicleId { get; set; }
    public Coordinate Coordinate { get; set; }
}