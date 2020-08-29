using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Query;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1
{
    public class RespGetWeiXinBaseDto
    {
        public int MediaID { get; set; }
        public string Number { get; set; }

        public string Name { get; set; }
        public string HeadIconURL { get; set; }
    }

    public class FansSexProportionDto
    {
        public decimal FansMalePer { get; set; }

        public decimal FansFemalePer { get; set; }
    }

    public class RespGetWeiXinDto : RespGetWeiXinBaseDto
    {
        public int AuthType { get; set; }//授权方式
        public string FansCountURL { get; set; }

        public string TwoCodeURL { get; set; }

        public int FansCount { get; set; }

        public decimal FansMalePer { get; set; }

        public decimal FansFemalePer { get; set; }

        public List<int> CommonlyClass { get; set; }

        public List<FansAreaDto> FansArea { get; set; }

        public string FansAreaShotUrl { get; set; }//粉丝区域截图

        public int PublishStatus { get; set; }//发布状态

        public int AuditStatus { get; set; }//审核状态

        public string FansSexScaleUrl { get; set; }//男女粉丝比例截图

        public int IsExist { get; set; }//是否已经存在
        public int LevelType { get; set; }//媒体级别（意见领袖或普通）
        public bool IsAuth { get; set; }//微信认证：
        public string Sign { get; set; }//描述／签名
        public int ProvinceID { get; set; }
        public int Source { get; set; }
        public int CityID { get; set; }
        public List<OrderRemarkDto> OrderRemark { get; set; }//下单备注
        public List<CoverageAreaDto> AreaMedia { get; set; }//区域媒体
        public bool IsAreaMedia { get; set; }
        public int WxId { get; set; }

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

    public class RespWeiXinItemDto : RespGetWeiXinBaseDto
    {
        public int WxId { get; set; }
        public string ADName { get; set; }
        public bool IsAddBackList { get; set; }
        public bool IsCollectionList { get; set; }
        public string CommonlyClass { get; set; }
        public decimal FansMalePer { get; set; }
        public decimal FansFemalePer { get; set; }
        public int FansCount { get; set; }
        public string Summary { get; set; }//简介
        public string FullName { get; set; }//全称\主体
        public string EnterpriseType { get; set; }//企业类型
        public string Location { get; set; }//Location
        public DateTime RegTime { get; set; }//注册时间
        public DateTime CreateTime { get; set; }//创建时间
        public List<FansAreaDto> FansArea { get; set; }
        public List<CoverageAreaDto> AreaMedia { get; set; }//区域媒体
        public bool IsAreaMedia { get; set; }
        public string AreaMapping { get; set; }
        public string ServiceTypeName { get; set; }//公众号类型
        public decimal MaLiIndex { get; set; }//玛丽值
    }

    public class RespMediaForMediaRoleDto : RespGetWeiXinBaseDto
    {
        public int CreateUserId { get; set; }
        public string UserName { get; set; }
        public string TrueName { get; set; }
        public string Mobile { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string SourceName { get; set; }
    }

    public class RespSearchMediaDto : SearchTitleResponse
    {
        public int BaseMediaId { get; set; }
    }
}