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
    /// ҵ���߼���ProjectInfo ��ժҪ˵����
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
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
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
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ProjectInfo GetProjectInfo(long ProjectID)
        {
            return Dal.ProjectInfo.Instance.GetProjectInfo(ProjectID);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ProjectInfo model)
        {
            return Dal.ProjectInfo.Instance.Insert(sqltran, model);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.ProjectInfo model)
        {
            return Dal.ProjectInfo.Instance.Update(model);
        }
        /// <summary>
        /// ����һ������
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
        /// �������񵼳�ʱ�õ��ͻ���Ϣ
        /// </summary>
        /// <param name="custIDFieldName">�Զ�����д洢CustID������</param>
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

        /// ������Ŀ��Ϣ����������
        /// <summary>
        /// ������Ŀ��Ϣ����������
        /// </summary>
        /// <param name="model">��Ŀ��Ϣ</param>
        /// <param name="errMsg"></param>
        public void RevokeCJKProjectTask(Entities.ProjectInfo model, out string errMsg)
        {
            errMsg = "";
            try
            {
                Dal.ProjectInfo.Instance.RevokeCJKProjectTask(model.ProjectID);
                if (model.Source == 6)//CJK��Ŀ����Ҫ���ʼ�
                {
                    #region ���ʼ�
                    int DealCount = 0;//�ܴ�����
                    int SuccessCount = 0;//�ܳɹ���
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
                            string mailBody2 = "����һ������������Լ����Ŀ�����Ŀ��������δ�����������Զ�������<br/><br/>��Ŀ���ƣ�<a href='" + url + "'>" + model.Name + "</a><br/><br/>����������" + DealCount.ToString() + "<br/><br/>ʵ�ʳɹ�����" + SuccessCount.ToString();
                            string Title = "����һ����Ŀ�ѽ���";
                            string[] reciver = new string[] { item.Split(':')[1] };
                            BLL.EmailHelper.Instance.SendMail(mailBody1, mailBody2, Title, reciver);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("������Ŀ������ʱ������ĿIDΪ��" + model.ProjectID.ToString(), ex);
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

                #region �����ʼ�
                //���ͽ������б�
                string[] userEmail = ConfigurationManager.AppSettings["CRMCJKProjectEmailInfo"].Trim() != "" ? ConfigurationManager.AppSettings["CRMCJKProjectEmailInfo"].Split(';') : null;
                if (userEmail != null && userEmail.Length > 0)
                {
                    foreach (string item in userEmail)
                    {
                        string mailBody1 = item.Split(':')[0];
                        StringBuilder mailBody2 = new StringBuilder();
                        mailBody2.Append("����һ������������Լ��Ŀ��������������Ŀ����������������δ�����������Զ�������");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            mailBody2.Append("<br/><br/>��Ŀ���ƣ�");
                            foreach (DataRow dr in dt.Rows)
                            {
                                string url = ConfigurationManager.AppSettings["WebBaseURL"] + "/ProjectManage/ViewProject.aspx?projectid=" + dr["ProjectID"].ToString();
                                mailBody2.Append("<a href='" + url + "'>" + dr["Name"].ToString() + "</a>��");
                            }
                            mailBody2.Remove(mailBody2.Length - 1, 1);
                            mailBody2.Append("<br/><br/>�������" + AreaHelper.Instance.GetAreaNameByID(AreaID.ToString()));
                        }
                        mailBody2.Append("<br/><br/>�ܴ�������" + DealCount + "<br/><br/>ʵ�ʳɹ�����" + SuccessCount);
                        string Title = "����һ��Ŀ��������������";
                        string[] reciver = new string[] { item.Split(':')[1] };
                        BLL.EmailHelper.Instance.SendMail(mailBody1, mailBody2.ToString(), Title, reciver);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("������Ŀ������ʱ����DeamandID��{0}��AreaID: {1}", DeamandID, AreaID), ex);
                errMsg = ex.Message;
            }

        }
        /// ����������Ŀ
        /// <summary>
        /// ����������Ŀ
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="Source">5��YJK 6��CJK</param>
        /// <param name="errMsg"></param>
        public void EndCCProject(long ProjectID, int Source, out string errMsg)
        {
            errMsg = "";
            Entities.ProjectInfo InfoModel = BLL.ProjectInfo.Instance.GetProjectInfo(ProjectID);
            if (InfoModel != null)
            {
                //5��YJK��Ŀ
                //6��CJK��Ŀ
                if (InfoModel.Source == Source && InfoModel.Status != 2)
                {
                    Dal.ProjectInfo.Instance.EndCCProjectForYiJiKe(ProjectID);

                    if (Source == 5)
                    {
                        int DealCount = 0;//�ܴ�����
                        int SuccessCount = 0;//�ܳɹ���
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

                        #region �����ʼ�
                        string[] userEmail;//���ͽ������б�
                        switch (Source)
                        {
                            case 5://�׼���-YJK��Ŀ
                                userEmail = ConfigurationManager.AppSettings["YiJiKeProjectEmailInfo"].Trim() != "" ? ConfigurationManager.AppSettings["YiJiKeProjectEmailInfo"].Split(';') : null;
                                break;
                            case 6://���̼���-CJK��Ŀ
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
                                string mailBody2 = "����һ������������Լ����Ŀ�����Ŀ��������δ�����������Զ�������<br/><br/>��Ŀ���ƣ�<a href='" + url + "'>" + InfoModel.Name + "</a><br/><br/>�ܴ�������" + DealCount.ToString() + "<br/><br/>ʵ�ʳɹ�����" + SuccessCount.ToString();
                                string Title = "����һ����Ŀ�ѽ���";
                                string[] reciver = new string[] { item.Split(':')[1] };
                                BLL.EmailHelper.Instance.SendMail(mailBody1, mailBody2, Title, reciver);
                            }
                        }
                        #endregion
                    }
                }
                else if (InfoModel.Source == Source && InfoModel.Status == 2)
                {
                    BLL.Loger.Log4Net.Info("���󵥺ţ�" + InfoModel.DemandID + ",���κţ�" + InfoModel.Batch + "��Ӧ����Ŀ�ѽ���");
                }
                else if (InfoModel.Source != Source)
                {
                    errMsg = "���󵥺ţ�" + InfoModel.DemandID + ",���κţ�" + InfoModel.Batch + "��Ӧ����Ŀ�����׼��ͻ��̼������Ӧ����Ŀ��";
                    BLL.Loger.Log4Net.Info(errMsg);

                }
            }
            else
            {
                errMsg = "û�ҵ����CC��Ŀ";
            }
        }
        /// �������̼�����Ŀ
        /// <summary>
        /// �������̼�����Ŀ
        /// </summary>
        /// <param name="CurrDemandID"></param>
        /// <param name="errMsg"></param>
        public void EndCCProjectForCJK(string CurrDemandID, out string errMsg)
        {
            errMsg = "";
            List<Entities.ProjectInfo> list = GetProjectInfoByDemandIDList(CurrDemandID);
            if (list != null && list.Count > 0)
            {
                BLL.Loger.Log4Net.Info("��ѯ��Ŀ������" + list.Count);
                foreach (Entities.ProjectInfo item in list)
                {
                    //������Ŀ
                    string info = "";
                    BLL.ProjectInfo.Instance.EndCCProject(item.ProjectID, 6, out info);
                    BLL.ProjectLog.Instance.InsertProjectLog(item.ProjectID, ProjectLogOper.L4_������Ŀ, "������Ŀ-" + item.Name, null, -1);
                    errMsg += info;
                }
            }
            else
            {
                BLL.Loger.Log4Net.Info("û���ҵ����󵥺ţ�" + CurrDemandID + "��Ӧ����Ŀ��");

                errMsg = "û���ҵ����󵥺ţ�" + CurrDemandID + "��Ӧ����Ŀ��";
            }
        }
        /// �������̼�����Ŀ
        /// <summary>
        /// �������̼�����Ŀ
        /// </summary>
        /// <param name="CurrDemandID">����ID</param>
        /// <param name="BatchNo">���κ�</param>
        /// <param name="errMsg">������Ϣ</param>
        public void EndCCProjectForCJKByBatch(string CurrDemandID, int BatchNo, out string errMsg)
        {
            errMsg = "";
            List<Entities.ProjectInfo> list = GetProjectInfoByDemandIDBathList(CurrDemandID, BatchNo);
            if (list != null && list.Count > 0)
            {
                BLL.Loger.Log4Net.Info("��ѯ��Ŀ������" + list.Count);
                foreach (Entities.ProjectInfo item in list)
                {
                    //������Ŀ
                    string info = "";

                    BLL.ProjectInfo.Instance.RevokeCJKProjectTask(item, out info);
                    if (string.IsNullOrEmpty(info))
                    {
                        BLL.Loger.Log4Net.Info("���󵥺ţ�" + item.DemandID + ",���κţ�" + item.Batch + "��Ӧ����Ŀ�ѽ���");
                    }
                    errMsg += info;
                }
            }
            else
            {
                BLL.Loger.Log4Net.Info("û���ҵ����󵥺ţ�" + CurrDemandID + "��Ӧ����Ŀ��");

                errMsg = "û���ҵ����󵥺ţ�" + CurrDemandID + "��Ӧ����Ŀ��";
            }
        }
        /// �����׼�����Ŀ
        /// <summary>
        /// �����׼�����Ŀ
        /// </summary>
        /// <param name="CurrDemandID"></param>
        /// <param name="errMsg"></param>
        public void EndCCProjectForYJK(string CurrDemandID, out string errMsg)
        {
            errMsg = "";
            Entities.ProjectInfo projectModel = GetProjectInfoByDemandID(CurrDemandID);
            if (projectModel != null)
            {
                //������Ŀ
                BLL.ProjectInfo.Instance.EndCCProject(projectModel.ProjectID, 5, out errMsg);
                BLL.ProjectLog.Instance.InsertProjectLog(projectModel.ProjectID, ProjectLogOper.L4_������Ŀ, "������Ŀ-" + projectModel.Name, null, -1);
            }
        }

        /// ��ȡ��Ŀ�����״̬
        /// <summary>
        /// ��ȡ��Ŀ�����״̬
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public int? GetProjectAutoCallACStatus(long projectid)
        {
            return Dal.ProjectInfo.Instance.GetProjectAutoCallACStatus(projectid);
        }

        /// ��ȡ��Ŀ���Զ���������Ϣ
        /// <summary>
        /// ��ȡ��Ŀ���Զ���������Ϣ
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public DataTable GetProjectAutoCallInfo(long projectid)
        {
            return Dal.ProjectInfo.Instance.GetProjectAutoCallInfo(projectid);
        }

        /// ��ȡ��Ŀ���Զ���������Ϣ
        /// <summary>
        /// ��ȡ��Ŀ���Զ���������Ϣ
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public Dictionary<string, int> GetProjectStatInfo(long projectid)
        {
            return Dal.ProjectInfo.Instance.GetProjectStatInfo(projectid);
        }

        /// ������Ŀʱ�����Զ����
        /// <summary>
        /// ������Ŀʱ�����Զ����
        /// </summary>
        /// <param name="projectid"></param>
        public int EndAutoCallProject(long projectid)
        {
            return Dal.ProjectInfo.Instance.EndAutoCallProject(projectid);
        }

        /// <summary>
        /// ������Ŀ���ƹؼ��֣�������Ŀ��������Ŀ�����б�������Ȩ�ޣ�
        /// </summary>
        /// <param name="projectName">��Ŀ���ƹؼ���</param>
        /// <param name="userID">��ǰ��¼��ID</param>
        /// <returns>������Ŀ�����б�</returns>
        public DataTable GetProjectNames(string projectName, int bgid, int scid, int pCatageID, int userID)
        {
            return Dal.ProjectInfo.Instance.GetProjectNames(projectName, bgid, scid, pCatageID, userID);
        }

        /// <summary>
        /// ���ݵ�ǰ��¼��ȡ��Ͻ�������������Ŀ
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetLastestProjectByUserID(QueryProjectInfo query, string order, int currentPage, int pageSize, out int totalCount, int userid)
        {
            return Dal.ProjectInfo.Instance.GetLastestProjectByUserID(query, order, currentPage, pageSize, out totalCount, userid);
        }

        /// <summary>
        /// ���ݵ�ǰ��¼��ȡ��Ͻ����������Ŀ�ķ��䣬�ύ�Լ���ͨ���ɹ���ͳ����
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ProjectReport(Entities.BusinessReport.QueryProjectReport query, string order, int currentPage, int pageSize, out int totalCount, int userid)
        {
            return Dal.ProjectInfo.Instance.GetB_ProjectReport(query, order, currentPage, pageSize, out totalCount, userid);
        }
        /// <summary>
        /// ���ݵ�ǰ��¼��ȡ��Ͻ����������Ŀ�ķ��䣬�ύ�Լ���ͨ���ɹ���ͳ�����ܼ�
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ProjectReportSum(Entities.BusinessReport.QueryProjectReport query, int userid)
        {
            return Dal.ProjectInfo.Instance.GetB_ProjectReportSum(query, userid);
        }

        /// <summary>
        /// ���ݵ�ǰ��¼��ȡ��Ͻ����������Ŀ�ķ��䣬�ύ�Լ���ͨ���ɹ���ͳ����
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ProjectReport_Excel(Entities.BusinessReport.QueryProjectReport query, int userid)
        {
            return Dal.ProjectInfo.Instance.GetB_ProjectReport_Excel(query, userid);
        }
        /// <summary>
        /// ���ݵ�ǰ��¼��ȡ��Ͻ����������Ŀ�ķ��䣬�ύ�Լ���ͨ���ɹ���ͳ�����ܼ�
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ProjectReportSum_Excel(Entities.BusinessReport.QueryProjectReport query, int userid)
        {
            return Dal.ProjectInfo.Instance.GetB_ProjectReportSum_Excel(query, userid);
        }

        /// <summary>
        /// ������Ŀ��ʧ������ȡ����ʧ��ԭ��
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
        /// ���ݵ�ǰ��¼��ȡ��Ͻ����������Ŀ�ķ��䣬�ύ�Լ���ͨ���ɹ���ͳ����
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ReturnVisitReport(Entities.BusinessReport.QueryReturnVisitReport query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectInfo.Instance.GetB_ReturnVisitReport(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ���ݵ�ǰ��¼��ȡ��Ͻ����������Ŀ�ķ��䣬�ύ�Լ���ͨ���ɹ���ͳ�����ܼ�
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ReturnVisitReportSum(Entities.BusinessReport.QueryReturnVisitReport query)
        {
            return Dal.ProjectInfo.Instance.GetB_ReturnVisitReportSum(query);
        }

        /// <summary>
        /// ȡ�ط÷���
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="typestr"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetReturnVisitCategory()
        {
            Dictionary<int, string> dC = new Dictionary<int, string>();
            //ȡCRM�ͻ��ط÷��࣬���������ǿͷ����
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
        /// �������ݵ�ǰ��¼��ȡ��Ͻ���������˻طü�¼���ύ�Լ���ͨ���ɹ���ͳ����
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ReturnVisitReport_ForExcel(Entities.BusinessReport.QueryReturnVisitReport query)
        {
            return Dal.ProjectInfo.Instance.GetB_ReturnVisitReport_ForExcel(query);
        }

        /// <summary>
        /// �������ݵ�ǰ��¼��ȡ��Ͻ�������лطü�¼���ύ�Լ���ͨ���ɹ���ͳ�����ܼ�
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ReturnVisitReportSum_ForExcel(Entities.BusinessReport.QueryReturnVisitReport query)
        {
            return Dal.ProjectInfo.Instance.GetB_ReturnVisitReportSum_ForExcel(query);
        }
    }
}

