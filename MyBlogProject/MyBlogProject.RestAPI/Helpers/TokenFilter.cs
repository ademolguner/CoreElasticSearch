using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlogProject.RestAPI.Helpers
{
    public class TokenFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string Token = context.HttpContext.Request.Headers["Authorization"];
            Console.Write($"Token Değeri : {Token}");
        }
    }
}
