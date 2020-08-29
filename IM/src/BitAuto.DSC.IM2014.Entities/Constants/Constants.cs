using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.DSC.IM2014.Entities.Constants
{
    public class Constant
    {
        /// <summary>
        /// 变量初始值
        /// </summary>
        public const int INT_INVALID_VALUE = -2;
        public const decimal DECIMAL_INVALID_VALUE = -1;
        public const string STRING_INVALID_VALUE = null;
        public const string STRING_EMPTY_VALUE = "";
        public static readonly DateTime DATE_INVALID_VALUE = new DateTime(1900, 01, 01);


        ///// <summary>
        ///// UserSettings存储在Session中使用的Key
        ///// </summary>
        //public const string USER_SETTINGS = "UserSettings";

        //分页用的  常数  
        public static int PageSize = 10;


    }
}
