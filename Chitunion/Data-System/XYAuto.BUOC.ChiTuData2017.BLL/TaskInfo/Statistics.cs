/********************************************************
*创建人：hant
*创建时间：2017/12/20 10:08:37 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo.Dto.Response;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;

namespace XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo
{
    public class Statistics
    {
        public static readonly Statistics Instance = new Statistics();



        public ResponseGetStatisticsByOrderUrl GetStatisticsByOrderUrl(RequestGetStatisticsByOrderUrl req, ref string code, ref string msg)
        {
            ResponseGetStatisticsByOrderUrl list = new ResponseGetStatisticsByOrderUrl();

            try
            {
                #region 验证
                //参数验证
                if (Authentication.Instance.ParaValid<RequestGetStatisticsByOrderUrl>(req, ref code, ref msg))
                {
                    //用户验证
                    if (Authentication.Instance.Access(Convert.ToInt32(req.appId), req.appkey, ref msg, ref code))
                    {
                        string sign = Authentication.Instance.GetSign<RequestGetStatisticsByOrderUrl>(req);
                        //签名验证
                        if (Authentication.Instance.SignValid(req.sign, sign, ref code, ref msg))
                        {
                            //调用次数
                            if (Authentication.Instance.CallNumber(Convert.ToInt32(req.appId), req.appkey, ref msg, ref code))
                            {
                                //获取数据
                                List<Entities.Task.TaskStatistics> listtask = GetStatisticsByCode(req, ref code, ref msg);
                                code = "1";
                                msg = "success";
                                list.List = listtask;
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error($"GetStatisticsByOrderUrl :{ex.Message}");
                msg = "系统错误";
                code = "-1";
                list.List = null;
            }
            return list;
        }

        private List<Entities.Task.TaskStatistics> GetStatisticsByCode(RequestGetStatisticsByOrderUrl req, ref string code, ref string msg)
        {
            int totalCount = 0;
            Uri uri = new Uri(req.orderurl);
            string queryString = uri.Query;
            NameValueCollection col = GetQueryString(queryString);
            string codeutm = col["utm_term"];
            return Dal.TaskInfo.Statistics.Instance.GetStatisticsByCode(codeutm, req.begindate, req.enddate, Convert.ToInt32(req.page_index), Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("InterfacePageSize")), out totalCount);
        }

        /// <summary>
        /// 将查询字符串解析转换为名值集合.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        private  NameValueCollection GetQueryString(string queryString)
        {
            return GetQueryString(queryString, null, true);
        }

        /// <summary>
        /// 将查询字符串解析转换为名值集合.
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="encoding"></param>
        /// <param name="isEncoded"></param>
        /// <returns></returns>
        private NameValueCollection GetQueryString(string queryString, Encoding encoding, bool isEncoded)
        {
            queryString = queryString.Replace("?", "");
            NameValueCollection result = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(queryString))
            {
                int count = queryString.Length;
                for (int i = 0; i < count; i++)
                {
                    int startIndex = i;
                    int index = -1;
                    while (i < count)
                    {
                        char item = queryString[i];
                        if (item == '=')
                        {
                            if (index < 0)
                            {
                                index = i;
                            }
                        }
                        else if (item == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string key = null;
                    string value = null;
                    if (index >= 0)
                    {
                        key = queryString.Substring(startIndex, index - startIndex);
                        value = queryString.Substring(index + 1, (i - index) - 1);
                    }
                    else
                    {
                        key = queryString.Substring(startIndex, i - startIndex);
                    }
                    if (isEncoded)
                    {
                        result[UrlDeCode(key, encoding)] = UrlDeCode(value, encoding);
                    }
                    else
                    {
                        result[key] = value;
                    }
                    if ((i == (count - 1)) && (queryString[i] == '&'))
                    {
                        result[key] = string.Empty;
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// 解码URL.
        /// </summary>
        /// <param name="encoding">null为自动选择编码</param>
        /// <param name="str"></param>
        /// <returns></returns>
        private string UrlDeCode(string str, Encoding encoding)
        {
            if (encoding == null)
            {
                Encoding utf8 = Encoding.UTF8;
                //首先用utf-8进行解码                     
                string code = HttpUtility.UrlDecode(str.ToUpper(), utf8);
                //将已经解码的字符再次进行编码.
                string encode = HttpUtility.UrlEncode(code, utf8).ToUpper();
                if (str == encode)
                    encoding = Encoding.UTF8;
                else
                    encoding = Encoding.GetEncoding("gb2312");
            }
            return HttpUtility.UrlDecode(str, encoding);
        }
    }
}
