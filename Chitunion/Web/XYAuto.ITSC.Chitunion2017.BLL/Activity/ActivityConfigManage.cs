using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.Utils.Config;

namespace XYAuto.ITSC.Chitunion2017.BLL.Activity
{
    /// <summary>
    /// 注释：ActivityConfig
    /// 作者：lix
    /// 日期：2018/6/12 17:47:58
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ActivityConfigManage
    {
        public static OneYuanActivityEntity GetActivityConfig()
        {
            var config = ConfigurationUtil.GetAppSettingValue("OneYuanTiXianActivity", true);
            try
            {
                return JsonConvert.DeserializeObject<OneYuanActivityEntity>(config);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
