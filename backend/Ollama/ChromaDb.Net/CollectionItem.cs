namespace ChromaDb.Net
{
    public record CollectionItem(string Id, string Document, IDictionary<string, string>? Metadata = null);
    
}
