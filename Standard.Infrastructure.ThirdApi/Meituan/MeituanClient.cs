using Standard.Infrastructure.Encryptions;
using Standard.Infrastructure.Helpers;
using Standard.Infrastructure.ThirdApi.Meituan;
using Standard.Infrastructure.WebClients;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Standard.Infrastructure.ThirdApi.Meituan
{
    /// <summary>
    /// 美团客户端
    /// </summary>
    public class MeituanClient
    {
        HttpWebClient client;

        MeituanRequest request;

        public MeituanClient()
        {
            client = new HttpWebClient();
            request = new MeituanRequest();
        }

        #region 方法

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MeituanClient Add(string key, object value)
        {
            string str = "";
            if (value != null)
                str = value.ToString();
            request.SetValue(key, str);
            return this;
        }

        #endregion

        #region 订单服务

        /// <summary>
        /// 确认订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task OrderConfirm()
        {
            if (!request.ExistsKey("orderId"))
            {
                throw new Exception("确认订单，orderId是必须的！");
            }

            string url = "http://api.open.cater.meituan.com/waimai/order/confirm";

            await PostRequest(url);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task OrderCancel()
        {
            if (!request.ExistsKey("orderId"))
            {
                throw new Exception("取消订单，orderId是必须的！");
            }

            if (!request.ExistsKey("reason"))
            {
                throw new Exception("取消订单，reason是必须的！");
            }

            string url = "http://api.open.cater.meituan.com/waimai/order/cancel";

            //=========================================
            //2001	商家超时接单【商家取消时填写】
            //2002	非顾客原因修改订单
            //2003	非客户原因取消订单
            //2004	配送延迟
            //2005	售后投诉
            //2006	用户要求取消
            //2007	其他原因（未传code，默认为此）
            //=========================================
            if (!request.ExistsKey("reasonCode"))
            {
                request.SetValue("reasonCode", "2001");
            }

            await PostRequest(url);
        }

        /// <summary>
        /// 自配送-配送
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task OrderDelivering()
        {
            if (!request.ExistsKey("orderId"))
            {
                throw new Exception("自配送-配送，orderId是必须的！");
            }

            string url = "http://api.open.cater.meituan.com/waimai/order/delivering";
            await PostRequest(url);
        }


        /// <summary>
        /// 自配送-送达
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task OrderDelivered()
        {
            if (!request.ExistsKey("orderId"))
            {
                throw new Exception("自配送-送达，orderId是必须的！");
            }

            string url = "http://api.open.cater.meituan.com/waimai/order/delivered";
            await PostRequest(url);
        }

        /// <summary>
        /// 众包-配送
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task OrderDispatchShip()
        {
            if (!request.ExistsKey("orderId"))
            {
                throw new Exception("众包-配送，orderId是必须的！");
            }

            string url = "http://api.open.cater.meituan.com/waimai/order/dispatchShip";
            await PostRequest(url);
        }

        /// <summary>
        /// 众包-取消配送
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task OrderCancelDispatch()
        {
            if (!request.ExistsKey("orderId"))
            {
                throw new Exception("众包-取消配送，orderId是必须的！");
            }

            string url = "http://api.open.cater.meituan.com/waimai/order/cancelDispatch";
            await PostRequest(url);
        }

        /// <summary>
        /// 众包-查询配送费
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> QueryZbShippingFee()
        {
            if (!request.ExistsKey("orderIds"))
            {
                throw new Exception("众包-查询配送费，orderIds是必须的！");
            }

            string url = "http://api.open.cater.meituan.com/waimai/order/queryZbShippingFee";
            return await GetRequest(url);
        }

        /// <summary>
        /// 众包-预下单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task OrderPrepareZbDispatch()
        {
            if (!request.ExistsKey("orderId"))
            {
                throw new Exception("众包-预下单，orderId是必须的！");
            }

            if (!request.ExistsKey("shippingFee"))
            {
                throw new Exception("众包-预下单，shippingFee是必须的！");
            }

            if (!request.ExistsKey("tipAmount"))
            {
                request.SetValue("tipAmount", "0.0");
            }

            string url = "http://api.open.cater.meituan.com/waimai/order/prepareZbDispatch";
            await PostRequest(url);
        }

        /// <summary>
        /// 众包-加小费
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task OrderUpdateZbDispatchTip()
        {
            if (!request.ExistsKey("orderId"))
            {
                throw new Exception("众包-加小费，orderId是必须的！");
            }
            if (!request.ExistsKey("tipAmount"))
            {
                throw new Exception("众包-加小费，tipAmount是必须的！");
            }

            string url = "http://api.open.cater.meituan.com/waimai/order/updateZbDispatchTip";

            await PostRequest(url);
        }

        /// <summary>
        /// 众包-确认单据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task OrderConfirmZbDispatch()
        {
            if (!request.ExistsKey("orderId"))
            {
                throw new Exception("众包-确认单据，orderId是必须的！");
            }

            string url = "http://api.open.cater.meituan.com/waimai/order/confirmZbDispatch";

            await PostRequest(url);
        }


        /// <summary>
        /// 同意退款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task OrderAgreeRefund()
        {
            if (!request.ExistsKey("orderId"))
            {
                throw new Exception("同意退款，orderId是必须的！");
            }

            if (!request.ExistsKey("reason"))
            {
                throw new Exception("同意退款，reason是必须的！");
            }

            string url = "http://api.open.cater.meituan.com/waimai/order/agreeRefund";

            await PostRequest(url);
        }

        /// <summary>
        /// 拒绝退款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task OrderRejectRefund()
        {
            if (!request.ExistsKey("orderId"))
            {
                throw new Exception("拒绝退款，orderId是必须的！");
            }

            if (!request.ExistsKey("reason"))
            {
                throw new Exception("拒绝退款，reason是必须的！");
            }

            string url = "http://api.open.cater.meituan.com/waimai/order/rejectRefund";

            await PostRequest(url);
        }


        /// <summary>
        /// 查询待确认单据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> QueryByEpoidsOrder()
        {
            if (!request.ExistsKey("epoiIds"))
            {
                throw new Exception("查询待确认单据，epoiIds是必须的！");
            }
            string url = "http://api.open.cater.meituan.com/waimai/order/queryByEpoids";

            return await PostRequest(url);
        }

        #endregion

        #region 门店服务

        /// <summary>
        /// 置营业
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task PoiOpen()
        {
            string url = "http://api.open.cater.meituan.com/waimai/poi/open";
            await PostRequest(url);
        }

        /// <summary>
        /// 置休息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task PoiClose()
        {
            string url = "http://api.open.cater.meituan.com/waimai/poi/close";
            await PostRequest(url);
        }

        /// <summary>
        /// 修改营业时间
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task PoiUpdateOpenTime()
        {
            if (!request.ExistsKey("openTime"))
            {
                throw new Exception("修改营业时间，openTime是必须的！");
            }
            string url = "http://api.open.cater.meituan.com/waimai/poi/updateOpenTime";
            await PostRequest(url);
        }

        /// <summary>
        /// 查询门店信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<dynamic> PoiQueryPoiInfo()
        {
            if (!request.ExistsKey("ePoiIds"))
            {
                throw new Exception("修改营业时间，ePoiIds是必须的！");
            }
            string url = "http://api.open.cater.meituan.com/waimai/poi/queryPoiInfo";
            return await GetRequest(url);
        }

        #endregion

        #region 商品服务

        /// <summary>
        /// 商品映射
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task DishMapping()
        {

            if (!request.ExistsKey("ePoiId"))
            {
                throw new Exception("商品映射，ePoiId是必须的！");
            }

            if (!request.ExistsKey("dishMappings"))
            {
                throw new Exception("商品映射，ePoiId是必须的！");
            }

            string url = "http://api.open.cater.meituan.com/waimai/dish/mapping";
            await PostRequest(url);
        }


        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> QueryBaseListByEPoiId()
        {
            if (!request.ExistsKey("ePoiIds"))
            {
                throw new Exception("获取商品信息，ePoiIds是必须的！");
            }
            string url = "http://api.open.cater.meituan.com/waimai/dish/queryBaseListByEPoiId";
            return await GetRequest(url);
        }

        #endregion

        #region 请求
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<dynamic> GetRequest(string url)
        {
            request.SetSign();
            var param = request.ToUrlParamter();
            var res = await client.GetAsync(url + "?" + param);

            if (res.StatusCode == System.Net.HttpStatusCode.OK || res.StatusCode == 0)
            {
                return ReturnResult(res);
            }
            else
            {
                throw new Exception("发起美团请求失败，原因：" + res.ErrorMessage);
            }
        }


        /// <summary>
        /// 发起Post请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<dynamic> PostRequest(string url)
        {
            request.SetSign();

            var res = await client.PostFormAsync(url, request.Params);

            if (res.StatusCode == System.Net.HttpStatusCode.OK || res.StatusCode == 0)
            {
                return ReturnResult(res);
            }
            else
            {
                throw new Exception("发起美团请求失败，原因：" + res.ErrorMessage);
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

            if (obj.error != null)
            {
                string error = JsonHelper.ToJson(obj.error);
                throw new Exception(error);
            }
            else
            {
                return obj.data;
            }
        }

        #endregion
    }




}
