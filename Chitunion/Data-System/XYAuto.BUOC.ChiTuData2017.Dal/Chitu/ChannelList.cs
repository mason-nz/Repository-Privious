/********************************************************
*创建人：hant
*创建时间：2017/12/26 11:07:32 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.Chitu
{
    public class ChannelList :DataBase
    {
        public static readonly ChannelList Instance = new ChannelList();

        public List<Entities.Chitu.ChannelList> GetList(string where)
        {
            var sql = @"
                        SELECT [AppName] AS ChannelName,[ChannelID]
  FROM [dbo].[AppInfo]
  WHERE [Status] = 0  " + where;
           
            var data = SqlHelper.ExecuteDataset(Chitunion_DataSystem2017 , CommandType.Text, sql);
            return DataTableToList<Entities.Chitu.ChannelList>(data.Tables[0]);
        }
    }
}
