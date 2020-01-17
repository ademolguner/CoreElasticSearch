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
                Name = "Skip",
                In = "header",
                Type = "int",
                Required = false,
                Default=0
            });
            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "ItemCount",
                In = "header",
                Type = "int",
                Required = false,
                Default=100
            });
        }
    }
}
