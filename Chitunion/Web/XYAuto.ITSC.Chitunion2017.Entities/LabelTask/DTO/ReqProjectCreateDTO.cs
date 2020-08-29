using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.LabelTask.DTO
{
    public class ReqProjectCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public int ProjectType { get; set; } = -2;
        public int TaskCount { get; set; } = 0;
        public string UploadFileURL { get; set; } = string.Empty;
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (string.IsNullOrEmpty(Name))
                sb.Append($"请输入项目名称!");

            if(Name.Length>40)
                sb.Append($"项目名称最多40个字符!");

            if(string.IsNullOrEmpty(UploadFileURL))
                sb.Append($"请上传账号文件!");

            if(TaskCount<=0 || TaskCount>10000)
                sb.Append($"任务数量必须>0且<=10000!");

            if (ProjectType == (int)Entities.LabelTask.ENUM.EnumProjectType.文章)
            {
                
            }
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
