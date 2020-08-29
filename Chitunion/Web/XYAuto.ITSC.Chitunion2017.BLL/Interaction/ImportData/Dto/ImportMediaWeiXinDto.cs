using System;
using System.Collections.Generic;
using BitAuto.EP.Common.Lib.Excel;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.ImportData.Dto
{
    public class ImportMediaWeiXinDto
    {
        public ImportMediaWeiXinDto()
        {
            this.CreateTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
            this.PublishStatus = (int)PublishStatusEnum.已上架;
        }

        public int MediaID { get; set; }
        [ExcelTitle("微信号")]
        public string Number { get; set; }
        [ExcelTitle("微信名称")]
        public string Name { get; set; }
        [ExcelTitle("头像")]
        public string HeadIconURL { get; set; }
        [ExcelTitle("二维码")]
        public string TwoCodeURL { get; set; }
        [ExcelTitle("公众号介绍")]
        public string Desc { get; set; }
        //公众号介绍
        //所在地
        //覆盖区域
        [ExcelTitle("所在地")]
        public string LocationPlace { get; set; }

        [ExcelTitle("覆盖区域")]
        public string CoverageArea { get; set; }


        [ExcelTitle("粉丝数")]
        public int FansCount { get; set; }
        [ExcelTitle("粉丝男百分比")]
        public decimal FansMalePer { get; set; }
        [ExcelTitle("粉丝女百比分")]
        public decimal FansFemalePer { get; set; }

        public int CategoryID { get; set; }
        [ExcelTitle("行业分类")]
        public string CategoryName { get; set; }
        public int ProvinceID { get; set; }
        public int CityID { get; set; }
        public string Sign { get; set; }
        [ExcelTitle("媒体领域")]
        public string AreaName { get; set; }
        public int AreaID { get; set; }


        public int LevelType { get; set; }
        [ExcelTitle("媒体级别")]
        public string LevelTypeName { get; set; }

        [ExcelTitle("微信是否认证")]
        public bool IsAuth { get; set; }
        [ExcelTitle("下单备注")]
        public string OrderRemark { get; set; }
        [ExcelTitle("是否预约")]
        public bool IsReserve { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        [ExcelTitle("创建人")]
        public int CreateUserID { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int LastUpdateUserID { get; set; }


        public int Source { get; set; }

        /* 互动参数 */

        public int RecID { get; set; }
        public int MeidaType { get; set; }
        [ExcelTitle("参考阅读数")]
        public int ReferReadCount { get; set; }
        [ExcelTitle("平均点赞数")]
        public int AveragePointCount { get; set; }
        [ExcelTitle("10W+阅读量的文章数 ")]
        public int MoreReadCount { get; set; }
        [ExcelTitle("原创文章数")]
        public int OrigArticleCount { get; set; }
        [ExcelTitle("周更新频率")]
        public int UpdateCount { get; set; }
        [ExcelTitle("最高阅读数")]
        public int MaxinumReading { get; set; }
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
        [ExcelTitle("单图文硬广发布")]
        public decimal AdPosition1单图文硬广发布 { get; set; }
        [ExcelTitle("单图文硬广原创+发布")]
        public decimal AdPosition1单图文硬广原创发布 { get; set; }
        [ExcelTitle("单图文软广发布")]
        public decimal AdPosition1单图文软广发布 { get; set; }
        [ExcelTitle("单图文软广原创+发布")]
        public decimal AdPosition1单图文软广原创发布 { get; set; }

        [ExcelTitle("多图文头条硬广发布")]
        public decimal AdPosition1多图文头条硬广发布 { get; set; }
        [ExcelTitle("多图文头条硬广原创+发布")]
        public decimal AdPosition1多图文头条硬广原创发布 { get; set; }
        [ExcelTitle("多图文头条软广发布")]
        public decimal AdPosition1多图文头条软广发布 { get; set; }
        [ExcelTitle("多图文头条软广原创+发布")]
        public decimal AdPosition1多图文头条软广原创发布 { get; set; }

        [ExcelTitle("多图文第二头条硬广发布")]
        public decimal AdPosition1多图文第二头条硬广发布 { get; set; }
        [ExcelTitle("多图文第二条硬广原创+发布")]
        public decimal AdPosition1多图文第二条硬广原创发布 { get; set; }
        [ExcelTitle("多图第二条软广发布")]
        public decimal AdPosition1多图第二条软广发布 { get; set; }
        [ExcelTitle("多图文第二条软广原创+发布")]
        public decimal AdPosition1多图文第二条软广原创发布 { get; set; }

        [ExcelTitle("多图文第3—n条硬广发布")]
        public decimal AdPosition1多图文第3n条硬广发布 { get; set; }
        [ExcelTitle("多图文第3—n条硬广原创+发布")]
        public decimal AdPosition1多图文第3n条硬广原创发布 { get; set; }
        [ExcelTitle("多图第3—n条软广发布")]
        public decimal AdPosition1多图第3n条软广发布 { get; set; }
        [ExcelTitle("多图文第3—n条软广原创+发布")]
        public decimal AdPosition1多图文第3n条软广原创发布 { get; set; }

        public int ADPosition2 { get; set; }
        public int ADPosition3 { get; set; }
        public decimal Price { get; set; }
        public bool IsCarousel { get; set; }
        public int BeginPlayDays { get; set; }

        public List<Entities.Publish.PublishDetailInfo> PublishDetailList
        {
            get
            {
                var list = new List<PublishDetailInfo>();

                #region 单图文

                if (AdPosition1单图文硬广发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1单图文硬广发布, (int)AdPositionMapping.单图文,
                        (int)AdTypeMapping.硬广, (int)AdFormality3.发布));
                }
                if (AdPosition1单图文硬广原创发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1单图文硬广原创发布, (int)AdPositionMapping.单图文,
                      (int)AdTypeMapping.硬广, (int)AdFormality3.原创发布));
                }
                if (AdPosition1单图文软广发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1单图文软广发布, (int)AdPositionMapping.单图文,
                      (int)AdTypeMapping.软广, (int)AdFormality3.发布));
                }
                if (AdPosition1单图文软广原创发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1单图文软广原创发布, (int)AdPositionMapping.单图文,
                      (int)AdTypeMapping.软广, (int)AdFormality3.原创发布));
                }
                #endregion

                #region 多图文头条

                if (AdPosition1多图文头条硬广发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1多图文头条硬广发布, (int)AdPositionMapping.多图文头条,
                        (int)AdTypeMapping.硬广, (int)AdFormality3.发布));
                }
                if (AdPosition1多图文头条硬广原创发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1多图文头条硬广原创发布, (int)AdPositionMapping.单图文,
                      (int)AdTypeMapping.硬广, (int)AdFormality3.原创发布));
                }
                if (AdPosition1多图文头条软广发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1多图文头条软广发布, (int)AdPositionMapping.单图文,
                      (int)AdTypeMapping.软广, (int)AdFormality3.发布));
                }
                if (AdPosition1多图文头条软广原创发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1多图文头条软广原创发布, (int)AdPositionMapping.单图文,
                      (int)AdTypeMapping.软广, (int)AdFormality3.原创发布));
                }

                #endregion

                #region 多图文第二头条

                if (AdPosition1多图文第二头条硬广发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1多图文第二头条硬广发布, (int)AdPositionMapping.多图文头条,
                        (int)AdTypeMapping.硬广, (int)AdFormality3.发布));
                }
                if (AdPosition1多图文第二条硬广原创发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1多图文第二条硬广原创发布, (int)AdPositionMapping.单图文,
                      (int)AdTypeMapping.硬广, (int)AdFormality3.原创发布));
                }
                if (AdPosition1多图第二条软广发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1多图第二条软广发布, (int)AdPositionMapping.单图文,
                      (int)AdTypeMapping.软广, (int)AdFormality3.发布));
                }
                if (AdPosition1多图文第二条软广原创发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1多图文第二条软广原创发布, (int)AdPositionMapping.单图文,
                      (int)AdTypeMapping.软广, (int)AdFormality3.原创发布));
                }

                #endregion

                #region 多图文第3n条

                if (AdPosition1多图文第3n条硬广发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1多图文第3n条硬广发布, (int)AdPositionMapping.多图文头条,
                        (int)AdTypeMapping.硬广, (int)AdFormality3.发布));
                }
                if (AdPosition1多图文第3n条硬广原创发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1多图文第3n条硬广原创发布, (int)AdPositionMapping.单图文,
                      (int)AdTypeMapping.硬广, (int)AdFormality3.原创发布));
                }
                if (AdPosition1多图第3n条软广发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1多图第3n条软广发布, (int)AdPositionMapping.单图文,
                      (int)AdTypeMapping.软广, (int)AdFormality3.发布));
                }
                if (AdPosition1多图文第3n条软广原创发布 > 0)
                {
                    list.Add(GetPublishDetailInfo(AdPosition1多图文第3n条软广原创发布, (int)AdPositionMapping.单图文,
                      (int)AdTypeMapping.软广, (int)AdFormality3.原创发布));
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
                MediaType = MediaTypeEnum.微信,
                PublishStatus = PublishStatusEnum.已上架,
                MediaID = MediaID
            };
        }
    }


}
