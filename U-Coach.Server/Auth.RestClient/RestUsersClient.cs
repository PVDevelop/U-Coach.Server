using PVDevelop.UCoach.Server.Auth.Contract;

namespace PVDevelop.UCoach.Server.Auth.RestClient
{
    public class RestUsersClient : IUsersClient
    {
        public string Create(CreateUserDto userDto)
        {
            var client = new RestSharp.RestClient("http://localhost:51669");
            var request =
                new RestSharp.RestRequest(Routes.CREATE_USER, RestSharp.Method.POST).
                AddJsonBody(userDto);

            var result = client.Execute(request);

            return
                result.StatusCode == System.Net.HttpStatusCode.OK ?
                result.Content :
                null;
        }
    }
}
