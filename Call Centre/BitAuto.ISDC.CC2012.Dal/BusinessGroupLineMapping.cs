using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类BusinessGroupLineMapping。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2015-04-09 06:39:11 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class BusinessGroupLineMapping : DataBase
    {
        public static readonly BusinessGroupLineMapping Instance = new BusinessGroupLineMapping();

        private const string P_BUSINESSGROUPLINEMAPPING_INSERT = "p_BusinessGroupLineMapping_Insert";

        protected BusinessGroupLineMapping() { }

        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(Entities.BusinessGroupLineMapping model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@LineID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.LineID;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateUserID;
            parameters[5].Value = DateTime.Now;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_BUSINESSGROUPLINEMAPPING_INSERT, parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int bgid)
        {
            string sql = "DELETE FROM BusinessGroupLineMapping WHERE BGID=" + bgid;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
    }
}

