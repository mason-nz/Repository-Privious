using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Drawing;

namespace CC2015_HollyFormsApp
{
    public class MongoDBHelper
    {
        /// 获取管辖分组
        /// <summary>
        /// 获取管辖分组
        /// </summary>
        /// <returns></returns>
        public static List<KeyValue> GetManageGroupList()
        {
            List<KeyValue> list = new List<KeyValue>();
            DataTable dt = AgentTimeStateHelper.Instance.GetManageBusinessGroups(LoginUser.UserID.Value);
            if (dt != null)
            {
                list.Add(new KeyValue("-1", "请选择"));
                foreach (DataRow dr in dt.Select())
                {
                    KeyValue kv = new KeyValue(dr["BGID"].ToString(), dr["Name"].ToString());
                    list.Add(kv);
                }
            }
            return list;
        }
        /// 获取客服状态
        /// <summary>
        /// 获取客服状态
        /// </summary>
        /// <returns></returns>
        public static List<KeyValue> GetAllHollyAgentState()
        {
            List<KeyValue> list = new List<KeyValue>();
            list.Add(new KeyValue("-1", "请选择"));
            list.Add(new KeyValue(((int)AgentStateForListen.置忙).ToString(), AgentStateForListen.置忙.ToString()));
            list.Add(new KeyValue(((int)AgentStateForListen.置闲).ToString(), AgentStateForListen.置闲.ToString()));
            list.Add(new KeyValue(((int)AgentStateForListen.振铃).ToString(), AgentStateForListen.振铃.ToString()));
            list.Add(new KeyValue(((int)AgentStateForListen.通话中).ToString(), AgentStateForListen.通话中.ToString()));
            list.Add(new KeyValue(((int)AgentStateForListen.话后).ToString(), AgentStateForListen.话后.ToString()));
            list.Add(new KeyValue(((int)AgentStateForListen.离线).ToString(), AgentStateForListen.离线.ToString()));
            return list;
        }

        /// 状态转换
        /// <summary>
        /// 状态转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static AgentStateForListen ConvertHollyCurrStatus(string value)
        {
            if (value == "休息" || value == "置忙")
            {
                return AgentStateForListen.置忙;
            }
            else if (value == "空闲")
            {
                return AgentStateForListen.置闲;
            }
            else if (value == "外拨" || value == "振铃")
            {
                return AgentStateForListen.振铃;
            }
            else if (value == "通话")
            {
                return AgentStateForListen.通话中;
            }
            else if (value == "事后整理")
            {
                return AgentStateForListen.话后;
            }
            else return AgentStateForListen.无;
        }
        /// 背景图片获取
        /// <summary>
        /// 背景图片获取
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Image ImageName(AgentStateForListen state)
        {
            switch (state)
            {
                case AgentStateForListen.通话中:
                    return global::CC2015_HollyFormsApp.Properties.Resources.通话;
                case AgentStateForListen.振铃:
                    return global::CC2015_HollyFormsApp.Properties.Resources.振铃;
                case AgentStateForListen.置忙:
                    return global::CC2015_HollyFormsApp.Properties.Resources.置忙;
                case AgentStateForListen.置闲:
                    return global::CC2015_HollyFormsApp.Properties.Resources.置闲;
                case AgentStateForListen.话后:
                    return global::CC2015_HollyFormsApp.Properties.Resources.话后;
                case AgentStateForListen.离线:
                    return global::CC2015_HollyFormsApp.Properties.Resources.签出;
                default:
                    return null;
            }
        }
        /// 顺序获取
        /// <summary>
        /// 顺序获取
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static int SortNumber(AgentStateForListen state)
        {
            //置忙-置闲-通话中-振铃-话后-离线
            switch (state)
            {
                case AgentStateForListen.振铃:
                    return 4;
                case AgentStateForListen.离线:
                    return 6;
                case AgentStateForListen.置忙:
                    return 1;
                case AgentStateForListen.置闲:
                    return 2;
                case AgentStateForListen.话后:
                    return 5;
                case AgentStateForListen.通话中:
                    return 3;
                default:
                    return 0;
            }
        }

        /// 获取临时表名
        /// <summary>
        /// 获取临时表名
        /// </summary>
        /// <returns></returns>
        private static string GetTableName()
        {
            try
            {
                List<MonitorParam> myCollection = MongoDB.Instance.QueryCommonData<MonitorParam>("MonitorParam");
                if (myCollection != null)
                {
                    MonitorParam mp = myCollection.FirstOrDefault(x => x.name == "MonitorParam");
                    if (mp != null)
                    {
                        return mp.value;
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("[****MongoDB****][GetTableName] 异常", ex);
                return "";
            }
        }
        /// 查询所有客服的即时信息
        /// <summary>
        /// 查询所有客服的即时信息
        /// </summary>
        /// <returns></returns>
        public static List<AgentInfo> GetAllAgentInfo()
        {
            string name = "MonitorAgentLiveData" + GetTableName();
            Loger.Log4Net.Info("[****MongoDB****][GetTableName] 查询临时表-表名配置=" + name);
            //获取在线客服
            List<AgentLive> live_list = MongoDB.Instance.QueryCommonData<AgentLive>(name);
            //和AgentInfoList GroupInfoList 合并
            foreach (AgentInfo info in MongoDB.Instance.AgentInfoList)
            {
                info.SetAgentLive(live_list.FirstOrDefault(x => x.AgentID == info.agentDn));
                info.GroupInfo = MongoDB.Instance.GroupInfoList.FirstOrDefault(x => x._id == info.departmentsCode_code);
            }
            return MongoDB.Instance.AgentInfoList;
        }
        /// 转换方向
        /// <summary>
        /// 转换方向
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Calltype ConvertCalltype(string value)
        {
            if (value == "呼入")
            {
                return Calltype.C1_呼入;
            }
            else if (value == "呼出")
            {
                return Calltype.C2_呼出;
            }
            else return Calltype.C0_未知;
        }
    }
}
