namespace PVDevelop.UCoach.Server.RestClient
{
    public interface IRestClient
    {
        IRestClient AddBody(object body);
        IRestResponse Execute();
    }
}
