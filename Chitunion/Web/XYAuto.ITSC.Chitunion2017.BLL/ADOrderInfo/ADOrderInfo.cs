using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using XYAuto.Utils;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Dal.SysRight;
using XYAuto.ITSC.Chitunion2017.BLL.ADOrderInfoDto.Dto;
using System.Linq;
using XYAuto.ITSC.Chitunion2017.BLL.ShoppingCart.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using Spire.Doc;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class ADOrderInfo
    {
        private int _currentUserID;
        public int currentUserID
        {
            get
            {
                try
                {
                    _currentUserID = Chitunion2017.Common.UserInfo.GetLoginUserID();
                }
                catch (Exception)
                {
                    _currentUserID = 1225;
                }
                return _currentUserID;
            }

        }
        string LoginPwdKey = Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");
        private const string ReportTemplate = "/UploadFiles/ReportTemplate/";
        private readonly string _uploadFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);
        #region Instance
        public static readonly ADOrderInfo Instance = new ADOrderInfo();
        #endregion

        #region Contructor
        public ADOrderInfo()
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
        public DataTable GetADOrderInfo(QueryADOrderInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ADOrderInfo.Instance.GetADOrderInfo(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ADOrderInfo.Instance.GetADOrderInfo(new QueryADOrderInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #region 张立彬
        /// <summary>
        /// 2017-02-24 张立彬
        /// 根据条件查询订单信息
        /// </summary>
        /// <param name="orderType">订单类型（0:主订单；1：子订单）</param>
        /// <param name="orderNum">	订单编号（精确查找）</param>
        /// <param name="demandDescribe">需求名称（模糊查询</param>
        /// <param name="mediaType">资源类型 （全部为0）</param>
        /// <param name="creater">创建人(模糊查询）</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="PageIndex">第几页</param>
        /// <param name="orderState">订单状态</param>
        /// <param name="CustomerId">  广告主ID（精确查找）</param>
        /// <param name="OrderSource"> 项目类型(0全部 1代客下单 2自助下单) </param>
        ///  <param name="IsCRM"> 是否关联CRM(0全部 1关联CRM 2否 不关联CRM) </param>
        ///  <param name="SubOrderNum"> 子订单号 </param>
        /// <returns></returns>
        public Dictionary<string, object> SelectOrderInfo(int orderType, string orderNum, string demandDescribe, int mediaType, string creater, int pagesize, int PageIndex, int orderState, string CustomerId, int OrderSource, int IsCRM, string SubOrderNum)
        {

            if (pagesize > Util.PageSize)
            {
                pagesize = Util.PageSize;
            }
            int customerId = 0;
            string LoginPwdKey = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");
            if (!string.IsNullOrWhiteSpace(CustomerId))
            {
                string tmp1 = System.Web.HttpUtility.UrlDecode(CustomerId, Encoding.UTF8);
                customerId = Convert.ToInt32(XYAuto.Utils.Security.DESEncryptor.Decrypt(tmp1, LoginPwdKey));
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("TotalCount", 0);

            List<OrderInfo> listInfo = new List<OrderInfo>();
            string strWhere = string.Empty;
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            int userId = Common.UserInfo.GetLoginUserID();
            string TalbeName = "";
            if (orderType == 0)
            {
                TalbeName = "O";
            }
            else
            {
                TalbeName = "S";
            }
            if (roleIdList.Contains("SYS001RL00004") || roleIdList.Contains("SYS001RL00001") || roleIdList.Contains("SYS001RL00005"))
            {
                strWhere = "";
            }
            //else if (roleIdList.Contains("SYS001RL00005"))
            //{

            //    string userIdList = UserDataPower.GetUseridListByUserID(userId, orderState);
            //    strWhere = " and (" + TalbeName + ".CreateUserID in " + userIdList;
            //    if (orderType != 0)
            //    {
            //        string strSelectMediaID = "";
            //        strSelectMediaID = GetOrderWhere(mediaType, userId);
            //        strWhere += " or " + strSelectMediaID;
            //    }
            //    strWhere += ")";
            //}

            else if (roleIdList.Contains("SYS001RL00002"))
            {
                strWhere = " and (" + TalbeName + ".CreateUserID=" + userId + " or O.CustomerID=" + userId + ")";
            }
            else if (roleIdList.Contains("SYS001RL00003"))
            {

                if (orderType == 0)
                {
                    strWhere = " and 1=2";
                }
                else
                {

                    string strSelectMediaID = GetOrderWhere(mediaType, userId);
                    strWhere = " and (S.CreateUserID=" + userId + " or " + strSelectMediaID + ")";
                }
            }
            else if (roleIdList.Contains("SYS001RL00008"))
            {
                strWhere = " and O.CreateUserID=" + userId;
            }
            else
            {
                strWhere = " and 1=2";
            }

            DataTable dt = Dal.ADOrderInfo.Instance.SelectOrderInfo(orderType, orderNum, demandDescribe, mediaType, creater, pagesize, PageIndex, orderState, strWhere, userId, customerId, OrderSource, IsCRM, SubOrderNum);
            if (dt != null && dt.Rows.Count > 0)
            {
                dic["TotalCount"] = dt.Columns["TotalCount"].Expression;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    OrderInfo order = new OrderInfo();
                    order.OrderNum = dt.Rows[i]["OrderID"] == DBNull.Value ? "" : dt.Rows[i]["OrderID"].ToString();
                    order.OrderName = dt.Rows[i]["OrderName"] == DBNull.Value ? "" : dt.Rows[i]["OrderName"].ToString();
                    order.OrderMoney = dt.Rows[i]["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[i]["TotalAmount"]);
                    order.MediaType = dt.Rows[i]["MediaType"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["MediaType"]);
                    order.MediaName = dt.Rows[i]["MediaName"] == DBNull.Value ? "" : dt.Rows[i]["MediaName"].ToString();
                    order.OrderState = dt.Rows[i]["OrderState"] == DBNull.Value ? "" : dt.Rows[i]["OrderState"].ToString();
                    order.OrderSource = dt.Rows[i]["OrderSource"] == DBNull.Value ? "" : dt.Rows[i]["OrderSource"].ToString();
                    order.CreateTime = dt.Rows[i]["CreateTime"].ToString();
                    order.Creater = dt.Rows[i]["UserName"] == DBNull.Value ? "" : dt.Rows[i]["UserName"].ToString();
                    order.CustomerName = dt.Rows[i]["CustomerName"] == DBNull.Value ? "" : dt.Rows[i]["CustomerName"].ToString();
                    order.MediaAccount = dt.Rows[i]["MediaAccount"] == DBNull.Value ? "" : dt.Rows[i]["MediaAccount"].ToString();
                    order.CrmNum = dt.Rows[i]["CrmNum"] == DBNull.Value ? "" : dt.Rows[i]["CrmNum"].ToString();
                    order.CostReferPrice = dt.Rows[i]["CostTotal"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["CostTotal"]);
                    if (roleIdList.Contains("SYS001RL00004") || roleIdList.Contains("SYS001RL00001"))
                    {

                        order.OrderCreatSource = "1";
                    }
                    else
                    {
                        order.OrderCreatSource = dt.Rows[i]["OrderCreatSource"].ToString();
                    }
                    listInfo.Add(order);

                }
            }
            dic.Add("OrderList", listInfo);
            return dic;
        }
        /// <summary>
        /// 2017-03-21 张立彬
        /// 根据项目ID查询订单信息
        /// </summary>
        /// <param name="orderID">项目ID</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> SelectSubOrderByOrderID(string orderID)
        {
            string strWhere = string.Empty;
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            int userId = Common.UserInfo.GetLoginUserID();
            if (roleIdList.Contains("SYS001RL00004") || roleIdList.Contains("SYS001RL00001"))
            {
                strWhere = "";
            }
            else if (roleIdList.Contains("SYS001RL00005"))
            {

                string userIdList = UserDataPower.GetUseridListByUserID(userId, 0);
                string strSelectMediaID = GetOrderWhere(0, userId);
                strWhere = " and (S.CreateUserID in " + userIdList + " or " + strSelectMediaID + ")";
            }
            else if (roleIdList.Contains("SYS001RL00002"))
            {
                strWhere = " and (S.CreateUserID=" + userId + " or AD.CustomerID=" + userId + ")"; ;
            }
            else if (roleIdList.Contains("SYS001RL00003"))
            {
                string strSelectMediaID = GetOrderWhere(0, userId);
                strWhere = " and (S.CreateUserID=" + userId + " or " + strSelectMediaID + ")";
            }
            else
            {
                strWhere = " and 1=2";
            }
            DataTable dt = Dal.ADOrderInfo.Instance.SelectSubOrderByOrderID(orderID, strWhere);
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("SubOrderID", dt.Rows[i]["SubOrderID"]);
                    dic.Add("MediaTypeName", dt.Rows[i]["MediaTypeName"]);
                    dic.Add("OrderState", dt.Rows[i]["OrderState"]);
                    dic.Add("TotalAmount", dt.Rows[i]["TotalAmount"]);
                    dic.Add("Status", dt.Rows[i]["Status"]);
                    dic.Add("MediaName", dt.Rows[i]["MediaName"]);
                    list.Add(dic);
                }

            }
            return list;
        }
        /// <summary>
        ///  2017-03-21张立彬
        ///  根据媒体了获取对应媒体的订单权限条件拼接
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public string GetOrderWhere(int mediaType, int userId)
        {
            string strSelectMediaID = "";
            switch (mediaType)
            {
                case 0:
                    strSelectMediaID = " (S.MediaID in (select MediaID from Media_Weixin where CreateUserID=" + userId + ") and  S.MediaType=14001) or (S.MediaID in (select MediaID from Media_PCAPP where CreateUserID=" + userId + ") and  S.MediaType=14002) or (S.MediaID in (select MediaID from Media_Weibo where CreateUserID=" + userId + ") and  S.MediaType=14003) or (S.MediaID in (select MediaID from Media_Video where CreateUserID=" + userId + ") and  S.MediaType=14004) or (S.MediaID in (select MediaID from Media_Broadcast where CreateUserID=" + userId + ") and S.MediaType=14005)";
                    break;
                case 14001:
                    strSelectMediaID = " (S.MediaID in (select MediaID from Media_Weixin where CreateUserID=" + userId + ") and S.MediaType=14001)";
                    break;
                case 14002:
                    strSelectMediaID = " (S.MediaID in (select MediaID from Media_PCAPP where CreateUserID=" + userId + ") and  S.MediaType=14002)";
                    break;
                case 14003:
                    strSelectMediaID = " (S.MediaID in (select MediaID from Media_Weibo where CreateUserID=" + userId + ") and  S.MediaType=14003)";
                    break;
                case 14004:
                    strSelectMediaID = " (S.MediaID in (select MediaID from Media_Video where CreateUserID=" + userId + ") and  S.MediaType=14004)";
                    break;
                case 14005:
                    strSelectMediaID = " (S.MediaID in (select MediaID from Media_Broadcast where CreateUserID=" + userId + ") and  S.MediaType=14005)";
                    break;
                default:
                    strSelectMediaID = " 1=2";
                    break;
            }
            return strSelectMediaID;
        }

        #endregion

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ADOrderInfo GetADOrderInfo(string OrderID)
        {

            return Dal.ADOrderInfo.Instance.GetADOrderInfo(OrderID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByOrderID(string OrderID)
        {
            QueryADOrderInfo query = new QueryADOrderInfo();
            query.OrderID = OrderID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetADOrderInfo(query, string.Empty, 1, 1, out count);
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
        public string Insert(Entities.ADOrderInfo model)
        {
            return Dal.ADOrderInfo.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string InsertADOrderInfo(Entities.ADOrderInfo model)
        {
            return Dal.ADOrderInfo.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ADOrderInfo model)
        {
            return Dal.ADOrderInfo.Instance.Update(model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string OrderID)
        {

            return Dal.ADOrderInfo.Instance.Delete(OrderID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, string OrderID)
        {

            return Dal.ADOrderInfo.Instance.Delete(sqltran, OrderID);
        }

        #endregion

        #region 根据项目号删除项目订单广告位排期信息
        /// <summary>
        /// 根据项目号删除项目订单广告位排期信息
        /// </summary>
        /// <param name="OrderID">项目号</param>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="ISDeleteADOrder">是否删除项目</param>
        /// <returns></returns>
        public string p_ADOrderAllInfo_Delete(string OrderID, int MediaType, bool ISDeleteADOrder)
        {
            return Dal.ADOrderInfo.Instance.p_ADOrderAllInfo_Delete(OrderID, MediaType, ISDeleteADOrder);
        }
        #endregion

        #region 更改主订单状态
        /// <summary>
        /// 根据订单号更改主订单状态
        /// </summary>
        /// <param name="orderid">主订单号</param>
        /// <param name="status">状态，具体值看订单状态枚举</param>
        /// <returns></returns>
        public int UpdateStatus_ADOrder(string orderid, int status)
        {
            return Dal.ADOrderInfo.Instance.UpdateStatus_ADOrder(orderid, status);
        }
        #endregion

        #region 更改主订单价格
        /// <summary>
        /// 根据订单号更改主订单状态
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="pirce"></param>
        /// <returns></returns>
        public int UpdateTotalAmount_ADOrder(string orderid, decimal total)
        {
            return Dal.ADOrderInfo.Instance.UpdateTotalAmount_ADOrder(orderid, total);
        }
        #endregion

        #region 自媒体-根据媒体类型、刊例广告位ID获取广告位信息
        /// <summary>
        /// 自媒体-根据媒体类型、刊例广告位ID获取广告位信息
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="pubDetailID"></param>
        /// <returns></returns>
        public DataTable GetSelfMediaDetail_MediaTypePubDetailID(int mediaType, int pubDetailID)
        {
            return Dal.ADOrderInfo.Instance.GetSelfMediaDetail_MediaTypePubDetailID(mediaType, pubDetailID);
        }
        #endregion

        #region 自媒体-根据媒体类型、媒体ID获取媒体信息
        /// <summary>
        /// 自媒体-根据媒体类型、媒体ID获取媒体信息
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="mediaID">嫖体ID</param>
        /// <returns></returns>
        public DataTable GetSelfMediaDetail_MediaTypeMediaID(int mediaType, int mediaID)
        {
            return Dal.ADOrderInfo.Instance.GetSelfMediaDetail_MediaTypeMediaID(mediaType, mediaID);
        }
        #endregion

        #region APP-根据广告位ID获取媒体名称广告位信息
        public DataTable GetAPPMediaDetail_PubDetailID(int pubDetailID)
        {
            return Dal.ADOrderInfo.Instance.GetAPPMediaDetail_PubDetailID(pubDetailID);
        }
        #endregion

        #region 根据媒体类型、广告位ID获取刊例ID、成本价、采购、销售折扣
        /// <summary>
        /// #region 根据媒体类型、广告位ID获取刊例ID、成本价、采购、销售折扣
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="pubDetailID">媒体ID</param>
        /// <returns></returns>
        public DataTable p_GetPubDetailInfo_Select(int mediaType, int pubDetailID)
        {
            return Dal.ADOrderInfo.Instance.p_GetPubDetailInfo_Select(mediaType, pubDetailID);
        }
        #endregion

        #region 根据个人姓名、公司名称或手机号模糊查询广告主
        /// <summary>
        /// 根据个人姓名、公司名称或手机号模糊查询广告主
        /// </summary>
        /// <param name="AEUserID">AE的userid</param>
        /// <param name="NameOrMobile">模糊查询的姓名、公司名称或手机号</param>
        /// <returns></returns>
        public DataTable p_ADMaster_Select(int AEUserID, string NameOrMobile, int IsAEAuth)
        {
            return Dal.ADOrderInfo.Instance.p_ADMaster_Select(AEUserID, NameOrMobile, IsAEAuth);
        }
        #endregion

        #region AE提交修改订单，验证广告主是否存在
        /// <summary>
        /// AE提交修改订单，验证广告主是否存在
        /// </summary>
        /// <param name="AEUserID">AE的userid</param>
        /// <param name="CustomerID">广告主UserID</param>
        /// <returns></returns>
        public string p_ADMaster_IsExist(int AEUserID, int CustomerID)
        {
            return Dal.ADOrderInfo.Instance.p_ADMaster_IsExist(AEUserID, CustomerID);
        }
        #endregion

        #region 根据媒体类型广告位ID获取刊例ID、成本价、采购、销售折扣V1.1
        /// <summary>
        /// #region 根据媒体类型、广告位ID获取刊例ID、成本价、采购、销售折扣
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="pubDetailID">媒体ID</param>
        /// <returns></returns>
        public DataTable p_GetPubDetailInfo_SelectV1_1(int mediaType, int pubDetailID)
        {
            return Dal.ADOrderInfo.Instance.p_GetPubDetailInfo_SelectV1_1(mediaType, pubDetailID);
        }
        #endregion

        #region 根据项目号删除项目订单广告位排期信息V1.1
        /// <summary>
        /// 根据项目号删除项目订单广告位排期信息
        /// </summary>
        /// <param name="OrderID">项目号</param>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="ISDeleteADOrder">是否删除项目</param>
        /// <returns></returns>
        public string p_ADOrderAllInfo_DeleteV1_1(string OrderID, int MediaType, bool ISDeleteADOrder)
        {
            return Dal.ADOrderInfo.Instance.p_ADOrderAllInfo_DeleteV1_1(OrderID, MediaType, ISDeleteADOrder);
        }
        #endregion

        #region 自媒体-根据媒体类型、刊例广告位ID获取广告位信息V1.1
        /// <summary>
        /// 自媒体-根据媒体类型、刊例广告位ID获取广告位信息
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="pubDetailID"></param>
        /// <returns></returns>
        public DataTable GetSelfMediaDetail_MediaTypePubDetailIDV1_1(int mediaType, int pubDetailID)
        {
            return Dal.ADOrderInfo.Instance.GetSelfMediaDetail_MediaTypePubDetailIDV1_1(mediaType, pubDetailID);
        }
        #endregion

        #region 根据项目号订单号查询订单操作日志V1.1
        public DataTable GeADOrderOperateInfoV1_1(string orderid, string suborderid)
        {
            return Dal.ADOrderInfo.Instance.GeADOrderOperateInfoV1_1(orderid, suborderid);
        }
        #endregion

        #region 根据微信号或名称模类查询V1.1
        public string QueryWeChat_NumerOrNameV1_1(int UserID, string NumberORName, int AuditStatus, out List<Dictionary<string, object>> dicList)
        {
            return Dal.ADOrderInfo.Instance.QueryWeChat_NumerOrNameV1_1(UserID, NumberORName, AuditStatus, out dicList);
        }
        #endregion
        #region 根据APP名称模类查询V1.1.4
        public string p_APPSelectByName(int UserID, string Name, int AuditStatus, out List<Dictionary<string, object>> dicList)
        {
            return Dal.ADOrderInfo.Instance.p_APPSelectByName(UserID, Name, AuditStatus, out dicList);
        }
        #endregion

        #region 新增项目
        public void AddOrderInfo(RequestADOrderInfoDto reqDto, out string orderid, out string msg, bool isFromShopCart = false)
        {
            Loger.Log4Net.Info("新增项目-----方法开始");
            orderid = reqDto.ADOrderInfo.OrderID;
            msg = string.Empty;
            #region 购物车添加到待审项目项目媒体生成空记录
            if (isFromShopCart)
            {
                var queryADDetails = from x in reqDto.ADDetails
                                     select x.MediaType;
                var detailMediaType = queryADDetails.Distinct();
                var mediaOrderMediaType = from x in reqDto.MediaOrderInfos
                                          select x.MediaType;
                var exceptMediaType = detailMediaType.Except(mediaOrderMediaType.Distinct());
                foreach (var item in exceptMediaType)
                {
                    reqDto.MediaOrderInfos.Add(new RequestMediaOrderInfoDto()
                    {
                        MediaType = item,
                        Note = "注：购物车添加到待审项目请补填需求"
                    });
                }
            }
            #endregion
            if (!reqDto.CheckSelfModel(out msg))
                return;

            if (reqDto.optType == (int)EnumAddModify.Modify)
            {
                UpdateDelelteInfo(reqDto, out msg);
                if (!string.IsNullOrEmpty(msg))
                    return;
            }
            else
            {
                //生成项目           
                orderid = InsertOrder(reqDto.ADOrderInfo);
                Loger.Log4Net.Info(string.Format($"新增项目-----生成项目号<{orderid}>"));
            }
            //生成项目媒体
            InsertMediaOrder(reqDto.MediaOrderInfos, orderid);

            string cartids = string.Empty;
            decimal priceAmmount = 0;
            var queryAll = reqDto.ADDetails.GroupBy(x => x.MediaType);
            foreach (var groupMediaType in queryAll)
            {

                var queryMedia = groupMediaType.GroupBy(x => x.MediaID);
                foreach (var groupitem in queryMedia)
                {
                    decimal itotal = 0;
                    string suborderid = string.Empty;
                    List<RequestADDetailDto> listGroup = groupitem.ToList();
                    foreach (var detailitem in listGroup)
                    {
                        cartids += detailitem.CartID + ",";
                        #region 生成订单&广告位&排期
                        if (detailitem.ADScheduleInfos.Count > 0)
                        {
                            List<Entities.DTO.ADScheduleDto> listADSchedule = new List<Entities.DTO.ADScheduleDto>();
                            foreach (var itemADSchedule in detailitem.ADScheduleInfos)
                            {
                                #region 生成订单
                                Entities.SubADInfo submodel = new Entities.SubADInfo()
                                {
                                    OrderID = orderid,
                                    MediaType = detailitem.MediaType,
                                    MediaID = groupitem.Key,
                                    Status = reqDto.ADOrderInfo.Status,
                                    CreateTime = DateTime.Now,
                                    CreateUserID = currentUserID,
                                    ChannelID = detailitem.ChannelID
                                };
                                //suborderid = SubADInfo.Instance.Insert(submodel);
                                suborderid = Dal.SubADInfo.Instance.InsertV1_1_4(submodel);
                                Loger.Log4Net.Info(string.Format($"新增项目[媒体类型]{detailitem.MediaType}[生成订单]<{suborderid}>"));
                                #endregion
                                #region 生成广告位
                                decimal adjustPrice = 0;
                                int detailid = -2;
                                int pubId = -2;
                                InsertADDetail(detailitem, itemADSchedule, orderid, suborderid, reqDto.ADOrderInfo.Status, out detailid, out adjustPrice, out pubId);
                                itotal += adjustPrice;
                                //更新子工单金额
                                SubADInfo.Instance.UpdateTotalAmmount_SubADInfo(adjustPrice, suborderid);
                                #endregion
                                listADSchedule.Add(new Entities.DTO.ADScheduleDto()
                                {
                                    ADDetailID = detailid,
                                    OrderID = orderid,
                                    SubOrderID = suborderid,
                                    MediaID = detailitem.MediaID,
                                    PubID = pubId,
                                    CreateTime = DateTime.Now,
                                    CreateUserID = currentUserID,
                                    BeginData = itemADSchedule.BeginData,
                                    EndData = itemADSchedule.EndData
                                });
                            }
                            ADScheduleInfo.Instance.Insert_BulkCopyToDB(Util.ListToDataTable(listADSchedule));
                        }
                        #endregion
                    }
                    priceAmmount += itotal;
                }
            }
            //更新主订单金额
            Instance.UpdateTotalAmount_ADOrder(orderid, priceAmmount);
            if (cartids.EndsWith(","))
                cartids = cartids.Substring(0, cartids.Length - 1);

            CartInfo.Instance.DeleteV1_1(currentUserID, cartids);
            Loger.Log4Net.Info("新增项目-----方法结束");
        }
        #endregion
        #region 修改项目
        public void UpdateDelelteInfo(RequestADOrderInfoDto reqDto, out string msg)
        {
            msg = string.Empty;
            #region 查询并更新主订单
            Entities.ADOrderInfo order = Instance.GetADOrderInfo(reqDto.ADOrderInfo.OrderID);
            if (order == null)
            {
                msg = "订单号：" + reqDto.ADOrderInfo.OrderID + "，不存在";
                return;
            }
            if (order.Status == (int)EnumOrderStatus.Draft || order.Status == (int)EnumOrderStatus.PendingAudit)
            {
                //可修改订单。
            }
            else
            {
                msg = "当前状态不允许修改项目";
                return;
            }
            //更新主订单
            order.OrderName = reqDto.ADOrderInfo.OrderName;
            order.Status = reqDto.ADOrderInfo.Status;
            order.CRMCustomerID = reqDto.ADOrderInfo.CRMCustomerID;
            order.CustomerText = reqDto.ADOrderInfo.CustomerText;
            try
            {
                //解码
                reqDto.ADOrderInfo.CustomerID = System.Web.HttpUtility.UrlDecode(reqDto.ADOrderInfo.CustomerID, Encoding.UTF8);
                //解密
                order.CustomerID = Convert.ToInt32(XYAuto.Utils.Security.DESEncryptor.Decrypt(reqDto.ADOrderInfo.CustomerID, LoginPwdKey));
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****ModifyOrderInfo 客户ID解码解密出错：->" + ex.Message);
            }
            Instance.Update(order);
            #endregion

            Instance.p_ADOrderAllInfo_DeleteV1_1(reqDto.ADOrderInfo.OrderID, 0, false);
        }
        #endregion
        #region 生成项目
        public string InsertOrder(RequestADOrderDto reqDto)
        {
            string orderid = string.Empty;
            Entities.ADOrderInfo order = new Entities.ADOrderInfo()
            {
                OrderName = reqDto.OrderName,
                Status = reqDto.Status,
                CreateTime = DateTime.Now,
                CreateUserID = currentUserID,
                CRMCustomerID = reqDto.CRMCustomerID,
                CustomerText = reqDto.CustomerText
            };

            try
            {
                //解码
                reqDto.CustomerID = System.Web.HttpUtility.UrlDecode(reqDto.CustomerID, Encoding.UTF8);
                //解密
                order.CustomerID = Convert.ToInt32(Utils.Security.DESEncryptor.Decrypt(
                        System.Web.HttpUtility.UrlDecode(reqDto.CustomerID, Encoding.UTF8),
                        Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey")));
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("新增项目[客户ID解码解密]出错:" + ex.Message);
            }
            return Instance.Insert(order);
        }
        #endregion
        #region 生成项目媒体
        public void InsertMediaOrder(List<RequestMediaOrderInfoDto> reqMediaOrders, string orderid)
        {
            foreach (var mediaOrderItem in reqMediaOrders)
            {
                int recid = MediaOrderInfo.Instance.Insert(new Entities.MediaOrderInfo()
                {
                    MediaType = mediaOrderItem.MediaType,
                    Note = mediaOrderItem.Note,
                    UploadFileURL = mediaOrderItem.UploadFileURL,
                    OrderID = orderid,
                    CreateTime = DateTime.Now,
                    CreateUserID = currentUserID
                });

                if (!string.IsNullOrEmpty(mediaOrderItem.UploadFileURL))
                {
                    UploadFileInfo.UploadFileInfo.Instance.Excute(new List<string>() { mediaOrderItem.UploadFileURL }, currentUserID,
                        Entities.Enum.UploadFileEnum.OrderManage, recid, "MediaOrderInfo");
                }
            }
        }
        #endregion
        #region 生成广告位&计算价格
        public void InsertADDetail(RequestADDetailDto reqDto, RequestADScheduleInfoDto reqADSchedule, string orderid, string suborderid, int orderStatus, out int detailid, out decimal AdjustPrice, out int pubId)
        {
            detailid = -2;
            AdjustPrice = 0;
            pubId = -2;

            Entities.ADDetailInfo detailEntity = ADDetailInfo.Instance.p_GetPubDetailInfo_SelectV1_1(reqDto.MediaType, reqDto.PubDetailID);
            if (detailEntity == null)
                detailEntity = new Entities.ADDetailInfo();

            detailEntity.OrderID = orderid;
            detailEntity.SubOrderID = suborderid;
            detailEntity.MediaType = reqDto.MediaType;
            detailEntity.MediaID = reqDto.MediaID;
            detailEntity.AdjustPrice = reqDto.AdjustPrice;
            detailEntity.AdjustDiscount = reqDto.AdjustDiscount;
            detailEntity.ADLaunchDays = reqDto.ADLaunchDays;
            detailEntity.CreateUserID = currentUserID;
            detailEntity.PubDetailID = reqDto.PubDetailID;
            detailEntity.SaleArea = reqDto.SaleAreaID;
            pubId = detailEntity.PubID;
            detailEntity.ChannelID = reqDto.ChannelID;
            detailEntity.CostReferencePrice = reqDto.CostReferencePrice;

            //计算价格草稿、待审状态下 成交价=销售价*折扣
            if (orderStatus == (int)EnumOrderStatus.Draft || orderStatus == (int)EnumOrderStatus.PendingAudit)
            {
                if (reqDto.MediaType == (int)EnumMediaType.WeChat)
                {
                    detailEntity.ADLaunchDays = 1;
                    //微信价格=原始价格*排期个数（已改为一个广告位对应一个排期）
                    detailEntity.AdjustPrice = detailEntity.OriginalPrice;
                }
                else if (reqDto.MediaType == (int)EnumMediaType.APP)
                {
                    if (detailEntity?.ADLaunchIDs == "11001")
                    {
                        //CPD计算天数  
                        int allDays = 0;
                        int holiDays = 0;
                        allDays += reqADSchedule.EndData.Subtract(reqADSchedule.BeginData).Days + 1;
                        if (detailEntity.HasHoliday == 1)
                        {
                            List<ResponseQueryHolidayDto> listHoliday = new List<ResponseQueryHolidayDto>();
                            holiDays += CartInfo.Instance.p_CALHolidays(reqADSchedule.BeginData, reqADSchedule.EndData, out listHoliday);
                        }
                        detailEntity.ADLaunchDays = allDays;
                        detailEntity.Holidays = holiDays;
                        detailEntity.AdjustPrice = detailEntity.HasHoliday == 1 ? detailEntity.OriginalPrice * (allDays - holiDays) + detailEntity.SalePrice_Holiday * holiDays
                            : detailEntity.OriginalPrice * allDays;
                    }
                    else if (detailEntity?.ADLaunchIDs == "11002")
                        detailEntity.AdjustPrice = detailEntity.OriginalPrice * detailEntity.ADLaunchDays;
                }
            }
            //广告位失效、下架、过期 则不计算金额
            //dto:201704 修改为 计算金额
            //if (detailEntity.expired == 0 && 
            //    (detailEntity.PublishStatus == (int)Entities.Enum.AppPublishStatus.已上架
            //     || detailEntity.PublishStatus == (int)Entities.Enum.PublishStatusEnum.已上架)
            //   )
            AdjustPrice = detailEntity.AdjustPrice;

            detailid = Dal.ADDetailInfo.Instance.InsertV1_1_4(detailEntity);
        }
        #endregion

        #region 根据项目号查询
        public void QueryADOrderInfoByOrderID(string orderID, out ResponseADOrderDto resADOrder, out List<ResponseMediaOrderInfoDto> resMOIList, out List<ResponseSubADInfoDto> resSubAInfoList)
        {
            DataTable dtADOrderInfo = null;
            DataTable dtMOI = null;
            DataTable dtSubADInfo = null;

            Dal.ADOrderInfo.Instance.QueryADOrderInfoByOrderID(orderID, out dtADOrderInfo, out dtMOI, out dtSubADInfo);
            resADOrder = Util.DataTableToEntity<ResponseADOrderDto>(dtADOrderInfo);
            resMOIList = Util.DataTableToList<ResponseMediaOrderInfoDto>(dtMOI);
            resSubAInfoList = Util.DataTableToList<ResponseSubADInfoDto>(dtSubADInfo);

        }
        #endregion
        #region 购物车添加到待审项目查询
        public void QueryAEOrderInfo(string orderID, out RequestADOrderDto resADOrder, out List<RequestMediaOrderInfoDto> resMOIList, out List<RequestADDetailDto> resADDetailsList)
        {
            DataTable dtADOrderInfo = null;
            DataTable dtMOI = null;
            DataTable dtADDetails = null;

            Dal.ADOrderInfo.Instance.QueryAEOrderInfo(orderID, out dtADOrderInfo, out dtMOI, out dtADDetails);
            resADOrder = Util.DataTableToEntity<RequestADOrderDto>(dtADOrderInfo);
            resMOIList = Util.DataTableToList<RequestMediaOrderInfoDto>(dtMOI);
            resADDetailsList = Util.DataTableToList<RequestADDetailDto>(dtADDetails);
        }
        #endregion
        #region V1.1.8
        #region 根据个人姓名、公司名称或手机号模糊查询广告主
        /// <summary>
        /// 根据个人姓名、公司名称或手机号模糊查询广告主
        /// </summary>
        /// <param name="AEUserID">AE的userid</param>
        /// <param name="NameOrMobile">模糊查询的姓名、公司名称或手机号</param>
        /// <returns></returns>
        public DataTable p_ADMaster_SelectV1_1_8(int AEUserID, string NameOrMobile, int IsAEAuth)
        {
            return Dal.ADOrderInfo.Instance.p_ADMaster_SelectV1_1_8(AEUserID, NameOrMobile, IsAEAuth);
        }
        #endregion
        #endregion
        #region 1.1.8
        #region 根据项目号查询
        public void QueryADOrderInfoByOrderIDV1_1_8(string orderID, out ResponseADOrderDto resADOrder, out List<ResponseMediaOrderInfoDto> resMOIList, out List<ResponseSubADInfoDto> resSubAInfoList)
        {
            DataTable dtADOrderInfo = null;
            DataTable dtMOI = null;
            DataTable dtSubADInfo = null;

            Dal.ADOrderInfo.Instance.QueryADOrderInfoByOrderIDV1_1_8(orderID, out dtADOrderInfo, out dtMOI, out dtSubADInfo);
            resADOrder = Util.DataTableToEntity<ResponseADOrderDto>(dtADOrderInfo);
            resMOIList = Util.DataTableToList<ResponseMediaOrderInfoDto>(dtMOI);
            resSubAInfoList = Util.DataTableToList<ResponseSubADInfoDto>(dtSubADInfo);
        }
        #endregion
        /// <summary>
        ///  修改CRM编号
        /// </summary>
        /// <param name="OrderNum">订单号</param>
        /// <param name="CrmNum">CRM编号</param>
        /// <returns></returns>
        public string UpdateCrmNum(UpdateCrmNumResDTO dto)
        {
            if (dto == null || dto.OrderNum == null || dto.OrderNum == "")
            {
                return "参数错误";
            }
            if (dto.CrmNum != null && dto.CrmNum != "")
            {
                if (dto.CrmNum.Length > 15)
                {
                    return "CRM合同编号长度不能大于15";
                }
                string pattern = @"^[a-zA-Z0-9]*$"; //匹配所有字符都在字母和数字之间 
                if (!System.Text.RegularExpressions.Regex.IsMatch(dto.CrmNum, pattern))
                {
                    return "CRM合同编号只能输入字母和数字";
                }
            }
            string RoleID = Common.UserInfo.GetLoginUserRoleIDs();
            if (!RoleID.Contains("SYS001RL00008") && !RoleID.Contains("SYS001RL00001") && !RoleID.Contains("SYS001RL00004") && !RoleID.Contains("SYS001RL00005"))
            {
                return "当前角色不可修改";
            }
            if (Dal.ADOrderInfo.Instance.UpdateCrmNum(dto.OrderNum, dto.CrmNum) > 0)
            {
                return "";
            }
            else
            {
                return "修改失败";
            }
        }
        /// <summary>
        /// zlb 2017-07-25
        /// 上传发文地址
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public string UpdatePostingAddressBySubID(UpdatePostingAddressResDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.SubOrderID) || string.IsNullOrWhiteSpace(dto.PostingAddress))
            {
                return "参数错误";
            }
            string Pattern = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&$%\$#\=~])*$";
            Regex r = new Regex(Pattern);
            Match m = r.Match(dto.PostingAddress);
            if (!m.Success)
            {
                return "请输入正确的地址格式";
            }

            if (Dal.ADOrderInfo.Instance.UpdatePostingAddressBySubID(dto.SubOrderID, dto.PostingAddress) > 0)
            {
                return "";
            }
            else
            {
                return "保存失败";
            }
        }
        /// <summary>
        /// zlb 2017-07-26
        /// 生成发案报告文件
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <returns></returns>
        public string SelectFinalReportUrlByOrderID(string OrderID, out string ErrorMessage)
        {
            if (string.IsNullOrWhiteSpace(OrderID))
            {
                ErrorMessage = "参数错误";
            }
            OrderID = OrderID == null ? "" : OrderID.Trim();
            DataSet ds = Dal.ADOrderInfo.Instance.SelectFeedBackFileInfoByOrderID(OrderID.Trim());
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
            {
                string finalReportURL = ds.Tables[0].Rows[0]["FinalReportURL"].ToString();
                if (finalReportURL != "")
                {
                    ErrorMessage = "";
                    return finalReportURL;
                }
                string OrderName = ds.Tables[0].Rows[0]["OrderName"].ToString();
                if (OrderName == "") OrderName = OrderID;

                BLL.Loger.Log4Net.Info("读取结案报告模板：" + _uploadFilePath + ReportTemplate + "ReportTemplate.docx");
                Document document = new Document(_uploadFilePath + ReportTemplate + "ReportTemplate.docx");

                OperateWord.InsertTable(document, ds.Tables[1]);
                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    List<string> listUrl = new List<string>();
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        DataRow dr = ds.Tables[2].Rows[i];
                        string FileUrl = dr["UploadFileURL"].ToString();
                        if (!string.IsNullOrWhiteSpace(FileUrl))
                        {
                            listUrl.Add(_uploadFilePath + FileUrl);
                            BLL.Loger.Log4Net.Info("读取结案报告图片：" + _uploadFilePath + FileUrl);
                        }
                    }
                    OperateWord.InsertImage(document, listUrl);
                }
                DateTime time = DateTime.Now;
                string relatedPath = string.Format(ReportTemplate + "{0}/{1}/{2}/{3}/", time.Year, time.Month, time.Day, time.Hour);
                string dir = _uploadFilePath + relatedPath.Replace(@"/", "\\");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string FileName = OrderName.Replace("/", "-").Replace(@"\", "-") + "结案报告.docx";
                document.SaveToFile(dir + FileName);
                Dal.ADOrderInfo.Instance.UpdateFinalReportURL(OrderID, relatedPath + FileName);
                BLL.Loger.Log4Net.Info("生成结案报告路径：" + dir + FileName);
                ErrorMessage = "";
                return relatedPath + FileName;

            }
            ErrorMessage = "该项目无结案报告";
            return "";
        }
        /// <summary>
        /// zlb 2017-07-31
        /// 查看发文地址
        /// </summary>
        /// <param name="SubOrderID"></param>
        /// <returns></returns>
        public string SelectPostingAddress(string SubOrderID)
        {
            SubOrderID = SubOrderID == null ? "" : SubOrderID.Trim();
            return Dal.ADOrderInfo.Instance.SelectPostingAddress(SubOrderID);
        }


        #endregion
        #region V1.1.8第二部分
        #region 根据项目号渲染生成二维码页
        public List<TwoBarCodeDetailDto> GetTwoBarCodeHistory(string orderID, out GetTwoBarCodeHistoryDto resDto)
        {
            return Dal.ADOrderInfo.Instance.GetTwoBarCodeHistory(orderID, out resDto);
        }
        #endregion        
        #region 根据投放城市、投放日期、已投放媒体帐号查询广告位
        public List<GetPublishDetailForRecommendAddPDDto> GetPublishDetailForRecommendAddPD(string MeidiaNumbers, DateTime launchTime, int provinceID = -2, int cityID = -2)
        {
            return Dal.ADOrderInfo.Instance.GetPublishDetailForRecommendAddPD(MeidiaNumbers, launchTime, provinceID, cityID);
        }
        #endregion
        #region 提交修改智投项目
        public void IntelligenceADOrderInfoCrud(RequestIntelligenceADOrderInfoCrudDto reqDto, out string orderid, out string msg)
        {
            Loger.Log4Net.Info("提交修改智投项目-----方法开始");
            orderid = reqDto.ADOrderInfo.OrderID;
            msg = string.Empty;
            if (!reqDto.CheckSelfModel(out msg))
                return;

            if (reqDto.optType == EnumIntelligenceADOrderInfoCrudOptType.ADDADOrderNote)
                IntelligenceADOrderInfoCrud_ADDADOrderNote(reqDto.ADOrderInfo, out msg);
            if (reqDto.optType == EnumIntelligenceADOrderInfoCrudOptType.ADD)
                IntelligenceADOrderInfoCrud_ADD(reqDto, out msg, out orderid);
            if (reqDto.optType == EnumIntelligenceADOrderInfoCrudOptType.Modify)
            {
                IntelligenceADOrderInfoCrud_Modify(reqDto, out msg);
                IntelligenceADOrderInfoCrud_ADDADOrderNote(reqDto.ADOrderInfo, out msg);
            }                
            if (!string.IsNullOrEmpty(msg))
                return;
        }
        public void IntelligenceADOrderInfoCrud_ADDADOrderNote(RequestIntelligenceADOrderDto reqDto, out string msg)
        {
            msg = string.Empty;
            if (string.IsNullOrEmpty(reqDto.OrderID))
            {
                msg = "智投添加需求项目号不能为空";
                Loger.Log4Net.Info($"智投添加需求项目号不能为空");
                return;
            }
            if (!string.IsNullOrEmpty(reqDto.Note))
            {
                if (reqDto.Note.Length > 500)
                {
                    msg = "项目需求最多输入500个汉字，请修改！";
                    Loger.Log4Net.Info($"项目需求最多输入500个汉字，请修改！");
                    return;
                }                
            }
            try
            {
                //解码
                reqDto.CustomerID = System.Web.HttpUtility.UrlDecode(reqDto.CustomerID, Encoding.UTF8);
                //解密
                reqDto.CustomerIDINT = Convert.ToInt32(Utils.Security.DESEncryptor.Decrypt(
                        System.Web.HttpUtility.UrlDecode(reqDto.CustomerID, Encoding.UTF8),
                        Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey")));

                Loger.Log4Net.Info($"根目录：{AppDomain.CurrentDomain.BaseDirectory}");

                string fileName = $"{reqDto.OrderID}.png";
                string relativeDir = $"/UploadFiles/ADOrderMarketingImage/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{DateTime.Now.Hour}";
                string uploadDir = _uploadFilePath + relativeDir;
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);
                string filePath = $"{uploadDir}/{fileName}";
                string sourcePath = $"{_uploadFilePath}/UploadFiles/ImageTemplate/test.png";
                Loger.Log4Net.Info($"targetPath：{filePath}");
                Loger.Log4Net.Info($"sourcePath：{sourcePath}");
                BLL.Util.DrawStringByImage(reqDto.MarketingPolices, sourcePath, filePath);
                reqDto.MarketingUrl = "http://www.chitunion.com/" + $"{relativeDir}/{fileName}";
                Dal.ADOrderInfo.Instance.UpdateRecommend_ADDADOrderNote(reqDto);
            }
            catch (Exception ex)
            {
                msg = $"智投添加需求操作失败：{ex.Message}";
                Loger.Log4Net.Info(msg);
                return;
            }
        }

        public void IntelligenceADOrderInfoCrud_ADD(RequestIntelligenceADOrderInfoCrudDto reqDto, out string msg, out string orderID)
        {
            msg = string.Empty;
            orderID = string.Empty;
            try
            {
                //解码
                reqDto.ADOrderInfo.CustomerID = System.Web.HttpUtility.UrlDecode(reqDto.ADOrderInfo.CustomerID, Encoding.UTF8);
                //解密
                reqDto.ADOrderInfo.CustomerIDINT = Convert.ToInt32(Utils.Security.DESEncryptor.Decrypt(
                        System.Web.HttpUtility.UrlDecode(reqDto.ADOrderInfo.CustomerID, Encoding.UTF8),
                        Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey")));

                Entities.ADOrderInfo adOrderModel = new Entities.ADOrderInfo()
                {
                    OrderName = reqDto.ADOrderInfo.OrderName,
                    Status = (int)Entities.EnumOrderStatus.Draft,
                    CustomerID = reqDto.ADOrderInfo.CustomerIDINT,
                    MarketingPolices = reqDto.ADOrderInfo.MarketingPolices,
                    UploadFileURL = reqDto.ADOrderInfo.UploadFileURL,
                    LaunchTime = reqDto.ADOrderInfo.LaunchTime.Date,
                    CRMCustomerID = reqDto.ADOrderInfo.CRMCustomerID,
                    CustomerText = reqDto.ADOrderInfo.CustomerText,
                    BudgetTotal = reqDto.ADOrderInfo.BudgetTotal,
                    OrderRemark = reqDto.ADOrderInfo.OrderRemark,
                    MasterID = reqDto.ADOrderInfo.MasterID,
                    BrandID = reqDto.ADOrderInfo.BrandID,
                    SerialID = reqDto.ADOrderInfo.SerialID,
                    MasterName = reqDto.ADOrderInfo.MasterName,
                    BrandName = reqDto.ADOrderInfo.BrandName,
                    SerialName = reqDto.ADOrderInfo.SerialName,
                    JKEntrance = reqDto.ADOrderInfo.JKEntrance,
                    Note=reqDto.ADOrderInfo.Note,
                    CreateUserID = currentUserID,
                    MediaType = 14001,
                    OrderType = 3
                };
                reqDto.ADOrderInfo.OrderID = Dal.ADOrderInfo.Instance.InsertV1_1_8(adOrderModel);
                orderID = reqDto.ADOrderInfo.OrderID;
                Loger.Log4Net.Info($"新增智投项目-----方法成功：{reqDto.ADOrderInfo.OrderID}");

                int recid = MediaOrderInfo.Instance.Insert(new Entities.MediaOrderInfo()
                {
                    MediaType = 14001,
                    Note = reqDto.ADOrderInfo.Note,
                    UploadFileURL = string.Empty,
                    OrderID = orderID,
                    CreateTime = DateTime.Now,
                    CreateUserID = currentUserID
                });

                decimal totalAmmount = 0;
                decimal costTotal = 0;
                decimal budget = 0;
                reqDto.ADOrderInfo.BudgetTotal = 0;
                foreach (var itemADetail in reqDto.ADDetails)
                {
                    Entities.ADOrderCityExtend cityExtendModel = new ADOrderCityExtend()
                    {
                        OrderID = reqDto.ADOrderInfo.OrderID,
                        ProvinceID = itemADetail.ProvinceID,
                        CityID = itemADetail.CityID,
                        Budget = itemADetail.Budget,
                        MediaCount = itemADetail.MediaCount,
                        OriginContain = itemADetail.OriginContain,
                        CreateUserID = currentUserID
                    };
                    budget += itemADetail.Budget;
                    int cityExtendID = 0;
                    cityExtendID = Dal.ADOrderInfo.Instance.p_ADOrderCityExtend_InsertV1_1_8(cityExtendModel);
                    Loger.Log4Net.Info($"新增智投项目-----新增项目城市方法成功：{cityExtendID}");
                    foreach (var itemPubDetail in itemADetail.PublishDetails)
                    {
                        Entities.ADDetailInfo aDDetailMode = new Entities.ADDetailInfo()
                        {
                            OrderID = reqDto.ADOrderInfo.OrderID,
                            CityExtendID = cityExtendID,
                            PubDetailID = itemPubDetail.PublishDetailID,
                            MediaType = itemPubDetail.MediaType,
                            MediaID = itemPubDetail.MediaID,
                            AdjustPrice = itemPubDetail.AdjustPrice,
                            EnableOriginPrice = itemPubDetail.EnableOriginPrice,
                            ChannelID = itemPubDetail.ChannelID,
                            CostReferencePrice = itemPubDetail.CostReferencePrice,
                            CostPrice = itemPubDetail.CostPrice,
                            FinalCostPrice = itemPubDetail.FinalCostPrice,
                            LaunchTime = itemPubDetail.LaunchTime,
                            CreateUserID = currentUserID,
                            Status = (int)EnumOrderStatus.Draft
                        };
                        #region 计算销售成本价
                        if (reqDto.ADOrderInfo.Status == (int)EnumOrderStatus.Draft || reqDto.ADOrderInfo.Status == (int)EnumOrderStatus.PendingAudit)
                        {
                            CalculateCostPrice(aDDetailMode);
                        }
                        #endregion
                        decimal totalAmmountTmp = 0;
                        decimal costTotalTmp = 0;
                        string subOrderID = string.Empty;
                        subOrderID = Dal.ADOrderInfo.Instance.p_ADDetailInfoInsert_IntelligenceV1_1_8(aDDetailMode, out totalAmmountTmp, out costTotalTmp);
                        totalAmmount += totalAmmountTmp;
                        costTotal += costTotalTmp;
                        Loger.Log4Net.Info($"新增智投项目-----新增订单广告位方法成功：{subOrderID}");
                    }
                }
                Dal.ADOrderInfo.Instance.UpdateADOrderAmmount(reqDto.ADOrderInfo.OrderID, totalAmmount, costTotal, budget);
                Loger.Log4Net.Info($"新增智投项目-----更新项目金额方法成功：{reqDto.ADOrderInfo.OrderID}");
            }
            catch (Exception ex)
            {
                msg = $"新增智投项目-----方法失败：{ex.Message}";
                Loger.Log4Net.Info(msg);
                return;
            }
        }
        public void IntelligenceADOrderInfoCrud_Modify(RequestIntelligenceADOrderInfoCrudDto reqDto, out string msg)
        {
            msg = string.Empty;
            if (string.IsNullOrEmpty(reqDto.ADOrderInfo.OrderID))
            {
                msg = "项目号为必填项";
                Loger.Log4Net.Info($"IntelligenceADOrderInfoCrud项目号为必填项");
                return;
            }
            try
            {
                Dal.ADOrderInfo.Instance.p_ADOrderAllInfo_DeleteV1_1_8(reqDto.ADOrderInfo.OrderID, 14001, false);
                Loger.Log4Net.Info($"修改智投项目-----方法删除项目信息成功：{reqDto.ADOrderInfo.OrderID}");
                //解码
                reqDto.ADOrderInfo.CustomerID = System.Web.HttpUtility.UrlDecode(reqDto.ADOrderInfo.CustomerID, Encoding.UTF8);
                //解密
                reqDto.ADOrderInfo.CustomerIDINT = Convert.ToInt32(Utils.Security.DESEncryptor.Decrypt(
                        System.Web.HttpUtility.UrlDecode(reqDto.ADOrderInfo.CustomerID, Encoding.UTF8),
                        Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey")));

                Entities.ADOrderInfo adOrderModel = new Entities.ADOrderInfo()
                {
                    OrderName = reqDto.ADOrderInfo.OrderName,
                    Status = reqDto.ADOrderInfo.Status,
                    CustomerID = reqDto.ADOrderInfo.CustomerIDINT,
                    MarketingPolices = reqDto.ADOrderInfo.MarketingPolices,
                    UploadFileURL = reqDto.ADOrderInfo.UploadFileURL,
                    LaunchTime = reqDto.ADOrderInfo.LaunchTime.Date,
                    CRMCustomerID = reqDto.ADOrderInfo.CRMCustomerID,
                    CustomerText = reqDto.ADOrderInfo.CustomerText,
                    BudgetTotal = reqDto.ADOrderInfo.BudgetTotal,
                    OrderRemark = reqDto.ADOrderInfo.OrderRemark,
                    MasterID = reqDto.ADOrderInfo.MasterID,
                    BrandID = reqDto.ADOrderInfo.BrandID,
                    SerialID = reqDto.ADOrderInfo.SerialID,
                    MasterName = reqDto.ADOrderInfo.MasterName,
                    BrandName = reqDto.ADOrderInfo.BrandName,
                    SerialName = reqDto.ADOrderInfo.SerialName,
                    JKEntrance = reqDto.ADOrderInfo.JKEntrance,
                    Note = reqDto.ADOrderInfo.Note,
                    CreateUserID = currentUserID,
                    MediaType = 14001,
                    OrderID = reqDto.ADOrderInfo.OrderID
                };
                Dal.ADOrderInfo.Instance.p_ADOrderInfo_UpdateV1_1_8(adOrderModel);
                Loger.Log4Net.Info($"修改智投项目-----方法更新项目成功：{reqDto.ADOrderInfo.OrderID}");

                decimal totalAmmount = 0;
                decimal costTotal = 0;
                decimal budget = 0;
                reqDto.ADOrderInfo.BudgetTotal = 0;
                foreach (var itemADetail in reqDto.ADDetails)
                {
                    Entities.ADOrderCityExtend cityExtendModel = new ADOrderCityExtend()
                    {
                        OrderID = reqDto.ADOrderInfo.OrderID,
                        ProvinceID = itemADetail.ProvinceID,
                        CityID = itemADetail.CityID,
                        Budget = itemADetail.Budget,
                        MediaCount = itemADetail.MediaCount,
                        OriginContain = itemADetail.OriginContain,
                        CreateUserID = currentUserID
                    };
                    budget += itemADetail.Budget;
                    int cityExtendID = 0;
                    cityExtendID = Dal.ADOrderInfo.Instance.p_ADOrderCityExtend_InsertV1_1_8(cityExtendModel);
                    Loger.Log4Net.Info($"新增智投项目-----新增项目城市方法成功：{cityExtendID}");
                    foreach (var itemPubDetail in itemADetail.PublishDetails)
                    {
                        Entities.ADDetailInfo aDDetailMode = new Entities.ADDetailInfo()
                        {
                            OrderID = reqDto.ADOrderInfo.OrderID,
                            CityExtendID = cityExtendID,
                            PubDetailID = itemPubDetail.PublishDetailID,
                            MediaType = itemPubDetail.MediaType,
                            MediaID = itemPubDetail.MediaID,
                            AdjustPrice = itemPubDetail.AdjustPrice,
                            EnableOriginPrice = itemPubDetail.EnableOriginPrice,
                            ChannelID = itemPubDetail.ChannelID,
                            CostReferencePrice = itemPubDetail.CostReferencePrice,
                            CostPrice = itemPubDetail.CostPrice,
                            FinalCostPrice = itemPubDetail.FinalCostPrice,
                            LaunchTime = itemPubDetail.LaunchTime,
                            CreateUserID = currentUserID,
                            Status = reqDto.ADOrderInfo.Status
                        };
                        #region 计算销售成本价
                        if (reqDto.ADOrderInfo.Status == (int)EnumOrderStatus.Draft || reqDto.ADOrderInfo.Status == (int)EnumOrderStatus.PendingAudit)
                        {
                            CalculateCostPrice(aDDetailMode);
                        }
                        #endregion
                        decimal totalAmmountTmp = 0;
                        decimal costTotalTmp = 0;
                        string subOrderID = string.Empty;
                        subOrderID = Dal.ADOrderInfo.Instance.p_ADDetailInfoInsert_IntelligenceV1_1_8(aDDetailMode, out totalAmmountTmp, out costTotalTmp);
                        totalAmmount += totalAmmountTmp;
                        costTotal += costTotalTmp;
                        Loger.Log4Net.Info($"新增智投项目-----新增订单广告位方法成功：{subOrderID}");
                    }
                }
                Dal.ADOrderInfo.Instance.UpdateADOrderAmmount(reqDto.ADOrderInfo.OrderID, totalAmmount, costTotal, budget);
                Loger.Log4Net.Info($"新增智投项目-----更新项目金额方法成功：{reqDto.ADOrderInfo.OrderID}");
            }
            catch (Exception ex)
            {
                msg = $"新增智投项目-----方法失败：{ex.Message}";
                Loger.Log4Net.Info(msg);
                return;
            }
        }

        #region 计算销售成本价
        public void CalculateCostPrice(Entities.ADDetailInfo aDDetailMode)
        {
            Loger.Log4Net.Info($"计算销售成本价-----广告位ID：{aDDetailMode.PubDetailID}，投放时间：{aDDetailMode.LaunchTime.Date.ToString()}");
            GetADPostionChannelIDDto position = Dal.ADOrderInfo.Instance.GetADPositionChannelIDByPubDetailID(aDDetailMode.PubDetailID);
            if (position == null)
            {
                aDDetailMode.FinalCostPrice = 0;
                Loger.Log4Net.Info($"计算销售成本价-----该广告位没有绑定渠道信息");
                return;
            }
            BLL.Media.Business.V1_8.ChannelInfoProvider ch = new BLL.Media.Business.V1_8.ChannelInfoProvider();
            Entities.Query.Media.ChannelQuery<Entities.DTO.V1_1.RespChannelListDto> query = new Entities.Query.Media.ChannelQuery<Entities.DTO.V1_1.RespChannelListDto>()
            {
                ChannelId = position.ChannelID,
                AdPosition1 = position.ADPosition1,
                AdPosition2 = position.ADPosition2,
                AdPosition3 = 7002,
                CooperateDate = aDDetailMode.LaunchTime.Date.ToString()
            };
            aDDetailMode.FinalCostPrice = ch.GetPolicyList(query);
            aDDetailMode.CostPrice = aDDetailMode.FinalCostPrice;
            Loger.Log4Net.Info($"计算销售成本价结果-----ChannelID：{position?.ChannelID},ADPosition1:{position?.ADPosition1},ADPosition2:{position?.ADPosition2},FinalCostPrice：{aDDetailMode.FinalCostPrice}");
        }
        #endregion
        #endregion
        #region 智投推荐
        //public List<ResponseIntelligenceRecommendDetailDto> IntelligenceRecommend(RequestIntelligenceRecommendDto reqDto)
        //{
        //    List<List<PubDetailCostPriceDto>> reqCostList = new List<List<PubDetailCostPriceDto>>();
        //    List<PubDetailCostPriceDto> pubCostList = new List<PubDetailCostPriceDto>();
        //    foreach (var area in reqDto.AreaInfo)
        //    {
        //        List<PubDetailCostPriceDto> areaCostList = new List<PubDetailCostPriceDto>();
        //        areaCostList = Dal.ADOrderInfo.Instance.GetPubDetailCostPrice(reqDto.LaunchTime, area.ProvinceID, area.CityID, area.MediaCount);
        //        if (areaCostList.Count > 0)
        //            pubCostList.AddRange(areaCostList);
        //    }

        //    var queryGroups = pubCostList.GroupBy(x => x.BaseMediaID);
        //    foreach (var queryGroup in queryGroups)
        //    {
        //        reqCostList.Add(queryGroup.ToList());
        //    }            

        //    Stack<PubDetailCostPriceDto> stack = new Stack<PubDetailCostPriceDto>();
        //    List<PubDetailIDResultDto> totalList = new List<PubDetailIDResultDto>();
        //    FindPubDetailByPrice(reqCostList, stack, ref totalList);
        //    var query = from t in totalList
        //                orderby t.AbsPrice ascending, t.OrderBy descending
        //                select t;
        //    var ret = query.Take(1);

        //    List<ResponseIntelligenceRecommendDetailDto> retDto = new List<ResponseIntelligenceRecommendDetailDto>();
        //    retDto = Dal.ADOrderInfo.Instance.GetRecommendDetail(ret.First().PublishDetailIDs);
        //    return null;
        //}
        public List<ResponseIntelligenceRecommendDto> IntelligenceRecommend(RequestIntelligenceRecommendDto reqDto)
        {
            List<ResponseIntelligenceRecommendDto> listRetDto = new List<ResponseIntelligenceRecommendDto>();
            int i = 0;
            foreach (var area in reqDto.AreaInfo)
            {
                Loger.Log4Net.Info($"智能推荐------{i++}省份：{area.ProvinceID}，城市：{area.CityID}");
                ResponseIntelligenceRecommendDto retDto = new ResponseIntelligenceRecommendDto();
                retDto = GetRecommendByAreaInfoNew(area, reqDto.LaunchTime, reqDto.OrderRemark, area.ProvinceID, area.CityID, area.MediaCount, area.Budget);
                if (retDto != null)
                    listRetDto.Add(retDto);
            }

            return listRetDto;
        }
        public ResponseIntelligenceRecommendDto GetRecommendByAreaInfo(RequestIntelligenceRecommendAreaInfoDto area, DateTime launchTime, string orderRemark, int provinceID = -2, int cityID = -2, int mediaCount = 3, decimal budget = 10000)
        {
            List<List<PubDetailCostPriceDto>> reqCostList = new List<List<PubDetailCostPriceDto>>();
            List<PubDetailCostPriceDto> pubCostList = new List<PubDetailCostPriceDto>();
            List<PubDetailCostPriceDto> areaCostList = new List<PubDetailCostPriceDto>();
            areaCostList = Dal.ADOrderInfo.Instance.GetPubDetailCostPrice(launchTime, orderRemark, area.ProvinceID, area.CityID, area.MediaCount, area.OriginContain);
            if (areaCostList.Count == 0)
                return null;

            Loger.Log4Net.Info($"智能推荐投放城市------查询智投推荐广告位成本价格结果JSON：{Newtonsoft.Json.JsonConvert.SerializeObject(areaCostList)}");
            pubCostList.AddRange(areaCostList);

            var queryGroups = pubCostList.GroupBy(x => x.BaseMediaID);
            foreach (var queryGroup in queryGroups)
            {
                reqCostList.Add(queryGroup.ToList());
            }
            Loger.Log4Net.Info($"智能推荐投放城市------按媒体ID分组结果JSON：{Newtonsoft.Json.JsonConvert.SerializeObject(reqCostList)}");
            Stack<PubDetailCostPriceDto> stack = new Stack<PubDetailCostPriceDto>();
            List<PubDetailIDResultDto> totalList = new List<PubDetailIDResultDto>();
            FindPubDetailByPrice(reqCostList, stack, ref totalList, budget);
            var query = from t in totalList
                        orderby t.AbsPrice ascending, t.OrderBy descending
                        select t;
            var ret = query.Take(1);

            Loger.Log4Net.Info($"智能推荐投放城市------最终价格组合计算结果JSON：{Newtonsoft.Json.JsonConvert.SerializeObject(ret)}");
            List<ResponseIntelligenceRecommendDetailDto> detailsDto = new List<ResponseIntelligenceRecommendDetailDto>();
            detailsDto = Dal.ADOrderInfo.Instance.GetRecommendDetail(ret.First().PublishDetailIDs, area.ProvinceID, area.CityID, area.OriginContain);

            if (detailsDto.Count == 0)
                return null;

            ResponseIntelligenceRecommendDto retDto = new ResponseIntelligenceRecommendDto()
            {
                ProvinceID = area.ProvinceID,
                CityID = area.CityID,
                ProvinceName = detailsDto.First().ProvinceName,
                CityName = detailsDto.First().CityName,
                PublishDetails = detailsDto
            };
            return retDto;
        }
        public void FindPubDetailByPrice(List<List<PubDetailCostPriceDto>> list, Stack<PubDetailCostPriceDto> stack, ref List<PubDetailIDResultDto> totalList, decimal budget = 10000)
        {
            if (stack.Count == list.Count)
            {
                //Console.WriteLine(string.Join(",", stack.Select(x => x.PublishDetailID.ToString() + ";" + x.CostReferencePrice.ToString()).ToArray()));
                //Console.WriteLine(string.Join(",", stack.Select(x => x.PublishDetailID.ToString()).ToArray()) + ";" + stack.Sum<PubDetailCostPriceDto>(x => x.CostReferencePrice));
                decimal tmpPrice = 0;
                tmpPrice = stack.Sum<PubDetailCostPriceDto>(x => x.CostReferencePrice);
                decimal tmpPrice2 = 0;
                tmpPrice2 = tmpPrice - budget;
                totalList.Add(new PubDetailIDResultDto()
                {
                    PublishDetailIDs = string.Join(",", stack.Select(x => x.PublishDetailID.ToString()).ToArray()),
                    TotalPrice = tmpPrice,
                    AbsPrice = Math.Abs(tmpPrice2),
                    OrderBy = tmpPrice2 > 0 ? 1 : 0
                });
            }
            else
            {
                List<PubDetailCostPriceDto> ints = list[stack.Count];
                foreach (var i in ints)
                {
                    stack.Push(i);
                    FindPubDetailByPrice(list, stack, ref totalList, budget);
                    stack.Pop();
                }
            }
        }
        public ResponseIntelligenceRecommendDto GetRecommendByAreaInfoNew(RequestIntelligenceRecommendAreaInfoDto area, DateTime launchTime, string orderRemark, int provinceID = -2, int cityID = -2, int mediaCount = 3, decimal budget = 10000)
        {
            List<List<PubDetailCostPriceDto>> reqCostList = new List<List<PubDetailCostPriceDto>>();
            List<PubDetailCostPriceDto> areaCostList = new List<PubDetailCostPriceDto>();
            areaCostList = Dal.ADOrderInfo.Instance.GetPubDetailCostPrice(launchTime, orderRemark, area.ProvinceID, area.CityID, area.MediaCount, area.OriginContain);
            if (areaCostList.Count == 0)
                return null;

            Loger.Log4Net.Info($"智能推荐投放城市------查询智投推荐广告位成本价格结果JSON：{Newtonsoft.Json.JsonConvert.SerializeObject(areaCostList)}");

            var queryGroups = areaCostList.GroupBy(x => x.BaseMediaID);
            foreach (var queryGroup in queryGroups)
            {
                reqCostList.Add(queryGroup.ToList());
            }

            Loger.Log4Net.Info($"智能推荐投放城市------媒体数：{queryGroups.Count()}，按媒体ID分组结果JSON：{Newtonsoft.Json.JsonConvert.SerializeObject(reqCostList)}");

            var findList = GetCombinationList(reqCostList, mediaCount > reqCostList.Count ? reqCostList.Count : mediaCount);
            Loger.Log4Net.Info($"智能推荐投放城市------按媒体数量组合结果记录数：{findList.Count}，JSON：{Newtonsoft.Json.JsonConvert.SerializeObject(findList)}");

            List<PubDetailIDResultDto> totalList = new List<PubDetailIDResultDto>();
            int i = 0;
            foreach (var itemFind in findList)
            {
                Loger.Log4Net.Info($"智能推荐投放城市------遍历组合结果：{i++}");
                Stack<PubDetailCostPriceDto> stack = new Stack<PubDetailCostPriceDto>();
                FindPubDetailByPrice(itemFind, stack, ref totalList, budget);
            }
            
            var query = from t in totalList
                        orderby t.AbsPrice ascending, t.OrderBy descending
                        select t;
            Loger.Log4Net.Info($"智能推荐投放城市------全部价格组合结果记录数：{totalList.Count}，JSON：{Newtonsoft.Json.JsonConvert.SerializeObject(query)}");
            var ret = query.Take(1);

            Loger.Log4Net.Info($"智能推荐投放城市------最终价格组合计算结果JSON：{Newtonsoft.Json.JsonConvert.SerializeObject(ret)}");
            List<ResponseIntelligenceRecommendDetailDto> detailsDto = new List<ResponseIntelligenceRecommendDetailDto>();
            detailsDto = Dal.ADOrderInfo.Instance.GetRecommendDetail(ret.First().PublishDetailIDs, area.ProvinceID, area.CityID, area.OriginContain);

            if (detailsDto.Count == 0)
                return null;

            ResponseIntelligenceRecommendDto retDto = new ResponseIntelligenceRecommendDto()
            {
                ProvinceID = area.ProvinceID,
                CityID = area.CityID,
                ProvinceName = detailsDto.First().ProvinceName,
                CityName = detailsDto.First().CityName,
                PublishDetails = detailsDto
            };
            return retDto;
        }
        /// <summary>
        /// 获得从n个不同元素中任意选取m个元素的组合的所有组合形式的列表
        /// </summary>
        /// <param name="elements">供组合选择的元素</param>
        /// <param name="m">组合中选取的元素个数</param>
        /// <returns>返回一个包含列表的列表，包含的每一个列表就是每一种组合可能</returns>
        public static List<List<List<T>>> GetCombinationList<T>(List<List<T>> elements, int m)
        {
            List<List<List<T>>> result = new List<List<List<T>>>();
            List<List<List<T>>> temp = null;
            List<List<T>> oneList = null;
            List<T> oneElment;
            List<List<T>> source = new List<List<T>>(elements);
            int n = 0;

            if (elements != null)
            {
                n = elements.Count;
            }
            if (n == m && m != 1)
            {
                result.Add(source);
                return result;
            }
            if (m == 1)
            {
                foreach (List<T> el in source)
                {
                    oneList = new List<List<T>>();
                    oneList.Add(el);
                    result.Add(oneList);
                    oneList = null;
                }
                return result;
            }

            for (int i = 0; i <= n - m; i++)
            {
                oneElment = source[0];
                source.RemoveAt(0);
                temp = GetCombinationList(source, m - 1);
                for (int j = 0; j < temp.Count; j++)
                {
                    oneList = new List<List<T>>();
                    oneList.Add(oneElment);
                    oneList.AddRange(temp[j]);
                    result.Add(oneList);
                    oneList = null;
                }
            }


            return result;
        }
        #endregion
        #region 根据项目号查看智投项目
        public IntelligenceADOrderInfoQueryDto IntelligenceADOrderInfoQuery(string orderID)
        {
            IntelligenceADOrderInfoQueryDto resDto = new IntelligenceADOrderInfoQueryDto();
            ResponseIntelligenceADOrderDto resADOrderInfo = new ResponseIntelligenceADOrderDto();
            List<ResponseIntelligenceADOrderAreaInfoDto> listAreainfo = new List<ResponseIntelligenceADOrderAreaInfoDto>();
            Dal.ADOrderInfo.Instance.IntelligenceADOrderInfoQuery_ByOrderID(orderID, out resADOrderInfo, out listAreainfo);

            foreach (var itemAreainfo in listAreainfo)
            {
                List<ResponseIntelligencePublishDetailDto> listPublishDetail = new List<ResponseIntelligencePublishDetailDto>();
                Dal.ADOrderInfo.Instance.IntelligenceADOrderInfoQuery_ByCityExtendID(itemAreainfo.CityExtendID, out listPublishDetail);
                itemAreainfo.PublishDetails = listPublishDetail;
            }
            try
            {
                //加密
                resADOrderInfo.CustomerID = XYAuto.Utils.Security.DESEncryptor.Encrypt(resADOrderInfo.CustomerIDINT.ToString(), LoginPwdKey);
                //编码
                resADOrderInfo.CustomerID = System.Web.HttpUtility.UrlEncode(resADOrderInfo.CustomerID, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("IntelligenceADOrderInfoQuery客户ID加密编码出错：->" + ex.Message);
            }
            resDto.ADOrderInfo = resADOrderInfo;
            resDto.AreaInfos = listAreainfo;
            return resDto;
        }
        #endregion
        #endregion
    }
}
