using System;
using System.Web.Mvc;
using MvcAuthrorization.Models;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.Contract;

namespace MvcAuthrorization.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IRestClientFactory _restClientFactory;

        public AuthenticationController(IRestClientFactory restClientFactory)
        {
            if(restClientFactory == null)
            {
                throw new ArgumentNullException(nameof(restClientFactory));
            }
            _restClientFactory = restClientFactory;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new LogonModel());
        }

        [HttpPost]
        public ActionResult LogonFacebook(LogonModel model)
        {
            var profile = new UserProfileModel()
            {
                AuthSystem = "Facebook"
            };

            var facebookAuthUrl = 
                _restClientFactory.
                CreateGet("api/facebook/authorization").
                AddParameter("redirect_uri", GetFacebookCodeRedirectUri()).
                Execute().
                CheckGetResult().
                GetJsonContent<FacebookRedirectDto>();

            return Redirect(facebookAuthUrl.Uri);
        }

        [HttpPost]
        public ActionResult LogonUCoach(LogonModel model)
        {
            var profile = new UserProfileModel()
            {
                AuthSystem = "UCoach",
                Name = "UNKNOWN"
            };

            return RedirectToProfile(profile);
        }

        [HttpGet]
        public ActionResult FacebookCode(string code)
        {
            var facebookUserProfile =
                _restClientFactory.
                CreateGet("api/facebook/user_profile").
                AddParameter("code", code).
                AddParameter("redirect_uri", GetFacebookCodeRedirectUri()).
                Execute().
                CheckGetResult().
                GetJsonContent<FacebookProfileDto>();

            var profile = new UserProfileModel()
            {
                AuthSystem = "Facebook",
                Name = facebookUserProfile.Name
            };

            return RedirectToProfile(profile);
        }

        private ActionResult RedirectToProfile(UserProfileModel model)
        {
            return RedirectToAction("Index", "UserProfile", model);
        }

        private string GetFacebookCodeRedirectUri()
        {
            return string.Format(
                "{0}://{1}/Authentication/FacebookCode",
                Request.Url.Scheme,
                Request.Url.Authority);
        }
    }
}