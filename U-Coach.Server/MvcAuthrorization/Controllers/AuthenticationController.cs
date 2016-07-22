using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcAuthrorization.Models;
using PVDevelop.UCoach.Server.WebApi;
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
                var response =
                    await builder.
                    AddParameter("redirect_uri", GetFacebookCodeRedirectUri()).
                    BuildGetAsync(Routes.FACEBOOK_REDIRECT_URI);

                var result =
                    response.
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
                var response =
                    await builder.
                    AddParameter("code", code).
                    AddParameter("redirect_uri", GetFacebookCodeRedirectUri()).
                    BuildGetAsync(Routes.FACEBOOK_TOKEN);

                var result = 
                    response.
                    EnsureSuccessStatusCode();

                result.CopyCookies(Response);

                return RedirectToHome();
            }
        }

        [HttpPost]
        public async Task<ActionResult> LogonUCoach(LogonModel model)
        {
            using (var builder = _actionResultBuilderFactory.CreateActionResultBuilder())
            {
                var response =
                    await builder.
                    AddParameter("login", model.Login).
                    AddParameter("password", model.Password).
                    BuildGetAsync(Routes.UCOACH_TOKEN);

                var result =
                    response.
                    EnsureSuccessStatusCode();

                result.CopyCookies(Response);

                return RedirectToHome();
            }
        }

        private ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
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