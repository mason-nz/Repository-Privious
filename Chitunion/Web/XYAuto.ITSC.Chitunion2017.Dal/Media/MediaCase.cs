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
    public class MediaCase : DataBase
    {
        public static readonly MediaCase Instance = new MediaCase();
        /// <summary>
        /// zlb
        /// 增加媒体案例
        /// </summary>
        /// <param name="CaseInfo">媒体案例信息</param>
        /// <param name="CreateUserID">创建人</param>
        /// <returns></returns>
        public int InsertMediaCaseInfo(XYAuto.ITSC.Chitunion2017.Entities.Media.MediaCase CaseInfo, int CreateUserID)
        {
            SqlParameter[] parameters = {
                new SqlParameter ("@MediaType",SqlDbType.Int),
                new SqlParameter("@MediaID",SqlDbType.Int),
                new SqlParameter("@MediaStatus",SqlDbType.Int),
                new SqlParameter("@CaseContent",SqlDbType.Text),
                new SqlParameter("@CreateUserID",SqlDbType.Int),
                new SqlParameter("@CreateTime ",SqlDbType.DateTime),
                new SqlParameter("@Result",SqlDbType.Int)
            };
            parameters[0].Value = (Int32)CaseInfo.MediaType;
            parameters[1].Value = CaseInfo.MediaID;
            parameters[2].Value = CaseInfo.CaseStatus;
            parameters[3].Value = CaseInfo.CaseContent.Trim();
            parameters[4].Value = CreateUserID;
            parameters[5].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            parameters[6].Value = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_InsertMediaCaseInfo", parameters);
            return Convert.ToInt32(parameters[6].Value);
        }

        /// <summary>
        /// zlb
        /// 根据媒体类型和ID,状态查询媒体案例
        /// </summary>
        /// <param name="MediaID">媒体ID</param>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="CaseStatus">0预览，1正式</param>
        /// <returns></returns>
        public DataTable SelectMediaCaseInfo(int MediaID, int MediaType, int CaseStatus, string mediaTableName)
        {
            string Number = "";
            if (mediaTableName == "Media_Weixin")
            {
                Number = ",Number";
            }
            else
            {
                Number = @",Number='APP'";
            }
            string sqlStr = "select Name" + Number + ",CaseContent from WeChat_Case WC inner join " + mediaTableName + " MW ON WC.MediaID=MW.MediaID  WHERE WC.MediaType=" + MediaType + " AND WC.MediaID=" + MediaID + " AND WC.Status=" + CaseStatus;
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr).Tables[0];
        }
        /// <summary>
        /// 2017-06-05 zlb
        /// 根基媒体类型和ID删除案例
        /// </summary>
        /// <param name="MediaID">媒体ID</param>
        /// <param name="MediaType">媒体类型</param>
        /// <returns>大于0删除成功;反之删除失败</returns>
        public int DeleteMediaCaseByMidAndMtype(int MediaID, int MediaType)
        {
            string sqlStr = "update WeChat_Case set Status=-1 where MediaType=" + MediaType + " and MediaID=" + MediaID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
        }

        /// <summary>
        /// 2017-06-05 zlb
        /// 根基媒体类型和ID删除案例
        /// </summary>
        /// <param name="BaseMID">基表ID</param>
        /// <param name="MediaType">媒体类型</param>
        /// <returns>大于0删除成功;反之删除失败</returns>
        public int DeleteCaseByBaseMidAndMtype(int BaseMID, int MediaType)
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
            string sqlStr1 = "update WeChat_Case set Status=-1 where MediaType=" + MediaType + " and MediaID in" + strSql;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr1);
        }
    }
}
