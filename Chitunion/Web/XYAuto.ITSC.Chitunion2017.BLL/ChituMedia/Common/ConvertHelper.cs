using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common
{
    public class ConvertHelper
    {
        #region 将对象变量转成字符串变量的方法
        /// <summary>
        /// 将对象变量转成字符串变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>字符串变量</returns>
        public static string ToString(object obj)
        {
            return (obj == null) ? string.Empty : obj.ToString();
        }
        #endregion

        #region 将对象变量转成32位整数型变量的方法
        /// <summary>
        /// 将对象变量转成32位整数型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>32位整数型变量</returns>
        public static int ToInt32(object obj)
        {
            int result;
            int.TryParse(ToString(obj), out result);
            return result;
        }
        #endregion

        #region 将对象变量转成64位整数型变量的方法
        /// <summary>
        /// 将对象变量转成64位整数型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>64位整数型变量</returns>
        public static long ToInt64(object obj)
        {
            long result;
            long.TryParse(ToString(obj), out result);
            return result;
        }
        #endregion

        #region 将对象变量转成双精度浮点型变量的方法
        /// <summary>
        /// 将对象变量转成双精度浮点型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>双精度浮点型变量</returns>
        public static double ToDouble(object obj)
        {
            double result;
            double.TryParse(ToString(obj), out result);
            return result;
        }
        #endregion

        #region 将对象转化为长整型的方法
        /// <summary>
        /// 将对象变量转化成长整型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>长整型变量</returns>
        public static long ToLong(object obj)
        {
            long result;
            long.TryParse(ToString(obj), out result);
            return result;
        }
        #endregion

        #region 将对象变量转成十进制数字变量的方法
        /// <summary>
        /// 将对象变量转成十进制数字变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>十进制数字变量</returns>
        public static decimal ToDecimal(object obj)
        {
            decimal result;
            decimal.TryParse(ToString(obj), out result);
            return result;
        }
        #endregion

        #region 将对象变量转成布尔型变量的方法
        /// <summary>
        /// 将对象变量转成布尔型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>布尔型变量</returns>
        public static bool ToBoolean(object obj)
        {
            bool result;
            bool.TryParse(ToString(obj), out result);
            return result;
        }
        #endregion

        #region 将对象变量转成日期型变量的方法
        /// <summary>
        /// 将对象变量转成日期型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>日期型变量</returns>
        public static DateTime ToDateTime(object obj)
        {
            DateTime result;
            DateTime.TryParse(ToString(obj), out result);
            return result;
        }
        #endregion

        #region 将对象变量转成可为null的日期型变量的方法
        /// <summary>
        /// 将对象变量转成可为null的日期型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>可为null的日期型变量</returns>
        public static DateTime? ToNullableDateTime(object obj)
        {
            DateTime result;
            DateTime.TryParse(ToString(obj), out result);
            if (result == DateTime.MinValue)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        #endregion


        #region "处理可空类型数据"

        /// <summary>
        /// 获取可空类型的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nullobject"></param>
        /// <returns></returns>
        public static object GetNullableValue(object nullobject)
        {
            if (nullobject == null) return DBNull.Value;

            Type t = nullobject.GetType();

            object obj = null;

            //var obj = nullobject; 
            if (t == typeof(System.Int32))
            {
                Nullable<int> objint = (Nullable<int>)nullobject;

                if (objint.HasValue)
                {
                    obj = objint.Value;
                    return obj;
                }
                else
                {
                    return DBNull.Value;
                }
            }
            if (t == typeof(System.Int16))
            {
                Nullable<short> objint = (Nullable<short>)nullobject;

                if (objint.HasValue)
                {
                    obj = objint.Value;
                    return obj;
                }
                else
                {
                    return DBNull.Value;
                }
            }

            if (t == typeof(System.Int64))
            {
                Nullable<double> objint = (Nullable<double>)nullobject;

                if (objint.HasValue)
                {
                    obj = objint.Value;
                    return obj;
                }
                else
                {
                    return DBNull.Value;
                }
            }
            //var obj = nullobject; 
            else if (t == typeof(System.DateTime))
            {
                Nullable<DateTime> objint = (Nullable<DateTime>)nullobject;

                if (objint.HasValue)
                {
                    obj = objint.Value;
                    return obj;
                }
                else
                {
                    return DBNull.Value;
                }
            }
            //var obj = nullobject; 
            else if (t == typeof(System.Boolean))
            {
                Nullable<Boolean> objint = (Nullable<Boolean>)nullobject;

                if (objint.HasValue)
                {
                    obj = objint.Value;
                    return obj;
                }
                else
                {
                    return DBNull.Value;
                }
            }

            else if (t == typeof(System.Byte))
            {
                Nullable<Byte> objint = (Nullable<Byte>)nullobject;

                if (objint.HasValue)
                {
                    obj = objint.Value;
                    return obj;
                }
                else
                {
                    return DBNull.Value;
                }
            }

            else if (t == typeof(System.Decimal))
            {
                Nullable<Decimal> objint = (Nullable<Decimal>)nullobject;

                if (objint.HasValue)
                {
                    obj = objint.Value;
                    return obj;
                }
                else
                {
                    return DBNull.Value;
                }
            }
            else if (t == typeof(System.String))
            {
                ;

                if (nullobject != null)
                {
                    return nullobject;
                }
                else
                {
                    return DBNull.Value;
                }
            }
            else
            {
                return DBNull.Value;
            }

        }
        #endregion 

        #region 根据分隔符将字符串变量转成字符串数组的方法
        /// <summary>
        /// 根据分隔符将字符串变量转成字符串数组的方法
        /// </summary>
        /// <param name="s">字符串变量</param>
        /// <param name="seperator">分隔符</param>
        /// <returns>字符串数组</returns>
        public static string[] ToStringArray(string s, char seperator)
        {
            return s.Split(seperator);
        }
        #endregion

        #region 根据分隔符将字符串变量转成32位整数型数组的方法
        /// <summary>
        /// 根据分隔符将字符串变量转成32位整数型数组的方法
        /// </summary>
        /// <param name="s">字符串变量</param>
        /// <param name="seperator">分隔符</param>
        /// <returns>32位整数型数组</returns>
        public static int[] ToInt32Array(string s, char seperator)
        {
            string[] sArray = ToStringArray(s, seperator);
            int[] iArray = new int[sArray.Length];
            for (int i = 0; i < sArray.Length; i++)
            {
                iArray[i] = ToInt32(sArray[i]);
            }
            return iArray;
        }
        #endregion

        #region 将数组变量转成逗号分割字符串的方法
        /// <summary>
        /// 将数组变量转成逗号分割字符串的方法
        /// </summary>
        /// <param name="nums">数组变量</param>
        /// <returns>逗号分割的字符串</returns>
        public static string ToArrayString(int[] nums)
        {
            string result = string.Empty;
            foreach (int num in nums)
            {
                result += num.ToString() + ",";
            }
            return result.TrimEnd(',');
        }
        /// <summary>
        /// 将数组变量转成逗号分割字符串的方法
        /// </summary>
        /// <param name="strs">数组变量</param>
        /// <returns>逗号分割的字符串</returns>
        public static string ToArrayString(string[] strs)
        {
            string result = string.Empty;
            foreach (string str in strs)
            {
                result += str + ",";
            }
            return result.TrimEnd(',');
        }
        #endregion

        #region 将字符串转成短整型变量的方法ToInt16(object obj)

        /// <summary>
        /// 将字符串转成短整型变量的方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>短整型变量</returns>
        public static short ToInt16(object obj)
        {
            short result;
            short.TryParse(ToString(obj), out result);
            return result;
        }
        #endregion

        #region 将对象转换为Byte数据类型ToByte(object obj)

        /// <summary>
        /// 将对象转换为Byte数据类型(8位无符号整数)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Byte ToByte(object obj)
        {
            Byte result;
            Byte.TryParse(ToString(obj), out result);
            return result;
        }

        #endregion

        #region 将对象转换为SByte数据类型ToSByte(object obj)

        /// <summary>
        /// 将对象转换为SByte数据类型(8位有符号整数)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static SByte ToSByte(object obj)
        {
            SByte result;
            SByte.TryParse(ToString(obj), out result);
            return result;
        }

        #endregion

        #region 将对象转换为Single数据类型ToSingle(object obj)

        /// <summary>
        /// 将对象转换为Single数据类型(单精度浮点数)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Single ToSingle(object obj)
        {
            Single result;
            Single.TryParse(ToString(obj), out result);
            return result;
        }

        #endregion

        #region 将对象变量转成单精度浮点型变量的方法
        /// <summary>
        /// 将对象变量转成单精度浮点型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>单精度浮点型变量</returns>
        public static float ToFloat(object obj)
        {
            float result;
            float.TryParse(ToString(obj), out result);
            return result;
        }
        #endregion             

        public static string ToNumCount(int Num)
        {
            if (Num > 10000)
            {

                return Math.Round((double)Num / 10000, 2) + "万";
            }
            return Num.ToString();
        }
        public static string ToLongNumCount(long Num)
        {
            if (Num > 10000)
            {

                return Math.Round((double)Num / 10000, 2) + "万";
            }
            return Num.ToString();
        }
        public static string ToPrice(decimal Num)
        {

            return String.Format("{0:N2}", Num) + "元";

        }
    }
}
