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
    /// ҵ���߼���ProjectTask_ReturnVisit ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-03-07 03:04:35 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectTask_ReturnVisit
    {
        #region Instance
        public static readonly ProjectTask_ReturnVisit Instance = new ProjectTask_ReturnVisit();
        #endregion

        #region Contructor
        protected ProjectTask_ReturnVisit()
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
        public DataTable GetProjectTask_ReturnVisit(QueryProjectTask_ReturnVisit query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetProjectTask_ReturnVisit(query, order, currentPage, pageSize, out totalCount);
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
        public DataTable GetReturnVisitReCordList(QueryReturnVisitRecord query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetReturnVisitReCordList(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetReturnVisitReCordListExport(QueryReturnVisitRecord query)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetReturnVisitReCordListExport(query);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ProjectTask_ReturnVisit.Instance.GetProjectTask_ReturnVisit(new QueryProjectTask_ReturnVisit(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ProjectTask_ReturnVisit GetProjectTask_ReturnVisit(long RecID)
        {

            return Dal.ProjectTask_ReturnVisit.Instance.GetProjectTask_ReturnVisit(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(long RecID)
        {
            QueryProjectTask_ReturnVisit query = new QueryProjectTask_ReturnVisit();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_ReturnVisit(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.ProjectTask_ReturnVisit model)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ProjectTask_ReturnVisit model)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.ProjectTask_ReturnVisit model)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ProjectTask_ReturnVisit model)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long RecID)
        {

            return Dal.ProjectTask_ReturnVisit.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {

            return Dal.ProjectTask_ReturnVisit.Instance.Delete(sqltran, RecID);
        }

        #endregion

        /// <summary>
        /// ���ݿͻ�idȡcc�ͻ��������б�
        /// ֻ��һ������Ա��
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public DataTable GetCustUserForCCOne(string custid)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetCustUserForCCOne(custid);
        }

        /// <summary>
        /// ���ݿͻ�idȡcc�ͻ��������б�
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        //public DataTable GetCustUserForCC(string custid)
        public DataTable GetCustUserForCC(string custIds, string order, int currentPage, int pageSize, out int totalCount)
        {
            //return Dal.ProjectTask_ReturnVisit.Instance.GetCustUserForCC(custid);
            return Dal.ProjectTask_ReturnVisit.Instance.GetCustUserForCC(custIds, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ɾ��cc�ͻ�������
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int DeleteCustUserMappingForCC(string CustID, string UserID)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.DeleteCustUserMappingForCC(CustID, UserID);
        }



        public void Delete(string custid)
        {
            Dal.ProjectTask_ReturnVisit.Instance.Delete(custid);
        }

        public DataTable GetTable(string custid)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetTable(custid);
        }

        public void updateStatus(string custid)
        {
            Dal.ProjectTask_ReturnVisit.Instance.updateStatus(custid);
        }
        public void InsertCCAddCRMContractLogForRV(int ContractID, int userid)
        {
            Dal.ProjectTask_ReturnVisit.Instance.InsertCCAddCRMContractLogForRV(ContractID, userid);
        }
        public void DeleteCCAddCRMContractLogForRV(int ContractID)
        {
            Dal.ProjectTask_ReturnVisit.Instance.DeleteCCAddCRMContractLogForRV(ContractID);
        }

        /// <summary>
        /// �ж���ϵ�˼�¼�Ƿ��Ǵ�CC��ӵ�
        /// </summary>
        /// <param name="ContractID"></param>
        /// <returns></returns>
        public bool CCAddCRMContractLogForRVIsHave(int ContractID)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.CCAddCRMContractLogForRVIsHave(ContractID);
        }

        /// <summary>
        /// ������Ŀ����ģ��ƥ�伯����Ŀ   
        /// </summary> 
        /// <returns>����</returns>
        public DataTable GetJiCaiProjectByName(string pName, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetJiCaiProjectByName(pName, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ȡ���ʷ���
        /// </summary>
        /// <returns></returns>
        public DataTable GetVisitType()
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetVisitType();
        }
        /// <summary>
        /// ȡ�ط���Ϣ���ݻط�id
        /// </summary>
        /// <returns></returns>
        public DataTable GetVisitInfoByRVID(string RVID)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetVisitInfoByRVID(RVID);
        }

        /// <summary>
        ///�ڻط���ʱ������һ������
        /// </summary>
        public void InsertReturnVisitCallReCord(string CRMCustID, string SessionID)
        {
            Dal.ProjectTask_ReturnVisit.Instance.InsertReturnVisitCallReCord(CRMCustID, SessionID);
        }
        /// <summary>
        /// ȡ�ط���ʱ����Ϣ
        /// </summary>
        /// <param name="CRMCustID"></param>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public DataTable GetReturnVisitCallReCord(string CRMCustID, string SessionID)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetReturnVisitCallReCord(CRMCustID, SessionID);
        }
        /// <summary>
        /// ȡ�ط���ʱ����Ϣ
        /// </summary>
        /// <param name="CRMCustID"></param>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public DataTable GetReturnVisitCallReCord(string CRMCustID)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetReturnVisitCallReCord(CRMCustID);
        }
        /// <summary>
        /// ɾ���ط���ʱ��Ϣ
        /// </summary>
        /// <param name="CRMCustID"></param>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public void DeleteReturnVisitCallReCord(string CRMCustID)
        {
            Dal.ProjectTask_ReturnVisit.Instance.DeleteReturnVisitCallReCord(CRMCustID);
        }

        /// <summary>
        /// �Ƿ���ڸÿͻ��ڸ�ҵ���������������Ӫ�ͷ�
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberIdList"></param>
        /// <returns></returns>
        public bool IsExistsCustMember(int userId,string memberIdList)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.IsExistsCustMember(userId,memberIdList);
        }


        /// <summary>
        /// ɾ��cc�ͻ���ҵ���ߵ���Ӫ�ͷ�
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberIdList"></param>
        /// <returns></returns>
        public int DeleteCustMemberOfBL(int userId, string memberIdList)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.DeleteCustMemberOfBL(userId,memberIdList);
        }

        /// <summary>
        /// �Ƿ�����Ӫ�ͷ�(userclass=7)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsYYKF(int userId)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.IsYYKF(userId);
        }
    }
}

