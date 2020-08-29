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
    /// 业务逻辑类ProjectTask_ReturnVisit 的摘要说明。
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
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetProjectTask_ReturnVisit(QueryProjectTask_ReturnVisit query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetProjectTask_ReturnVisit(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetReturnVisitReCordList(QueryReturnVisitRecord query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetReturnVisitReCordList(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetReturnVisitReCordListExport(QueryReturnVisitRecord query)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetReturnVisitReCordListExport(query);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ProjectTask_ReturnVisit.Instance.GetProjectTask_ReturnVisit(new QueryProjectTask_ReturnVisit(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_ReturnVisit GetProjectTask_ReturnVisit(long RecID)
        {

            return Dal.ProjectTask_ReturnVisit.Instance.GetProjectTask_ReturnVisit(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
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
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.ProjectTask_ReturnVisit model)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ProjectTask_ReturnVisit model)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ProjectTask_ReturnVisit model)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ProjectTask_ReturnVisit model)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long RecID)
        {

            return Dal.ProjectTask_ReturnVisit.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {

            return Dal.ProjectTask_ReturnVisit.Instance.Delete(sqltran, RecID);
        }

        #endregion

        /// <summary>
        /// 根据客户id取cc客户负责人列表
        /// 只有一个负责员工
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public DataTable GetCustUserForCCOne(string custid)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetCustUserForCCOne(custid);
        }

        /// <summary>
        /// 根据客户id取cc客户负责人列表
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
        /// 删除cc客户负责人
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
        /// 判断联系人记录是否是从CC添加的
        /// </summary>
        /// <param name="ContractID"></param>
        /// <returns></returns>
        public bool CCAddCRMContractLogForRVIsHave(int ContractID)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.CCAddCRMContractLogForRVIsHave(ContractID);
        }

        /// <summary>
        /// 根据项目名称模糊匹配集采项目   
        /// </summary> 
        /// <returns>表集合</returns>
        public DataTable GetJiCaiProjectByName(string pName, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetJiCaiProjectByName(pName, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 取访问分类
        /// </summary>
        /// <returns></returns>
        public DataTable GetVisitType()
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetVisitType();
        }
        /// <summary>
        /// 取回访信息根据回访id
        /// </summary>
        /// <returns></returns>
        public DataTable GetVisitInfoByRVID(string RVID)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetVisitInfoByRVID(RVID);
        }

        /// <summary>
        ///在回访临时表增加一条数据
        /// </summary>
        public void InsertReturnVisitCallReCord(string CRMCustID, string SessionID)
        {
            Dal.ProjectTask_ReturnVisit.Instance.InsertReturnVisitCallReCord(CRMCustID, SessionID);
        }
        /// <summary>
        /// 取回访临时表信息
        /// </summary>
        /// <param name="CRMCustID"></param>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public DataTable GetReturnVisitCallReCord(string CRMCustID, string SessionID)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetReturnVisitCallReCord(CRMCustID, SessionID);
        }
        /// <summary>
        /// 取回访临时表信息
        /// </summary>
        /// <param name="CRMCustID"></param>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public DataTable GetReturnVisitCallReCord(string CRMCustID)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.GetReturnVisitCallReCord(CRMCustID);
        }
        /// <summary>
        /// 删除回访临时信息
        /// </summary>
        /// <param name="CRMCustID"></param>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public void DeleteReturnVisitCallReCord(string CRMCustID)
        {
            Dal.ProjectTask_ReturnVisit.Instance.DeleteReturnVisitCallReCord(CRMCustID);
        }

        /// <summary>
        /// 是否存在该客户在该业务线上所分配的运营客服
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberIdList"></param>
        /// <returns></returns>
        public bool IsExistsCustMember(int userId,string memberIdList)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.IsExistsCustMember(userId,memberIdList);
        }


        /// <summary>
        /// 删除cc客户该业务线的运营客服
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberIdList"></param>
        /// <returns></returns>
        public int DeleteCustMemberOfBL(int userId, string memberIdList)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.DeleteCustMemberOfBL(userId,memberIdList);
        }

        /// <summary>
        /// 是否是运营客服(userclass=7)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsYYKF(int userId)
        {
            return Dal.ProjectTask_ReturnVisit.Instance.IsYYKF(userId);
        }
    }
}

