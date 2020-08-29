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
	/// 业务逻辑类ProjectTask_CSTMember_Brand 的摘要说明。
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
	public class ProjectTask_CSTMember_Brand
	{
		#region Instance
		public static readonly ProjectTask_CSTMember_Brand Instance = new ProjectTask_CSTMember_Brand();
		#endregion

		#region Contructor
		protected ProjectTask_CSTMember_Brand()
		{}
		#endregion

        #region Select

        public DataTable GetProjectTask_CSTMember_MainBrand(int memberId)
        {
            return Dal.ProjectTask_CSTMember_Brand.Instance.GetProjectTask_CSTMember_MainBrand(memberId);
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
        public DataTable GetProjectTask_CSTMember_Brand(QueryProjectTask_CSTMember_Brand query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_CSTMember_Brand.Instance.GetProjectTask_CSTMember_Brand(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public List<Entities.ProjectTask_CSTMember_Brand> GetProjectTask_CSTMember_Brand(int memberId)
        {
            return Dal.ProjectTask_CSTMember_Brand.Instance.GetProjectTask_CSTMember_Brand(memberId);
        }

        /// <summary>
        /// 获取会员下的所有主营品牌ID
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public string GetProjectTask_CSTMember_BrandIDs(int memberId)
        {
            string brandIds = string.Empty;
            List<Entities.ProjectTask_CSTMember_Brand> list = GetProjectTask_CSTMember_Brand(memberId);
            int count = 0;
            foreach (Entities.ProjectTask_CSTMember_Brand info in list)
            {
                brandIds += info.BrandID;
                if (count < list.Count - 1)
                {
                    brandIds += ",";
                }
                count++;
            }

            return brandIds;
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ProjectTask_CSTMember_Brand.Instance.GetProjectTask_CSTMember_Brand(new QueryProjectTask_CSTMember_Brand(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_CSTMember_Brand GetProjectTask_CSTMember_Brand(int CSTMemberID, int BrandID)
        {

            return Dal.ProjectTask_CSTMember_Brand.Instance.GetProjectTask_CSTMember_Brand(CSTMemberID, BrandID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByCSTMemberIDAndBrandID(int CSTMemberID, int BrandID)
        {
            QueryProjectTask_CSTMember_Brand query = new QueryProjectTask_CSTMember_Brand();
            query.CSTMemberID = CSTMemberID;
            query.BrandID = BrandID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_CSTMember_Brand(query, string.Empty, 1, 1, out count);
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
        public void Insert(Entities.ProjectTask_CSTMember_Brand model)
        {
            Dal.ProjectTask_CSTMember_Brand.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ProjectTask_CSTMember_Brand model)
        {
            return Dal.ProjectTask_CSTMember_Brand.Instance.Update(model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int CSTMemberID, int BrandID)
        {

            return Dal.ProjectTask_CSTMember_Brand.Instance.Delete(CSTMemberID, BrandID);
        }

        /// <summary>
        /// 删除会员下的主营品牌
        /// </summary>
        /// <param name="CSTMemberID"></param>
        /// <returns></returns>
        public int Delete(int CSTMemberID)
        {
            return Dal.ProjectTask_CSTMember_Brand.Instance.Delete(CSTMemberID);
        }

        /// <summary>
        /// 删除会员下的主营品牌
        /// </summary>
        /// <param name="tid">任务ID</param>
        /// <returns></returns>
        public int DeleteByTID(string tid)
        {
            return Dal.ProjectTask_CSTMember_Brand.Instance.DeleteByTID(tid);
        }
        #endregion

        #region IsExist
        /// <summary>
        /// 是否存在此主营品牌
        /// </summary>
        /// <param name="cstMemberId"></param>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public bool IsExist(int cstMemberId, int brandId)
        {
            return Dal.ProjectTask_CSTMember_Brand.Instance.IsExist(cstMemberId, brandId);
        }
        #endregion

        public Entities.ProjectTask_CSTMember_Brand GetProjectTask_CSTMember_Brand_ID1(int ID)
        {
            return Dal.ProjectTask_CSTMember_Brand.Instance.GetProjectTask_CSTMember_Brand1(ID);
        }

	}
}

