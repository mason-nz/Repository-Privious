using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto
{
    public class RequestMediaPublicParam
    {
        public int MediaID { get; set; }

        [Necessary(MtName = "BusinessType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int BusinessType { get; set; }

        [Necessary(MtName = "OperateType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int OperateType { get; set; }

        [Necessary(MtName = "帐号")]
        public string Number { get; set; }

        [Necessary(MtName = "名称")]
        public string Name { get; set; }

        [Necessary(MtName = "头像url")]
        public string HeadIconURL { get; set; }

        [Necessary(MtName = "FansCount", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int FansCount { get; set; }

        [Necessary(MtName = "CategoryID", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int CategoryID { get; set; }

        //[Necessary(MtName = "ProvinceID", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int ProvinceID { get; set; }

        //[Necessary(MtName = "CityID", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int CityID { get; set; }

        [Necessary(MtName = "CoverageArea")]
        public string CoverageArea { get; set; }//覆盖区域 格式： 省Id-城市Id,省Id-城市Id

        public int Status { get; set; }
        public int Source { get; set; }//来源 自营 ，自助

        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int LastUpdateUserID { get; set; }
    }
}