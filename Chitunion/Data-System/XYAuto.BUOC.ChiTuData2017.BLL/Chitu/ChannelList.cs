/********************************************************
*创建人：hant
*创建时间：2017/12/26 11:14:50 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Chitu
{
    public class ChannelList
    {
        public static readonly ChannelList Instance = new ChannelList();

        public List<Entities.Chitu.ChannelList> GetList()
        {
            List<Entities.Chitu.ChannelList> list = new List<Entities.Chitu.ChannelList>();
            StringBuilder where = new StringBuilder();
            if (RolesVerification.Instance.IsAllData())
            {
                where.Append(" AND ChannelID >0");
            }
            else
            {
                where.AppendFormat(" AND ChannelID ='{0}' ",BLL.TaskInfo.AppInfo.Instance.GeChannelIDByUserId(XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID));
            }
            list = Dal.Chitu.ChannelList.Instance.GetList(where.ToString());
            if (RolesVerification.Instance.IsAllData())
            {
                var item = new Entities.Chitu.ChannelList();

                item.ChannelName = "全部";
                item.ChannelID = -2;
                list.Add(item);
            }
            return list.OrderBy(o=>o.ChannelID).ToList();
        }
    }
}
