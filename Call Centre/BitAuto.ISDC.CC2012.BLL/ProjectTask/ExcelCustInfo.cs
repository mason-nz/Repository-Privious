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
	/// 业务逻辑类ExcelCustInfo 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:33 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ExcelCustInfo
	{
		#region Instance
		public static readonly ExcelCustInfo Instance = new ExcelCustInfo();
		#endregion

		#region Contructor
		protected ExcelCustInfo()
		{}
		#endregion

        #region Select
        public DataTable GetExcelCustInfo_Manage(QueryExcelCustInfo queryExcelCustInfo, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ExcelCustInfo.Instance.GetExcelCustInfo_Manage(queryExcelCustInfo, currentPage, pageSize, out totalCount);
        }
        #endregion

        #region Insert
        public void InsertExcelCustInfo_ForBitch(List<Entities.ExcelCustInfo> models)
        {
            Dal.ExcelCustInfo.Instance.InsertExcelCustInfo_ForBitch(models);
        }
        #endregion

        public Entities.ExcelCustInfo GetExcelCustInfo(int id)
        {
            return Dal.ExcelCustInfo.Instance.GetExcelCustInfo(id);
        }

        /// <summary>
        /// 统计
        /// </summary>
        public void StatNewCustCheckRecordsByUserID(QueryExcelCustInfo query, out int totalCount, out  int noProcessCount, out int processingCount, out  int finishedCount)
        {
            Dal.ExcelCustInfo.Instance.StatNewCustCheckRecordsByUserID(query, out totalCount, out noProcessCount, out processingCount, out finishedCount);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string ids)
        {
            Dal.ExcelCustInfo.Instance.Delete(ids);
        }

        public void Delete(QueryExcelCustInfo query)
        {
            Dal.ExcelCustInfo.Instance.Delete(query);
        }
        public bool HasTaskStatusMoreThanAssign(string custIDs)
        {
            return Dal.ExcelCustInfo.Instance.HasTaskStatusMoreThanAssign(custIDs);
        }

        public bool HasTaskStatusMoreThanAssign(QueryExcelCustInfo query)
        {
            return Dal.ExcelCustInfo.Instance.HasTaskStatusMoreThanAssign(query);
        }

        /// <summary>
        /// 根据客户名称，查询Excel导入列表
        /// </summary>
        /// <param name="custName"></param>
        /// <returns></returns>
        public DataTable GetExcelCustInfoByCustName(string custName)
        {
            return Dal.ExcelCustInfo.Instance.GetExcelCustInfoByCustName(custName);
        }

	}
}

