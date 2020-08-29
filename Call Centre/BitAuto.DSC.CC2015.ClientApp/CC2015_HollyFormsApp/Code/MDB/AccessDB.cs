using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using CC2015_HollyFormsApp.CCWeb.CallRecordService;

namespace CC2015_HollyFormsApp
{
    public class AccessDB
    {
        public OleDbConnection Conn;
        //连接字符串
        public string ConnString;

        /// 构造函数   
        /// <summary>   
        /// 构造函数   
        /// </summary>   
        public AccessDB()
        {
            ConnString = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + MDBFileHelper.MdbFileName;
            Conn = new OleDbConnection(ConnString);
            Conn.Open();
        }
        /// 请在数据传递完毕后调用该函数，关闭数据链接。   
        /// <summary>   
        /// 请在数据传递完毕后调用该函数，关闭数据链接。   
        /// </summary>   
        public void Close()
        {
            Conn.Close();
        }
        /// 根据SQL命令返回数据DataTable数据表
        /// <summary>   
        /// 根据SQL命令返回数据DataTable数据表
        /// </summary>   
        /// <param name="SQL"></param>   
        /// <returns></returns>   
        public DataTable SelectToDataTable(string SQL)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            OleDbCommand command = new OleDbCommand(SQL, Conn);
            adapter.SelectCommand = command;
            DataTable Dt = new DataTable();
            adapter.Fill(Dt);
            return Dt;
        }
        /// 执行SQL命令
        /// <summary>   
        /// 执行SQL命令
        /// </summary>   
        /// <param name="SQL"></param>   
        /// <returns></returns>   
        public bool ExecuteSQLNonquery(string SQL)
        {
            OleDbCommand cmd = new OleDbCommand(SQL, Conn);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
