using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using XYAuto.Utils;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;
using XYAuto.ITSC.Chitunion2017.BLL.ShoppingCart.Dto;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class CartInfo
    {
        #region Instance
        public static readonly CartInfo Instance = new CartInfo();
        #endregion

        #region Contructor
        protected CartInfo()
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
        public DataTable GetCartInfo(QueryCartInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CartInfo.Instance.GetCartInfo(query, order, currentPage, pageSize, out totalCount);
        }              
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.CartInfo.Instance.GetCartInfo(new QueryCartInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 根据主键Recid得到一个对象实体
        /// </summary>
        public Entities.CartInfo GetCartInfo(int recid)
        {

            return Dal.CartInfo.Instance.GetCartInfo(recid);
        }

        #endregion      

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int  Insert(Entities.CartInfo model)
        {           
            return Dal.CartInfo.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertCartInfo(Entities.CartInfo model)
        {
            return Dal.CartInfo.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.CartInfo model)
        {
            return Dal.CartInfo.Instance.Update(model);
        }      

        #endregion
        
        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int recid)
        {
            return Dal.CartInfo.Instance.Delete(recid);
        }
        #endregion        

        #region 将DataRow转成实体
        /// <summary>
        /// 将DataRow转成实体
        /// </summary>
        public Entities.CartInfo DataRowToModel(DataRow row)
        {
            return Dal.CartInfo.Instance.DataRowToModel(row);
        }
        #endregion        

        #region 根据媒体类型、创建人、广告位查看记录是否存在
        public bool IsExistsDetailID(int mediaType, int userID, int detailID)
        {
            return Dal.CartInfo.Instance.IsExistsDetailID(mediaType, userID, detailID);
        }
        #endregion

        #region 根据媒体类型、创建人、媒体ID查看记录是否存在
        public bool IsExistsMediaID(int mediaType, int userID, int mediaID)
        {
            return Dal.CartInfo.Instance.IsExistsMediaID(mediaType, userID, mediaID);
        }
        #endregion

        #region 根据媒体类型、创建人、广告位ID删除记录
        /// <summary>
        /// 根据媒体类型、创建人、广告位ID删除记录
        /// </summary>
        /// <param name="mediatype">媒体类型</param>
        /// <param name="userid">当前登录人ID</param>
        /// <param name="ids">广告位ID拼接字符串</param>
        /// <returns></returns>
        public int Delete_APPMedia(int mediatype, int userid, string ids)
        {
            return Dal.CartInfo.Instance.Delete_APPMedia(mediatype, userid, ids);
        }
        #endregion

        #region 根据媒体类型、创建人、媒体ID删除记录
        /// <summary>
        /// 根据媒体类型、创建人、媒体ID删除记录
        /// </summary>
        /// <param name="mediatype">媒体类型</param>
        /// <param name="userid">当前登录人ID</param>
        /// <param name="ids">媒体ID拼接字符串</param>
        /// <returns></returns>
        public int Delete_SelfMedia(int mediatype, int userid, string ids)
        {
            return Dal.CartInfo.Instance.Delete_SelfMedia(mediatype, userid, ids);
        }
        #endregion

        #region 根据媒体类型、创建人清空购物车
        /// <summary>
        /// 根据媒体类型、创建人清空购物车
        /// </summary>
        /// <param name="mediatype">媒体类型</param>
        /// <param name="userid">创建人</param>
        /// <returns></returns>
        public int ClearAll_CartInfo(int mediatype, int userid)
        {
            return Dal.CartInfo.Instance.ClearAll_CartInfo(mediatype, userid);
        }
        #endregion

        #region 自媒体更新广告位
        /// <summary>
        /// 自媒体更新广告位
        /// </summary>
        /// <param name="pubdetailid">广告位ID</param>
        /// <param name="mediaid">媒体ID</param>
        /// <param name="userid">创建人</param>
        /// <returns></returns>
        public int UpdatePubDetailID_CartInfo(int pubdetailid, int mediaid, int userid)
        {
            return Dal.CartInfo.Instance.UpdatePubDetailID_CartInfo(pubdetailid, mediaid, userid);
        }
        #endregion

        #region 根据媒体类型、创建人清空购物车(未选择的不清)
        /// <summary>
        /// 根据媒体类型、创建人清空购物车(未选择的不清)
        /// </summary>
        /// <param name="mediatype">媒体类型</param>
        /// <param name="userid">创建人</param>
        /// <param name="isAll">是否全清</param>
        /// <returns></returns>
        public int ClearAll_CartInfo(int mediatype, int userid, string ids)
        {
            return Dal.CartInfo.Instance.ClearAll_CartInfo(mediatype, userid, ids);
        }
        #endregion

        #region 根据媒体类型、创建人获取购物车信息
        /// <summary>
        /// 根据媒体类型、创建人获取购物车信息
        /// </summary>
        /// <param name="mediatype">媒体类型</param>
        /// <param name="userid">创建人UserID</param>
        /// <returns></returns>
        public DataTable GetCartInfo_MediaTypeUserID(int mediatype, int userid)
        {
            return Dal.CartInfo.Instance.GetCartInfo_MediaTypeUserID(mediatype, userid);
        }
        #endregion

        #region 添加购物V1.1 Insert                
        public string InsertV1_1(Entities.CartInfo model, out int CartID)
        {
            return Dal.CartInfo.Instance.InsertV1_1(model,out CartID);
        }
        #endregion
        #region 删除购物V1.1 DeleteV1_1                
        public string DeleteV1_1(int currentUserID, string CartIDS)
        {
            return Dal.CartInfo.Instance.DeleteV1_1(currentUserID, CartIDS);
        }
        #endregion
        #region 购物车排期(新增修改删除)              
        public string CartScheduleInfo_Oper(int OptType, int CartID, int RecID, DateTime BeginTime, int CreateUserID, out int RecIDNew)
        {
            return Dal.CartInfo.Instance.CartScheduleInfo_Oper(OptType, CartID, RecID, BeginTime, CreateUserID,out RecIDNew);

        }
        #endregion
        #region 购物车查询V1.1              
        public DataTable ADCartInfoDetail_SelectV1_1(int MediaType, int CreateUserID)
        {
            return Dal.CartInfo.Instance.ADCartInfoDetail_SelectV1_1(MediaType, CreateUserID);

        }
        public List<ResponseQuerySelfItemDto> ADCartInfoDetailWeChat_Select(int CreateUserID)
        {
            return Util.DataTableToList<ResponseQuerySelfItemDto>(Dal.CartInfo.Instance.ADCartInfoDetail_SelectV1_1(14001, CreateUserID));

        }
        #endregion
        #region 购物车排期查询V1.1              
        public DataTable ADCartScheduleInfo_Select(int CartID)
        {
            return Dal.CartInfo.Instance.ADCartScheduleInfo_Select(CartID);

        }
        public List<T> ADCartScheduleInfoWeChat_Select<T>(int CartID)
        {
            return Util.DataTableToList<T>(Dal.CartInfo.Instance.ADCartScheduleInfo_Select(CartID));

        }
        #endregion
        #region 购物车AE待审项目查询V1.1              
        public DataTable OrderIDName_Select(int MediaType, int AEUserID, out string Msg)
        {
            return Dal.CartInfo.Instance.OrderIDName_Select(MediaType, AEUserID, out Msg);

        }
        #endregion
        #region 购物车中当前选择的广告位在项目中是否已存在V1.1                
        public string PubDetailVertify_ADOrderOrCart(string OrderID, string PublishDetailIDs)
        {
            return Dal.CartInfo.Instance.PubDetailVertify_ADOrderOrCart(OrderID, PublishDetailIDs);
        }
        #endregion
        #region 根据媒体类型创建人清空购物车(未选择的不清)V1.1       
        public int ClearAll_CartInfoV1_1(int mediatype, int userid, string ids)
        {
            return Dal.CartInfo.Instance.ClearAll_CartInfoV1_1(mediatype, userid, ids);
        }
        #endregion

        #region UpdateV1.1.4
        public void UpdateV1_4(Entities.CartInfo model)
        {
            Dal.CartInfo.Instance.UpdateV1_4(model);
        }
        #endregion
        #region 购物车AE待审项目查询V1.4
        public void OrderIDName_SelectV1_4(int AEUserID, out string Msg, out List<Dictionary<string, object>> dicList)
        {
            Dal.CartInfo.Instance.OrderIDName_SelectV1_4(AEUserID, out Msg, out dicList);
        }
        #endregion
        #region APP添加购物V1.1.4 Insert                
        public string InsertAPP(Entities.CartInfo model, out int CartID)
        {
            return Dal.CartInfo.Instance.InsertAPP(model, out CartID);
        }
        #endregion
        #region APP购物车排期(新增修改删除)              
        public string CartScheduleInfoAPP_Oper(int OptType, int CartID, int RecID, DateTime BeginTime, DateTime EndTime, int CreateUserID, out int RecIDNew)
        {
            return Dal.CartInfo.Instance.CartScheduleInfoAPP_Oper(OptType, CartID, RecID, BeginTime, EndTime, CreateUserID, out RecIDNew);
        }
        #endregion
        #region 购物车中当前选择的广告位在项目中是否已存在V1.1.4               
        public string p_PubDetailVertify_ADOrderOrCart(string orderID, int cartID, int mediaType, int pubDetailID, int saleAreaID)
        {
            return Dal.CartInfo.Instance.p_PubDetailVertify_ADOrderOrCart(orderID, cartID, mediaType, pubDetailID, saleAreaID);
        }
        #endregion
        #region APP购物车查询V1.1.4              
        public List<ResponseQueryAPPItemDto> p_ADCartInfoDetailAPP_Select(int CreateUserID)
        {
            return Util.DataTableToList<ResponseQueryAPPItemDto>(Dal.CartInfo.Instance.p_ADCartInfoDetailAPP_Select(14002, CreateUserID));

        }
        #endregion
        #region 查询一段时间范围内的节假是天数及详情V1.4
        public int p_CALHolidays<T>(DateTime beginDate, DateTime endDate, out List<T> listHoliday)
        {
            DataTable dtHoliday;
            int retval = Dal.CartInfo.Instance.p_CALHolidays(beginDate, endDate, out dtHoliday);
            listHoliday = Util.DataTableToList<T>(dtHoliday);
            return retval;
        }
        #endregion
        #region 购物车投放V1.1.4                
        public string Delivery(Entities.CartInfo model)
        {
            return Dal.CartInfo.Instance.Delivery(model);
        }
        #endregion

        #region V1.1.8
        #region 购物车中当前选择的广告位在项目中是否已存在             
        public string p_PubDetailVertify_ADOrderOrCartV1_1_8(string orderID, int cartID, int mediaType, int pubDetailID, int saleAreaID)
        {
            return Dal.CartInfo.Instance.p_PubDetailVertify_ADOrderOrCartV1_1_8(orderID, cartID, mediaType, pubDetailID, saleAreaID);
        }
        #endregion
        public string InsertAPPV1_1_8(Entities.CartInfo model, out int CartID)
        {
            return Dal.CartInfo.Instance.InsertAPPV1_1_8(model, out CartID);
        }
        public string InsertV1_1_8(Entities.CartInfo model, out int CartID)
        {
            return Dal.CartInfo.Instance.InsertV1_1_8(model, out CartID);
        }
        public string CartScheduleInfo_OperV1_1_8(int OptType, int CartID, int RecID, DateTime BeginTime, int CreateUserID, out int RecIDNew)
        {
            return Dal.CartInfo.Instance.CartScheduleInfo_OperV1_1_8(OptType, CartID, RecID, BeginTime, CreateUserID, out RecIDNew);
        }
        public string CartScheduleInfoAPP_OperV1_1_8(int OptType, int CartID, int RecID, DateTime BeginTime, DateTime EndTime, int CreateUserID, out int RecIDNew)
        {
            return Dal.CartInfo.Instance.CartScheduleInfoAPP_OperV1_1_8(OptType, CartID, RecID, BeginTime, EndTime, CreateUserID, out RecIDNew);
        }
        #region 购物车AE待审项目查询
        public void OrderIDName_SelectV1_1_8(int AEUserID, out string Msg, out List<Dictionary<string, object>> dicList)
        {
            Dal.CartInfo.Instance.OrderIDName_SelectV1_1_8(AEUserID, out Msg, out dicList);
        }
        #endregion
        #endregion
    }
}
