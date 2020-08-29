using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class IVRSatisfaction
    {
        public static readonly IVRSatisfaction Instance = new IVRSatisfaction();
        protected IVRSatisfaction()
        { }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.IVRSatisfaction model)
        {
            return Dal.IVRSatisfaction.Instance.Insert(model);
        }
        /// <summary>
        /// 根据CallID查询满意度分数
        /// </summary>
        /// <param name="callid"></param>
        /// <returns></returns>
        public int GetIVRScoreBYCallID(long callid, string tableEndName)
        {
            return Dal.IVRSatisfaction.Instance.GetIVRScoreBYCallID(callid, tableEndName);
        }
    }
}
