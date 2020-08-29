using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.LabelTask.DTO;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LabelTask
{
    public class LabelTask : DataBase
    {
        #region 当前登录人UserID
        private int _currentUserID;
        public int currentUserID
        {
            get
            {
                try
                {
                    _currentUserID = Chitunion2017.Common.UserInfo.GetLoginUserID();
                }
                catch (Exception)
                {
                    _currentUserID = 1225;
                }
                return _currentUserID;
            }
        }
        private static string LoginPwdKey = Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");
        #endregion        
        public static readonly LabelTask Instance = new LabelTask();

        public List<ResponseAuditUserDTO> GetAuditUser()
        {
            string sqlstr = @"
                            SELECT  VUI.UserName ,
                                    VUI.SysName TrueName ,
                                    VUI.Mobile ,
                                    VUI.UserID
                            FROM    dbo.v_UserInfo VUI
                                    JOIN dbo.UserRole UR ON UR.UserID = VUI.UserID
                                                            AND UR.RoleID = 'SYS001RL00010'";
           
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
            return DataTableToList<ResponseAuditUserDTO>(ds.Tables[0]);
        }

        public int GetWxIDByWxNumber(string wxNumber)
        {
            string sql = @" --根据微信号查询是否存在
                            SELECT TOP 1
                                    RecID
                            FROM    Weixin_OAuth
                            WHERE   Status = 0
                                    AND WxNumber = @WxNumber";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@WxNumber",SqlFilter(wxNumber))
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        public int GetMediaIDByAPPName(string appName)
        {
            string sql = @" --根据APP名称查询是否存在
                            SELECT TOP 1
                                    RecID
                            FROM    Media_BasePCAPP
                            WHERE   Status = 0
                                    AND Name = @Name";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Name",SqlFilter(appName))
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        #region 查看媒体帐号是否已经生成任务
        public bool IsGeneratedTaskByMediaNum(string mediaNumber,int mediaType)
        {
            string sql = @" --根据微信号查询是否存在
                            SELECT  COUNT(1)
                            FROM    dbo.LB_Task LBT
                            JOIN dbo.LB_Project LBP ON LBT.ProjectID=LBP.ProjectID
                            WHERE   LBT.MediaType = @MediaType		
                                    AND LBT.MediaNum = @MediaNumber
		                            AND ((LBP.Status<>62004) OR(LBP.Status=62004 AND LBT.Status<>63001))";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MediaNumber",SqlFilter(mediaNumber)),
                new SqlParameter("@MediaType",mediaType)
            };
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return (int)ds.Tables[0].Rows[0][0] > 0;
        }
        #endregion
        #region 查询媒体文章未处理任务数
        public int StatisticsArticleCount(int mediaType, int resourceType,string mediaNumbers)
        {
            string sql = $@"--查询EXCEL媒体帐号文章未处理数量
                            SELECT  COUNT(1)
                            FROM    BaseData2017.dbo.ArticleInfo AIF
                            WHERE   AIF.Resource = @Resource
                                    AND AIF.DataId IN ( {mediaNumbers} )
                                    AND AIF.RecID NOT IN (
                                    SELECT  LBT.ArticleID
                                    FROM    dbo.LB_Task LBT
                                            JOIN dbo.LB_Project LBP ON LBT.ProjectID = LBP.ProjectID
                                    WHERE   LBT.MediaType = @MediaType
                                            AND LBP.ProjectType = 61002
                                            AND LBT.MediaNum IN ( {mediaNumbers} )
                                            AND LBT.Status IN(63005,63006))";
            SqlParameter[] parameters = new SqlParameter[]
            {
                //new SqlParameter("@MediaNumber",mediaNumbers),
                new SqlParameter("@MediaType",mediaType),
                 new SqlParameter("@Resource",resourceType)
            };
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return (int)ds.Tables[0].Rows[0][0];
        }
        #endregion
        #region 查询媒体帐号未处理任务数
        public int StatisticsMediaNumCount(int mediaType, string mediaNumbers)
        {
            //媒体：当前条件下未处理的数量 = 当前条件下从来没有打过标签的数量 + 当前条件下其中一个打过标签的数量
            //已处理数量即：任务状态为：63005待审、63006已审
            string sql = $@"--查询EXCEL媒体帐号已处理数量
                            SELECT  COUNT(1)
                            FROM    dbo.LB_Task LBT
                                    JOIN dbo.LB_Project LBP ON LBT.ProjectID = LBP.ProjectID
                            WHERE   LBT.MediaType = @MediaType
                                    AND LBP.ProjectType = 61001
                                    AND LBT.Status IN(63005,63006)
                                    AND LBT.MediaNum IN ( {mediaNumbers} )
                                    ";
            SqlParameter[] parameters = new SqlParameter[]
            {
                //new SqlParameter("@MediaNumber",mediaNumbers),
                new SqlParameter("@MediaType",mediaType)
            };
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return (int)ds.Tables[0].Rows[0][0];
        }
        public int p_StatisticsMediaNumCount(int mediaType, string mediaNumbers)
        {            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MediaNumbers",mediaNumbers),
                new SqlParameter("@MediaType",mediaType)
            };
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_StatisticsMediaNumCount", parameters);
            return (int)ds.Tables[0].Rows[0][0];
        }
        #endregion
        #region 查询标签任务列表
        public ResLabelListQueryDTO LabelListQuery(ReqLabelListQueryDTO query)
        {
            string orderBy = " LBT.CreateTime DESC ";
            string where = string.Empty;

            Common.UserRole currentRole = Chitunion2017.Common.UserInfo.GetUserRole();
            
            //数据权限
            //打标签人：只看分给自己数据
            //标签审核：只看待审、已审
            if (currentRole.IsLabelOpt)
            {
                where += $" AND LBTA.AssignUserID={currentUserID}";
            }                
            //else if(currentRole.IsLabelAudit)
            //    where += $" AND LBT.Status IN(63005,63006)";


            if (Enum.IsDefined(typeof(Entities.LabelTask.ENUM.EnumProjectType), query.projectType))
                where += $" AND LBP.ProjectType={query.projectType}";

            if (!string.IsNullOrEmpty(query.keyWord))
                where += $" AND (LBT.MediaNum LIKE '%{query.keyWord}%' OR LBT.MediaName LIKE '%{query.keyWord}%' OR LBT.ArticleTitle LIKE '%{query.keyWord}%')";

            if (Enum.IsDefined(typeof(Entities.LabelTask.ENUM.EnumTaskStatus), query.Status))
            {
                if(currentRole.IsLabelAudit)
                    where += $" AND LBT.Status={query.Status}";
                else if(currentRole.IsLabelOpt)
                    where += $" AND LBTA.Status={query.Status}";
            }                

            if (query.auditUserID != -2)
                where += $" AND	LBT.AuditUserID={query.auditUserID}";

            DateTime tmpDate = new DateTime(2000, 1, 1);
            if (query.submitBeginTime > tmpDate && query.submitEndTime > tmpDate)
                where += $" AND (LBT.SubmitTime BETWEEN '{query.submitBeginTime.ToString("yyyy-MM-dd")}' AND '{query.submitEndTime.ToString("yyyy-MM-dd") + " 23:59:59"}')";

            if (query.auditBeginTime > tmpDate && query.auditEndTime > tmpDate)
                where += $" AND (LBT.AuditTime BETWEEN '{query.auditBeginTime.ToString("yyyy-MM-dd")}' AND '{query.auditEndTime.ToString("yyyy-MM-dd") + " 23:59:59"}')";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@totalRecorder",SqlDbType.Int,4),
                new SqlParameter("@where",where),
                new SqlParameter("@order",orderBy),
                new SqlParameter("@pagesize",query.pageSize),
                new SqlParameter("@indexpage",query.pageIndex),
                new SqlParameter("@CurrentUserID",currentUserID),
                new SqlParameter("@Status",query.Status)
            };
            parameters[0].Direction = ParameterDirection.Output;
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_LabelListQuery", parameters);
            ResLabelListQueryDTO ret = new ResLabelListQueryDTO()
            {
                List = DataTableToList<ResLabelModelQueryDTO>(ds.Tables[0]),
                TotalCount = (int)parameters[0].Value
            };
            return ret;
        }
        #endregion
        #region 领取任务
        public List<ResLabelModelQueryDTO> LabelReceiveTask()
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Msg",SqlDbType.VarChar,200),
                new SqlParameter("@CurrentUserID",currentUserID)
            };
            parameters[0].Direction = ParameterDirection.Output;
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_LabelReceiveTask", parameters);
            
            return DataTableToList<ResLabelModelQueryDTO>(ds.Tables[0]);
        }
        #endregion
        #region 删除停止项目
        public void LabelProjectStatus(int projectID, int status)
        {
            string sql = @"UPDATE dbo.LB_Project SET Status=@Status WHERE ProjectID=@ProjectID";
            if(status==(int)Entities.LabelTask.ENUM.EnumProjectStatus.删除)
                sql = @"UPDATE dbo.LB_Project SET IsDeleted=@Status WHERE ProjectID=@ProjectID";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@ProjectID",projectID),
                new SqlParameter("@Status",status)
            };

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }
        #endregion
        #region 任务列表审核领取
        public int LabelTaskAuditQuery(int taskID,out string msg)
        {
            msg = string.Empty;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Msg",SqlDbType.VarChar,200),
                new SqlParameter("@NewTaskID",SqlDbType.Int,4),
                new SqlParameter("@TaskID",taskID),
                new SqlParameter("@CurrentUserID",currentUserID)
            };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Direction = ParameterDirection.Output;
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_LabelTaskAuditQuery", parameters);
            msg= (string)parameters[0].Value;
            return (int)parameters[1].Value;
        }
        #endregion
        #region 任务列表审核领取
        public int LabelTaskAuditCancel(int taskID)
        {
            string sql = @" --根据微信号查询是否存在
                            DELETE FROM dbo.LB_ReceiveAuditLable WHERE TaskID=@TaskID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TaskID",taskID)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }
        #endregion
    }
}
