﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Shared.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        //code below returns id of user from jwt token
        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;

        //code below could also be useful, but the code aboce is shorter and does the same thing
        //public string GetUserId => _httpContextAccessor.HttpContext.User.Claims.Where(x=>x.Type=="sub").FirstOrDefault().Value;
    }
}
