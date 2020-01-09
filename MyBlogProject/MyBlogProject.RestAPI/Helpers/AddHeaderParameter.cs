using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlogProject.RestAPI.Helpers
{
    public class AddHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "isMobile",
                In = "header",
                Type = "bool",
                Required = false,
            });
            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "senderName",
                In = "header",
                Type = "string",
                Required = false,
            });
        }
    }
}
