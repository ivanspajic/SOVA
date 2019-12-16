using AutoMapper;
using Data_Layer;
using Data_Layer.Database_Context;
using Data_Layer_Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;

namespace SOVA
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
                .AddJsonFile("appsettings.dbConnectionString.json", optional: true, reloadOnChange: true)
                .Build();

            // for local database connection
            var dbConnectionString = builder.GetSection("StackOverflow:ConnectionString").Value;

            // for ruc's database connection
            //var dbConnectionString = "host=rawdata.ruc.dk;db=raw4;uid=raw4;pwd=yzOrEFi)";

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.All;
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IQuestionRepository>(provider => new QuestionRepository(new SOVAContext(dbConnectionString)));
            services.AddTransient<IAnswerRepository>(provider => new AnswerRepository(new SOVAContext(dbConnectionString)));
            services.AddTransient<IUserHistoryRepository>(provider => new UserHistoryRepository(new SOVAContext(dbConnectionString)));
            services.AddTransient<ICommentRepository>(provider => new CommentRepository(new SOVAContext(dbConnectionString)));
            services.AddTransient<IUserRepository>(provider => new UserRepository(new SOVAContext(dbConnectionString)));
            services.AddTransient<IAnnotationRepository>(provider => new AnnotationRepository(new SOVAContext(dbConnectionString)));
            services.AddTransient<IMarkingRepository>(provider => new MarkingRepository(new SOVAContext(dbConnectionString)));
            services.AddTransient<ILinkPostRepository>(provider => new LinkPostRepository(new SOVAContext(dbConnectionString)));

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
            app.UseFileServer();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
