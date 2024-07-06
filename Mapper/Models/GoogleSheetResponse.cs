using Newtonsoft.Json;

namespace Mapper.Models
{
    public class GoogleSheetResponse
    {
        [JsonProperty("values")]
        public List<List<object>> Values { get; set; }
    }
}
