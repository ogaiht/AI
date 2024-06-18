namespace ChromaDb.Net
{
    public interface IChromaCollection
    {
        /*Task AddAsync(
            IEnumerable<string>? ids,
            IEnumerable<float[]>? embeddings = null,
            IEnumerable<IDictionary<string, string>>? metadata = null,
            IEnumerable<string>? documents = null,
            CancellationToken cancellationToken = default);*/

        Task AddAsync(
            IEnumerable<CollectionItem> collectionItems,
            CancellationToken cancellationToken = default);

        Task<QueryResult> QueryAsync(string searchText, int numberOrResults = 10, MetadataFilter[]? metadataFilters = null, string[]? include = null, CancellationToken cancellationToken = default);
    }
}
