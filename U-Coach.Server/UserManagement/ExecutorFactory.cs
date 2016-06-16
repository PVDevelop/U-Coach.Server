using PVDevelop.UCoach.Server.AuthService;
using PVDevelop.UCoach.Server.Mongo;
using System.Linq;

namespace PVDevelop.UCoach.Server.UserManagement
{
    public static class ExecutorFactory
    {
        public static IExecutor CreateExecutor(string[] args)
        {
            if(args.Length == 0)
            {
                return new HelpExecutor();
            }
            switch (args[0])
            {
                case "c":
                    var mongoConnSettings = new MongoConnectionSettings("mongo");
                    var mongoRep = new MongoRepository<User>(mongoConnSettings);
                    var userService = new UserService(mongoRep);
                    var user = new CreateUserExecutor(userService);
                    user.Setup(args.Skip(1).ToArray());
                    return user;
                default:
                    return new HelpExecutor();
            }
        }
    }
}
