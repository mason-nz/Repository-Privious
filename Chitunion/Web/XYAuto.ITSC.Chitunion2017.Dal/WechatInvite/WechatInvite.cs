using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.WechatInvite;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.WechatInvite;
using XYAuto.ITSC.Chitunion2017.Entities.WechatSign;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.WechatInvite
{
    public class WechatInvite : DataBase
    {
        public static readonly WechatInvite Instance = new WechatInvite();
        /// <summary>
        /// 插入邀请好友记录
        /// </summary>
        /// <param name="InviteInfo">邀请信息</param>
        /// <returns></returns>
        public int InsertInviteRecord(WechatInviteInfo InviteInfo)
        {
            string strSql = $@"INSERT INTO dbo.LE_InviteRecord
                                ( InviteUserID,
                                  BeInvitedUserID,
                                  InviteTime,
                                  RedEvesStatus,
                                  RedEvesPrice,
                                  IP
                                )
                        VALUES  ( {InviteInfo.InviteUserID},
                                  {InviteInfo.BeInvitedUserID}, 
                                  GETDATE(),
                                  {InviteInfo.RedEvesStatus},
                                   0,
                                   '{InviteInfo.IP}'
                                )";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }

        /// <summary>
        /// 判断是否被邀请过
        /// </summary>
        /// <param name="WxBeInvitedUserID">被邀请人微信ID</param>
        /// <returns></returns>
        public bool IsBeInserted(int WxBeInvitedUserID)
        {
            string strSql = $"SELECT COUNT(1) FROM  LE_InviteRecord WHERE WxBeInvitedUserID={WxBeInvitedUserID}";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? true : (Convert.ToInt32(obj) > 0 ? false : true);
        }
        /// <summary>
        ///获取邀请人当日RedEvesStatus状态红包的个数
        /// </summary>
        /// <param name="InvitedUserID">邀请人ID</param>
        /// <param name="RedEvesStatus">（104001领取红包 104002已领取 104003当日红包已领完）</param>
        /// <returns></returns>
        public int GetRedEvesByStatus(int InviterId, int RedEvesStatus)
        {
            string strSql = $"SELECT COUNT(1) FROM  LE_InviteRecord WHERE RedEvesStatus={RedEvesStatus} and InviteUserID={InviterId} and  CONVERT(VARCHAR(10),InviteTime,23)= CONVERT(VARCHAR(10),GETDATE(),23)";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// 更新红包金额（领取红包）
        /// </summary>
        /// <param name="RedEvesPrice">红包金额</param>
        /// <param name="UserId">领取人</param>
        /// <param name="RecID">主键ID</param>
        /// <returns></returns>
        public int UpdateRedEves(decimal RedEvesPrice, int UserId, int RecID)
        {
            string strSql = $"UPDATE LE_InviteRecord SET RedEvesPrice={RedEvesPrice},ReceiveTime=GETDATE(),RedEvesStatus={(int)RedEvesStatusEnum.已领取} WHERE RecID={RecID} AND InviteUserID={UserId} AND  RedEvesStatus={(int)RedEvesStatusEnum.待领取}";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// 获取被邀请好友列表
        /// </summary>
        /// <param name="UserId">邀请人内部ID</param>
        /// <param name="RecID">起始ID</param>
        /// <param name="TopCount">行数</param>
        /// <returns></returns>
        public DataTable GetBeInvitedList(int UserId, int RecID, int TopCount)
        {
            string strSql = $"SELECT TOP {TopCount} IR.RecID,WU.nickname Nickname,WU.headimgurl HeadImgurl,IR.InviteTime,IR.RedEvesPrice,IR.RedEvesStatus  FROM  LE_InviteRecord IR LEFT JOIN  v_LE_WeiXinUser WU ON IR.BeInvitedUserID=WU.UserID WHERE  InviteUserID={UserId} and IR.RecID<{RecID} ORDER BY IR.RecID DESC";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// 获取邀请好友数量
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public int GetBeInvitedCount(int UserId)
        {
            string strSql = $"SELECT count(1)  FROM  LE_InviteRecord IR  WHERE  InviteUserID={UserId}";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// 查询用户是否被邀请过
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public bool IsBeInvited(int UserId)
        {
            string strSql = $"SELECT count(1)  FROM  LE_InviteRecord IR  WHERE  BeInvitedUserID={UserId}";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? false : (Convert.ToInt32(obj) > 0 ? true : false);
        }
        /// <summary>
        ///获取邀请人邀请日RedEvesStatus状态红包的个数
        /// </summary>
        /// <param name="InvitedUserID">邀请人ID</param>
        /// <param name="RedEvesStatus">（104001领取红包 104002已领取 104003当日红包已领完）</param>
        /// <param name="RecId">邀请表主键ID</param>
        /// <returns></returns>
        public int GetRedEvesByStatus(int InviterId, int RedEvesStatus, int RecId)
        {
            string strSql = $"SELECT COUNT(1) FROM  LE_InviteRecord WHERE RedEvesStatus={RedEvesStatus} and InviteUserID={InviterId} and  CONVERT(VARCHAR(10),InviteTime,23)= (SELECT CONVERT(VARCHAR(10),InviteTime,23) FROM LE_InviteRecord WHERE RecID={RecId})";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// 更新待领取的红包为不可领取
        /// </summary>
        /// <param name="UserId">领取人</param>
        /// <param name="RecId">邀请表主键ID</param>
        /// <returns></returns>
        public int CancelRedEves(int UserId, int RecId)
        {
            string strSql = $"UPDATE LE_InviteRecord SET RedEvesStatus={(int)RedEvesStatusEnum.不可领取} WHERE InviteUserID={UserId} AND  RedEvesStatus={(int)RedEvesStatusEnum.待领取} and  CONVERT(VARCHAR(10),InviteTime,23)= (SELECT CONVERT(VARCHAR(10),InviteTime,23) FROM LE_InviteRecord WHERE RecID={RecId})";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        public int GetInvitedMaxId(int UserId)
        {
            string strSql = $@"SELECT  MAX(RecID) FROM  dbo.LE_InviteRecord WHERE  InviteUserID={UserId}";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : (obj.ToString() == "" ? 0 : Convert.ToInt32(obj));
        }
        public int GetRedEvesStatus(int UserId, int RecId)
        {
            string strSql = $@"SELECT RedEvesStatus FROM  dbo.LE_InviteRecord WHERE  InviteUserID={UserId } and  RecID={RecId}";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : (obj.ToString() == "" ? 0 : Convert.ToInt32(obj));
        }
        /// <summary>
        /// 更新被邀请人尚未完成分享的红包为待领取状态
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public int UpdateRedEves(int UserId)
        {
            string strSql = $"UPDATE LE_InviteRecord SET RedEvesStatus={(int)RedEvesStatusEnum.待领取} WHERE BeInvitedUserID={UserId} AND  RedEvesStatus={(int)RedEvesStatusEnum.尚未完成分享}";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
    }

}
