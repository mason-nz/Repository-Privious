using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace BitAuto.ISDC.CC2012.WebService
{
    public class CRMCytMemberHelper
    {
        private string CRMCytMemberURL = System.Configuration.ConfigurationManager.AppSettings["CRMSalseTypeURL"].ToString();//服务URL

        #region Instance
        public static readonly CRMCytMemberHelper Instance = new CRMCytMemberHelper();
        #endregion

        /// <summary>
        /// 根据经销商编号字符串数组,返回经销商,和销售类型List
        /// </summary>
        /// <param name="MemberCode">经销商编号</param>
        /// <returns>返回DataSet类表</returns>
        public List<DictionaryEntry> GetMemberContactByMemberCode(string[] MemberCodeArray)
        {
            return (List<DictionaryEntry>)WebServiceHelper.InvokeWebService(CRMCytMemberURL, "GetMemberTypeByMemCode", new object[] { MemberCodeArray });
        }
    }
}
