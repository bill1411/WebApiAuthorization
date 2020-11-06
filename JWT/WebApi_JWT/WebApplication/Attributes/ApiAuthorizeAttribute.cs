
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using WebApplication.Helper;
using WebApplication.Models;

namespace WebApplication.Attributes
{
    public class ApiAuthorizeAttribute: AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            //获取header中关于auth的参数
            var authHeader = from t in actionContext.Request.Headers where t.Key == "auth" select t.Value.FirstOrDefault();
            if (authHeader != null)
            {
                string token = authHeader.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    try
                    {
                        const string secret = "No one can stand in the way of the Rejuvenation of the Chinese people!";

                        //初始化jwt算法对象
                        IJwtAlgorithm algorithm = new HMACSHA256Algorithm();   //除了256  还有512   318
                        //初始化序列化对象
                        IJsonSerializer serializer = new JsonNetSerializer();
                        //初始化urlencode对象
                        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                        //初始化时间对象
                        IDateTimeProvider provider = new UtcDateTimeProvider();
                        //初始化验证对象
                        IJwtValidator validator = new JwtValidator(serializer,provider);
                        //定义jwtDeEncode对象
                        IJwtDecoder deCoder = new JwtDecoder(serializer,validator,urlEncoder,algorithm);
                        //解密并验证token值，如果错误json为null  否则放行
                        var json = deCoder.DecodeToObject<AuthInfo>(token, secret, verify: true);
                        //获取extraHeaders
                        IDictionary<string, string> dict = deCoder.DecodeHeaderToDictionary(token);
                        if (json != null && dict != null)
                        {
                            //token发布的时间
                            string iat = dict["iat"].ToString();
                            //token失效的时间
                            string exp = dict["exp"].ToString();
                            //验证token是否在有效期内
                            if (CheckTokenTime(iat, exp))
                            {
                                actionContext.RequestContext.RouteData.Values.Add("auth", json);
                                return true;
                            }
                            return false;
                        }
                        return false;
                    }
                    catch(Exception ex)
                    {
                        return false;
                    }
                }
                return false;
            }
            return false;
        }

        #region  验证token是否有效
        private bool CheckTokenTime(string iat,string exp)
        {
            DateTime beginTime = DateTime.Parse(UnixHelper.LongDateTimeToDateTimeString(iat));
            DateTime expTime = DateTime.Parse(UnixHelper.LongDateTimeToDateTimeString(exp));
            if (beginTime < DateTime.Now && DateTime.Now <= expTime)
                return true;
            else
                return false;
        }
        #endregion 
    }
}