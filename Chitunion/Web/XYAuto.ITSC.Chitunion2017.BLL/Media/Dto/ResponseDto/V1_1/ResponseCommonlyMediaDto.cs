using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto.V1_1
{
    public class ResponseCommonlyMediaDto
    {
        public int MediaId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public int FansCount { get; set; }
        public decimal Price { get; set; }
    }
}