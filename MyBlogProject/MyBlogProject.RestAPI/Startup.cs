using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyBlogProject.Business.Abstract;
using Swashbuckle.AspNetCore.Swagger; 
using MyBlogProject.Business.Concrete.Manager;
using MyBlogProject.Business.ElasticSearchOptions;
using MyBlogProject.Business.ElasticSearchOptions.Abstract;
using MyBlogProject.Business.ElasticSearchOptions.Conrete;
using MyBlogProject.DataAccess.Abstract;
using MyBlogProject.DataAccess.Concrete;
using MyBlogProject.DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.IO;
using MyBlogProject.RestAPI.Helpers;
using AutoMapper;
using MyBlogProject.Business.Mappings;

namespace MyBlogProject.RestAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPostService, PostManager>();
            services.AddScoped<IPostDal, PostDal>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICategoryDal, CategoryDal>();
            services.AddScoped<ITagService, TagManager>();
            services.AddScoped<ITagDal, TagDal>();
            services.AddScoped<ICommentService, CommentManager>();
            services.AddScoped<ICommentDal, CommentDal>();
            services.AddScoped<IPostTagService, PostTagManager>();
            services.AddScoped<IPostTagDal, PostTagDal>();
            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IUserDal, UserDal>();

            services.AddDbContext<AdemBlogDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:BlogDb"]));
            services.AddScoped<IElasticSearchService, ElasticSearchManager>();
            services.AddScoped<IElasticSearchConfigration, ElasticSearchConfigration>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Configuration.GetSection("Swagger:SwaggerName").Value, new Info
                {
                    Title = Configuration.GetSection("Swagger:SwaggerDoc:Title").Value,
                    Version = Configuration.GetSection("Swagger:SwaggerDoc:Title").Value,
                    Description = Configuration.GetSection("Swagger:SwaggerDoc:Description").Value,
                    Contact = new Contact()
                    {
                        Name = Configuration.GetSection("Swagger:SwaggerDoc:Contact:Name").Value,
                        Url =  Configuration.GetSection("Swagger:SwaggerDoc:Contact:Url").Value,
                        Email = Configuration.GetSection("Swagger:SwaggerDoc:Contact:Url").Value
                    },
                    TermsOfService = "http://swagger.io/terms/"
                });
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey",
                });
                c.OperationFilter<AddHeaderParameter>();
            });
            services.AddScoped<TokenFilter>();
            //automapper config  start
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CustomMappingProfile());
            });
            //IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mappingConfig.CreateMapper());
            ////automapper config  end
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                //TODO: Either use the SwaggerGen generated Swagger contract (generated from C# classes)
                //c.SwaggerEndpoint(
                //    String.Format(Configuration.GetSection("Swagger:UseSwaggerUI:SwaggerEndpoint").Value, Configuration.GetSection("Swagger:SwaggerName").Value),
                //    Configuration.GetSection("Swagger:UseSwaggerUI:Name").Value);
                //c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint(Configuration.GetSection("Swagger:UseSwaggerUI:SwaggerEndpoint").Value, "Swagger Test .Net Core");

                //TODO: Or alternatively use the original Swagger contract that's included in the static files
                // c.SwaggerEndpoint("/swagger-original.json", "Swagger Petstore Original");
            });

          
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
