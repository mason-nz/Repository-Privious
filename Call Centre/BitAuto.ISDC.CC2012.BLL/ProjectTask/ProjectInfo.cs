using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Configuration;
using System.ComponentModel;
using System.Threading;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类ProjectInfo 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:28 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectInfo
    {
        public static readonly ProjectInfo Instance = new ProjectInfo();
        protected ProjectInfo() { }

        #region Select
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetProjectInfo(QueryProjectInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectInfo.Instance.GetProjectInfo(query, order, currentPage, pageSize, out totalCount);
        }
        public DataTable GetProjectInfo(QueryProjectInfo query, string order, int currentPage, int pageSize, out int totalCount, int userid)
        {
            return Dal.ProjectInfo.Instance.GetProjectInfo(query, order, currentPage, pageSize, out totalCount, userid);
        }
        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectInfo GetProjectInfo(long ProjectID)
        {
            return Dal.ProjectInfo.Instance.GetProjectInfo(ProjectID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ProjectInfo model)
        {
            return Dal.ProjectInfo.Instance.Insert(sqltran, model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ProjectInfo model)
        {
            return Dal.ProjectInfo.Instance.Update(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ProjectInfo model)
        {
            return Dal.ProjectInfo.Instance.Update(sqltran, model);
        }

        public DataTable getCreateUser()
        {
            return Dal.ProjectInfo.Instance.getCreateUser();
        }

        public DataTable GetDataByName(string ProjectName)
        {
            return Dal.ProjectInfo.Instance.GetDataByName(ProjectName);
        }

        /// <summary>
        /// 其他任务导出时得到客户信息
        /// </summary>
        /// <param name="custIDFieldName">自定义表中存储CustID的列名</param>
        /// <returns></returns>
        public DataTable GetExportCustInfoByTempt(string custIDFieldName, string projectID)
        {
            return Dal.ProjectInfo.Instance.GetExportCustInfoByTempt(custIDFieldName, projectID);
        }

        public DataTable GetExportCustInfoByUnitTest(string custID)
        {
            return Dal.ProjectInfo.Instance.GetExportCustInfoByUnitTest(custID);
        }

        public int GetCRMCust(string custID)
        {
            return Dal.ProjectInfo.Instance.GetCRMCust(custID);
        }

        public DataTable p_GerNoExistsCustID(string custIDs)
        {
            return Dal.ProjectInfo.Instance.p_GerNoExistsCustID(custIDs);
        }

        public Entities.ProjectInfo GetProjectInfoByDemandID(string CurrDemandID)
        {
            Entities.ProjectInfo model = new Entities.ProjectInfo();
            Entities.QueryProjectInfo query = new Entities.QueryProjectInfo();
            query.DemandID = CurrDemandID;

            int totalCount = 0;
            DataTable dt = GetProjectInfo(query, "", 1, 999, out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                model = Dal.ProjectInfo.Instance.LoadSingleProjectInfo(dt.Rows[0]);
            }
            else
            {
                model = null;
            }
            return model;
        }

        public Entities.ProjectInfo GetProjectInfoByDemandID(string CurrDemandID, int Batch)
        {
            Entities.ProjectInfo model = new Entities.ProjectInfo();
            Entities.QueryProjectInfo query = new Entities.QueryProjectInfo();
            query.DemandID = CurrDemandID;
            query.Batch = Batch;

            int totalCount = 0;
            DataTable dt = GetProjectInfo(query, "", 1, 999, out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                model = Dal.ProjectInfo.Instance.LoadSingleProjectInfo(dt.Rows[0]);
            }
            else
            {
                model = null;
            }
            return model;
        }

        public List<Entities.ProjectInfo> GetProjectInfoByDemandIDList(string CurrDemandID)
        {
            List<Entities.ProjectInfo> model = new List<Entities.ProjectInfo>();
            Entities.QueryProjectInfo query = new Entities.QueryProjectInfo();
            query.DemandID = CurrDemandID;

            int totalCount = 0;
            DataTable dt = GetProjectInfo(query, "", 1, 999, out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    model.Add(Dal.ProjectInfo.Instance.LoadSingleProjectInfo(dr));
                }
            }
            return model;
        }
        public List<Entities.ProjectInfo> GetProjectInfoByDemandIDBathList(string CurrDemandID, int Batch)
        {
            List<Entities.ProjectInfo> model = new List<Entities.ProjectInfo>();
            Entities.QueryProjectInfo query = new Entities.QueryProjectInfo();
            query.DemandID = CurrDemandID;
            query.Batch = Batch;
            int totalCount = 0;
            DataTable dt = GetProjectInfo(query, "", 1, 999, out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    model.Add(Dal.ProjectInfo.Instance.LoadSingleProjectInfo(dr));
                }
            }
            return model;
        }

        /// 根据项目信息，撤销任务
        /// <summary>
        /// 根据项目信息，撤销任务
        /// </summary>
        /// <param name="model">项目信息</param>
        /// <param name="errMsg"></param>
        public void RevokeCJKProjectTask(Entities.ProjectInfo model, out string errMsg)
        {
            errMsg = "";
            try
            {
                Dal.ProjectInfo.Instance.RevokeCJKProjectTask(model.ProjectID);
                if (model.Source == 6)//CJK项目，需要发邮件
                {
                    #region 发邮件
                    int DealCount = 0;//总处理量
                    int SuccessCount = 0;//总成功量
                    int totalCount = 0;

                    Entities.QueryLeadsTask query = new QueryLeadsTask();
                    query.ProjectID = (int)model.ProjectID;
                    //query.Status = (int)LeadsTaskStatus.Processed;
                    DataTable dt = BLL.LeadsTask.Instance.GetLeadsTask(query, "", 1, 999999, out totalCount);
                    DealCount = dt.Rows.Count;

                    query = new QueryLeadsTask();
                    query.ProjectID = (int)model.ProjectID;
                    query.IsSuccess = 1;
                    dt = BLL.LeadsTask.Instance.GetLeadsTask(query, "", 1, 999999, out totalCount);
                    SuccessCount = dt.Rows.Count;

                    string[] userEmail = ConfigurationManager.AppSettings["CRMCJKProjectEmailInfo"].Trim() != "" ? ConfigurationManager.AppSettings["CRMCJKProjectEmailInfo"].Split(';') : null;

                    if (userEmail != null && userEmail.Length > 0)
                    {
                        foreach (string item in userEmail)
                        {
                            string mailBody1 = item.Split(':')[0];
                            string url = ConfigurationManager.AppSettings["WebBaseURL"] + "/ProjectManage/ViewProject.aspx?projectid=" + model.ProjectID.ToString();
                            string mailBody2 = "您有一个关于线索邀约的项目已完成目标数量，未处理任务已自动撤销。<br/><br/>项目名称：<a href='" + url + "'>" + model.Name + "</a><br/><br/>总线索量：" + DealCount.ToString() + "<br/><br/>实际成功量：" + SuccessCount.ToString();
                            string Title = "您有一个项目已结束";
                            string[] reciver = new string[] { item.Split(':')[1] };
                            BLL.EmailHelper.Instance.SendMail(mailBody1, mailBody2, Title, reciver);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("撤销项目下任务时报错，项目ID为：" + model.ProjectID.ToString(), ex);
                errMsg = ex.Message;
            }
        }
        public void RevokeCJKTaskByDemandID_Area(string DeamandID, int AreaID, out string errMsg)
        {
            errMsg = "";
            try
            {
                int DealCount, SuccessCount = 0;
                DataTable dt = Dal.ProjectInfo.Instance.RevokeCJKTaskByDemandID_Area(DeamandID, AreaID, out DealCount, out SuccessCount);

                #region 发送邮件
                //发送接收人列表
                string[] userEmail = ConfigurationManager.AppSettings["CRMCJKProjectEmailInfo"].Trim() != "" ? ConfigurationManager.AppSettings["CRMCJKProjectEmailInfo"].Split(';') : null;
                if (userEmail != null && userEmail.Length > 0)
                {
                    foreach (string item in userEmail)
                    {
                        string mailBody1 = item.Split(':')[0];
                        StringBuilder mailBody2 = new StringBuilder();
                        mailBody2.Append("您有一个关于线索邀约项目的落地区域已完成目标数量，该区域下未处理任务已自动撤销。");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            mailBody2.Append("<br/><br/>项目名称：");
                            foreach (DataRow dr in dt.Rows)
                            {
                                string url = ConfigurationManager.AppSettings["WebBaseURL"] + "/ProjectManage/ViewProject.aspx?projectid=" + dr["ProjectID"].ToString();
                                mailBody2.Append("<a href='" + url + "'>" + dr["Name"].ToString() + "</a>，");
                            }
                            mailBody2.Remove(mailBody2.Length - 1, 1);
                            mailBody2.Append("<br/><br/>完成区域：" + AreaHelper.Instance.GetAreaNameByID(AreaID.ToString()));
                        }
                        mailBody2.Append("<br/><br/>总处理量：" + DealCount + "<br/><br/>实际成功量：" + SuccessCount);
                        string Title = "您有一个目标落地区域已完成";
                        string[] reciver = new string[] { item.Split(':')[1] };
                        BLL.EmailHelper.Instance.SendMail(mailBody1, mailBody2.ToString(), Title, reciver);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("撤销项目下任务时报错：DeamandID：{0}，AreaID: {1}", DeamandID, AreaID), ex);
                errMsg = ex.Message;
            }

        }
        /// 结束集客项目
        /// <summary>
        /// 结束集客项目
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="Source">5：YJK 6：CJK</param>
        /// <param name="errMsg"></param>
        public void EndCCProject(long ProjectID, int Source, out string errMsg)
        {
            errMsg = "";
            Entities.ProjectInfo InfoModel = BLL.ProjectInfo.Instance.GetProjectInfo(ProjectID);
            if (InfoModel != null)
            {
                //5：YJK项目
                //6：CJK项目
                if (InfoModel.Source == Source && InfoModel.Status != 2)
                {
                    Dal.ProjectInfo.Instance.EndCCProjectForYiJiKe(ProjectID);

                    if (Source == 5)
                    {
                        int DealCount = 0;//总处理量
                        int SuccessCount = 0;//总成功量
                        int totalCount = 0;

                        Entities.QueryLeadsTask query = new QueryLeadsTask();
                        query.ProjectID = (int)ProjectID;
                        query.Status = (int)LeadsTaskStatus.Processed;
                        DataTable dt = BLL.LeadsTask.Instance.GetLeadsTask(query, "", 1, 999999, out totalCount);
                        DealCount = dt.Rows.Count;

                        query = new QueryLeadsTask();
                        query.ProjectID = (int)ProjectID;
                        query.IsSuccess = 1;
                        dt = BLL.LeadsTask.Instance.GetLeadsTask(query, "", 1, 999999, out totalCount);
                        SuccessCount = dt.Rows.Count;

                        #region 发送邮件
                        string[] userEmail;//发送接收人列表
                        switch (Source)
                        {
                            case 5://易集客-YJK项目
                                userEmail = ConfigurationManager.AppSettings["YiJiKeProjectEmailInfo"].Trim() != "" ? ConfigurationManager.AppSettings["YiJiKeProjectEmailInfo"].Split(';') : null;
                                break;
                            case 6://厂商集客-CJK项目
                                userEmail = ConfigurationManager.AppSettings["CRMCJKProjectEmailInfo"].Trim() != "" ? ConfigurationManager.AppSettings["CRMCJKProjectEmailInfo"].Split(';') : null;
                                break;
                            default: userEmail = null;
                                break;
                        }
                        if (userEmail != null && userEmail.Length > 0)
                        {
                            foreach (string item in userEmail)
                            {
                                string mailBody1 = item.Split(':')[0];
                                string url = ConfigurationManager.AppSettings["WebBaseURL"] + "/ProjectManage/ViewProject.aspx?projectid=" + InfoModel.ProjectID.ToString();
                                string mailBody2 = "您有一个关于线索邀约的项目已完成目标数量，未处理任务已自动撤销。<br/><br/>项目名称：<a href='" + url + "'>" + InfoModel.Name + "</a><br/><br/>总处理量：" + DealCount.ToString() + "<br/><br/>实际成功量：" + SuccessCount.ToString();
                                string Title = "您有一个项目已结束";
                                string[] reciver = new string[] { item.Split(':')[1] };
                                BLL.EmailHelper.Instance.SendMail(mailBody1, mailBody2, Title, reciver);
                            }
                        }
                        #endregion
                    }
                }
                else if (InfoModel.Source == Source && InfoModel.Status == 2)
                {
                    BLL.Loger.Log4Net.Info("需求单号：" + InfoModel.DemandID + ",批次号：" + InfoModel.Batch + "对应的项目已结束");
                }
                else if (InfoModel.Source != Source)
                {
                    errMsg = "需求单号：" + InfoModel.DemandID + ",批次号：" + InfoModel.Batch + "对应的项目不是易集客或厂商集客相对应的项目。";
                    BLL.Loger.Log4Net.Info(errMsg);

                }
            }
            else
            {
                errMsg = "没找到相关CC项目";
            }
        }
        /// 结束厂商集客项目
        /// <summary>
        /// 结束厂商集客项目
        /// </summary>
        /// <param name="CurrDemandID"></param>
        /// <param name="errMsg"></param>
        public void EndCCProjectForCJK(string CurrDemandID, out string errMsg)
        {
            errMsg = "";
            List<Entities.ProjectInfo> list = GetProjectInfoByDemandIDList(CurrDemandID);
            if (list != null && list.Count > 0)
            {
                BLL.Loger.Log4Net.Info("查询项目数量：" + list.Count);
                foreach (Entities.ProjectInfo item in list)
                {
                    //结束项目
                    string info = "";
                    BLL.ProjectInfo.Instance.EndCCProject(item.ProjectID, 6, out info);
                    BLL.ProjectLog.Instance.InsertProjectLog(item.ProjectID, ProjectLogOper.L4_结束项目, "结束项目-" + item.Name, null, -1);
                    errMsg += info;
                }
            }
            else
            {
                BLL.Loger.Log4Net.Info("没有找到需求单号：" + CurrDemandID + "对应的项目。");

                errMsg = "没有找到需求单号：" + CurrDemandID + "对应的项目。";
            }
        }
        /// 结束厂商集客项目
        /// <summary>
        /// 结束厂商集客项目
        /// </summary>
        /// <param name="CurrDemandID">需求单ID</param>
        /// <param name="BatchNo">批次号</param>
        /// <param name="errMsg">返回信息</param>
        public void EndCCProjectForCJKByBatch(string CurrDemandID, int BatchNo, out string errMsg)
        {
            errMsg = "";
            List<Entities.ProjectInfo> list = GetProjectInfoByDemandIDBathList(CurrDemandID, BatchNo);
            if (list != null && list.Count > 0)
            {
                BLL.Loger.Log4Net.Info("查询项目数量：" + list.Count);
                foreach (Entities.ProjectInfo item in list)
                {
                    //结束项目
                    string info = "";

                    BLL.ProjectInfo.Instance.RevokeCJKProjectTask(item, out info);
                    if (string.IsNullOrEmpty(info))
                    {
                        BLL.Loger.Log4Net.Info("需求单号：" + item.DemandID + ",批次号：" + item.Batch + "对应的项目已结束");
                    }
                    errMsg += info;
                }
            }
            else
            {
                BLL.Loger.Log4Net.Info("没有找到需求单号：" + CurrDemandID + "对应的项目。");

                errMsg = "没有找到需求单号：" + CurrDemandID + "对应的项目。";
            }
        }
        /// 结束易集客项目
        /// <summary>
        /// 结束易集客项目
        /// </summary>
        /// <param name="CurrDemandID"></param>
        /// <param name="errMsg"></param>
        public void EndCCProjectForYJK(string CurrDemandID, out string errMsg)
        {
            errMsg = "";
            Entities.ProjectInfo projectModel = GetProjectInfoByDemandID(CurrDemandID);
            if (projectModel != null)
            {
                //结束项目
                BLL.ProjectInfo.Instance.EndCCProject(projectModel.ProjectID, 5, out errMsg);
                BLL.ProjectLog.Instance.InsertProjectLog(projectModel.ProjectID, ProjectLogOper.L4_结束项目, "结束项目-" + projectModel.Name, null, -1);
            }
        }

        /// 获取项目的外呼状态
        /// <summary>
        /// 获取项目的外呼状态
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public int? GetProjectAutoCallACStatus(long projectid)
        {
            return Dal.ProjectInfo.Instance.GetProjectAutoCallACStatus(projectid);
        }

        /// 获取项目的自动外呼相关信息
        /// <summary>
        /// 获取项目的自动外呼相关信息
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public DataTable GetProjectAutoCallInfo(long projectid)
        {
            return Dal.ProjectInfo.Instance.GetProjectAutoCallInfo(projectid);
        }

        /// 获取项目的自动外呼相关信息
        /// <summary>
        /// 获取项目的自动外呼相关信息
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public Dictionary<string, int> GetProjectStatInfo(long projectid)
        {
            return Dal.ProjectInfo.Instance.GetProjectStatInfo(projectid);
        }

        /// 结束项目时结束自动外呼
        /// <summary>
        /// 结束项目时结束自动外呼
        /// </summary>
        /// <param name="projectid"></param>
        public int EndAutoCallProject(long projectid)
        {
            return Dal.ProjectInfo.Instance.EndAutoCallProject(projectid);
        }

        /// <summary>
        /// 根据项目名称关键字，查找项目表，返回项目名称列表（带数据权限）
        /// </summary>
        /// <param name="projectName">项目名称关键字</param>
        /// <param name="userID">当前登录人ID</param>
        /// <returns>返回项目名称列表</returns>
        public DataTable GetProjectNames(string projectName, int bgid, int scid, int pCatageID, int userID)
        {
            return Dal.ProjectInfo.Instance.GetProjectNames(projectName, bgid, scid, pCatageID, userID);
        }

        /// <summary>
        /// 根据当前登录人取管辖分组下最近的项目
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetLastestProjectByUserID(QueryProjectInfo query, string order, int currentPage, int pageSize, out int totalCount, int userid)
        {
            return Dal.ProjectInfo.Instance.GetLastestProjectByUserID(query, order, currentPage, pageSize, out totalCount, userid);
        }

        /// <summary>
        /// 根据当前登录人取管辖分组所有项目的分配，提交以及接通，成功等统计量
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ProjectReport(Entities.BusinessReport.QueryProjectReport query, string order, int currentPage, int pageSize, out int totalCount, int userid)
        {
            return Dal.ProjectInfo.Instance.GetB_ProjectReport(query, order, currentPage, pageSize, out totalCount, userid);
        }
        /// <summary>
        /// 根据当前登录人取管辖分组所有项目的分配，提交以及接通，成功等统计量总计
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ProjectReportSum(Entities.BusinessReport.QueryProjectReport query, int userid)
        {
            return Dal.ProjectInfo.Instance.GetB_ProjectReportSum(query, userid);
        }

        /// <summary>
        /// 根据当前登录人取管辖分组所有项目的分配，提交以及接通，成功等统计量
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ProjectReport_Excel(Entities.BusinessReport.QueryProjectReport query, int userid)
        {
            return Dal.ProjectInfo.Instance.GetB_ProjectReport_Excel(query, userid);
        }
        /// <summary>
        /// 根据当前登录人取管辖分组所有项目的分配，提交以及接通，成功等统计量总计
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ProjectReportSum_Excel(Entities.BusinessReport.QueryProjectReport query, int userid)
        {
            return Dal.ProjectInfo.Instance.GetB_ProjectReportSum_Excel(query, userid);
        }

        /// <summary>
        /// 根据项目和失败类型取具体失败原因
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="typestr"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetProjectFailReason(int projectid, string typestr)
        {
            Dictionary<int, string> dC = new Dictionary<int, string>();
            DataTable dt = Dal.ProjectInfo.Instance.GetProjectFailReason(projectid, typestr);
            if (dt != null && dt.Rows.Count > 0)
            {
                string keyvalue = dt.Rows[0]["TFValue"].ToString();
                if (!string.IsNullOrEmpty(keyvalue))
                {
                    string[] frist = keyvalue.Split(';');
                    for (int i = 0; i < frist.Length; i++)
                    {
                        string second = frist[i];
                        if (second.Split('|').Length > 1)
                        {
                            dC.Add(int.Parse(second.Split('|')[0]), second.Split('|')[1]);
                        }
                    }
                }
            }
            return dC;
        }
        /// <summary>
        /// 根据当前登录人取管辖分组所有项目的分配，提交以及接通，成功等统计量
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ReturnVisitReport(Entities.BusinessReport.QueryReturnVisitReport query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectInfo.Instance.GetB_ReturnVisitReport(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 根据当前登录人取管辖分组所有项目的分配，提交以及接通，成功等统计量总计
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ReturnVisitReportSum(Entities.BusinessReport.QueryReturnVisitReport query)
        {
            return Dal.ProjectInfo.Instance.GetB_ReturnVisitReportSum(query);
        }

        /// <summary>
        /// 取回访分类
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="typestr"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetReturnVisitCategory()
        {
            Dictionary<int, string> dC = new Dictionary<int, string>();
            //取CRM客户回访分类，分类类型是客服相关
            DataTable dt = Dal.ProjectInfo.Instance.GetACDictInfoByDictType("101");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dC.Add(int.Parse(dt.Rows[i]["DictID"].ToString()), dt.Rows[i]["DictName"].ToString());
                }

            }
            return dC;
        }
        /// <summary>
        /// 导出根据当前登录人取管辖分组所有人回访记录，提交以及接通，成功等统计量
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ReturnVisitReport_ForExcel(Entities.BusinessReport.QueryReturnVisitReport query)
        {
            return Dal.ProjectInfo.Instance.GetB_ReturnVisitReport_ForExcel(query);
        }

        /// <summary>
        /// 导出根据当前登录人取管辖分组所有回访记录，提交以及接通，成功等统计量总计
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ReturnVisitReportSum_ForExcel(Entities.BusinessReport.QueryReturnVisitReport query)
        {
            return Dal.ProjectInfo.Instance.GetB_ReturnVisitReportSum_ForExcel(query);
        }
    }
}

