using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.WeChat
{
    /// <summary>
    /// 注释：WeixinInfo
    /// 作者：masj
    /// 日期：2018/6/13 17:01:53
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class WeixinInfo : DataBase
    {
        public static readonly WeixinInfo Instance = new WeixinInfo();

        public DataTable GetList()
        {
            string sql = @"SELECT RecID,Name,AppID,AppSecret FROM dbo.LE_WeixinInfo WHERE Status=0";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        /// <summary>
        /// 根据AppId，返回主键ID
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public int GetRecIDByAppId(string appId)
        {
            var sql = $@"SELECT RecID FROM dbo.LE_WeixinInfo
WHERE Status=0 AND AppID='{StringHelper.SqlFilter(appId)}'";

            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return int.Parse(ds.Tables[0].Rows[0][0].ToString());
                }
            }
            return -1;
        }

        /// <summary>
        /// 在表LE_WeiXinUser中，维护指定openIds的userType内容
        /// </summary>
        /// <param name="openIds"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public bool UpdateUserType(List<string> openIds, int userType)
        {
            string sql = $@"UPDATE dbo.LE_WeiXinUser 
                            SET UserType={userType}
                            WHERE openid IN ('{string.Join("','", openIds)}')";
            int flag = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
            return flag > 0 ? true : false;
        }
    }
}
