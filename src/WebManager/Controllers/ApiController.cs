using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;

namespace WebManager.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ApiController : Controller
    {
        public string Username => HttpContext.User.Claims.First(m => m.Type == JwtClaimTypes.GivenName).Value;
    }
}
