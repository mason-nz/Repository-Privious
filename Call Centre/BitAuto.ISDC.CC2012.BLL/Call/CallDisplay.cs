using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using System.Data.SqlClient;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CallDisplay
    {
        public static CallDisplay Instance = new CallDisplay();
        private static TelNumManage manage = null;
        public static TelNumManage Manage
        {
            get
            {
                if (manage == null)
                {
                    manage = CallDisplay.Instance.CreateTelNumManage();
                }
                return manage;
            }
        }

        /// 创建热线管理类
        /// <summary>
        /// 创建热线管理类
        /// </summary>
        /// <returns></returns>
        public TelNumManage CreateTelNumManage()
        {
            return Dal.CallDisplay.Instance.CreateTelNumManage();
        }
        /// 根据技能组获取热线
        /// <summary>
        /// 根据技能组获取热线
        /// </summary>
        /// <param name="ManufacturerSGID"></param>
        /// <returns></returns>
        public DataTable GetCallDisplayByManufacturerSGID(string ManufacturerSGID)
        {
            return Dal.CallDisplay.Instance.GetCallDisplayByManufacturerSGID(ManufacturerSGID);
        }


        /// 获取北京库变化的热线表数据
        /// <summary>
        /// 获取北京库变化的热线表数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetChangedCallDisplayFromBJ(long maxid)
        {
            return Dal.CallDisplay.Instance.GetChangedCallDisplayFromBJ(maxid);
        }
        /// 获取西安库热线表最大时间戳
        /// <summary>
        /// 获取西安库热线表最大时间戳
        /// </summary>
        /// <returns></returns>
        public long GetCallDisplayMaxTimeStamp_XA()
        {
            return Dal.CallDisplay.Instance.GetCallDisplayMaxTimeStamp_XA();
        }
        /// 批量入库西安库的临时CallDisplay表中
        /// <summary>
        /// 批量入库西安库的临时CallDisplay表中
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool BulkCopyToCallDisplayTemp_XA(DataTable dt, out string msg)
        {
            //清空数据表
            Dal.CallDisplay.Instance.ClearCallDisplayTemp_XA();
            //映射关系
            List<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
            list.Add(new SqlBulkCopyColumnMapping("CDID", "CDID"));
            list.Add(new SqlBulkCopyColumnMapping("CallNum", "CallNum"));
            list.Add(new SqlBulkCopyColumnMapping("OutCallNum", "OutCallNum"));
            list.Add(new SqlBulkCopyColumnMapping("Remark", "Remark"));
            list.Add(new SqlBulkCopyColumnMapping("TelMainNum", "TelMainNum"));
            list.Add(new SqlBulkCopyColumnMapping("HotlineID", "HotlineID"));
            list.Add(new SqlBulkCopyColumnMapping("WorkOrderDataSourceID", "WorkOrderDataSourceID"));
            list.Add(new SqlBulkCopyColumnMapping("AreaCode", "AreaCode"));
            list.Add(new SqlBulkCopyColumnMapping("Status", "Status"));
            list.Add(new SqlBulkCopyColumnMapping("OrderNum", "OrderNum"));
            list.Add(new SqlBulkCopyColumnMapping("CreateTime", "CreateTime"));
            list.Add(new SqlBulkCopyColumnMapping("MutilID", "MutilID"));
            list.Add(new SqlBulkCopyColumnMapping("long_TIMESTAMP", "TIMESTAMP"));

            //批量新增
            msg = "";
            Util.BulkCopyToDB(dt, Dal.BlackWhiteList.Instance.ConnCCBlackWhiteSynch, "CallDisplay_Temp", 10000, list, out msg);
            return true;
        }
        /// 从临时表更新正式表
        /// <summary>
        /// 从临时表更新正式表
        /// </summary>
        /// <returns></returns>
        public int[] UpdateCallDisplayFromTemp_XA()
        {
            return Dal.CallDisplay.Instance.UpdateCallDisplayFromTemp_XA();
        }
    }
}
