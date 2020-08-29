using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.Provider.Dto.Request
{
    public class ReqCreateShareDto
    {

        public int ShareType { get; set; }
        public ShareContentDto ShareContent { get; set; }
    }

    public class ShareContentDto
    {
        public int TaskId { get; set; }
        /// <summary>
        /// 分享内容的类别 1：任务    2:海报
        /// </summary>
        public int ShareContentType { get; set; }
        public string Ip { get; set; }
        public string ShareUrl { get; set; }
    }

    public enum ShareContentTypeEnum
    {
        任务物料 = 208001,
        海报
    }
}
