using ChromaDb.Net.Embeddings;

namespace ChromaDb.Net
{
    public interface IChromaClient
    {
        Task<IChromaCollection> CreateCollectionAsync(string name, string? tenant = null, string? database = null, IEmbeddingFunction? embeddingFunction = null, IDictionary<string, string>? metadata = null, CancellationToken cancellationToken = default);
        Task<IChromaCollection> GetCollectionAsync(string name, string? tenant = null, string? database = null, IEmbeddingFunction? embeddingFunction = null, CancellationToken cancellationToken = default);
    }
}