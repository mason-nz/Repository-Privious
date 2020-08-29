using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BitAuto.DSC.APPReport2016.BLL
{
   public  class DictInfo
    {
       public static DictInfo Instance = new DictInfo();

         /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <param name="dictIds"></param>
        /// <param name="dictType"></param>
        /// <returns></returns>
       public DataTable GetData(string dictIds = "", string dictType = "")
       {
           return Dal.DictInfo.Instance.GetData(dictIds, dictType);
       }
    }
}
