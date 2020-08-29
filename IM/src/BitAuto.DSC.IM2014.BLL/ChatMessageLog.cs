using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM2014.Entities;
using BitAuto.DSC.IM2014.Entities.Constants;

namespace BitAuto.DSC.IM2014.BLL
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 业务逻辑类ChatMessageLog 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-03-05 10:05:58 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ChatMessageLog
	{
		#region Instance
		public static readonly ChatMessageLog Instance = new ChatMessageLog();
		#endregion

		#region Contructor
		protected ChatMessageLog()
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
		public DataTable GetChatMessageLog(QueryChatMessageLog query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.ChatMessageLog.Instance.GetChatMessageLog(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ChatMessageLog.Instance.GetChatMessageLog(new QueryChatMessageLog(),string.Empty,1,1000000,out totalCount);
		}

        /// <summary>
        /// 根据会话ID获取聊天记录
        /// </summary>
        /// <param name="AllocID"></param>
        /// <returns></returns>
        public string GetChatMessageLog(int AllocID)
        {
            QueryChatMessageLog query = new QueryChatMessageLog();
            query.AllocID = AllocID;

            int totalCount = 0;
            DataTable dt = Dal.ChatMessageLog.Instance.GetChatMessageLog(query, " cm.CreateTime asc", 1, 99999, out totalCount);

            //注：消息内容一定要去掉英文引号、回车换行符、回车、换行
            string tmsg = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    tmsg += "{\"name\":\"" + row["UserName"] + "\",\"content\":\"" + row["Content"].ToString().Trim().Replace("\"", "\\\"").Replace("\r\n", "&#13;&#10;").Replace("\r", "&#13;").Replace("\n", "&#10;") + "\",\"rectime\":\"" + row["CreateTime"] + "\"},";
                }
                if (tmsg.EndsWith(","))
                    tmsg = tmsg.Substring(0, tmsg.Length - 1);

                //if (dt.Rows.Count > 1)
                tmsg = "[" + tmsg + "]";
            }

            

            return tmsg;
        }

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.ChatMessageLog GetChatMessageLog(long MessageID)
		{
			
			return Dal.ChatMessageLog.Instance.GetChatMessageLog(MessageID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByMessageID(long MessageID)
		{
			QueryChatMessageLog query = new QueryChatMessageLog();
			query.MessageID = MessageID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetChatMessageLog(query, string.Empty, 1, 1, out count);
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
		public long  Insert(Entities.ChatMessageLog model)
		{
			return Dal.ChatMessageLog.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.ChatMessageLog model)
		{
			return Dal.ChatMessageLog.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.ChatMessageLog model)
		{
			return Dal.ChatMessageLog.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ChatMessageLog model)
		{
			return Dal.ChatMessageLog.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(long MessageID)
		{
			
			return Dal.ChatMessageLog.Instance.Delete(MessageID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long MessageID)
		{
			
			return Dal.ChatMessageLog.Instance.Delete(sqltran, MessageID);
		}

		#endregion

	}
}

