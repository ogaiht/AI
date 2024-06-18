using Newtonsoft.Json;

namespace ChromaDb.Net.Embeddings.Ollama
{
    internal record EmbeddingResult([JsonProperty("embedding")] float[] Embedding) { }
}
