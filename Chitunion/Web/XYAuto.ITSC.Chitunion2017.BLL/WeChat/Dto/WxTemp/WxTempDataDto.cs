using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat.Dto.WxTemp
{
    /// <summary>
    /// 注释：WxTempDataDto
    /// 作者：masj
    /// 日期：2018/5/19 14:47:02
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class WxTempDataDto
    {
        /// <summary>
        /// 微信号名称
        /// </summary>
        public string WxNum { get; set; }

        /// <summary>
        /// 微信开发者AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 微信开发者AppSecret
        /// </summary>
        //[Newtonsoft.Json.JsonIgnore()]
        public string AppSecret { get; set; }

        /// <summary>
        /// 微信模板数据集合
        /// </summary>
        public List<WxTempDataTemp> TempList { get; set; }
    }

    /// <summary>
    /// 微信模板数据
    /// </summary>
    public class WxTempDataTemp
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 模板标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 模板内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 模板示例描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 模板参数（多个用逗号分隔）
        /// </summary>
        public string Paras { get; set; }
    }
}
