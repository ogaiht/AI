namespace ChromaDb.Net
{
    internal static class ApiRoutes
    {
        internal static readonly string CollectionsURI = "api/v1/collections";

        internal static string CreateURI(params string[] paths)
        {
            return string.Join("/", paths);
        }
    }
}
