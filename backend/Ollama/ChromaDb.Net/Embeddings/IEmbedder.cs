namespace ChromaDb.Net.Embeddings
{
    public interface IEmbedder<in TInput, TOutput>
    {
        Task<IEnumerable<TOutput[]>> GenerateAsync(IEnumerable<TInput> data, CancellationToken cancellationToken = default);
    }
}
