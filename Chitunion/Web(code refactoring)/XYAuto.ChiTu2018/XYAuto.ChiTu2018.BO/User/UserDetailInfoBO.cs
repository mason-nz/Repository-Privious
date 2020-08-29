using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.DAO.Chitunion2017.User;
using XYAuto.ChiTu2018.Entities.Chitunion2017;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.User;
using XYAuto.ChiTu2018.Entities.User.Dto;

namespace XYAuto.ChiTu2018.BO.User
{
    /// <summary>
    /// 注释：UserDetailInfoBO
    /// 作者：lix
    /// 日期：2018/5/11 10:15:43
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class UserDetailInfoBO
    {
        private static IUserDetailInfo UserDetailInfo()
        {
            return IocMannager.Instance.Resolve<IUserDetailInfo>();
        }
        public UserDetailInfo GetUserDetailByUserId(int userId)
        {
            return IocMannager.Instance.Resolve<IUserDetailInfo>().Retrieve(s => s.UserID == userId);
        }
        public IEnumerable<UserDetailInfo> Queryable()
        {
            return UserDetailInfo().Queryable();
        }
        public RespUserDetailDto GetUserDetail(int userId)
        {
            var userBasicList = from u in new UserInfoBO().GetAllUser().Where(t => t.UserID == userId)
                join ud in new UserDetailInfoBO().Queryable().Where(t => t.UserID == userId) on u.UserID equals
                    ud.UserID
                select new RespUserDetailDto
                {
                    Type = (int) u.Type,
                    Status = ud.Status,
                    TrueName = ud.TrueName,
                    IdentityNo = ud.IdentityNo,
                    IdCardFrontUrl = ud.IDCardFrontURL,
                    IdCardBackUrl = ud.IDCardBackURL,
                    BLicenceUrl = ud.BLicenceURL,
                    Reason = ud.Reason
                };
            
            return userBasicList.FirstOrDefault();
        }

        public bool IsExistsIdentityNo(int userId, int category, string identityNo)
        {
            var leftQuery =
                from u in new UserInfoBO().GetAllUser().Where(t => t.UserID != userId && t.Category == category)
                join ud in new UserDetailInfoBO().Queryable().Where(t => t.UserID == userId) on u.UserID equals ud.UserID
                select new
                {
                    Detail = ud
                };
            return leftQuery.Any();
        }

        public UserDetailInfo AddUserDetail(UserDetailInfo info)
        {
            return UserDetailInfo().Add(info);
        }

        public UserDetailInfo UpdateUserDetail(UserDetailInfo info)
        {
            return UserDetailInfo().Put(info);
        }

    }
}
