using System;
using Quartz;
using System.Configuration;
using System.Collections.Generic;
using XYAuto.BUOC.BOP2017.Infrastruction;
using System.Reflection;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.Web;

namespace XYAuto.BUOC.BOP2017.GdtDeriveUserInfo.QuartzJobs
{
    public sealed class PullDeriveUserInfoJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            #region 拉取DeriveUserInfo 并同步给智慧云
            try
            {
                DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:00"));
                int Id = BLL.GDT.GDTDeriveUserInfo.Instance.SelectMaxId();
                string url = string.Format(ConfigurationManager.AppSettings["DeriveUserInfo"] + "?page={0}&id={1}", 1, Id);
                string json = CommonHelper.HttpGet(url);
                if (json.Length>0 && !json.Contains("Query Data Empty"))
                {
                    PullJsonData item = Newtonsoft.Json.JsonConvert.DeserializeObject<PullJsonData>(json);
                    if (item != null && item.AllData.Count > 0)
                    {
                        InsertUserInfo(dt, item);
                    }
                    if (item.TotalPage > 1)
                    {
                        for (int i = 2; i < item.TotalPage; i++)
                        {
                            url = string.Format(ConfigurationManager.AppSettings["DeriveUserInfo"] + "?page={0}&id={1}", i, Id);
                            InsertUserInfo(dt, Newtonsoft.Json.JsonConvert.DeserializeObject<PullJsonData>(CommonHelper.HttpGet(url)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("PullDeriveUserInfoJob 错误" + ex.ToString());
            }
            #endregion
        }

        /// <summary>
        /// DeriveUserInfo 入库
        /// </summary>
        /// <param name="dt">拉取时间</param>
        /// <param name="item">拉取地信息</param>
        private static void InsertUserInfo(DateTime dt, PullJsonData item)
        {
            foreach (var entity in item.AllData)
            {
                try
                {
                    Entities.GDT.GDTDeriveUserInfo model = new Entities.GDT.GDTDeriveUserInfo();
                    model.Brand = entity.Brand;
                    model.CarModel = entity.CarModel;
                    model.CarType = entity.CarType;
                    model.City = entity.City;
                    model.Dealer = entity.Dealer;
                    model.DeviceNumber = entity.device_number;
                    model.ID = entity.ID;
                    model.Name = entity.Name;
                    model.OnLikeType = entity.onlike_type;
                    model.Phone = entity.Phone;
                    model.Province = entity.Province;
                    model.SourceUrl = entity.Source_url;
                    model.Time = entity.Time;
                    model.VisitIP = entity.Visit_ip;
                    model.CreateTime = dt;
                    int id = BLL.GDT.GDTDeriveUserInfo.Instance.Insert(model);
                    model.DeriveUserInfoId = id;

                    #region 线索接口 提交智慧云
                    try
                    {
                        int DemandBillNo = BLL.Demand.Demand.Instance.GetDemandBillNoByPromotionUrlCode(GetLastStr(model.SourceUrl, 10));
                        if (DemandBillNo > 0)
                        {
                            Dictionary<string, object> dict = new Dictionary<string, object>();
                            dict.Add("UserName", model.Name);
                            dict.Add("Mobile", model.Phone);
                            dict.Add("BrandID", model.Brand);
                            dict.Add("SerialID", model.CarModel);
                            dict.Add("OrderTime", model.Time);
                            dict.Add("IP", model.VisitIP);
                            dict.Add("DeciveID", model.DeviceNumber);
                            dict.Add("DealerID", model.Dealer);
                            dict.Add("CarId", model.CarType);
                            dict.Add("CityID", model.City);
                            //dict.Add("ClueType", (int)(ClueType)Enum.Parse(typeof(ClueType), model.OnLikeType));
                            dict.Add("ClueType", GetClueType(model.OnLikeType));
                            dict.Add("OrganizeAdsID", DemandBillNo);
                            dict.Add("Source", 1);
                            dict.Add("PutSource", "");
                            dict.Add("SourceId", model.ID);

                            if (IsValid(model) && GetClueType(model.OnLikeType) > 0)
                            {
                                string timestamp = DateTime.Now.ToString("yyyyMMdd");
                                string json = ForeachClassProperties<Entities.GDT.GDTDeriveUserInfo>(model, DemandBillNo.ToString(), timestamp);
                                string signature = HttpUtility.UrlEncode(CommonHelper.SHA1(json));
                                string postdata = JsonConvert.SerializeObject(dict);
                                string result = PushClueData(postdata, signature, timestamp);
                                PullResult pres = JsonConvert.DeserializeObject<PullResult>(result);
                                if (pres.Success.ToLower() == "true")
                                {
                                    //DeriveUserInfo Status 状态  NULL:接受  1：同步智慧云成功
                                    BLL.GDT.GDTDeriveUserInfo.Instance.UpdaetDeriveUserInfoByKeyId(model.DeriveUserInfoId, 1);
                                }
                                else
                                {
                                    Loger.Log4Net.Info("落地页：" + model.SourceUrl + " ClueID：" + model.ID + "  Url："+ string.Format(ConfigurationManager.AppSettings["ClueAddress"], signature, timestamp) + " PostData："+ postdata + "String："+ json + " Result：" + result);
                                }
                            }
                            else
                            {
                                Loger.Log4Net.Info("落地页：" + model.SourceUrl + " 字段不满足智慧云条件");
                            }
                        }
                        else
                        {
                            Loger.Log4Net.Info("落地页：" + model.SourceUrl + " 找不到需求ID");
                        }
                    }
                    catch (Exception ex)
                    {
                        Loger.Log4Net.Info("线索接口错误 ID：" + model.ID + " ErrorMessage ：" + ex.ToString());
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    Loger.Log4Net.Error("DeriveUserInfo 入库错误" + ex.ToString());
                    continue;
                }
            }
        }


        /// <summary>
        /// 推送线索
        /// </summary>
        private static string PushClueData(string postdata,string signature,string timestamp)
        {
            string url = string.Format(ConfigurationManager.AppSettings["ClueAddress"], signature, timestamp);
            string result = CommonHelper.PostHttp(url, postdata, "application/json");
            return result;
        }

      

        /// <summary>
        /// 反射遍历对象属性
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="model">对象</param>
        private static string ForeachClassProperties<T>(T model, string DemandBillNo, string timestamp)
        {
            System.Text.StringBuilder sbpostdata = new System.Text.StringBuilder();
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("appkey", ConfigurationManager.AppSettings["AppKey"]);
            result.Add("appsecret", ConfigurationManager.AppSettings["AppSecret"]);
            result.Add("timestamp", timestamp);
            result.Add("putsource", "");//投放源（预留）
            result.Add("source", "1");//广点通来源默认为1
            Type t = model.GetType();
            PropertyInfo[] PropertyList = t.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                if (item.Name.ToLower() == "onliketype")
                {
                    int id = GetClueType(item.GetValue(model).ToString()); //(int)(ClueType)Enum.Parse(typeof(ClueType), item.GetValue(model).ToString());
                    result.Add(TransformDictName(item.Name.ToLower()), id.ToString());
                }
                else if (item.Name.ToLower() == "sourceurl")
                {
                    result.Add(TransformDictName(item.Name.ToLower()), DemandBillNo);
                }
                else if (item.Name.ToLower() == "createtime" || item.Name.ToLower() == "deriveuserinfoid" || item.Name.ToLower() == "province" || item.Name.ToLower() == "status")
                {
                    continue;
                }
                else
                {
                    result.Add(TransformDictName(item.Name.ToLower()), item.GetValue(model).ToString());
                }
            }
            Dictionary<string, string> dicByKey = result.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
            foreach (var item in dicByKey)
            {
                if (item.Value.Length == 0)
                {
                    sbpostdata.Append("");
                }
                else
                {
                    if (item.Key.ToLower() == "ordertime")
                    {
                        sbpostdata.Append(Convert.ToDateTime(item.Value).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        sbpostdata.Append(item.Value);
                    }
                }
                   
            }
            return sbpostdata.ToString().Replace("\r\n","");
        }

        private static bool IsValid(Entities.GDT.GDTDeriveUserInfo model)
        {
            int error = 0;

            #region 数据验证
            if (model.Name.Length == 0)
            {
                error++;
            }
            if (model.Phone.Length == 0)
            {
                error++;
            }
            if (model.Brand == 0)
            {
                error++;
            }
            if (model.CarModel == 0)
            {
                error++;
            }
            if (model.CarType == 0)
            {
                error++;
            }
            if (model.Time.Length == 0)
            {
                error++;
            }
            if (model.VisitIP.Length == 0)
            {
                error++;
            }
            if (model.OnLikeType.Length == 0)
            {
                error++;
            }
            if (model.ID == 0)
            {
                error++;
            }
            #endregion
            if (error > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 名称转换
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string TransformDictName(string name)
        {
            string result = name;
            if (name == "id")
            {
                result = "sourceid";
            }
            if (name == "name")
            {
                result = "username";
            }
            if (name == "phone")
            {
                result = "mobile";
            }
            if (name == "brand")
            {
                result = "brandid";
            }
            if (name == "carmodel")
            {
                result = "serialid";
            }
            if (name == "cartype")
            {
                result = "carid";
            }
            if (name == "city")
            {
                result = "cityid";
            }
            if (name == "time")
            {
                result = "ordertime";
            }
            if (name == "visitip")
            {
                result = "ip";
            }
            if (name == "dealer")
            {
                result = "dealerid";
            }
            if (name == "devicenumber")
            {
                result = "deciveid";
            }
            if (name == "onliketype")
            {
                result = "cluetype";
            }
            if (name == "sourceurl")
            {
                result = "organizeadsid";
            }
            return result;
        }

        /// <summary>
        /// 获取字符串后十位
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private static string GetLastStr(string str, int num)
        {
            int count = 0;
            if (str.Length > num)
            {
                count = str.Length - num;
                str = str.Substring(count, num);
            }
            return str;
        }


        private static int GetClueType(string name)
        {
            if (name == "新车")
            {
                return 1;
            }
            if (name == "试驾")
            {
                return 2;
            }
            if (name == "置换")
            {
                return 3;
            }
            return 0;
        }
    }

    public enum ClueType
    {
        新车 = 1,
        试驾,
        置换,
    }

    public class PullJsonData
    {
        public int TotalStrip { get; set; }

        public int TotalPage { get; set; }

        public List<AllData> AllData { get; set; }

        public string code { get; set; }

        public string msg { get; set; }
    }

    public class AllData
    {
      public int DeriveUserInfoId { get; set; }
      public int ID { get; set; }
      public string Name { get; set; }
      public string Phone { get; set; }
      public int Brand { get; set; }
      public int Province { get; set; }
      public int City { get; set; }
      public int Dealer { get; set; }
      public int CarType { get; set; }
      public int CarModel { get; set; }
      public string onlike_type { get; set; }
      public string device_number { get; set; }
      public string Time { get; set; }
      public string Visit_ip { get; set; }
      public string Source_url { get; set; }
      public DateTime CreateTime { get; set; }
    }

    public class PullResult
    {
        public string Success { get; set; }
        public string Data { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
