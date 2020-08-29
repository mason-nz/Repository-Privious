using System;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.BO.User;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.User;
using XYAuto.ChiTu2018.Entities.Enum.UserInfo;
using XYAuto.ChiTu2018.Service.OAuth2.Dto;

namespace XYAuto.ChiTu2018.Service.Wechat
{
    /// <summary>
    /// 注释：LEWeiXinUser
    /// 作者：lihf
    /// 日期：2018/5/15 11:21:41
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LEWeiXinUserService
    {
        private LEWeiXinUserService() { }
        private static readonly Lazy<LEWeiXinUserService> Linstance = new Lazy<LEWeiXinUserService>(() => new LEWeiXinUserService());

        public static LEWeiXinUserService Instance => Linstance.Value;

        public void UpdateStatusByOpenId(int status, DateTime updatetime, string openid)
        {
            var lewxModel = new LEWeiXinUserBO().GetModel(x => x.openid == openid);
            if (lewxModel == null) return;
            var subscribe = 1;
            if (status == -1)
                subscribe = 0;

            lewxModel.subscribe = subscribe;
            lewxModel.Status = status;
            lewxModel.LastUpdateTime = updatetime;

            new LEWeiXinUserBO().Update(lewxModel);
        }

        public LE_WeiXinUser GetUnionAndUserId(string openId)
        {
            return new LEWeiXinUserBO().GetModel(x => x.openid == openId);
        }

        public bool IsExistOpenId(string openId, UserInfoCategoryEnum userCategoryEnum = UserInfoCategoryEnum.媒体主)
        {
            switch (userCategoryEnum)
            {
                case UserInfoCategoryEnum.媒体主:
                    return new LEWeiXinUserBO().GetModel(x => x.openid == openId && x.UserID > 0) != null;
                case UserInfoCategoryEnum.广告主:
                    return new LEWeiXinUserBO().GetModel(x => x.openid == openId && x.AdvertiserUserId > 0) != null;
            }

            return false;
        }

        public LE_WeiXinUser WeiXinUserOperation(WeiXinUserDto entity, UserInfoCategoryEnum categoryId = UserInfoCategoryEnum.媒体主)
        {
            if (entity.Source == 102)
                entity.Source = 3;

            var userId = new LEWeiXinUserBO().GetModelByOpenIdAndCategory(entity.openid, (int)categoryId)?.UserID ?? 0;
            if (userId > 0)
            {
                var weixinUser = categoryId == UserInfoCategoryEnum.媒体主 ? new LEWeiXinUserBO().GetModel(x => x.UserID == userId) : new LEWeiXinUserBO().GetModel(x => x.AdvertiserUserId == userId);

                if (weixinUser != null)
                {
                    weixinUser.subscribe = entity.subscribe;
                    weixinUser.nickname = entity.nickname;
                    weixinUser.sex = entity.sex;
                    weixinUser.city = entity.city;
                    weixinUser.country = entity.country;
                    weixinUser.province = entity.province;
                    weixinUser.language = entity.language;
                    weixinUser.headimgurl = entity.headimgurl;
                    weixinUser.remark = entity.remark;
                    weixinUser.groupid = entity.groupid;
                    weixinUser.tagid_list = entity.tagid_list;
                    weixinUser.LastUpdateTime = DateTime.Now;
                    weixinUser.Status = 0;
                    new LEWeiXinUserBO().Update(weixinUser);
                }
                else
                {
                    weixinUser = new LEWeiXinUserBO().Insert(new LE_WeiXinUser()
                    {
                        subscribe = entity.subscribe,
                        openid = entity.openid,
                        nickname = entity.nickname,
                        sex = entity.sex,
                        city = entity.city,
                        country = entity.country,
                        province = entity.province,
                        language = entity.language,
                        headimgurl = entity.headimgurl,
                        subscribe_time = entity.subscribe_time,
                        unionid = entity.unionid,
                        remark = entity.remark,
                        groupid = entity.groupid,
                        tagid_list = entity.tagid_list,
                        UserID = categoryId == UserInfoCategoryEnum.媒体主 ? userId : 0,
                        CreateTime = entity.CreateTime,
                        LastUpdateTime = entity.LastUpdateTime,
                        AuthorizeTime = DateTime.Now,
                        QRcode = entity.QRcode,
                        Inviter = entity.Inviter,
                        InvitationQR = entity.InvitationQR,
                        Status = 0,
                        Source = entity.Source,
                        AdvertiserUserId = categoryId == UserInfoCategoryEnum.广告主 ? userId : 0
                    });

                }

                return weixinUser;
            }
            else
            {
                #region 插入UserInfo

                var userInfo =
                    new UserInfoBO().Insert(new UserInfo()
                    {
                        UserName = entity.unionid,
                        Mobile = string.Empty,
                        Pwd = string.Empty,
                        Type = 1002,
                        Source = entity.RegisterFrom,
                        IsAuthMTZ = false,
                        IsAuthAE = false,
                        Status = 0,
                        CreateTime = DateTime.Now,
                        CreateUserID = 0,
                        LastUpdateTime = DateTime.Now,
                        LastUpdateUserID = 0,
                        Category = (int)categoryId,
                        Email = string.Empty,
                        LockState = 0,
                        SleepState = 0,
                        RegisterType = entity.RegisterType,
                        PromotionChannelID = entity.PromotionChannelID
                    });
                #endregion

                var weixinUser = new LEWeiXinUserBO().GetModelByOpenId(entity.openid);
                if (weixinUser != null)
                {
                    weixinUser.subscribe = entity.subscribe;
                    weixinUser.nickname = entity.nickname;
                    weixinUser.sex = entity.sex;
                    weixinUser.city = entity.city;
                    weixinUser.country = entity.country;
                    weixinUser.province = entity.province;
                    weixinUser.language = entity.language;
                    weixinUser.headimgurl = entity.headimgurl;
                    weixinUser.remark = entity.remark;
                    weixinUser.groupid = entity.groupid;
                    weixinUser.tagid_list = entity.tagid_list;
                    weixinUser.LastUpdateTime = DateTime.Now;
                    weixinUser.Status = 0;
                    weixinUser.UserID = categoryId == UserInfoCategoryEnum.媒体主 ? userInfo.UserID : 0;
                    weixinUser.AdvertiserUserId = categoryId == UserInfoCategoryEnum.广告主 ? userInfo.UserID : 0;
                    new LEWeiXinUserBO().Update(weixinUser);
                }
                else
                {
                    weixinUser = new LEWeiXinUserBO().Insert(new LE_WeiXinUser()
                    {
                        subscribe = entity.subscribe,
                        openid = entity.openid,
                        nickname = entity.nickname,
                        sex = entity.sex,
                        city = entity.city,
                        country = entity.country,
                        province = entity.province,
                        language = entity.language,
                        headimgurl = entity.headimgurl,
                        subscribe_time = entity.subscribe_time,
                        unionid = entity.unionid,
                        remark = entity.remark,
                        groupid = entity.groupid,
                        tagid_list = entity.tagid_list,
                        UserID = categoryId == UserInfoCategoryEnum.媒体主 ? userInfo.UserID : 0,
                        CreateTime = entity.CreateTime,
                        LastUpdateTime = entity.LastUpdateTime,
                        AuthorizeTime = DateTime.Now,
                        QRcode = entity.QRcode,
                        Inviter = entity.Inviter,
                        InvitationQR = entity.InvitationQR,
                        Status = 0,
                        Source = entity.Source,
                        AdvertiserUserId = categoryId == UserInfoCategoryEnum.广告主 ? userInfo.UserID : 0
                    });
                }

                return weixinUser;
            }
        }

        public bool GetUserSence(int userid)
        {            
            return new LEWXUserSceneBO().GetModel(x => x.UserID == userid) != null;
        }

        public LE_WeiXinUser Update(LE_WeiXinUser model)
        {
            return new LEWeiXinUserBO().Update(model);
        }
    }
}
