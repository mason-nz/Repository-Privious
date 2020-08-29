using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Dal;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class WOrderBusiType
    {
        public static WOrderBusiType Instance = new WOrderBusiType();


        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="status">是否可用,可以用“,”分割</param>
        /// <returns></returns>
        public DataTable GetAllData(string status)
        {
            return Dal.WOrderBusiType.Instance.GetAllData(new Entities.QueryWOrderBusiTypeInfo() { Status = status });
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="status">是否可用,可以用“,”分割</param>
        /// <returns></returns>
        public DataTable GetAllData(Entities.QueryWOrderBusiTypeInfo query)
        {
            return Dal.WOrderBusiType.Instance.GetAllData(query);
        }


        /// <summary>
        /// 最大排序号
        /// </summary>
        /// <returns></returns>
        public int GetMaxSortNum()
        {
            return Dal.WOrderBusiType.Instance.GetMaxSortNum();
        }

        /// <summary>
        /// 上移、下移
        /// </summary>
        /// <param name="CurrId"></param>
        /// <param name="CurrSort"></param>
        /// <param name="NextId"></param>
        /// <param name="NextSort"></param>
        public bool ChangeOrder(int CurrId, int CurrSort, int NextId, int NextSort)
        {
            int loginID = BLL.Util.GetLoginUserID();
            Entities.WOrderBusiTypeInfo entity = new WOrderBusiTypeInfo();
            entity.RecID = CurrId;
            entity.SortNum = NextSort;
            entity.LastUpdateTime = DateTime.Now;
            entity.LastUpdateUserID = loginID;

            Entities.WOrderBusiTypeInfo entity2 = new WOrderBusiTypeInfo();
            entity2.RecID = NextId;
            entity2.SortNum = CurrSort;
            entity2.LastUpdateTime = DateTime.Now;
            entity2.LastUpdateUserID = loginID;


            List<Entities.WOrderBusiTypeInfo> list = new List<WOrderBusiTypeInfo>();
            list.Add(entity);
            list.Add(entity2);
            return Dal.WOrderBusiType.Instance.UpdateTran(list);

        }
    }
}
