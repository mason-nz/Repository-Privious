using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.ITSC.Chitunion2017.Dal.SysRight
{
    /// <summary>
    /// 2017-03-07 张立彬
    /// 用户数据权限
    /// </summary>
    public class UserDataPower : DataBase
    {
        /// <summary>
        /// 根据用户ID 查询所负责的用户ID集合
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="OrderState"></param>
        /// <returns></returns>
        public static string GetUseridListByUserID(int userID, int OrderState)
        {
            //todo:需产品确认是否判断授权给AE
            string strSql = "";
            if (OrderState == 16001)
            {
                strSql = "select  UserID  from UserInfo where IsAuthMTZ='True' and AuthAEUserID=" + userID;
            }
            else
            {
                strSql = "select  UserID from UserInfo where   AuthAEUserID=" + userID;
            }
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
            StringBuilder sb = new StringBuilder("(");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["UserID"] != DBNull.Value)
                    {
                        sb.Append(dt.Rows[i]["UserID"].ToString() + ",");
                    }
                }
            }
            sb.Append(userID + ")");
            return sb.ToString();
        }
        /// <summary>
        /// 根据 用户ID和子订单ID 查询字订单数量
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="subOrderNum">子订单号</param>
        /// <param name="mediaWhere">创建媒体条件</param>
        /// <returns></returns>
        public static int GetSubOrderCount(int userID, string subOrderNum)
        {
            string strSql = "select count(1)  from SubADInfo where  CreateUserID=" + userID + " and SubOrderID=@subOrderNum";
            SqlParameter[] parameters = {
                                    new SqlParameter("@subOrderNum", SqlDbType.NVarChar, 20)
                    };
            parameters[0].Value = subOrderNum;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// 查询订单媒体数量
        /// </summary>
        /// <param name="subOrderNum">订单号</param>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static int GetMediaSubOrder(string subOrderNum, int mediaType, int userID)
        {
            string mediaTable = "";
            switch (mediaType)
            {
                case 14001:
                    mediaTable = "Media_Weixin";
                    break;
                case 14002:
                    mediaTable = "Media_PCAPP";
                    break;
                case 14003:
                    mediaTable = "Media_Weibo";
                    break;
                case 14004:
                    mediaTable = "Media_Video";
                    break;
                case 14005:
                    mediaTable = "Media_Broadcast";
                    break;
                default:
                    break;
            }
            if (mediaTable == "")
            {
                return 0;
            }
            //媒体主不能创建订单，故不加查询用户ID条件 后期用到在加
            string strSql = "SELECT COUNT(1) FROM " + mediaTable + " M WHERE M.CreateUserID=@userID AND M.MediaID=(SELECT top 1 MediaID FROM SubADInfo S where S.SubOrderID=@subOrderNum)";
            SqlParameter[] parameters = {
                                    new SqlParameter("@subOrderNum", SqlDbType.VarChar, 20),
                                    new SqlParameter("@userID", SqlDbType.Int)
                    };
            parameters[0].Value = subOrderNum;
            parameters[1].Value = userID;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// 查询子订单单号
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="feedBackDataID">反馈数据ID</param>
        /// <returns></returns>
        public static string GetSubOrderID(int mediaType, int feedBackDataID)
        {
            string mediaTable = "";
            switch (mediaType)
            {
                case 14001:
                    mediaTable = "OrderFeedbackData_Weixin";
                    break;
                case 14002:
                    mediaTable = "OrderFeedbackData_PC";
                    break;
                case 14003:
                    mediaTable = "OrderFeedbackData_Weibo";
                    break;
                case 14004:
                    mediaTable = "OrderFeedbackData_Video";
                    break;
                case 14005:
                    mediaTable = "OrderFeedbackData_Live";
                    break;
                default:
                    break;
            }
            if (mediaTable == "")
            {
                return "";
            }
            //媒体主不能创建订单，故不加查询用户ID条件 后期用到在加
            string strSql = "SELECT SubOrderCode FROM " + mediaTable + "  WHERE RecID=" + feedBackDataID;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? "" : obj.ToString();
        }
        /// <summary>
        ///  增加和删除反馈数据权限验证
        /// </summary>
        /// <param name="subOrderID">订单ID</param>
        /// <param name="mediaType">媒体类型</param>
        /// <returns></returns>
        public static int FeedbackDataVerification(string subOrderID, int mediaType)
        {
            int userId = Common.UserInfo.GetLoginUserID();
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            if (!roleIdList.Contains("SYS001RL00004") && !roleIdList.Contains("SYS001RL00001") && !roleIdList.Contains("SYS001RL00005"))
            {

                //if (roleIdList.Contains("SYS001RL00005"))
                //{
                //    //string strSelectMediaID = "";
                //    //switch (mediaType)
                //    //{
                //    //    case 14001:
                //    //        strSelectMediaID = " (MediaID in (select MediaID from Media_Weixin where CreateUserID=" + userId + ") and MediaType=14001)";
                //    //        break;
                //    //    case 14002:
                //    //        strSelectMediaID = " (MediaID in (select MediaID from Media_PCAPP where CreateUserID=" + userId + ") and  MediaType=14002)";
                //    //        break;
                //    //    case 14003:
                //    //        strSelectMediaID = " (MediaID in (select MediaID from Media_Weibo where CreateUserID=" + userId + ") and  MediaType=14003)";
                //    //        break;
                //    //    case 14004:
                //    //        strSelectMediaID = " (MediaID in (select MediaID from Media_Video where CreateUserID=" + userId + ") and  MediaType=14004)";
                //    //        break;
                //    //    case 14005:
                //    //        strSelectMediaID = " (MediaID in (select MediaID from Media_Broadcast where CreateUserID=" + userId + ") and  MediaType=14005)";
                //    //        break;
                //    //    default:
                //    //        strSelectMediaID = " 1=2";
                //    //        break;
                //    //}
                //    // string userIdList = GetUseridListByUserID(userId, 0);
                //    int orderCount = UserDataPower.GetMediaSubOrder(subOrderID, mediaType, userId);
                //    if (orderCount <= 0)
                //    {
                //        return 0;
                //    }
                //    return 1;
                //}
                if (roleIdList.Contains("SYS001RL00003"))
                {
                    int orderCount = UserDataPower.GetMediaSubOrder(subOrderID, mediaType, userId);
                    if (orderCount <= 0)
                    {
                        return 0;
                    }
                    return 1;
                }
                else if (roleIdList.Contains("SYS001RL00008"))
                {
                    int orderCount = UserDataPower.GetSubOrderCount(userId, subOrderID);
                    if (orderCount <= 0)
                    {
                        return 0;
                    }
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            return 1;

        }

    }
}
