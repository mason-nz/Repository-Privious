using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Senparc.Weixin;
using Senparc.Weixin.Helpers.Extensions;
using Senparc.Weixin.MP.AdvancedAPIs.User;


namespace XYAuto.ChiTu2018.WeChat.QueryDataConsole.WeChatUser
{
    /// <summary>
    /// 注释：UserService
    /// 作者：masj
    /// 日期：2018/5/25 15:56:19
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class UserService
    {
        public readonly static UserService Instance = new UserService();

        /// <summary>
        /// 获取公众号的OpenId列表
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="nextOpenId"></param>
        /// <param name="openIds"></param>
        public void GetOpenIdsByAppId(string appid, string nextOpenId, ref List<string> openIds)
        {
            var resultList = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Get(appid, nextOpenId);
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(JsonConvert.SerializeObject(resultList));
            if (resultList != null)
            {
                if (resultList.count > 0)
                {
                    openIds = openIds.Union(resultList.data.openid).ToList<string>();
                    //GetUserInfoByOpenIds(appid, openIds);
                }
                if (!string.IsNullOrEmpty(resultList.next_openid))
                {
                    GetOpenIdsByAppId(appid, resultList.next_openid, ref openIds);
                }
            }
        }

        public DataTable GetUserInfoByOpenIds(string appid, List<string> openIds,int userTypeId)
        {
            DataTable userInfos = new DataTable();
            userInfos.Columns.Add("微信用户在公众号内唯一标识（openid）", typeof (string));
            userInfos.Columns.Add("微信用户在开放平台中的唯一标识（unionid）", typeof(string));
            userInfos.Columns.Add("微信用户昵称（nickname）", typeof(string));
            userInfos.Columns.Add("微信用户关注状态（1-已关注，0-未关注）", typeof(int));
            userInfos.Columns.Add("微信用户关注时间（subscribe_time）", typeof(string));
            userInfos.Columns.Add("微信用户二维码扫码场景（qr_scene）", typeof(string));
            userInfos.Columns.Add("微信用户二维码扫码场景描述（qr_scene_str）", typeof(string));
            int loopLength = 100;
            if (openIds != null && openIds.Count > 0)
            {
                List<BatchGetUserInfoData> list = openIds.Select(openId => new BatchGetUserInfoData()
                {
                    openid = openId,
                    LangEnum = Language.zh_CN
                }).ToList();

                var list2 = list.Take(loopLength).ToList();
                while (list2 != null && list2.Count > 0)
                {
                    var result = Senparc.Weixin.MP.AdvancedAPIs.UserApi.BatchGetUserInfo(appid, list2);
                    if (result != null)
                    {
                        XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("获取用户基本信息列表：\r\n" + result.ToJson());

                        foreach (var userInfoJson in result.user_info_list)
                        {
                            DataRow dr=userInfos.NewRow();
                            dr["微信用户在公众号内唯一标识（openid）"] = userInfoJson.openid;
                            dr["微信用户在开放平台中的唯一标识（unionid）"] = userInfoJson.unionid;
                            dr["微信用户昵称（nickname）"] = userInfoJson.nickname;
                            dr["微信用户关注状态（1-已关注，0-未关注）"] = userInfoJson.subscribe;
                            dr["微信用户关注时间（subscribe_time）"] = XYAuto.CTUtils.Sys.ConverHelper.ConvertDateTimeByTimeStamp(userInfoJson.subscribe_time);
                            dr["微信用户二维码扫码场景（qr_scene）"] = userInfoJson.qr_scene;
                            dr["微信用户二维码扫码场景描述（qr_scene_str）"] = userInfoJson.qr_scene_str;
                            userInfos.Rows.Add(dr);
                            //string openid = userInfoJson.openid;
                            string msg = $"获取用户基本信息，openId={userInfoJson.openid},unionid={userInfoJson.unionid},nickname={userInfoJson.nickname},subscribe={userInfoJson.subscribe},subscribe_time={ XYAuto.CTUtils.Sys.ConverHelper.ConvertDateTimeByTimeStamp(userInfoJson.subscribe_time)},qr_scene={userInfoJson.qr_scene},qr_scene={userInfoJson.qr_scene_str}";
                            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(msg);
                        }
                    }
                    if (list.Count > loopLength)
                    {
                        list.RemoveRange(0, loopLength);
                        list2 = list.Take(loopLength).ToList();
                    }
                    else
                    {
                        list.RemoveRange(0, list.Count);
                        list2 = null;
                    }

                }



                //foreach (string openId in openIds)
                //{
                //    var userInfoJson = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(appid, openId);
                //    if (userInfoJson != null)
                //    {
                //        string msg = $"获取用户基本信息，openId={userInfoJson.openid},subscribe={userInfoJson.subscribe},subscribe_time={ XYAuto.CTUtils.Sys.ConverHelper.ConvertDateTimeByTimeStamp(userInfoJson.subscribe_time)},qr_scene={userInfoJson.qr_scene},qr_scene={userInfoJson.qr_scene_str}";
                //        XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(msg);
                //    }
                //}
            }
            return userInfos;




        }
    }
}
