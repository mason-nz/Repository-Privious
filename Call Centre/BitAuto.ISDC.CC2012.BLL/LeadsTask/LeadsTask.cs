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
using BitAuto.Utils.Config;
using System.Messaging;
using NPOI.HPSF;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���LeadsTask ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-05-19 11:30:50 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class LeadsTask
    {
        #region Instance
        public static readonly LeadsTask Instance = new LeadsTask();
        #endregion

        #region Contructor
        protected LeadsTask()
        { }
        #endregion

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
        public DataTable GetLeadsTask(QueryLeadsTask query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.LeadsTask.Instance.GetLeadsTask(query, order, currentPage, pageSize, out totalCount);
        }
        public DataTable GetLeadsTaskForExport(QueryLeadsTask query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.LeadsTask.Instance.GetLeadsTaskForExport(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetLeadsTaskForExport(QueryLeadsTask query, string order, int currentPage, int pageSize, out int totalCount, string taskcreatestart, string taskcreateend, string tasksubstart, string tasksubend)
        {
            return Dal.LeadsTask.Instance.GetLeadsTaskForExport(query, order, currentPage, pageSize, out totalCount, taskcreatestart, taskcreateend, tasksubstart, tasksubend);
        }


        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.LeadsTask.Instance.GetLeadsTask(new QueryLeadsTask(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.LeadsTask GetLeadsTask(string TaskID)
        {

            return Dal.LeadsTask.Instance.GetLeadsTask(TaskID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByTaskID(string TaskID)
        {
            QueryLeadsTask query = new QueryLeadsTask();
            query.TaskID = TaskID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetLeadsTask(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Insert
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Insert(Entities.LeadsTask model)
        {
            Dal.LeadsTask.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.LeadsTask model)
        {
            Dal.LeadsTask.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.LeadsTask model)
        {
            return Dal.LeadsTask.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.LeadsTask model)
        {
            return Dal.LeadsTask.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(string TaskID)
        {

            return Dal.LeadsTask.Instance.Delete(TaskID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, string TaskID)
        {

            return Dal.LeadsTask.Instance.Delete(sqltran, TaskID);
        }

        #endregion


        /// <summary>
        /// ����Ʒ��id,ȡ������������
        /// </summary>
        /// <param name="BrandID"></param>
        /// <returns></returns>
        public DataTable GetSerialByBrandID(int BrandID)
        {
            return Dal.LeadsTask.Instance.GetSerialByBrandID(BrandID);
        }


        //���ݳ���IDȡ�����б�
        /// <summary>
        /// ���ݳ���IDȡ�����б�
        /// </summary>
        /// <param name="SerialID">���ͣɣ�</param>
        /// <returns></returns>
        public DataTable GetCarListByCarSerialID(int SerialID)
        {
            DataSet dtCarList = null;
            DataTable dtCarNew = new DataTable();
            //��crmȡ������Ϣ
            dtCarList = Dal.LeadsTask.Instance.GetCarListByCarSerialID(SerialID);
            if (dtCarList != null)
            {
                dtCarNew.Columns.Add("CarID", typeof(int));
                dtCarNew.Columns.Add("CarName", typeof(string));
                dtCarNew.Columns.Add("CarYearType", typeof(int));
                //ȡ�������
                DataTable dtYear = dtCarList.Tables[1];
                for (int i = 0; i < dtYear.Rows.Count; i++)
                {
                    DataView dv = dtCarList.Tables[0].DefaultView;
                    dv.RowFilter = "CarYearType=" + dtYear.Rows[i]["CarYearType"];
                    if (dv != null && dv.Count > 0)
                    {
                        DataRow row = dtCarNew.NewRow();
                        row["carid"] = 0;
                        row["CarName"] = dtYear.Rows[i]["CarYearType"] + " ��";
                        row["CarYearType"] = dtYear.Rows[i]["CarYearType"];
                        dtCarNew.Rows.Add(row);
                        for (int n = 0; n < dv.Count; n++)
                        {
                            DataRow rownew = dtCarNew.NewRow();
                            rownew["carid"] = dv[n]["carid"];
                            rownew["CarName"] = dv[n]["CarName"];
                            rownew["CarYearType"] = dv[n]["CarYearType"];
                            dtCarNew.Rows.Add(rownew);
                        }
                    }
                }
            }
            return dtCarNew;
        }

        //��������ID����ȡ������Ϣ
        public DataTable GetTaskInfoListByIDs(string TaskIDS)
        {
            return Dal.LeadsTask.Instance.GetTaskInfoListByIDs(TaskIDS);
        }

        #region �׼�����ش���
        /// �������� 
        /// <summary>
        /// �������� 
        /// </summary>
        /// <param name="transactional">�Ƿ���������</param>
        /// <returns></returns>
        public bool CreateQueue(bool transactional, out string QueueName)
        {
            QueueName = @".\private$\" + (ConfigurationManager.AppSettings["MSMQName"] ?? "CC_GetLeadFormCRM");

            if (MessageQueue.Exists(QueueName))
            {
                return true;
            }
            else
            {
                if (MessageQueue.Create(QueueName, transactional) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// ����status���飬��ȡ��״̬������
        /// <summary>
        /// ����status���飬��ȡ��״̬������
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetStatusNum(Entities.QueryLeadsTask query)
        {
            return Dal.LeadsTask.Instance.GetStatusNum(query);
        }


        /// YJK-��������ID�����κ�����������Լ����
        /// <summary>
        /// YJK-��������ID�����κ�����������Լ����
        /// </summary>
        /// <param name="CurrDemandID">����ID</param>
        /// <param name="CurrBathNo">���κ�</param>
        /// <param name="sourceDt">CRM�ӿ�ȡ��������Դ</param>
        public void GetDataByYJKDemandIDAndBathNo(string CurrDemandID, int CurrBathNo, DataTable sourceDt, out string errMsg)
        {
            errMsg = "";
            DateTime LastDealTime = DateTime.Parse(sourceDt.Rows[0]["LastCallTime"].ToString());
            //�½���Ŀ
            Entities.ProjectInfo newInfoModel;
            //������Ŀ
            Entities.ProjectInfo projectInfoModel;
            //��Ŀ��Դ=5����������=160
            if (CreateProject(CurrDemandID, 5, 160, CurrBathNo, null, out newInfoModel, out projectInfoModel))
            {
                //��Ŀ�������˳�
                return;
            }
            // LeadsTask�������������
            List<Entities.LeadsTask> taskList;
            List<Entities.LeadsTaskOperationLog> logList;
            List<Entities.ProjectDataSoure> dslist;
            //����LeadsTask
            string carinfo = CreateYJKLeadTask(CurrDemandID, sourceDt, out taskList, out logList, out dslist);
            //�������
            errMsg = DataToDB("YJK", CurrDemandID, CurrBathNo, errMsg, LastDealTime, newInfoModel, projectInfoModel, taskList, logList, dslist, carinfo, null);
        }

        #region Add=Masj,Date=2015-04-23
        ///// CJK-��������ID�����κ�����������Լ����
        ///// <summary>
        ///// CJK-��������ID�����κ�����������Լ����
        ///// </summary>
        ///// <param name="CurrDemandID">����ID</param>
        ///// <param name="CurrBathNo">���κ�</param>
        ///// <param name="sourceDt">CRM�ӿ�ȡ��������Դ</param>
        //public void GetDataByCJKDemandIDAndBathNo(string CurrDemandID, int CurrBathNo, int ExpectedNum, DataTable sourceDt, out string errMsg)
        //{
        //    errMsg = "";
        //    DateTime LastDealTime = DateTime.Parse(sourceDt.Rows[0]["LastCallTime"].ToString());
        //    //�½���Ŀ
        //    Entities.ProjectInfo newInfoModel;
        //    //������Ŀ
        //    Entities.ProjectInfo projectInfoModel;
        //    //��Ŀ��Դ=6����������=160
        //    if (CreateProject(CurrDemandID, 6, 160, CurrBathNo, ExpectedNum, out newInfoModel, out projectInfoModel))
        //    {
        //        //��Ŀ�������˳�
        //        return;
        //    }
        //    // LeadsTask�������������
        //    List<Entities.LeadsTask> taskList;
        //    List<Entities.LeadsTaskOperationLog> logList;
        //    List<Entities.ProjectDataSoure> dslist;
        //    //����LeadsTask
        //    string carinfo = CreateCJKLeadTask(CurrDemandID, sourceDt, out taskList, out logList, out dslist);
        //    //�������
        //    errMsg = DataToDB("CJK", CurrDemandID, CurrBathNo, errMsg, LastDealTime, newInfoModel, projectInfoModel, taskList, logList, dslist, carinfo);
        //}
        #endregion

        #region ��������������Լ����  add by yangxl 2015-3-19

        public void GetDataByCJKDemandIDAndBathInBulk(string CurrDemandID, int CurrBathNo, int ExpectedNum, DataTable sourceDt, out string errMsg)
        {
            errMsg = "";
            DateTime LastDealTime = DateTime.Parse(sourceDt.Rows[0]["LastCallTime"].ToString());
            //�½���Ŀ
            Entities.ProjectInfo newInfoModel;
            //������Ŀ
            Entities.ProjectInfo projectInfoModel;
            //��Ŀ��Դ=6����������=160
            if (CreateProject(CurrDemandID, 6, 160, CurrBathNo, ExpectedNum, out newInfoModel, out projectInfoModel))
            {
                //��Ŀ�������˳�
                return;
            }
            // LeadsTask�������������
            //List<Entities.LeadsTask> taskList;
            //List<Entities.LeadsTaskOperationLog> logList;
            //List<Entities.ProjectDataSoure> dslist;
            DataTable dtTasks;
            DataTable dtLogs;
            DataTable dtDataSources;
            //����LeadsTask
            BLL.Loger.Log4Net.Info("��ʼ���÷���CreateCJKLeadTaskBulk ��ȡDataTable");
            string carinfo = CreateCJKLeadTaskBulk(CurrDemandID, CurrBathNo.ToString(), sourceDt, out dtTasks, out dtLogs, out dtDataSources);
            //�������
            errMsg = DataToDBBulk("CJK", CurrDemandID, CurrBathNo, errMsg, LastDealTime, newInfoModel, projectInfoModel, dtTasks, dtLogs, dtDataSources, carinfo, ExpectedNum);
        }



        /// <summary>
        /// �����������κţ���Ŀ�ţ����ڹ�������ŵ绰����ʱ��¼��ϸ��־
        /// </summary>
        /// <param name="CurrDemandID"></param>
        /// <param name="CurrBathNo"></param>
        /// <param name="ProjectNo"></param>
        /// <param name="sourceDt"></param>
        /// <param name="dtTask"></param>
        /// <param name="dtTaskLog"></param>
        /// <param name="dtTaskDataSource"></param>
        /// <returns></returns>
        private string CreateCJKLeadTaskBulk(string currDemandID, string currBathNo, DataTable sourceDt, out DataTable dtTask, out DataTable dtTaskLog, out DataTable dtTaskDataSource)
        {

            dtTask = new DataTable();
            dtTaskLog = new DataTable();
            dtTaskDataSource = new DataTable();

            #region  ��ʼ������

            dtTask.Columns.Add("TaskID");
            dtTask.Columns.Add("ProjectID");
            dtTask.Columns.Add("DemandID");
            dtTask.Columns.Add("RelationID", typeof(System.Guid));

            dtTask.Columns.Add("UserName");
            dtTask.Columns.Add("Tel");
            dtTask.Columns.Add("Sex");
            dtTask.Columns.Add("ProvinceID");

            dtTask.Columns.Add("CityID");
            dtTask.Columns.Add("Status");
            dtTask.Columns.Add("OrderCarMasterID");
            dtTask.Columns.Add("OrderCarMaster");

            dtTask.Columns.Add("OrderCarSerialID");
            dtTask.Columns.Add("OrderCarSerial");
            dtTask.Columns.Add("OrderCarID");
            dtTask.Columns.Add("OrderCar");

            dtTask.Columns.Add("DealerID");
            dtTask.Columns.Add("DealerName");
            dtTask.Columns.Add("OrderCreateTime");
            dtTask.Columns.Add("DCarMasterID");

            dtTask.Columns.Add("DCarMaster");
            dtTask.Columns.Add("DCarSerialID");
            dtTask.Columns.Add("DCarSerial");
            dtTask.Columns.Add("DCarID");

            dtTask.Columns.Add("DCarName");
            dtTask.Columns.Add("IsSuccess");
            dtTask.Columns.Add("FailReason");
            dtTask.Columns.Add("Remark");

            dtTask.Columns.Add("AssignUserID");
            dtTask.Columns.Add("CreateTime");
            dtTask.Columns.Add("CreateUserID");
            dtTask.Columns.Add("LastUpdateTime");

            dtTask.Columns.Add("LastUpdateUserID");
            dtTask.Columns.Add("LastDealTime");
            dtTask.Columns.Add("DealerIDBF");
            dtTask.Columns.Add("DealerNameBF");

            dtTask.Columns.Add("DemandVersion");
            dtTask.Columns.Add("IsJT");
            dtTask.Columns.Add("PBuyCarTime");
            dtTask.Columns.Add("ThinkCar");

            dtTask.Columns.Add("OrderNum");
            dtTask.Columns.Add("TargetProvinceID");
            dtTask.Columns.Add("TargetCityID");
            dtTask.Columns.Add("OrderSource");
            dtTask.Columns.Add("OrderID");

            dtTaskDataSource.Columns.Add("ProjectID");
            dtTaskDataSource.Columns.Add("Source");
            dtTaskDataSource.Columns.Add("RelationID");
            dtTaskDataSource.Columns.Add("Status");
            dtTaskDataSource.Columns.Add("CreateTime");
            dtTaskDataSource.Columns.Add("CreateUserID");

            dtTaskLog.Columns.Add("TaskID");
            dtTaskLog.Columns.Add("OperationStatus");
            dtTaskLog.Columns.Add("TaskStatus");
            dtTaskLog.Columns.Add("Remark");
            dtTaskLog.Columns.Add("CreateTime");
            dtTaskLog.Columns.Add("CreateUserID");

            #endregion

            int maxID = BLL.LeadsTask.Instance.GetMaxIDFromTaskID();//��ȡ���ID

            int OrderSerialID;
            string OrderSerialName;
            int OrderMasterID;
            string OrderMasterName;
            string TaskID;
            DataRow drTmp = null;

            //�������˺������绰
            CheckTelBatch(sourceDt, currDemandID, currBathNo);


            foreach (DataRow dr in sourceDt.Rows)
            {
                string tel = dr["UserMobile"].ToString();
                OrderSerialID = BitAuto.YanFa.Crm2009.Entities.CommonFunction.ObjectToInteger(dr["SerialID"]);
                OrderSerialName = BLL.CarTypeAPIFromCC.Instance.GetSerialNameBySerialID(OrderSerialID);
                OrderMasterID = BLL.CarTypeAPIFromCC.Instance.GetMasterBrandIDBySerialID(OrderSerialID);
                OrderMasterName = BLL.CarTypeAPIFromCC.Instance.GetMasterNameByMasterID(OrderMasterID);

                TaskID = "CJK" + (++maxID).ToString().PadLeft(7, '0');

                //����Task��

                drTmp = dtTask.NewRow();
                drTmp["TaskID"] = TaskID;
                drTmp["ProjectID"] = dr["ProvinceID"].ToString();
                drTmp["DemandID"] = currDemandID;
                drTmp["UserName"] = dr["UserName"].ToString();
                drTmp["Tel"] = tel;
                drTmp["ProvinceID"] = dr["ProvinceID"].ToString();
                drTmp["CityID"] = dr["CityID"].ToString();
                drTmp["OrderCreateTime"] = DateTime.Parse(dr["OrderTime"].ToString());
                drTmp["Sex"] = BitAuto.YanFa.Crm2009.Entities.CommonFunction.ObjectToInteger(dr["Sex"]) == 0 ? 2 : 1;
                drTmp["OrderNum"] = dr["RecID"];
                drTmp["TargetCityID"] = dr["TargetCityID"];
                drTmp["TargetProvinceID"] = dr["TargetProvinceID"];
                drTmp["DealerName"] = dr["DealerName"];
                drTmp["OrderCarSerialID"] = OrderSerialID;
                drTmp["OrderCarSerial"] = OrderSerialName;
                drTmp["OrderCarMasterID"] = OrderMasterID;
                drTmp["OrderCarMaster"] = OrderMasterName;
                drTmp["Status"] = (int)LeadsTaskStatus.NoAllocation;
                drTmp["CreateTime"] = DateTime.Now;
                drTmp["CreateUserID"] = -3;
                drTmp["LastUpdateTime"] = DateTime.Now;
                drTmp["LastDealTime"] = dr["LastCallTime"];
                drTmp["RelationID"] = "00000000-0000-0000-0000-000000000000";
                drTmp["OrderSource"] = dr["OrderSource"];//�����������������ֶΣ�add=masj,date=2015-08-20
                drTmp["OrderID"] = dr.Table.Columns.Contains("OrderID") ? dr["OrderID"] : string.Empty;//����������ID���ֶΣ�add=masj,date=2015-10-10
                dtTask.Rows.Add(drTmp);

                drTmp = dtTaskLog.NewRow();

                drTmp["TaskID"] = TaskID;
                drTmp["CreateTime"] = DateTime.Now;
                drTmp["CreateUserID"] = -3;
                drTmp["OperationStatus"] = (int)Leads_OperationStatus.Gen;
                drTmp["Remark"] = "��������";
                drTmp["TaskStatus"] = (int)LeadsTaskStatus.NoAllocation;

                dtTaskLog.Rows.Add(drTmp);

                drTmp = dtTaskDataSource.NewRow();
                drTmp["ProjectID"] = -2;
                drTmp["RelationID"] = TaskID;
                drTmp["Status"] = 1;
                drTmp["CreateTime"] = DateTime.Now;
                drTmp["CreateUserID"] = -3;
                drTmp["Source"] = 6;

                dtTaskDataSource.Rows.Add(drTmp);

                //rowNum++;
            }

            BitAuto.YanFa.Crm2009.Entities.CJKDemandInfo info = BitAuto.YanFa.Crm2009.BLL.CJKDemandBll.Instance.GetCJKDemandInfo(currDemandID);
            string carinfo = "";
            if (info != null)
            {
                carinfo = info.BrandName + "-" + info.SerialNames;
            }
            return carinfo;
        }


        private string DataToDBBulk(string type, string CurrDemandID, int CurrBathNo, string errMsg, DateTime LastDealTime, Entities.ProjectInfo newInfoModel, Entities.ProjectInfo projectInfoModel, DataTable dtTasks,
            DataTable dtLogs, DataTable dtDataSources, string carinfo, int expectedNum)
        {
            int oldCount = 0;
            if (projectInfoModel != null)
            {
                oldCount = BLL.ProjectDataSoure.Instance.GetDataCountByProjectID(projectInfoModel.ProjectID);
            }
            //��ȡ��ע��Ϣ
            string ProjectNote = GetCJKProjectNote(oldCount + dtTasks.Rows.Count, CurrDemandID, CurrBathNo, carinfo, LastDealTime);


            long projectID = 0;
            string projectName = "";

            #region �����ύ����
            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");
            SqlBulkCopy blBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, tran);
            blBulkCopy.BulkCopyTimeout = 5000;

            #region ӳ����

            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TaskID", "TaskID"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ProjectID", "ProjectID"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DemandID", "DemandID"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("RelationID", "RelationID"));

            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("UserName", "UserName"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Tel", "Tel"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Sex", "Sex"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ProvinceID", "ProvinceID"));

            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CityID", "CityID"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Status", "Status"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OrderCarMasterID", "OrderCarMasterID"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OrderCarMaster", "OrderCarMaster"));

            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OrderCarSerialID", "OrderCarSerialID"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OrderCarSerial", "OrderCarSerial"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OrderCarID", "OrderCarID"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OrderCar", "OrderCar"));

            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DealerID", "DealerID"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DealerName", "DealerName"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OrderCreateTime", "OrderCreateTime"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DCarMasterID", "DCarMasterID"));

            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DCarMaster", "DCarMaster"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DCarSerialID", "DCarSerialID"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DCarSerial", "DCarSerial"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DCarID", "DCarID"));

            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DCarName", "DCarName"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("IsSuccess", "IsSuccess"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("FailReason", "FailReason"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Remark", "Remark"));

            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("AssignUserID", "AssignUserID"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CreateTime", "CreateTime"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CreateUserID", "CreateUserID"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LastUpdateTime", "LastUpdateTime"));

            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LastUpdateUserID", "LastUpdateUserID"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LastDealTime", "LastDealTime"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DealerIDBF", "DealerIDBF"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DealerNameBF", "DealerNameBF"));

            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DemandVersion", "DemandVersion"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("IsJT", "IsJT"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PBuyCarTime", "PBuyCarTime"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ThinkCar", "ThinkCar"));

            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OrderNum", "OrderNum"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TargetProvinceID", "TargetProvinceID"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TargetCityID", "TargetCityID"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OrderSource", "OrderSource"));
            blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OrderID", "OrderID"));
            #endregion


            try
            {

                BLL.Loger.Log4Net.Info("��ʼ������Ŀ��������ProjectID");
                DateTime dtNow = DateTime.Now;
                if (newInfoModel != null)
                {
                    //������Ŀ
                    newInfoModel.Notes = ProjectNote;
                    projectID = BLL.ProjectInfo.Instance.Insert(tran, newInfoModel);
                    projectName = newInfoModel.Name;
                    BLL.ProjectLog.Instance.InsertProjectLog(projectID, ProjectLogOper.L1_�½���Ŀ, "�½���Ŀ-" + projectName, tran, -1);
                }
                else
                {
                    //�༭
                    projectID = projectInfoModel.ProjectID;
                    projectName = projectInfoModel.Name;
                    projectInfoModel.Notes = ProjectNote;
                    BLL.ProjectInfo.Instance.Update(tran, projectInfoModel);
                }
                //��������
                for (int i = 0; i < dtTasks.Rows.Count; i++)
                {
                    dtTasks.Rows[i]["ProjectID"] = projectID;
                    dtDataSources.Rows[i]["projectID"] = projectID;
                }
                BLL.Loger.Log4Net.Info("��ʼ������������");
                blBulkCopy.DestinationTableName = "LeadsTask";
                blBulkCopy.WriteToServer(dtTasks);

                //�����LeadsTaskOperationLog
                blBulkCopy.ColumnMappings.Clear();
                blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TaskID", "TaskID"));
                blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CreateTime", "CreateTime"));
                blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CreateUserID", "CreateUserID"));
                blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OperationStatus", "OperationStatus"));
                blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Remark", "Remark"));
                blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TaskStatus", "TaskStatus"));

                BLL.Loger.Log4Net.Info("��ʼ��������LeadsTaskOperationLog");
                blBulkCopy.DestinationTableName = "LeadsTaskOperationLog";
                blBulkCopy.WriteToServer(dtLogs);

                //�����ProjectDataSoure
                blBulkCopy.ColumnMappings.Clear();
                blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ProjectID", "ProjectID"));
                blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("RelationID", "RelationID"));
                blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Status", "Status"));
                blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CreateTime", "CreateTime"));
                blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CreateUserID", "CreateUserID"));
                blBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Source", "Source"));
                BLL.Loger.Log4Net.Info("��ʼ��������ProjectDataSoure");
                blBulkCopy.DestinationTableName = "ProjectDataSoure";
                blBulkCopy.WriteToServer(dtDataSources);

                //foreach (Entities.LeadsTask taskItem in taskList)
                //{
                //    taskItem.ProjectID = (int)projectID;
                //    BLL.LeadsTask.Instance.Insert(tran, taskItem);
                //}
                ////��Ŀ��������
                //foreach (Entities.ProjectDataSoure dsItem in dslist)
                //{
                //    dsItem.ProjectID = (int)projectID;
                //    BLL.ProjectDataSoure.Instance.Insert(tran, dsItem);
                //}
                ////��־
                //foreach (Entities.LeadsTaskOperationLog logItem in logList)
                //{
                //    BLL.LeadsTaskOperationLog.Instance.Insert(tran, logItem);
                //}
                //�ύ
                tran.Commit();
                BLL.Loger.Log4Net.Info("�����������ݿ��������: ��ʱ��" + (DateTime.Now - dtNow).TotalMilliseconds.ToString() + " ����");

                //�����ʼ�
                if (newInfoModel != null)
                {
                    SendMail(type, CurrDemandID, LastDealTime, oldCount + dtTasks.Rows.Count, projectID, projectName, expectedNum);
                }
            }
            catch (Exception ex)
            {
                if (tran.Connection != null)
                {
                    tran.Rollback();
                }
                errMsg = ex.StackTrace;
                BLL.Loger.Log4Net.Info("����ȡ���Leads���񡿳�������ID��" + CurrDemandID + "���κţ�" + CurrBathNo.ToString() + "������Ϣ��" + ex.StackTrace);
                UnhandledExceptionFunction(null, new UnhandledExceptionEventArgs(ex, false));
            }
            finally
            {
                connection.Close();
            }
            #endregion
            return errMsg;
        }


        #endregion

        /// ������Ŀ
        /// <summary>
        /// ������Ŀ
        /// </summary>
        /// <param name="CurrDemandID"></param>
        /// <param name="newInfoModel"></param>
        /// <param name="projectInfoModel"></param>
        /// <returns></returns>
        private bool CreateProject(string CurrDemandID, int Source, int PCatageID, int Batch, int? ExpectedNum,
            out Entities.ProjectInfo newInfoModel, out Entities.ProjectInfo projectInfoModel)
        {
            string new_name = "";
            newInfoModel = null;
            if (Source == 5)
            {
                //�׼���
                projectInfoModel = BLL.ProjectInfo.Instance.GetProjectInfoByDemandID(CurrDemandID);
                new_name = "������Լ" + CurrDemandID;
            }
            else if (Source == 6)
            {
                //C����
                projectInfoModel = BLL.ProjectInfo.Instance.GetProjectInfoByDemandID(CurrDemandID, Batch);
                new_name = "������Լ" + CurrDemandID + "D" + Batch.ToString("00");
            }
            else
            {
                projectInfoModel = null;
            }

            if (projectInfoModel == null)
            {
                //û�У�������
                newInfoModel = new Entities.ProjectInfo()
                {
                    BGID = 20,//������ID
                    PCatageID = PCatageID,//������Լ����
                    Name = new_name,
                    Source = Source,
                    CreateTime = DateTime.Now,
                    CreateUserID = -3,
                    Notes = "",
                    Status = 1,

                    DemandID = CurrDemandID,
                    Batch = Batch,
                    ExpectedNum = ExpectedNum,
                    //���̼���Ĭ�Ͽ�ʼ������Ч�飬��֤������cc�ڲ�
                    IsBlacklistCheck = 1,
                    BlacklistCheckType = 2
                };
            }
            else
            {
                if (projectInfoModel.Status == 2)
                {
                    //������ѽ�������ֱ�ӷ���
                    return true;
                }
            }
            return false;
        }

        /// ������������-YJK
        /// <summary>
        /// ������������-YJK
        /// </summary>
        /// <param name="CurrDemandID"></param>
        /// <param name="sourceDt"></param>
        /// <param name="taskList"></param>
        /// <param name="logList"></param>
        /// <param name="dslist"></param>
        /// <param name="DCarMaster"></param>
        /// <param name="DCarSerial"></param>
        /// <returns></returns>
        private string CreateYJKLeadTask(string CurrDemandID, DataTable sourceDt,
            out List<Entities.LeadsTask> taskList, out List<Entities.LeadsTaskOperationLog> logList,
            out List<Entities.ProjectDataSoure> dslist)
        {
            int maxID = BLL.LeadsTask.Instance.GetMaxIDFromTaskID();//��ȡ���ID
            taskList = new List<Entities.LeadsTask>();
            Entities.LeadsTask task = new Entities.LeadsTask();
            logList = new List<Entities.LeadsTaskOperationLog>();
            Entities.LeadsTaskOperationLog log = new Entities.LeadsTaskOperationLog();
            Entities.ProjectDataSoure dsModel;
            dslist = new List<Entities.ProjectDataSoure>();
            //����-��
            int DCarMasterID = 0;
            string DCarMaster = "";
            int DCarSerialID = 0;
            string DCarSerial = "";
            //��Ա��Ϣ
            string DealerID = "";
            string DealerName = "";

            int rowNum = 0;
            foreach (DataRow dr in sourceDt.Rows)
            {
                #region ��ֵƷ�ơ����͡����������
                int OrderSerialID = BitAuto.YanFa.Crm2009.Entities.CommonFunction.ObjectToInteger(dr["OldCSID"]);
                int OrderCarID = BitAuto.YanFa.Crm2009.Entities.CommonFunction.ObjectToInteger(dr["OldCarID"]);
                int OrderMasterID = BLL.CarTypeAPIFromCC.Instance.GetMasterBrandIDBySerialID(OrderSerialID);
                string OrderMasterName = BLL.CarTypeAPIFromCC.Instance.GetMasterNameByMasterID(OrderMasterID);
                string OrderSerialName = BLL.CarTypeAPIFromCC.Instance.GetSerialNameBySerialID(OrderSerialID);
                string OrderCarName = BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(OrderCarID);
                DealerID = dr["OldMemberCode"].ToString();
                BitAuto.YanFa.Crm2009.Entities.DMSMember member = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMemberByMemberCode(DealerID);
                if (member != null)
                {
                    DealerName = member.Name;
                }
                if (rowNum == 0)  //�������ݶ�һ��������ֻ��Ҫ��һ��
                {
                    DCarSerialID = BitAuto.YanFa.Crm2009.Entities.CommonFunction.ObjectToInteger(dr["CSID"]);
                    DCarSerial = BLL.CarTypeAPIFromCC.Instance.GetSerialNameBySerialID(DCarSerialID);
                    DCarMasterID = BLL.CarTypeAPIFromCC.Instance.GetMasterBrandIDBySerialID(DCarSerialID);
                    DCarMaster = BLL.CarTypeAPIFromCC.Instance.GetMasterNameByMasterID(DCarMasterID);
                }
                #endregion

                #region ����
                string TaskID = "YJK" + (++maxID).ToString().PadLeft(7, '0');
                task = new Entities.LeadsTask()
                {
                    TaskID = TaskID,
                    DemandID = CurrDemandID,
                    RelationID = new Guid(dr["ID"].ToString()),
                    UserName = dr["UserName"].ToString(),
                    Tel = dr["UserMobile"].ToString(),
                    Sex = dr["Gender"].ToString() == "0" ? 2 : 1,  //CC��1��2Ů��CRM��1��0Ů
                    ProvinceID = int.Parse(dr["ProvinceID"].ToString()),
                    CityID = int.Parse(dr["CityID"].ToString()),
                    Status = (int)LeadsTaskStatus.NoAllocation,
                    OrderCarMasterID = OrderMasterID,
                    OrderCarMaster = OrderMasterName,
                    OrderCarSerialID = OrderSerialID,
                    OrderCarSerial = OrderSerialName,
                    OrderCarID = OrderCarID,
                    OrderCar = OrderCarName,
                    DealerID = DealerID,
                    DealerName = DealerName,
                    OrderCreateTime = DateTime.Parse(dr["OrderTime"].ToString()),
                    DCarMasterID = DCarMasterID,
                    DCarMaster = DCarMaster,
                    DCarSerialID = DCarSerialID,
                    DCarSerial = DCarSerial,
                    CreateTime = DateTime.Now,
                    CreateUserID = -3,
                    LastDealTime = DateTime.Parse(dr["LastCallTime"].ToString())
                };

                taskList.Add(task);
                #endregion

                //��������������־
                log = CreateLeadsTaskOperationLog(logList, log, TaskID);
                //������Ŀ���ݹ�����
                dsModel = CreateProjectDataSoure(dslist, TaskID, 5);
                rowNum++;
            }
            return DCarMaster + "-" + DCarSerial;
        }

        #region Add=Masj,Date=2015-04-23,ע�͵�
        ///// ������������-CJK
        ///// <summary>
        ///// ������������-CJK
        ///// </summary>
        ///// <param name="CurrDemandID"></param>
        ///// <param name="sourceDt"></param>
        ///// <param name="taskList"></param>
        ///// <param name="logList"></param>
        ///// <param name="dslist"></param>
        ///// <param name="DCarMaster"></param>
        ///// <param name="DCarSerial"></param>
        ///// <returns></returns>
        //private string CreateCJKLeadTask(string CurrDemandID, DataTable sourceDt,
        //    out List<Entities.LeadsTask> taskList, out List<Entities.LeadsTaskOperationLog> logList,
        //    out List<Entities.ProjectDataSoure> dslist)
        //{
        //    int maxID = BLL.LeadsTask.Instance.GetMaxIDFromTaskID();//��ȡ���ID
        //    taskList = new List<Entities.LeadsTask>();
        //    Entities.LeadsTask task = new Entities.LeadsTask();
        //    logList = new List<Entities.LeadsTaskOperationLog>();
        //    Entities.LeadsTaskOperationLog log = new Entities.LeadsTaskOperationLog();
        //    Entities.ProjectDataSoure dsModel;
        //    dslist = new List<Entities.ProjectDataSoure>();
        //    int rowNum = 0;
        //    foreach (DataRow dr in sourceDt.Rows)
        //    {
        //        #region ��ֵƷ�ơ����͡����������
        //        int OrderSerialID = BitAuto.YanFa.Crm2009.Entities.CommonFunction.ObjectToInteger(dr["SerialID"]);
        //        string OrderSerialName = BLL.CarTypeAPIFromCC.Instance.GetSerialNameBySerialID(OrderSerialID);
        //        int OrderMasterID = BLL.CarTypeAPIFromCC.Instance.GetMasterBrandIDBySerialID(OrderSerialID);
        //        string OrderMasterName = BLL.CarTypeAPIFromCC.Instance.GetMasterNameByMasterID(OrderMasterID);
        //        #endregion

        //        #region ����
        //        string TaskID = "CJK" + (++maxID).ToString().PadLeft(7, '0');
        //        task = new Entities.LeadsTask()
        //        {
        //            TaskID = TaskID,
        //            DemandID = CurrDemandID,
        //            UserName = dr["UserName"].ToString(),
        //            Tel = dr["UserMobile"].ToString(),
        //            ProvinceID = int.Parse(dr["ProvinceID"].ToString()),
        //            CityID = int.Parse(dr["CityID"].ToString()),
        //            OrderCreateTime = DateTime.Parse(dr["OrderTime"].ToString()),
        //            Sex = BitAuto.YanFa.Crm2009.Entities.CommonFunction.ObjectToInteger(dr["Sex"]) == 0 ? 2 : 1,  //CC��1��2Ů��CRM��1��0Ů
        //            OrderNum = BitAuto.YanFa.Crm2009.Entities.CommonFunction.ObjectToInteger(dr["RecID"]),//���
        //            TargetCityID = (DBNull.Value == dr["TargetCityID"]) ? "" : dr["TargetCityID"].ToString(),
        //            TargetProvinceID = (DBNull.Value == dr["TargetProvinceID"]) ? "" : dr["TargetProvinceID"].ToString(),

        //            //��Ա
        //            DealerID = null,
        //            DealerName = (DBNull.Value == dr["DealerName"]) ? "" : dr["DealerName"].ToString(),

        //            //�������Ϣ
        //            //�µ�-��
        //            OrderCarSerialID = OrderSerialID,
        //            OrderCarSerial = OrderSerialName,
        //            OrderCarMasterID = OrderMasterID,
        //            OrderCarMaster = OrderMasterName,
        //            //����-��
        //            DCarMasterID = null,
        //            DCarMaster = null,
        //            DCarSerialID = null,
        //            DCarSerial = null,

        //            Status = (int)LeadsTaskStatus.NoAllocation,
        //            CreateTime = DateTime.Now,
        //            CreateUserID = -3,
        //            LastUpdateTime = DateTime.Now,
        //            LastDealTime = DateTime.Parse(dr["LastCallTime"].ToString())
        //        };

        //        taskList.Add(task);
        //        #endregion

        //        //��������������־
        //        log = CreateLeadsTaskOperationLog(logList, log, TaskID);
        //        //������Ŀ���ݹ�����
        //        dsModel = CreateProjectDataSoure(dslist, TaskID, 6);
        //        rowNum++;
        //    }
        //    CJKDemandInfo info = CJKDemandBll.Instance.GetCJKDemandInfo(CurrDemandID);
        //    string carinfo = "";
        //    if (info != null)
        //    {
        //        carinfo = info.BrandName + "-" + info.SerialNames;
        //    }
        //    return carinfo;
        //}
        #endregion

        /// �������
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="type"></param>
        /// <param name="CurrDemandID"></param>
        /// <param name="CurrBathNo"></param>
        /// <param name="errMsg"></param>
        /// <param name="LastDealTime"></param>
        /// <param name="newInfoModel"></param>
        /// <param name="projectInfoModel"></param>
        /// <param name="taskList"></param>
        /// <param name="logList"></param>
        /// <param name="dslist"></param>
        /// <param name="DCarMaster"></param>
        /// <param name="DCarSerial"></param>
        /// <returns></returns>
        private string DataToDB(string type, string CurrDemandID, int CurrBathNo, string errMsg, DateTime LastDealTime,
            Entities.ProjectInfo newInfoModel, Entities.ProjectInfo projectInfoModel, List<Entities.LeadsTask> taskList,
            List<Entities.LeadsTaskOperationLog> logList, List<Entities.ProjectDataSoure> dslist, string carinfo, int? expectedNum)
        {
            int oldCount = 0;
            if (projectInfoModel != null)
            {
                oldCount = BLL.ProjectDataSoure.Instance.GetDataCountByProjectID(projectInfoModel.ProjectID);
            }
            //��ȡ��ע��Ϣ
            string ProjectNote = "";
            if (type == "YJK")
            {
                ProjectNote = GetYJKProjectNote(oldCount + taskList.Count, CurrDemandID, carinfo, LastDealTime);
            }
            else if (type == "CJK")
            {
                ProjectNote = GetCJKProjectNote(oldCount + taskList.Count, CurrDemandID, CurrBathNo, carinfo, LastDealTime);
            }

            long projectID = 0;
            string projectName = "";
            #region �����ύ����
            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");
            SqlBulkCopy blBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, tran);
            blBulkCopy.BulkCopyTimeout = 5000;
            try
            {
                if (newInfoModel != null)
                {
                    //������Ŀ
                    newInfoModel.Notes = ProjectNote;
                    projectID = BLL.ProjectInfo.Instance.Insert(tran, newInfoModel);
                    projectName = newInfoModel.Name;
                    BLL.ProjectLog.Instance.InsertProjectLog(projectID, ProjectLogOper.L1_�½���Ŀ, "�½���Ŀ-" + projectName, tran);
                }
                else
                {
                    //�༭
                    projectID = projectInfoModel.ProjectID;
                    projectName = projectInfoModel.Name;
                    projectInfoModel.Notes = ProjectNote;
                    BLL.ProjectInfo.Instance.Update(tran, projectInfoModel);
                }
                //��������
                foreach (Entities.LeadsTask taskItem in taskList)
                {
                    taskItem.ProjectID = (int)projectID;
                    BLL.LeadsTask.Instance.Insert(tran, taskItem);
                }
                //��Ŀ��������
                foreach (Entities.ProjectDataSoure dsItem in dslist)
                {
                    dsItem.ProjectID = (int)projectID;
                    BLL.ProjectDataSoure.Instance.Insert(tran, dsItem);
                }
                //��־
                foreach (Entities.LeadsTaskOperationLog logItem in logList)
                {
                    BLL.LeadsTaskOperationLog.Instance.Insert(tran, logItem);
                }
                //�ύ
                tran.Commit();
                //�����ʼ�
                if (newInfoModel != null)
                {
                    SendMail(type, CurrDemandID, LastDealTime, oldCount + taskList.Count, projectID, projectName, expectedNum);
                }
            }
            catch (Exception ex)
            {
                if (tran.Connection != null)
                {
                    tran.Rollback();
                }
                errMsg = ex.StackTrace;
                BLL.Loger.Log4Net.Info("����ȡ���Leads���񡿳�������ID��" + CurrDemandID + "���κţ�" + CurrBathNo.ToString() + "������Ϣ��" + ex.StackTrace);
                UnhandledExceptionFunction(null, new UnhandledExceptionEventArgs(ex, false));
            }
            finally
            {
                connection.Close();
            }
            #endregion
            return errMsg;
        }


        /// ��ȡ��Ŀ��ע��Ϣ
        /// <summary>
        /// ��ȡ��Ŀ��ע��Ϣ
        /// </summary>
        /// <param name="count"></param>
        /// <param name="CurrDemandID"></param>
        /// <param name="carinfo"></param>
        /// <param name="LastDealTime"></param>
        /// <returns></returns>
        public string GetYJKProjectNote(int count, string CurrDemandID, string carinfo, DateTime LastDealTime)
        {
            string ProjectNote = "����Ŀ�����󵥺ţ�"
                + CurrDemandID + "��Ŀ��Ʒ�Ƴ��ͣ�"
                + carinfo + "���ɣ���������"
                + count.ToString() + "��������������"
                + LastDealTime.ToString("yyyy-MM-dd");
            return ProjectNote;
        }
        /// ��ȡ��Ŀ��ע��Ϣ
        /// <summary>
        /// ��ȡ��Ŀ��ע��Ϣ
        /// </summary>
        /// <param name="count"></param>
        /// <param name="CurrDemandID"></param>
        /// <param name="CurrBathNo"></param>
        /// <param name="LastDealTime"></param>
        /// <returns></returns>
        public string GetCJKProjectNote(int count, string CurrDemandID, int CurrBathNo, string carinfo, DateTime LastDealTime)
        {
            string ProjectNote = "����Ŀ�����󵥺ţ�"
                + CurrDemandID + "�����κţ�"
                + CurrBathNo + "��Ŀ��Ʒ�Ƴ��ͣ�"
                + carinfo + "���ɣ���������"
                + count.ToString() + "��������������"
                + LastDealTime.ToString("yyyy-MM-dd");
            return ProjectNote;
        }

        /// �����ʼ�
        /// <summary>
        /// �����ʼ�
        /// </summary>
        /// <param name="type">���ͣ�CJK-���̼��ͣ�YJK-�׼��ͣ�</param>
        /// <param name="CurrDemandID">����ID</param>
        /// <param name="LastDealTime">������ʱ��</param>
        /// <param name="totolCount">��������</param>
        /// <param name="projectID">��ĿID</param>
        /// <param name="projectName">��Ŀ����</param>
        /// <param name="expectedNum">Ŀ��ļ�����(Ԥ�������)</param>
        private void SendMail(string type, string CurrDemandID, DateTime LastDealTime, int totolCount, long projectID, string projectName, int? expectedNum)
        {
            string[] userEmail;//���ͽ������б�
            int GetCount = 0;
            switch (type)
            {
                case "YJK"://�׼���-YJK��Ŀ
                    userEmail = ConfigurationManager.AppSettings["YiJiKeProjectEmailInfo"].Trim() != "" ? ConfigurationManager.AppSettings["YiJiKeProjectEmailInfo"].Split(';') : null;
                    GetCount = GetYJKProcessNum(CurrDemandID);
                    break;
                case "CJK"://���̼���-CJK��Ŀ
                    userEmail = ConfigurationManager.AppSettings["CRMCJKProjectEmailInfo"].Trim() != "" ? ConfigurationManager.AppSettings["CRMCJKProjectEmailInfo"].Split(';') : null;
                    //GetCount = GetCJKProcessNum(CurrDemandID);
                    break;
                default: userEmail = null;
                    break;
            }

            //�������������Ŀ�������ʼ�
            if (userEmail != null && userEmail.Length > 0)
            {
                foreach (string item in userEmail)
                {
                    string mailBody1 = item.Split(':')[0];
                    string url = ConfigurationManager.AppSettings["WebBaseURL"] + "/ProjectManage/ViewProject.aspx?projectid="
                        + projectID.ToString();
                    string mailBody2 = "����һ������������Լ������Ŀ���ɡ�<br/><br/>��Ŀ���ƣ�<a href='" + url + "'>"
                        + projectName + "</a><br/><br/>����������" + totolCount.ToString();
                    switch (type)
                    {
                        case "YJK": mailBody2 += "<br/><br/>���������" + ((GetCount < 0) ? 0 : GetCount).ToString();
                            break;
                        case "CJK": mailBody2 += "<br/><br/>Ԥ���������" + ((expectedNum == null) ? 0 : expectedNum.Value).ToString();
                            break;
                        default:
                            break;
                    }
                    mailBody2 += "<br/><br/>���������ڣ�" + LastDealTime.ToString("yyyy-MM-dd");
                    string Title = "����һ���µ���Ŀ����";
                    string[] reciver = new string[] { item.Split(':')[1] };
                    BLL.EmailHelper.Instance.SendMail(mailBody1, mailBody2, Title, reciver);
                }
            }
        }

        /// ��ȡ������-YJK
        /// <summary>
        /// ��ȡ������-YJK
        /// </summary>
        /// <param name="CurrDemandID"></param>
        /// <returns></returns>
        private int GetYJKProcessNum(string CurrDemandID)
        {
            int GetCount = 0;//�账����
            BitAuto.YanFa.Crm2009.Entities.YJKDemand.YJKDemandQuery query = new YanFa.Crm2009.Entities.YJKDemand.YJKDemandQuery();
            query.DemandId = CurrDemandID;
            DataTable DemandTable = BitAuto.YanFa.Crm2009.BLL.YJKDemandBLL.Instance.GetYJKDemandInfo(query, string.Empty);
            if (DemandTable != null && DemandTable.Rows.Count > 0)
            {
                //ʵ�ʼ�������
                int _PracticalNum = 0;
                if (DemandTable.Rows[0]["PracticalNum"] != DBNull.Value)
                {
                    int.TryParse(DemandTable.Rows[0]["PracticalNum"].ToString(), out _PracticalNum);
                }
                //Ŀ�꼯������
                int _ExpectedNum = 0;
                if (DemandTable.Rows[0]["ExpectedNum"] != DBNull.Value)
                {
                    int.TryParse(DemandTable.Rows[0]["ExpectedNum"].ToString(), out _ExpectedNum);
                }

                GetCount = _ExpectedNum - _PracticalNum;//��Ҫ������
            }
            return GetCount;
        }
        /// ��ȡ������-CJK
        /// <summary>
        /// ��ȡ������-CJK
        /// </summary>
        /// <param name="CurrDemandID"></param>
        /// <returns></returns>
        private int GetCJKProcessNum(string CurrDemandID)
        {
            //�账����
            int GetCount = 0;
            BitAuto.YanFa.Crm2009.Entities.CJKDemandInfo info = BitAuto.YanFa.Crm2009.BLL.CJKDemandBll.Instance.GetCJKDemandInfo(CurrDemandID);
            if (info != null)
            {
                //ʵ�ʼ�������
                int _PracticalNum = info.PracticalNum;
                //Ŀ�꼯������
                int _ExpectedNum = info.ExpectedNum;
                //��Ҫ������
                GetCount = _ExpectedNum - _PracticalNum;
            }
            return GetCount;
        }

        /// ������־
        /// <summary>
        /// ������־
        /// </summary>
        /// <param name="logList"></param>
        /// <param name="log"></param>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        private Entities.LeadsTaskOperationLog CreateLeadsTaskOperationLog(List<Entities.LeadsTaskOperationLog> logList,
            Entities.LeadsTaskOperationLog log, string TaskID)
        {
            log = new Entities.LeadsTaskOperationLog()
            {
                TaskID = TaskID,
                CreateTime = DateTime.Now,
                CreateUserID = -3,
                OperationStatus = (int)Leads_OperationStatus.Gen,
                Remark = "��������",
                TaskStatus = (int)LeadsTaskStatus.NoAllocation
            };
            logList.Add(log);
            return log;
        }

        /// ���ɹ�������
        /// <summary>
        /// ���ɹ�������
        /// </summary>
        /// <param name="dslist"></param>
        /// <param name="TaskID"></param>
        /// <param name="Source"></param>
        /// <returns></returns>
        private Entities.ProjectDataSoure CreateProjectDataSoure(List<Entities.ProjectDataSoure> dslist, string TaskID, int Source)
        {
            Entities.ProjectDataSoure dsModel;
            dsModel = new Entities.ProjectDataSoure();
            dsModel.ProjectID = -2;
            dsModel.RelationID = TaskID;
            dsModel.Source = Source;
            dsModel.Status = 1;//1��������
            dsModel.CreateTime = DateTime.Now;
            dsModel.CreateUserID = -3;
            dslist.Add(dsModel);
            return dsModel;
        }

        /// ��ȡ���id
        /// <summary>
        /// ��ȡ���id
        /// </summary>
        /// <returns></returns>
        public int GetMaxIDFromTaskID()
        {
            return Dal.LeadsTask.Instance.GetMaxIDFromTaskID();
        }

        static void UnhandledExceptionFunction(Object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            BLL.Loger.Log4Net.Debug("�������", e);

            string errorMsg = e.Message;
            string stackTrace = e.StackTrace;
            string source = e.Source;

            string mailBody = string.Format("��λ��Ϣ��{0}<br/>����Source��{1}<br/>����StackTrace��{2}<br/>IsTerminating:{3}<br/>",
                errorMsg, source, stackTrace, args.IsTerminating);
            string subject = "�ͻ���������ϵͳ�����������";
            string[] userEmail = ConfigurationManager.AppSettings["ReceiveErrorEmail"].Split(';');
            if (userEmail != null && userEmail.Length > 0)
            {
                BLL.EmailHelper.Instance.SendErrorMail(mailBody, subject, userEmail);
            }
        }
        #endregion

        /// <summary>
        /// ������Ŀ��ȡ��Ŀ�³ɹ�������
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public int GetSumSuccess(int projectid)
        {
            return Dal.LeadsTask.Instance.GetSumSuccess(projectid);
        }
        /// <summary>
        /// �������ݼ����˺��������ݣ������Σ�ÿ��5000
        /// </summary>
        /// <param name="dtSource">ԭ���ݼ�</param>
        /// <param name="CurrDemandID">���󵥺�</param>
        /// <param name="CurrBathNo">����</param>
        /// <returns></returns>
        public DataTable CheckTelBatch(DataTable dtSource, string currDemandID, string currBathNo)
        {
            //ȫ��д�ļ���Ϣ
            string msg = string.Empty;
            //��һ��ȫ�ֵĺ�����table,�����ռ����β��ҵĺ������绰����
            DataTable dt = new DataTable();
            dt.Columns.Add("tel", System.Type.GetType("System.String"));
            //i���ڿ���5000����һ��
            int i = 0;
            //tels���ڴ洢ÿ�β�ѯ�ĵ绰�����ַ���
            string tels = string.Empty;
            foreach (DataRow dr in dtSource.Rows)
            {
                tels += dr["UserMobile"].ToString() + ",";
                i = i + 1;
                //5000����һ�ο⣬���߲���5000�����Ǳ����������һ��
                if (i == 5000 || dr == dtSource.Rows[dtSource.Rows.Count - 1])
                {
                    //ÿ5000����һ�����ݷ���������ĺ������绰�������ذ���������ΪҪ��¼��־
                    DataTable dtBlack = Dal.LeadsTask.Instance.CheckTelBatch(tels.Substring(0, (tels.Length - 1)));
                    if (dtBlack != null)
                    {
                        foreach (DataRow drBlack in dtBlack.Rows)
                        {
                            DataRow dtRow = dt.NewRow();
                            dtRow["tel"] = drBlack["tel"].ToString();
                            dt.Rows.Add(dtRow);
                        }
                    }
                    i = 0;
                    tels = string.Empty;
                }
            }
            //��������������dtSource��
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string tel = row["tel"].ToString();
                    DataRow[] rowExists = dtSource.Select(" UserMobile='" + tel + "'");
                    if (rowExists != null && rowExists.Length > 0)
                    {
                        for (int n = 0; n < rowExists.Length; n++)
                        {
                            dtSource.Rows.Remove(rowExists[n]);
                            msg += "{CurrDemandID:" + currDemandID + ",CurrBathNo:" + currBathNo + ",Tel:" + tel + "},";
                        }
                    }
                }
            }
            //�ļ���¼�ú���������ŵĺ��룬����������
            if (!string.IsNullOrEmpty(msg))
            {
                BLL.Util.LogForService("log", "info", "�����˵ĵ绰������Ϣ��" + msg.Substring(0, msg.Length - 1));
            }
            return dtSource;
        }

        /// <summary>
        /// ���Է���
        /// </summary>
        /// <returns></returns>
        public DataTable GetCrm()
        {
            DataTable dt = Dal.LeadsTask.Instance.GetCrm();
            dt.Columns.Add("LastCallTime");
            foreach (DataRow row in dt.Rows)
            {
                var dtEndDate = DateTime.Now.Date;
                row["LastCallTime"] = dtEndDate.AddDays(3).Date;
            }
            return dt;
        }
    }
}

