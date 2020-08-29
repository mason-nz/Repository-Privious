using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.Materiel.DTO
{
    public class ResGetDivideUserDTO
    {
        public List<RestDivideUserDTO> List { get; set; }
    }
    public class RestDivideUserDTO
    {
        public int UserId { get; set; } = -2;
        public string UserName { get; set; } = string.Empty;
    }
}
