using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthStudy
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthStudy", Version = "v1" });
            });
            string sercrt = Configuration.GetSection("sercrt").Value.ToString();
            string issuer = Configuration.GetSection("issuer").Value.ToString();
            string audience = Configuration.GetSection("audience").Value.ToString();
            services.AddAuthentication("Bearer").AddJwtBearer(options =>
            {
                //指定验证类型为Bearer
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    //设置指定参数
                    //是否验证密钥
                    ValidateIssuerSigningKey = true,
                    //指定具体密钥
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(sercrt)),
                    //是否指定颁发者
                    ValidateIssuer = true,
                    //指定颁发者
                    ValidIssuer = issuer,
                    //是否指定接收者
                    ValidateAudience = true,
                    //指定接收者
                    ValidAudience = audience,
                    //验证过期
                    RequireExpirationTime = true,
                    //必须验证超时
                    ValidateLifetime = true
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthStudy v1"));
            }

            app.UseRouting();
            //开启鉴权  --判断用户是否登陆了
            app.UseAuthentication();
            //开启授权  --给予权限
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
