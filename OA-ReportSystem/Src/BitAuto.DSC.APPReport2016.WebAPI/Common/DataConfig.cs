using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.DSC.APPReport2016.WebAPI.Common
{
    public class DataConfig
    {
        #region 会员类型

        public static string MemberTypeCode = "9";
        //车易通
        public static string CytCode = "90001";
        //车盟通
        public static string CmtCode = "90002";
        //微信通
        public static string WxtCode = "90003";

        //全部会员类型
        public static string MemberTypeAll = CytCode + "," + CmtCode + "," + WxtCode;

        #endregion


        #region 人员
        //年龄
        public static string EmpolyeeNLCode = "3";
        //司龄
        public static string EmployeeSLCode = "4";
        //职级
        public static string EmployeeZJCode = "5";
        //职位
        public static string EmpolyeeZWCode = "6";

        #endregion

        /// <summary>
        /// 季度
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetYearQuarter(int year)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Q1", year + "01_" + year + "03");
            dic.Add("Q2", year + "04_" + year + "06");
            dic.Add("Q3", year + "07_" + year + "09");
            dic.Add("Q4", year + "10_" + year + "12");

            return dic;

        }
    }
}