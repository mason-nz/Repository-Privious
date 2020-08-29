using System;
using System.Collections.Generic;
using BitAuto.EP.Common.Lib.Excel;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.ImportData.Dto
{
    public class ImportMediaVideoDto
    {
        public ImportMediaVideoDto()
        {
            this.PublishStatus = (int) PublishStatusEnum.已上架;
        }
        public int MediaID { get; set; }
        [ExcelTitle("所属平台")]
        public int Platform { get; set; }
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
        public string FansCountURL { get; set; }
        [ExcelTitle("行业分类")]
        public int CategoryID { get; set; }
        [ExcelTitle("职业")]
        public int Profession { get; set; }

        //所在地
        //覆盖区域
        [ExcelTitle("是否认证")]
        public bool AuthType { get; set; }
        [ExcelTitle("媒体级别")]
        public int LevelType { get; set; }
        public int ProvinceID { get; set; }
        public int CityID { get; set; }
        [ExcelTitle("是否预约")]
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
        [ExcelTitle("平均播放数")]
        public int AveragePlayCount { get; set; }
        [ExcelTitle("平均点赞数")]
        public int AveragePointCount { get; set; }
        [ExcelTitle("平均评论数")]
        public int AverageCommentCount { get; set; }
        [ExcelTitle("平均弹幕数")]
        public int AverageBarrageCount { get; set; }
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
        [ExcelTitle("直发价")]
        public decimal AdPosition1直发价 { get; set; }
        [ExcelTitle("原创+直发")]
        public decimal AdPosition1原创直发 { get; set; }
        [ExcelTitle("转发价")]
        public decimal AdPosition1转发价 { get; set; }


        [ExcelTitle("软广直发价")]
        public decimal AdPosition1软广直发价 { get; set; }
        [ExcelTitle("软广原创+直发价")]
        public decimal AdPosition1软广原创直发价 { get; set; }
        [ExcelTitle("软广转发价")]
        public decimal AdPosition1软广转发价 { get; set; }


        public List<Entities.Publish.PublishDetailInfo> PublishDetailList
        {
            get
            {
                var list = new List<PublishDetailInfo>();

                #region 直发

                if (AdPosition1直发价 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1直发价, (int)AdFormality4.直发, 0, 0));
                }
                if (AdPosition1原创直发 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1原创直发, (int)AdFormality4.原创发布, 0, 0));
                }
                if (AdPosition1转发价 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1转发价, (int)AdFormality4.转发, 0, 0));
                }

                #endregion

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
                MediaType = MediaTypeEnum.视频,
                PublishStatus = PublishStatusEnum.已上架,
                MediaID = MediaID
            };
        }
    }
}
