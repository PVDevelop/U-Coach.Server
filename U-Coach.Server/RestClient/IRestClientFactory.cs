namespace PVDevelop.UCoach.Server.RestClient
{
    public interface IRestClientFactory
    {
        IRestClient CreateGet(string resource, params string[] segments);

        IRestClient CreatePost(string resources, params string[] segments);

        IRestClient CreatePut(string resource, params string[] segments);
    }
}
