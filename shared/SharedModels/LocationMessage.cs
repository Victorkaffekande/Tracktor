namespace SharedModels;

public class LocationMessage
{
    public Guid FleetId { get; set; }
    public Guid VehicleId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

}