using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class ProjectTask_BusinessScale
    {
        #region Instance
        public static readonly ProjectTask_BusinessScale Instance = new ProjectTask_BusinessScale();
        #endregion

        #region Contructor
        protected ProjectTask_BusinessScale()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetProjectTask_BusinessScale(QueryProjectTask_BusinessScale query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_BusinessScale.Instance.GetProjectTask_BusinessScale(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ProjectTask_BusinessScale.Instance.GetProjectTask_BusinessScale(new QueryProjectTask_BusinessScale(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// 根据任务ID获取二手车规模(包括删除的)
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public IList<Entities.ProjectTask_BusinessScale> GetAllProjectTask_BusinessScaleByTID(string tId)
        {
            return Dal.ProjectTask_BusinessScale.Instance.GetAllProjectTask_BusinessScaleByTID(tId);
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_BusinessScale GetProjectTask_BusinessScale(int RecID)
        {

            return Dal.ProjectTask_BusinessScale.Instance.GetProjectTask_BusinessScale(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryProjectTask_BusinessScale query = new QueryProjectTask_BusinessScale();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_BusinessScale(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.ProjectTask_BusinessScale model)
        {
            return Dal.ProjectTask_BusinessScale.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ProjectTask_BusinessScale model)
        {
            return Dal.ProjectTask_BusinessScale.Instance.Update(model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.ProjectTask_BusinessScale.Instance.Delete(RecID);
        }

        #endregion
    }
}
