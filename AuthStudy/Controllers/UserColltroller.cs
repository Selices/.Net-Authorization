using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthStudy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserColltroller: ControllerBase
    {
        private IConfiguration _configuration;
        public UserColltroller(IConfiguration configuration) {
            _configuration = configuration;
        }
        [HttpPost("Login")]
        public string Login(string UserName,string PassWord) {
            if (UserName != "666" || PassWord !="123456")
            {
                return "ERROR";
            }
            //生成Token
            return GetToken("123", UserName);
        }
        private string GetToken(string UserId, string UserName) {
            //设置密钥
            string sercrt = _configuration.GetSection("sercrt").Value.ToString();
            //颁发者
            string issuer = _configuration.GetSection("issuer").Value.ToString();
            //接收者
            string audience = _configuration.GetSection("audience").Value.ToString();
            //制定密钥
            var sercrtKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(sercrt));
            //设置凭据 指定对应的签名算法
            var signingCredentials = new SigningCredentials(sercrtKey, SecurityAlgorithms.HmacSha256);
            //设置payload 每一个claim都代表一个属性的键值对
            var claims = new Claim[] {
             new Claim("userID",UserId),
             new Claim("userName",UserName),
             new Claim("roles","Admin")
            };
            //组装生成token数据
            SecurityToken securityToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                signingCredentials: signingCredentials,
                //设置过期时间为30分钟
                expires:DateTime.Now.AddMinutes(30)
                );
            //生成token
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
