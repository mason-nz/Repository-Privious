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
    public class ImportMediaBroadcastDto
    {
        public ImportMediaBroadcastDto()
        {
            this.PublishStatus = (int) PublishStatusEnum.已上架;
        }
        public int MediaID { get; set; }

        [ExcelTitle("所属平台")]
        public int Platform { get; set; }

        public string RoomID { get; set; }
        [ExcelTitle("平台账号")]
        public string Number { get; set; }
        [ExcelTitle("昵称")]
        public string Name { get; set; }
        [ExcelTitle("头像")]
        public string HeadIconURL { get; set; }
        [ExcelTitle("性别")]
        public string Sex { get; set; }
        [ExcelTitle("粉丝数")]
        public int FansCount { get; set; }
        [ExcelTitle("行业分类")]
        public int CategoryID { get; set; }
        [ExcelTitle("职业")]
        public int Profession { get; set; }
        //简介
        //所在地
        //覆盖区域

        public int ProvinceID { get; set; }
        public int CityID { get; set; }
        [ExcelTitle("是否认证")]
        public int IsAuth { get; set; }
        [ExcelTitle("媒体级别")]
        public int LevelType { get; set; }
        [ExcelTitle("预约情况")]
        public bool IsReserve { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int LastUpdateUserID { get; set; }

        [ExcelTitle("创建人")]
        public string CreateUserName { get; set; }


        /* 互动参数 */
        public int RecID { get; set; }
        public int MeidaType { get; set; }

        [ExcelTitle("直播观众数")]
        public int AudienceCount { get; set; }
        [ExcelTitle("最高观众数")]
        public int MaximumAudience { get; set; }
        [ExcelTitle("平均观众数")]
        public int AverageAudience { get; set; }
        [ExcelTitle("累计打赏数")]
        public int CumulateReward { get; set; }
        [ExcelTitle("累计收入")]
        public int CumulateIncome { get; set; }
        [ExcelTitle("累计点赞数")]
        public int CumulatePoints { get; set; }
        [ExcelTitle("累计送出数")]
        public int CumulateSendCount { get; set; }
        public string ScreenShotURL { get; set; }

        /* 刊例 */
        public int PubID { get; set; }
        public int MediaType { get; set; }
        public string PubCode { get; set; }
        [ExcelTitle("刊例开始日期")]
        public DateTime BeginTime { get; set; }
        [ExcelTitle("刊例结束日期")]
        public DateTime EndTime { get; set; }
        [ExcelTitle("采购折扣")]
        public decimal PurchaseDiscount { get; set; }
        [ExcelTitle("销售折扣")]
        public decimal SaleDiscount { get; set; }
        //public int Status { get; set; }
        public int PublishStatus { get; set; }

        /* 刊例详情 */
        [ExcelTitle("现场直播价")]
        public decimal AdPosition1现场直播价 { get; set; }
        [ExcelTitle("直播广告值入价")]
        public decimal AdPosition1直播广告值入价 { get; set; }

        public List<Entities.Publish.PublishDetailInfo> PublishDetailList
        {
            get
            {
                var list = new List<PublishDetailInfo>();

                if (AdPosition1现场直播价 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1现场直播价, (int)AdFormality5.活动现场直播, 0, 0));
                }
                if (AdPosition1直播广告值入价 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1直播广告值入价, (int)AdFormality5.直播广告植入, 0, 0));
                }
                return list;
            }
            set { }
        }

        public PublishDetailInfo GetPublishDetailInfo(decimal price, int adPosition1, int adPosition2,
          int adPosition3)
        {
            return new PublishDetailInfo()
            {
                PubID = PubID,
                ADPosition1 = adPosition1,
                ADPosition2 = adPosition2,
                ADPosition3 = adPosition3,
                Price = price,
                CreateTime = CreateTime,
                CreateUserID = CreateUserID,
                MediaType = MediaTypeEnum.直播,
                PublishStatus = PublishStatusEnum.已上架,
                MediaID = MediaID
            };
        }
    }
}
