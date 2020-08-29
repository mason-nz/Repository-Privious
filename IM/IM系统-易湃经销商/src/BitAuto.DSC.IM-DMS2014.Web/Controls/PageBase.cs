using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.Utils.Config;

namespace BitAuto.DSC.IM_DMS2014.Web.Controls
{
    public class PageBase : FootPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
        }

        /// <summary>
        /// 提示“没有权限访问”的页面地址
        /// </summary>
        public static string NotAccessMsgPagePath
        {
            get { return ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath"); }
        }
    }
}