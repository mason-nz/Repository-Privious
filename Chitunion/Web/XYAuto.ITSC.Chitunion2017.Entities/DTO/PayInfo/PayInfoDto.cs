using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.PayInfo
{
    public class PayInfoDto
    {
        public bool IsAdd { get; set; }
        public string OldAccountName { get; set; }
        public int OldAccountType { get; set; }
        public string AccountName { get; set; }
        public int AccountType { get; set; }

    }
}
