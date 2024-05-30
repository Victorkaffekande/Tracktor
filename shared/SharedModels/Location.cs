namespace SharedModels;

public class Location
{
    public Guid FleetId { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime Timestamp { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    //for devlopment purpose only
    public string? Info { get; set; }
}