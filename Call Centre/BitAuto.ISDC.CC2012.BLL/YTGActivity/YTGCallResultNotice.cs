using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class YTGCallResultNotice : CommonBll
    {
        private YTGCallResultNotice() { }
        private static YTGCallResultNotice _instance = null;
        public static new YTGCallResultNotice Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new YTGCallResultNotice();
                }
                return _instance;
            }
        }

        /// 获取需求回传的数据
        /// <summary>
        /// 获取需求回传的数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetNeedCallResultNotice(int attempts)
        {
            return Dal.YTGCallResultNotice.Instance.GetNeedCallResultNotice(attempts);
        }
    }
}
