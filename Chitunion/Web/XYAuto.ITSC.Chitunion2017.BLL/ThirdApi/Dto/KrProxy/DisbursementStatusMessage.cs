namespace XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.KrProxy
{

    public class RespDisbursementStatusMessage
    {
        public DisbursementStatusMessage Data { get; set; }
        public string Sign { get; set; }
    }

    /// <summary>
    /// 付款单状态消息
    /// </summary>
    public class DisbursementStatusMessage
    {
        /// <summary>
        /// 付款申请单号
        /// </summary>
        public string DisbursementNo { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 业务单号
        /// </summary>
        public string BizDisbursementNo { get; set; }

        /// <summary>
        /// 业务收款单号
        /// </summary>
        public string BizNo { get; set; }

        /// <summary>
        /// 付款状态
        /// </summary>
        public DisbursementStatus Status { get; set; }

        /// <summary>
        /// 付款状态名称
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 付款状态
    /// </summary>
    public enum DisbursementStatus
    {
        待处理 = 0,
        已锁定 = 2,
        已撤销 = 4,
        处理中 = 6,
        待审核 = 8,
        结算中 = 10,
        结算成功 = 12,
        结算失败 = 14
    }
}