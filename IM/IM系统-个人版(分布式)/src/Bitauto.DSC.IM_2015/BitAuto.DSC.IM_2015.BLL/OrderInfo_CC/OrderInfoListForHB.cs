using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using BitAuto.Utils.Config;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.BLL
{
    public class OrderInfoListForHB
    {
        private string HbaseInterface = "";
        private Dictionary<string, string> OType = null;

        public string TestGetData()
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("phoneNumber", "13399286343");
                parameters.Add("startTime", "2276-10-03 20:00:00");
                parameters.Add("endTime", "2276-10-03 20:00:10");
                HttpWebResponse rep = HttpHelper.CreatePostHttpResponse("http://192.168.104.29:8080/queryServer/qryorder", parameters, null);
                string data = HttpHelper.GetResponseString(rep);
                return data;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public OrderInfoListForHB()
        {
            HbaseInterface = CommonFunction.ObjectToString(System.Configuration.ConfigurationManager.AppSettings["HbaseInterface"]);
            OType = new Dictionary<string, string>();
            OType.Add("1", "车易通");
            OType.Add("2", "车易通");
            OType.Add("3", "车易通");
            OType.Add("5", "专题活动");
            OType.Add("7", "惠买车");
            OType.Add("8", "车服通");
            OType.Add("9", "汽车金融");
            OType.Add("11", "易车商城");
        }

        public List<OrderData> QueryOrderInfo(string tel, DateTime st, DateTime et)
        {
            try
            {
                List<OrderData> result = new List<OrderData>();
                if (string.IsNullOrEmpty(HbaseInterface))
                {
                    throw new Exception("HbaseInterface未配置！");
                }
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("phoneNumber", tel);
                parameters.Add("orderType", "[1,2,3,5,7,8,9,11]");
                parameters.Add("startTime", st.ToString("yyyy-MM-dd HH:mm:ss"));
                parameters.Add("endTime", et.ToString("yyyy-MM-dd HH:mm:ss"));
                HttpWebResponse rep = HttpHelper.CreatePostHttpResponse(HbaseInterface, parameters, null);
                string data = HttpHelper.GetResponseString(rep);
                //var jsondata2 = (List<OrderInfoHBList>)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(data, typeof(List<OrderInfoHBList>));

                OrderInfoHBList jsondata = (OrderInfoHBList)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(data, typeof(OrderInfoHBList));

                if (jsondata != null)
                {
                    //按照时间倒叙排列
                    jsondata.orders.Reverse();
                    foreach (OrderInfoHB item in jsondata.orders)
                    {
                        OrderData info = new OrderData();
                        info.UserName = GetItemData_User(CommonFunction.ObjectToInteger(item.orderType), item);
                        string[] array = GetItemData_Car(CommonFunction.ObjectToInteger(item.orderType), item);
                        info.MasterBrand = array[2];
                        info.Brand = array[1];
                        info.UserPhone = item.userPhone;
                        info.Serial = array[0];
                        info.OrderTime = CommonFunction.GetDateTimeStr(item.orderTime);
                        info.Source = OType[item.orderType];
                        info.OrderID = GetItemData_ID(CommonFunction.ObjectToInteger(item.orderType), item);
                        info.Remark = GetItemData_Remark(CommonFunction.ObjectToInteger(item.orderType), item);
                        info.Url = GetItemData_Url(CommonFunction.ObjectToInteger(item.orderType), item);
                        info.OrderType = item.orderType;
                        result.Add(info);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("OrderInfoListForHB-QueryOrderInfo", ex);
                return new List<OrderData>();
            }
        }

        /// 获取用户名称
        /// <summary>
        /// 获取用户名称
        /// </summary>
        /// <param name="ordertype"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetItemData_User(int ordertype, OrderInfoHB item)
        {
            switch (ordertype)
            {
                case 1:
                case 2:
                case 3:
                case 5:
                case 7:
                case 8:
                case 11:
                    return GetValue(item.orderDetail, "UserName");
                case 9:
                    return GetValue(item.orderDetail, "Name");
                default:
                    return "";
            }
        }
        /// 获取车型信息
        /// <summary>
        /// 获取车型信息
        /// </summary>
        /// <param name="ordertype"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private string[] GetItemData_Car(int ordertype, OrderInfoHB item)
        {
            string[] array = new string[3];
            string csid = "";
            switch (ordertype)
            {
                case 1:
                case 2:
                case 3:
                case 7:
                case 9:
                case 11:
                    string carid = GetValue(item.orderDetail, "CarID");
                    DataRow dr = null;
                    if (BLL.DictionaryDataCache.Instance.Car_Car.ContainsKey(carid))
                    {
                        dr = BLL.DictionaryDataCache.Instance.Car_Car[carid];
                    }
                    if (dr != null)
                    {
                        csid = dr["CsID"].ToString();
                    }
                    break;
                case 5:
                case 8:
                    csid = GetValue(item.orderDetail, "SerialID");
                    break;
            }
            if (!string.IsNullOrEmpty(csid))
            {
                DataRow cs_dr = null;
                if (BLL.DictionaryDataCache.Instance.Car_Serial.ContainsKey(csid))
                {
                    cs_dr = BLL.DictionaryDataCache.Instance.Car_Serial[csid];
                }
                if (cs_dr != null)
                {
                    array[0] = cs_dr["Name"].ToString(); //车型

                    DataRow b_dr = null;
                    if (BLL.DictionaryDataCache.Instance.Car_Brand.ContainsKey(cs_dr["BrandID"].ToString()))
                    {
                        b_dr = BLL.DictionaryDataCache.Instance.Car_Brand[cs_dr["BrandID"].ToString()];
                    }
                    if (b_dr != null)
                    {
                        array[1] = b_dr["Name"].ToString(); //品牌
                    }


                    DataRow mb_dr = null;
                    if (BLL.DictionaryDataCache.Instance.Car_MasterBrand.ContainsKey(cs_dr["MasterBrandID"].ToString()))
                    {
                        mb_dr = BLL.DictionaryDataCache.Instance.Car_MasterBrand[cs_dr["MasterBrandID"].ToString()];
                    }
                    if (mb_dr != null)
                    {
                        array[2] = mb_dr["Name"].ToString(); //主品牌
                    }
                }
            }
            return array;
        }

        /// 获取订单ID
        /// <summary>
        /// 获取订单ID
        /// </summary>
        /// <param name="ordertype"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetItemData_ID(int ordertype, OrderInfoHB item)
        {
            switch (ordertype)
            {
                case 1:
                case 2:
                case 3:
                    return GetValue(item.orderDetail, "ClueID");
                case 5:
                case 7:
                case 8:
                case 9:
                case 11:
                    return GetValue(item.orderDetail, "OrderID");
                default:
                    return "";
            }
        }
        /// 获取备注信息
        /// <summary>
        /// 获取备注信息
        /// </summary>
        /// <param name="ordertype"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetItemData_Remark(int ordertype, OrderInfoHB item)
        {
            string info = "";
            string DealerID = "";
            string DealerName = "";
            switch (ordertype)
            {
                case 1:
                case 2:
                case 3:
                    //经销商名称
                    DealerID = GetValue(item.orderDetail, "DealerID");
                    if (BLL.DictionaryDataCache.Instance.DMSMember.ContainsKey(DealerID))
                    {
                        DealerName = BLL.DictionaryDataCache.Instance.DMSMember[DealerID]["Name"].ToString();
                    }
                    info = DealerName;
                    break;
                case 5:
                    //专题名称
                    string AGuid = GetValue(item.orderDetail, "AGuid").ToLower();
                    string Aname = "";
                    if (BLL.DictionaryDataCache.Instance.AcitvityInfo.ContainsKey(AGuid))
                    {
                        Aname = BLL.DictionaryDataCache.Instance.AcitvityInfo[AGuid]["ActivityName"].ToString();
                    }
                    info = Aname;
                    break;
                case 7:
                case 9:
                    break;
                case 8:
                    //详细信息
                    //经销商名称
                    DealerID = GetValue(item.orderDetail, "DealerID");
                    if (BLL.DictionaryDataCache.Instance.DMSMember.ContainsKey(DealerID))
                    {
                        DealerName = BLL.DictionaryDataCache.Instance.DMSMember[DealerID]["Name"].ToString();
                    }
                    info = "经销商名称：" + DealerName + "<br/>";
                    //套餐名称：套餐内容1
                    string GoodsName = GetValue(item.orderDetail, "GoodsName");
                    info += "套餐名称：" + GoodsName + "<br/>";
                    //套餐金额：500元
                    string Price = GetValue(item.orderDetail, "Price");
                    info += "套餐金额：" + Price + "<br/>";
                    //套餐详情：有礼券
                    string Content = GetValue(item.orderDetail, "Content");
                    info += "套餐详情：" + Content + "<br/>";
                    //订单状态：状态1
                    string UserStatusName = GetValue(item.orderDetail, "UserStatusName");
                    info += "订单状态：" + UserStatusName + "<br/>";
                    //代金券：200元
                    //暂无数据
                    //行驶里程：1700公里
                    string Mileage = GetValue(item.orderDetail, "Mileage");
                    info += "行驶里程：" + Mileage + "<br/>";
                    //预约时间：2015-01-07 
                    string BookingDate = GetValue(item.orderDetail, "BookingDate");
                    info += "预约时间：" + BookingDate;
                    break;
                case 11:
                    //促销信息
                    //促销信息
                    //如：
                    //①订金10000元抵购车款
                    //②从下单到拥有最快仅需48小时
                    //③车源紧俏，请从速购车
                    string Remark = GetValue(item.orderDetail, "Remark");
                    info = Remark;
                    break;
            }
            return info;

        }
        /// 获取跳转链接
        /// <summary>
        /// 获取跳转链接
        /// </summary>
        /// <param name="ordertype"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetItemData_Url(int ordertype, OrderInfoHB item)
        {
            string info = "";
            switch (ordertype)
            {
                case 1:
                case 2:
                case 3:
                case 8:
                    //无链接
                    break;
                case 5:
                    //专题链接
                    string AGuid = GetValue(item.orderDetail, "AGuid").ToLower();
                    if (BLL.DictionaryDataCache.Instance.AcitvityInfo.ContainsKey(AGuid))
                    {
                        info = BLL.DictionaryDataCache.Instance.AcitvityInfo[AGuid]["Url"].ToString();
                    }
                    break;
                case 7: //惠买车
                    string url = BLL.CallRecord_ORIG_Business.Instance.GetTaskUrl("16", "157", "", "");
                    info = string.Format(url, GetValue(item.orderDetail, "OrderID"));
                    break;
                case 9: //汽车金融
                    string url_login = ConfigurationUtil.GetAppSettingValue("CarFinancial_URL");
                    Uri uri = new Uri(url_login);
                    info = uri.Scheme + "://" + uri.Authority + "/LoanOrder/HistoryOrder?orderid=" + GetValue(item.orderDetail, "OrderID") + "&type=viewdetail";
                    break;
                case 11: //商城
                    //info = "http://www.yichemall.com/manage/order/Detail?id=" + GetValue(item.orderDetail, "OrderID"); ;
                    break;
            }
            return info;
        }


        /// 获取数据
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetValue(List<List<string>> orderDetail, string key)
        {
            foreach (List<string> item in orderDetail)
            {
                if (item[0].ToLower().Trim() == key.ToLower().Trim())
                {
                    return item[1];
                }
            }
            return "";
        }
        private string GetValue(List<List<string>> orderDetail)
        {
            string info = "";
            foreach (List<string> item in orderDetail)
            {
                info += item[0] + "\t" + item[1] + "\r\n";
            }
            return info;
        }
    }


    public class HttpHelper
    {
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        public static HttpWebResponse CreateGetHttpResponse(string url, int timeout, string userAgent, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //对服务端证书进行有效性校验（非第三方权威机构颁发的证书，如自己生成的，不进行验证，这里返回true）
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;    //http版本，默认是1.1,这里设置为1.0
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "GET";

            //设置代理UserAgent和超时
            //request.UserAgent = userAgent;
            //request.Timeout = timeout;
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/json";

            //设置代理UserAgent和超时
            //request.UserAgent = userAgent;
            //request.Timeout = timeout; 

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //发送POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                buffer.Append("{");
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i != 0)
                    {
                        buffer.Append(",");
                    }
                    if (parameters[key].StartsWith("["))
                    {
                        buffer.AppendFormat("\"{0}\":{1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("\"{0}\":\"{1}\"", key, parameters[key]);
                    }
                    i++;
                }
                buffer.Append("}");
                byte[] data = Encoding.ASCII.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            string[] values = request.Headers.GetValues("Content-Type");
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 获取请求的数据
        /// </summary>
        public static string GetResponseString(HttpWebResponse webresponse)
        {
            using (Stream s = webresponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 验证证书
        /// </summary>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
    }
}
