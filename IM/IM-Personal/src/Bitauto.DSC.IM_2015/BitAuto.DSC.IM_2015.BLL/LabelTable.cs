using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.BLL
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 业务逻辑类LabelTable 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:04 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class LabelTable
	{
		#region Instance
		public static readonly LabelTable Instance = new LabelTable();
		#endregion

		#region Contructor
		protected LabelTable()
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
		public DataTable GetLabelTable(QueryLabelTable query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.LabelTable.Instance.GetLabelTable(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
            //数据权限：所属区域
			int totalCount=0;
            string areastr = BLL.BaseData.Instance.GetAgentRegionByUserID(BLL.Util.GetLoginUserID().ToString());
            int areaint;
            if (int.TryParse(areastr, out areaint))
            {
                ;
            }
            else
            {
                areaint = -1;
            }
            return Dal.LabelTable.Instance.GetLabelTable(new QueryLabelTable() { AreaType = areaint }, string.Empty, 1, 1000000, out totalCount);
		}
        /// <summary>
        /// 获取全部标签 及 标识是否所属当前组
        /// </summary>
        /// <param name="bgid"></param>
        /// <returns></returns>
        public DataTable GetLabelTableByBGID(int bgid,int region)
        {
            return Dal.LabelTable.Instance.GetLabelTableByBGID(bgid, region);
        }
		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.LabelTable GetLabelTable(int LTID)
		{
			
			return Dal.LabelTable.Instance.GetLabelTable(LTID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByLTID(int LTID)
		{
			QueryLabelTable query = new QueryLabelTable();
			query.LTID = LTID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetLabelTable(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.LabelTable model)
		{
			return Dal.LabelTable.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.LabelTable model)
		{
			return Dal.LabelTable.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.LabelTable model)
		{
			return Dal.LabelTable.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.LabelTable model)
		{
			return Dal.LabelTable.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int LTID)
		{
			
			return Dal.LabelTable.Instance.Delete(LTID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int LTID)
		{
			
			return Dal.LabelTable.Instance.Delete(sqltran, LTID);
		}

		#endregion

        /// 上下移动数据
        /// <summary>
        /// 上下移动数据
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="sortnum"></param>
        /// <param name="type">1上-1下</param>
        /// <returns></returns>
        public bool MoveUpOrDown(int recid, int sortnum, int type)
        {
            return Dal.LabelTable.Instance.MoveUpOrDown(recid, sortnum, type);
        }
	}
}

