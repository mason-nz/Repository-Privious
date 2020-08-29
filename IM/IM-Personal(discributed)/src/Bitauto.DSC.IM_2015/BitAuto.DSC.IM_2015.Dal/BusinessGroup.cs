using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BitAuto.Utils.Data;

namespace BitAuto.DSC.IM_2015.Dal
{
    public class BusinessGroup : DataBase
    {
        public static readonly BusinessGroup Instance = new BusinessGroup();
        private BusinessGroup() { }

        /// <summary>
        /// 根据userid获取业务组标签(加数据权限)
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="OnlyByGroup">是否只是按所在分组查询数据</param>
        /// <param name="OnlyByGroup">是否显示停用的标签</param>
        /// <returns></returns>
        public DataTable GetBusinessGroupTagsByUserID(int UserID)
        {
            SqlParameter parameter = new SqlParameter("@userId", UserID);

            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.StoredProcedure, "GetBusinessGroupTagsByUserID", parameter);

            return ds.Tables[0];
        }

    }
}
