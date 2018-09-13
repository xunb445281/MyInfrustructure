using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Standard.Infrastructure;

namespace Standard.Infrastructure.Webs.Commons
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class HttpResult : JsonResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        private readonly StateCode code;
        /// <summary>
        /// 消息
        /// </summary>
        private readonly string message;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly dynamic data;

        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        public HttpResult(StateCode code, string message, dynamic data = null) : base(null)
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        public override Task ExecuteResultAsync(ActionContext context)
        {
            this.Value = new
            {
                Code = code.Value(),
                Message = message,
                Data = data
            };
            return base.ExecuteResultAsync(context);
        }
    }
}
