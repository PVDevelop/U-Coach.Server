using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcAuthrorization.Models;
using PVDevelop.UCoach.Server.WebApi;

namespace MvcAuthrorization.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IActionResultBuilderFactory _actionResultBuilderFactory;

        public UserProfileController(IActionResultBuilderFactory actionResultBuilderFactory)
        {
            if (actionResultBuilderFactory == null)
            {
                throw new ArgumentNullException(nameof(actionResultBuilderFactory));
            }
            _actionResultBuilderFactory = actionResultBuilderFactory;
        }

        [HttpGet]
        public ActionResult Index(UserProfileModel profileModel)
        {
            return View(profileModel);
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            using (var builder = _actionResultBuilderFactory.CreateActionResultBuilder())
            {
                (await builder.
                    BuildPostAsync(PVDevelop.UCoach.Server.HttpGateway.Contract.Routes.LOGOUT, new ByteArrayContent(new byte[0]))).
                    EnsureSuccessStatusCode();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}