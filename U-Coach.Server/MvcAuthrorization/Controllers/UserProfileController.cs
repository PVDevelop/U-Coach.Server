using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcAuthrorization.Models;
using PVDevelop.UCoach.Server.Role.Contract;
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
        public async Task<ActionResult> Index()
        {
            UserInfoDto userInfoDto;
            using (var builder = _actionResultBuilderFactory.CreateActionResultBuilder())
            {
                userInfoDto = (await 
                    builder.
                    AddCookies(Request.ToCookieCollection()).
                    BuildGetAsync(PVDevelop.UCoach.Server.HttpGateway.Contract.Routes.USER_INFO)).
                    EnsureSuccessStatusCode().
                    ToJson<UserInfoDto>();
            }

            var profileModel = new UserProfileModel()
            {
                Id = userInfoDto.Id
            };

            return View(profileModel);
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            using (var builder = _actionResultBuilderFactory.CreateActionResultBuilder())
            {
                var result = (await 
                    builder.
                    AddCookies(Request.ToCookieCollection()).
                    BuildPostAsync(PVDevelop.UCoach.Server.HttpGateway.Contract.Routes.LOGOUT, new ByteArrayContent(new byte[0]))).
                    EnsureSuccessStatusCode();

                result.CopyCookies(Response);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}