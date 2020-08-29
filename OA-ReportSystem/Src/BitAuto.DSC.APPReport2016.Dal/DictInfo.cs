using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.Utils.Data;

namespace BitAuto.DSC.APPReport2016.Dal
{
    public class DictInfo
    {
        public static DictInfo Instance = new DictInfo();

        /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <param name="dictIds"></param>
        /// <param name="dictType"></param>
        /// <returns></returns>
        public DataTable GetData(string dictIds="", string dictType="")
        {
            string sql = "SELECT *  FROM DictInfo where Status=0 ";
            if (!string.IsNullOrEmpty(dictIds))
            {
                sql += " and DictId in (" + dictIds + ") ";
            }
            if (!string.IsNullOrEmpty(dictType))
            {
                sql += " and DictType='" + dictType + "'";
            }

            sql += " order by ordernum";
            return SqlHelper.ExecuteDataset(DataBase.ConnectionStrings,CommandType.Text,sql).Tables[0];

        }
    }
}
