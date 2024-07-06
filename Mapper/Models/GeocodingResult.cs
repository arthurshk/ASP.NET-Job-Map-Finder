using Mapper.Controllers;
using Newtonsoft.Json;

namespace Mapper.Models
{
    public class GeocodingResult
    {
        [JsonProperty("geometry")]
        public GeocodingGeometry Geometry { get; set; }
    }
}
