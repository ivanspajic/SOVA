using System;
using System.IO;
using System.Text;
using _2._Data_Layer_Abstractions;
using _3._Data_Layer;
using _3._Data_Layer.Database_Context;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace _1._SOVA
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.dbConnectionString.json", optional: false, reloadOnChange: true)
                .Build();

            var dbConnectionString = builder.GetSection("StackOverflow:ConnectionString").Value;
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IQuestionRepository>(provider => new QuestionRepository(new SOVAContext(dbConnectionString)));
            services.AddTransient<IAnswerRepository>(provider => new AnswerRepository(new SOVAContext(dbConnectionString)));
            services.AddTransient<IUserHistoryRepository>(provider => new UserHistoryRepository(new SOVAContext(dbConnectionString)));
            services.AddTransient<ICommentRepository>(provider => new CommentRepository(new SOVAContext(dbConnectionString)));
            services.AddTransient<IUserRepository>(provider => new UserRepository(new SOVAContext(dbConnectionString)));
            services.AddTransient<IAnnotationRepository>(provider => new AnnotationRepository(new SOVAContext(dbConnectionString)));
            services.AddTransient<IMarkingRepository>(provider => new MarkingRepository(new SOVAContext(dbConnectionString)));

            var key = Encoding.UTF8.GetBytes(builder.GetSection("Auth:Key").Value);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("<h1 style='font-family:Helvetica;'> <img src='https://ruc.dk/sites/default/files/2017-05/ruc_logo_download_en.png' width=500px><br><br><div style='text-align: center;'><marquee>SOVA Webservice by raw4.</marquee></div><br><br><iframe style='display: block; margin:auto' width='560' height='315' src='https://www.youtube.com/embed/cvChjHcABPA' frameborder='0' allow='accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowfullscreen></iframe></h1>");
                });
                endpoints.MapControllers();
            });
        }
    }
}
