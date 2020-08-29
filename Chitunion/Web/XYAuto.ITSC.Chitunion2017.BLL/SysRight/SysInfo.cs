using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace XYAuto.ITSC.Chitunion2017.BLL.SysRight
{
    public class SysInfo
    {
        #region Instance
        public static readonly SysInfo Instance = new SysInfo();
        #endregion

        #region Contructor
        protected SysInfo()
        {
        }
        #endregion

        //#region Select
        ///// <summary>
        ///// 按照查询条件查询  分页
        ///// </summary>
        ///// <param name="querySysInfo">查询值对象，用来存放查询条件</param>        
        ///// <param name="currentPage">页号,-1不分页</param>
        ///// <param name="pageSize">每页记录数</param>
        ///// <param name="totalCount">总行数</param>
        ///// <returns>角色资源对应表集合</returns>
        //public DataTable GetSysInfo(QuerySysInfo querySysInfo, int currentPage, int pageSize, out int totalCount)
        //{
        //    return Dal.SysInfo.Instance.GetSysInfo(querySysInfo, currentPage, pageSize, out  totalCount);
        //}
        public DataTable GetSysInfoAll()
        {
            return Dal.SysRight.SysInfo.Instance.GetSysInfoAll();
        }

        //public XYAuto.YanFa.SysRightsManager.Entities.SysInfo GetSysInfo(int id)
        //{
        //    return Dal.SysInfo.Instance.GetSysInfo(id);
        //}
        //public string GetSysNameBySysCode(string sysID)
        //{
        //    QuerySysInfo querySysInfo = new QuerySysInfo();
        //    querySysInfo.SysID = sysID;
        //    DataTable dt = new DataTable();
        //    int count = 0;
        //    dt = GetSysInfo(querySysInfo, 1, int.MaxValue, out count);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        return dt.Rows[0]["SysName"].ToString().Trim();
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        //public bool IsExistBySysName(string sysName)
        //{
        //    QuerySysInfo querySysInfo = new QuerySysInfo();
        //    querySysInfo.ExistSysName = sysName;
        //    querySysInfo.Status = Constant.INT_INVALID_VALUE;
        //    DataTable dt = new DataTable();
        //    int count = 0;
        //    dt = GetSysInfo(querySysInfo, 1, int.MaxValue, out count);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //public DataTable GetSysInfoByMaxRecID()
        //{
        //    return Dal.SysInfo.Instance.GetSysInfoByMaxRecID();
        //}
        //public string GenSysID()
        //{
        //    string sysID = "SYS001";
        //    DataTable dt = new DataTable();
        //    dt = GetSysInfoByMaxRecID();
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        sysID = dt.Rows[0]["SysID"].ToString().Trim();
        //        int id = 0;
        //        int.TryParse(sysID.Substring(3, 3), out id);
        //        id++;
        //        sysID = "SYS" + id.ToString().PadLeft(3, '0');
        //    }
        //    return sysID;
        //}
        //#endregion

        //#region Insert
        ///// <summary>
        ///// 添加详细
        ///// </summary>
        ///// <param name="SysInfo">值对象</param>
        ///// <returns>成功:索引值;失败:-1</returns>
        //public int InsertSysInfo(Entities.SysInfo SysInfo)
        //{
        //    return Dal.SysInfo.Instance.InsertSysInfo(SysInfo);
        //}
        //#endregion

        //#region Updata
        ///// <summary>
        ///// 更新详细
        ///// </summary>
        ///// <param name="SysInfo">值对象</param>
        ///// <returns>成功:1;失败:-1</returns>
        //public int UpdataSysInfo(Entities.SysInfo SysInfo)
        //{
        //    return Dal.SysInfo.Instance.UpdataSysInfo(SysInfo);
        //}
        //#endregion
    }
}
