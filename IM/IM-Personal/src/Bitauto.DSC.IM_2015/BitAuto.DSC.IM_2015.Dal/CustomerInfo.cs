using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类CustomerInfo。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:01 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CustomerInfo : DataBase
    {
        public static readonly CustomerInfo Instance = new CustomerInfo();

        protected CustomerInfo()
        { }

        /// 通过会员号获取客户信息
        /// <summary>
        /// 通过会员号获取客户信息
        /// </summary>
        /// <param name="membercode"></param>
        /// <returns></returns>
        public Entities.CustomerInfo GetCustomerInfoByMemberCode(string membercode)
        {
            string sql = "select * from CustomerInfo where membercode='" + membercode + "'";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt != null && dt.Rows.Count != 0)
            {
                return new Entities.CustomerInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }


        public int CallBackUpdate(Entities.CustomerInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@MemberCode", model.MemberCode),
					new SqlParameter("@LastUserID", model.LastUserID),
					new SqlParameter("@LastMessageTime", model.LastMessageTime),
					new SqlParameter("@LastBeginTime", model.LastBeginTime),
					new SqlParameter("@Distribution", model.Distribution+1)
					//new SqlParameter("@Status", SqlDbType.DateTime),
					//new SqlParameter("@LastUpdateTime", DateTime.Now),
					//new SqlParameter("@LastUpdateUserID", model.LastUserID)
                                        };


            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CallBackUpdate", parameters);
        }
    }
}


