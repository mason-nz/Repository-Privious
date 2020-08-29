using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.Publish
{
    /// <summary>
    /// ls
    /// </summary>
    public class PublishDetailInfo
    {
        public int RecID { get; set; }
        public int PubID { get; set; }
        public MediaTypeEnum MediaType { get; set; }
        public int MediaID { get; set; }
        private int adposition1 = Constant.INT_INVALID_VALUE;
        public int ADPosition1 {
            get { return adposition1; }
            set { adposition1 = value; }
        }
        private int adposition2 = Constant.INT_INVALID_VALUE;
        public int ADPosition2 {
            get { return adposition2; }
            set { adposition2 = value; }
        }
        private int adposition3 = Constant.INT_INVALID_VALUE;
        public int ADPosition3 {
            get { return adposition3; }
            set { adposition3 = value; }
        }
        public decimal Price { get; set; }
        public bool IsCarousel { get; set; }
        public int BeginPlayDays { get; set; }
        private PublishStatusEnum publishstatus = PublishStatusEnum.新建;
        public PublishStatusEnum PublishStatus {
            get { return publishstatus; }
            set { publishstatus = value; }
        }
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; }


        public string ADPosition1Name { get; set; }
        public string ADPosition2Name { get; set; }
        public string ADPosition3Name { get; set; }

        #region V1_1
        /// <summary>
        /// 销售价格V1_1
        /// </summary>
        public decimal SalePrice { get; set; }
        #endregion

        public decimal CostReferencePrice { get; set; }
        public int CostDetailID { get; set; }

    }
}
