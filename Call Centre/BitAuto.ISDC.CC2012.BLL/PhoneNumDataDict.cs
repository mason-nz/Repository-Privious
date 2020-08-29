using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Web.Script.Serialization;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class PhoneNumDataDict
    {
        /// <summary>
        /// 根据电话号码获取地区ID
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="ProvinceID"></param>
        /// <param name="CityID"></param>
        /// <returns></returns>
        public static void GetAreaId(string Number, out int ProvinceID, out int CityID)
        {
            string msg = "";
            ProvinceID = 0;
            CityID = 0;
            bool IsMobileNum = false;//是否是手机号
            bool IsTelNum = false;//是否是座机号

            ///前缀或者区号
            string areNo = "";

            #region 判断格式

            IsMobileNum = BLL.Util.IsHandset(Number);
            IsTelNum = BLL.Util.IsTelNumber(Number);

            if (!IsMobileNum && !IsTelNum)
            {
                msg = "电话号码格式不正确";
                return;
            }

            #endregion

            #region 提取区号或者手机前缀
            if (IsMobileNum)
            {
                //是手机号
                areNo = Number.Substring(0, 7);
            }
            else
            {
                //是座机号

                string proStr = Number.Substring(0, 2);
                if (proStr == "01" || proStr == "02")
                {
                    areNo = Number.Substring(0, 3);
                }
                else
                {
                    areNo = Number.Substring(0, 4);
                }
            }

            #endregion

            #region 查询电话号码归属地
            Dal.PhoneNumDataDict.GetAreaID(areNo, out ProvinceID, out CityID, out msg);
            #endregion

            #region 如果在数据库里没有查到，就去网上查询
            bool flag = false;
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IsCallThridPhoneAreaInterface"], out flag);
            if (flag)
            {
                if (ProvinceID == 0 && CityID == 0)//如果没有在数据库中找到
                {
                    #region 如果没有在数据库中找到，去网站查询电话归属地，插入自己的数据库
                    //从www.023001.com网站查询
                    // GetInfoFromWeb023001(ref ProvinceID, ref CityID, areNo);

                    //从百度所用的www.showji.com网站查询
                    //GetInfoFromWebShouji(ref ProvinceID, ref CityID, areNo);

                    //从百度所用的sj.kvgo.net网站查询
                    GetInfoFromWebKvgo(ref ProvinceID, ref CityID, areNo);
                    #endregion
                }

                //再次在库中查询归属地
                Dal.PhoneNumDataDict.GetAreaID(areNo, out ProvinceID, out CityID, out msg);
            }
            #endregion
        }

        public static bool VerifyFormat(string Number, int LocalProvinceID, List<int> LocalCityID, out string outNumber, out string errorMsg)
        {
            //增加400电话验证
            outNumber = "";
            int ProvinceID = 0;
            int CityID = 0;
            bool IsMobileNum = false;//是否是手机号
            bool IsTelNum = false;//是否是座机号
            bool Is400Tel = false;//是否400电话

            ///前缀或者区号
            string areNo = "";

            #region 判断格式

            IsMobileNum = BLL.Util.IsHandset(Number);
            IsTelNum = BLL.Util.IsTelNumber(Number);
            Is400Tel = BLL.Util.Is400Tel(Number);

            if (!IsMobileNum && !IsTelNum && !Is400Tel)
            {
                errorMsg = "电话号码格式不正确"; return false;
            }

            if (Is400Tel)//如果是400电话 不查询归属地
            {
                errorMsg = "";
                outNumber = Number;
                return true;
            }

            #endregion

            #region 提取区号或者手机前缀
            if (IsMobileNum)
            {
                //是手机号
                areNo = Number.Substring(0, 7);
            }
            else
            {
                //是座机号
                string proStr = Number.Substring(0, 2);
                if (proStr == "01" || proStr == "02")
                {
                    areNo = Number.Substring(0, 3);
                }
                else
                {
                    areNo = Number.Substring(0, 4);
                }
            }
            #endregion

            #region 查询电话号码归属地
            Dal.PhoneNumDataDict.GetAreaID(areNo, out ProvinceID, out CityID, out errorMsg);
            #endregion

            #region 如果在数据库里没有查到，就去网上查询
            if (ProvinceID == 0 && CityID == 0)//如果没有在数据库中找到
            {
                #region 如果没有在数据库中找到，去网站查询电话归属地
                //从www.023001.com网站查询
                // GetInfoFromWeb023001(ref ProvinceID, ref CityID, areNo);

                //从百度所用的www.showji.com网站查询
                //GetInfoFromWebShouji(ref ProvinceID, ref CityID, areNo);
                bool flag = false;
                bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IsCallThridPhoneAreaInterface"], out flag);
                if (flag)
                {
                    //从百度所用的sj.kvgo.net网站查询
                    GetInfoFromWebKvgo(ref ProvinceID, ref CityID, areNo);
                }
                #endregion
            }
            //本地省和城市
            if (ProvinceID == LocalProvinceID && (LocalCityID == null || LocalCityID.Contains(CityID)))
            {
                if (IsTelNum)
                {
                    outNumber = Number.Substring(3);
                }
                else if (IsMobileNum)
                {
                    outNumber = Number;
                }
            }
            else//外地号码
            {
                if (IsTelNum)
                {
                    outNumber = Number;
                }
                else if (IsMobileNum)
                {
                    outNumber = "0" + Number;
                }
            }
            #endregion
            return true;
        }

        /// <summary>
        /// 验证电话号码
        /// </summary>
        /// <param name="Number">原始输入的电话号码</param>
        /// <param name="outNumber">输出电话号码</param>
        /// <param name="errorMsg">若有错误，输出错误信息</param>
        /// <returns>验证成功，返回True，否则返回False</returns>
        public static bool VerifyFormatBeiJin(string Number, out string outNumber, out string errorMsg)
        {
            //if (ProvinceID == 2)//北京，本地
            return VerifyFormat(Number, 2, null, out outNumber, out errorMsg);
        }
        public static bool VerifyFormatXiAn(string Number, out string outNumber, out string errorMsg)
        {
            //if (ProvinceID == 23 && (CityID == 2301 || CityID == 2302))//西安，咸阳也算作本地
            return VerifyFormat(Number, 23, new List<int>() { 2301, 2302 }, out outNumber, out errorMsg);
        }

        private static void GetInfoFromWebKvgo(ref int ProvinceID, ref int CityID, string areNo)
        {
            string str = "";
            try
            {
                str = GetAndPostWeb.PostDataToUrl("type=json&number=" + areNo, "http://sj.kvgo.net");
                //str = GetAndPostWeb.GetDataToUrl("type=json&number=" + areNo, "http://sj.kvgo.net");
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                var mm = serializer.Deserialize<MobileModel>(str);
                if (string.IsNullOrEmpty(mm.city) || mm.city == "未知")
                {
                    string errStr = "在http://sj.kvgo.net查询【" + areNo + "】时,没有查询到归属地";

                    //BLL.Util.SendErrorEmail(errStr, @"\BitAuto.ISDC.CC2012.BLL\PhoneNumDataDict.cs", @"\BitAuto.ISDC.CC2012.BLL\PhoneNumDataDict.cs");
                }
                else
                {
                    //插入到自己的数据库
                    AddToMyDb(mm.province + "-" + mm.city, mm.mobile, mm.area_code, ref ProvinceID, ref CityID);
                }
            }
            catch (Exception ex)
            {
                string errStr = "在http://sj.kvgo.net查询【" + areNo + "】时出错:" + ex.Message.ToString();
                Loger.Log4Net.Error(errStr, ex);
                ProvinceID = CityID = 0;
                //BLL.Util.SendErrorEmail(errStr, ex.Source, ex.StackTrace);
                //BLL.Util.SendEmailAsync(errStr, ex.Source, ex.StackTrace);

                //实例委托
                //AsyncEventHandler asy = new AsyncEventHandler(BLL.Util.SendErrorEmail);

                ////异步调用开始
                //IAsyncResult ia = asy.BeginInvoke(errStr, ex.Source, ex.StackTrace, null, null);
                ////异步结束
                //asy.EndInvoke(ia);
            }
        }

        /// <summary>
        /// 从http://www.023001.com网站获取数据
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="CityID"></param>
        /// <param name="areNo"></param>
        private static void GetInfoFromWeb023001(ref int ProvinceID, ref int CityID, string areNo)
        {
            #region 如果没有在数据库中找到，去网站查询电话归属地

            string html = "";

            try
            {
                html = GetAndPostWeb.HttpPost("http://www.023001.com/inc/phone.asp", "m=" + areNo, "gb2312");

                //替换掉没用的字符
                html = html.Replace("</br>", "").Replace("\r\n", "").Replace("\"", "\'").Replace(" width=460 border='1' align='center' cellpadding='4' bordercolor=#1488bb style='border-collapse: collapse'", "").ToUpper();

                //将返回的html以XML格式放入XmlDocument
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(html);


                string Location = doc.SelectNodes("//TABLE//TR")[1].SelectNodes("TD")[1].InnerText;//归属地
                if (Location != "未知-未知")
                {
                    //如果找到了归属地

                    string MoileNum = doc.SelectNodes("//TABLE//TR")[0].SelectNodes("TD")[1].InnerText;//手机号
                    string CardType = doc.SelectNodes("//TABLE//TR")[2].SelectNodes("TD")[1].InnerText;//手机号类型
                    string AreaCode = doc.SelectNodes("//TABLE//TR")[3].SelectNodes("TD")[1].InnerText;//座机区号
                    string ZipCode = doc.SelectNodes("//TABLE//TR")[4].SelectNodes("TD")[1].InnerText;//邮政编码

                    AddToMyDb(Location, MoileNum, AreaCode);
                }
            }
            catch (Exception ex)
            {
                ProvinceID = 0;
                CityID = 0;
            }
            #endregion
        }
        /// <summary>
        /// 将从网上查询到得手机归属地信息插入数据库
        /// </summary>
        /// <param name="Location">网上查询到得手机归属地</param>
        /// <param name="MoileNum">网上查询到得手机号</param>
        /// <param name="PhoneNum">网上查询到得座机区号</param>
        private static void AddToMyDb(string Location, string MoileNum, string PhoneNum)
        {
            #region 插入数据库中


            Entities.PhoneNumDataDict model = new Entities.PhoneNumDataDict();
            model.DistrictNum = PhoneNum;
            model.PhonePrefix = MoileNum;
            model.Status = 0;
            model.CreateTime = DateTime.Now;

            if (Location.Split('-').Length > 0)
            {
                //BLL.PhoneNumDataDict.GetAreaId
                int AreaID = 0;
                int AreaLevel = 0;
                int AreaPid = 0;
                int ProviceID = 0;

                Dal.PhoneNumDataDict.GetAreaInfoByName(Location.Split('-')[1].Trim(), out AreaID, out AreaLevel, out AreaPid, out ProviceID);

                if (AreaID != 0)
                {
                    model.AreaID = AreaID;
                    model.AreaLevel = AreaLevel;
                    model.AreaName = Location.Split('-')[1].Trim();
                    model.AreaPid = AreaPid;
                    model.ProviceID = ProviceID;

                    int retID = BLL.PhoneNumDataDict.Add(model);

                }
            }

            #endregion
        }
        /// <summary>
        /// 入库且返回网上查到的省份ID，城市ID
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="MoileNum"></param>
        /// <param name="PhoneNum"></param>
        /// <param name="ProvinceID"></param>
        /// <param name="CityID"></param>
        private static void AddToMyDb(string Location, string MoileNum, string PhoneNum, ref int ProviceID, ref int CityID)
        {
            #region 插入数据库中


            Entities.PhoneNumDataDict model = new Entities.PhoneNumDataDict();
            model.DistrictNum = PhoneNum;
            model.PhonePrefix = MoileNum;
            model.Status = 0;
            model.CreateTime = DateTime.Now;

            if (Location.Split('-').Length > 0)
            {
                //BLL.PhoneNumDataDict.GetAreaId
                int AreaID = 0; int AreaPid = 0;
                int AreaLevel = 0;
                CityID = 0;
                ProviceID = 0;

                Dal.PhoneNumDataDict.GetAreaInfoByName(Location.Split('-')[1].Trim(), out AreaID, out AreaLevel, out AreaPid, out ProviceID);

                if (AreaID != 0)
                {
                    model.AreaID = AreaID;
                    model.AreaLevel = AreaLevel;
                    model.AreaName = Location.Split('-')[1].Trim();
                    model.AreaPid = AreaPid;
                    model.ProviceID = ProviceID;
                    CityID = AreaID;
                    int retID = BLL.PhoneNumDataDict.Add(model);

                }
            }

            #endregion
        }
        private static int Add(Entities.PhoneNumDataDict model)
        {
            return Dal.PhoneNumDataDict.Add(model);
        }
        /// <summary>
        /// 判断电话号码的归属地是否是北京
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public static bool IsLocalityNumber(string Number)
        {
            bool isLocal = false;

            int ProvinceID = 0;
            int CityID = 0;
            GetAreaId(Number, out ProvinceID, out CityID);
            if (ProvinceID == 2)
            {
                //如果是北京
                isLocal = true;
            }
            return isLocal;
        }
    }

    public class MobileModel
    {
        public string mobile
        { get; set; }
        public string province
        { get; set; }
        public string city
        { get; set; }
        public string post_code
        { get; set; }
        public string area_code
        { get; set; }
        public string card
        { get; set; }
        public string number
        { get; set; }
        public string error
        { get; set; }
    }
}
