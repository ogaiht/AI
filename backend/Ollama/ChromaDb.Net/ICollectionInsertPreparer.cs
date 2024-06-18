using ChromaDb.Net.Embeddings;

namespace ChromaDb.Net
{
    public interface ICollectionInsertPreparer
    {
        /*Task<CollectionInsert> PrepareAsync(
            IEmbeddingFunction embeddingFunction, 
            bool requireEmbeddingsOrDocuments, 
            IEnumerable<string> ids, 
            IEnumerable<float[]>? embeddings = null,
            IEnumerable<IDictionary<string, string>>? metadata = null,
            IEnumerable<string>? documents = null,
            CancellationToken cancellationToken = default
        );*/

        Task<CollectionInsert> PrepareAsync(
           IEnumerable<CollectionItem> collectionItems,
           IEmbeddingFunction embeddingFunction,
           CancellationToken cancellationToken = default);
    }
}