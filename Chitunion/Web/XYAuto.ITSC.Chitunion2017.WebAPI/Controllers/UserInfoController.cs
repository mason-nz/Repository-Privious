using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    public class UserInfoController : ApiController
    {
        int currentUserID = Chitunion2017.Common.UserInfo.GetLoginUserID();
        private string LoginPwdKey = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");

        #region 根据个人姓名、公司名称或手机号模糊查询广告主
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetUserInfoByUserID(string userid)
        {

            BLL.Loger.Log4Net.Info("[UserInfoController]******GetUserInfoByUserID begin...userid->" + userid + "******");
            try
            {
                //解码
                userid = System.Web.HttpUtility.UrlDecode(userid, Encoding.UTF8);
                //解密
                int custid = Convert.ToInt32(XYAuto.Utils.Security.DESEncryptor.Decrypt(userid, LoginPwdKey));
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic = BLL.UserDetailInfo.Instance.GetUserInfoByUserID(custid);

                BLL.Loger.Log4Net.Info("[UserInfoController]******GetUserInfoByUserID end...userid->" + userid + "******");
                return WebAPI.Common.Util.GetJsonDataByResult(dic, "操作成功", 0);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[UserInfoController]******GetUserInfoByUserID end...userid->" + userid + ",errorMsg:" + ex.Message);
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion

        #region 根据条件查询广告主列表（zlb 2017-07-21）
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult SelectAdveristerList(int UserSource, int Status, int UserType, int ProvinceID, int CityID, int PageIndex, int PageSize, string UserName = "", string Mobile = "", string TrueName = "", string StarDate = "", string EndDate = "")
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.Advertiser.Advertiser.Instance.SelectAdveristerList(UserName, Mobile, TrueName, UserSource, Status, UserType, ProvinceID, CityID, StarDate, EndDate, PageIndex, PageSize);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[UserInfoController]*****SelectAdveristerList ->UserName:" + UserName + " ->Mobile:" + Mobile + " ->TrueName:" + TrueName + ",查询广告主列表出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        #endregion
        /// <summary>
        /// zlb 2017-07-21
        /// 批量启用禁用用户
        /// </summary>
        /// <param name="req">用户ID信息</param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult UpdateUserStatusInfo([FromBody]UpdateUserStatusResDTO req)
        {
            string strJson = Json.JsonSerializerBySingleData(req);
            BLL.Loger.Log4Net.Info("[UserInfoController]******UpdateUserStatusInfo begin...->UpdateUserStatusResDTO:" + strJson + "");
            string messageStr = "";
            try
            {
                messageStr = BLL.Advertiser.Advertiser.Instance.UpdateUserStatusInfo(req);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[UserInfoController]*****UpdateUserStatusInfo ->UpdateUserStatusResDTO:" + strJson + ",批量启用禁用用户失败:" + ex.Message);
                throw ex;
            }
            Common.JsonResult jr;
            if (messageStr != "")
            {
                jr = Common.Util.GetJsonDataByResult(null, messageStr, -1);
            }
            else
            {
                jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
            }
            BLL.Loger.Log4Net.Info("[UserInfoController]******UpdateUserStatusInfo end->");
            return jr;
        }
        /// <summary>
        /// zlb 2017-07-21
        /// 批量启用禁用用户
        /// </summary>
        /// <param name="req">用户ID信息</param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult UpdateUserPwdInfo([FromBody]UpdateUserPwdReqsDTO req)
        {
            string strJson = Json.JsonSerializerBySingleData(req);
            BLL.Loger.Log4Net.Info("[UserInfoController]******UpdateUserPwdInfo begin...->UpdateUserStatusResDTO:" + strJson + "");
            string messageStr = "";
            try
            {
                messageStr = BLL.Advertiser.Advertiser.Instance.UpdateUserPwdInfo(req);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[UserInfoController]*****UpdateUserPwdInfo ->UpdateUserStatusResDTO:" + strJson + ",批量重置用户密码失败:" + ex.Message);
                throw ex;
            }
            Common.JsonResult jr;
            if (messageStr != "")
            {
                jr = Common.Util.GetJsonDataByResult(null, messageStr, -1);
            }
            else
            {
                jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
            }
            BLL.Loger.Log4Net.Info("[UserInfoController]******UpdateUserPwdInfo end->");
            return jr;
        }
    }
}
