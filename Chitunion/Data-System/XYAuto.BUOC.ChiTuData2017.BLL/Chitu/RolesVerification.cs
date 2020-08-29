/********************************************************
*创建人：hant
*创建时间：2017/12/25 10:02:36 
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
    public class RolesVerification
    {
        public static readonly RolesVerification Instance = new RolesVerification();
        public  bool IsAllData()
        {
            bool result = false;
            string roles = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().Roles.ToUpper();
            if (roles.Contains("SYS004RL00018") || roles.Contains("SYS004RL00027"))
            {
                result = true;
            }
            return result;
        }


        public  bool IsViewData()
        {
            bool result = false;
            string roles = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().Roles.ToUpper();
            if (roles.Contains("SYS004RL00029"))
            {
                result = true;
            }
            return result;
        }
    }
}
