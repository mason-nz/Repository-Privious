using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Recommend.Dto.RequestDto
{
    public class AddRecommendDto
    {
        //[Necessary(MtName = "ADDetailID", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入广告位Id")]
        public int ADDetailID { get; set; }

        [Necessary(MtName = "MediaId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入媒体Id")]
        public int MediaId { get; set; }

        [Necessary(MtName = "BusinessType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入合法的业务类型")]
        public int BusinessType { get; set; }

        public int TemplateID { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public int CategoryId { get; set; }
        public int CreateUserId { get; set; }
    }

    public class UpdateRecommendDto
    {
        [Necessary(MtName = "RecId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入Id")]
        public int RecId { get; set; }

        // [Necessary(MtName = "SortNumber", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入排序SortNumber")]
        public int SortNumber { get; set; }

        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
    }

    public class PublishRecommendDto
    {
        [Necessary(MtName = "BusinessType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入合法的业务类型")]
        public int BusinessType { get; set; }

        //[Necessary(MtName = "CategoryId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入分类Id")]
        //public int CategoryId { get; set; }
    }

    public class DeleteRecommendDto
    {
        [Necessary(MtName = "RecId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入Id")]
        public int RecId { get; set; }
    }
}