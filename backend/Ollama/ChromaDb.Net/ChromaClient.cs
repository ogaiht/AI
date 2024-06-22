using ChromaDb.Net.Embeddings;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace ChromaDb.Net
{
    public class ChromaClient : IChromaClient
    {
        private readonly HttpClient _httpClient;
        private readonly IEmbeddingFunction _embeddingFunction;
        private readonly ICollectionInsertPreparer _insertPreparer;

        public ChromaClient(string url, IEmbeddingFunction embeddingFunction, ICollectionInsertPreparer insertPreparer)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };
            _embeddingFunction = embeddingFunction;
            _insertPreparer = insertPreparer;
        }

        public async Task<IChromaCollection> CreateCollectionAsync(string name, string? tenant = null, string? database = null, IEmbeddingFunction? embeddingFunction = null, IDictionary<string, string>? metadata = null, CancellationToken cancellationToken = default)
        {            
            string uri = BuildUri(ApiRoutes.CollectionsURI, ("tenant", tenant), ("database", database));
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, new CreateCollection(name, metadata), cancellationToken: cancellationToken);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync(cancellationToken);
            ChromaCollectionInfo? collection = JsonConvert.DeserializeObject<ChromaCollectionInfo>(content);
            if (collection == null)
            {
                throw new Exception();
            }
            return new ChromaCollection(_httpClient, collection, embeddingFunction ?? _embeddingFunction, _insertPreparer);
        }

        public async Task<IChromaCollection> GetCollectionAsync(string name, string? tenant = null, string? database = null, IEmbeddingFunction? embeddingFunction = null, CancellationToken cancellationToken = default)
        {
            string uri = BuildUri(ApiRoutes.CollectionsURI, name, ("tenant", tenant), ("database", database));
            HttpResponseMessage response = await _httpClient.GetAsync(uri, cancellationToken);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync(cancellationToken);
            ChromaCollectionInfo? collection = JsonConvert.DeserializeObject<ChromaCollectionInfo>(content);
            if (collection == null)
            {
                throw new Exception();
            }
            return new ChromaCollection(_httpClient, collection, embeddingFunction ?? _embeddingFunction, _insertPreparer);
        }

        private static string BuildUri(string uri, params (string, string?)[] values)
        {
            return BuildUri(uri, null, values);
        }

        private static string BuildUri(string uri, string? id, params (string, string?)[] values)
        {
            QueryStringBuilder sb = new();
            foreach ((string key, string? value) in values)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    sb.Add(key, value);
                }
            }
            string queryString = sb.ToString();
            string finalUri = string.IsNullOrWhiteSpace(id) ? uri : $"{uri}/{id}";
            return !string.IsNullOrWhiteSpace(queryString)
                ? $"{finalUri}?{queryString}"
                : finalUri;
        }
    }
}
