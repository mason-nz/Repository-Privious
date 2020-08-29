using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_11;

namespace XYAuto.ITSC.Chitunion2017.BLL.Materiel
{
    public class MaterielChannel
    {
        public static readonly MaterielChannel Instance = new MaterielChannel();

        public bool SaveChannel(SaveChannelReqDTO req, ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            var extend = Dal.Materiel.MaterielExtend.Instance.GetEntity(req.MaterielID);
            if (extend == null)
            {
                msg = "物料不存在!";
                return false;
            }
            if (req.ChannelList != null && req.ChannelList.Count > 0)
            {
                foreach (var item in req.ChannelList)
                {
                    if (item.ChannelID.Equals(0) && !string.IsNullOrWhiteSpace(extend.FootContentUrl))
                    {
                        string key = Common.Util.GenerateRandomCode(10);
                        item.PromotionUrlCode = key;
                        if (extend.FootContentUrl.ToLower().StartsWith("http://h5."))
                        {
                            item.PromotionUrl = string.Format("{0}{1}channel=qingniao&sub={2}", extend.FootContentUrl, extend.FootContentUrl.Contains("?") ? "&" : "?", key);
                        } else
                        {
                            item.PromotionUrl = string.Format("{0}{1}utm_source=qingniao&utm_term={2}", extend.FootContentUrl, extend.FootContentUrl.Contains("?") ? "&" : "?", key);
                        }
                    }
                    item.MaterielID = req.MaterielID;
                    item.CreateUserID = ur.UserID;
                    item.CreateTime = DateTime.Now;
                    item.LastUpdateTime = DateTime.Now;
                }
            }
            bool res = Dal.Materiel.MaterielChannel.Instance.Save(req.MaterielID, req.ChannelList);
            return res;
        }
    }
}
