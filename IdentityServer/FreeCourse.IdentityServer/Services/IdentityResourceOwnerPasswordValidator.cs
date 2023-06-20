using FreeCourse.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //after request, api understands that the user pass and email is correct
            //so it should give us a token
            var existUser = await _userManager.FindByEmailAsync(context.UserName);
            if(existUser == null) 
            {
                var errors = new Dictionary<string, object>
                {
                    { "errors", new List<string> { "email or password is wrong" } }
                };
                context.Result.CustomResponse=errors;
                return;
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(existUser, context.Password);
            if (passwordCheck == false)
            {
                var errors = new Dictionary<string, object>
                {
                    { "errors", new List<string> { "email or password is wrong" } }
                };
                context.Result.CustomResponse = errors;
                return;
            }


            context.Result = new GrantValidationResult(existUser.Id.ToString(), 
                OidcConstants.AuthenticationMethods.Password);
        }
    }
}
