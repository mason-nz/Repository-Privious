using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.SurveyManage
{
    /// <summary>
    /// 保存试卷时临时用的对照
    /// </summary>
    public class OptionSkip
    {
        /// <summary>
        /// 选项ID
        /// </summary>
        public int Soid { get; set; }

        /// <summary>
        /// 问题ID
        /// </summary>
        public int Sqid { get; set; }

        /// <summary>
        /// 跳题要跳到的问题ID
        /// </summary>
        public int LinkId { get; set; }
    }
}