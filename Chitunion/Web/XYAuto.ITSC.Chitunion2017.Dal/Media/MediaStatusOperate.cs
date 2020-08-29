using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Media
{
    /// <summary>
    /// 媒体状态操作 2017-04-24 张立彬
    /// </summary>
    public class MediaStatusOperate : DataBase
    {
        public static readonly MediaStatusOperate Instance = new MediaStatusOperate();
        /// <summary>
        /// 媒体和AE删除媒体 2017-04-24 张立彬
        /// </summary>
        /// <param name="MediaTableName">媒体表名</param>
        /// <param name="MediaID">媒体ID</param>
        /// <param name="UserId">当前操作人ID</param>
        /// <param name="RoleIdList">角色</param>
        /// <returns></returns>
        public int UpdateAEorMediaRoleStatus(string MediaTableName, int MediaID, int UserId, string RoleIdList)
        {
            string strSql = "update " + MediaTableName + " set Status=-1 where MediaID=" + MediaID;
            if (RoleIdList.Contains("SYS001RL00005"))
            {
                strSql += "and  'SYS001RL00005' in (SELECT RoleID  FROM dbo.UserRole WHERE  UserID=" + MediaTableName + ".CreateUserID)";
            }
            else
            {
                strSql += " and CreateUserID=" + UserId;
            }
            if (MediaTableName == "Media_PCAPP")
            {
                strSql += ";update App_AdTemplate set Status=-1 where BaseMediaID=(select top 1 BaseMediaID from Media_PCAPP where MediaID=" + MediaID + " and CreateUserID=" + UserId + ")";
            }
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, strSql);
        }
        /// <summary>
        /// 运营和超级管理员删除媒体 2017-04-24 张立彬
        /// </summary>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="MediaID">媒体ID</param>
        /// <returns></returns>
        public int UpdateSuperStatus(int MediaType, int MediaID)
        {
            string MediaTableName = "Weixin_OAuth";
            string strSql = "";
            switch (MediaType)
            {
                case (int)EnumMediaType.WeChat:
                    MediaTableName = "Weixin_OAuth";
                    strSql = "; update Media_Weixin set Status=-1 where Number=(select WxNumber from Weixin_OAuth where  RecID=" + MediaID + ")";
                    break;
                case (int)EnumMediaType.APP:
                    MediaTableName = "Media_BasePCAPP";
                    strSql = ";update Media_PCAPP set Status=-1 WHERE Name=(SELECT TOP 1 Name FROM dbo.Media_BasePCAPP WHERE RecID=" + MediaID + ")";
                    strSql += ";update App_AdTemplate set Status=-1 where BaseMediaID=" + MediaID;
                    break;
            }
            strSql += ";update " + MediaTableName + " set Status=-1 where RecID=" + MediaID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, strSql);
        }

        /// <summary>
        /// 2017-06-06 zlb
        /// 删除没有上架的刊例 
        /// </summary>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="MediaID">媒体ID</param>
        /// <param name="Wx_Status">刊例状态</param>
        public void DeletePublishByMediaId(int MediaType, int MediaID, int Wx_Status)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE Publish_BasicInfo SET  IsDel=-1 WHERE MediaType={0} AND MediaID={1} AND Wx_Status!={2}", MediaType, MediaID, Wx_Status);
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sb.ToString());
        }

        /// <summary>
        /// 2017-06-06 zlb
        /// 查询媒体下上架刊例数量
        /// </summary>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="MediaID">媒体ID</param>
        /// <param name="Wx_Status">刊例状态</param>
        /// <returns></returns>
        public int SelectPulishCount(int MediaType, int MediaID, int Wx_Status)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select count(1) from Publish_BasicInfo WHERE MediaType={0} AND MediaID={1} AND Wx_Status={2}", MediaType, MediaID, Wx_Status);
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, System.Data.CommandType.Text, sb.ToString());
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 根据基表ID删除刊例
        /// </summary>
        /// <returns></returns>
        public int DeletePublishByBaseMID(int MediaType, int BaseMID, int Wx_Status)
        {
            string strSql = "(select MediaID FROM Media_Weixin where Number=(select WxNumber from Weixin_OAuth where  RecID=" + BaseMID + "))";
            switch (MediaType)
            {
                case (int)EnumMediaType.WeChat:
                    strSql = "(select MediaID FROM Media_Weixin where Number=(select WxNumber from Weixin_OAuth where  RecID=" + BaseMID + "))";
                    break;
                case (int)EnumMediaType.APP:
                    strSql = "(SELECT  MediaID FROM Media_PCAPP  WHERE Name = (SELECT TOP 1 Name FROM dbo.Media_BasePCAPP WHERE RecID = " + BaseMID + "))";
                    break;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE Publish_BasicInfo SET  IsDel=-1 WHERE MediaType={0} AND MediaID in {1} AND Wx_Status!={2}", MediaType, strSql, Wx_Status);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sb.ToString());
        }


        /// <summary>
        /// 上下架媒体 2017-04-24 张立彬
        /// </summary>
        /// <param name="MediaType"></param>
        /// <param name="MediaID"></param>
        /// <param name="PublishState"></param>
        /// <returns></returns>
        public int UpdatePublishStatus(int MediaType, int MediaID, int PublishState)
        {
            string StrSql = "update Media_Weixin set PublishStatus=" + PublishState + " where MediaID=" + MediaID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, StrSql);
        }
        /// <summary>
        /// 审核媒体 2017-04-25 张立彬
        /// </summary>
        /// <param name="MediaID"></param>
        /// <param name="MediaType"></param>
        /// <param name="RejectMsg"></param>
        /// <param name="Status"></param>
        /// <param name="CreateUserID"></param>
        /// <returns></returns>
        public int ToExamineWxMedia(int MediaID, int MediaType, string RejectMsg, int Status, int CreateUserID)
        {
            SqlParameter[] parameters = {
                new SqlParameter ("@MediaType",SqlDbType.Int),
                new SqlParameter("@MediaID",SqlDbType.Int),
                new SqlParameter("@Status",SqlDbType.Int),
                new SqlParameter("@RejectMsg",SqlDbType.VarChar,200),
                new SqlParameter("@CreateUserID",SqlDbType.Int),
                new SqlParameter("@CreateTime ",SqlDbType.DateTime),
                new SqlParameter("@Result",SqlDbType.Int)
            };
            parameters[0].Value = MediaType;
            parameters[1].Value = MediaID;
            parameters[2].Value = Status;
            parameters[3].Value = RejectMsg;
            parameters[4].Value = CreateUserID;
            parameters[5].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            parameters[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ToExamineMediaInfo", parameters);
            return Convert.ToInt32(parameters[6].Value);
        }
        /// <summary>
        /// 2017-06-06 zlb
        /// 查询待审核的媒体
        /// </summary>
        /// <param name="AuditStatus">43001待审核 43002 审核通过 43003 驳回</param>
        /// <param name="MediaTableName">媒体表名</param>
        /// <returns></returns>
        public int SelectNextMediaID(int AuditStatus, string MediaTableName)
        {
            string StrSql = "select top 1 MediaID from " + MediaTableName + " where  Status=0 and AuditStatus=" + AuditStatus;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, System.Data.CommandType.Text, StrSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// 2017-06-06 zlb
        /// 审核APP媒体
        /// </summary>
        /// <param name="MediaID">媒体ID</param>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="RejectMsg">驳回理由</param>
        /// <param name="AuditStatus"> 43002 审核通过 43003 驳回</param>
        /// <param name="CreateUserID">审核人</param>
        /// <returns></returns>
        public int ToExamineAppMedia(int MediaID, int MediaType, string RejectMsg, int AuditStatus, int CreateUserID)
        {
            SqlParameter[] parameters = {
                new SqlParameter ("@MediaType",SqlDbType.Int),
                new SqlParameter("@MediaID",SqlDbType.Int),
                new SqlParameter("@Status",SqlDbType.Int),
                new SqlParameter("@RejectMsg",SqlDbType.VarChar,200),
                new SqlParameter("@CreateUserID",SqlDbType.Int),
                new SqlParameter("@CreateTime ",SqlDbType.DateTime),
                new SqlParameter("@Result",SqlDbType.Int)
            };
            parameters[0].Value = MediaType;
            parameters[1].Value = MediaID;
            parameters[2].Value = AuditStatus;
            parameters[3].Value = RejectMsg;
            parameters[4].Value = CreateUserID;
            parameters[5].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            parameters[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ToExamineAppMediaInfo", parameters);
            return Convert.ToInt32(parameters[6].Value);
        }

    }
}
