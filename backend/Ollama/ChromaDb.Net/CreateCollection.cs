using Newtonsoft.Json;

namespace ChromaDb.Net
{
    public record CreateCollection
    (
        [JsonProperty("name")] string Name,
        [JsonProperty("metadata")] IDictionary<string, string>? Metadate = null,
        [JsonProperty("get_or_create")] bool GetOrCreate = false
     );
}
