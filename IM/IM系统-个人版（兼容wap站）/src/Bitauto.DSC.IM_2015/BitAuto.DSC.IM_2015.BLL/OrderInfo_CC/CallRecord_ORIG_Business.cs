using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using BitAuto.DSC.IM_2015.Entities;
using System.Web.Caching;
using BitAuto.Utils;

namespace BitAuto.DSC.IM_2015.BLL
{
    public class CallRecord_ORIG_Business
    {
        #region Instance
        //public static string EPHBuyCarBGID = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCC_HBuyBGID"].ToString();//惠买车BGID
        //public static string EPHBuyCarSCID = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCC_HBuySCID"].ToString();//惠买车SCID
        public static readonly CallRecord_ORIG_Business Instance = new CallRecord_ORIG_Business();
        #endregion

        #region Contructor
        protected CallRecord_ORIG_Business()
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
        public DataTable GetCallRecord_ORIG_Business(QueryCallRecord_ORIG_Business query, string order, int currentPage, int pageSize, out int totalCount)
        {                                            
            return Dal.CallRecord_ORIG_Business.Instance.GetCallRecord_ORIG_Business(query, order, currentPage, pageSize, out totalCount);
        }

        public Entities.CallRecord_ORIG_Business GetByCallID(Int64 CallID)
        {
            return Dal.CallRecord_ORIG_Business.Instance.GetByCallID(CallID);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.CallRecord_ORIG_Business.Instance.GetCallRecord_ORIG_Business(new QueryCallRecord_ORIG_Business(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CallRecord_ORIG_Business GetCallRecord_ORIG_Business(long RecID)
        {

            return Dal.CallRecord_ORIG_Business.Instance.GetCallRecord_ORIG_Business(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(long RecID)
        {
            QueryCallRecord_ORIG_Business query = new QueryCallRecord_ORIG_Business();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCallRecord_ORIG_Business(query, string.Empty, 1, 1, out count);
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
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByCallID(Int64 callid)
        {
            return Dal.CallRecord_ORIG_Business.Instance.IsExistsByCallID(callid);
        }
        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.CallRecord_ORIG_Business model)
        {
            BLL.Loger.Log4Net.Info("[BLL]InsertCallRecord_ORIG_Business ...插入开始...CallID:" + model.CallID);
            return Dal.CallRecord_ORIG_Business.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.CallRecord_ORIG_Business model)
        {
            return Dal.CallRecord_ORIG_Business.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.CallRecord_ORIG_Business model)
        {
            return Dal.CallRecord_ORIG_Business.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.CallRecord_ORIG_Business model)
        {
            return Dal.CallRecord_ORIG_Business.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long RecID)
        {

            return Dal.CallRecord_ORIG_Business.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {

            return Dal.CallRecord_ORIG_Business.Instance.Delete(sqltran, RecID);
        }

        #endregion

        private Random R = new Random();

        public DataTable getListBySourceAndCallID(int source, Int64 callID)
        {
            return Dal.CallRecord_ORIG_Business.Instance.getListBySourceAndCallID(source, callID);
        }

        public string GetTaskUrl(string BGID, string SCID, string Source, string CarType)
        {
            string url = "";

            #region 获取DataTable

            DataTable businessDt = new DataTable();
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            if (objCache["BusinessInfo"] != null)
            {
                businessDt = (DataTable)objCache["BusinessInfo"];
            }
            else
            {
                businessDt = Dal.CallRecord_ORIG_Business.Instance.GetAllURL();
                objCache.Insert("BusinessInfo", businessDt, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
            }
            #endregion

            if (BGID != "" && SCID != "" && businessDt != null && businessDt.Rows.Count > 0)
            {
                string where = "1=1";

                where += " And BGID=" + StringHelper.SqlFilter(BGID);
                where += " And SCID=" + StringHelper.SqlFilter(SCID);

                if (Source != "")
                {
                    where += " And Source='" + StringHelper.SqlFilter(Source) + "'";
                }
                else
                {
                    where += " ANd Source is Null";
                }
                if (CarType != "")
                {
                    where += " And CarType='" + StringHelper.SqlFilter(CarType) + "'";
                }
                else
                {
                    where += " ANd CarType is Null";
                }

                DataRow[] rowList = businessDt.Select(where);
                if (rowList.Length > 0)
                {
                    url = rowList[0]["BusinessDetailURL"].ToString();
                }
                else
                {
                    url = "";
                }
            }
            else
            {
                url = "";
            }

            return url;
        }

        public int AddBusinessUrl(int BGID, int SCID, string webBaseUrl)
        {
            return Dal.CallRecord_ORIG_Business.Instance.AddBusinessUrl(BGID, SCID, webBaseUrl);
        }

        public int DeleteBusinessUrl(int BGID, int SCID)
        {
            return Dal.CallRecord_ORIG_Business.Instance.DeleteBusinessUrl(BGID, SCID);
        }
        /// <summary>
        /// 根据业务组，分类取url
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <returns></returns>
        public DataTable GetBusinessUrl(int BGID, int SCID)
        {
            return Dal.CallRecord_ORIG_Business.Instance.GetBusinessUrl(BGID, SCID);
        }

        /// <summary>
        /// 返回业务查看页面，由于业务里包括易湃的惠买车
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <param name="GetViewUrl"></param>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <param name="CreateUserID"></param>
        /// <returns></returns>
        public string GetViewUrl(string BusinessID, string GetViewUrl, string BGID, string SCID)
        {
            //如果是惠买车
            //if (BGID == EPHBuyCarBGID && SCID == EPHBuyCarSCID)
            //{
            //    GetViewUrl = string.Format(GetViewUrl, CreateUserID, BusinessID);
            //}
            //else
            //{
            GetViewUrl += "&r={1}";
            GetViewUrl = string.Format(GetViewUrl, BusinessID, R.Next(100000));

            //}
            return GetViewUrl;
        }

        /// <summary>
        /// 根据CallID，更新业务数据，businessID为业务ID，BGID为分组ID，SCID为分类ID
        /// </summary>
        /// <param name="Verifycode"></param>
        /// <param name="callID"></param>
        /// <param name="businessID"></param>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <param name="createuserid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int UpdateBusinessDataByCallID(Int64 callID, string businessID, int BGID, int SCID, int createuserid, ref string msg)
        {
            msg = "";
            try
            {
                if (BLL.CallRecord_ORIG_Business.Instance.IsExistsByCallID(callID))
                {
                    BLL.Loger.Log4Net.Info("[BLL.UpdateBusinessDataByCallID]在系统中已经存在业务数据，更新！callID=" + callID);
                    Entities.CallRecord_ORIG_Business model = new Entities.CallRecord_ORIG_Business();
                    model.CallID = callID;
                    model.BGID = BGID;
                    model.SCID = SCID;
                    model.BusinessID = businessID;
                    model.CreateUserID = createuserid;
                    model.CreateTime = DateTime.Now;
                    int recID = BLL.CallRecord_ORIG_Business.Instance.Update(model);
                    BLL.Loger.Log4Net.Info("[BLL.UpdateBusinessDataByCallID]更新完毕：callID=" + callID);
                    return recID;
                }
                else
                {
                    BLL.Loger.Log4Net.Info("[BLL.UpdateBusinessDataByCallID]准备插入数据：callID=" + callID);
                    Entities.CallRecord_ORIG_Business model = new Entities.CallRecord_ORIG_Business();
                    model.CallID = callID;
                    model.BGID = BGID;
                    model.SCID = SCID;
                    model.BusinessID = businessID;
                    model.CreateUserID = createuserid;
                    model.CreateTime = DateTime.Now;
                    int recID = BLL.CallRecord_ORIG_Business.Instance.Insert(model);

                    model.RecID = recID;

                    string descMsg = string.Empty;
                    BLL.GetLogDesc.getAddLogInfo(model, out descMsg);
                    BLL.Loger.Log4Net.Info(descMsg);
                    BLL.Loger.Log4Net.Info("[BLL.UpdateBusinessDataByCallID]插入完毕：callID=" + callID);

                    return recID;

                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BLL.Loger.Log4Net.Info("[BLL.UpdateBusinessDataByCallID]出错！callID=" + callID + ",errorMsg:" + msg);
                return -1;
            }

        }
    }
}
