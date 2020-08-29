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
    /// 数据访问类STDATA_USER_NOAGG。
    /// </summary>
    /// <author>
    /// lihf
    /// </author>
    /// <history>
    /// 2013-08-13 12:11:00 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class STDATA_USER_NOAGG : DataBase
    {
        #region Instance
        public static readonly STDATA_USER_NOAGG Instance = new STDATA_USER_NOAGG();
        #endregion

        #region const
        private const string P_STDATA_USER_NOAGG_INSERT = "p_STDATA_USER_NOAGG_Insert";
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条坐席登录时长统计数据
        /// </summary>
        public int Insert(Entities.STDATA_USER_NOAGG model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@UserName", SqlDbType.NVarChar,50),
					new SqlParameter("@TTimeAgLoggedOn", SqlDbType.Int,4),					
					new SqlParameter("@BEGINTIME", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.User_Name;
            parameters[2].Value = model.TTimeAgLoggedOn;
            parameters[3].Value = model.BEGIN_TIME;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_STDATA_USER_NOAGG_INSERT, parameters);
            return int.Parse(parameters[0].Value.ToString());
        }
        #endregion
    }
}
