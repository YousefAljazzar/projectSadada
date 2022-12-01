using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace projectSadada.Attributes
{
    public class SadadaAuthrizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
        }
    }
}
