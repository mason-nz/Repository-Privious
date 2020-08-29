using System;
using System.Collections.Generic;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Constants
{
    public class Constant
    {
        /// <summary>
        /// 变量初始值
        /// </summary>
        public const int INT_INVALID_VALUE = -2;
        public const int INT_INVALID_VALUE_TOINT = -10000;
        public const decimal DECIMAL_INVALID_VALUE = -1;
        public const string STRING_INVALID_VALUE = null;
        public const string STRING_EMPTY_VALUE = "";
        public static readonly DateTime DATE_INVALID_VALUE = new DateTime(1900, 01, 01);


        /// <summary>
        /// UserSettings存储在Session中使用的Key
        /// </summary>
        public const string USER_SETTINGS = "UserSettings";

        //分页用的  常数  
        public static int PageSize = 10;

        //微信授权应用
        public const int ServiceTypeAdd = 35001;//微信号类型
        public const int VerifyTypeAdd = 36002;//认证类型
        public const int OauthIDAdd = 37000;//权限
        public const int ArticleTypeAdd = 6001;//图文类型



    }
}
