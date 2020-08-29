using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.StockBroker
{
    public class LoginDto
    {
        /// <summary>
        /// 经纪人ID
        /// </summary>
        public int dealerID { get; set; } = -2;
        /// <summary>
        /// 经销商名称
        /// </summary>
        public string dealerName { get; set; } = string.Empty;
        /// <summary>
        /// 联系人
        /// </summary>
        public string contacts { get; set; } = string.Empty;
        /// <summary>
        /// 联系电话
        /// </summary>
        public string contactNumber { get; set; } = string.Empty;
        /// <summary>
        /// 营业执照
        /// </summary>
        public string businessLicence { get; set; } = string.Empty;       
        /// <summary>
        /// 经纪人状态
        /// 0:停用，1：启用
        /// </summary>
        public int status { get; set; } = -2;
    }
}
