using System;
using LangChain.Databases;
using LangChain.Databases.Chroma;
using LangChain.Providers;
using LangChain.Providers.Ollama;
using LangChain.DocumentLoaders;
using LangChain.Extensions;
using Document = LangChain.DocumentLoaders.Document;

namespace Tests
{
	public class OllamaTest
	{
		public static async Task Test()
		{
			OllamaProvider provider = CreateProvider();
			OllamaEmbeddingModel ollamaEmbeddingModel = new(provider, "mxbai-embed-large");
			OllamaChatModel ollamaChatModel = new(provider, "llama3");
			IVectorDatabase database = new ChromaVectorDatabase(new HttpClient(), "http://localhost:8000");
			IVectorCollection collection = await database.GetCollectionAsync("harrypotter");
			/*IVectorCollection collection = await chromaVectorDatabase.AddDocumentsFromAsync<PdfPigPdfLoader>(
				embeddingModel: ollamaEmbeddingModel,
				dimensions: 1024,
				dataSource: DataSource.FromUrl("https://canonburyprimaryschool.co.uk/wp-content/uploads/2016/01/Joanne-K.-Rowling-Harry-Potter-Book-1-Harry-Potter-and-the-Philosophers-Stone-EnglishOnlineClub.com_.pdf"),
    			collectionName: "harrypotter", // Can be omitted, use if you want to have multiple collections,				
				textSplitter: null
			);*/

			const string question = "What is Harry's address?";

			IReadOnlyCollection<Document> documents = await collection.GetSimilarDocuments(ollamaEmbeddingModel, question, 5);

			string prompt = $"""
				Use the following pieces of context to answer the question at the end.
				If the answer is not in the context then say you don't know. Use only the context to find the answer.

				{documents.AsString()}

				Question: {question}
				Helpful Answer:
			""";	


			ChatResponse answer = await ollamaChatModel.GenerateAsync(prompt);
			
			Console.WriteLine($"Answer: {answer.LastMessageContent}");

			ChatResponse chatResponse = await ollamaChatModel.GenerateAsync("What is the distance from Earth to Sagittarius A*? Answer:");
			Console.WriteLine(chatResponse.LastMessageContent);
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
	}
}

