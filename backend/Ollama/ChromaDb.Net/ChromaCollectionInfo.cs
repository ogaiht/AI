using Newtonsoft.Json;

namespace ChromaDb.Net
{
    public record ChromaCollectionInfo
    (
        [JsonProperty("name")] string Name,
        [JsonProperty("id")] string Id,
        [JsonProperty("metadata")] IDictionary<string, string> Metadata,
        [JsonProperty("tenant")] string Tenant,
        [JsonProperty("database")] string Database
    );
}
