using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CustomerCallInPressKey
    {
        public readonly static CustomerCallInPressKey Instance = new CustomerCallInPressKey();

        /// 删除厂家id下所有数据
        /// <summary>
        /// 删除厂家id下所有数据
        /// </summary>
        /// <param name="vid"></param>
        public void DeleteDataByVenderCallID(string vendercallid)
        {
            Dal.CustomerCallInPressKey.Instance.DeleteDataByVenderCallID(vendercallid);
        }
    }
}
