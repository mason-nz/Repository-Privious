using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class QueryQS_IM_Result
    {
        /// <summary>
        /// 对话id
        /// </summary>
        public string CSID { get; set; }
        /// <summary>
        /// 对话开始时间
        /// </summary>
        public string BeginTime { get; set; }
        /// <summary>
        /// 对话结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 消息次数起始值
        /// </summary>
        public string BeginCount { get; set; }
        /// <summary>
        /// 消息次数终止值
        /// </summary>
        public string EndCount { get; set; }

        /// <summary>
        /// 坐席ID
        /// </summary>
        public string AgentUserID { get; set; }
        /// <summary>
        /// 评分开始日期
        /// </summary>
        public string ScoreBeginTime { get; set; }
        /// <summary>
        /// 评分结束日期
        /// </summary>
        public string ScoreEndTime { get; set; }
        /// <summary>
        /// 评分表
        /// </summary>
        public string ScoreTable { get; set; }

        /// <summary>
        /// 组ID
        /// </summary>
        public string BGID { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public string BusinessLine { get; set; }
        /// <summary>
        /// 申诉开始日期
        /// </summary>
        public string AppealBeginTime { get; set; }
        /// <summary>
        /// 申诉结束日期
        /// </summary>
        public string AppealEndTime { get; set; }
        /// <summary>
        /// 评分人
        /// </summary>
        public string ScoreCreater { get; set; }

        /// <summary>
        /// 成绩
        /// </summary>
        public string Qualified { get; set; }
        /// <summary>
        /// 评分状态
        /// </summary>
        public string QSResultStatus { get; set; }
        /// <summary>
        /// 申诉结果
        /// </summary>
        public string QSStateResult { get; set; }

        /// <summary>
        /// 是否评价
        /// </summary>
        public string QSResultScore { get; set; }
        /// <summary>
        /// 对服务评价
        /// </summary>
        public string PerQSResultScore { get; set; }
        /// <summary>
        /// 对产品评价
        /// </summary>
        public string ProQSResultScore { get; set; }

        /// <summary>
        /// 当前登录人
        /// </summary>
        public int LoginUerID { get; set; }

        public void InitCheck()
        {
            //会话id
            if (!string.IsNullOrEmpty(CSID))
            {
                CSID = CommonFunction.ObjectToInteger(CSID).ToString();
            }

            //会话时间需要处理
            if (!string.IsNullOrEmpty(BeginTime))
            {
                BeginTime = BeginTime.Insert(10, " ");
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                EndTime = EndTime.Insert(10, " ");
            }

            //消息次数
            if (!string.IsNullOrEmpty(BeginCount))
            {
                BeginCount = CommonFunction.ObjectToInteger(BeginCount).ToString();
            }
            if (!string.IsNullOrEmpty(EndCount))
            {
                EndCount = CommonFunction.ObjectToInteger(EndCount).ToString();
            }
        }
    }
}
