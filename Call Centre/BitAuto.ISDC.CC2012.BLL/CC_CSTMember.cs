using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
   public class CC_CSTMember
   {
       #region Instance
       public static readonly CC_CSTMember Instance = new CC_CSTMember();
       #endregion

        /// <summary>
        /// 获取要导出的排期信息
        /// </summary>
        /// <param name="MemberStr"></param>
        /// <returns></returns>
        public DataTable GetOrderInfo(string MemberStr)
        {
            return Dal.CC_CSTMember.Instance.GetOrderInfo(MemberStr);
        }

        /// <summary>
        /// 根据任务编号查询车商通会员信息
        /// </summary>
        /// <param name="TID"></param>
        /// <returns></returns>
        public List<Entities.ProjectTask_CSTMember> GetCC_CSTMemberByTID(string TID)
        {
            return Dal.CC_CSTMember.Instance.GetCC_CSTMemberByTID(TID);
        }
    }
}
