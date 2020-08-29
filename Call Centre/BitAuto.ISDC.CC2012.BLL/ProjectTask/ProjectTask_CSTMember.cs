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
	/// 业务逻辑类ProjectTask_CSTMember 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:29 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ProjectTask_CSTMember
	{
		#region Instance
		public static readonly ProjectTask_CSTMember Instance = new ProjectTask_CSTMember();
		#endregion

		#region Contructor
		protected ProjectTask_CSTMember()
		{}
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
        public DataTable GetProjectTask_CSTMember(QueryProjectTask_CSTMember query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(query, order, currentPage, pageSize, out totalCount);
        }
        public int GetIDByCSTRecID(string CSTRecID)
        {
            return Dal.ProjectTask_CSTMember.Instance.GetIDByCSTRecID(CSTRecID);
        }
        public string GetIDByCSTTID(int ID)
        {
            return Dal.ProjectTask_CSTMember.Instance.GetIDByCSTTID(ID);
        }
        /// <summary>
        /// 获取车商通全称变更记录
        /// </summary>
        /// <param name="memberID"></param>
        /// <returns></returns>
        public DataTable GetCstMemberFullNameHistory(string originalCSTRecID)
        {
            DataTable dt = null;
            try
            {
                dt = BLL.ProjectTask_AuditContrastInfo.Instance.GetProjectTask_AuditContrastInfo(originalCSTRecID, 7, "FullName");
                DataColumn dcFullName = new DataColumn("FullName");
                dt.Columns.Add(dcFullName);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string ContrastInfoInside = dr["ContrastInfoInside"].ToString();
                        string[] array = ContrastInfoInside.Split(',');
                        for (int i = 0; i < array.Length; i += 2)
                        {
                            if (array[i].Trim() == "")
                            {
                                continue;
                            }
                            string colName = array[i].Split(':')[0];
                            string colValue = array[i].Split('(')[1].Trim('\'');
                            if (colName.Equals("FullName"))
                            {
                                dr["FullName"] = BLL.Util.UnEscapeString(colValue);
                            }
                        }

                    }

                }
            }
            catch (Exception e)
            {

            }
            return dt;
        }
        /// <summary>
        /// 根据任务编号查询车商通会员信息
        /// </summary>
        /// <param name="TID"></param>
        /// <returns></returns>
        public List<Entities.ProjectTask_CSTMember> GetProjectTask_CSTMemberByTID(string TID)
        {
            return Dal.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberByTID(TID);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(new QueryProjectTask_CSTMember(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_CSTMember GetProjectTask_CSTMember(int ID)
        {

            return Dal.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(ID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Entities.ProjectTask_CSTMember GetProjectTask_CSTMemberModel(int ID)
        {
            return Dal.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberModel(ID);
        }
        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByID(int ID)
        {
            QueryProjectTask_CSTMember query = new QueryProjectTask_CSTMember();
            query.ID = ID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_CSTMember(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否存在同一名称的会员
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool IsExistSameName(string where)
        {
            return Dal.ProjectTask_CSTMember.Instance.IsExistSameName(where);
        }
        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.ProjectTask_CSTMember model)
        {
            return Dal.ProjectTask_CSTMember.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ProjectTask_CSTMember model)
        {
            return Dal.ProjectTask_CSTMember.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCstMember(Entities.ProjectTask_CSTMember model)
        {
            Dal.ProjectTask_CSTMember_Brand.Instance.Delete(model.ID);
            Dal.ProjectTask_CSTLinkMan.Instance.DeleteByCstMemberID(model.ID);
            return Dal.ProjectTask_CSTMember.Instance.Update(model);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int ID)
        {
            Entities.ProjectTask_CSTMember c = this.GetProjectTask_CSTMember(ID);
            if (c == null) { throw new Exception("无法得到此会员"); }

            if (c.OriginalCSTRecID != null) { throw new Exception("不可删除CRM中已有的会员"); }


            Dal.ProjectTask_CSTMember_Brand.Instance.Delete(ID);
            Dal.ProjectTask_CSTLinkMan.Instance.DeleteByCstMemberID(ID);
            return Dal.ProjectTask_CSTMember.Instance.Delete(ID);
        }

        /// <summary>
        /// 根据任务ID删除新增的车商通会员
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public int DeleteNewProjectTask_CSTMemberByTID(string tid)
        {
            return Dal.ProjectTask_CSTMember.Instance.DeleteNewProjectTask_CSTMemberByTID(tid);
        }
        /// <summary>
        /// 根据任务ID，更新表ProjectTask_CSTMember，字段Status为-1
        /// </summary>
        /// <param name="tid">任务ID</param>
        /// <returns></returns>
        public int DeleteByTID(string tid)
        {
            BLL.ProjectTask_CSTLinkMan.Instance.DeleteByTID(tid);//删除车商通会员联系人
            BLL.ProjectTask_CSTMember_Brand.Instance.DeleteByTID(tid);//删除车商通会员主营品牌信息
            return Dal.ProjectTask_CSTMember.Instance.DeleteByTID(tid);
        }
        #endregion

        public DataTable GetProjectTask_CSTMembersBySourceCC(QueryProjectTask_CSTMember query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberBySourceCC(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 根据条件查询从呼叫中心系统创建的车商通会员申请状态——统计数
        /// </summary>
        /// <param name="query"></param>
        /// <param name="applyForCount"></param>
        /// <param name="createSuccessfulCount"></param>
        /// <param name="createUnsuccessful"></param>
        /// <param name="rejected"></param>
        public void StatProjectTask_CSTMembersBySourceCC(Entities.QueryProjectTask_CSTMember query, out int applyForCount, out  int createSuccessfulCount, out  int createUnsuccessful, out int rejected)
        {
            DataTable dt = Dal.ProjectTask_CSTMember.Instance.StatProjectTask_CSTMemberBySourceCC(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                applyForCount = GetStatCountBySyncStatus(dt, (int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.ApplyFor);
                createSuccessfulCount = GetStatCountBySyncStatus(dt, (int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.CreateSuccessful);
                createUnsuccessful = GetStatCountBySyncStatus(dt, (int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.CreateUnsuccessful);
                rejected = GetStatCountBySyncStatus(dt, (int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.Rejected);
            }
            else
            {
                applyForCount = createSuccessfulCount = createUnsuccessful = rejected = 0;
            }
        }

        private int GetStatCountBySyncStatus(DataTable dt, int CSTSyncStatus)
        {
            int count = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["SyncStatus"].ToString().Trim() == CSTSyncStatus.ToString())
                    {
                        count = int.Parse(dr["SyncStatusCount"].ToString().Trim());
                        break;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 查询是否是已开通的会员
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public bool IsOpenSuccessMember(int memberId)
        {
            bool isSuccessOpen = false;
            //查询是否是开通的会员
            Entities.ProjectTask_CSTMember cstMember = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(memberId);
            if (!string.IsNullOrEmpty(cstMember.OriginalCSTRecID))
            {
                BitAuto.YanFa.Crm2009.Entities.CstMember cstMemberInfo = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetCstMemberModel(cstMember.OriginalCSTRecID);

                if (cstMemberInfo != null)
                {
                    if (cstMemberInfo.SyncStatus == (int)BitAuto.YanFa.Crm2009.Entities.EnumCSTSyncStatus.CreateSuccessful)
                    {
                        isSuccessOpen = true;
                    }
                }
            }

            return isSuccessOpen;
        }



        public void ProjectTask_CSTMemberDelete(int memberId)
        {
            Entities.ProjectTask_CSTMember c = this.GetProjectTask_CSTMember(memberId);
            if (c == null) { throw new Exception("无法得到此会员"); }

            Dal.ProjectTask_CSTMember.Instance.Delete(memberId);
        }

        /// <summary>
        /// 获取要导出的排期信息
        /// </summary>
        /// <param name="MemberStr"></param>
        /// <returns></returns>
        public DataTable GetOrderInfo(string MemberStr)
        {
            return Dal.ProjectTask_CSTMember.Instance.GetOrderInfo(MemberStr);
        }

	}
}

