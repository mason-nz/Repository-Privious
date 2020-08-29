using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类STDATA_USER_NOAGG 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// lihf
    /// </author>
    /// <history>
    /// 2013-08-13 11:08:09 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class STDATA_USER_NOAGG
    {
        public STDATA_USER_NOAGG()
        {
            _User_Name = Constant.STRING_EMPTY_VALUE;
            _TTimeAgLoggedOn = Constant.INT_INVALID_VALUE;
            _BEGIN_TIME = Constant.DATE_INVALID_VALUE;
        }

        private string _User_Name;
        public string User_Name
        {
            set { _User_Name = value; }
            get { return _User_Name; }
        }

        private int _TTimeAgLoggedOn;
        public int TTimeAgLoggedOn
        {
            set { _TTimeAgLoggedOn = value; }
            get { return _TTimeAgLoggedOn; }
        }

        private DateTime _BEGIN_TIME;
        public DateTime BEGIN_TIME
        {
            set { _BEGIN_TIME = value; }
            get { return _BEGIN_TIME; }
        }
        
    }
}
