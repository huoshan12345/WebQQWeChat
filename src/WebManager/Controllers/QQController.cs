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
            return _qqService.GetQQList(Username);
        }

        [Authorize]
        [HttpPost]
        public string LoginClient()
        {
            return _qqService.LoginClient(Username);
        }

        [Authorize]
        [HttpGet]
        public IReadOnlyList<QQNotifyEvent> Poll([FromQuery]string qqId)
        {
            return _qqService.Poll(Username, qqId);
        }

        [Authorize]
        [HttpPost]
        public Task<DataResult> SendMsg(string qqId, MessageModel message)
        {
            return _qqService.SendMsg(Username, qqId, message);
        }
    }
}
