using Standard.Infrastructure.Helpers;
using Standard.Infrastructure.WebClients;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Standard.Infrastructure.ThirdApi.Eleme
{
    public class ElemeClient
    {
        HttpWebClient client = null;
        ElmRequest request = null;

        public ElemeClient()
        {
            client = new HttpWebClient();
            request = new ElmRequest();
        }


        #region 方法

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ElemeClient Add(string key, object value)
        {
            request.SetParams(key, value);
            return this;
        }

        /// <summary>
        /// 设置token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ElemeClient AddToken(string token)
        {
            request.token = token;
            return this;
        }

        #endregion

        #region 授权

        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <param name="url"></param>
        /// <param name="poiId"></param>
        /// <returns></returns>
        public string GetAuthCode(string host, long poiId)
        {
            string returnUrl = "http://" + host + "/api/elmpoi/authcallback";
            returnUrl = HttpUtility.HtmlEncode(returnUrl);
            return $"{ElemeOptions.GetAuthUrl()}?response_type=code&client_id={ElemeOptions.Key}&redirect_uri={returnUrl}&state={poiId}&scope=all";
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<string> GetToken(string code, string state, string host)
        {
            string authCode = System.Convert.ToBase64String(Encoding.ASCII.GetBytes($"{ElemeOptions.Key}:{ElemeOptions.Secret}"));
            client.AddAuthorization("Basic", authCode);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("grant_type", "authorization_code");
            dic.Add("code", code);
            string returnUrl = "http://" + host + "/api/elmpoi/authcallback";
            //returnUrl = HttpUtility.UrlEncode(returnUrl);
            dic.Add("redirect_uri", returnUrl);
            dic.Add("client_id", ElemeOptions.Key);
            var res = await client.PostFormAsync(ElemeOptions.GetAuthTokenUrl(), dic);
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return res.Result;
            }
            else
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<string> RefreshToken(string refreshToken)
        {
            string authCode = System.Convert.ToBase64String(Encoding.ASCII.GetBytes($"{ElemeOptions.Key}:{ElemeOptions.Secret}"));
            client.AddAuthorization("Basic", authCode);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("grant_type", "refresh_token");
            dic.Add("refresh_token", refreshToken);
            var res = await client.PostFormAsync(ElemeOptions.GetAuthTokenUrl(), dic);
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return res.Result;
            }
            else
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        #endregion

        #region 订单
        /// <summary>
        /// 确认订单
        /// </summary>
        /// <returns></returns>
        public async Task OrderConfirm()
        {
            if (!request.ExistsParams("orderId"))
            {
                throw new Exception("请求失败，orderId是必须的！");
            }

            request.action = "eleme.order.confirmOrderLite";
            await PostRequest();
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <returns></returns>
        public async Task OrderCancel()
        {
            if (!request.ExistsParams("orderId"))
            {
                throw new Exception("请求失败，orderId是必须的！");
            }

            if (!request.ExistsParams("type"))
            {
                throw new Exception("请求失败，type是必须的！");
            }

            request.action = "eleme.order.cancelOrderLite";

            await PostRequest();
        }

        /// <summary>
        /// 同意退单/同意取消单
        /// </summary>
        /// <returns></returns>
        public async Task OrderAgreeRefund()
        {
            if (!request.ExistsParams("orderId"))
            {
                throw new Exception("请求失败，orderId是必须的！");
            }

            request.action = "eleme.order.agreeRefundLite";

            await PostRequest();
        }

        /// <summary>
        /// 不同意退单/不同意取消单
        /// </summary>
        /// <returns></returns>
        public async Task OrderRejectRefund()
        {
            if (!request.ExistsParams("orderId"))
            {
                throw new Exception("请求失败，orderId是必须的！");
            }

            request.action = "eleme.order.disagreeRefundLite";

            await PostRequest();
        }

        /// <summary>
        /// 订单确认送达
        /// </summary>
        /// <returns></returns>
        public async Task OrderDelivered()
        {
            if (!request.ExistsParams("orderId"))
            {
                throw new Exception("请求失败，orderId是必须的！");
            }

            request.action = "eleme.order.receivedOrderLite";

            await PostRequest();
        }

        /// <summary>
        /// 订单自行配送(众包配送异常)
        /// </summary>
        /// <returns></returns>
        public async Task OrderDelivering()
        {
            if (!request.ExistsParams("orderId"))
            {
                throw new Exception("请求失败，orderId是必须的！");
            }

            request.action = "eleme.order.deliveryBySelfLite";

            await PostRequest();
        }

        /// <summary>
        /// 众包-查询费用,返回费用
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetDeliveryFeeForCrowd()
        {
            if (!request.ExistsParams("orderId"))
            {
                throw new Exception("请求失败，orderId是必须的！");
            }

            request.action = "eleme.order.getDeliveryFeeForCrowd";

            var result = await PostRequest();

            return Infrastructure.Helpers.Convert.ToDouble(result);
        }

        /// <summary>
        /// 众包-呼叫配送
        /// </summary>
        /// <returns></returns>
        public async Task CallDelivery()
        {
            if (!request.ExistsParams("orderId"))
            {
                throw new Exception("请求失败，orderId是必须的！");
            }

            if (!request.ExistsParams("fee"))
            {
                throw new Exception("请求失败，fee是必须的！");
            }

            request.action = "eleme.order.callDelivery";

            await PostRequest();
        }

        /// <summary>
        /// 众包-取消呼叫配送
        /// </summary>
        /// <returns></returns>
        public async Task CancelDelivery()
        {
            if (!request.ExistsParams("orderId"))
            {
                throw new Exception("请求失败，orderId是必须的！");
            }

            request.action = "eleme.order.cancelDelivery";

            await PostRequest();
        }

        /// <summary>
        /// 众包-订单加小费
        /// </summary>
        /// <returns></returns>
        public async Task AddDeliveryTipByOrderId()
        {
            if (!request.ExistsParams("orderId"))
            {
                throw new Exception("请求失败，orderId是必须的！");
            }

            if (!request.ExistsParams("tip"))
            {
                throw new Exception("请求失败，tip是必须的！");
            }

            request.action = "eleme.order.addDeliveryTipByOrderId";

            await PostRequest();
        }
        #endregion

        #region 签约服务

        /// <summary>
        /// 查询店铺当前生效合同类型,返回contractTypeName，string
        /// 专送和快送属于平台，不需要调用发起配送
        /// 众包需要自行调用呼叫配送
        /// 自配送调用自配送
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> GetEffectServicePackContract()
        {
            if (!request.ExistsParams("shopId"))
            {
                throw new Exception("请求失败，shopId是必须的！");
            }

            request.action = "eleme.packs.getEffectServicePackContract";

            var result = await PostRequest();

            return result;
        }

        #endregion

        #region 店铺
        /// <summary>
        /// 更新店铺信息
        /// </summary>
        /// <returns></returns>
        public async Task UpdateShop()
        {
            if (!request.ExistsParams("shopId"))
            {
                throw new Exception("请求失败，orderId是必须的！");
            }

            if (!request.ExistsParams("properties"))
            {
                throw new Exception("请求失败，properties是必须的！");
            }

            request.action = "eleme.shop.updateShop";

            await PostRequest();
        }

        #endregion

        #region 商品

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<dynamic> GetItems()
        {
            if (!request.ExistsParams("queryPage"))
            {
                throw new Exception("请求失败，queryPage是必须的！");
            }

            request.action = "eleme.product.item.queryItemByPage";

            return await PostRequest();
        }


        #endregion 

        #region 请求
        /// <summary>
        /// 发起Post请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<dynamic> PostRequest()
        {
            request.SetSign();

            string url = ElemeOptions.GetUrl();

            var res = await client.PostJsonAsync(url, request);

            if (res.StatusCode == System.Net.HttpStatusCode.OK || res.StatusCode == 0)
            {
                return ReturnResult(res);
            }
            else
            {
                throw new Exception("发起饿了么请求失败，原因：" + res.ErrorMessage);
            }
        }

        /// <summary>
        /// 判断返回值
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        private static dynamic ReturnResult(HttpResponseData res)
        {
            var result = res.Result.Trim();

            var obj = JsonHelper.FromJson<dynamic>(result);

            if (obj.error == null)
            {
                return obj.result;
            }
            else
            {
                string error = JsonHelper.ToJson(obj.error);
                throw new Exception(error);
            }
        }
        #endregion

    }
}
