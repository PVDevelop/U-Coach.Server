using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcAuthrorization.Models;

namespace MvcAuthrorization.Controllers
{
    public class UserProfileController : Controller
    {
        [HttpGet]
        public ActionResult Index(UserProfileModel profileModel)
        {
            return View(profileModel);
        }
    }
}