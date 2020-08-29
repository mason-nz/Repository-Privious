using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.LabelTask;

namespace XYAuto.ITSC.Chitunion2017.BLL.LabelTask
{
    public class LB_Project
    {
        public static readonly LB_Project Instance = new LB_Project();

        /// <summary>
        /// 查询标签项目列表
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public Dictionary<string, object> SelectProjectList(int PageIndex, int pageSize)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("TotalCount", 0);

            DataTable dt = Dal.LabelTask.LB_Project.Instance.SelectProjectList(PageIndex, pageSize);
            List<ProjectInfo> ProList;
            if (dt != null && dt.Rows.Count > 0)
            {
                dic["TotalCount"] = dt.Columns["TotalCount"].Expression;
                ProList = Common.Util.DataTableToList<ProjectInfo>(dt);
            }
            else
            {
                ProList = new List<ProjectInfo>();
            }
            dic.Add("ProjectInfo", ProList);
            return dic;
        }
        public ProjectInfo SelectProjectInfo(int projectID)
        {
            DataTable dt = Dal.LabelTask.LB_Project.Instance.SelectProjectInfo(projectID);
            List<ProjectInfo> proList = Common.Util.DataTableToList<ProjectInfo>(dt);
            if (proList != null && proList.Count > 0)
            {
                string uploadFileURL = proList[0].UploadFileURL;

                if (uploadFileURL.Contains("/"))
                {
                    string fileName1 = uploadFileURL.Substring(uploadFileURL.LastIndexOf('/') + 1);
                    if (fileName1.Contains("$"))
                    {
                        proList[0].UploadFileName = fileName1.Substring(0, fileName1.LastIndexOf('$')) + "." + fileName1.Substring(fileName1.LastIndexOf('.') + 1);
                    }
                }
                return proList[0];
            }
            else
            {
                return new ProjectInfo();
            }
        }
    }
}
