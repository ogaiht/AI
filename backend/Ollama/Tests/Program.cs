// See https://aka.ms/new-console-template for more information
using ChromaDb.Net;
using ChromaDb.Net.Embeddings.Ollama;
using Tests;

await OllamaTest.Test();


OllamaEmbeddingFunction ollamaEmbeddingFunction = new OllamaEmbeddingFunction("http://localhost:11434", "mxbai-embed-large");

//IEnumerable<float[]> embeddings = await ollamaEmbeddingFunction.GenerateAsync("I am the king of the world!", "My name is John.");*/


ChromaClient chromaClient = new ChromaClient("http://localhost:8000", ollamaEmbeddingFunction, new CollectionInsertPreparer());

IChromaCollection chromaCollection = await chromaClient.GetCollectionAsync("persons");

await chromaCollection.AddAsync(new CollectionItem[]
{
    new CollectionItem("info1", "Hello, my name is John")
});

QueryResult queryResult = await chromaCollection.QueryAsync("John");





Console.WriteLine("Hello, World!");