using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Dal;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CommonAttachment
    {
        public static CommonAttachment Instance = new CommonAttachment();

        /// 查询某一个ID下的所有附件信息
        /// <summary>
        /// 查询某一个ID下的所有附件信息
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="dataid"></param>
        /// <returns></returns>
        public List<CommonAttachmentInfo> GetCommonAttachmentList(BitAuto.ISDC.CC2012.BLL.Util.ProjectTypePath projectid, string dataid)
        {
            List<CommonAttachmentInfo> list = Dal.CommonAttachment.Instance.GetCommonAttachmentList((int)projectid, dataid);
            if (list != null)
            {
                foreach (CommonAttachmentInfo item in list)
                {
                    //对FilePath进行完整化赋值
                    item.FilePath = "/upload/" + BLL.Util.GetUploadProject((BitAuto.ISDC.CC2012.BLL.Util.ProjectTypePath)item.BTypeID, "") + item.FilePath;
                    // 将缩略图文件路径存入model.SmallFilePath中
                    string[] smallFileAllPathArr = item.FilePath.Split('.');
                    item.SmallFilePath = smallFileAllPathArr[0] + "_small." + smallFileAllPathArr[1];
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
        public List<CommonAttachmentInfo> GetAttachmentProcessListByOrderID(string OrderID, int StoragePathType)
        {
            DataTable dt = Dal.CommonAttachment.Instance.GetAttachmentProcessListByOrderID(OrderID, StoragePathType);
            List<CommonAttachmentInfo> list = new List<CommonAttachmentInfo>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CommonAttachmentInfo model = new CommonAttachmentInfo(dr);
                    //对FilePath进行完整化赋值
                    model.FilePath = "/upload/" + BLL.Util.GetUploadProject((BitAuto.ISDC.CC2012.BLL.Util.ProjectTypePath)model.BTypeID, "") + model.FilePath;
                    // 将缩略图文件路径存入model.SmallFilePath中
                    string[] smallFileAllPathArr = model.FilePath.Split('.');
                    model.SmallFilePath = smallFileAllPathArr[0] + "_small." + smallFileAllPathArr[1];
                    list.Add(model);
                }
            }
            return list;
        }
    }
}
