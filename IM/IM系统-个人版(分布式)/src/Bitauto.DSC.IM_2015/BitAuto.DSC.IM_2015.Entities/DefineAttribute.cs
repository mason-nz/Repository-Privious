using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;

namespace BitAuto.DSC.IM_2015.Entities
{
    /// 自定义属性：数据库表
    /// <summary>
    /// 自定义属性：数据库表
    /// 强斐
    /// 2014-10-23
    /// </summary>
    public class DBTableAttribute : Attribute
    {
        public string TableName { get; set; }
        public DBTableAttribute(string name)
        {
            TableName = name;
        }
    }

    /// 自定义属性：字段
    /// <summary>
    /// 自定义属性：字段
    /// 强斐
    /// 2014-10-23
    /// </summary>
    public class DBFieldAttribute : Attribute
    {
        /// 数据库字段名称
        /// <summary>
        /// 数据库字段名称
        /// </summary>
        public string DBField { get; set; }
        /// 字段类型
        /// <summary>
        /// 字段类型
        /// </summary>
        public SqlDbType DBType { get; set; }
        /// 长度
        /// <summary>
        /// 长度
        /// </summary>
        public int DBLength { get; set; }
        /// 是否主键
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool DBKey { get; set; }
        /// 是否自增
        /// <summary>
        /// 是否自增
        /// </summary>
        public bool IDENTITY { get; set; }

        /// 属性的值
        /// <summary>
        /// 属性的值
        /// </summary>
        public object Value { get; set; }
        /// 关联属性
        /// <summary>
        /// 关联属性
        /// </summary>
        public PropertyInfo ProInfo { get; set; }

        /// 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="DBField"></param>
        /// <param name="DBType"></param>
        /// <param name="DBLength"></param>
        /// <param name="DBKey"></param>
        public DBFieldAttribute(string DBField, SqlDbType DBType, int DBLength = 0, bool DBKey = false, bool IDENTITY = false)
        {
            this.DBField = DBField;
            this.DBType = DBType;
            this.DBLength = DBLength;
            this.DBKey = DBKey;
            this.IDENTITY = IDENTITY;
        }
    }

    /// 自定义属性：字段是否被修改的属性标示
    /// <summary>
    /// 自定义属性：字段是否被修改的属性标示
    /// 强斐
    /// 2014-10-23
    /// </summary>
    public class IsModifyAttribute : Attribute
    {
        /// 数据库字段名称
        /// <summary>
        /// 数据库字段名称
        /// </summary>
        public string DBField { get; set; }
        /// 属性的值
        /// <summary>
        /// 属性的值
        /// </summary>
        public bool Value { get; set; }

        /// 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="DBField"></param>
        public IsModifyAttribute(string DBField)
        {
            this.DBField = DBField;
        }
    }

    /// 自定义属性辅助类
    /// <summary>
    /// 自定义属性辅助类
    /// 强斐
    /// 2014-10-23
    /// </summary>
    public class AttributeHelper
    {
        /// 获取所有的字段属性
        /// <summary>
        /// 获取所有的字段属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        private static Dictionary<string, DBFieldAttribute> GetDBFieldAttributes<T>(T info)
        {
            Dictionary<string, DBFieldAttribute> list = new Dictionary<string, DBFieldAttribute>();
            foreach (PropertyInfo proInfo in info.GetType().GetProperties())
            {
                object[] attrs = proInfo.GetCustomAttributes(typeof(DBFieldAttribute), true);
                if (attrs.Length == 1)
                {
                    DBFieldAttribute attr = (DBFieldAttribute)attrs[0];
                    if (attr != null)
                    {
                        attr.Value = proInfo.GetValue(info, null);
                        attr.ProInfo = proInfo;
                        list.Add(attr.DBField, attr);
                    }
                }
            }
            return list;
        }
        /// 获取所有的是否修改属性
        /// <summary>
        /// 获取所有的是否修改属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        private static Dictionary<string, IsModifyAttribute> GetIsModifyAttributes<T>(T info)
        {
            Dictionary<string, IsModifyAttribute> list = new Dictionary<string, IsModifyAttribute>();
            foreach (PropertyInfo proInfo in info.GetType().GetProperties())
            {
                object[] attrs = proInfo.GetCustomAttributes(typeof(IsModifyAttribute), true);
                if (attrs.Length == 1)
                {
                    IsModifyAttribute attr = (IsModifyAttribute)attrs[0];
                    if (attr != null)
                    {
                        attr.Value = (bool)proInfo.GetValue(info, null);
                        list.Add(attr.DBField, attr);
                    }
                }
            }
            return list;
        }
        /// 获取数据库表名
        /// <summary>
        /// 获取数据库表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        private static string GetTableName<T>(T info)
        {
            object[] attrs = info.GetType().GetCustomAttributes(typeof(DBTableAttribute), true);
            if (attrs.Length == 1)
            {
                DBTableAttribute attr = (DBTableAttribute)attrs[0];
                if (attr != null)
                {
                    return attr.TableName;
                }
            }
            return null;
        }
        /// 创建sql参数
        /// <summary>
        /// 创建sql参数
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        private static SqlParameter CreateSqlParameter(DBFieldAttribute attr)
        {
            if (attr.DBLength == 0)
            {
                return new SqlParameter("@" + attr.DBField, attr.DBType) { Value = attr.Value };
            }
            else
            {
                return new SqlParameter("@" + attr.DBField, attr.DBType, attr.DBLength) { Value = attr.Value };
            }
        }

