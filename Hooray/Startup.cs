using Hooray.Core.AppsettingModels;
using Hooray.Core.Interfaces;
using Hooray.Core.Services;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.DBContexts;
using Hooray.Infrastructure.MySQLManager;
using Hooray.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Hooray
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
            services.AddCors();
            //services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpContextAccessor();
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });

            services.AddControllers();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddTransient<ICampaignDetailRepository, CampaignDetailRepository>();
            services.AddTransient<ICampaignActionRepository, CampaignActionRepository>();
            services.AddTransient<IMySQLManager, MySQLManager>();
            services.AddTransient<ICampaignService, CampaignService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ILineMessageRepository, LineMessageRepository>();

            services.AddTransient<IIntroSplashScreenServices, IntroSplashScreenServices>();
            services.AddTransient<IMySQLManagerRepository, MySQLManagerRepository>();
            services.AddTransient<IRegisterServices, RegisterServices>();
            services.AddTransient<ISendOTPServices, SendOTPServices>();
            services.AddTransient<IRenewOTPServices, RenewOTPServices>();
            services.AddTransient<ISendOTPServices, SendOTPServices>();
            services.AddTransient<IJoinOnFeedTabServices, JoinOnFeedTabServices>();
            services.AddTransient<IComentServices, ComentServices>();
            services.AddTransient<IPrizeResultTabServices, PrizeResultTabServices>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IHambergerMenuServices, HambergerMenuServices>();
            services.AddTransient<IChangeServices, ChangeServices>();
            services.AddTransient<IPageFeedResource, PageFeedResource>();

            services.AddTransient<ILineSendMessageService, LineSendMessageService>();
            services.AddTransient<ICheckVersionService, CheckVersionService>();
            services.AddTransient<IRedeemService, RedeemService>();
            services.AddTransient<IRedeemRepository, RedeemRepository>();

            services.Configure<LineMessages>(Configuration.GetSection("LineMessages"));
            services.Configure<Resource>(Configuration.GetSection("Resource"));
            //services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            services.Configure<LineMessages>(Configuration.GetSection("LineMessages")); // Get value appsetting.json
            services.Configure<UrlDownloadApp>(Configuration.GetSection("UrlDownloadApp"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hooray", Version = "v1" });

                // Customize
                // Set the comments path for the Swagger JSON and UI.
                var xmlfile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlpath = Path.Combine(AppContext.BaseDirectory, xmlfile);
                c.IncludeXmlComments(xmlpath);
                c.CustomSchemaIds(x => x.FullName);
            });

            services.AddApiVersioning();

            services.AddDbContext<devhoorayContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("DefaultConnection"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hooray v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseCors(builder =>
            {
                builder
                //.WithOrigins("https://hooray.dev.fysvc.com",
                //             "https://hooray.uat.fysvc.com")
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    OnPrepareResponse = ctx => {
            //        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
            //        ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers",
            //          "Origin, X-Requested-With, Content-Type, Accept");
            //    },
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
