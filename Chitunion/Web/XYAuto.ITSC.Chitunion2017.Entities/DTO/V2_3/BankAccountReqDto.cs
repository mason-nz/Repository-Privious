using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V2_3
{
    public class BankAccountReqDto
    {
        public int AccountType { get; set; }
        public string OldAccountName { get; set; }
        public string NewAccountName { get; set; }
    }
}
