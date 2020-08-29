using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class UpdateUserStatusResDTO
    {
        public List<int> UserIDList { get; set; }
        public int Status { get; set; }
    }
}
