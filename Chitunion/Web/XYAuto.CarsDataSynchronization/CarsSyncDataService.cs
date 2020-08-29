using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Topshelf;
using log4net;
using System.Net;
using System.IO;
namespace XYAuto.CarsDataSynchronization
{
    /// <summary>
    /// 2017-03-03 张立彬
    /// 汽车信息同步
    /// </summary>
    public class CarsSyncDataService : DataBase
    {

        List<string> CarsSyncError = new List<string>(); //【个数大于0，则出错】 用于收集错误信息

        readonly ILog Log = LogManager.GetLogger(typeof(CarsSyncDataService));

        private Timer _timer = null;

        string publicUrl = HostUrl;
        public CarsSyncDataService()
        {

            _timer = new Timer(60 * 1000) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) => RunService();
        }
        public void Start()
        {
            Log.Info("同步车型数据（车系、品牌）-----服务开始");
            _timer.Start();

        }
        public void Stop()
        {
            Log.Info("同步车型数据（车系、品牌）-----服务结束");
            _timer.Stop();
        }

        //public bool Start(HostControl hostControl)
        //{
        //    Log.Info("同步汽车车系数据；同步汽车品牌数据");
        //    _timer.Start();
        //    return true;
        //    //throw new NotImplementedException();
        //}
        //public bool Stop(HostControl hostControl)
        //{
        //    throw new NotImplementedException();
        //}
        /// <summary>
        /// 开始同步服务
        /// </summary>
        private void RunService()
        {
            
            if (DateTime.Now.Hour == OperateHour && DateTime.Now.Minute == OperateMinute)
            {
                Log.Info("[同步车型数据（车系、品牌）]任务轮训------------开始");
                CarsSyncError.Clear();
                //同步汽车和车系品牌
                CarsBrandSyncData();
                //todo:发送邮件
                //if (CarsSyncError.Count > 0)
                //{
                //   //发送邮件
                //  /MailHelper.SendMail("[同步部门人员服务]", string.Join("<br />", (string[])CarsSyncError.ToArray<string>()));
                //}
                Log.Info("[同步车型数据（车系、品牌）]任务轮训------------结束");
            }
            
        }
        /// <summary>
        /// 同步汽车品牌
        /// </summary>
        public void CarsBrandSyncData()
        {
            string Url = publicUrl + "carbrand/getlist";
            //查询出所有【行圆汽车公司】下所有的汽车品牌
            Log.Info("[同步汽车品牌]查询" + Url + "接口------------开始");
            DateTime dtStart = DateTime.Now;
            string result = InvokeWebService(Url, CarsSyncDataService.Enums.HttpType.GET, Encoding.UTF8);
            DateTime dtEnd = DateTime.Now;
            TimeSpan span = (TimeSpan)(dtEnd - dtStart);
            Log.Info("同步汽车品牌->" + Url + "—>用时：" + span.TotalMilliseconds + "毫秒");
            string str = "";
            try
            {
                CarBrandListModel resultModel = Json.JsonDeserializeBySingleDataNew<CarBrandListModel>(result);

                if (resultModel == null || resultModel.Result != "1")
                {

                    str = "[同步汽车品牌]查询" + Url + "接口出错 原因：" + resultModel == null ? "返回为null" : resultModel.Message;
                    CarsSyncError.Add(str);
                }
                else
                {
                    string Describe = CarInfoBll.Instance.InserCarBrandInfo(resultModel.Data.data);
                    Log.Info(Describe);
                    str = "[同步汽车品牌]查询" + Url + "接口------------完成";
                    if (resultModel.Data != null)
                    {
                        foreach (var item in resultModel.Data.data)
                        {
                            CarsSerailSyncData(item.BrandID);
                        }
                    }
                }
                Log.Info(str);

            }
            catch (Exception ex)
            {
                //记录日志
                Log.Info("同步汽车品牌]查询" + Url + "接口出错", ex);
                CarsSyncError.Add(str);
                return;
            }
        }
        /// <summary>
        /// 同步汽车车系
        /// </summary>
        /// <param name="CarBrandID">汽车品牌ID</param>
        public void CarsSerailSyncData(string CarBrandID)
        {
            string Url = publicUrl + "carserial/getlistbyid";
            string param = "?id=" + CarBrandID;
            Url = Url + param;
            //查询出所有【行圆汽车公司】下所有的汽车车系
            Log.Info("[同步汽车车系]查询" + Url + "接口------------开始");
            DateTime dtStart = DateTime.Now;
            string result = InvokeWebService(Url, CarsSyncDataService.Enums.HttpType.GET, Encoding.UTF8);
            DateTime dtEnd = DateTime.Now;
            TimeSpan span = (TimeSpan)(dtEnd - dtStart);
            Log.Info("同步汽车车系->" + Url + "—>用时：" + span.TotalMilliseconds + "毫秒");
            string str = "";
            try
            {
                CardSerialListModel resultModel = Json.JsonDeserializeBySingleDataNew<CardSerialListModel>(result);

                if (resultModel == null || resultModel.Result != "1")
                {

                    str = "[同步汽车车系]查询" + Url + "接口出错 原因：" + resultModel == null ? "返回为null" : resultModel.Message;
                    CarsSyncError.Add(str);
                }
                else
                {
                    string Describe = CarInfoBll.Instance.InserCarSerailInfo(resultModel.Data.data, CarBrandID);
                    Log.Info(Describe);
                    str = "[同步汽车车系]查询" + Url + "接口------------完成";
                }

                Log.Info(str);

            }
            catch (Exception ex)
            {
                //记录日志
                Log.Info("同步汽车车系]查询" + Url + "接口出错", ex);
                CarsSyncError.Add(str);
                return;
            }
        }
        /// <summary>
        /// 动态调用web服务 
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="parm">JSON</param>
        /// <param name="httpType">Enums.HttpType</param>
        /// <param name="encoding">Encoding</param>
        /// <returns></returns>
        public string InvokeWebService(string url, Enums.HttpType httpType, Encoding encoding, string parm = null)
        {

            string responseHTML = "";
            try
            {
                //string username = "admin";
                //string password = "nimda";
                //注意这里的格式，为 "username:password"
                // string usernamePassword = username + ":" + password;
                CredentialCache mycache = new CredentialCache();
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(url);
                myReq.Proxy = null;
                myReq.Timeout = 10000;//设置5秒响应
                myReq.Method = httpType.ToString();
                //mycache.Add(new Uri(url), "Basic", new NetworkCredential(username, password));
                // myReq.Credentials = mycache;
                // myReq.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));
                myReq.Headers.Add("Accept-Charset", "UTF-8");
                myReq.ContentType = "application/json";
                if (httpType.ToString() == "POST")
                {
                    byte[] replybyte = Encoding.UTF8.GetBytes(parm);
                    myReq.ContentLength = replybyte.Length;
                    Stream newStream = myReq.GetRequestStream();
                    newStream.Write(replybyte, 0, replybyte.Length);
                    newStream.Close();
                }
                HttpWebResponse AddVehiclResponse = (HttpWebResponse)myReq.GetResponse();

                Stream dataStream = AddVehiclResponse.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, encoding);
                responseHTML = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                AddVehiclResponse.Close();
            }
            catch (Exception ex)
            {
                Log.Info(url + "WebService访问出错", ex);
                responseHTML = ex.ToString();
            }
            return responseHTML;
        }
        /// <summary>
        /// HTTP请求方式枚举
        /// </summary>
        public class Enums
        {
            /// <summary>
            /// http请求类型
            /// </summary>
            public enum HttpType { GET, POST };
        }



    }
}