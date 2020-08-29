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
    /// 业务逻辑类KnowledgeCategory 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-21 10:19:09 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class KnowledgeCategory
    {
        #region Instance
        public static readonly KnowledgeCategory Instance = new KnowledgeCategory();
        #endregion

        #region Contructor
        protected KnowledgeCategory()
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
        public DataTable GetKnowledgeCategory(QueryKnowledgeCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.KnowledgeCategory.Instance.GetKnowledgeCategory(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetKnowledgeCategoryWithRegion(QueryKnowledgeCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.KnowledgeCategory.Instance.GetKnowledgeCategoryWithRegion(query, BLL.Util.GetLoginUserID(),order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetKnowledgeCategoryForSearch(QueryKnowledgeCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.KnowledgeCategory.Instance.GetKnowledgeCategoryForSearch(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 根据父名称 取得子节点列表
        /// </summary>
        /// <param name="Parentname"></param>
        /// <returns></returns>
        public DataTable GetCategoryByPName(string Parentname, int RegionID)
        {
            return Dal.KnowledgeCategory.Instance.GetCategoryByPName(Parentname,RegionID);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.KnowledgeCategory.Instance.GetKnowledgeCategory(new QueryKnowledgeCategory(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// 取节点下子节点以及该节点名称
        /// </summary>
        /// <param name="KCID"></param>
        /// <param name="Parentname"></param>
        /// <returns></returns>
        public DataTable GetCategoryByPID(int KCID, string Parentname)
        {
            return Dal.KnowledgeCategory.Instance.GetCategoryByPID(KCID, Parentname);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.KnowledgeCategory GetKnowledgeCategory(int KCID)
        {

            return Dal.KnowledgeCategory.Instance.GetKnowledgeCategory(KCID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByKCID(int KCID)
        {
            QueryKnowledgeCategory query = new QueryKnowledgeCategory();
            query.KCID = KCID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKnowledgeCategory(query, string.Empty, 1, 1000000, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 判断某节点是否有子节点
        /// </summary>
        /// <param name="KCID"></param>
        /// <returns></returns>
        public bool IsExistsChildByKCID(int KCID)
        {
            QueryKnowledgeCategory query = new QueryKnowledgeCategory();
            query.Pid = KCID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKnowledgeCategory(query, string.Empty, 1, 1, out count);
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

        /// <summary>
        /// 取某极节点
        /// </summary>
        /// <returns></returns>
        public DataTable GetCategory(int level)
        {
            QueryKnowledgeCategory query = new QueryKnowledgeCategory();
            query.Level = level;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKnowledgeCategory(query, string.Empty, 1, 1000000, out count);
            return dt;
        }
        public DataTable GetCategoryWithRegion(int level)
        {
            QueryKnowledgeCategory query = new QueryKnowledgeCategory();
            query.Level = level;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKnowledgeCategoryWithRegion(query, string.Empty, 1, 1000000, out count);
            return dt;
        }
        /// <summary>
        /// 取某节点下子节点
        /// </summary>
        /// <returns></returns>
        public DataTable GetCategoryByPID(int KCID)
        {
            QueryKnowledgeCategory query = new QueryKnowledgeCategory();
            query.Pid = KCID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKnowledgeCategory(query, string.Empty, 1, 1000000, out count);
            return dt;
        }



        /// <summary>
        /// 修改知识点分类
        /// </summary>
        /// <param name="kcid"></param>
        /// <param name="klid"></param>
        /// <returns></returns>
        public int Update(string kcid, string klid)
        {

            return Dal.KnowledgeCategory.Instance.Update(kcid, klid);
        }

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.KnowledgeCategory model)
        {
            return Dal.KnowledgeCategory.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.KnowledgeCategory model)
        {
            return Dal.KnowledgeCategory.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.KnowledgeCategory model)
        {
            return Dal.KnowledgeCategory.Instance.Update(model);
        }


        /// <summary>
        /// 取节点下子节点，且子节点不存在子节点
        /// </summary>
        /// <param name="KCID"></param>
        /// <returns></returns>
        public DataTable GetCategoryByPIDNotSon(int KCID)
        {
            return Dal.KnowledgeCategory.Instance.GetCategoryByPIDNotSon(KCID);
        }

        /// <summary>
        /// 取节点下子节点，且子节点存在子节点
        /// </summary>
        /// <param name="KCID"></param>
        /// <returns></returns>
        public DataTable GetCategoryByPIDHaveSon(int KCID)
        {
            return Dal.KnowledgeCategory.Instance.GetCategoryByPIDHaveSon(KCID);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.KnowledgeCategory model)
        {
            return Dal.KnowledgeCategory.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int KCID)
        {

            return Dal.KnowledgeCategory.Instance.Delete(KCID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int KCID)
        {

            return Dal.KnowledgeCategory.Instance.Delete(sqltran, KCID);
        }

        #endregion



        /// <summary>
        /// 更新知识库类别状态
        /// </summary>
        /// <param name="RequestKCID">类别id</param>
        /// <param name="msg">输出信息</param>
        public void ChangeKnowledgeCategoryStatus(string RequestKCID, out string msg)
        {
            msg = string.Empty;
            int kcid = 0;
            if (int.TryParse(RequestKCID, out kcid))
            {
                #region  通过实体进行操作
                //Entities.KnowledgeCategory model = GetKnowledgeCategory(kcid);
                //if (model != null)
                //{
                //    if (model.Status == 0)//正在使用
                //    {
                //        model.Status = 1;
                //    }
                //    else if (model.Status == 1) //已经停用
                //    {
                //        model.Status = 0;
                //    }
                //    int result = 0;
                //    try
                //    {
                //        result = Dal.KnowledgeCategory.Instance.UpdateKnowledgeCategoryStatus(model);
                //        if (result > 0)
                //        {
                //            msg = "{msg:'success'}";
                //        }
                //        else
                //        {
                //            msg = "{msg:'操作失败！'}";
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        msg = "{msg:'" + ex.Message + "'}";
                //    }
                //}
                #endregion

                int status = Dal.KnowledgeCategory.Instance.GetKnowledgeCategoryStatusByKCID(kcid);
                if (status != -5)
                {
                    Entities.KnowledgeCategory model = new Entities.KnowledgeCategory();
                    model.KCID = kcid;
                    if (status == 0)//正在使用
                    {
                        model.Status = 1;
                    }
                    else if (status == 1) //已经停用
                    {
                        model.Status = 0;
                    }
                    int result = 0;
                    try
                    {
                        result = Dal.KnowledgeCategory.Instance.UpdateKnowledgeCategoryStatus(model);
                        if (result > 0)
                        {
                            msg = "{msg:'success'}";
                        }
                        else
                        {
                            msg = "{msg:'操作失败！'}";
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = "{msg:'" + ex.Message + "'}";
                    }
                }
                else
                {
                    msg = "{msg:'未找到指定类别！'}";
                }
            }
        }

        /// <summary>
        /// 根据分类id逻辑删除分类（如果该分类已经被使用，则不能删除）
        /// </summary>
        /// <param name="RequestKCID">分类id</param>
        /// <param name="msg">输出信息</param>
        public void DeleteKnowledgeCategory(string RequestKCID, out string msg)
        {
            msg = string.Empty;
            int kcid;
            Entities.KnowledgeCategory model = new Entities.KnowledgeCategory();
            if (int.TryParse(RequestKCID, out kcid))
            {
                model.KCID = kcid;
                int count = Dal.KnowledgeCategory.Instance.GetCountTheKnowledgeCategoryUsed(model);
                if (count > 0)
                {
                    msg = "{msg:'该类别在知识库中有使用，无法删除！'}";
                }
                else
                {
                    Dal.KnowledgeCategory.Instance.DeleteKnowledgeCategory(model, out msg);
                }
            }
            else
            {
                msg = "{msg:'参数类型错误！'}";
            }
        }

        public void DeleteKnowledgeCategoryAndChildren(string RequestKCID, out string msg)
        {
            msg = string.Empty;
            int kcid;
            Entities.KnowledgeCategory model = new Entities.KnowledgeCategory();
            if (int.TryParse(RequestKCID, out kcid))
            {
                model.KCID = kcid;
                string kcids = GetChildrenKnowledgeCategoryKCIDS(RequestKCID);
                int count = Dal.KnowledgeCategory.Instance.GetNotDelStatusNum(kcids);
                if (count > 0)
                {
                    msg = "{msg:'该分类或该分类的子分类在知识库中有使用，无法删除，请尝试分步删除！'}";
                }
                else
                {
                    Dal.KnowledgeCategory.Instance.DeleteKnowledgeCategoryAndChildren(kcids, out msg);
                }
            }
            else
            {
                msg = "{msg:'参数类型错误！'}";
            }
        }
        public string GetChildrenKnowledgeCategoryKCIDS(string RequestKCID)
        {
            DataTable tbAllKC = Dal.KnowledgeCategory.Instance.GetAllKnowledgeCategory();

            string strIds = RequestKCID + "," + GetChildId(RequestKCID, tbAllKC);
            return strIds.Substring(0, strIds.Length - 1);
        }
        public string GetChildId(string KCID, DataTable tbAllKC)
        {
            string strIds = "";
            foreach (DataRow row in tbAllKC.Rows)
            {
                if (row["Pid"].ToString() == KCID)
                {
                    strIds += row["KCID"].ToString() + "," + GetChildId(row["KCID"].ToString(), tbAllKC);
                }
            }
            return strIds;
        }

        /// <summary>
        /// 添加新的分类信息
        /// </summary>
        /// <param name="RequestName">分类名称</param>
        /// <param name="RequestKCID">父类Id</param>
        /// <param name="RequestLevel">分类级别</param>
        /// <param name="msg">输出信息</param>
        public void InsertKnowledgeCategory(string RequestName, string RequestKCID, string RequestLevel, int Regionid, out string msg)
        {
            msg = string.Empty;
            int kcid;
            int level;
            Entities.KnowledgeCategory model = new Entities.KnowledgeCategory();

            model.Name = RequestName;
            if (int.TryParse(RequestKCID, out kcid) && int.TryParse(RequestLevel, out level))
            {
                model.Pid = kcid;
                model.Level = level;
                model.Regionid = Regionid;
                Dal.KnowledgeCategory.Instance.InsertKnowledgeCategory(model, out msg);
            }
            else
            {
                msg = "{msg:'参数类型错误！'}";
            }
        }

        /// <summary>
        /// 根据分类Id更新指定的分类信息
        /// </summary>
        /// <param name="RequestName">分类名称</param>
        /// <param name="RequestKCID">分类Id</param>
        /// <param name="msg"></param>
        public void UpdateKnowledgeCategory(string RequestName, string RequestKCID, out string msg)
        {
            msg = string.Empty;
            int kcid;
            Entities.KnowledgeCategory model = new Entities.KnowledgeCategory();
            model.Name = RequestName;
            if (int.TryParse(RequestKCID, out kcid))
            {
                model.KCID = kcid;
                Dal.KnowledgeCategory.Instance.UpdateKnowledgeCategory(model, out msg);
            }
            else
            {
                msg = "{msg:'参数类型错误！'}";
            }
        }


        /// <summary>
        /// 根据父分类id和级别，获取下级子分类下拉列表信息
        /// </summary>
        /// <param name="RequestLevel">级别</param>
        /// <param name="RequestKCID">父类别Id</param>
        /// <param name="msg"></param>
        public void BindKnowledgeCategory(string RequestLevel, string RequestKCID, int Regionid, out string msg)
        {
            msg = string.Empty;
            int kcid;
            int level;
            if (int.TryParse(RequestKCID, out kcid) && int.TryParse(RequestLevel, out level))
            {
                Entities.QueryKnowledgeCategory query = new Entities.QueryKnowledgeCategory();
                query.Level = level;
                query.Pid = kcid;
                query.Regionid = Regionid;

                DataTable dt = new DataTable(); ;
                dt = Dal.KnowledgeCategory.Instance.BindKnowledgeCategory(query);

                if (dt.Rows.Count > 0)
                {
                    msg += "{root:[";
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    msg += "{name:'" + dt.Rows[i]["Name"].ToString() + "',kcid:'" + dt.Rows[i]["KCID"].ToString() + "'},";
                }
                if (dt.Rows.Count > 0)
                {
                    msg = msg.TrimEnd(',') + "]}";
                }
            }
        }


        /// <summary>
        /// 根据父分类id和级别，获取子分类详细信息
        /// </summary>
        /// <param name="msg"></param>
        public void BindChildrenCategoryInfo(string RequestKCID, string RequestLevel, string RequestName, int Regionid, out string msg)
        {
            msg = string.Empty;
            int kcid;
            int level;

            if (int.TryParse(RequestKCID, out kcid) && int.TryParse(RequestLevel, out level))
            {
                Entities.QueryKnowledgeCategory query = new Entities.QueryKnowledgeCategory();
                query.Level = level;
                query.Pid = kcid;
                query.Name = RequestName;
                query.Regionid = Regionid;

                DataTable dt = new DataTable(); ;

                dt = Dal.KnowledgeCategory.Instance.BindChildrenCategoryInfo(query);

                if (dt.Rows.Count > 0)
                {
                    msg += "{root:[";
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    msg += "{name:'" + dt.Rows[i]["Name"].ToString() + "',kcid:'" + dt.Rows[i]["KCID"].ToString() + "',status:'" + dt.Rows[i]["Status"].ToString() + "',parentName:'" + dt.Rows[i]["parentName"].ToString() + "',sortnum:'" + dt.Rows[i]["SortNum"].ToString() + "'},";
                }
                if (dt.Rows.Count > 0)
                {
                    msg = msg.TrimEnd(',') + "]}";
                }

                #region
                //if (string.IsNullOrEmpty(RequestName))
                //{
                //    if (dt.Rows.Count <= 0)
                //    {
                //        #region  查询数据
                //        string strSelect2 = "SELECT  parentName =  ISNULL((SELECT name FROM KnowledgeCategory WHERE KCID=aa.Pid),'') " +
                //                ",aa.Name,aa.Status,aa.KCID,Pid " +
                //                "FROM    KnowledgeCategory aa " +
                //                "WHERE  aa.Status<>-1 AND aa.kcid='" + kcid + "' AND aa.Level='" + (level - 1) + "' ";
                //        using (SqlConnection conn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStrings_CC"].ToString()))
                //        {
                //            conn2.Open();
                //            SqlDataAdapter adp2 = new SqlDataAdapter(strSelect2, conn2);
                //            adp2.Fill(dt);
                //        }
                //        #endregion

                //        if (dt.Rows.Count > 0)
                //        {
                //            msg += "{root:[";
                //        }
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            msg += "{name:'" + dt.Rows[i]["Name"].ToString() + "',kcid:'" + dt.Rows[i]["KCID"].ToString() + "',status:'" + dt.Rows[i]["Status"].ToString() + "',parentName:'" + dt.Rows[i]["parentName"].ToString() + "'},";
                //        }
                //        if (dt.Rows.Count > 0)
                //        {
                //            msg = msg.TrimEnd(',') + "]}";
                //        }
                //    }
                //}
                #endregion
            }
        }

        public void SortNumUpOrDown(string sortid, string type, string info, out string msg)
        {
            Dal.KnowledgeCategory.Instance.SortNumUpOrDown(sortid, type, info, out  msg);
        }

    }
}

