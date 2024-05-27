namespace StreakyAPi.Model.Request
{
    public class LocationRequest
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public double Radius { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
