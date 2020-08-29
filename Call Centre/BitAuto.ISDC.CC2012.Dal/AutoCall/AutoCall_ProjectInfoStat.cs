using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类AutoCall_ProjectInfoStat。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2015-09-14 09:57:59 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class AutoCall_ProjectInfoStat : DataBase
    {
        public static readonly AutoCall_ProjectInfoStat Instance = new AutoCall_ProjectInfoStat();
    }
}

