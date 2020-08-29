using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CustomerCallIn
    {
        public static CustomerCallIn Instance = new CustomerCallIn();

        /// 根据厂家id获取信息
        /// <summary>
        /// 根据厂家id获取信息
        /// </summary>
        /// <param name="VenderCallID"></param>
        /// <returns></returns>
        public CustomerCallInInfo GetCustomerCallInInfoByVenderCallID(string VenderCallID)
        {
            return Dal.CustomerCallIn.Instance.GetCustomerCallInInfoByVenderCallID(VenderCallID);
        }
    }
}
