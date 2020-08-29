using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.ShoppingCart.Dto
{
    public class RequestAPPAddShoppingDto
    {
        public int MediaType { get; set; } = 14002;
        public List<RequestAPPAddShppingIDDto> IDs { get; set; }
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (!Enum.IsDefined(typeof(Entities.EnumMediaType), MediaType))
            {
                sb.Append("媒体类型不存在!");
            }

            if (IDs == null || IDs.Count == 0)
            {
                sb.Append("媒体ID、广告位ID对象数组不能为空!");
            }
            else
            {
                if (IDs.Count > 50)
                {
                    sb.Append("媒体ID广告位ID对象数量不能超过50!");
                }
                else
                {
                    foreach (RequestAPPAddShppingIDDto item in IDs)
                    {
                        if (item.MediaID == 0 || item.MediaID == -2)
                        {
                            sb.Append("媒体ID不存在!");
                            break;
                        }

                        if (item.PublishDetailID == 0 || item.PublishDetailID == -2)
                        {
                            sb.Append("广告位ID不存在!");
                            break;
                        }                        
                    }
                    #region APP广告位验证
                    /**
                     * 广告位定义：广告位ID+区域ID
                     * 1广告位自己的排期不可以有交集
                     * 2不同广告位所属排期不可以有交集                   
                    **/
                    var itemGroups = IDs.GroupBy(l => l.PublishDetailID);
                    foreach (var itemGroup in itemGroups)
                    {
                        var queryGroup2 = itemGroup.GroupBy(x => x.SaleAreaID);
                        foreach (var itemGroup2 in queryGroup2)
                        {
                            foreach (var item in itemGroup2)
                            {
                                if (item.ADSchedule.Count > 0)
                                {
                                    foreach (var itemADS in item.ADSchedule)
                                    {
                                        if (itemADS.BeginData.Date > itemADS.EndData.Date)
                                        {
                                            sb.Append("广告位排期结束时间要大于等于开始时间!\n");
                                            msg = sb.ToString();
                                            return false;
                                        }
                                        if (itemADS.BeginData.Date <= DateTime.Now.Date)
                                        {
                                            sb.Append("排期开始日期必须大于当天!\n");
                                            msg = sb.ToString();
                                            return false;
                                        }
                                        if (itemADS.EndData.Date <= DateTime.Now.Date)
                                        {
                                            sb.Append("排期结束日期必须大于当天!\n");
                                            msg = sb.ToString();
                                            return false;
                                        }
                                        if(itemADS.BeginData.Date.AddMonths(6)< itemADS.EndData.Date)
                                        {
                                            sb.Append("排期时间跨度不能超过6个月!\n");
                                            msg = sb.ToString();
                                            return false;
                                        }
                                        var queryADS = from t in item.ADSchedule
                                                       where t.BeginData != itemADS.BeginData && t.EndData != itemADS.EndData
                                                        && ((itemADS.BeginData.Date >= t.BeginData.Date && itemADS.BeginData.Date <= t.EndData.Date)
                                                            ||
                                                            (itemADS.BeginData.Date <= t.BeginData.Date && itemADS.BeginData.Date >= t.EndData.Date)
                                                            ||
                                                            (itemADS.BeginData.Date <= t.BeginData.Date && itemADS.EndData.Date >= t.BeginData.Date)
                                                            ||
                                                            (itemADS.BeginData.Date <= t.EndData.Date && itemADS.EndData.Date >= t.EndData.Date)
                                                           )
                                                       select t;
                                        if (queryADS.Count() > 0)
                                        {
                                            sb.Append("广告位排期不能有交叉!\n");
                                            msg = sb.ToString();
                                            return false;
                                        }

                                        queryADS = from t in item.ADSchedule
                                                   where t.BeginData == itemADS.BeginData && t.EndData == itemADS.EndData                                                    
                                                   select t;
                                        if (queryADS.Count() > 1)
                                        {
                                            sb.Append("广告位排期不能有重复!\n");
                                            msg = sb.ToString();
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            }      
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }

    public class RequestAPPAddShppingIDDto
    {
        public int MediaID { get; set; } = -2;
        public int PublishDetailID { get; set; } = -2;
        public int SaleAreaID { get; set; } = -2;
        public int ADLaunchDays { get; set; } = 0;
        public List<RequestAPPAddShppingADScheduleDto> ADSchedule { get; set; }
    }
    public class RequestAPPAddShppingADScheduleDto
    {
        public DateTime BeginData { get; set; }
        public DateTime EndData { get; set; }
    }
}
