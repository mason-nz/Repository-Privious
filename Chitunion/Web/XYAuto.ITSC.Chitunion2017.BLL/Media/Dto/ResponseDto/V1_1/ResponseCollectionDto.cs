using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto.V1_1
{
    public class ResponseCollectionDto
    {
        public int MediaId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string HeadIconURL { get; set; }
        public int FansCount { get; set; }
        public int PublishStatus { get; set; }
        public int Status { get; set; }
        public int MaxinumReading { get; set; }//最高阅读数
        public int ReferReadCount { get; set; }//参考阅读数
        public string CommonlyClass { get; set; }
        public decimal Price { get; set; }
    }
}