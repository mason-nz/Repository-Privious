using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Data.SqlClient;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class ProjectLog
    {
        public static ProjectLog Instance = new ProjectLog();

        /// 查询项目日志
        /// <summary>
        /// 查询项目日志
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public DataTable GetProjectLog(long projectid, int pageindex, int pagesize, out int total)
        {
            return Dal.ProjectLog.Instance.GetProjectLog(projectid, pageindex, pagesize, out total);
        }

        /// 插入日志
        /// <summary>
        /// 插入日志
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="oper"></param>
        /// <param name="remark"></param>
        public void InsertProjectLog(long projectid, ProjectLogOper oper, string remark, SqlTransaction sqltran = null, int? sysuserid = null)
        {
            //不能因为写日志异常引起系统崩溃
            try
            {
                int userid = 0;
                if (sysuserid.HasValue)
                {
                    userid = sysuserid.Value;
                }
                else
                {
                    userid = BLL.Util.GetLoginUserID();
                }
                ProjectLogInfo info = new ProjectLogInfo();
                info.ProjectID = projectid;
                info.OperName = BLL.Util.GetEnumOptText(typeof(ProjectLogOper), (int)oper);
                info.Remark = remark.TrimEnd(new char[] { ';', '；' });
                info.CreateUserID = userid;
                info.CreateTime = DateTime.Now;

                if (sqltran != null)
                    CommonBll.Instance.InsertComAdoInfo(sqltran, info);
                else
                    CommonBll.Instance.InsertComAdoInfo(info);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(ex.Message);
                BLL.Loger.Log4Net.Info(ex.StackTrace);
            }

        }
    }
}
