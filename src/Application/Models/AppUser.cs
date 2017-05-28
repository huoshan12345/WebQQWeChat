using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Application.Models
{
    public class AppUser : IdentityUser
    {
        public bool IsAdmin { get; set; }
    }
}
