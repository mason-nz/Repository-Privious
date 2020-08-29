using System;
using System.Data.Entity;
using XYAuto.ChiTu2018.DAO.Chitunion2017.User;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.User;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Impl.User
{
    /// <summary>
    /// 注释：UserDetailInfoImpl
    /// 作者：lix
    /// 日期：2018/5/11 9:41:21
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public sealed class UserDetailInfoImpl : RepositoryImpl<UserDetailInfo>, IUserDetailInfo
    {
        public int UpdateUserDetail(UserDetailInfo userDetailInfo)
        {
            int returnInt = 0;
            using (var db = new Chitunion2017DbContext())
            {
                db.Entry(userDetailInfo).State = EntityState.Modified;
                db.Entry(userDetailInfo).Property(p => p.TrueName).IsModified = true;
                db.Entry(userDetailInfo).Property(p => p.ProvinceID).IsModified = true;
                db.Entry(userDetailInfo).Property(p => p.CityID).IsModified = true;
                db.Entry(userDetailInfo).Property(p => p.Address).IsModified = true;
                db.Entry(userDetailInfo).Property(p => p.BLicenceURL).IsModified = true;
                db.Entry(userDetailInfo).Property(p => p.OrganizationURL).IsModified = true;
                db.Entry(userDetailInfo).Property(p => p.IDCardFrontURL).IsModified = true;
                db.Entry(userDetailInfo).Property(p => p.IDCardBackURL).IsModified = true;
                db.Entry(userDetailInfo).Property(p => p.IdentityNo).IsModified = true;
                db.Entry(userDetailInfo).Property(p => p.LastUpdateTime).IsModified = true;
                db.Entry(userDetailInfo).Property(p => p.LastUpdateTime).CurrentValue = DateTime.Now;
                db.Entry(userDetailInfo).Property(p => p.Sex).IsModified = true;
                returnInt = db.SaveChanges();
            }
            return returnInt;
        }
    }
}
