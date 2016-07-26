using System.Web.Http;
using System.Web.Http.Dispatcher;
using PVDevelop.UCoach.Server.WebApi;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Role.Domain;
using PVDevelop.UCoach.Server.Role.Mongo;
using PVDevelop.UCoach.Server.Timing;
using StructureMap;
using PVDevelop.UCoach.Server.Role.WebApi;
using PVDevelop.UCoach.Server.RestClient;
using System;
using PVDevelop.UCoach.Server.Role.FacebookRestClient;
using PVDevelop.UCoach.Server.Role.FacebookContract;
using PVDevelop.UCoach.Server.Role.Domain.Validator;

namespace Role.IisWebApiHost
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private Container _container;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new StructureMapControllerActivator(SetupContainer()));
        }

        private Container SetupContainer()
        {
            _container = new Container(x =>
            {
                x.For<IUserService>().Use<UserService>();
                x.For<ITokenGenerator>().Use<TokenGenerator>();
                x.For<IUtcTimeProvider>().Use<UtcTimeProvider>();

                SetupMongo(x);
                SetupAuth(x);
                SetupFacebook(x);
                SetupValidation(x);
            });

            return _container;
        }

        private static void SetupMongo(ConfigurationExpression x)
        {
            x.
                For<IConnectionStringProvider>().
                Use<ConfigurationConnectionStringProvider>().
                Ctor<string>().
                Is("mongo").
                Named("mongo_settings");

            x.
                For<IMongoRepository<MongoToken>>().
                Use<MongoRepository<MongoToken>>().
                Ctor<IConnectionStringProvider>().
                IsNamedInstance("mongo_settings");

            x.
                For<IMongoRepository<MongoUser>>().
                Use<MongoRepository<MongoUser>>().
                Ctor<IConnectionStringProvider>().
                IsNamedInstance("mongo_settings");

            x.For<ITokenRepository>().Use<TokenRepository>();
            x.For<IUserRepository>().Use<UserRepository>();
        }

        private static void SetupFacebook(ConfigurationExpression x)
        {
            x.For<ISettingsProvider<IFacebookOAuthSettings>>().Use<ConfigurationSectionSettingsProvider<IFacebookOAuthSettings>>().Ctor<string>().Is("facebookSettings");
            x.For<IFacebookOAuthSettings>().Use<FacebookOAuthSettingsSection>();
            x.For<IFacebookClient>().Use<RestFacebookClient>();
        }

        private static void SetupAuth(ConfigurationExpression x)
        {
            x.
                For<IConnectionStringProvider>().
                Use<ConfigurationConnectionStringProvider>().
                Ctor<string>().
                Is("auth").
                Named("auth_settings");

            x.
                For<IRestClientFactory>().
                Use<RestClientFactory>().
                Ctor<IConnectionStringProvider>().
                IsNamedInstance("auth_settings").
                Named("auth_rest_client_factory");

            x.
                For<PVDevelop.UCoach.Server.Auth.Contract.IUsersClient>().
                    Use<PVDevelop.UCoach.Server.Auth.RestClient.RestUsersClient>().
                    Ctor<IRestClientFactory>().
                    IsNamedInstance("auth_rest_client_factory");
        }

        private static void SetupValidation(ConfigurationExpression x)
        {
            x.For<IAuthTokenValidator>().Use<FacebookValidatorAdapter>().Named("facebook_validator");
            x.For<IAuthTokenValidator>().Use<UCoachValidatorAdapter>().Named("ucoach_validator");
            x.
                For<IAuthTokenValidatorContainer>().
                Use<AuthTokenValidatorContainer>().
                Ctor<IAuthTokenValidator>("facebookValidator").
                IsNamedInstance("facebook_validator").
                Ctor<IAuthTokenValidator>("uCoachValidator").
                IsNamedInstance("ucoach_validator");

        }
    }
}
