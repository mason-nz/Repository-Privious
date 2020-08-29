using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{
    /// <summary>
    /// 注释：内部员工信息
    /// 作者：masj
    /// 日期：2018/5/28 15:16:43
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class EmployeeInfo : DataBase
    {
        public static readonly EmployeeInfo Instance = new EmployeeInfo();

        /// <summary>
        /// 根据UserID，查询是否为内部员工
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public bool IsExistByUserID(int userid)
        {
            string sql = $@"SELECT COUNT(*) FROM dbo.EmployeeInfo AS ei
                        JOIN dbo.UserDetailInfo AS udi ON udi.IdentityNo=ei.IDCard
                        WHERE udi.Status=2 AND udi.UserID={userid}";

            DataSet obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (obj != null && obj.Tables[0].Rows.Count > 0)
            {
                return int.Parse(obj.Tables[0].Rows[0][0].ToString()) > 0 ? true : false;
            }
            return false;
        }
    }
}
;