using System;
using System.Threading.Tasks;
using API.Auth;
using EventGenerator.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Activity.Repository;
using Models.Entries.Repository;
using Models.Maraphone.Repository;
using Models.Maraphone.Task.Repository;
using Models.Tokens.Repository;
using Models.Users.Repository;
using SimpleMailSender;

namespace API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddSingleton<UserRepository>();
            services.AddSingleton<TokenRepository>();
            services.AddSingleton<MaraphoneRepository>();
            services.AddSingleton<ActivityRepository>();
            services.AddSingleton<ContentRepository>();
            services.AddSingleton<EntryRepository>();
            services.AddSingleton<ActivityFinishedRepository>();
            services.AddSingleton<RegistrationEventInfoRepository>();
            services.AddSingleton<StartSprintEventRepository>();
            services.AddSingleton<SubscribeEventInfoRepository>();
            
            services.AddSingleton<Configuration>();
            services.AddSingleton<MailSender>();
            services.AddHostedService<CronWorker>();
//            services.AddHostedService<AuthDaemon>();

            services.AddCors();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
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

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}