using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1
{
    public class RequestWeiXinDto
    {
        public RequestWeiXinDto()
        {
            this.CreateTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
            this.CityID = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.ProvinceID = Entities.Constants.Constant.INT_INVALID_VALUE;
        }

        public int Version { get; set; }
        public int MediaID { get; set; }

        [Necessary(MtName = "AuthType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int AuthType { get; set; }//授权方式

        //[Necessary(MtName = "OperateType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        //public int OperateType { get; set; }

        [Necessary(MtName = "帐号")]
        public string Number { get; set; }

        [Necessary(MtName = "名称")]
        public string Name { get; set; }

        [Necessary(MtName = "头像url")]
        public string HeadIconURL { get; set; }

        [Necessary(MtName = "FansCount", IsValidateThanAt = true, ThanMaxValue = 500, Message = "{0}必须大于{1}")]
        public int FansCount { get; set; }

        //[Necessary(MtName = "粉丝数截图")]
        public string FansCountUrl { get; set; }

        //[Necessary(MtName = "男粉丝数比例", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public decimal FansMalePer { get; set; }

        //[Necessary(MtName = "女粉丝数比例", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public decimal FansFemalePer { get; set; }

        [Necessary(MtName = "常见分类")]
        public string CommonlyClass { get; set; }

        public string FansArea { get; set; }
        public string FansAreaShotUrl { get; set; }//区域截图

        public int PublishStatus { get; set; }//发布状态

        public int AuditStatus { get; set; }//审核状态

        public string FansSexScaleUrl { get; set; }//男女粉丝比例截图

        public int LevelType { get; set; }//媒体级别（意见领袖或普通）
        public bool IsAuth { get; set; }//微信认证：
        public string Sign { get; set; }//描述／签名
        public string TwoCodeURL { get; set; }

        public int ProvinceID { get; set; }
        public int Source { get; set; }
        public int CityID { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int LastUpdateUserID { get; set; }

        public bool IsAreaMedia { get; set; }//是否是区域媒体

        //public string OrderRemark { get; set; }
        public List<OrderRemarkDto> OrderRemark { get; set; }//下单备注

        public List<CoverageAreaDto> AreaMedia { get; set; }//区域媒体

        /* 资质 */

        //企业名称
        public string EnterpriseName { get; set; }

        //资质1
        public string QualificationOne { get; set; }

        //资质2
        public string QualificationTwo { get; set; }

        //营业执照
        public string BusinessLicense { get; set; }
    }
}