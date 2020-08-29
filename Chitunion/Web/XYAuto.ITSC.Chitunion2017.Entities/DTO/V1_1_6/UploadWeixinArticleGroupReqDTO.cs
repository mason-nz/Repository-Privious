using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6
{
    public class UploadWeixinArticleGroupReqDTO
    {
        public int GroupID { get; set; }
        public List<int> UploadWxIDs { get; set; }
    }
}
