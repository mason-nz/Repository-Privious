/********************************************************
*创建人：lixiong
*创建时间：2017/8/31 10:41:03
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Materiel.Dto
{
    public class ReqBaseGroup
    {
        [Necessary(MtName = "GroupId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int GroupId { get; set; }
    }

    public class RequestSubmitCleanDto : ReqBaseGroup
    {
        public CleanItem Head { get; set; }
        public List<CleanItem> Body { get; set; }
        public int[] DeleteList { get; set; }
    }

    public class CleanItem
    {
        [Necessary(MtName = "ArticleId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int ArticleId { get; set; }

        [Necessary(MtName = "Title")]
        public string Title { get; set; }

        [Necessary(MtName = "Content")]
        public string Content { get; set; }

        [Necessary(MtName = "Abstract")]
        public string Abstract { get; set; }
    }
}