using Newtonsoft.Json;
using System.Text;

namespace ChromaDb.Net.Embeddings.Ollama
{
    public class OllamaEmbeddingFunction : IEmbeddingFunction
    {
        private readonly HttpClient _httpClient;
        private readonly string _model;

        public OllamaEmbeddingFunction(string url, string model)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };
            _model = model;
        }

        public async Task<IEnumerable<float[]>> GenerateAsync(IEnumerable<string> data, CancellationToken cancellationToken = default)
        {
            List<float[]> embeddings = new List<float[]>();
            foreach (string item in data)
            {
                string value = JsonConvert.SerializeObject(new
                {
                    model = _model,
                    prompt = item
                });
                StringContent content = new StringContent(value, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("api/embeddings", content, cancellationToken);
                response.EnsureSuccessStatusCode();
                string stringContent = await response.Content.ReadAsStringAsync(cancellationToken);
                EmbeddingResult? embeddingResult = JsonConvert.DeserializeObject<EmbeddingResult>(stringContent);
                if (embeddingResult != null)
                {
                    embeddings.Add(embeddingResult.Embedding);
                }
            }
            return embeddings;
        }
    }
}
