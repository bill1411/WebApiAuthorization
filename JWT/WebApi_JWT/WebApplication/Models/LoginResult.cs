using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class LoginResult
    {
        public int Code { get; set; }

        public string Token { get; set; }

        public string Message { get; set; }
        public string Time { get; set; }
    }
}