using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Kr;

namespace XYAuto.ITSC.Chitunion2017.BLL.WechatWithdrawals
{
    public class WechatWithdrawals
    {
        public readonly static WechatWithdrawals Instance = new WechatWithdrawals();
        public Dictionary<string, object> GetWithdrawalsInfo(int WithdrawalsId)
        {
            int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            Dictionary<string, object> dic = new Dictionary<string, object>();

            var info = Util.DataTableToEntity<RespWithdrawalsInfoDto>(
                 Dal.WechatWithdrawals.WechatWithdrawals.Instance.GetWithdrawalsInfo(WithdrawalsId, userId));
            dic.Add("WithdrawalsInfo", SetReason(info));
            return dic;
        }

        private RespWithdrawalsInfoDto SetReason(RespWithdrawalsInfoDto info)
        {
            if (info != null)
            {
                info.Reason = new KrErrorMessageProvider().GetKrBaseDto(info.Reason).ErrorMessage;
                return info;
            }
            return info;
        }
    }
}
