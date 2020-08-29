using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.Controller
{
    public class Sign
    {
        public static readonly Sign Instance = new Sign();

        public object IsValidActivity(Dto.IsValidActivity.ReqDto req, ref string errorMsg)
        {
            errorMsg = string.Empty;
            if (!req.CheckSelfModel(out errorMsg))
                return null;

            //var inviteDate = Convert.ToDateTime(Util.GetAppSettings("InviteDate"));
            //var signDate = Convert.ToDateTime(Util.GetAppSettings("SignDate"));
            var tmpDate = new DateTime(1900, 1, 1);
            if (req.ActivityType == (int)Dto.IsValidActivity.EnumActivityType.签到有礼)
                tmpDate = Convert.ToDateTime(ConfigurationManager.AppSettings["SignDate"]);//Util.GetAppSettings("SignDate")
            else
                tmpDate = Convert.ToDateTime(ConfigurationManager.AppSettings["InviteDate"]);

            TimeSpan ts = tmpDate.Date.Subtract(DateTime.Now.Date);
            return ts.Days > 0;
        }
    }
}
