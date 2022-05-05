using AuthStudy.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Options;
using Services.InterFace;

namespace AuthStudy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserColltroller: ControllerBase
    {
        private IUserServices _userServices;
        private IConfiguration _configuration;
        //修改json的值 可以在程序运行的情况下加载进来
        private IOptions<TokenModel> _options;
        public List<int> MyList; 
        public UserColltroller(IConfiguration configuration, IOptions<TokenModel> options, IUserServices userServices) {
            _configuration = configuration;
            _userServices = userServices;
            _options = options;
            MyList = new List<int>();
            MyList.Add(1);
            MyList.Add(2);
            MyList.Add(3);
            MyList.Add(4);
            MyList.Add(5);
            MyList.Add(6);
            MyList.Add(7);
            MyList.Add(8);
            MyList.Add(9);
            MyList.Add(10);
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

            //加载配置
            TokenModel tokenModel = _configuration.Get<TokenModel>();


            /* //设置密钥
             string sercrt = _configuration.GetSection("sercrt").Value.ToString();
             //颁发者
             string issuer = _configuration.GetSection("issuer").Value.ToString();
             //接收者
             string audience = _configuration.GetSection("audience").Value.ToString();*/

            //_options.Value.issuer 使用全局option加载配置
            //制定密钥
            var sercrtKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Value.issuer));
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
                issuer: _options.Value.issuer,
                audience: _options.Value.audience,
                claims: claims,
                signingCredentials: signingCredentials,
                //设置过期时间为30分钟
                expires:DateTime.Now.AddMinutes(30)
                );
            //生成token
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
        /// <summary>
        /// 在集合中查找集合
        /// linq方式实现
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryList")]
        public List<int> QueryList() { 
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(3);
            list.Add(5);
            //寻找 1,3,5 的数据集合
            List<int> thisList = MyList.Where(s => list.Contains(s)).ToList();
            return thisList;
        }
        /// <summary>
        /// 查询字符串中出现频率最高的字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet("Sort")]
        public char Sort(string str) { 
            //转成char数组
            var chars = str.ToCharArray();
            var max =chars.GroupBy(s=>s).OrderByDescending(s=>s.Count()).FirstOrDefault();
            var min = chars.GroupBy(s => s).OrderBy(s => s.Count()).FirstOrDefault();
            return min.SingleOrDefault();
        }
        /// <summary>
        /// 测试配置接口
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("SelectName/{Id}")]
        public Task<string> SelectName(string Id) {
            // string aaa = _options.Value.issuer;
            return _userServices.GetUserId(Id);
        }
    }
}
