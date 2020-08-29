using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.Utils.Data;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CommonAttachment : DataBase
    {
        public static CommonAttachment Instance = new CommonAttachment();

        /// 查询某一个ID下的所有附件信息
        /// <summary>
        /// 查询某一个ID下的所有附件信息
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="dataid"></param>
        /// <returns></returns>
        public List<CommonAttachmentInfo> GetCommonAttachmentList(int btypeid, string relatedid)
        {
            string sql = "SELECT * FROM CommonAttachment WHERE BTypeID=" + btypeid + " AND RelatedID='" + relatedid + "' ORDER BY RecID";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            List<CommonAttachmentInfo> list = new List<CommonAttachmentInfo>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new CommonAttachmentInfo(dr));
                }
            }
            return list;
        }

        /// 查询某一个工单ID下的所有处理记录的附件信息
        /// <summary>
        /// 查询某一个工单ID下的所有处理记录的附件信息
        /// </summary>
        /// <param name="OrderID"></param>        
        /// <returns></returns>
        public DataTable GetAttachmentProcessListByOrderID(string OrderID, int StoragePathType)
        {
            string sql = @"SELECT RelatedID,BTypeID,FileName,FilePath 
            FROM CommonAttachment 
            WHERE  BTypeID=" + StoragePathType + @" 
            AND RelatedID IN (SELECT CAST(RecID AS VARCHAR) FROM  WOrderProcess WHERE OrderID='" + OrderID + "') ";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            return dt;
        }
    }
}
