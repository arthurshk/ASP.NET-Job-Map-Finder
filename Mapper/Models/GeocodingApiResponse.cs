using Mapper.Controllers;
using Newtonsoft.Json;

namespace Mapper.Models
{
    public class GeocodingApiResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("results")]
        public List<GeocodingResult> Results { get; set; }
    }
}
