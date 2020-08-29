using System;
using System.Collections.Generic;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class ModifyPublishReqDTO
    {
        public string Pwd { get; set; }
        public ModifyPublish Publish { get; set; }
        public List<ModifyPublishDetail> Details { get; set; }
        public List<ADPrice> PriceList { get; set; }

        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            if (this.Publish == null)
            {
                sb.AppendLine("缺少刊例信息");
            }
            else
            {
                #region 刊例验证
                if (!System.Enum.IsDefined(typeof(Enum.MediaTypeEnum), Publish.MediaType))
                    sb.AppendLine("媒体类型错误");
                //if (Publish.MediaID.Equals(0) && string.IsNullOrWhiteSpace(Publish.MediaName))
                 if (Publish.PubID.Equals(0) && Publish.MediaID.Equals(0) && string.IsNullOrEmpty(Publish.MediaName))
                        sb.AppendLine("缺少媒体ID");
                if (Publish.BeginTime.Equals(DateTime.MinValue))
                    sb.AppendLine("执行周期开始时间错误");
                if (Publish.EndTime.Equals(DateTime.MinValue))
                    sb.AppendLine("执行周期结束时间错误");
                if (Publish.BeginTime.Date > Publish.EndTime.Date)
                    sb.AppendLine("执行周期时间错误");
                if (Publish.EndTime.Date < DateTime.Now.Date)
                    sb.AppendLine("执行周期结束时间不能小于当前时间");
                if (!Publish.MediaType.Equals((int)Enum.MediaTypeEnum.APP))
                {
                    if (Publish.PurchaseDiscount < 0 || Publish.PurchaseDiscount > 1)
                        sb.AppendLine("采购折扣错误");
                    if (Publish.SaleDiscount < 0 || Publish.SaleDiscount > 1)
                        sb.AppendLine("销售折扣错误");
                }
                if (Publish.SaleDiscount < Publish.PurchaseDiscount)
                    sb.AppendLine("销售折扣应大于等于采购折扣");

                if (Publish.MediaType.Equals((int)Enum.MediaTypeEnum.APP)) {
                    if(Publish.PubID.Equals(0) && Publish.TemplateID.Equals(0))
                        sb.AppendLine("缺少模板ID");
                }
                #endregion
            }
            if (Publish.MediaType.Equals((int)Enum.MediaTypeEnum.微信))
            {
                #region V1.1.1微信
                if (this.Details == null || this.Details.Count.Equals(0))
                {
                    sb.AppendLine("缺少广告位信息");
                }
                else
                {
                    List<int> ps1 = new List<int> { -2, 6001, 6002, 6003, 6004 };
                    List<int> ps2 = new List<int> { -2, 8001, 8002, 8003 };
                    List<int> ps3 = new List<int> { -2 };
                    foreach (var d in Details)
                    {
                        if (!ps1.Contains(d.ADPosition1) || !ps2.Contains(d.ADPosition2) || !ps3.Contains(d.ADPosition3))
                        {
                            sb.AppendLine("维度错误!");
                            break;
                        }
                        if (d.Price < 0 || d.SalePrice < 0)
                        {
                            sb.AppendLine("价格不能小于0");
                            break;
                        }
                    }
                }

                #endregion
            }
            else if (Publish.MediaType.Equals((int)Enum.MediaTypeEnum.APP)) {
                #region V1.1.4APP
                if (this.Publish.TemplateID <= 0)
                {
                    sb.AppendLine("缺少模板");
                }
                if (Publish.PurchaseDiscount < 0)
                    sb.AppendLine("采购折扣错误");
                if (Publish.SaleDiscount < 0)
                    sb.AppendLine("销售折扣错误");
                if (this.PriceList == null || this.PriceList.Count.Equals(0))
                {
                    sb.AppendLine("缺少价格信息");
                }
                else
                {
                    List<int> ps1 = new List<int> { 11001, 11002 };
                    List<int> ps2 = new List<int> { 12001, 12002, 12003 };
                    foreach (var p in this.PriceList)
                    {
                        if (p.SalePrice <= 0m)
                            sb.Append("缺少销售价");
                        //媒体主不需要维护刊例价
                        //if (p.PubPrice <= 0m)
                        //sb.Append("缺少刊例价");
                        //if (p.SalePrice < p.PubPrice * Publish.PurchaseDiscount)
                        //{
                            //sb.Append("销售价应大于等于采购价");
                           // break;
                        //}
                        //if (p.PubPrice_Holiday < p.PubPrice_Holiday * Publish.PurchaseDiscount)
                        //{
                            //sb.Append("销售价应大于等于采购价");
                            //break;
                       // }
                    }
                }

                #endregion
            }
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }

    public class ModifyPublish
    {
        public int PubID { get; set; }
        public int TemplateID { get; set; }
        public string ADName { get; set; }
        public string PubName { get; set; }
        public string PubCode { get; set; }
        public int MediaType { get; set; }
        public int MediaID { get; set; }
        public string MediaName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        private decimal purchasediscount = 1;
        public decimal PurchaseDiscount {
            get { return purchasediscount; }
            set { purchasediscount = value; }
        }

        private decimal salediscount = 1;
        public decimal SaleDiscount {
            get { return salediscount; }
            set { salediscount = value; }
        }

        public bool IsAppointment { get; set; }
        public List<int> OrderRemark { get; set; }
        public string ImgUrl { get; set; }
        public int Wx_Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int CreateUserID { get; set; }
        public int LastUpdateUserID { get; set; }

        private bool hasholiday = false;
        public bool HasHoliday {
            get { return hasholiday; }
            set { hasholiday = value; }
        }
    }

    public class ModifyPublishDetail
    {
        public int ADPosition1 { get; set; }
        public int ADPosition2 { get; set; }
        public int ADPosition3 { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public List<string> ImgUrls { get; set; }
    }

    /// <summary>
    /// APP用
    /// </summary>
    public class ADPrice
    {
        public int RecID { get; set; }
        public int CarouselNumber { get; set; }
        public int SaleType { get; set; }
        public int SalePlatform { get; set; }
        public int ADStyle { get; set; }
        public int SaleArea { get; set; }
        public int ClickCount { get; set; }
        public int ExposureCount { get; set; }
        public decimal PubPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal PubPrice_Holiday { get; set; }
        public decimal SalePrice_Holiday { get; set; }
        public int Status { get; set; }
    }
}
