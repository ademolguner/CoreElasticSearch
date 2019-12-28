using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyBlogProject.Business.Abstract;
using MyBlogProject.Business.Concrete.Manager;
using MyBlogProject.Business.ElasticSearchOptions;
using MyBlogProject.Business.ElasticSearchOptions.Configurations;
using MyBlogProject.DataAccess.Abstract;
using MyBlogProject.DataAccess.Concrete;
using MyBlogProject.DataAccess.Concrete.EntityFramework;

namespace MyBlogProject.MvcWebUI
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
            services.AddDbContext<AdemBlogDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:BlogDb"]));
            services.AddScoped<IElasticSearchService, ElasticSearchManager>();
            services.AddScoped<IElasticSearchConfigration, ElasticSearchConfigration>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
