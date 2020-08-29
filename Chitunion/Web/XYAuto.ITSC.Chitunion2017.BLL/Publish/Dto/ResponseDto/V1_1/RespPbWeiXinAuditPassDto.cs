using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1
{
    public class RespPbDto
    {
        public int PubID { get; set; }
        public string PubName { get; set; }
        public int MediaID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string HeadIconURL { get; set; }

        //public string RejectMsg { get; set; }

        public int PublishStatus { get; set; }
    }

    public class RespPbWeiXinAuditPassDto : RespPbDto
    {
        public int Wx_Status { get; set; }//微信刊例状态
        public string Wx_StatusName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsAppointment { get; set; }//是否预约
        public int UserID { get; set; }
        public string TrueName { get; set; }
        public DateTime AuditDateTime { get; set; }//审核时间
    }
}