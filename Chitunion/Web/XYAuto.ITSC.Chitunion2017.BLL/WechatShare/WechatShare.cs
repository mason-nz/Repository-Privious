using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.WechatShare;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.ITSC.Chitunion2017.BLL.WechatShare
{
    public class WechatShare
    {
        public static readonly WechatShare Instance = new WechatShare();

        public void AddWechatShare(ReqsShare Dto)
        {
            if (Dto == null || string.IsNullOrWhiteSpace(Dto.ShareUrl))
            {
                Loger.Log4Net.Error("AddWechatShare:无效参数");
                return;
            }
            int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            if (userId <= 0)
            {
                Loger.Log4Net.Error("AddWechatShare:未登陆的分享");
                return;
            }
            Entities.LETask.LeShareDetail entity = new Entities.LETask.LeShareDetail();
            if (Dto.ShareUrl.Contains("/inviteManager/"))
            {
                entity.Type = (int)LeShareDetailTypeEnum.邀请;
            }
            else
            {
                entity.Type = (int)LeShareDetailTypeEnum.其他;
            }
            entity.ShareURL = Dto.ShareUrl;
            entity.OrderCoding = "";
            entity.ShareResult = 1;
            entity.IP = Util.GetIP($"用户{userId}分享功能");
            entity.Status = 0;
            entity.CreateUserId = userId;
            entity.CreateTime = DateTime.Now;
            var excuteId = Dal.LETask.LeShareDetail.Instance.Insert(entity);
            if (excuteId <= 0)
            {
                Loger.Log4Net.Error($" AddWechatShare 添加失败：{JsonConvert.SerializeObject(entity)}");
            }
        }

    }
}
