using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcAuthrorization.Models;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.WebApi;
using System.Collections.Generic;
using System.Web;
using System.Net.Http.Headers;
using PVDevelop.UCoach.Server.HttpGateway.Contract;

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
                    BuildGetAsync(PVDevelop.UCoach.Server.HttpGateway.Contract.Routes.FACEBOOK_REDIRECT_URI)).
                    EnsureSuccessStatusCode().
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
                    BuildGetAsync(PVDevelop.UCoach.Server.HttpGateway.Contract.Routes.FACEBOOK_TOKEN)).
                    EnsureSuccessStatusCode();

                result.CopyCookies(HttpContext.Response);

                var profile = new UserProfileModel()
                {
                    AuthSystem = "Facebook"
                };

                return RedirectToProfile(profile);
            }
        }

        [HttpPost]
        public ActionResult LogonUCoach(LogonModel model)
        {
            var profile = new UserProfileModel()
            {
                AuthSystem = "UCoach"
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