using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto
{
    public class ResponseFrontDto
    {
        public int PubID { get; set; }
        public int MediaID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string HeadIconURL { get; set; }

        public int FansCount { get; set; }

        public string FansCountName
        {
            get
            {
                if (FansCount < 10000)
                    return FansCount.ToString();
                else
                {
                    var count = Convert.ToDecimal(FansCount) / 10000;
                    return string.Format("{0}万", Math.Round(count, 1, MidpointRounding.AwayFromZero));
                }
            }
        }
    }

    public class ResponseFrontWeiXinDto : ResponseFrontDto
    {
        public int UpdateCount { get; set; }//更新次数
        public int ReferReadCount { get; set; }//参考阅读数
        public bool IsReserve { get; set; }//是否预约
        public string OrderRemark { get; set; }
        public bool IsAuth { get; set; }

        [JsonIgnore]
        public decimal SaleDiscount { get; set; }//销售折扣

        public List<AdPositionEntity> FirstName { get; set; }
        public List<AdPositionEntity> SecondName { get; set; }
        public List<AdPositionEntity> ThridName { get; set; }

        public decimal Price { get; set; }

        public bool CanOriginal //是否可原创
        {
            get
            {
                return Price > 0;
            }
        }
    }

    public class ResponseFrontWeiBoDto : ResponseFrontDto
    {
        public bool IsReserve { get; set; }
        public string OrderRemark { get; set; }
        public int CategoryID { get; set; }
        public string Sign { get; set; }
        public string CategoryName { get; set; }
        public int AuthType { get; set; }
        public int AverageForwardCount { get; set; }
        public int AverageCommentCount { get; set; }
        public int AveragePointCount { get; set; }

        [JsonIgnore]
        public decimal SaleDiscount { get; set; }//销售折扣

        public List<AdPositionEntity> FirstName { get; set; }
        public List<AdPositionEntity> SecondName { get; set; }
    }

    public class ResponseFrontApp
    {
        public int AdRecID { get; set; }//广告位Id
        public int PubID { get; set; }
        public int MediaID { get; set; }
        public string Name { get; set; }
        public string AdPosition { get; set; } //广告位置
        public string AdForm { get; set; } //广告形式
        public string AdLegendURL { get; set; } //广告图例
        public string Style { get; set; } //样式
        public int CarouselCount { get; set; } //轮播数
        public bool CanClick { get; set; } //是否可点击
        public string PlayPosition { get; set; } //位置

        private string _sysPlatform;

        public string SysPlatform
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_sysPlatform))
                    return string.Empty;
                var spPlatform = _sysPlatform.Split(',');
                var sbCode = new StringBuilder();
                foreach (var item in spPlatform)
                {
                    if (string.IsNullOrWhiteSpace(item)) continue;
                    if (item.Equals(((int)SysSysPlatform.IOS).ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        sbCode.AppendFormat("{0}/", SysSysPlatform.IOS);
                    }
                    else if (item.Equals(((int)SysSysPlatform.Android).ToString(),
                        StringComparison.OrdinalIgnoreCase))
                    {
                        sbCode.AppendFormat("{0}/", SysSysPlatform.Android);
                    }
                    else
                    {
                        sbCode.AppendFormat("--");
                    }
                }
                return sbCode.ToString().Trim('/');
            }
            set { _sysPlatform = value; }
        } //系统平台(枚举)

        public string SaleMode { get; set; } //售卖方式

        public bool IsCarousel { get; set; } //是否轮播

        public decimal Price
        {
            get;
            set;
        }

        public string CityName { get; set; }
        public string CategoryName { get; set; }

        [JsonIgnore]
        public decimal SaleDiscount { get; set; }//销售折扣

        public string SalePriceName
        {
            get
            {
                if (SalePrice < 10000)
                    return SalePrice.ToString("N");
                else
                {
                    var count = Convert.ToDecimal(SalePrice) / 10000;
                    return string.Format("{0}万", Math.Round(count, 2, MidpointRounding.AwayFromZero).ToString("N"));
                }
            }
        }

        public string PriceName
        {
            get
            {
                if (Price < 10000)
                    return Price.ToString("N");
                else
                {
                    var count = Convert.ToDecimal(Price) / 10000;
                    return string.Format("{0}万", Math.Round(count, 2, MidpointRounding.AwayFromZero).ToString("N"));
                }
            }
        }

        public decimal SalePrice//折后价
        {
            get
            {
                if (SaleDiscount > 0)
                {
                    return Price * SaleDiscount;
                }
                return Price;
            }
            set { }
        }
    }

    public class ResponseFrontVideo : ResponseFrontDto
    {
        public string Sex { get; set; }
        public bool IsReserve { get; set; }

        [JsonIgnore]
        public int AuthType { get; set; }

        public bool IsAuth
        {
            get { return AuthType == 1; }
        }

        public string Platform { get; set; }

        [JsonIgnore]
        public int ProvinceID { get; set; }

        public string ProvinceName { get; set; }

        [JsonIgnore]
        public int CityID { get; set; }

        public string CityName { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }

        private decimal _firstPrice;

        public decimal FirstPrice
        {
            get
            {
                if (SaleDiscount > 0)
                {
                    return Math.Round(_firstPrice * SaleDiscount, 2, MidpointRounding.AwayFromZero);
                }
                return _firstPrice;
            }
            set { _firstPrice = value; }
        }

        private decimal _secondPrice;

        public decimal SecondPrice
        {
            get
            {
                if (SaleDiscount > 0)
                {
                    return Math.Round(_secondPrice * SaleDiscount, 2, MidpointRounding.AwayFromZero);
                }
                return _secondPrice;
            }
            set { _secondPrice = value; }
        }

        //private bool _canOriginal;

        public bool CanOriginal //是否可原创
        {
            get
            {
                return Price > 0;
            }
        }

        [JsonIgnore]
        public decimal SaleDiscount { get; set; }//销售折扣
    }

    public class ResponseFrontBroadcast : ResponseFrontVideo
    {
    }

    public class ResponseFrontPcWap : ResponseFrontApp
    {
    }

    public class AdPositionEntity
    {
        public string AdName { get; set; }
        public decimal Price { get; set; }
    }
}