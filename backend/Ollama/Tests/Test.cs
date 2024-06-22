using LangChain.Databases;
using LangChain.Providers;
using LangChain.Providers.Ollama;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.Connectors.Memory.Chroma;
using Microsoft.SemanticKernel.Connectors.Memory.Chroma.Http.ApiSchema;

namespace Tests;

public class Test
{

    public static async void DoTest()
    {
        ServiceCollection serviceDescriptors = new ServiceCollection();
        
        LangChain.Databases.Chroma.ChromaVectorDatabase db = new LangChain.Databases.Chroma.ChromaVectorDatabase(new HttpClient(), "http://localhost:8000");
        IVectorCollection vectorCollection = await db.GetCollectionAsync("person");
        ChromaClient chroma = new ChromaClient("http://localhost:8000");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        ChromaCollectionModel chromaCollectionModel = await chroma.GetCollectionAsync("person");


#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }

}
/*
private static void ConfigProvider(IServiceCollection serviceCollection)

    private static IEmbeddingModel CreateEmbeddingModel()
    {
        OllamaEmbeddingModel ollamaEmbeddingModel = new(provider, "mxbai-embed-large");
    }

    private static OllamaProvider CreateProvider()
    {
        OllamaProvider ollamaProvider = new(options: new OllamaOptions()
        {
            Stop = ["\n"],
            Temperature = 0.0f
        });
        return ollamaProvider;
    }	 
}*/
