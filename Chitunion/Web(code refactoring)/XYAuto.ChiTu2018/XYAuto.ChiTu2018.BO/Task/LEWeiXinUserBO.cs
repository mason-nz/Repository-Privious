/// <summary>
/// 注释：LEWeiXinUserBO
/// 作者：lihf
/// 日期：2018/5/10 14:37:50
/// 版权所有：Copyright  2018 行圆汽车-分发业务中心
/// </summary>
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.BO.User;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Task;
using XYAuto.ChiTu2018.DAO.Chitunion2017.User;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.User;
using XYAuto.ChiTu2018.Entities.Enum.User;
using XYAuto.ChiTu2018.Entities.Enum.UserInfo;
using XYAuto.ChiTu2018.Entities.Extend.User;
using XYAuto.ChiTu2018.Infrastructure;
using XYAuto.ChiTu2018.Infrastructure.Exceptions;

namespace XYAuto.ChiTu2018.BO.Task
{
    public class LEWeiXinUserBO
    {
        private readonly ILEWeiXinUser _leWeiXinUser;

        public LEWeiXinUserBO()
        {
            _leWeiXinUser = IocMannager.Instance.Resolve<ILEWeiXinUser>();
        }

        private static ILEWeiXinUser LEWeiXinUser()
        {
            return IocMannager.Instance.Resolve<ILEWeiXinUser>();
        }

        public LE_WeiXinUser GetModel(Expression<Func<LE_WeiXinUser, bool>> expression)
        {
            return LEWeiXinUser().Retrieve(expression);
        }
        public LE_WeiXinUser GetModelByOpenIdAndCategory(string openId, int categoryId)
        {
            //var weixinUserList = LEWeiXinUser().Queryable().Where(x => true); //LEWeiXinUser().Queryable();
            //var userContext = new IoCConfig().Resolve<IUserInfo>().Queryable();

            //var userList = from weixinUser in weixinUserList
            //    join userInfo in userContext on weixinUser.UserID equals userInfo.UserID
            //    where weixinUser.openid == openId && userInfo.Category == categoryId
            //    select new XYAuto.ChiTu2018.Entities.Chitunion2017.LE.LE_WeiXinUser() {UserID = weixinUser.UserID};
            //return userList.FirstOrDefault();

            var wxUser = LEWeiXinUser().Retrieve(x => x.openid == openId);
            if (wxUser == null) return null;

            var userInfo = new UserInfoBO().GetInfo((int)wxUser.UserID);
            if (userInfo != null && userInfo.Category == categoryId) return wxUser;
            return null;
        }

        public LE_WeiXinUser Update(LE_WeiXinUser modeLeWeiXinUser)
        {
            return LEWeiXinUser().Put(modeLeWeiXinUser);
        }
        public LE_WeiXinUser Insert(LE_WeiXinUser modeLeWeiXinUser)
        {
            return LEWeiXinUser().Add(modeLeWeiXinUser);
        }

        public IEnumerable<LE_WeiXinUser> Queryable()
        {
            return LEWeiXinUser().Queryable();
        }
        public LE_WeiXinUser GetModelByUserId(int userId)
        {
            return LEWeiXinUser().Retrieve(x => x.UserID == userId);
        }
        public LE_WeiXinUser GetModelByAdvertiserUserId(int userId)
        {
            return LEWeiXinUser().Retrieve(x => x.AdvertiserUserId == userId);
        }

        public LE_WeiXinUser GetModelByOpenId(string openId)
        {
            return LEWeiXinUser().Retrieve(x => x.openid == openId);
        }

        public LE_WeiXinUser GetModelByUserId(int userId, int category)
        {
            Expression<Func<LE_WeiXinUser, bool>> expression = t => t.Status == 0;
            expression = (int)UserInfoCategoryEnum.媒体主 == category ? expression.And(t => t.UserID == userId) : expression.And(t => t.AdvertiserUserId == userId);
            return LEWeiXinUser().Retrieve(expression);
        }

        /// <summary>
        /// 用户和微信用户表一对多关系
        /// </summary>
        /// <param name="unionid"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private UserDetailDo GetWxUserJoinUser(string unionid, UserCategoryEnum categoryId = UserCategoryEnum.媒体主)
        {
            var userContext = IocMannager.Instance.Resolve<IUserInfo>();

            var userList = from wx in _leWeiXinUser.Queryable()
                           join user in userContext.Queryable() on wx.UserID equals user.UserID
                           where wx.unionid == unionid && user.Category == (int)categoryId
                           orderby wx.CreateTime ascending
                           select new UserDetailDo { UserId = user.UserID, Mobile = user.Mobile };

            return userList.AsNoTracking().FirstOrDefault();
        }

