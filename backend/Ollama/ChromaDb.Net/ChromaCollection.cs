using ChromaDb.Net.Embeddings;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace ChromaDb.Net
{
    public class ChromaCollection : IChromaCollection
    {
        private readonly HttpClient _httpClient;
        private readonly ChromaCollectionInfo _collectionInfo;
        private readonly IEmbeddingFunction _embeddingFunction;
        private readonly ICollectionInsertPreparer _collectionInsertPreparer;

        public ChromaCollection(
            HttpClient httpClient, 
            ChromaCollectionInfo collectionInfo, 
            IEmbeddingFunction embeddingFunction, 
            ICollectionInsertPreparer collectionItemPreparer
        )
        {
            _httpClient = httpClient;
            _collectionInfo = collectionInfo;
            _embeddingFunction = embeddingFunction;
            _collectionInsertPreparer = collectionItemPreparer;
        }

       /* public async Task AddAsync(
            IEnumerable<string>? ids,
            IEnumerable<float[]>? embeddings = null,
            IEnumerable<IDictionary<string, string>>? metadata = null,
            IEnumerable<string>? documents = null,
            CancellationToken cancellationToken = default)
        {
            CollectionInsert collectionInsert = await _collectionInsertPreparer.PrepareAsync(_embeddingFunction, true, ids, embeddings, metadata, documents, cancellationToken);            
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(ApiRoutes.GetRouteToCollection(_collectionInfo.Id), collectionInsert);
            response.EnsureSuccessStatusCode();
        }*/

        public async Task AddAsync(
            IEnumerable<CollectionItem> collectionItems,
            CancellationToken cancellationToken = default)
        {
            CollectionInsert collectionInsert = await _collectionInsertPreparer.PrepareAsync( collectionItems, _embeddingFunction, cancellationToken);
            string uri = ApiRoutes.CreateURI(ApiRoutes.CollectionsURI, _collectionInfo.Id, "add");
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, collectionInsert, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task<QueryResult> QueryAsync(string searchText, int numberOrResults = 10, MetadataFilter[]? metadataFilters = null, string[]? include = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                throw new ArgumentNullException(nameof(searchText));
            }
            IEnumerable<float[]> embedding = await _embeddingFunction.GenerateAsync(new string[] { searchText }, cancellationToken);
            if (embedding == null)
            {
                throw new InvalidOperationException();
            }
            if (include == null)
            {
                include = new string[] { "metadatas", "documents", "distances" };
            }
            QueryRequest queryRequest  = new QueryRequest(QueryEmbeddings:  embedding, NumberOfResults: numberOrResults, Include: include);
            string uri = ApiRoutes.CreateURI(ApiRoutes.CollectionsURI, _collectionInfo.Id, "query");
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, queryRequest, cancellationToken);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync(cancellationToken);
            QueryResult? queryResult = JsonConvert.DeserializeObject<QueryResult>(content);
            if (queryResult == null)
            {
                throw new InvalidOperationException();
            }
            return queryResult;
        }

        private record QueryRequest
        (
            [JsonProperty("where")]
            IDictionary<string, object>? Where = null,
            [JsonProperty("where_document")]
            IDictionary<string, object>? WhereDocument = null,
            [JsonProperty("query_embeddings")]
            IEnumerable<IEnumerable<float>>? QueryEmbeddings = null,
            [JsonProperty("n_results")]
            int NumberOfResults = 10,
            [JsonProperty("include")]
            string[]? Include = null
        );
    }

    public enum MetadataFilterCondition
    {
        Equal
    }

    public record MetadataFilter
    (
        string Key,
        MetadataFilterCondition condition,
        string value
    );

    public record QueryResult(
        [JsonProperty("ids")]
        IEnumerable<IEnumerable<string>> Ids,
        [JsonProperty("distances")]
         IEnumerable<IEnumerable<float>> Distances,
        [JsonProperty("metadatas")]
         IEnumerable<IEnumerable<IDictionary<string, string>?>>? Metadatas = null,
        [JsonProperty("embeddings")]
         IEnumerable<IEnumerable<float>>? Embeddings = null,
        [JsonProperty("documents")]
        IEnumerable<IEnumerable<string?>>? Documents = null,
        [JsonProperty("uris")]
        IEnumerable<IEnumerable<string>>? Uris = null,
        [JsonProperty("data")]
        IEnumerable<IEnumerable<string>>? Data = null
    );

    public record Query
    {
        public Query(
            [JsonProperty("where")]
            IDictionary<string, object>? Where = null,
            [JsonProperty("where_document")]
            IDictionary<string, object>? WhereDocument = null,
            [JsonProperty("query_embeddings")]
            IEnumerable<IEnumerable<float>>? QueryEmbeddings = null,
            [JsonProperty("n_results")]
            int NumberOfResults = 10,
            [JsonProperty("include")]
            params string[]? Include)
        {
            if (Include == null || Include.Length == 0)
            {
                Include = new string[] { "metadatas", "documents", "distances" };
            }
        }
    }
}
