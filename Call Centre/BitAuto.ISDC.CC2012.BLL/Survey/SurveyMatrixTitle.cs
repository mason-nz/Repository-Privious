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
	/// 业务逻辑类SurveyMatrixTitle 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-10-24 10:32:18 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class SurveyMatrixTitle
	{
		#region Instance
		public static readonly SurveyMatrixTitle Instance = new SurveyMatrixTitle();
		#endregion

		#region Contructor
		protected SurveyMatrixTitle()
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
		public DataTable GetSurveyMatrixTitle(QuerySurveyMatrixTitle query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(query,order,currentPage,pageSize,out totalCount);
		}

        /// <summary>
        /// 获取某试题下纵坐标或横坐标的个数
        /// </summary>
        /// <param name="sqid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetSurveyMatrixTitleCount(int sqid, int type)
        {
            return Dal.SurveyMatrixTitle.Instance.GetSurveyMatrixTitleCount(sqid, type);
        }
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(new QuerySurveyMatrixTitle(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

        /// <summary>
        /// 统计单个项
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public DataTable StatOptionForMatrixRadio(int SMTID,int SPIID)
        {
            return Dal.SurveyMatrixTitle.Instance.StatOptionForMatrixRadio(SMTID, SPIID);
        }

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.SurveyMatrixTitle GetSurveyMatrixTitle(int SMTID)
		{
			
			return Dal.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(SMTID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsBySMTID(int SMTID)
		{
			QuerySurveyMatrixTitle query = new QuerySurveyMatrixTitle();
			query.SMTID = SMTID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetSurveyMatrixTitle(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.SurveyMatrixTitle model)
		{
			return Dal.SurveyMatrixTitle.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.SurveyMatrixTitle model)
		{
			return Dal.SurveyMatrixTitle.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.SurveyMatrixTitle model)
		{
			return Dal.SurveyMatrixTitle.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.SurveyMatrixTitle model)
		{
			return Dal.SurveyMatrixTitle.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int SMTID)
		{
			
			return Dal.SurveyMatrixTitle.Instance.Delete(SMTID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int SMTID)
		{
			
			return Dal.SurveyMatrixTitle.Instance.Delete(sqltran, SMTID);
		}

		#endregion




        public List<Entities.SurveyMatrixTitle> GetMatrixTitleList(int siid)
        {
            return Dal.SurveyMatrixTitle.Instance.GetMatrixTitleList(siid);
        }

        /// <summary>
        /// 根据问卷ID取问卷下所有矩阵行列
        /// </summary>
        /// <param name="SIID"></param>
        /// <returns></returns>
        public DataTable GetAllMatrixDataTableBySIID(int SIID)
        {
            DataTable dt = null;
            int RowCount = 0;
            QuerySurveyMatrixTitle query = new QuerySurveyMatrixTitle();
            query.SIID = SIID;
            query.Status = 0;
            dt = GetSurveyMatrixTitle(query, "", 1, 100000, out RowCount);
            return dt;
        }

        /// <summary>
        /// 取矩阵行列
        /// </summary>
        /// <param name="SQID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public DataTable GetMatrixDataTable(DataTable MatrixDataTable, int SQID, int Type)
        {
            DataTable dt = null;
            //int RowCount = 0;
            //QuerySurveyMatrixTitle query = new QuerySurveyMatrixTitle();
            //query.SQID = SQID;
            //query.Type = Type;
            //query.Status = 0;
            //dt = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(query, "", 1, 100000, out RowCount);
            if (MatrixDataTable != null)
            {
                dt = MatrixDataTable.Clone();
                DataView dv = MatrixDataTable.DefaultView;
                dv.RowFilter = "SQID=" + SQID + " and Type=" + Type;
                if (dv != null && dv.Count > 0)
                {
                    for (int i = 0; i < dv.Count; i++)
                    {
                        DataRow row = dt.NewRow();
                        row["SMTID"] = dv[i]["SMTID"];
                        row["SIID"] = dv[i]["SIID"];
                        row["SQID"] = dv[i]["SQID"];
                        row["TitleName"] = dv[i]["TitleName"];
                        row["Status"] = dv[i]["Status"];
                        row["Type"] = dv[i]["Type"];
                        row["CreateTime"] = dv[i]["CreateTime"];
                        row["CreateUserID"] = dv[i]["CreateUserID"];
                        dt.Rows.Add(row);
                    }
                }
            }
            return dt;
        }
    }
}

