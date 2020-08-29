using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.DSC.IM_2015.Entities.Constants;
namespace BitAuto.DSC.IM_2015.Entities
{
    public class QueryAgentStatusDetail
    {
        public QueryAgentStatusDetail()
        {
            Starttime = Constant.DATE_INVALID_VALUE;
            UserName = Constant.STRING_EMPTY_VALUE;
            AgentNum = Constant.STRING_EMPTY_VALUE;
            UserID = Constant.INT_INVALID_VALUE;
            Status = Constant.INT_INVALID_VALUE;

        }

        /// <summary>
        /// 客服
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public string AgentNum { get; set; }
        /// <summary>
        /// 员工Id
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime Starttime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
