namespace PVDevelop.UCoach.Server.RestClient
{
    public interface IRestResponse
    {
        HttpStatusCode Status { get; }
        string Content { get; }
    }
}
