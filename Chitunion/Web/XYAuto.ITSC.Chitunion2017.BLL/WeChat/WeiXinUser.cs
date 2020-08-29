/********************************************************
*创建人：hant
*创建时间：2018/1/17 13:30:25 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat
{
    public class WeiXinUser
    {
        public static readonly WeiXinUser Instance = new WeiXinUser();

        public int Insert(Entities.WeChat.WeiXinUser entity)
        {
            return Dal.WeChat.WeiXinUser.Instance.Insert(entity);
        }


        public bool WeiXinUserOperation(Entities.WeChat.WeiXinUser entity, UserCategoryEnum categoryId = UserCategoryEnum.媒体主)
        {
            Loger.Log4Net.Info($"WeiXinUserOperation BLL openid：{entity.openid}...");
            return Dal.WeChat.WeiXinUser.Instance.WeiXinUserOperation(entity, categoryId);
        }


        public bool InsetWeiXinAndUserInfo(Entities.WeChat.WeiXinUser entity)
        {
            return Dal.WeChat.WeiXinUser.Instance.InsetWeiXinAndUserInfo(entity);
        }


        public bool WeiXinUserAuthorization(Entities.WeChat.WeiXinUser entity)
        {
            return Dal.WeChat.WeiXinUser.Instance.WeiXinUserAuthorization(entity);
        }
        public void Update(Entities.WeChat.WeiXinUser entity)
        {
            Dal.WeChat.WeiXinUser.Instance.Update(entity);
        }

        public void UpdateSync(Entities.WeChat.WeiXinUser entity)
        {
            Dal.WeChat.WeiXinUser.Instance.UpdateSync(entity);
        }



        public bool UpdateUnionID(string openid, string unionid)
        {
            return Dal.WeChat.WeiXinUser.Instance.UpdateUnionID(openid,unionid);
        }

       

        public bool IsExist(string unionid)
        {
            return Dal.WeChat.WeiXinUser.Instance.IsExist(unionid);
        }


        public bool IsExistOpneId(string OpneId, UserCategoryEnum userCategoryEnum = UserCategoryEnum.媒体主)
        {

            return Dal.WeChat.WeiXinUser.Instance.IsExistOpneId(OpneId, userCategoryEnum);
        }


        public Entities.WeChat.WeiXinUser GetUserInfoByUserId(int UserId, UserCategoryEnum userCategoryEnum = UserCategoryEnum.媒体主)
        {

            return Dal.WeChat.WeiXinUser.Instance.GetUserInfoByUserId(UserId, userCategoryEnum);
        }

        public bool IsExistOpneIdAndUnionId(string OpneId, string UnionId)
        {
            return Dal.WeChat.WeiXinUser.Instance.IsExistOpneIdAndUnionId(OpneId, UnionId);
        }

        /// <summary>
        /// 根据OPENID获取UNIONID 和 USERID
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public Entities.DTO.V2_3.WXUserInfoRspDTO GetUnionAndUserId(string openid)
        {
            return Dal.WeChat.WeiXinUser.Instance.GetUnionAndUserId(openid);
        }

        public void UpdateStatusByOpneId(int status, DateTime updatetime, string openid)
        {
            Dal.WeChat.WeiXinUser.Instance.UpdateStatusByOpneId(status, updatetime, openid);
        }
        public void UpdateStatusByOpneId(int status, DateTime updatetime,DateTime subscribe_time, string openid)
        {
            Dal.WeChat.WeiXinUser.Instance.UpdateStatusByOpneId(status, updatetime, subscribe_time, openid);
        }

        public void UpdateStatusByOpneIdAndUnionId(int status, DateTime updatetime, string openid, string unionid)
        {
            Dal.WeChat.WeiXinUser.Instance.UpdateStatusByOpneIdAndUnionId(status, updatetime, openid,unionid);
        }


        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public string GetAccessToken(string appId, string appSecret)
        {
            return Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessToken(appId,appSecret);
        }


        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="actionname">Senparc.Weixin.MP.QrCode_ActionName 枚举类</param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public string CreateQrCode(string appId, int userid, Senparc.Weixin.MP.QrCode_ActionName actionname = Senparc.Weixin.MP.QrCode_ActionName.QR_LIMIT_STR_SCENE )
        {
            try
            {
                StringBuilder json = new StringBuilder();


                //var userinfo = BLL.WeChat.WeiXinUser.Instance.GetUserInfo(openid);
                var qrCodeResult = Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.Create(appId, 2592000, userid, actionname, json.ToString());
                return QrCodeApi.GetShowQrCodeUrl(qrCodeResult.ticket);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[CreateQrCode]报错", ex);
                return null;
            }
        }

        public Entities.WeChat.WeiXinUser GetUserInfo(string openid)
        {
            return Dal.WeChat.WeiXinUser.Instance.GetUserInfo(openid);
        }

        public bool GetUserSence(int userid)
        {
            return Dal.WeChat.WeiXinUser.Instance.GetUserSence(userid);
        }

        public List<Entities.WeChat.WeiXinUser> GetUsers()
        {
            return Dal.WeChat.WeiXinUser.Instance.GetUsers();
        }
        public int ExecuteNonQuery(string strSql)
        {
            return Dal.WeChat.WeiXinUser.Instance.ExecuteNonQuery(strSql);
        }
    }
}
