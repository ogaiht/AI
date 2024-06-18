using Newtonsoft.Json;

namespace ChromaDb.Net
{
    public record CollectionInsert(
        [JsonProperty("ids")] IEnumerable<string> Ids,
        [JsonProperty("embeddings")] IEnumerable<float[]> Embeddings,
        [JsonProperty("metadata")] IEnumerable<IDictionary<string, string>?>? Metadata,
        [JsonProperty("documents")] IEnumerable<string>? Documents
     );
}
