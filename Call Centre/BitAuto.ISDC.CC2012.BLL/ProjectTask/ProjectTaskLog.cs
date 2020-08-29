using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类ProjectTaskLog 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:33 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectTaskLog
    {
        #region Instance
        public static readonly ProjectTaskLog Instance = new ProjectTaskLog();
        #endregion

        #region Contructor
        protected ProjectTaskLog()
        { }
        #endregion

        public DataTable GetProjectTaskLog(string tid)
        {
            return Dal.ProjectTaskLog.Instance.GetProjectTaskLog(tid);
        }

        public int InsertProjectTaskLog(Entities.ProjectTaskLog model)
        {
            return Dal.ProjectTaskLog.Instance.InsertProjectTaskLog(model);
        }
        public int InserOtherTaskLog(Entities.ProjectTaskLog model)
        {
            return Dal.ProjectTaskLog.Instance.InserOtherTaskLog(model);
        }
    }
}

