using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.EP.Common.Lib.Excel;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.ImportData.Dto
{
    public class ImportMediaAppDto
    {
        public ImportMediaAppDto()
        {
            this.PublishStatus = (int)PublishStatusEnum.已上架;
        }
        public int MediaID { get; set; }
        [ExcelTitle("媒体名称")]
        public string Name { get; set; }

        public string HeadIconURL { get; set; }
        public int CategoryID { get; set; }
        public int ProvinceID { get; set; }
        public int CityID { get; set; }
        public int Terminal { get; set; }
        public int DailyLive { get; set; }
        public int DailyIP { get; set; }
        public string WebSite { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
         [ExcelTitle("创建人")]
        public int CreateUserID { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int LastUpdateUserID { get; set; }


        /* Publish_ExtendInfoPCAPP */

        public int ADDetailID { get; set; }
        public int PubID { get; set; }
        public int MediaType { get; set; }

        public string AdLegendURL { get; set; }
        [ExcelTitle("广告位位置")]
        public string AdPosition { get; set; }
        [ExcelTitle("广告形式")]
        public string AdForm { get; set; }
        [ExcelTitle("时长（秒）")]
        public int DisplayLength { get; set; }
        [ExcelTitle("是否可点击")]
        public bool CanClick { get; set; }
        [ExcelTitle("轮播数")]
        public int CarouselCount { get; set; }
        [ExcelTitle("位置")]
        public string PlayPosition { get; set; }
        [ExcelTitle("日均曝光量")]
        public int DailyExposureCount { get; set; }
        public bool CPM { get; set; }
        public bool CarouselPlay { get; set; }
        [ExcelTitle("日均点击量")]
        public int DailyClickCount { get; set; }
        public bool CPM2 { get; set; }
        public bool CarouselPlay2 { get; set; }
        [ExcelTitle("第三方监测")]
        public string ThrMonitor { get; set; }
        [ExcelTitle("系统平台")]
        public string SysPlatform { get; set; }
        [ExcelTitle("样式")]
        public string Style { get; set; }
        [ExcelTitle("是否配送")]
        public bool IsDispatching { get; set; }
        [ExcelTitle("广告展示逻辑")]
        public string ADShow { get; set; }
        [ExcelTitle("广告位说明")]
        public string ADRemark { get; set; }
        public string AcceptBusinessIDs { get; set; }
        public string NotAcceptBusinessIDs { get; set; }

        [ExcelTitle("接受行业")]
        public string AcceptBusinessNames { get; set; }
        [ExcelTitle("不接受行业")]
        public string NotAcceptBusinessNames { get; set; }



        /* 刊例 */

        public string PubCode { get; set; }
        [ExcelTitle("刊例开始日期")]
        public DateTime BeginTime { get; set; }
        [ExcelTitle("刊例结束日期")]
        public DateTime EndTime { get; set; }
        //[ExcelTitle("采购折扣")]
        public decimal PurchaseDiscount { get; set; }

        public decimal SaleDiscount { get; set; }
        //public int Status { get; set; }
        public int PublishStatus { get; set; }

        /* 刊例详情 */

        [ExcelTitle("售卖方式")]
        public string AdPosition1售卖方式 { get; set; }


        public int ADPosition1 { get; set; }
        public int ADPosition2 { get; set; }
        public int ADPosition3 { get; set; }
        [ExcelTitle("价格")]
        public decimal Price { get; set; }
        public bool IsCarousel { get; set; }

        [ExcelTitle("起投天数")]
        public int BeginPlayDays { get; set; }

        public List<Entities.Publish.PublishDetailInfo> PublishDetailList
        {
            get
            {
                var list = new List<PublishDetailInfo>();

                if (!string.IsNullOrWhiteSpace(AdPosition1售卖方式))
                {
                    list.Add(new PublishDetailInfo()
                    {
                        PubID = PubID,
                        ADPosition1 = AdPosition1售卖方式.Equals("CPD", StringComparison.OrdinalIgnoreCase) ? (int)SaleMode.CPD : (int)SaleMode.CPM,
                        ADPosition2 = 0,
                        ADPosition3 = 0,
                        Price = Price,
                        CreateTime = CreateTime,
                        CreateUserID = CreateUserID,
                        MediaType = MediaTypeEnum.APP,
                        MediaID = MediaID,
                        BeginPlayDays = BeginPlayDays,
                        IsCarousel = CarouselCount > 0,
                        PublishStatus = PublishStatusEnum.已上架
                    });
                }

                return list;
            }
            set { }
        }

    }
}
