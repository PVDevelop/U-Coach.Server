namespace PVDevelop.UCoach.Server.RestClient
{
    public interface IRestClientFactory
    {
        IRestClient CreatePost(string resources);
    }
}
