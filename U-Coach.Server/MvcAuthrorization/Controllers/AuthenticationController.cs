﻿using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcAuthrorization.Models;
using PVDevelop.UCoach.Server.WebApi;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.HttpGateway.Contract;

namespace MvcAuthrorization.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IActionResultBuilderFactory _actionResultBuilderFactory;

        public AuthenticationController(IActionResultBuilderFactory actionResultBuilderFactory)
        {
            if (actionResultBuilderFactory == null)
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
                    BuildGetAsync(PVDevelop.UCoach.Server.HttpGateway.Contract.Routes.FACEBOOK_REDIRECT_URI);

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
                var logonDto = new FacebookLogonDto()
                {
                    Code = code,
                    RedirectUri = GetFacebookCodeRedirectUri()
                };

                var response =
                    await builder.
                    BuildPutAsync(PVDevelop.UCoach.Server.HttpGateway.Contract.Routes.FACEBOOK_LOGON, logonDto);

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
                var logonDto = new UCoachLogonDto()
                {
                    Login = model.Login,
                    Password = model.Password
                };

                var response =
                    await builder.
                    BuildPutAsync(PVDevelop.UCoach.Server.HttpGateway.Contract.Routes.UCOACH_LOGON, logonDto);

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