        private int Exist(string openid)
        {
            return _leWeiXinUser.Queryable().AsNoTracking().Where(s => s.openid == openid).Select(s => s.WeiXinUserID).FirstOrDefault();
        }

        #region 事物-微信用户操作

        /// <summary>
        /// 微信用户操作
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Tuple<bool, int> WeiXinUserOperation(Entities.Extend.User.WeiXinUserOperateDo entity)
        {
            if (entity.Source == 102)
            {
                entity.Source = 3;
            }
            var isSuccess = false;
            var userInfo = GetWxUserJoinUser(entity.unionid);
            var userId = userInfo == null ? 0 : userInfo.UserId;
            using (var scope = new TransactionScope(TransactionScopeOption.Required)) //开启事务
            {
                try
                {
                    if (userId > 0)
                    {
                        //todo:1判断wx user 是否存在
                        var wxUserId = Exist(entity.openid);
                        if (wxUserId > 0)
                        {
                            //todo:update 更新用户信息，但是外面的逻辑根本没有处理
                            entity.WeiXinUserId = wxUserId;
                            if (WeiXinUserOperationForUpdateWxUser(entity) > 0)
                            {
                                //todo:success
                                isSuccess = true;
                            }
                        }
                        else
                        {
                            wxUserId = WeiXinUserOperationForAddWxUser(entity, userId);
                            if (wxUserId > 0)
                            {
                                //todo:success
                                isSuccess = true;
                            }
                        }
                    }
                    else
                    {
                        //todo:先插入user 表
                        userId = WeiXinUserOperationForAddUser(entity);
                        if (userId > 0)
                        {
                            var wxUserId = WeiXinUserOperationForAddWxUser(entity, userId);
                            if (wxUserId > 0)
                            {
                                //todo:success
                                isSuccess = true;
                            }
                        }
                    }
                    if (isSuccess)
                        scope.Complete();
                }
                catch (Exception exception)
                {
                    throw new WeiXinUserOperationException($"WeiXinUserOperation 操作失败:{exception.Message}" +
                                                           $"{exception.StackTrace ?? string.Empty}");
                }
            }

            return Tuple.Create(isSuccess, userId);
        }

        private int WeiXinUserOperationForAddUser(Entities.Extend.User.WeiXinUserOperateDo entity)
        {
            var userContext = IocMannager.Instance.Resolve<IUserInfo>();
            var info = userContext.Add(new UserInfo()
            {
                UserName = entity.unionid,
                Mobile = "",
                Pwd = "",
                Type = 1002,
                Source = entity.RegisterFrom,
                IsAuthMTZ = false,
                AuthAEUserID = 0,
                IsAuthAE = false,
                SysUserID = 0,
                EmployeeNumber = "",
                Status = 0,
                CreateTime = DateTime.Now,
                CreateUserID = 0,
                LastUpdateTime = DateTime.Now,
                LastUpdateUserID = 0,
                Category = 29002,
                Email = "",
                LockState = 0,
                SleepState = 0,
                LockType = 0,
                SleepStatus = 0,
                RegisterType = entity.RegisterType,
                PromotionChannelID = entity.PromotionChannelID,
                RegisterIp = entity.RegisterIp
            });
            return info == null ? 0 : info.UserID;
        }

        private int WeiXinUserOperationForAddWxUser(Entities.Extend.User.WeiXinUserOperateDo entity, int userId)
        {
            var info = _leWeiXinUser.Add(new LE_WeiXinUser()
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
                UserID = userId,
                CreateTime = DateTime.Now,
                LastUpdateTime = DateTime.Now,
                AuthorizeTime = entity.AuthorizeTime,
                QRcode = entity.QRcode,
                Inviter = entity.Inviter,
                InvitationQR = entity.InvitationQR,
                Status = entity.Status,
                Source = entity.Source,
                AdvertiserUserId = 0
            });
            return info == null ? 0 : info.WeiXinUserID;
        }

        private int WeiXinUserOperationForUpdateWxUser(Entities.Extend.User.WeiXinUserOperateDo entity)
        {
            return _leWeiXinUser.WeiXinUserOperationForUpdateWxUser(entity);
        }


        #endregion
    }
}
