using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Materiel;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_11
{
    public class SaveChannelReqDTO
    {
        public SaveChannelReqDTO()
        {
            this.ChannelList = new List<MaterielChannel>();
        }

        public int MaterielID { get; set; }
        public List<MaterielChannel> ChannelList { get; set; }

        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();

            if (this.MaterielID.Equals(0))
            {
                sb.AppendLine("缺少物料ID!");
            }
            else if (this.ChannelList != null && this.ChannelList.Count > 0)
            {
                if (this.ChannelList.Count > 50)
                {
                    sb.AppendLine("一次最多添加50个媒体!");
                }
                else if (this.ChannelList.Any(i => string.IsNullOrWhiteSpace(i.MediaTypeName)))
                {
                    sb.AppendLine("媒体类型不能为空!");
                }
                else if (this.ChannelList.Any(i => string.IsNullOrWhiteSpace(i.MediaNumber)))
                {
                    sb.AppendLine("媒体账号不能为空!");
                }
                else if (this.ChannelList.GroupBy(i => new { i.MediaTypeName, i.MediaNumber }).Any(g => g.Count() > 1))
                {
                    this.ChannelList.GroupBy(i => new { i.MediaTypeName, i.MediaNumber })
                        .Where(gb=>gb.Count() >1).ToList()
                        .ForEach(gb => {
                            sb.AppendLine(string.Format("类型:{0} 账号:{1} 重复!", gb.Key.MediaTypeName, gb.Key.MediaNumber));
                        });
                    //sb.AppendLine("同一媒体类型的账号不能重复!");
                }
                else
                {
                    #region 费用类型、付费模式、渠道类型
                    Enum.MaterielChannelPayTypeEnum payType;
                    foreach (var item in this.ChannelList)
                    {

                        if (!System.Enum.IsDefined(typeof(Enum.MaterielChannelPayTypeEnum), item.PayType))
                        {
                            sb.AppendLine("费用类型错误!");
                            break;
                        }
                        else
                        {
                            System.Enum.TryParse(item.PayType.ToString(), out payType);
                            if (payType == Enum.MaterielChannelPayTypeEnum.免费)
                            {
                                item.PayMode = Constants.Constant.INT_INVALID_VALUE;
                                item.UnitCost = 0;
                            }
                            else if (!System.Enum.IsDefined(typeof(Enum.MaterielChannelPayModelEnum), item.PayMode))
                            {
                                sb.AppendLine("付费模式错误!");
                                break;
                            }
                        }

                        if (!System.Enum.IsDefined(typeof(Enum.MaterielChannelTypeEnum), item.ChannelType))
                        {
                            sb.AppendLine("渠道类型错误!");
                            break;
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
