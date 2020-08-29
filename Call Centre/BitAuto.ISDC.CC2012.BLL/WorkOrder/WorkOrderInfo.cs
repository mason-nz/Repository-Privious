using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���WorkOrderInfo ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-08-23 10:24:21 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class WorkOrderInfo
    {
        #region Instance
        public static readonly WorkOrderInfo Instance = new WorkOrderInfo();
        #endregion

        #region Contructor
        protected WorkOrderInfo()
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
        public DataTable GetWorkOrderInfo(QueryWorkOrderInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderInfo.Instance.GetWorkOrderInfo(query, order, currentPage, pageSize, out totalCount);
        }
        public DataTable GetWorkOrderInfoForDemandInfo(QueryWorkOrderInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderInfo.Instance.GetWorkOrderInfoForDemandInfo(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// ���ղ�ѯ������ѯ(��������Ȩ��)
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetWorkOrderInfoForList(QueryWorkOrderInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderInfo.Instance.GetWorkOrderInfoForList(query, order, currentPage, pageSize, out totalCount, BLL.Loger.Log4Net);
        }
        /// <summary>
        /// ��ѯԱ������Ŀͻ��µĹ���
        /// </summary>
        /// <param name="query"></param>
        /// <param name="userId"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderInfoByUserID(QueryWorkOrderInfo query, int userId, string departmentId, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderInfo.Instance.GetWorkOrderInfoByUserID(query, userId, departmentId, order, currentPage, pageSize, out totalCount, BLL.Loger.Log4Net);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderInfoForExport(QueryWorkOrderInfo query, int workCategory, int userId, string order)
        {
            return Dal.WorkOrderInfo.Instance.GetWorkOrderInfoForExport(query, workCategory, userId, order);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.WorkOrderInfo.Instance.GetWorkOrderInfo(new QueryWorkOrderInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.WorkOrderInfo GetWorkOrderInfo(string OrderID)
        {

            return Dal.WorkOrderInfo.Instance.GetWorkOrderInfo(OrderID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByOrderID(string OrderID)
        {
            QueryWorkOrderInfo query = new QueryWorkOrderInfo();
            query.OrderID = OrderID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderInfo(query, string.Empty, 1, 1, out count);
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
        public string Insert(Entities.WorkOrderInfo model)
        {
            //���������������ֶΣ�BGID��SCID Ϊ�˼����޸ĵط�ֱ����Insert��������� lxw 13-10-11
            int loginID = Util.GetLoginUserID();
            model.BGID = SurveyCategory.Instance.GetSelfBGIDByUserID(loginID);
            model.SCID = SurveyCategory.Instance.GetSelfSCIDByUserID(loginID);

            return Dal.WorkOrderInfo.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string InsertWorkOrderInfo(Entities.WorkOrderInfo model)
        {
            return Dal.WorkOrderInfo.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public string Insert(SqlTransaction sqltran, Entities.WorkOrderInfo model)
        {
            return Dal.WorkOrderInfo.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.WorkOrderInfo model)
        {
            return Dal.WorkOrderInfo.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.WorkOrderInfo model)
        {
            return Dal.WorkOrderInfo.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(string OrderID)
        {

            return Dal.WorkOrderInfo.Instance.Delete(OrderID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, string OrderID)
        {

            return Dal.WorkOrderInfo.Instance.Delete(sqltran, OrderID);
        }

        #endregion

        /// <summary>
        /// ���� ���ȡ������
        /// </summary> 
        /// <returns></returns>
        public int GetMax()
        {
            return Dal.WorkOrderInfo.Instance.GetMax();
        }

        public DataTable GetProcessOrderUserID(string orderID)
        {
            return Dal.WorkOrderInfo.Instance.GetProcessOrderUserID(orderID);
        }

        /// <summary>
        /// ��ȡ���й����Ĵ�����
        /// </summary>
        /// <returns></returns>
        public DataTable GetCreateUser(string where, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderInfo.Instance.GetCreateUser(where, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ���ݹ���IDȡ�ͻ�ID
        /// </summary>
        /// <param name="WorkOrderID"></param>
        /// <returns></returns>
        public string GetCustIDByWorkOrderTel(Entities.WorkOrderInfo model)
        {
            if (model == null) return "";
            return Dal.WorkOrderInfo.Instance.GetCustIDByWorkOrderID(model.OrderID);
            //return BLL.CustBasicInfo.Instance.GetMaxNewCustBasicInfoByTel(model.ContactTel);
        }

        /// <summary>
        /// ��ѯĳ���ż����Ӳ���
        /// </summary>
        /// <param name="departID"></param>
        /// <returns></returns>
        public DataTable GetChildDepartMent(string[] DepartIDs)
        {
            return Dal.WorkOrderInfo.Instance.GetChildDepartMent(DepartIDs);
        }

        /// <summary>
        /// ���ݹ���ID��ȡ¼��URL
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderRecordUrl_OrderID(string orderid)
        {
            return Dal.WorkOrderInfo.Instance.GetWorkOrderRecordUrl_OrderID(orderid);
        }

        /// <summary>
        /// ���򳵹�������
        /// </summary>
        /// <param name="query">WorkCategory(1:����2:������)</param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable WorkOrderInfoExportHMC(QueryWorkOrderInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderInfo.Instance.WorkOrderInfoExportHMC(query, order, currentPage, pageSize, out totalCount);
        }

        public bool HasConversation(string OrderID)
        {
            return Dal.WorkOrderInfo.Instance.HasConversation(OrderID);
        }
    }
}