        /// 返回Insert参数
        /// <summary>
        /// 返回Insert参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlParameter[] GetInsertSqlParameter<T>(T info, out string sql)
        {
            string tablename = GetTableName(info);
            List<SqlParameter> paras = new List<SqlParameter>();
            sql = "Insert into " + tablename + " ( {0} ) values ( {1} )";

            Dictionary<string, DBFieldAttribute> dic1 = GetDBFieldAttributes(info);
            string str1 = "";
            string str2 = "";
            foreach (string key in dic1.Keys)
            {
                if (dic1[key].IDENTITY == false)
                {
                    paras.Add(CreateSqlParameter(dic1[key]));
                    str1 += key + ",";
                    str2 += "@" + key + ",";
                }
            }

            if (str1 == "" && str2 == "")
            {
                return null;
            }
            sql = string.Format(sql, str1.TrimEnd(','), str2.TrimEnd(','));
            return paras.ToArray();
        }
        /// 返回Update参数
        /// <summary>
        /// 返回Update参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlParameter[] GetUpdateSqlParameter<T>(T info, out string sql)
        {
            string tablename = GetTableName(info);
            List<SqlParameter> paras = new List<SqlParameter>();
            sql = "update " + tablename + " set {0} where {1}";

            string set = "";
            string where = "";
            Dictionary<string, DBFieldAttribute> dic1 = GetDBFieldAttributes(info);
            Dictionary<string, IsModifyAttribute> dic2 = GetIsModifyAttributes(info);
            foreach (string key in dic1.Keys)
            {
                //是否是主键
                if (dic1[key].DBKey)
                {
                    where += key + "=@" + key + " and ";
                    paras.Add(CreateSqlParameter(dic1[key]));
                }
                else
                {
                    //值是否被修改，修改才更新
                    if (dic2.ContainsKey(key) && dic2[key].Value)
                    {
                        set += key + "=@" + key + ",";
                        paras.Add(CreateSqlParameter(dic1[key]));
                    }
                }
            }

            if (set.Length > 0)
            {
                set = set.TrimEnd(',');
            }
            if (where.Length > 0)
            {
                where = where.Remove(where.Length - 4);
            }
            if (set == "" || where == "")
            {
                return null;
            }
            sql = string.Format(sql, set, where);
            return paras.ToArray();
        }
        /// 返回Delete参数
        /// <summary>
        /// 返回Delete参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlParameter[] GetDeleteSqlParameter<T>(T info, out string sql)
        {
            string tablename = GetTableName(info);
            List<SqlParameter> paras = new List<SqlParameter>();
            sql = "delete " + tablename + " where {0}";

            string where = "";
            Dictionary<string, DBFieldAttribute> dic1 = GetDBFieldAttributes(info);
            foreach (string key in dic1.Keys)
            {
                //是否是主键
                if (dic1[key].DBKey)
                {
                    where += key + "=@" + key + " and ";
                    paras.Add(CreateSqlParameter(dic1[key]));
                }
            }

            if (where.Length > 0)
            {
                where = where.Remove(where.Length - 4);
            }
            if (where == "")
            {
                return null;
            }
            sql = string.Format(sql, where);
            return paras.ToArray();
        }
        /// 返回Select参数
        /// <summary>
        /// 返回Select参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlParameter[] GetSelectSqlParameter<T>(T info, out string sql)
        {
            string tablename = GetTableName(info);
            List<SqlParameter> paras = new List<SqlParameter>();
            sql = "select * from " + tablename + " where {0}";

            string where = "";
            Dictionary<string, DBFieldAttribute> dic1 = GetDBFieldAttributes(info);
            foreach (string key in dic1.Keys)
            {
                //是否是主键
                if (dic1[key].DBKey)
                {
                    where += key + "=@" + key + " and ";
                    paras.Add(CreateSqlParameter(dic1[key]));
                }
            }

            if (where.Length > 0)
            {
                where = where.Remove(where.Length - 4);
            }
            if (where == "")
            {
                return null;
            }
            sql = string.Format(sql, where);
            return paras.ToArray();
        }
        /// 设置自增主键
        /// <summary>
        /// 设置自增主键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="id"></param>
        public static void SetIDENTITY<T>(T info, int id)
        {
            Dictionary<string, DBFieldAttribute> dic = GetDBFieldAttributes(info);
            foreach (string key in dic.Keys)
            {
                if (dic[key].IDENTITY)
                {
                    dic[key].ProInfo.SetValue(info, id, null);
                    break;
                }
            }
        }
    }
}
