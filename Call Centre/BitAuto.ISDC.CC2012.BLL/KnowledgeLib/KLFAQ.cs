using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using NPOI.SS.Formula.Functions;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类KLFAQ 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-21 10:19:08 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class KLFAQ
    {
        #region Instance
        public static readonly KLFAQ Instance = new KLFAQ();
        #endregion

        #region Contructor
        protected KLFAQ()
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
        public DataTable GetKLFAQ(QueryKLFAQ query, string order, int currentPage, int pageSize, out int totalCount)
        {
            //知识点分类处理

            return Dal.KLFAQ.Instance.GetKLFAQ(query, order, currentPage, pageSize, out totalCount);
        }
        public DataTable GetKLFAQ(QueryKnowledgeLib query, string order, int currentPage, int pageSize, string wherePlug, out int totalCount)
        {
            return Dal.KLFAQ.Instance.GetKLFAQForManage(query, order, currentPage, pageSize, wherePlug, out totalCount);
        }


        public DataSet GetKLFAQReport(int UserId, string order, string where, int currentPage, int pageSize)
        {
            return Dal.KLFAQ.Instance.GetKLFAQReport(UserId, order, where, currentPage, pageSize);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.KLFAQ.Instance.GetKLFAQ(new QueryKLFAQ(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region 判断一个知识点下是否
        /// <summary>
        /// 判断一个知识点下是否有FAQ
        /// </summary>
        /// <param name="knoledgeID">知识点ID</param>
        /// <returns></returns>
        public bool IsHaveFAQ(string knoledgeID)
        {
            if (knoledgeID == "" || knoledgeID == null)
            {
                return false;
            }
            return Dal.KLFAQ.Instance.IsHaveFAQ(knoledgeID);
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.KLFAQ GetKLFAQ(long KLFAQID)
        {

            return Dal.KLFAQ.Instance.GetKLFAQ(KLFAQID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByKLFAQID(long KLFAQID)
        {
            QueryKLFAQ query = new QueryKLFAQ();
            query.KLFAQID = KLFAQID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKLFAQ(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.KLFAQ model)
        {
            return Dal.KLFAQ.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.KLFAQ model)
        {
            return Dal.KLFAQ.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.KLFAQ model)
        {
            return Dal.KLFAQ.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.KLFAQ model)
        {
            return Dal.KLFAQ.Instance.Update(sqltran, model);
        }

        public bool UpdateFAQs(SqlTransaction sqltran, List<Entities.KLFAQ> FAQList, long KLID)
        {
            string msg = "";
            try
            {
                foreach (Entities.KLFAQ FAQ in FAQList)
                {
                    FAQ.ModifyTime = DateTime.Now;
                    FAQ.ModifyUserID = Util.GetLoginUserID();
                    FAQ.KLID = KLID;
                    if (FAQ.KLFAQID == 0)
                    {//新增
                        FAQ.CreateTime = DateTime.Now;
                        FAQ.CreateUserID = Util.GetLoginUserID();
                        Insert(FAQ);
                        msg += "添加FAQ，问题：" + FAQ.Ask + ",答案：" + FAQ.Question + "；";
                    }
                    else
                    {//修改
                        Entities.KLFAQ FAQOri = new Entities.KLFAQ();
                        FAQOri = GetKLFAQ(FAQ.KLFAQID);
                        if (FAQ.Ask != FAQOri.Ask || FAQ.Question != FAQOri.Question)
                        {
                            msg += " 修改FAQ";
                            if (FAQ.Ask != FAQOri.Ask)
                            {
                                msg += ",问题由“" + FAQOri.Ask + "”改为“" + FAQ.Ask + "”";
                            }
                            else
                            {
                                msg += ",答案由“" + FAQOri.Question + "”改为“" + FAQ.Question + "”";
                            }
                            msg += ";";
                            Update(FAQ);
                        }
                    }
                }
                //写入日志
                BLL.Util.InsertUserLog(msg);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Delete

        public int DeleteByKLID(SqlTransaction sqltran, long KLID)
        {
            return Dal.KLFAQ.Instance.DeleteByKLID(sqltran, KLID);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long KLFAQID)
        {
            return Dal.KLFAQ.Instance.Delete(KLFAQID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long KLFAQID)
        {
            return Dal.KLFAQ.Instance.Delete(sqltran, KLFAQID);
        }

        public bool Delete(SqlTransaction sqltran, string KLFAQIDs, out string msg)
        {
            KLFAQIDs = removeLastComma(KLFAQIDs);
            string[] FAQIDArr = KLFAQIDs.Split(',');

            try
            {
                msg = "";
                foreach (string FAQID in FAQIDArr)
                {
                    long faqID = long.Parse(FAQID);
                    Entities.KLFAQ FAQ = new Entities.KLFAQ();
                    FAQ = this.GetKLFAQ(faqID);
                    if (FAQ != null)
                    {
                        msg += "删除FAQ成功，ID：" + FAQID + ",问题：" + FAQ.Ask + "答案：" + FAQ.Question;
                        Delete(sqltran, faqID);
                    }
                }
                //写入日志
                BLL.Util.InsertUserLog(msg);
                return true;
            }
            catch
            {
                msg = "FAQ删除失败！";
                return false;
            }
        }
        #region 智能去除最后一个逗号
        /// <summary>
        /// 智能去除最后一个逗号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string removeLastComma(string str)
        {
            int strLength = str.Length;
            if (str == null || str == "")
            {
                return "";
            }
            else
            {
                if (str.Substring(strLength - 1, 1) == ",")
                {
                    return str.Substring(0, strLength - 1);
                }
                else
                {
                    return str;
                }
            }
        }
        #endregion
        #endregion

    }
}

