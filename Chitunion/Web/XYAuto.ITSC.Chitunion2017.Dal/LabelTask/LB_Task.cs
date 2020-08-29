using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.LabelTask;
using XYAuto.ITSC.Chitunion2017.Entities.LabelTask.DTO;
using XYAuto.Utils.Data;
using static XYAuto.ITSC.Chitunion2017.Entities.LabelTask.ENUM;

namespace XYAuto.ITSC.Chitunion2017.Dal.LabelTask
{
    public class LB_Task : DataBase
    {
        public static readonly LB_Task Instance = new LB_Task();
        #region Insert
        public int Insert(Entities.LabelTask.LB_Task model, out string msg)
        {
            msg = string.Empty;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Msg",SqlDbType.VarChar,200),
                new SqlParameter("@GenerateTaskCount",SqlDbType.Int,4),
                new SqlParameter("@TaskCount",model.TaskCount),
                new SqlParameter("@ProjectID",model.ProjectID),
                new SqlParameter("@MediaType",model.MediaType),
                new SqlParameter("@MediaNum",model.MediaNum),
                new SqlParameter("@MediaName",model.MediaName),
                new SqlParameter("@CreateUserID",model.CreateUserID)
            };

            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_LBTask_Insert", parameters);
            msg = (string)parameters[0].Value;
            return (int)parameters[1].Value;
        }
        #endregion
        public void SyncData(DataTable sourceTable, string tableName, out string errorMsg, int batchSize = 10000)
        {
            errorMsg = string.Empty;
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlBulkCopyByDataTable(conn, trans, tableName, sourceTable, batchSize);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        //trans.Rollback();
                        errorMsg = ex.Message;
                    }
                }
            }
        }
        /// <summary>
        /// zlb 2017-08-08
        /// 打标签
        /// </summary>
        /// <param name="ReqDTO"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int InserLableTakeInfo(ReqLableTaskDTO ReqDTO, int UserID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT  INTO dbo.LB_TaskLabel (TaskID,DictType,IsCustom,DictId,Name,CreateTime,CreateUserID) VALUES ");
            DateTime dtNow = DateTime.Now;
            foreach (var item in ReqDTO.CategoryInfo)
            {
                sb.AppendFormat("({0},{1},{2},{3},'{4}','{5}',{6}),", ReqDTO.TaskID, (int)item.DictType, 0, item.DictId, SqlFilter(item.DictName), dtNow, UserID);
            }
            if (ReqDTO.SceneInfo != null)
            {
                foreach (var item in ReqDTO.SceneInfo)
                {
                    sb.AppendFormat("({0},{1},{2},{3},'{4}','{5}',{6}),", ReqDTO.TaskID, (int)item.DictType, 0, item.DictId, SqlFilter(item.DictName), dtNow, UserID);
                }
            }
            if (ReqDTO.CustomSceneInfo != null)
            {
                foreach (var item in ReqDTO.CustomSceneInfo)
                {
                    sb.AppendFormat("({0},{1},{2},{3},'{4}','{5}',{6}),", ReqDTO.TaskID, (int)item.DictType, 1, 0, SqlFilter(item.DictName), dtNow, UserID);
                }
            }
            //foreach (var item in ReqDTO.CustomLableInfo)
            //{
            //    sb.AppendFormat("({0},{1},{2},{3},'{4}','{5}',{6}),", ReqDTO.TaskID, (int)item.DictType, 1, 0, SqlFilter(item.DictName), dtNow, UserID);
            //}
            string strSql = sb.ToString();
            strSql = strSql.Remove(strSql.LastIndexOf(","), 1);
            int result = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
            if (result > 0)
            {
                InserLableIpInfo(ReqDTO.IPInfo, ReqDTO.TaskID, dtNow, UserID);
            }
            return result;
        }
        /// <summary>
        /// zlb 2017-08-08
        /// 插入标签：IP和子IP
        /// </summary>
        /// <param name="IpList"></param>
        /// <param name="TaskID"></param>
        /// <param name="dtNow"></param>
        /// <param name="UserID"></param>
        public void InserLableIpInfo(List<LableIP> IpList, int TaskID, DateTime dtNow, int UserID)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in IpList)
            {
                string strSqlIP = "INSERT  INTO dbo.LB_TaskLabel (TaskID,DictType,IsCustom,DictId,Name,CreateTime,CreateUserID) VALUES ";
                strSqlIP += string.Format("({0},{1},{2},{3},'{4}','{5}',{6});SELECT @@Identity", TaskID, (int)item.DictType, 0, item.DictId, SqlFilter(item.DictName), dtNow, UserID);
                object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSqlIP);
                int lableID = obj == null ? 0 : Convert.ToInt32(obj);
                if (lableID > 0)
                {
                    if (item.SonIP != null)
                    {
                        foreach (var itemSonIP in item.SonIP)
                        {
                            string strSqlSonIP = "INSERT  INTO dbo.LB_IPSubLabel (LabelID,Name,CreateTime,CreateUserID,DictId) VALUES ";
                            strSqlSonIP += string.Format("({0},'{1}','{2}',{3},{4});SELECT @@Identity", lableID, SqlFilter(itemSonIP.DictName), dtNow, UserID, itemSonIP.DictId);
                            object objSonip = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSqlSonIP);
                            int SonipID = objSonip == null ? 0 : Convert.ToInt32(objSonip);
                            if (SonipID > 0)
                            {
                                if (itemSonIP.CustomLableInfo != null)
                                {
                                    foreach (var itemIPTag in itemSonIP.CustomLableInfo)
                                    {
                                        sb.Append("INSERT  INTO dbo.LB_SonIPLabel (SonIpID,Name,CreateTime,CreateUserID) VALUES ");
                                        sb.AppendFormat("({0},'{1}','{2}',{3});", SonipID, itemIPTag.DictName, dtNow, UserID);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sb.ToString());
            }
        }
        /// <summary>
        /// zlb 2017-08-08
        /// 查询该用户对应的任务打标签的数量（验证是否已打标签）
        /// </summary>
        /// <param name="TaskID">任务ID</param>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public int SelectSelfLableTaskeCount(int TaskID, int UserID)
        {
            string strSql = $"SELECT COUNT(1) FROM LB_TaskAssign WHERE TaskID=  { TaskID }  AND CreateUserID= { UserID} AND Status!=" + (int)EnumTaskStatus.待打标签;
            Object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// zlb 2017-08-08
        /// 查询该用户是否领取该任务
        /// </summary>
        /// <param name="TaskID">任务ID</param>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public int SelectSelfTaskCount(int TaskID, int UserID)
        {
            string strSql = "SELECT COUNT(1) FROM LB_TaskAssign WHERE TaskID=" + TaskID + " AND CreateUserID=" + UserID;
            Object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// zlb 2017-08-08
        /// 查询该任务下打标签人的数量
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public int SelectLableTaskeCount(int TaskID, int Status)
        {
            string strSql = $"SELECT COUNT(1) FROM LB_TaskAssign  WHERE  TaskID = {TaskID } AND Status={Status} ";
            Object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// zlb 2017-08-08
        /// 查询对应角色下用户数量
        /// </summary>
        /// <param name="RoleID">角色ID</param>
        /// <returns></returns>
        public int SlelectUserCountByRoleID(string RoleID)
        {
            string strSql = "SELECT COUNT(1) FROM dbo.UserRole R INNER JOIN dbo.UserInfo U ON R.UserID=U.UserID WHERE U.Status=0 AND R.status=0 AND RoleID='" + RoleID + "'";
            Object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// zlb 2017-08-08
        /// 修改任务状态
        /// </summary>
        /// <returns></returns>
        public int UpdateTaskStatus(int TaskID, int Status, int KeyWord, int Summary)
        {
            string strSql = "UPDATE LB_Task SET Status=" + Status;
            if (Status != 63006)
            {
                strSql += ",SubmitTime='" + DateTime.Now + "' ";
            }
            else
            {
                strSql += ",KeyWord=" + KeyWord + ",Summary=" + Summary;
            }
            strSql += " WHERE TaskID=" + TaskID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// zlb 2017-08-16
        /// 修改任务状态
        /// </summary>
        /// <returns></returns>
        public int UpdateTaskKeyAndSummary(int TaskID, int KeyWord, int Summary)
        {
            string strSql = "UPDATE LB_Task SET KeyWord=" + KeyWord + ",Summary=" + Summary + " WHERE TaskID=" + TaskID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// zlb 2017-08-08
        /// 修改项目状态
        /// </summary>
        /// <returns></returns>
        public int UpdateProjectStatus(int ProjectID, int Status)
        {
            string strSql = "UPDATE LB_Project SET Status=" + Status + " WHERE IsDeleted=0 and Status!=62004 and ProjectID=" + ProjectID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// zlb 2017-08-08
        /// 修改任务审核时间
        /// </summary>
        /// <returns></returns>
        public int UpdateTaskAuditTime(int TaskID, int CreateUserID)
        {
            string strSql = "UPDATE LB_Task SET AuditUserID=" + CreateUserID + ",AuditTime='" + DateTime.Now + "' WHERE TaskID=" + TaskID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// zlb 2017-08-08
        /// 修改任务分配表状态
        /// </summary>
        /// <returns></returns>
        public int UpdateTaskAssignStatus(int TaskID, int Status, int Summary, int UserID)
        {
            string strSql = "UPDATE LB_TaskAssign SET  Status=" + Status;
            if (Status == (int)EnumTaskStatus.已打标签)
            {
                strSql += ",Summary =" + Summary;
            }
            strSql += " WHERE TaskID=" + TaskID;
            if (Status == (int)EnumTaskStatus.已打标签)
            {
                strSql += " and CreateUserID=" + UserID;
            }
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// zlb 2017-08-16
        /// 审核通过
        /// </summary>
        /// <returns></returns>
        public int UpdateTaskAssignStatus(int TaskID, int Status)
        {
            string strSql = "UPDATE LB_TaskAssign SET Status=" + Status + " WHERE TaskID=" + TaskID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// zlb 2017-08-08
        /// 查询项目ID
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public int SelectProjectID(int TaskID)
        {
            string strSql = "SELECT top 1 ProjectID FROM LB_Task  WHERE TaskID= " + TaskID;
            Object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// zlb 2017-08-08
        /// 查询项目项目下未审核且未通过的媒体数量
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public int SelectTaskCount(int ProjectID)
        {
            string strSql = "SELECT count(1) FROM LB_Task  WHERE Status!=63006 and ProjectID=" + ProjectID;
            Object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }



        /// <summary>
        /// zlb 2017-08-08
        /// 插入操作日志
        /// </summary>
        /// <param name="TaskID">任务ID</param>
        /// <param name="UserID">创建人ID</param>
        /// <param name="OptType">操作类型（64001:提交，64002：审核，64003：修改）</param>
        /// <param name="OptContent">内容</param>
        /// <returns></returns>
        public int InsertTaskOperateInfo(int TaskID, int UserID, int OptType, string OptContent)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@TaskID", SqlDbType.Int),
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@OptType", SqlDbType.Int),
                    new SqlParameter("@OptContent", SqlDbType.Text),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime)

                    };
            parameters[0].Value = TaskID;
            parameters[1].Value = UserID;
            parameters[2].Value = OptType;
            parameters[3].Value = OptContent;
            parameters[4].Value = DateTime.Now;
            string strSql = "insert into LB_TaskOperateInfo (TaskID,OptType,OptContent,CreateTime,CreateUserID) values (@TaskID,@OptType,@OptContent,@CreateTime,@UserID)";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
        }

        /// <summary>
        /// zlb 2017-08-08
        /// 查询媒体或文章信息
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public DataTable SelectMediaOrArticleInfo(ENUM.EnumMediaType MediaType, ENUM.EnumProjectType ProjectType, int TaskID)
        {
            string strSql = "";
            if (ProjectType == ENUM.EnumProjectType.媒体)
            {
                switch (MediaType)
                {
                    case ENUM.EnumMediaType.微信:
                        strSql = "SELECT T.MediaType,W.NickName AS MediaName,T.MediaNum AS MediaOrArticle,W.HeadImg,T.ArticleID,A.Content,T.KeyWord,T.Summary FROM LB_Task T INNER JOIN dbo.Weixin_OAuth W ON T.MediaNum=W.WxNumber Left JOIN LB_ArticleInfo A ON T.ArticleID=A.ArticleID";
                        break;
                    case ENUM.EnumMediaType.APP:
                        strSql = "SELECT T.MediaType,W.Name AS MediaName,W.Name as MediaOrArticle,W.HeadIconURL AS HeadImg,T.ArticleID,A.Content,T.KeyWord,T.Summary FROM LB_Task T INNER JOIN dbo.Media_BasePCAPP W ON T.MediaNum = W.Name Left JOIN LB_ArticleInfo A ON T.ArticleID=A.ArticleID ";
                        break;
                    case ENUM.EnumMediaType.微博:
                        strSql = "SELECT T.MediaType,W.Name AS MediaName,T.MediaNum AS MediaOrArticle,W.HeadIconURL AS HeadImg,T.ArticleID,A.Content,T.KeyWord,T.Summary FROM LB_Task T INNER JOIN dbo.Media_Weibo W ON T.MediaNum=W.Number Left JOIN LB_ArticleInfo A ON T.ArticleID=A.ArticleID";
                        break;
                    case ENUM.EnumMediaType.视频:
                        strSql = "SELECT T.MediaType,W.Name AS MediaName,T.MediaNum AS MediaOrArticle,W.HeadIconURL AS HeadImg,T.ArticleID,A.Content,T.KeyWord,T.Summary FROM LB_Task T INNER JOIN dbo.Media_Video W ON T.MediaNum=W.Number Left JOIN LB_ArticleInfo A ON T.ArticleID=A.ArticleID";
                        break;
                    case ENUM.EnumMediaType.直播:
                        strSql = "SELECT T.MediaType,W.Name AS MediaName,T.MediaNum AS MediaOrArticle,W.HeadIconURL AS HeadImg,T.ArticleID,A.Content,T.KeyWord,T.Summary FROM LB_Task T INNER JOIN dbo.Media_Broadcast W ON T.MediaNum=W.Number Left JOIN LB_ArticleInfo A ON T.ArticleID=A.ArticleID";
                        break;
                    case ENUM.EnumMediaType.头条:
                        strSql = "SELECT T.MediaType,MediaName,T.MediaNum AS MediaOrArticle,HeadImg='',T.ArticleID,A.Content,T.KeyWord,T.Summary FROM  LB_Task T Left JOIN LB_ArticleInfo A ON T.ArticleID=A.ArticleID";
                        break;
                }
            }
            else
            {
                strSql = "SELECT T.MediaType,MediaName,T.ArticleTitle AS MediaOrArticle,HeadImg='',T.ArticleID,A.Content,T.KeyWord,T.Summary FROM  LB_Task T Left JOIN LB_ArticleInfo A ON T.ArticleID=A.ArticleID";
            }

            strSql += " WHERE TaskID=" + TaskID;
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// zlb 2017-08-08
        /// 根据任务ID查询媒体类型
        /// </summary>
        /// <param name="TaskID">任务ID</param>
        /// <returns></returns>
        public DataTable SelectMediaTypeByTaskID(int TaskID)
        {
            string strSql = "SELECT top 1  MediaType,P.ProjectType FROM LB_Task T INNER JOIN dbo.LB_Project P ON T.ProjectID=P.ProjectID WHERE T.TaskID=" + TaskID;
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];

        }
        /// <summary>
        /// zlb 2017-08-09
        /// 查看未审核的媒体或文章标签信息
        /// </summary>
        /// <param name="TaskID">任务ID</param>
        /// <param name="UserID">打标签人ID</param>
        /// <returns></returns>
        public DataSet SelectTaskLableInfo(int TaskID, int UserID)
        {
            string strSql = "SELECT T.LabelID,DictType,IsCustom,DictId,Name,T.CreateUserID,U.SysName as  Creater  FROM LB_TaskLabel T LEFT JOIN v_UserInfo U ON T.CreateUserID=U.UserID WHERE T.TaskID=" + TaskID;
            if (UserID > 0)
            {
                strSql += " AND T.CreateUserID=" + UserID;
            }
            strSql += " SELECT I.RecID AS SonIpID,I.LabelID,I.DictId,I.Name,T.CreateUserID,U.SysName as Creater FROM LB_IPSubLabel I INNER JOIN  LB_TaskLabel T ON I.LabelID=T.LabelID LEFT JOIN v_UserInfo U ON I.CreateUserID=U.UserID WHERE T.TaskID=" + TaskID;
            if (UserID > 0)
            {
                strSql += " AND T.CreateUserID=" + UserID;
            }
            strSql += " SELECT S.SonIpID,S.Name,T.CreateUserID,U.SysName as Creater FROM LB_IPSubLabel I INNER JOIN  LB_TaskLabel T ON I.LabelID=T.LabelID INNER JOIN LB_SonIPLabel S ON S.SonIpID=I.RecID LEFT JOIN v_UserInfo U ON I.CreateUserID=U.UserID WHERE T.TaskID=" + TaskID;
            if (UserID > 0)
            {
                strSql += " AND T.CreateUserID=" + UserID;
                strSql += " SELECT TOP 1 Summary  FROM LB_TaskAssign WHERE   TaskID=" + TaskID + " AND CreateUserID=" + UserID + " ORDER  BY CreateTime desc;";
                strSql += $" SELECT TOP 1 U.SysName AS Creater,T.CreateTime FROM dbo.LB_TaskOperateInfo T LEFT JOIN v_UserInfo U ON T.CreateUserID = U.UserID WHERE T.CreateUserID={UserID} and TaskID = { TaskID } AND OptType = 64001  ORDER BY T.CreateTime DESC;";
                strSql += $" SELECT TOP 1 U.SysName AS Creater,T.CreateTime FROM dbo.LB_TaskOperateInfo T LEFT JOIN v_UserInfo U ON T.CreateUserID = U.UserID WHERE TaskID = { TaskID } AND OptType = 64002 ORDER BY T.CreateTime DESC; ";
            }

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// zlb 2017-08-09
        /// 查看已审核的媒体或文章标签信息
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public DataSet SelectTaskAuditLableInfo(int TaskID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT T.RecID AS LabelID,DictType,IsCustom,DictId,Name,U.SysName as Creater  FROM LB_TaskAuditPassed T LEFT JOIN v_UserInfo U ON T.CreateUserID=U.UserID WHERE T.TaskID={0};

            SELECT I.RecID AS SonIpID,I.AuditLabelID AS LabelID,I.DictId, I.Name, U.SysName as Creater FROM LB_TaskAudit_IPSubLabel I INNER JOIN  LB_TaskAuditPassed T ON I.AuditLabelID = T.RecID LEFT JOIN v_UserInfo U ON I.CreateUserID = U.UserID WHERE T.TaskID = {0};

             SELECT S.AuditSonIpID AS SonIpID,S.Name,T.CreateUserID,U.SysName as Creater FROM LB_TaskAudit_IPSubLabel I INNER JOIN  LB_TaskAuditPassed T ON I.AuditLabelID=T.RecID INNER JOIN LB_TaskAudit_SonIPLabel S ON S.AuditSonIpID=I.RecID LEFT JOIN v_UserInfo U ON I.CreateUserID=U.UserID WHERE T.TaskID={0};

            SELECT U.SysName AS Creater,T.CreateTime FROM dbo.LB_TaskOperateInfo T LEFT JOIN v_UserInfo U ON T.CreateUserID = U.UserID WHERE TaskID = {0} AND OptType = 64001 ORDER BY T.CreateTime DESC;

            SELECT TOP 1 U.SysName AS Creater,T.CreateTime FROM dbo.LB_TaskOperateInfo T LEFT JOIN v_UserInfo U ON T.CreateUserID = U.UserID WHERE TaskID = {0} AND OptType = 64002 ORDER BY T.CreateTime DESC;

            SELECT TOP 2 U.SysName AS Creater,T.CreateTime,OptContent FROM dbo.LB_TaskOperateInfo T LEFT JOIN v_UserInfo U ON T.CreateUserID = U.UserID  WHERE TaskID = {0} AND(OptType = 64002 OR OptType = 64003) ORDER BY T.CreateTime DESC;", TaskID);

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sb.ToString());
        }
        /// <summary>
        /// zlb 2017-08-11
        /// 根据任务ID查询项目类型
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public int SelectProjectTypeByTaskID(int TaskID, int Status)
        {
            string strSql = $"SELECT TOP 1 ProjectType FROM LB_Project WHERE  ProjectID=(SELECT TOP 1 ProjectID FROM dbo.LB_Task WHERE Status={Status} AND TaskID={TaskID})";
            Object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// zlb 2017-08-11
        /// 根据任务ID查询项目ID
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="DictType"></param>
        /// <returns></returns>
        public DataTable SelectLableInfoByTaskID(int TaskID, int DictType)
        {
            string strSql = $"SELECT RecID FROM LB_TaskAuditPassed WHERE TaskID={TaskID} AND  DictType={DictType}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        public int ExmainLableInfo(ReqLableTaskDTO ReqDTO, string AuditLabelID, int ProjectType, int UserID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" DELETE FROM  LB_TaskAuditPassed WHERE TaskID={0}", ReqDTO.TaskID);
            if (!string.IsNullOrEmpty(AuditLabelID))
            {
                sb.AppendFormat(" DELETE FROM  LB_TaskAudit_SonIPLabel WHERE AuditSonIpID IN( SELECT RecID FROM  LB_TaskAudit_IPSubLabel WHERE  AuditLabelID IN ({0}))", AuditLabelID);
                sb.AppendFormat(" DELETE FROM  LB_TaskAudit_IPSubLabel WHERE AuditLabelID IN({0})", AuditLabelID);
            }
            sb.Append(" INSERT  INTO dbo.LB_TaskAuditPassed (TaskID,DictType,IsCustom,DictId,Name,CreateTime,CreateUserID,ProjectType) VALUES ");
            DateTime dtNow = DateTime.Now;
            foreach (var item in ReqDTO.CategoryInfo)
            {
                sb.AppendFormat("({0},{1},{2},{3},'{4}','{5}',{6},{7}),", ReqDTO.TaskID, (int)item.DictType, 0, item.DictId, SqlFilter(item.DictName), dtNow, UserID, ProjectType);
            }
            if (ReqDTO.SceneInfo != null)
            {
                foreach (var item in ReqDTO.SceneInfo)
                {
                    sb.AppendFormat("({0},{1},{2},{3},'{4}','{5}',{6},{7}),", ReqDTO.TaskID, (int)item.DictType, 0, item.DictId, SqlFilter(item.DictName), dtNow, UserID, ProjectType);
                }
            }
            if (ReqDTO.CustomSceneInfo != null)
            {
                foreach (var item in ReqDTO.CustomSceneInfo)
                {
                    sb.AppendFormat("({0},{1},{2},{3},'{4}','{5}',{6},{7}),", ReqDTO.TaskID, (int)item.DictType, 1, 0, SqlFilter(item.DictName), dtNow, UserID, ProjectType);
                }
            }
            //foreach (var item in ReqDTO.CustomLableInfo)
            //{
            //    sb.AppendFormat("({0},{1},{2},{3},'{4}','{5}',{6},{7}),", ReqDTO.TaskID, (int)item.DictType, 1, 0, SqlFilter(item.DictName), dtNow, UserID, ProjectType);
            //}
            string strSql = sb.ToString();
            strSql = strSql.Remove(strSql.LastIndexOf(","), 1);
            int result = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
            if (result > 0)
            {
                InserAuditLableIpInfo(ReqDTO.IPInfo, ReqDTO.TaskID, dtNow, UserID, ProjectType);
            }
            return result;
        }
        /// <summary>
        /// zlb 2017-08-08
        /// 插入标签：IP和子IP
        /// </summary>
        /// <param name="IpList"></param>
        /// <param name="TaskID"></param>
        /// <param name="dtNow"></param>
        /// <param name="UserID"></param>
        public void InserAuditLableIpInfo(List<LableIP> IpList, int TaskID, DateTime dtNow, int UserID, int ProjectType)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in IpList)
            {
                string strSqlIP = "INSERT  INTO dbo.LB_TaskAuditPassed (TaskID,DictType,IsCustom,DictId,Name,CreateTime,CreateUserID,ProjectType) VALUES ";
                strSqlIP += string.Format("({0},{1},{2},{3},'{4}','{5}',{6},{7});SELECT @@Identity", TaskID, (int)item.DictType, 0, item.DictId, SqlFilter(item.DictName), dtNow, UserID, ProjectType);
                object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSqlIP);
                int lableID = obj == null ? 0 : Convert.ToInt32(obj);
                if (lableID > 0)
                {
                    if (item.SonIP != null)
                    {
                        foreach (var itemSonIP in item.SonIP)
                        {
                            string strSqlSonIP = "INSERT  INTO dbo.LB_TaskAudit_IPSubLabel (AuditLabelID,Name,CreateTime,CreateUserID,DictId) VALUES ";
                            strSqlSonIP += string.Format("({0},'{1}','{2}',{3},{4});SELECT @@Identity", lableID, SqlFilter(itemSonIP.DictName), dtNow, UserID, itemSonIP.DictId);
                            object objSonip = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSqlSonIP);
                            int SonipID = objSonip == null ? 0 : Convert.ToInt32(objSonip);
                            if (SonipID > 0)
                            {
                                if (itemSonIP.CustomLableInfo != null)
                                {
                                    foreach (var itemIPTag in itemSonIP.CustomLableInfo)
                                    {
                                        sb.Append("INSERT  INTO dbo.LB_TaskAudit_SonIPLabel (AuditSonIpID,Name,CreateTime,CreateUserID) VALUES ");
                                        sb.AppendFormat("({0},'{1}','{2}',{3});", SonipID, itemIPTag.DictName, dtNow, UserID);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sb.ToString());
            }
        }
        /// <summary>
        /// zlb 2017-08-28
        /// 查询标签信息
        /// </summary>
        /// <param name="LableType">标签类型</param>
        /// <returns></returns>
        public DataTable SelectLableInfo(int LableType)
        {
            string strSql = $"SELECT TitleID AS DictId,Name AS DictName FROM TitleBasicInfo WHERE  Status=0 AND Type={LableType}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }

        /// <summary>
        /// zlb 2017-08-29
        /// 查询子Ip信息
        /// </summary>
        /// <param name="IpId">父IpId</param>
        /// <returns></returns>
        public DataTable SelectSonIpInfo(int IpId)
        {
            string strSql = $"SELECT T.TitleID AS DictId,Name AS DictName FROM TitleBasicInfo T INNER JOIN  dbo.IPTitleInfo I ON T.TitleID=I.SubIP  WHERE I.PIP={IpId} AND T.Type=65005 GROUP BY T.TitleID,Name";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        public DataTable SelectTitleBasicInfo(string ListLableName)
        {
            string strSql = $"SELECT T.TitleID,T.Name  FROM TitleBasicInfo T WHERE T.Name in ({ListLableName}) and Type={65004}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }

        public int InsertTitleBasicInfo(List<string> listLableName, int UserID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT  INTO dbo.TitleBasicInfo (Name,Type,Status,CreateTime,CreateUserID) VALUES ");
            DateTime dtNow = DateTime.Now;
            foreach (var item in listLableName)
            {
                sb.AppendFormat(" ('{0}',{1},{2},'{3}',{4}), ", SqlFilter(item), 65004, 0, dtNow, UserID);
            }
            string strSql = sb.ToString();
            strSql = strSql.Remove(strSql.LastIndexOf(","), 1);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        public int InsertIPTitleInfo(int PIP, int SubPIP, int UserID, DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT  INTO dbo.IPTitleInfo (PIP,SubIP,TitleID,Status,CreateTime,CreateUserID) VALUES ");
            DateTime dtNow = DateTime.Now;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.AppendFormat(" ({0},{1},{2},{3},'{4}',{5}), ", PIP, SubPIP, Convert.ToInt32(dt.Rows[i]["TitleID"]), 0, dtNow, UserID);
            }
            string strSql = sb.ToString();
            strSql = strSql.Remove(strSql.LastIndexOf(","), 1);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }

        public int DeleteIPTitleInfo(int PIP, int SubPIP, string ListLableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"DELETE FROM  IPTitleInfo WHERE PIP={PIP} AND SubIP={SubPIP} AND TitleID IN (SELECT TitleID FROM dbo.TitleBasicInfo WHERE type=65004  AND  Name IN ({ListLableName})) ");
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sb.ToString());
        }
    }
}
