/**
*
*创建人：lixiong
*创建时间：2018/5/10 15:12:56
*说明：
*版权所有：Copyright  2018 行圆汽车-分发业务中心
*/
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.Entities.User.Dto;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.DAO.Chitunion2017.User;
using XYAuto.ChiTu2018.Entities.Chitunion2017;
using XYAuto.ChiTu2018.Entities.Chitunion2017.User;
using XYAuto.ChiTu2018.Entities.Extend.Profit.LE;
using XYAuto.ChiTu2018.Entities.Extend.User;
using System.Linq.Expressions;
using System.Transactions;
using XYAuto.ChiTu2018.Entities.ThirdModel;
using XYAuto.ChiTu2018.Infrastructure.AutoMapper;
using XYAuto.ChiTu2018.Infrastructure;

namespace XYAuto.ChiTu2018.BO.User
{
    public class UserInfoBO
    {
        private static IUserInfo UserInfo()
        {
            return IocMannager.Instance.Resolve<IUserInfo>();
        }
        public UserInfo GetInfo(int userId)
        {
            return IocMannager.Instance.Resolve<IUserInfo>().Retrieve(s => s.UserID == userId);
        }

        public UserDetailDo GetJoinDo(int userId)
        {
            var userContext = IocMannager.Instance.Resolve<IUserInfo>().Queryable().AsNoTracking();
            var userDetailContext = IocMannager.Instance.Resolve<IUserDetailInfo>().Queryable().AsNoTracking();

            var userDetailDo = from userInfo in userContext
                               join detailInfo in userDetailContext on userInfo.UserID equals detailInfo.UserID
                               where userInfo.UserID == userId
                               select
                                   new UserDetailDo() { UserId = userInfo.UserID, AuditStatus = detailInfo.Status, Mobile = userInfo.Mobile };
            return userDetailDo.FirstOrDefault();
        }
        public UserInfo Update(UserInfo modelUserInfo)
        {
            return UserInfo().Put(modelUserInfo);
        }
        public UserInfo Insert(UserInfo modelUserInfo)
        {
            return UserInfo().Add(modelUserInfo);
        }
        public UserInfo GetUserInfoByMobile(string mobile, int categoryId, int userID = 0)
        {
            Expression<Func<UserInfo, bool>> expression = t => t.Status == 0 && t.Mobile == mobile && t.Category == categoryId;
            if (userID > 0)
            {
                expression = expression.And(s => s.UserID != userID);
            }
            return UserInfo().Retrieve(expression);

        }

        public string GetMobileByUserId(int userId)
        {
            UserInfo userInfo = UserInfo().Retrieve(t => t.UserID == userId);
            return userInfo?.Mobile ?? string.Empty;
        }

        public IEnumerable<UserInfo> GetAllUser()
        {
            return UserInfo().Queryable();
        }
        public bool IsExistsIdentityNo(List<int> listUserId, int category, string identityNo)
        {
            return UserInfo().Queryable().Join(IocMannager.Instance.Resolve<IUserDetailInfo>().Queryable(),
                u => u.UserID, d => d.UserID, (u, d) => new { u, d }).Any(x => x.u.Category == category &&
                                                                               x.d.IdentityNo == identityNo &&
                                                                               !listUserId.Contains(x.d.UserID));
        }
        public RespUserBasicDto GetUserRelatedInfo(int userId)
        {
            var userBasicList = from u in GetAllUser().Where(t => t.UserID == userId)
                                join w in new LEWeiXinUserBO().Queryable().Where(t => t.UserID == userId) on u.UserID equals w.UserID into uw
                                from nr in uw.DefaultIfEmpty()
                                join ub in new LeUserBankAccountBO().Queryable().Where(t => t.UserID == userId && t.Status == 0) on u.UserID equals ub.UserID into uub
                                from ubb in uub.DefaultIfEmpty()
                                join ud in new UserDetailInfoBO().Queryable().Where(t => t.UserID == userId) on u.UserID equals ud.UserID into uud
                                from udd in uud.DefaultIfEmpty()
                                select new RespUserBasicDto
                                {
                                    Mobile = u.Mobile,
                                    NickName = nr == null ? "" : nr.nickname,
                                    IsFollow = nr?.Status ?? -1,
                                    AccountName = ubb == null ? "" : ubb.AccountName,
                                    AccountType = ubb?.AccountType ?? -1,
                                    HeadImgUrl = nr == null ? "" : nr.headimgurl,
                                    Status = udd?.Status ?? 0,
                                    Sex = udd?.Sex ?? -2,
                                    IdentityNo = udd?.IdentityNo ?? "",
                                    TrueName = udd?.TrueName ?? ""
                                };
            return userBasicList.FirstOrDefault();
        }

        public bool CleanUserInfoForM(int oldUserId, int newUserId)
        {
            var result = UserInfo().CleanUserInfoForM(oldUserId, newUserId);
            return result.Item1 > 0 && result.Item2;
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="userAll"></param>
        public string AddUserInfo(UserInfoAll userAll)
        {
            try
            {
                using (var tranScope = new TransactionScope())
                {
                    if (new UserDetailInfoBO().Queryable().All(p => p.UserID != userAll.UserID))
                        new UserDetailInfoBO().AddUserDetail(userAll.MapTo<UserDetailInfo>());
                    else
                        IocMannager.Instance.Resolve<IUserDetailInfo>().UpdateUserDetail(userAll.MapTo<UserDetailInfo>());
                    UserInfo().UpdateUserMobile(userAll.MapTo<UserInfo>());
                    new LeUserBankAccountBO().TransUpdate(userAll.UserID, userAll.AccountType, userAll.AccountName);

                    tranScope.Complete();

                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 获取汽车大全端登录用户
        /// </summary>
        /// <param name="channelUserId">汽车大全用户Id</param>
        /// <param name="category">分类</param>
        /// <returns></returns>
        public UserInfo GetUserInfoByChannelUid(string channelUserId, int category)
        {
            return UserInfo().Retrieve(t => t.Status == 0 && t.ChannelUserID == channelUserId && t.Category == category);
        }
        /// <summary>
        /// 更新汽车大全用户
        /// </summary>
        /// <param name="carUser"></param>
        public void UpdateCarUser(CarToChiTuUser carUser)
        {
            UserInfo().UpdateCarUser(carUser);
        }


    }
}
