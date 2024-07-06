using Newtonsoft.Json;

namespace Mapper.Models
{
    public class GeocodingGeometry
    {
        [JsonProperty("location")]
        public Location Location { get; set; }
    }
}
