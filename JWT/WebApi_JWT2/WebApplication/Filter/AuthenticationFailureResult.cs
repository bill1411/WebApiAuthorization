﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApplication.Filter
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public string _FailureReason { get; }
        public HttpRequestMessage _Request { get; }
        public AuthenticationFailureResult(string FailureReason, HttpRequestMessage request)
        {
            _FailureReason = FailureReason;
            _Request = request;
        }
        HttpResponseMessage HandleResponseMessage()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = _Request,
                ReasonPhrase = _FailureReason
            };
            return response;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(HandleResponseMessage());
        }
    }
}