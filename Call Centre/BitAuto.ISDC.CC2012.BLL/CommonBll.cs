using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Dal;
using System.Data.SqlClient;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
    /// 可以单独使用，也可以继承使用
    /// <summary>
    /// 可以单独使用，也可以继承使用
    /// </summary>
    public class CommonBll
    {
        private static CommonBll instance = null;
        public static CommonBll Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommonBll();
                }
                return instance;
            }
        }

        public static string Default_Conn
        {
            get
            {
                return CommonDal.Default_Conn;
            }
        }
        public static string CRM_Conn
        {
            get
            {
                return CommonDal.CRM_Conn;
            }
        }
        public static string SYS_Conn
        {
            get
            {
                return CommonDal.SYS_Conn;
            }
        }
        public static string CC_conn
        {
            get
            {
                return CommonDal.Default_Conn;
            }
        }

        #region 通用实体类增删改查BLL（适用[ADO自动生成工具]生成的实体类）
        /// 新增 (自动生成)
        /// <summary>
        /// 新增 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool InsertComAdoInfo<T>(T info)
        {
            return CommonDal.Instance.InsertComAdoInfo<T>(info);
        }
        /// 修改 (自动生成)
        /// <summary>
        /// 修改 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool UpdateComAdoInfo<T>(T info)
        {
            return CommonDal.Instance.UpdateComAdoInfo<T>(info);
        }
        /// 删除 (自动生成)
        /// <summary>
        /// 删除 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs">主键序列值（和实体类中构造方法中的主键参数顺序一致）</param>
        /// <returns></returns>
        public bool DeleteComAdoInfo<T>(params object[] objs)
        {
            T info = (T)Activator.CreateInstance(typeof(T), objs);
            return CommonDal.Instance.DeleteComAdoInfo<T>(info);
        }

        /// 新增 (自动生成)
        /// <summary>
        /// 新增 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool InsertComAdoInfo<T>(SqlTransaction sqltran, T info)
        {
            return CommonDal.Instance.InsertComAdoInfo<T>(info, sqltran);
        }
        /// 修改 (自动生成)
        /// <summary>
        /// 修改 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool UpdateComAdoInfo<T>(SqlTransaction sqltran, T info)
        {
            return CommonDal.Instance.UpdateComAdoInfo<T>(info, sqltran);
        }
        /// 删除 (自动生成)
        /// <summary>
        /// 删除 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs">主键序列值（和实体类中构造方法中的主键参数顺序一致）</param>
        /// <returns></returns>
        public bool DeleteComAdoInfo<T>(SqlTransaction sqltran, params object[] objs)
        {
            T info = (T)Activator.CreateInstance(typeof(T), objs);
            return CommonDal.Instance.DeleteComAdoInfo<T>(info, sqltran);
        }

        /// 新增 (自动生成)
        /// <summary>
        /// 新增 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool InsertComAdoInfo<T>(string connstr, T info)
        {
            return CommonDal.Instance.InsertComAdoInfo<T>(info, connstr);
        }
        /// 修改 (自动生成)
        /// <summary>
        /// 修改 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool UpdateComAdoInfo<T>(string connstr, T info)
        {
            return CommonDal.Instance.UpdateComAdoInfo<T>(info, connstr);
        }
        /// 删除 (自动生成)
        /// <summary>
        /// 删除 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs">主键序列值（和实体类中构造方法中的主键参数顺序一致）</param>
        /// <returns></returns>
        public bool DeleteComAdoInfo<T>(string connstr, params object[] objs)
        {
            T info = (T)Activator.CreateInstance(typeof(T), objs);
            return CommonDal.Instance.DeleteComAdoInfo<T>(info, connstr);
        }

        /// 查询 (自动生成)
        /// <summary>
        /// 查询 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs">主键序列值（和实体类中构造方法中的主键参数顺序一致）</param>
        /// <returns></returns>
        public T GetComAdoInfo<T>(params object[] objs)
        {
            T info = (T)Activator.CreateInstance(typeof(T), objs);
            return CommonDal.Instance.GetComAdoInfo(info);
        }
        /// 查询 (自动生成)
        /// <summary>
        /// 查询 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs">主键序列值（和实体类中构造方法中的主键参数顺序一致）</param>
        /// <returns></returns>
        public DataTable GetComAdoData<T>(params object[] objs)
        {
            T info = (T)Activator.CreateInstance(typeof(T), objs);
            return CommonDal.Instance.GetComAdoData(info);
        }
        #endregion

        /// 校验表是否存在
        /// <summary>
        /// 校验表是否存在
        /// </summary>
        /// <param name="tablenames"></param>
        /// <returns></returns>
        public bool CheckTableExists(params string[] tablenames)
        {
            return CommonDal.Instance.CheckTableExists(tablenames);
        }
    }
}
