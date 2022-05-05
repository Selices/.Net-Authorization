using AuthStudy.Model;
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
            //��������
            TokenModel tokenModel = Configuration.Get<TokenModel>();
            //��չ����
            services.AddUserServices();
            services.AddAuthentication("Bearer").AddJwtBearer(options =>
            {
                //ָ����֤����ΪBearer
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    //����ָ������
                    //�Ƿ���֤��Կ
                    ValidateIssuerSigningKey = true,
                    //ָ��������Կ
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenModel.sercrt)),
                    //�Ƿ�ָ���䷢��
                    ValidateIssuer = true,
                    //ָ���䷢��
                    ValidIssuer = tokenModel.issuer,
                    //�Ƿ�ָ��������
                    ValidateAudience = true,
                    //ָ��������
                    ValidAudience = tokenModel.audience,
                    //��֤����
                    RequireExpirationTime = true,
                    //������֤��ʱ
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
            //������Ȩ  --�ж��û��Ƿ��½��
            app.UseAuthentication();
            //������Ȩ  --����Ȩ��
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
