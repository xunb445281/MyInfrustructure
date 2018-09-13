using Microsoft.AspNetCore.Mvc;
using Standard.Infrastructure.Webs.Commons;
using Standard.Infrastructure.Webs.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Webs.Controllers
{
    /// <summary>
    /// WebApi控制器
    /// </summary>
    [Route("api/[controller]")]
    [ExceptionHandler]
    [AutoValidateAntiforgeryToken]
    public class WebApiControllerBase : Controller
    {
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="message">消息</param>
        protected virtual IActionResult Success(dynamic data = null, string message = null)
        {
            //if (message == null)
            //    message = R.Success;
            return new HttpResult(StateCode.Ok, message, data);
        }

        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="message">消息</param>
        protected IActionResult Fail(string message)
        {
            return new HttpResult(StateCode.Fail, message);
        }
    }
}
