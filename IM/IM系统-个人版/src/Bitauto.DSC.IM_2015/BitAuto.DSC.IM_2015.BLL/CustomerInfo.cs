using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类CustomerInfo 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:01 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CustomerInfo : CommonBll
    {
        public static readonly new CustomerInfo Instance = new CustomerInfo();

        protected CustomerInfo()
        {
        }

        /// 通过会员号获取客户信息
        /// <summary>
        /// 通过会员号获取客户信息
        /// </summary>
        /// <param name="membercode"></param>
        /// <returns></returns>
        public Entities.CustomerInfo GetCustomerInfoByMemberCode(string membercode)
        {
            return Dal.CustomerInfo.Instance.GetCustomerInfoByMemberCode(membercode);
        }

        public int CallBackUpdate(Entities.CustomerInfo model)
        {
            return Dal.CustomerInfo.Instance.CallBackUpdate(model);
        }
    }
}

