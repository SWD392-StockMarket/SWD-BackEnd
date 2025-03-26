using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.Data.DTOs.Authentication;
using SWD.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Service.Interface
{
    public interface IAuthService
    {
         Task<string> GenerateToken(User user);
        IActionResult GoogleLogin();
        Task<IActionResult> GoogleResponse(HttpContext httpContext);
    }
}
