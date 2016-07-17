using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcAuthrorization.Models;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.WebApi;

namespace MvcAuthrorization.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IActionResultBuilderFactory _actionResultBuilderFactory;

        public AuthenticationController(IActionResultBuilderFactory actionResultBuilderFactory)
        {
            if(actionResultBuilderFactory == null)
            {
                throw new ArgumentNullException(nameof(actionResultBuilderFactory));
            }
            _actionResultBuilderFactory = actionResultBuilderFactory;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new LogonModel());
        }

        [HttpPost]
        public async Task<ActionResult> LogonFacebook(LogonModel model)
        {
            using (var builder = _actionResultBuilderFactory.CreateActionResultBuilder())
            {
                var result =
                    (await builder.
                    AddParameter("redirect_uri", GetFacebookCodeRedirectUri()).
                    BuildAsync("api/facebook/authorization")).
                    ToJson<FacebookRedirectDto>();

                return Redirect(result.Uri);
            }
        }

        [HttpGet]
        public async Task<ActionResult> FacebookCode(string code)
        {
            using (var builder = _actionResultBuilderFactory.CreateActionResultBuilder())
            {
                var result =
                    (await builder.
                    AddParameter("code", code).
                    AddParameter("redirect_uri", GetFacebookCodeRedirectUri()).
                    BuildAsync("api/facebook/connection")).
                    ToJson<FacebookConnectionDto>();

                var profile = new UserProfileModel()
                {
                    AuthSystem = "Facebook",
                    Name = result.Name
                };

                return RedirectToProfile(profile);
            }
            //var facebookConnection =
            //    _restClientFactory.
            //    CreateGet("api/facebook/connection").
            //    AddParameter("code", code).
            //    AddParameter("redirect_uri", GetFacebookCodeRedirectUri()).
            //    Execute().
            //    CheckGetResult().
            //    GetJsonContent<FacebookConnectionDto>();

            //var profile = new UserProfileModel()
            //{
            //    AuthSystem = "Facebook",
            //    Name = facebookConnection.Name
            //};

            //return RedirectToProfile(profile);
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