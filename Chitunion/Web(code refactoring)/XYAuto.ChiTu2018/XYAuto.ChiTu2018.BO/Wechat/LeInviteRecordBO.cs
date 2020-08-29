using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Task;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Wechat;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Extend.Profit;
using XYAuto.ChiTu2018.Entities.Extend.Profit.LE;

namespace XYAuto.ChiTu2018.BO.Wechat
{
    /// <summary>
    /// 注释：邀请功能BO层
    /// 作者：zhanglb
    /// 日期：2018/5/11 15:32:59
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LeInviteRecordBO
    {
        private static ILeInviteRecord LeInviteRecord()
        {
            return IocMannager.Instance.Resolve<ILeInviteRecord>();
        }

        public LE_InviteRecord AddInviteRecord(LE_InviteRecord dto)
        {
            return LeInviteRecord().AddInviteRecord(dto);
        }

        public int GetInvitedCount(int userId)
        {
            return LeInviteRecord().GetInvitedCount(userId);
        }

        public IEnumerable<BeInviterModel> GetInvitedList(int userId, int recId, int topCount)
        {
            var invitedList = from ir in LeInviteRecord().GetInvitedList(userId, recId, topCount)
                            join w in new LEWeiXinUserBO().Queryable()
                            on ir.BeInvitedUserID equals w.UserID into iw
                            from nr in iw.DefaultIfEmpty()
                            select new BeInviterModel
                            {
                                RecId = ir.RecID,
                                Nickname = nr?.nickname ?? "",
                                HeadImgurl = nr?.headimgurl ?? "",
                                InviteTime = Convert.ToDateTime(ir.InviteTime),
                                RedEvesPrice = Convert.ToDecimal(ir.RedEvesPrice),
                                RedEvesStatus = Convert.ToInt32(ir.RedEvesStatus)
                            };
            return invitedList;

        }

        public int GetInvitedMaxRecId(int userId)
        {
            return LeInviteRecord().GetInvitedMaxRecId(userId);
        }

        public int GetRedEvesCount(int userId, int recId, int redEvesStatus)
        {
            return LeInviteRecord().GetRedEvesCount(userId, recId, redEvesStatus);
        }
        public int GetTodayRedEvesCount(int userId, int redEvesStatus)
        {
            return LeInviteRecord().GetTodayRedEvesCount(userId, redEvesStatus);
        }
        public int GetRedEvesStatus(int userId, int recId)
        {
            return LeInviteRecord().GetRedEvesStatus(userId, recId);
        }
        /// <summary>
        /// 获取被邀请用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public LE_InviteRecord GetBeInviter(int userId)
        {
            return LeInviteRecord().GetBeInviter(userId);
        }

        public int UpdateRedEves(int userId)
        {
            return LeInviteRecord().UpdateRedEves(userId);
        }

        public int UpdateRedEves(int userId, int recId)
        {
            return LeInviteRecord().UpdateRedEves(userId, recId);
        }

        public int UpdateRedEvesPrice(int recId, int userId, decimal redEvesPrice, int beforeStatus, int afterStatus)
        {
            return LeInviteRecord().UpdateRedEvesPrice(recId, userId, redEvesPrice, beforeStatus, afterStatus);
        }
    }
}
