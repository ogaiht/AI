namespace Tests.Documents;

public interface IDocumentSearch
{
    Task<IEnumerable<Document>> FindRelevantDocumentsAsync(string query, int limit = 10, CancellationToken cancellationToken = default);
}
