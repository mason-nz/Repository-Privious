using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.DictInfo
{
    public class DictInfo
    {
        public readonly static DictInfo Instance = new DictInfo();

        public string GetSharingPlatform(int type)
        {
            var dt = Dal.DictInfo.DictInfo.Instance.GetSharingPlatform(type);
            var listId = new List<int>();
            foreach (DataRow row in dt.Rows)
            {
                if (type == 108)
                    listId.Add(Convert.ToInt32(row["Id"].ToString()));
                else
                    listId.Add(Convert.ToInt32(row["Id"].ToString()) - type*1000);
            }
            return string.Join("", listId);
        }
    }
}
