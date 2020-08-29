using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.WebService.BAA.QiCheTongService;
using BitAuto.ISDC.CC2012.WebService.BAA.QiCheTongQueryService;

namespace BitAuto.ISDC.CC2012.WebService
{
    public class QiCheTongHelper
    {
        QiCheTongProxy proxy = new QiCheTongProxy();
        QiCheTongQueryProxy proxyQuery = new QiCheTongQueryProxy();

        #region Instance
        public static readonly QiCheTongHelper Instance = new QiCheTongHelper();
        //private string QiCheTongServiceURL = System.Configuration.ConfigurationManager.AppSettings["QiCheTongServiceURL"];//汽车通手机号注册接口
        //private string QiCheTongQueryServiceURL = System.Configuration.ConfigurationManager.AppSettings["QiCheTongQueryServiceURL"];//汽车通根据手机号查询接口
        #endregion

        //#region Contructor
        //protected QiCheTongHelper()
        //{ }
        //#endregion

        /// <summary>
        /// 呼叫中心添加电话注册用户，（成功:Success= 0|失败:Failed = 8|用户名已存在:DuplicateUserName = 6|手机已经存在:DuplicateMobile = 9|手机号无效:InvalidMobile = 10）
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns></returns>
        public RegisterMobileUserInfo RegisterMobileUser(string mobile)
        {
            //return WebServiceHelper.InvokeWebService(QiCheTongServiceURL, "RegisterMobileUser", new object[] { mobile });
            return proxy.RegisterMobileUser(mobile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public UserInfoDto[] GetUserInfoByMobile(string mobile)
        {
            //return WebServiceHelper.InvokeWebService(QiCheTongQueryServiceURL, "GetUserInfoByMobile", new object[] { mobile });
            return proxyQuery.GetUserInfoByMobile(mobile);
        }

        /// <summary>
        /// 根据电话号码，返回汽车通用户ID，若能查到多个用户，只取第一个用户的ID
        /// </summary>
        /// <param name="mobile">电话号码</param>
        /// <returns>能找到为正整数，找不到为-1，异常为-2</returns>
        public int GetUserIDByMobile(string mobile)
        {
            int userid = -1;
            //object obj = WebServiceHelper.InvokeWebService(QiCheTongQueryServiceURL, "GetUserInfoByMobile", new object[] { mobile });
            UserInfoDto[] userArray = proxyQuery.GetUserInfoByMobile(mobile);
            try
            {
                //if (((object[])obj).LongLength == 0)
                //{
                //    return -1;
                //}                
                //object objUserInfo = ((object[])obj)[0];
                //int.TryParse(objUserInfo.GetType().GetField("Id").GetValue(objUserInfo).ToString(), out userid);
                if (userArray.Length==0)
                {
                    return -1;
                }
                userid = userArray[0].Id;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("调用接口报错，方法：GetUserInfoByMobile，URL："+proxyQuery.Url, ex);
                return -2;
            }
            return userid;
        }
    }


    //public class RegisterMobileUserInfo
    //{
    //    //用户ID
    //    public int UserId { get; set; }
    //    //手机号
    //    public string Mobile { get; set; }
    //    //密码
    //    public string PassWord { get; set; }
    //    //结果
    //    public EnumUserCreateStatus RegisterResult { get; set; }
    //}
    //public enum EnumUserCreateStatus
    //{
    //    //成功
    //    Success = 0,
    //    //用户名无效
    //    InvalidUserName = 1,
    //    //密码无效
    //    InvalidPassword = 2,
    //    //密码问题无效
    //    InvalidQuestion = 3,
    //    //密码问题答案无效
    //    InvalidAnswer = 4,
    //    //电子邮件无效
    //    InvalidEmail = 5,
    //    //用户名已存在
    //    DuplicateUserName = 6,
    //    //Email已存在
    //    DuplicateEmail = 7,
    //    //失败
    //    Failed = 8,
    //    //手机已经存在
    //    DuplicateMobile = 9,
    //    //手机号无效
    //    InvalidMobile = 10,
    //}

    class QiCheTongProxy : BAA.QiCheTongService.AuthenWebService
    {
        public QiCheTongProxy()
        {
            string strTimeout = System.Configuration.ConfigurationManager.AppSettings["QiCheTongServiceTimeout"];
            int timeout = 0;
            this.Timeout = 1000 * (int.TryParse(strTimeout, out timeout) ? timeout : WebServiceHelper.WebServiceTimeout);

            string url = System.Configuration.ConfigurationManager.AppSettings["QiCheTongServiceURL"];
            if (string.IsNullOrEmpty(url) == false)
            {
                this.Url = url;
            }
        }
    }

    class QiCheTongQueryProxy : BAA.QiCheTongQueryService.UserManagerWebService
    {
        public QiCheTongQueryProxy()
        {
            string strTimeout = System.Configuration.ConfigurationManager.AppSettings["QiCheTongQueryServiceTimeout"];
            int timeout = 0;
            this.Timeout = 1000 * (int.TryParse(strTimeout, out timeout) ? timeout : WebServiceHelper.WebServiceTimeout);

            string url = System.Configuration.ConfigurationManager.AppSettings["QiCheTongQueryServiceURL"];
            if (string.IsNullOrEmpty(url) == false)
            {
                this.Url = url;
            }
        }
    }
}
