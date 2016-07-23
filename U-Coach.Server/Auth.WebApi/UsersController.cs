﻿using System;
using System.Net;
using System.Web.Http;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Auth.Service;

namespace PVDevelop.UCoach.Server.Auth.WebApi
{
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            if(userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            _userService = userService;
        }

#warning в Post запросе передается body в виде JSON - объекта, поэтому здесь надо сделать класс Dtoб в котором будут login, password
        [HttpPost]
        [Route(Routes.CREATE_USER)]
        public IHttpActionResult CreateUser([FromUri]string login, [FromBody] string passwor)
        {
            var result = _userService.Create(login, passwor);
            return Content(HttpStatusCode.Created, result);
        }

#warning здесь password передается в body, поэтому его надо в отдельный Dto
        [HttpPut]
        [Route(Routes.LOGON_USER)]
        public IHttpActionResult LogonUser([FromUri] string login, [FromBody] string password)
        {
            var result = _userService.Logon(login, password);
            return Ok(result);
        }

#warning здесь token передается в body, поэтому его надо в отдельный Dto
        [HttpPut]
        [Route(Routes.VALIDATE_USER_TOKEN)]
        public IHttpActionResult ValidateToken([FromBody] string token)
        {
            _userService.ValidateToken(token);
            return Ok();
        }
    }
}
