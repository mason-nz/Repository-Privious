using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.DSC.IM_2015.BLL;
using BitAuto.Utils.Config;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Core
{
    public class EPDMSMember
    {
        /*
        /// 通过链接获取会员信息
        /// <summary>
        /// 通过链接获取会员信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static CometClient GetDMSMemberByUrl(string title, Uri url, Uri posturl, string para)
        {
            try
            {
                //域名验证
                if (!CheckDomainName(url))
                {
                    return null;
                }

                string key = ConfigurationUtil.GetAppSettingValue("EPDESEncryptorKey");
                EP_DESEncryptor code = new EP_DESEncryptor(key);
                string eWord = code.DesDecrypt(para);
                EPInfoEntity model = (EPInfoEntity)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(eWord, typeof(EPInfoEntity));
                if (model.IsValid)
                {
                    DataTable dt = BLL.BaseData.Instance.GetDMSMemberByCode(model.DealerId);
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        //访问记录入库
                        Entities.EPVisitLog info = InsertEPVisitLog(title, posturl, model);
                        //客户入库
                        InsertCustomerInfo(info.MemberCode);
                        //返回实体
                        CometClient client = GetCometClient(dt, info);
                        return client;
                    }
                    else
                    {
                        Loger.Log4Net.Error("URL非法：获取不到经销商信息 json:" + eWord);
                        return null;
                    }
                }
                else
                {
                    Loger.Log4Net.Error("URL非法：时间超过30分钟 json:" + eWord);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("GetDMSMemberByUrl异常：" + url.ToString(), ex);
                return null;
            }
        }


        /// 插入一条访问记录
        /// <summary>
        /// 插入一条访问记录
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private static Entities.EPVisitLog InsertEPVisitLog(string title, Uri url, EPInfoEntity model)
        {
            Entities.EPVisitLog info = new Entities.EPVisitLog();
            info.VisitID = model.UserId + "-" + model.DateTimeFormat;
            info.LoginID = model.UserId;
            info.MemberCode = model.DealerId;
            info.VisitRefer = model.AppId;
            info.UserReferTitle = title;
            if (!string.IsNullOrEmpty(url.Query))
            {
                info.UserReferURL = url.AbsoluteUri.Replace(url.Query, "");
            }
            else
            {
                info.UserReferURL = url.AbsoluteUri;
            }
            info.ContractName = model.UserName;
            info.ContractJob = model.Post;
            info.ContractPhone = model.Mobile;
            info.ContractEmail = model.Email;
            info.CreateTime = DateTime.Now;

            if (BLL.EPVisitLog.Instance.GetComAdoInfo<Entities.EPVisitLog>(info.VisitID) == null)
            {
                BLL.EPVisitLog.Instance.InsertComAdoInfo(info);
            }
            return info;
        }
        /// 构建CometClient
        /// <summary>
        /// 构建CometClient
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static CometClient GetCometClient(DataTable dt, Entities.EPVisitLog info)
        {
            //构建实体类
            CometClient client = new CometClient();
            client.VisitID = info.ValueOrDefault_VisitID;
            client.LoginID = info.ValueOrDefault_LoginID;
            client.MemberCode = info.ValueOrDefault_MemberCode;
            client.MemberName = dt.Rows[0]["MemberName"].ToString();
            client.UserReferTitle = info.ValueOrDefault_UserReferTitle;
            client.UserReferUrl = info.ValueOrDefault_UserReferURL;
            client.VisitRefer = info.ValueOrDefault_VisitRefer;
            client.ContractName = info.ValueOrDefault_ContractName;
            client.ContractJob = info.ValueOrDefault_ContractJob;
            client.ContractPhone = info.ValueOrDefault_ContractPhone;
            client.ContractEmail = info.ValueOrDefault_ContractEmail;

            client.ProvinceID = CommonFunc.ObjectToInteger(dt.Rows[0]["ProvinceID"]);
            client.CityID = CommonFunc.ObjectToInteger(dt.Rows[0]["CityID"]);
            client.CountyID = CommonFunc.ObjectToInteger(dt.Rows[0]["CountyID"]);

            client.ProvinceName = CommonFunc.ObjectToString(dt.Rows[0]["ProvinceName"]);
            client.CityName = CommonFunc.ObjectToString(dt.Rows[0]["CityName"]);
            client.CountyName = CommonFunc.ObjectToString(dt.Rows[0]["CountyName"]);

            client.CityGroupId = CommonFunc.ObjectToString(dt.Rows[0]["CityGroup"]);
            client.CityGroupName = CommonFunc.ObjectToString(dt.Rows[0]["CityGroupName"]);

            client.DistrictId = CommonFunc.ObjectToString(dt.Rows[0]["District"]);
            client.DistrictName = CommonFunc.ObjectToString(dt.Rows[0]["DistrictName"]);

            client.LastMessageTime = CommonFunc.ObjectToDateTime(dt.Rows[0]["LastMessageTime"], new DateTime(1900, 01, 01));
            client.LastConBeginTime = CommonFunc.ObjectToDateTime(dt.Rows[0]["LastBeginTime"], new DateTime(1900, 01, 01));

            client.Distribution = CommonFunc.ObjectToInteger(dt.Rows[0]["Distribution"]);

            return client;
        }
        /// 验证域名
        /// <summary>
        /// 验证域名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static bool CheckDomainName(Uri url)
        {
            string DomainName = ConfigurationUtil.GetAppSettingValue("DomainName");
            //Loger.Log4Net.Info("合法域名：" + DomainName);
            string[] array = DomainName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string key in array)
            {
                if (url.Host.ToLower().EndsWith(key.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
        /// 客户表新增
        /// <summary>
        /// 客户表新增
        /// </summary>
        /// <param name="membercode"></param>
        private static void InsertCustomerInfo(string membercode)
        {
            Entities.CustomerInfo info = BLL.CustomerInfo.Instance.GetCustomerInfoByMemberCode(membercode);
            if (info == null)
            {
                //新增
                info = new Entities.CustomerInfo();
                info.MemberCode = membercode;
                info.Status = 0;
                info.Distribution = 0;
                info.CreateTime = DateTime.Now;
                BLL.CustomerInfo.Instance.InsertComAdoInfo(info);
            }
        }

        /// [测试方法] 获取一个测试用的会员信息
        /// <summary>
        /// [测试方法] 获取一个测试用的会员信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="mobile"></param>
        /// <param name="email"></param>
        /// <param name="post"></param>
        /// <param name="dealerid"></param>
        /// <returns></returns>
        public static string GetTestDMSMemberUrl(int userid, string username, string mobile, string email, string post, string dealerid = "50002218")
        {
            EPInfoEntity model = new EPInfoEntity();
            model.DateTimeFormat = DateTime.Now.ToString("yyyyMMddHHmmss");
            model.UserId = userid;
            model.UserName = username;
            model.Mobile = mobile;
            model.Email = email;
            model.Post = post;
            model.DealerId = dealerid;
            model.AppId = Guid.NewGuid().ToString();

            string key = ConfigurationUtil.GetAppSettingValue("EPDESEncryptorKey");
            EP_DESEncryptor code = new EP_DESEncryptor(key);
            return code.DesEncrypt(Newtonsoft.Json.JavaScriptConvert.SerializeObject(model));
        }
        */
    }
}
