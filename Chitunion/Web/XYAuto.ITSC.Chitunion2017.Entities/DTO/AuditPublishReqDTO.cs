using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class AuditPublishReqDTO
    {
        public int PubID { get; set; }
        public List<int> PubIDs { get; set; }
        public string RejectReason { get; set; }
        public int OpType { get; set; }
        public bool IsBatch { get; set; }

        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (!IsBatch)
            {
                #region 单条审核
                if (PubID <= 0)
                {
                    sb.Append("刊例ID错误！");
                }
                else
                {
                    int[] arr = new int[] { 42002, 42003, 42011, 42012 };
                    if (!arr.Contains(OpType))
                    {
                        sb.Append("操作类型错误！");
                    }
                }
                msg = sb.ToString();
                #endregion
            }
            else {
                #region 批量审核
                if (PubIDs == null || PubIDs.Count.Equals(0))
                {
                    sb.Append("刊例ID错误！");
                }
                else
                {
                    int[] arr = new int[] { 1, 2, 3, 4 };
                    if (!arr.Contains(OpType))
                    {
                        sb.Append("操作类型错误！");
                    }
                }
                msg = sb.ToString();
                #endregion
            }
            return msg.Length.Equals(0);
        }
    }
}
