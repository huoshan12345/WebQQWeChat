using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.QQModels;
using Application.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Module.Impl;

namespace WebManager.Controllers
{
    public class QQController : ApiController
    {
        private readonly IQQService _qqService;

        public QQController(IQQService qqService)
        {
            _qqService = qqService;
        }

        [HttpGet]
        public int Test()
        {
            return 99;
        }

        [Authorize]
        [HttpGet]
        public IReadOnlyList<QQClientModel> GetQQList()
        {
            var username = HttpContext.User.Claims.First(m => m.Type == JwtClaimTypes.GivenName).Value;
            return _qqService.GetQQList(username);
        }

        [Authorize]
        [HttpPost]
        public string LoginClient()
        {
            var username = HttpContext.User.Claims.First(m => m.Type == JwtClaimTypes.GivenName).Value;
            return _qqService.LoginClient(username);
        }

        [Authorize]
        [HttpGet]
        public IReadOnlyList<QQNotifyEvent> GetAndClearEvents([FromQuery]string id)
        {
            var username = HttpContext.User.Claims.First(m => m.Type == JwtClaimTypes.GivenName).Value;
            return _qqService.GetAndClearEvents(username, id);
        }
    }
}
