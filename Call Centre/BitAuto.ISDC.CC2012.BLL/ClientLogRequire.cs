using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class ClientLogRequire
    {
        public static ClientLogRequire Instance = new ClientLogRequire();

        /// 获取所有坐席及在线状态
        /// <summary>
        /// 获取所有坐席及在线状态
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllEmployeeAgent(ClientLogRequireQuery query, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ClientLogRequire.Instance.GetAllEmployeeAgent(query, currentPage, pageSize, out totalCount);
        }
        /// 获取请求数据
        /// <summary>
        /// 获取请求数据
        /// </summary>
        /// <param name="logdate"></param>
        /// <param name="agentid"></param>
        /// <param name="vendorid"></param>
        /// <returns></returns>
        public ClientLogRequireInfo GetClientLogRequireInfo(DateTime logdate, int agentid, int vendorid)
        {
            return Dal.ClientLogRequire.Instance.GetClientLogRequireInfo(logdate, agentid, vendorid);
        }
        /// 获取请求数据
        /// <summary>
        /// 获取请求数据
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="vendorid"></param>
        /// <returns></returns>
        public DataTable GetClientLogRequireInfo(int agentid, int vendorid)
        {
            return Dal.ClientLogRequire.Instance.GetClientLogRequireInfo(agentid, vendorid);
        }
    }
}
