using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Attributes;
using WebApplication.Helper;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class TokenController : ApiController
    {
        //1，fromBody:在cation方法传入参数后添加[frombody]属性，参数将以一个整体的josn对象的形式传递。
        //2，fromform:在cation方法传入参数后添加[frombody] 属性，参数将以表单的形式提交。
        [HttpPost]
        public LoginResult Login([FromBody]LoginRequest request)
        {
            LoginResult response = new LoginResult();
            if (request.UserName == "admin" && request.Password == "123456")
            {
                //初始化payload
                AuthInfo info = new AuthInfo { UserName = "amdin", Roles = new List<string> { "Admin", "Manager" }, IsAdmin = true };


                Dictionary<String, Object> claims = new Dictionary<String, Object>();
                //JWT的签发者
                claims.Add("iss", "www.baidu.com");
                claims.Add("role", "admin,manager");
                //定义签发的对象 比如用户ID
                claims.Add("sub", "2008050260");
                //签发时间
                claims.Add("iat", UnixHelper.GetTimeStamp(DateTime.Now).ToString());
                //过期时间  默认延长15分钟
                claims.Add("exp", UnixHelper.GetTimeStamp(DateTime.Now.AddMinutes(1)).ToString());


                try
                {
                    const string secret = "No one can stand in the way of the Rejuvenation of the Chinese people!";
                    //初始化jwt算法对象
                    IJwtAlgorithm algorithm = new HMACSHA256Algorithm();   //除了256  还有512   318
                    //初始化序列化对象
                    IJsonSerializer serializer = new JsonNetSerializer();
                    //初始化urlencode对象
                    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                    //定义jwtEncode对象
                    IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
                    //生成token值
                    var token = encoder.Encode(claims,info, secret);
                    //生成token的例子： 
                    /*第一个.号前字符串称之为header   base64解码后的明文为：{"typ":"JWT","alg":"HS256"}   
                     *第二个.号前字符串称之为payload，可自定义一些非敏感信息  base64解码后的明文为：{"UserName":"amdin","Roles":["Admin","Manager"],"IsAdmin":true}
                     * 第三部分为验签部分 组成方式是：
                         HMACSHA256(  
                                    base64UrlEncode(header) + "." +
                                    base64UrlEncode(payload),
                                    SECREATE_KEY
                                    )
                     */
                    response.Message = "请求成功";
                    response.Code = 10000;
                    response.Token = token;
                    response.Time = DateTime.Now.ToString();
                }
                catch {

                }
            }
            else
            {
                response.Message = "用户名或密码错误";
                response.Code = 10001;
                response.Token = "";
                response.Time = DateTime.Now.ToString();
            }

            return response;

        }

        [ApiAuthorize]
        public string Get()
        {
            AuthInfo info = RequestContext.RouteData.Values["auth"] as AuthInfo;
            if (info == null)
            {
                return "info信息获取失败";
            }
            else
            {
                return $"获取到了，Auth的Name是 {info.UserName}";
            }
        }
    }
}
