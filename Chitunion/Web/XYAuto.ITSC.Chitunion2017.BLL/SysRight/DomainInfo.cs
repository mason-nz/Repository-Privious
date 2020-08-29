using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace XYAuto.ITSC.Chitunion2017.BLL.SysRight
{
    public class DomainInfo
    {
        #region Instance
        public static readonly DomainInfo Instance = new DomainInfo();
        #endregion

        #region Contructor
        protected DomainInfo()
        {
        }
        #endregion

        #region Select
        ///// <summary>
        ///// 按照查询条件查询  分页
        ///// </summary>
        ///// <param name="queryDomainInfo">查询值对象，用来存放查询条件</param>        
        ///// <param name="currentPage">页号,-1不分页</param>
        ///// <param name="pageSize">每页记录数</param>
        ///// <param name="totalCount">总行数</param>
        ///// <returns>角色资源对应表集合</returns>
        //public DataTable GetDomainInfo(QueryDomainInfo queryDomainInfo, int currentPage, int pageSize, out int totalCount)
        //{
        //    return Dal.DomainInfo.Instance.GetDomainInfo(queryDomainInfo, currentPage, pageSize, out  totalCount);
        //}

        /// <summary>
        /// 获取某系统的 系统域名
        /// </summary>
        /// <param name="sysID"></param>
        /// <returns></returns>
        public DataTable GetDomainInfoBySysID(string sysID)
        {
            return Dal.SysRight.DomainInfo.Instance.GetDomainInfoBySysID(sysID);
        }
        #endregion

        //#region Insert
        ///// <summary>
        ///// 添加详细
        ///// </summary>
        ///// <param name="DomainInfo">值对象</param>
        ///// <returns>成功:索引值;失败:-1</returns>
        //public int InsertDomainInfo(Entities.DomainInfo DomainInfo)
        //{
        //    return Dal.DomainInfo.Instance.InsertDomainInfo(DomainInfo);
        //}
        //#endregion

        //#region Update
        ///// <summary>
        ///// 更新详细
        ///// </summary>
        ///// <param name="DomainInfo">值对象</param>
        ///// <returns>成功:1;失败:-1</returns>
        //public int UpdateDomainInfo(Entities.DomainInfo DomainInfo)
        //{
        //    return Dal.DomainInfo.Instance.UpdateDomainInfo(DomainInfo);
        //}
        //#endregion

        //#region delete
        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="redID">redID</param>
        ///// <returns>成功:索引值;失败:-1</returns>
        //public int DeleteDomainInfo(int redID)
        //{
        //    return Dal.DomainInfo.Instance.DeleteDomainInfo(redID);
        //}

        //#endregion

        //#region SelectByID
        ///// <summary>
        ///// 按照ID查询符合条件的一条记录
        ///// </summary>
        ///// <param name="id">索引ID</param>
        ///// <returns>符合条件的一个值对象</returns>
        //public Entities.DomainInfo GetDomainInfo(int id)
        //{
        //    return Dal.DomainInfo.Instance.GetDomainInfo(id);
        //}


        //#endregion
    }
}
