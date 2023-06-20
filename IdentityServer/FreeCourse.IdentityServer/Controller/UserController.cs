using FreeCourse.IdentityServer.DTOs;
using FreeCourse.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace FreeCourse.IdentityServer.Controller
{
    //tag below means that JWT token must include "IdentityServer.Api" in its scope credentials
    //needs to be defined in startup class like "services.AddLocalApiAuthentication();" in ConfigureServices method 
    [Authorize(LocalApi.PolicyName)]

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupDTO signupDTO)
        {
            var appUser = new ApplicationUser
            {
                UserName=signupDTO.Username,
                Email=signupDTO.Email,
                City=signupDTO.City,
            };
            var result = await _userManager.CreateAsync(appUser, signupDTO.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok("User created succesfully");
        }
    }
}
