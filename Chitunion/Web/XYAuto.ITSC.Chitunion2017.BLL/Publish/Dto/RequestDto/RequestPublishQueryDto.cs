using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto
{
    /// <summary>
    /// 后台页面的查询条件
    /// </summary>
    public class RequestPublishQueryDto : CreatePublishQueryBase
    {
        public RequestPublishQueryDto()
        {
            this.Platform = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.CategoryId = Entities.Constants.Constant.INT_INVALID_VALUE;
            //this.Wx_Status = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.SearchId = Entities.Constants.Constant.INT_INVALID_VALUE;
            //this.IsPassed = Entities.Constants.Constant.INT_INVALID_VALUE;
        }

        public int CreateUserId { get; set; }//是为了判断角色，如果是AE,媒体主 只能看到自己添加的刊例

        public int Platform { get; set; }//平台
        public int CategoryId { get; set; }//分类
        public string Wx_Status { get; set; }//审核状态
        public int SearchId { get; set; }
        public string PubName { get; set; }
        public string Keyword { get; set; }
        public bool IsPassed { get; set; }//是否通过
        public string SubmitUser { get; set; }//提交人
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }

        //public string StrStatus { get; set; }//多选
    }

    /// <summary>
    /// 前台页面的查询条件
    /// </summary>
    public class RequestFrontPublishQueryDto : CreatePublishQueryBase
    {
        public RequestFrontPublishQueryDto()
        {
            Sex = Entities.Constants.Constant.INT_INVALID_VALUE;
            IsAuth = Entities.Constants.Constant.INT_INVALID_VALUE;
            FansSex = Entities.Constants.Constant.INT_INVALID_VALUE;
            AuthType = Entities.Constants.Constant.INT_INVALID_VALUE;
            SaleMode = Entities.Constants.Constant.INT_INVALID_VALUE;
            LevelType = Entities.Constants.Constant.INT_INVALID_VALUE;
            Platform = Entities.Constants.Constant.INT_INVALID_VALUE;
            Profession = Entities.Constants.Constant.INT_INVALID_VALUE;
            CategoryID = Entities.Constants.Constant.INT_INVALID_VALUE;
            OrderRemark = Entities.Constants.Constant.INT_INVALID_VALUE;
            FansCountUnit = 10000;
            DailyExposureCountUnit = 10000;
            PriceUnit = 1;
            OrderByReference = Entities.Constants.Constant.INT_INVALID_VALUE;
            SearchMediaId = Entities.Constants.Constant.INT_INVALID_VALUE;
            Price = string.Empty;
            FansCount = string.Empty;
            CoverageArea = string.Empty;
        }

        public string Keyword { get; set; }//关键字查询

        public int SearchMediaId { get; set; }

        public int CategoryID { get; set; }

        public int FansCountUnit { get; set; }//粉丝数的单位：万

        private int _priceUnit;

        public int PriceUnit
        {
            get
            {
                if (_priceUnit < 1)
                {
                    return 1;
                }
                else if (_priceUnit > 10000)
                {
                    return 10000;
                }
                else
                {
                    return _priceUnit;
                }
            }
            set { _priceUnit = value; }
        }//价格的单位：万

        public int DailyExposureCountUnit { get; set; }//日均曝光的单位：万

        public string Price { get; set; }
        public string FansCount { get; set; }
        public string CoverageArea { get; set; }//覆盖区域

        public int IsAuth { get; set; }//是否为微信认证

        public int OrderRemark { get; set; }
        public int LevelType { get; set; }//媒体级别（意见领袖：4001 普通：4002）
        public int FansSex { get; set; }//受众性别比例（不限：-1，男>50%：0，女>50%：1）
        public int Sex { get; set; }//媒体主性别

        /* 微博 */
        public int AuthType { get; set; }//微博认证枚举（默认：-1）
        public int Profession { get; set; }//职业

        public string DailyExposureCount { get; set; }//日均曝光

        public int Platform { get; set; }//平台
        public int OrderByReference { get; set; }//参考报价排序

        public int SaleMode { get; set; }//售卖方式
        public string MediaName { get; set; }//媒体名称
        public string AdForm { get; set; }//广告形式
    }
}