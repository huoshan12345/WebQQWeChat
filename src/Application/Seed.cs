using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class Seed
    {
        public static async Task AddData(IServiceProvider provider)
        {
            var userManager = provider.GetRequiredService<UserManager<AppUser>>();
            await userManager.CreateAsync(new AppUser()
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
            }, "Admin@12345");
        }
    }
}
