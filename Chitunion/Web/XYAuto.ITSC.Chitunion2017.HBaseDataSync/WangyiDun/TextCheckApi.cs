using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync.WangyiDun
{
    public class TextCheckApi
    {
        public static readonly TextCheckApi Instance = new TextCheckApi();
        private string DunWangyi_SecretId = ConfigurationManager.AppSettings["DunWangyi_SecretId"];
        private string DunWangyi_SecretKey = ConfigurationManager.AppSettings["DunWangyi_SecretKey"];
        private string DunWangyi_TextBusinessID = ConfigurationManager.AppSettings["DunWangyi_TextBusinessID"];
        private string DunWangyi_TextBusinessURL = ConfigurationManager.AppSettings["DunWangyi_TextBusinessURL"];
        private int DunWangyi_InterfaceTimeOut = ConfigurationManager.AppSettings["DunWangyi_InterfaceTimeOut"] == null ? 3000 : int.Parse(ConfigurationManager.AppSettings["DunWangyi_InterfaceTimeOut"].ToString());



        /// <summary>
        /// 根据文章文本内容，调用网易易盾接口，检测是否符合规范
        /// </summary>
        /// <param name="articleID">文章标识</param>
        /// <param name="textContent">文章文本内容</param>
        /// <param name="articleUrl"></param>
        /// <returns>返回调用接口内容</returns>
        public TextResult Check(string articleID, string textContent, string articleUrl)
        {
            try
            {
                Dictionary<String, String> parameters = new Dictionary<String, String>();

                long curr = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                String time = curr.ToString();

                // 1.设置公共参数
                parameters.Add("secretId", DunWangyi_SecretId);
                parameters.Add("businessId", DunWangyi_TextBusinessID);
                parameters.Add("version", "v3");
                parameters.Add("timestamp", time);
                parameters.Add("nonce", new Random().Next().ToString());

                // 2.设置私有参数
                parameters.Add("dataId", articleID);
                parameters.Add("content", textContent);
                //parameters.Add("dataType", "1");
                //parameters.Add("ip", "123.115.77.137");
                parameters.Add("account", articleID);
                //parameters.Add("deviceType", "4");
                //parameters.Add("deviceId", "92B1E5AA-4C3D-4565-A8C2-86E297055088");
                //parameters.Add("callback", "ebfcad1c-dba1-490c-b4de-e784c2691768");
                parameters.Add("publishTime", time);

                // 3.生成签名信息
                String signature = Utils.genSignature(DunWangyi_SecretKey, parameters);
                parameters.Add("signature", signature);

                // 4.发送HTTP请求
                HttpClient client = Utils.makeHttpClient();
                DateTime dt = DateTime.Now;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                String result = Utils.doPost(client, DunWangyi_TextBusinessURL, parameters, DunWangyi_InterfaceTimeOut);
                sw.Stop();
                BLL.Loger.Log4Net.Info($"接口[{DunWangyi_TextBusinessURL}]，调用耗时：{sw.ElapsedMilliseconds},返回数据为：{result}");
                TextResult tr = JsonConvert.DeserializeObject<TextResult>(result);
                if (tr != null && tr.code == 200 && tr.result.action != 0)
                {
                    BLL.Loger.Log4Net.Info($"接口[{DunWangyi_TextBusinessURL}],返回有问题数据为：articleID={articleID},articleUrl={articleUrl},action={tr.result.action},labels={JsonConvert.SerializeObject(tr.result.labels)}");
                }
                return tr;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error($"调用网易云接口（文本检测）异常：参数:articleID={articleID},textContent={textContent}", ex);
                return null;
            }
        }
    }
}
