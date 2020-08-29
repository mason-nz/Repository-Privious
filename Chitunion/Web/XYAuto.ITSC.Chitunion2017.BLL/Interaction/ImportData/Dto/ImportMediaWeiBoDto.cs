using System;
using System.Collections.Generic;
using BitAuto.EP.Common.Lib.Excel;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.ImportData.Dto
{
    public class ImportMediaWeiBoDto
    {
        public ImportMediaWeiBoDto()
        {
            this.PublishStatus = (int) PublishStatusEnum.已上架;
        }

        public int MediaID { get; set; }
        [ExcelTitle("微博账号")]
        public string Number { get; set; }
        [ExcelTitle("微博昵称")]
        public string Name { get; set; }
        [ExcelTitle("性别")]
        public string Sex { get; set; }
        [ExcelTitle("头像")]
        public string HeadIconURL { get; set; }
        //简介
        //所在地
        //覆盖区域
        [ExcelTitle("粉丝数")]
        public int FansCount { get; set; }
        public string FansCountURL { get; set; }
        [ExcelTitle("粉丝性别")]
        public string FansSex { get; set; }
        [ExcelTitle("行业分类")]
        public int CategoryID { get; set; }
        [ExcelTitle("媒体领域")]
        public int AreaID { get; set; }
        [ExcelTitle("职业")]
        public int Profession { get; set; }
        public int ProvinceID { get; set; }
        public int CityID { get; set; }
        [ExcelTitle("媒体级别")]
        public int LevelType { get; set; }
        [ExcelTitle("微博认证")]
        public int AuthType { get; set; }
        public string Sign { get; set; }
        [ExcelTitle("下单备注")]
        public int OrderRemark { get; set; }
        [ExcelTitle("是否预约")]
        public bool IsReserve { get; set; }
        public int Status { get; set; }
        public int Source { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int LastUpdateUserID { get; set; }
        [ExcelTitle("创建人")]
        public string CreateUserName { get; set; }

        /* 互动参数 */

        public int RecID { get; set; }
        public int MeidaType { get; set; }
        [ExcelTitle("平均转发数")]
        public int AverageForwardCount { get; set; }
        [ExcelTitle("平均评论数")]
        public int AverageCommentCount { get; set; }
        [ExcelTitle("平均点赞数")]
        public int AveragePointCount { get; set; }
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
        [ExcelTitle("硬广直发价")]
        public decimal AdPosition1硬广直发价 { get; set; }
        [ExcelTitle("硬广原创+直发价")]
        public decimal AdPosition1硬广原创直发价 { get; set; }
        [ExcelTitle("硬广转发价")]
        public decimal AdPosition1硬广转发价 { get; set; }

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

                #region 硬广

                if (AdPosition1硬广直发价 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1硬广直发价, (int)AdTypeMapping.硬广, (int)AdFormality4.直发, 0));
                }
                if (AdPosition1硬广原创直发价 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1硬广原创直发价, (int)AdTypeMapping.硬广, (int)AdFormality4.原创发布, 0));
                }
                if (AdPosition1硬广转发价 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1硬广转发价, (int)AdTypeMapping.硬广, (int)AdFormality4.转发, 0));
                }
                #endregion

                #region 软广

                if (AdPosition1软广直发价 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1软广直发价, (int)AdTypeMapping.软广, (int)AdFormality4.直发, 0));
                }
                if (AdPosition1软广原创直发价 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1软广原创直发价, (int)AdTypeMapping.软广, (int)AdFormality4.原创发布, 0));
                }
                if (AdPosition1软广转发价 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1软广转发价, (int)AdTypeMapping.软广, (int)AdFormality4.转发, 0));
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
                MediaType = MediaTypeEnum.微博,
                PublishStatus = PublishStatusEnum.已上架,
                MediaID = MediaID
            };
        }
    }
}
