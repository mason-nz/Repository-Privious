/********************************************************
*创建人：hant
*创建时间：2017/12/21 15:37:58 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Chitu.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.BLL.Chitu.Dto.Response;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Chitu
{
    public class DataStatisticsByDate
    {
        public static readonly DataStatisticsByDate Instance = new DataStatisticsByDate();

        /// <summary>
        /// 渠道每日汇总数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResponseDataStatisticsByDate GetData(RequestDataStatisticsByDate req)
        {
            int totalcount = 0;
            ResponseDataStatisticsByDate StatisticsByDate = new ResponseDataStatisticsByDate();
            //if (RolesVerification.Instance.IsAllData() && !(RolesVerification.Instance.IsViewData()))
            //{
            //    req.ChannelID = BLL.TaskInfo.AppInfo.Instance.GeChannelIDByUserId(XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID);
            //}
            List<Entities.Chitu.DataStatisticsByDate> list = GetDataByChannelAndDate(req, out totalcount);
            StatisticsByDate.TotalCount = totalcount;
            List<StatisticsByData> listmodel = new List<StatisticsByData>();
            if (totalcount > 0)
            {
                
                foreach (var item in list)
                {
                    Dto.Response.StatisticsByData data = new StatisticsByData();
                    data.ChannelID = item.ChannelID;
                    data.ChannelName = item.ChannelName;
                    data.Date = item.Date;
                    data.OrderNumber = item.OrderNumber;
                    data.Profit = item.Profit;
                    data.StateOfSettlement = item.StateOfSettlement;
                    data.TimeOfSettlement = item.TimeOfSettlement;
                    listmodel.Add(data);
                }
                StatisticsByDate.List = listmodel;
            }
            else
            {
                StatisticsByDate.List = listmodel;
            }

            return StatisticsByDate;
        }

        private List<Entities.Chitu.DataStatisticsByDate> GetDataByChannelAndDate(RequestDataStatisticsByDate req,out int totalCount)
        {            
            StringBuilder swhere = new StringBuilder();

            if (req.BeginDate != null &&  req.BeginDate.Length > 0)
            {
                swhere.AppendFormat(" AND Date >= '{0}'", req.BeginDate);
            }
            if (req.EndDate != null && req.EndDate.Length > 0)
            {
                swhere.AppendFormat(" AND Date <= '{0}'", req.EndDate);
            }
            if (req.ChannelID > 0)
            {
                swhere.AppendFormat(" AND ChannelID = {0}", req.ChannelID);
            }
            if (req.StateOfSettlement > 0)
            {
                swhere.AppendFormat(" AND StateOfSettlement= ", req.StateOfSettlement);
            }
            return Dal.Chitu.DataStatisticsByDate.Instance.GetDataByWhere(swhere.ToString(),req.PageIndex,req.PageSize,out totalCount);
        }
    }
}
