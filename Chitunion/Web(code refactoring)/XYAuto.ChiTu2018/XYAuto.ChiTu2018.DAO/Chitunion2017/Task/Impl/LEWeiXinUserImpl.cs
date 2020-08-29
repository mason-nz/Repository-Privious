/// <summary>
/// 注释：LEWeiXinUserImpl
/// 作者：lihf
/// 日期：2018/5/10 10:09:52
/// 版权所有：Copyright  2018 行圆汽车-分发业务中心
/// </summary>

using System;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Extend.User;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Task.Impl
{
    public class LEWeiXinUserImpl : RepositoryImpl<LE_WeiXinUser>, ILEWeiXinUser
    {
        public LE_WeiXinUser GetModelByUserId(int userId)
        {
            return Retrieve(t => t.UserID == userId);
        }

        public int WeiXinUserOperationForUpdateWxUser(WeiXinUserOperateDo entity)
        {
            var entry = context.LE_WeiXinUser.Attach(new LE_WeiXinUser()
            {
                WeiXinUserID = entity.WeiXinUserId,
                subscribe = entity.subscribe,
                nickname = entity.nickname,
                sex = entity.sex,
                city = entity.city,
                country = entity.country,
                province = entity.province,
                language = entity.language,
                headimgurl = entity.headimgurl,
                remark = entity.remark,
                groupid = entity.groupid,
                tagid_list = entity.tagid_list,
                LastUpdateTime = DateTime.Now,
                InvitationQR = entity.InvitationQR,
            });
            context.Entry(entry).Property(p => p.subscribe).IsModified = true;
            context.Entry(entry).Property(p => p.nickname).IsModified = true;
            context.Entry(entry).Property(p => p.sex).IsModified = true;
            context.Entry(entry).Property(p => p.city).IsModified = true;
            context.Entry(entry).Property(p => p.country).IsModified = true;
            context.Entry(entry).Property(p => p.province).IsModified = true;
            context.Entry(entry).Property(p => p.language).IsModified = true;
            context.Entry(entry).Property(p => p.headimgurl).IsModified = true;
            context.Entry(entry).Property(p => p.remark).IsModified = true;
            context.Entry(entry).Property(p => p.groupid).IsModified = true;
            context.Entry(entry).Property(p => p.tagid_list).IsModified = true;
            context.Entry(entry).Property(p => p.LastUpdateTime).IsModified = true;
            context.Entry(entry).Property(p => p.InvitationQR).IsModified = true;
            return context.SaveChanges();
        }
    }
}
