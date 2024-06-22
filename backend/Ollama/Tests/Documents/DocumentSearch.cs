using LangChain.Databases;
using LangChain.Providers;
using LangChain.Extensions;
using LCDocument = LangChain.DocumentLoaders.Document;

namespace Tests.Documents;

public class DocumentSearch : IDocumentSearch
{
    private readonly IVectorCollection _vectorCollection;
    private readonly IEmbeddingModel _embeddingModel;

    public DocumentSearch(IVectorCollection vectorCollection, IEmbeddingModel embeddingModel)
    {
        _vectorCollection = vectorCollection;
        _embeddingModel = embeddingModel;
    }

    public async Task<IEnumerable<Document>> FindRelevantDocumentsAsync(string query, int limit = 10, CancellationToken cancellationToken = default)
    {
        List<Document> resultDocuments = [];
        IReadOnlyCollection<LCDocument> documents = await _vectorCollection.GetSimilarDocuments(_embeddingModel, query, limit, cancellationToken: cancellationToken);
        foreach (LCDocument document in documents)
        {
            resultDocuments.Add(new Document(document.PageContent));
        }
        return [.. resultDocuments];
    }
}
