using ChromaDb.Net.Embeddings;

namespace ChromaDb.Net
{
    public class CollectionInsertPreparer : ICollectionInsertPreparer
    {

        public async Task<CollectionInsert> PrepareAsync(
            IEnumerable<CollectionItem> collectionItems,
            IEmbeddingFunction embeddingFunction,
            CancellationToken cancellationToken = default)
        {
            if (embeddingFunction == null)
            {
                throw new ArgumentNullException(nameof(embeddingFunction));
            }
            string[] documents = collectionItems
                .Select(i => i.Document)
                .ToArray();
            if (documents.Any(i => string.IsNullOrEmpty(i)))
            {
                throw new InvalidOperationException("Documents cannot be null.");
            }
            IEnumerable<float[]>  embeddings = await embeddingFunction.GenerateAsync(documents, cancellationToken);
            if (IsNullOrEmpty(embeddings))
            {
                throw new InvalidOperationException("Embeddings cannot be null.");
            }
            string[] ids = collectionItems
                .Select(i => i.Id)
                .ToArray();
            string[] duplicatedKeys = ids
                .GroupBy(id => id)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToArray();
            if (duplicatedKeys.Length > 0)
            {
                throw new InvalidOperationException($"Duplicated ids found: {string.Join(", ", duplicatedKeys)}");
            }
            IDictionary<string, string>?[] metadata = collectionItems
                .Select(i => i.Metadata)
                .ToArray();
            return new CollectionInsert(ids, embeddings, metadata, documents);
        }

       /* public async Task<CollectionInsert> PrepareAsync(
            IEmbeddingFunction embeddingFunction,
            bool requireEmbeddingsOrDocuments,
            IEnumerable<string> ids,
            IEnumerable<float[]>? embeddings = null,
            IEnumerable<IDictionary<string, string>>? metadata = null,
            IEnumerable<string>? documents = null,
            CancellationToken cancellationToken = default)
        {
            if (requireEmbeddingsOrDocuments && IsNullOrEmpty(embeddings) && IsNullOrEmpty(documents))
            {
                throw new InvalidOperationException("Both embeddings and documents cannot be null.");
            }
            if (IsNullOrEmpty(embeddings) && !IsNullOrEmpty(documents))
            {
                if (embeddingFunction != null)
                {
                    embeddings = await embeddingFunction.GenerateAsync(documents, cancellationToken);
                }
                else
                {
                    throw new InvalidOperationException("An Embedding Function is required to create embeddings.");
                }
            }
            if (IsNullOrEmpty(embeddings))
            {
                throw new InvalidOperationException("Embeddings cannot be null.");
            }
            int idsLength = ids.Count();
            if (!IsDataValid(idsLength, embeddings) || !IsDataValid(idsLength, metadata) || !IsDataValid(idsLength, documents))
            {
                throw new InvalidOperationException("When provided, Ids, embeddings, metadata, and documents must all be the same length.");
            }
            string[] duplicatedKeys = ids
                .GroupBy(id => id)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToArray();
            if (duplicatedKeys.Length > 0)
            {
                throw new InvalidOperationException($"Duplicated ids found: {string.Join(", ", duplicatedKeys)}");
            }
            return new CollectionInsert(ids, embeddings, metadata, documents);
        }*/

        private static bool IsNullOrEmpty<T>(IEnumerable<T>? enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        private static bool IsDataValid<T>(int expectedLength, IEnumerable<T>? data)
        {
            return data != null && data.Count() == expectedLength;
        }
    }
}
