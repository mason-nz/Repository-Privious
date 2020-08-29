using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.Publish
{
    /// <summary>
    /// ls
    /// </summary>
    public class PublishBasicInfo
    {

        public int PubID { get; set; }

        public MediaTypeEnum MediaType { get; set; }
      
        public int MediaID { get; set; }

        public string PubCode { get; set; }

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

        private AuditStatusEnum status = AuditStatusEnum.初始状态;
        public AuditStatusEnum Status
        {
            get { return status; }
            set { status = value; }
        }

        private PublishStatusEnum publishstatus = PublishStatusEnum.新建;
        public PublishStatusEnum PublishStatus {

            get { return publishstatus; }
            set { publishstatus = value; }
        }

        public DateTime CreateTime { get; set; }

        public int CreateUserID { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int LastUpdateUserID { get; set; }

        public string MediaName { get; set; }

        #region V1_1
        /// <summary>
        /// 刊例名称V1_1 
        /// </summary>
        public string PubName { get; set; }
        private PublishBasicStatusEnum wx_status = PublishBasicStatusEnum.默认;
        /// <summary>
        /// 刊例状态V1_1
        /// </summary>
        public PublishBasicStatusEnum Wx_Status {
            get { return wx_status; }
            set { wx_status = value; }
        }
        /// <summary>
        /// 是否预约V1_1
        /// </summary>
        public bool IsAppointment { get; set; }

        /// <summary>
        /// 删除标志 -1：删除 
        /// </summary>
        public int IsDel { get; set; }
        #endregion

        #region V1.1.4
        public int TemplateID { get; set; }

        private bool hasholiday = false;
        public bool HasHoliday {
            get { return hasholiday; }
            set { hasholiday = value; }
        }
        #endregion

        public decimal OriginalReferencePrice { get; set; }
    }
}
