namespace SharedModels;

public class Location
{
    public Guid CompanyId { get; set; }
    public Guid TractorId { get; set; }
    public DateTime Timestamp { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}