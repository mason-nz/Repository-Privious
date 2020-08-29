using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Enum.Wechat;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Wechat.Impl
{
    public sealed class LeInviteRecordImpl : RepositoryImpl<LE_InviteRecord>, ILeInviteRecord
    {
        public LE_InviteRecord AddInviteRecord(LE_InviteRecord dto)
        {
            return Add(dto);
        }
        public int GetInvitedCount(int userId)
        {
            return Queryable().Count(t => t.InviteUserID == userId);
        }

        public IEnumerable<LE_InviteRecord> GetInvitedList(int userId, int recId, int topCount)
        {
            return Queryable().Where(t => t.InviteUserID == userId && t.RecID <= recId).OrderByDescending(t => t.RecID).Take(topCount);
        }

        public int GetInvitedMaxRecId(int userId)
        {
            LE_InviteRecord inviteRecord = Queryable().Where(t => t.InviteUserID == userId).OrderByDescending(t => t.RecID).FirstOrDefault();
            return inviteRecord?.RecID ?? 0;

        }

        public int GetRedEvesCount(int userId, int recId, int redEvesStatus)
        {
            var inviteRecord = Retrieve(i => i.RecID == recId);
            var date = DateTime.Now.AddDays(1);
            if (inviteRecord?.InviteTime != null)
            {
                date = Convert.ToDateTime(inviteRecord.InviteTime);
            }
            return Queryable().Count(t => t.InviteUserID == userId && t.RedEvesStatus == redEvesStatus &&
            SqlFunctions.DateDiff("day", t.InviteTime, date) == 0);
        }

        public int GetTodayRedEvesCount(int userId, int redEvesStatus)
        {
            return Queryable().Count(t => t.InviteUserID == userId && t.RedEvesStatus == redEvesStatus && SqlFunctions.DateDiff("day", t.InviteTime, DateTime.Now) == 0);
        }

        public int GetRedEvesStatus(int userId, int recId)
        {
            return (int)Retrieve(t => t.InviteUserID == userId && t.RecID == recId).RedEvesStatus;
        }

        public LE_InviteRecord GetBeInviter(int userId)
        {
            return Retrieve(t => t.BeInvitedUserID == userId);

        }
        public int UpdateRedEves(int userId)
        {
            string strSql = "UPDATE LE_InviteRecord SET RedEvesStatus=@afterStatus WHERE BeInvitedUserID=@userId AND  RedEvesStatus=@beforeStatus";
            object[] paras = {
                 new SqlParameter("@userId",userId),
                 new SqlParameter("@beforeStatus",(int)RedEvesStatusEnum.待领取),
                 new SqlParameter("@afterStatus",(int)RedEvesStatusEnum.不可领取)
                };
            return context.Database.ExecuteSqlCommand(strSql, paras);
        }
        public int UpdateRedEves(int userId, int recId)
        {
            string strSql = "UPDATE LE_InviteRecord SET RedEvesStatus=@afterStatus WHERE InviteUserID=@userId AND (RedEvesStatus=@beforeStatus or RedEvesStatus=@unfinishedStatus) AND CONVERT(VARCHAR(10),InviteTime,23)= (SELECT CONVERT(VARCHAR(10),InviteTime,23) FROM LE_InviteRecord WHERE RecID=@recId)";
            object[] paras = {
                 new SqlParameter("@recId",recId),
                 new SqlParameter("@userId",userId),
                 new SqlParameter("@beforeStatus",(int)RedEvesStatusEnum.待领取),
                 new SqlParameter("@unfinishedStatus",(int)RedEvesStatusEnum.尚未完成分享),
                 new SqlParameter("@afterStatus",(int)RedEvesStatusEnum.不可领取)
                };
            return context.Database.ExecuteSqlCommand(strSql, paras);
        }
        public int UpdateRedEvesPrice(int recId, int userId, decimal redEvesPrice, int beforeStatus, int afterStatus)
        {
            string strSql = "UPDATE LE_InviteRecord SET RedEvesPrice=@redEvesPrice,ReceiveTime=GETDATE(),RedEvesStatus=@afterStatus WHERE RecID=@recId AND InviteUserID=@userId AND  RedEvesStatus=@beforeStatus";
            object[] paras = {
                 new SqlParameter("@recId",recId),
                 new SqlParameter("@userId",userId),
                 new SqlParameter("@redEvesPrice",redEvesPrice),
                 new SqlParameter("@beforeStatus",beforeStatus),
                 new SqlParameter("@afterStatus",afterStatus)
                };
            return context.Database.ExecuteSqlCommand(strSql, paras);
        }
    }
}
