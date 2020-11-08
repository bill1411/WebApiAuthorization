using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Filter;
using WebApplication.Helper;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class LoginController : ApiController
    {
        // 这个属性是必须的，表示这个类是不需要token验证的
        [HttpPost]
        [AllowAnonymous] 
        public string GetToken(dynamic obj)
        {
            string username = obj.username;
            string password = obj.password;
            if (CheckUser(username, password))
            {
                AuthInfo model = new AuthInfo();
                model.Role = "admin,manager";
                model.UserName = "admin";
                model.UserPhone = "888888";
                return TokenHelper.GenerateToken(model);
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }


        // 此方法验证的token需要调用Authentication属性方法
        [HttpGet]
        [Authentication] 
        public string Get()
        {
            return "value";
        }

        private bool CheckUser(string username, string password)
        {
            if (username == "admin" && password == "888888")
                return true;
            else
                return false;
        }
    }
}
