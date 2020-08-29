using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto.V1_1
{
    public class ResponsePullBackDto
    {
        public int MediaId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string HeadIconURL { get; set; }
        public int FansCount { get; set; }
        public int PublishStatus { get; set; }
        public int Status { get; set; }
    }
}