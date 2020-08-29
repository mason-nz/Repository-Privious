using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.BLL
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 业务逻辑类ComSentence 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:00 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ComSentence
	{
		#region Instance
		public static readonly ComSentence Instance = new ComSentence();
		#endregion

		#region Contructor
		protected ComSentence()
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
		public DataTable GetComSentence(QueryComSentence query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.ComSentence.Instance.GetComSentence(query,order,currentPage,pageSize,out totalCount);
		}

        public DataTable GetComSentenceList(QueryComSentence query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ComSentence.Instance.GetComSentenceList(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetAllLableWithCM(int bgid)
	    {
            return Dal.ComSentence.Instance.GetAllLableWithCM(bgid);
	    }

	    /// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ComSentence.Instance.GetComSentence(new QueryComSentence(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.ComSentence GetComSentence(int CSID)
		{
			
			return Dal.ComSentence.Instance.GetComSentence(CSID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByCSID(int CSID)
		{
			QueryComSentence query = new QueryComSentence();
			query.CSID = CSID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetComSentence(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.ComSentence model)
		{
			return Dal.ComSentence.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.ComSentence model)
		{
			return Dal.ComSentence.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.ComSentence model)
		{
			return Dal.ComSentence.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ComSentence model)
		{
			return Dal.ComSentence.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int CSID)
		{
			
			return Dal.ComSentence.Instance.Delete(CSID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int CSID)
		{
			
			return Dal.ComSentence.Instance.Delete(sqltran, CSID);
		}

		#endregion

        #region 查询标签是否在常用语中被使用过
        public bool LabelIsUsedInCS(int LTID)
        {
            Entities.QueryComSentence query = new QueryComSentence();
            query.LTID = LTID;

            int totoal;
            DataTable dt = GetComSentence(query, "", 1, 9999, out totoal);

            if (dt != null && dt.Rows.Count>0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 查询同一标签下常用语是否有重复
        public bool IsRepeatLableCS(int LTID,string CSName)
        {
            Entities.QueryComSentence query = new QueryComSentence();
            query.LTID = LTID;
            query.Name = CSName;

            int totoal;
            DataTable dt = GetComSentence(query, "", 1, 9999, out totoal);

            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region 查询该标签是否已被常用语使用
        public bool IsLabelUsedInCS(int LTID)
        {
            Entities.QueryComSentence query = new QueryComSentence();
            query.LTID = LTID;

            int totoal;
            DataTable dt = GetComSentence(query, "", 1, 9999, out totoal);

            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }
        #endregion

        public bool MoveUpOrDown(Entities.QueryComSentence query, int sortnum, int type)
        {
            return Dal.ComSentence.Instance.MoveUpOrDown(query, sortnum, type);
        }
    }
}

