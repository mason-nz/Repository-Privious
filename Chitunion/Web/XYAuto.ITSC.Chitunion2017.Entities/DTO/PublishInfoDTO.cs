using System.Collections.Generic;
using System.Linq;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using System;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    /// <summary>
    /// ls
    /// </summary>
    public class PublishInfoDTO 
    {
        public Entities.Publish.PublishBasicInfo Publish { get; set; }
        public List<string> Prices { get; set; }

        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            if (Publish == null)
                sb.AppendLine("缺少刊例信息");
            else
            {
                #region 刊例验证
                if (!System.Enum.IsDefined(typeof(MediaTypeEnum), Publish.MediaType))
                    sb.AppendLine("媒体类型错误");
                if (!Publish.MediaType.Equals(MediaTypeEnum.APP) && Publish.MediaID.Equals(0))
                    sb.AppendLine("缺少媒体ID");
                if (Publish.MediaType.Equals(MediaTypeEnum.APP) && Publish.MediaID.Equals(0) && string.IsNullOrWhiteSpace(Publish.MediaName))
                    sb.AppendLine("缺少媒体名称");
                if (Publish.BeginTime.Equals(DateTime.MinValue))
                    sb.AppendLine("执行周期开始时间错误");
                if (Publish.EndTime.Equals(DateTime.MinValue))
                    sb.AppendLine("执行周期结束时间错误");
                if (Publish.BeginTime > Publish.EndTime)
                    sb.AppendLine("执行周期时间错误");
                if (Publish.PurchaseDiscount < 0 || Publish.PurchaseDiscount > 1)
                    sb.AppendLine("采购折扣错误");
                if (Publish.SaleDiscount < 0 || Publish.SaleDiscount > 1)
                    sb.AppendLine("销售折扣错误");
                #endregion
            }

            if (!Publish.MediaType.Equals(MediaTypeEnum.APP))
            {
                if (Prices == null || Prices.Count.Equals(0))
                    sb.AppendLine("缺少价格信息");
                else
                {
                    #region 自媒体价格验证
                    if (Prices.Count > 16)
                    {
                        sb.AppendLine("价格信息过多");
                    }
                    else
                    {
                        int num = 0;
                        switch (Publish.MediaType)
                        {
                            case MediaTypeEnum.微信:
                                if (Prices.Any(p => !p.Split('-').Count().Equals(4) || !(int.TryParse(p.Split('-').Last(), out num) && num > 0)))
                                    sb.AppendLine("价格信息错误");
                                break;
                            case MediaTypeEnum.微博:
                                if (Prices.Any(p => !p.Split('-').Count().Equals(3) || !(int.TryParse(p.Split('-').Last(), out num) && num > 0)))
                                    sb.AppendLine("价格信息错误");
                                break;
                            case MediaTypeEnum.视频:
                                if (Prices.Any(p => !p.Split('-').Count().Equals(2) || !(int.TryParse(p.Split('-').Last(), out num) && num > 0)))
                                    sb.AppendLine("价格信息错误");
                                break;
                            case MediaTypeEnum.直播:
                                if (Prices.Any(p => !p.Split('-').Count().Equals(2) || !(int.TryParse(p.Split('-').Last(), out num) && num > 0)))
                                    sb.AppendLine("价格信息错误");
                                break;
                        }
                        //保证一个维度只能出现一次
                        List<string> posList = new List<string>();
                        foreach (string price in Prices) {
                            string key = price.Remove(price.Length - price.Split('-').Last().Length);
                            if (posList.Contains(key))
                            {
                                sb.AppendLine("价格信息错误");
                                break;
                            }
                            else
                                posList.Add(key);
                        }
                    }
                    #endregion
                }
            }
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}