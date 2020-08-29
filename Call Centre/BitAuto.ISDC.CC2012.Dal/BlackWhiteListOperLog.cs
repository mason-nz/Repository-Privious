using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Data.SqlClient;

namespace BitAuto.ISDC.CC2012.Dal
{
    /// 黑白名单日志类
    /// <summary>
    /// 黑白名单日志类
    /// </summary>
    public class BlackWhiteListOperLog : DataBase
    {
        public static BlackWhiteListOperLog Instance = new BlackWhiteListOperLog();

        /// <summary>
        /// 根据条件取黑白名单操作日志
        /// </summary>
        public DataTable GetBlackWhiteListOperLog(QueryBlackWhiteListOperLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string whereStr = GetWhere(query);
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};
            parameters[0].Value = whereStr;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_BlackWhiteListOperLog_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 提取公共条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetWhere(QueryBlackWhiteListOperLog query)
        {
            StringBuilder whereStr = new StringBuilder();
            if (!string.IsNullOrEmpty(query.PhoneNum))
            {
                if (query.PhoneNum.IndexOf(',') > 0)
                {
                    whereStr.Append(" and PhoneNum in (");
                    string[] phoneS = query.PhoneNum.Split(',');
                    for (int i = 0; i < phoneS.Length; i++)
                    {
                        whereStr.Append("'" + SqlFilter(phoneS[i]) + "'");
                        if (i != phoneS.Length - 1)
                        {
                            whereStr.Append(",");
                        }
                    }
                    whereStr.Append(")");
                }
                else
                {
                    whereStr.Append(" and PhoneNum='" + SqlFilter(query.PhoneNum) + "'");
                }
            }
            if (query.BWType != Constant.INT_INVALID_VALUE)
            {
                whereStr.Append(" and BWType=" + query.BWType);
            }
            return whereStr.ToString();
        }

        /// <summary>
        /// 增加一条日志记录
        /// </summary>
        public int Add(BlackWhiteListOperLogInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BlackWhiteListOperLog(");
            strSql.Append("BWRecID,BWType,PhoneNum,CallID,OperType,OperUserID,OperTime)");
            strSql.Append(" values (");
            strSql.Append("@BWRecID,@BWType,@PhoneNum,@CallID,@OperType,@OperUserID,@OperTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@BWRecID", SqlDbType.Int,4),
					new SqlParameter("@BWType", SqlDbType.Int,4),
					new SqlParameter("@PhoneNum", SqlDbType.VarChar,20),
					new SqlParameter("@CallID", SqlDbType.BigInt,8),
					new SqlParameter("@OperType", SqlDbType.Int,4),
					new SqlParameter("@OperUserID", SqlDbType.Int,4),
					new SqlParameter("@OperTime", SqlDbType.DateTime)};
            parameters[0].Value = model.BWRecID;
            parameters[1].Value = model.BWType;
            parameters[2].Value = model.PhoneNum;
            parameters[3].Value = model.CallID;
            parameters[4].Value = model.OperType;
            parameters[5].Value = model.OperUserID;
            parameters[6].Value = model.OperTime;

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
    }
}
