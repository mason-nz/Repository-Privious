﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;

namespace XYAuto.ChiTu2018.API.Models.Withdrawals
{
    /// <summary>
    /// 注释：ReqThirdWithdrawlsDto
    /// 作者：lix
    /// 日期：2018/5/18 11:30:47
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ReqThirdWithdrawlsDto: ReqThirdBaseDto
    {
        /// <summary>
        /// 提现金额：必填
        /// </summary>
        [Required()]
        [Range(0, Double.MaxValue)]
        public decimal WithdrawalsPrice { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 申请来源
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public int ApplySource { get; set; }

    }
}