/********************************************************
*创建人：hant
*创建时间：2017/12/21 17:55:20 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XYAuto.BUOC.ChiTuData2017.BLL.Chitu.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.BLL.Chitu.Dto.Response;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;
using XYAuto.ChiTu.Excel;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Chitu
{
    public class Order
    {

        private const string UpladFilesPath = "/UploadFiles/";
        private readonly string _uploadFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);
        private readonly string _todayDate = DateTime.Now.ToString("yyyy-MM-dd");

        public static readonly Order Instance = new Order();

        /// <summary>
        /// 返回订单列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResponseOrderList GetOrderList(RequestOrder req)
        {
            //if (RolesVerification.Instance.IsAllData() && !(RolesVerification.Instance.IsViewData()))
            //{
            //    req.ChannelID = BLL.TaskInfo.AppInfo.Instance.GeChannelIDByUserId(XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID);
            //}
            ResponseOrderList orderlist = new ResponseOrderList();
            List<ResponseOrder> list = new List<ResponseOrder>();
            int totalCount = 0;
            DataSet ds = GetOrder(req, out totalCount);
            orderlist.TotalCount = totalCount;
            if(ds != null &&ds.Tables.Count>0 &&ds.Tables[0].Rows.Count>0)
            {
                list = DataTableToList<ResponseOrder>(ds.Tables[0]);
            }
            orderlist.List = list;
            return orderlist;
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ResponseOrderDetial GetOrderDetial(int orderid)
        {
            int userid = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
            
            ResponseOrderDetial model = new ResponseOrderDetial();
            List<AccountBalance> list = new List<AccountBalance>();
            DataSet dsOrder = GetOrderByOrderId(orderid);
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count>0)
            {
                if (RolesVerification.Instance.IsAllData())
                {
                    model = GetOrder(orderid, ref list, dsOrder);
                }
                else
                {
                    int channelid = BLL.TaskInfo.AppInfo.Instance.GeChannelIDByUserId(userid);

                    if (RolesVerification.Instance.IsViewData() && channelid == Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ChannelID"]))
                    {
                        model = GetOrder(orderid, ref list, dsOrder);
                    }
                }
            }
            model.List = list;
            return model;
        }

        private ResponseOrderDetial GetOrder(int orderid, ref List<AccountBalance> list, DataSet dsOrder)
        {
            ResponseOrderDetial model = DataTableToEntity<ResponseOrderDetial>(dsOrder.Tables[0]);
            DataSet dsBalance = GetOrderBalanceByOrderId(orderid);
            if (dsBalance != null && dsBalance.Tables.Count > 0 && dsBalance.Tables[0].Rows.Count > 0)
            {
                list = DataTableToList<AccountBalance>(dsBalance.Tables[0]);
            }

            return model;
        }

        public string Export(RequestOrder req)
        {
            //if (RolesVerification.Instance.IsAllData() && !(RolesVerification.Instance.IsViewData()))
            //{
            //    req.ChannelID = BLL.TaskInfo.AppInfo.Instance.GeChannelIDByUserId(XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID);
            //}
            //{Guid.NewGuid()}
            var fileName = $"渠道订单下载-订单列表.xlsx";
            var dicFilePath = GetStatPath(fileName);

            List<ResponseOrder> itemorder = new List<ResponseOrder>();
            List<ResponseOrderChannel> itemchannel = new List<ResponseOrderChannel>();
            if (RolesVerification.Instance.IsAllData())
            {
                itemorder = DataTableToList<ResponseOrder>(GetOrderExcel(req).Tables[0]);
                if (itemorder.Count > 0)
                {
                    new ExcelHelper<ResponseOrder>().SaveExcelToFile(itemorder, dicFilePath.Item1, fileName);
                }
                else
                {
                    return "暂无数据";
                }

            }
            else
            {
                itemchannel = DataTableToList<ResponseOrderChannel>(GetOrderExcel(req).Tables[0]);
                if (itemchannel.Count > 0)
                {
                    new ExcelHelper<ResponseOrderChannel>().SaveExcelToFile(itemchannel, dicFilePath.Item1, fileName);
                }
                else
                {
                    return "暂无数据";
                }

            }
            var httpUrl = dicFilePath.Item2;//前端http url地址
                                            // return dicFilePath.Item1 + @"\" + fileName;
            return httpUrl;

        }

        /// <summary>
        /// 生成文件名,item1:物理path ,item2:生成后的url路径
        /// </summary>
        /// <param name="fileName">需要定义的文件名称：111.png</param>
        /// <param name="statisticsDataTypeEnum"></param>
        /// <returns>item1:物理path ,item2:生成后的url路径</returns>
        public Tuple<string, string> GetStatPath(string fileName)
        {
            //UploadLoad
            string relatedPath = $"{UpladFilesPath}ExportExcel/Order/{_todayDate}/";
            var webFilePath = relatedPath + fileName;
            string dir = _uploadFilePath + relatedPath.Replace(@"/", "\\");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return new Tuple<string, string>(dir, webFilePath);
        }

        private DataSet GetOrderExcel(RequestOrder req)
        {
            string where = string.Empty;
            string swhere = OrderWhere(req,ref where);
            DataSet ds = Dal.Chitu.Order.Instance.GetOrderExcel(swhere,where);
            return ds;
        }

        /// <summary>
        /// 订单分页
        /// </summary>
        /// <param name="req"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        private DataSet GetOrder(RequestOrder req, out int totalCount)
        {
            string abwhere = string.Empty;
            string swhere = OrderWhere(req,ref abwhere);
            return Dal.Chitu.Order.Instance.GetOrder(swhere, abwhere, req.PageIndex, req.PageSize, out totalCount);
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private static string OrderWhere(RequestOrder req,ref string abwhere)
        {
           
            StringBuilder where = new StringBuilder();
            StringBuilder swhere = new StringBuilder();
            if (req.BeginTime != null && req.BeginTime.ToString().Length > 0)
            {
                if (req.Status == 193001)
                {
                    swhere.AppendFormat(" AND OI.CreateTime>='{0}'", req.BeginTime);
                }
                else
                {
                    swhere.AppendFormat(" AND OI.EndTime>='{0}'", req.BeginTime);
                }
                
                where.AppendFormat(" AND StatisticsTime>='{0}'", Convert.ToDateTime(req.BeginTime).AddDays(-15));
            }
            if (req.EndTime != null && req.EndTime.ToString().Length > 0)
            {
                if (req.Status == 193001)
                {
                    swhere.AppendFormat(" AND OI.CreateTime<'{0}'", Convert.ToDateTime(req.EndTime).AddDays(1));
                }
                else
                {
                    swhere.AppendFormat(" AND OI.EndTime<='{0}'", req.EndTime);
                }
                where.AppendFormat(" AND StatisticsTime<='{0}'",Convert.ToDateTime(req.EndTime).AddDays(20));
            }
            if (req.Status > 0)
            {
                swhere.AppendFormat(" AND OI.Status={0}", req.Status);
            }
            if (req.OrderType > 0)
            {
                swhere.AppendFormat(" AND OrderType={0}", req.OrderType);
            }
            if (req.ChannelID > 0)
            {
                swhere.AppendFormat(" AND OI.ChannelID={0}", req.ChannelID);
            }
            if (req.OrderID > 0)
            {
                swhere.AppendFormat(" AND OI.RecId={0}", req.OrderID);
            }
            if (req.TaskID > 0)
            {
                swhere.AppendFormat(" AND OI.TaskID={0}", req.TaskID);
            }
            abwhere = where.ToString();
            return swhere.ToString();
        }

        /// <summary>
        /// 订单详细信息
        /// </summary>
        /// <param name="orderid">订单ID</param>
        /// <returns></returns>
        private DataSet GetOrderByOrderId(int orderid)
        {
            return Dal.Chitu.Order.Instance.GetOrderByOrderId(orderid);
        }

        /// <summary>
        /// 订单统计信息
        /// </summary>
        /// <param name="orderid">订单ID</param>
        /// <returns></returns>
        private DataSet GetOrderBalanceByOrderId(int orderid)
        {
            return Dal.Chitu.Order.Instance.GetOrderBalanceByOrderId(orderid);
        }

        private List<T> DataTableToList<T>(DataTable table) //where T : EntityBase, new()
        {
            List<T> list = new List<T>();
            if (table != null && table.Rows != null && table.Rows.Count > 0)
            {
                for (var i = 0; i < table.Rows.Count; i++)
                {
                    //创建泛型对象
                    T entity = Activator.CreateInstance<T>();
                    //属性和名称相同时则赋值
                    for (var j = 0; j < table.Columns.Count; j++)
                    {
                        var property = entity.GetType().GetProperty(table.Columns[j].ColumnName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                        if (property != null && table.Rows[i][j] != DBNull.Value)
                        {
                            property.SetValue(entity, table.Rows[i][j], null);
                        }
                    }
                    list.Add(entity);
                }
            }
            return list;
        }


        private T DataTableToEntity<T>(DataTable table) //where T : EntityBase, new()
        {
            var entity = Activator.CreateInstance<T>();
            if (table.Rows.Count == 0)
                return default(T);
            for (var i = 0; i < table.Columns.Count; i++)
            {
                //var property = entity.GetType().GetProperty(table.Columns[i].ColumnName);
                var property = entity.GetType().GetProperty(table.Columns[i].ColumnName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);

                if (property != null && table.Rows[0][i] != DBNull.Value)
                {
                    property.SetValue(entity, table.Rows[0][i], null);
                }
            }
            return entity;
        }

    }
}
