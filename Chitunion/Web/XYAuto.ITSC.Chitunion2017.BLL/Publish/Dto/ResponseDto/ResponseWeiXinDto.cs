using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto
{
    public class ResponseDto
    {
        public int PubID { get; set; }
        public int MediaID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string HeadIconURL { get; set; }

        public string RejectMsg { get; set; }
        public int Status { get; set; }
        public int PublishStatus { get; set; }
    }

    public class ResponseWeiXinDto : ResponseDto
    {
        public string TwoCodeURL { get; set; }
        public int FansCount { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<AdPositionEntity> FirstName { get; set; }
        public List<AdPositionEntity> SecondName { get; set; }
        public List<AdPositionEntity> ThridName { get; set; }
        public List<AdPositionEntity> FourthName { get; set; }

        public string Source { get; set; }//来源

        [JsonIgnore]
        public decimal SaleDiscount { get; set; }//销售折扣
    }

    public class ResponseWeiBoDto : ResponseDto
    {
        public string Sex { get; set; }
        public int FansCount { get; set; }

        public List<AdPositionEntity> FirstName { get; set; }
        public List<AdPositionEntity> SecondName { get; set; }
        public List<AdPositionEntity> ThridName { get; set; }

        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Source { get; set; }//来源

        [JsonIgnore]
        public decimal SaleDiscount { get; set; }//销售折扣
    }

    public class ResponseVideoDto : ResponseDto
    {
        [JsonIgnore]
        public decimal SaleDiscount { get; set; }//销售折扣

        public int FansCount { get; set; }

        private decimal _firstPrice;

        public decimal FirstPrice
        {
            get
            {
                if (SaleDiscount > 0)
                {
                    return SaleDiscount * _firstPrice;
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
                    return SaleDiscount * _secondPrice;
                }
                return _secondPrice;
            }
            set { _secondPrice = value; }
        }

        private decimal _thirdPrice;

        public decimal ThirdPrice
        {
            get
            {
                if (SaleDiscount > 0)
                {
                    return SaleDiscount * _thirdPrice;
                }
                return _thirdPrice;
            }
            set { _thirdPrice = value; }
        }

        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Source { get; set; }//来源
        public string Platform { get; set; }//平台
    }

    public class ResponseAppDto
    {
        public int PubID { get; set; }
        public int MediaID { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string RejectMsg { get; set; }
        public int AdCount { get; set; }
        public string Source { get; set; }//来源
        public string Category { get; set; }
    }

    public class ResponseAppDtoByYunYing : ResponseAppDto
    {
        public string UserName { get; set; }
        public int PendingAuditAdCount { get; set; }//待审核广告位
        public int AlreadyShelvesAdCount { get; set; }//已上架广告位
        public int AlreadyUnderShelfAdCount { get; set; }//已下架广告位
    }

    public class ResponseBroadcastDto : ResponseDto
    {
        public int FansCount { get; set; }

        private decimal _firstPrice;

        public decimal FirstPrice
        {
            get
            {
                if (SaleDiscount > 0)
                {
                    return SaleDiscount * _firstPrice;
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
                    return SaleDiscount * _secondPrice;
                }
                return _secondPrice;
            }
            set { _secondPrice = value; }
        }

        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public string Source { get; set; }//来源
        public string Platform { get; set; }//平台

        [JsonIgnore]
        public decimal SaleDiscount { get; set; }//销售折扣
    }
}