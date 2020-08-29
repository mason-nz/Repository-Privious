using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CC_MagazineReturn
    {
        #region Instance
        public static readonly CC_MagazineReturn Instance = new CC_MagazineReturn();
        #endregion

        #region Contructor
        protected CC_MagazineReturn()
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
        public DataTable GetCC_MagazineReturn(QueryCC_MagazineReturn query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CC_MagazineReturn.Instance.GetCC_MagazineReturn(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.CC_MagazineReturn.Instance.GetCC_MagazineReturn(new QueryCC_MagazineReturn(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// 获取所有标记信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDistinctTitle()
        {
            return Dal.CC_MagazineReturn.Instance.GetDistinctTitle();
        }
        #endregion


        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CC_MagazineReturn GetCC_MagazineReturn(int RecID)
        {

            return Dal.CC_MagazineReturn.Instance.GetCC_MagazineReturn(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryCC_MagazineReturn query = new QueryCC_MagazineReturn();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCC_MagazineReturn(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.CC_MagazineReturn model)
        {
            return Dal.CC_MagazineReturn.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.CC_MagazineReturn model)
        {
            return Dal.CC_MagazineReturn.Instance.Update(model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.CC_MagazineReturn.Instance.Delete(RecID);
        }

        #endregion

    }
}
