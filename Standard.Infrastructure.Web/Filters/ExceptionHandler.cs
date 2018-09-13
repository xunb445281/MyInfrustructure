using Microsoft.AspNetCore.Mvc.Filters;
using Standard.Infrastructure.Webs.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Webs.Filters
{
    /// <summary>
    /// 异常处理过滤器
    /// </summary>
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常处理
        /// </summary>
        public override void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 200;
            context.Result = new HttpResult(StateCode.Fail, context.Exception.Message);
        }
    }
}
