using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Module.Impl;

namespace WebManager.Controllers
{
    public class QQController : Controller
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
            var username = HttpContext.User.Identity.Name;
            return _qqService.GetQQList(username);
        }

        [Authorize]
        [HttpPost]
        public string LoginClient()
        {
            var username = HttpContext.User.Identity.Name;
            return _qqService.LoginClient(username);
        }

        [Authorize]
        [HttpGet]
        public IReadOnlyList<QQNotifyEvent> GetAndClearEvents([FromQuery]string id)
        {
            var username = HttpContext.User.Identity.Name;
            return _qqService.GetAndClearEvents(username, id);
        }
    }
}
