/********************************************************
*创建人：hant
*创建时间：2018/1/25 16:11:05 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V2_3;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat
{
    public class Order
    {
        public static readonly Order Instance = new Order();
        public Entities.DTO.V2_3.OrderResDTO GetOrderByStatus(int userid, int status, int PageIndex, int PageSize, out int totalCount)
        {
            return Dal.WeChat.Order.Instance.GetOrderByStatus(userid, status, PageIndex, PageSize, out totalCount);
        }

        public string GetMaterialUrlByTaskId(int taskId)
        {
            Entities.LETask.LeTaskInfo info = XYAuto.ITSC.Chitunion2017.Dal.LETask.LeTaskInfo.Instance.GetInfo(taskId);
            return info.MaterialUrl;
        }

        public Entities.LETask.LeTaskInfo GetTaskInfo(int taskid)
        {
            Entities.LETask.LeTaskInfo info = XYAuto.ITSC.Chitunion2017.Dal.LETask.LeTaskInfo.Instance.GetInfo(taskid);
            return info;
        }


        public Entities.DTO.V2_3.OrderInfoRspDTO GetOrder(int orderid, int userid)
        {
            Entities.DTO.V2_3.OrderInfoRspDTO entity = new Entities.DTO.V2_3.OrderInfoRspDTO();
            var orderInfo = Dal.LETask.LeAdOrderInfo.Instance.GetAdOrderInfo(orderid, userid);

            if (orderInfo != null && orderInfo.TaskID > 0)
            {
                var task = XYAuto.ITSC.Chitunion2017.BLL.WeChat.Order.Instance.GetTaskInfo(orderInfo.TaskID);
                entity.OrderId = orderInfo.RecID;
                entity.OrderName = orderInfo.OrderName;
                entity.OrderUrl = orderInfo.OrderUrl;
                if (entity.OrderUrl.Contains("ct_m"))
                {
                    int index = entity.OrderUrl.IndexOf("ct_m");
                    entity.OrderUrl = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("DominArticle") + entity.OrderUrl.Substring(index - 1);
                }
                entity.ReceiveTime = orderInfo.CreateTime;
                entity.BillingRuleName = orderInfo.BillingRuleName;
                entity.ImgUrl = task.ImgUrl;
                entity.Synopsis = task.Synopsis;
                entity.TaskName = task.TaskName;
                entity.CPCUnitPrice = orderInfo.CPCUnitPrice;
                var respDto = new RespOrderIncomeDto();
                var tupList = Dal.LETask.LeAccountBalance.Instance.GetAccountBalances(orderid, userid);
                if (tupList != null && tupList.Item1.Count > 0)
                {
                    entity.List = new List<List>();
                    foreach (var item in tupList.Item1)
                    {
                        entity.List.Add(new XYAuto.ITSC.Chitunion2017.Entities.DTO.V2_3.List { CPCCount = item.CPCShowCount, CPCTotalPrice = item.CPCTotalPrice, Date = item.StatisticsTime });
                    }
                    OrderIncomeTotalItem total = new OrderIncomeTotalItem();
                    entity.Extend = new Extend();
                    entity.List.OrderByDescending(a => a.Date).ToList();
                    entity.Extend.TotalCPCCount = tupList.Item2.TotalCPCCount;
                    entity.Extend.TotalCPCTotalPrice = tupList.Item2.TotalCPCTotalPrice;
                }
                else
                {
                    entity.List = new List<List>();
                    entity.Extend = new Extend();
                }
            }
            else
            {
                entity.List = new List<List>();
                entity.Extend = new Extend();
            }
            return entity;
        }
        /// <summary>
        /// 获取用户前一天订单数量
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetUserDayOrderCount()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            dic.Add("OrderCount", Dal.WeChat.Order.Instance.GetUserDayOrderCount(userId, 1));
            dic.Add("IsNewUser", Dal.UserManage.UserInfo.Instance.IsNewUser(userId, 24));
            return dic;
        }
        /// <summary>
        /// 获取用户订单总数量
        /// </summary>
        /// <returns></returns>
        public int GetUserTotalOrderCount()
        {
            int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            return Dal.WeChat.Order.Instance.GetUserDayOrderCount(userId, 0);
        }

    }
}
