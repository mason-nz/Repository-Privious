﻿using System;

namespace XYAuto.ChiTu2018.Service.LE.Query.Dto.Response
{
    /// <summary>
    /// 注释：RespWithdrawalsDto
    /// 作者：lix
    /// 日期：2018/5/15 12:46:33
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class RespWithdrawalsDto
    {
        public int Id { get; set; }
        public int RecId { get; set; }
        public string UserName { get; set; }
        public string UserTypeName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public decimal WithdrawalsPrice { get; set; }
        public decimal IndividualTaxPeice { get; set; }
        public decimal PracticalPrice { get; set; }
        public string PayeeAccount { get; set; }
        public DateTime? PayDate { get; set; }
        public string Reason { get; set; }

        public int PayStatus { get; set; }
        public string PayStatusName { get; set; }
        public string TrueName { get; set; }

        public DateTime AuditTime { get; set; }
        public int AuditStatus { get; set; }
        public string AuditStatusName { get; set; }
        public string RejectMsg { get; set; }
    }
}
