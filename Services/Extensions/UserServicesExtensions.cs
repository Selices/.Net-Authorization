using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.InterFace;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UserServicesExtensions
    {
        //扩展方法 注册服务
        //首先名称空间需要是扩展服务的名称空间 是静态类 静态方法  在调用时要指向(this)扩展服务
        public static void AddUserServices(this IServiceCollection services)
        {
            services.AddScoped<IUserServices,UserServices>();
        }
    }
}
