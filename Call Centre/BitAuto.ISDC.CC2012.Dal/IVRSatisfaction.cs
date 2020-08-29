using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
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
    /// 数据访问类IVRSatisfaction。
    /// </summary>
    /// <author>
    /// lihf
    /// </author>
    /// <history>
    /// 2013-07-16 12:11:00 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class IVRSatisfaction : DataBase
    {
        public static readonly IVRSatisfaction Instance = new IVRSatisfaction();
        private const string P_IVRSATISFACTION_INSERT = "p_IVRSatisfaction_Insert";
        ///  增加一条满意度数据
        /// <summary>
        ///  增加一条满意度数据
        /// </summary>
        public int Insert(Entities.IVRSatisfaction model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@CallID", SqlDbType.BigInt,8),
					new SqlParameter("@CallRecordID", SqlDbType.BigInt,8),					
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CallID;
            parameters[2].Value = model.CallRecordID;
            parameters[3].Value = model.Score;
            parameters[4].Value = model.CreateTime;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_IVRSATISFACTION_INSERT, parameters);
            return int.Parse(parameters[0].Value.ToString());
        }
        /// 根据CallID查询满意度分数
        /// <summary>
        /// 根据CallID查询满意度分数
        /// </summary>
        /// <param name="callid"></param>
        /// <returns></returns>
        public int GetIVRScoreBYCallID(long callid, string tableEndName)
        {
            string sql = "SELECT Score FROM dbo.IVRSatisfaction" + tableEndName + " WHERE CallRecordID=" + callid;
            DataSet ds;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);

            int score = -2;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out score))
                { }
            }

            return score;
        }
    }
}
