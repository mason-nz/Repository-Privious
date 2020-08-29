using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using MongoDB;
using System.Data;

namespace CC2015_HollyFormsApp
{
    /// 在线客服
    /// <summary>
    /// 在线客服
    /// </summary>
    public class AgentLive
    {
        /// <summary>
        /// 客服工号
        /// </summary>
        public string AgentID { get; set; }
        /// <summary>
        /// 客服分机号
        /// </summary>
        public string AgentDN { get; set; }
        /// <summary>
        /// 客服当前状态
        /// </summary>
        public string CurrStatus { get; set; }
        public AgentStateForListen CurrStatus_Enum { get { return MongoDBHelper.ConvertHollyCurrStatus(CurrStatus); } }
        /// <summary>
        /// 当前状态开始时间
        /// </summary>
        public string StartTime { get; set; }
        public DateTime StartTime_Date { get { return CommonFunction.ObjectToDateTime(StartTime); } }
        /// <summary>
        /// 联络ID
        /// </summary>
        public string ContactID { get; set; }
        /// <summary>
        /// 技能组ID
        /// </summary>
        public string SkillID { get; set; }
        public int SkillID_Int { get { return CommonFunction.ObjectToInteger(SkillID); } }
        /// <summary>
        /// 呼叫方向
        /// </summary>
        public string CallDirection { get; set; }
        public string CallDirectionDesc { get; set; }
        public int CallDirection_Int { get { return CommonFunction.ObjectToInteger(CallDirection); } }
        public Calltype CallDirection_Calltype { get { return MongoDBHelper.ConvertCalltype(CallDirectionDesc); } }

        /// <summary>
        /// 主叫
        /// </summary>
        public string ANI { get; set; }
        /// <summary>
        /// 被叫
        /// </summary>
        public string DNIS { get; set; }
        /// 有效手机号
        /// <summary>
        /// 有效手机号
        /// </summary>
        public string UserPhone
        {
            get
            {
                if (CallDirection_Int == 1)
                {
                    return BitAuto.ISDC.CC2012.BLL.Util.HaoMaProcess(Common.AddPrex(ANI), Common.GetAreaCode());
                }
                else if (CallDirection_Int == 2)
                {
                    return BitAuto.ISDC.CC2012.BLL.Util.HaoMaProcess(Common.AddPrex(DNIS), Common.GetAreaCode());
                }
                else return "";
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is AgentLive)
            {
                AgentLive b = obj as AgentLive;
                return this.AgentID.Equals(b.AgentID);
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return AgentID + " " + AgentDN;
        }
    }
    /// 客服信息
    /// <summary>
    /// 客服信息
    /// </summary>
    public class AgentInfo
    {
        /// <summary>
        /// 客服工号
        /// </summary>
        public string agentDn { get; set; }
        /// <summary>
        /// 所属业务分组-合力定义的ID
        /// </summary>
        public List<string> departmentsCode { get; set; }
        public string departmentsCode_code { get { if (departmentsCode.Count > 0) return departmentsCode[0]; else return ""; } }
        /// <summary>
        /// 客服名称
        /// </summary>
        public string userName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is AgentInfo)
            {
                AgentInfo b = obj as AgentInfo;
                return agentDn.EndsWith(b.agentDn);
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return userName;
        }

        #region 其他信息
        public AgentLive AgentLive { get; private set; }
        public GroupInfo GroupInfo { get; set; }

        /// 分机号
        /// <summary>
        /// 分机号
        /// </summary>
        public string ExtensionNum
        {
            get
            {
                if (AgentLive == null)
                {
                    return "";
                }
                else
                {
                    return AgentLive.AgentDN;
                }
            }
        }
        /// 当前状态
        /// <summary>
        /// 当前状态
        /// </summary>
        public AgentStateForListen CurrStatus
        {
            get
            {
                if (AgentLive == null)
                {
                    return AgentStateForListen.离线;
                }
                else
                {
                    return AgentLive.CurrStatus_Enum;
                }
            }
        }
        /// 所属分组
        /// <summary>
        /// 所属分组
        /// </summary>
        public string BgName
        {
            get
            {
                if (GroupInfo == null)
                {
                    return "";
                }
                else
                {
                    return GroupInfo.orgName;
                }
            }
        }
        /// BGID
        /// <summary>
        /// BGID
        /// </summary>
        public int BGID
        {
            get
            {
                if (GroupInfo == null)
                {
                    return 0;
                }
                else
                {
                    return GroupInfo.orgMarker_Int;
                }
            }
        }
        /// UserID
        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }
        #endregion

        #region 设置在线信息
        private bool islock_data = false;
        private DateTime lockst_data = Common.GetCurrentTime();

        private bool islock_dead = false;
        private DateTime lockst_dead = Common.GetCurrentTime();

        /// 设置在线信息
        /// <summary>
        /// 设置在线信息
        /// </summary>
        /// <param name="live"></param>
        public void SetAgentLive(AgentLive live)
        {
            ///////////////////自动转离线逻辑///////////////////////////
            //厂家在线数据表会不定时的drop+create
            //为了防止在drop之后查询不到数据时界面显示离线的情况
            //加islock_dead锁来判断数据是否是真正的转离线
            //只有离线状态超过5s，才会判断是真正的离线
            //逻辑：
            //1.转离线时，首先加锁
            //2.在进入下轮查询后，如果发现还是转离线时，尝试解锁，加锁时间到达门限值，则能解锁，表示真正的离线
            //3.否则可能只是数据被drop掉了而已，不进行离线

            ///////////////////非自动转离线下-状态切换/////////////////////////
            //厂家的数据会有3，5秒延时
            //为了防止手动修改状态之后，数据库还没有刷新过来时，界面数据显示旧数据的问题
            //加入数据锁islock_data
            //逻辑：
            //1.如果手动修改状态，调用LockAgentLive方法加锁
            //2.设置状态时，先对加锁状态的数据进行解锁
            //3.解锁方式有两种：一是加锁时间到达门限值10s（防止长时间加锁造成异常） 二是传入的数据和旧的数据状态相同（表示数据库数据已被正确同步）
            //4.判断是否锁定，未锁定则赋值，否则等待下一轮查询，继续尝试解锁

            #region 转离线时
            //当AgentLive != null && live == null时，表示自动转离线
            //可能是真正的离线；也可能是drop表后还没来得及create表
            if (AgentLive != null && live == null)
            {
                //转离线时，先锁定
                if (islock_dead == false)
                {
                    Log("转离线时-锁定");
                    Lock(true, 2);
                }
                else
                {
                    Log("转离线时-已锁定");
                    //达到门限值，解锁
                    if (IsOverTime(2))
                    {
                        Log("转离线时-解锁");
                        Lock(false, 2);
                        //设置离线
                        AgentLive = live;
                    }
                }
            }
            else
            {
                //退出转离线时，解锁
                Lock(false, 2);

                #region 状态切换
                //被LockAgentLive方法锁定了
                if (islock_data)
                {
                    Log("状态切换-锁定");
                    //锁定到达最大时间，解锁
                    if (IsOverTime(1))
                    {
                        Log("状态切换-时间解锁");
                        Lock(false, 1);
                    }
                    else
                        //状态相等 或者 都是离线
                        if ((AgentLive != null && live != null && AgentLive.CurrStatus == live.CurrStatus) || (AgentLive == null && live == null))
                        {
                            Log("状态切换-数据解锁");
                            //数据相同，解锁
                            Lock(false, 1);
                        }
                }
                //解锁状态下，才能赋值
                if (islock_data == false)
                {
                    AgentLive = live;
                }
                #endregion
            }
            #endregion
        }
        /// 给在线信息加锁
        /// <summary>
        /// 给在线信息加锁
        /// </summary>
        /// <param name="state"></param>
        public void LockAgentLive(OperForListen state)
        {
            if (state == OperForListen.强制置忙)
            {
                AgentLive.CurrStatus = "置忙";
            }
            else if (state == OperForListen.强制置闲)
            {
                AgentLive.CurrStatus = "空闲";
            }
            else if (state == OperForListen.强制签出)
            {
                AgentLive = null;
            }
            else if (state == OperForListen.强拆)
            {
                AgentLive.CurrStatus = "空闲";
            }
            else
            {
                return;
            }
            if (AgentLive != null)
            {
                //设置状态开始时间
                AgentLive.StartTime = Common.GetCurrentTime().ToString();
            }
            //设置锁定
            Lock(true, 1);
        }

        /// 锁定/解锁
        /// <summary>
        /// 锁定/解锁
        /// </summary>
        /// <param name="b"></param>
        private void Lock(bool b, int type)
        {
            if (b)
            {
                if (type == 1)
                {
                    islock_data = true;
                    lockst_data = Common.GetCurrentTime();
                }
                else if (type == 2)
                {
                    islock_dead = true;
                    lockst_dead = Common.GetCurrentTime();
                }
            }
            else
            {
                if (type == 1)
                {
                    islock_data = false;
                }
                else if (type == 2)
                {
                    islock_dead = false;
                }
            }
        }
        /// 是否到最大锁定时间了
        /// <summary>
        /// 是否到最大锁定时间了
        /// </summary>
        /// <returns></returns>
        private bool IsOverTime(int type)
        {
            DateTime st = new DateTime();
            int menxian = 5;
            if (type == 1)
            {
                st = lockst_data;
                menxian = 10;
            }
            else if (type == 2)
            {
                st = lockst_dead;
                menxian = 5;
            }
            if ((Common.GetCurrentTime() - st).TotalSeconds > menxian)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Log(string msg)
        {
            if (this.ToString() == "刘颖")
            {
                System.Diagnostics.Debug.WriteLine(this.ToString() + "--" + msg);
            }
        }
        #endregion
    }
    /// 分组信息
    /// <summary>
    /// 分组信息
    /// </summary>
    public class GroupInfo
    {
        /// <summary>
        /// 合力定义的ID
        /// </summary>
        public string _id { get; set; }
        /// <summary>
        /// CC定义的ID
        /// </summary>
        public string orgMarker { get; set; }
        public int orgMarker_Int { get { return CommonFunction.ObjectToInteger(orgMarker); } }
        /// <summary>
        /// 业务分组名称
        /// </summary>
        public string orgName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is GroupInfo)
            {
                GroupInfo b = obj as GroupInfo;
                return _id.Equals(b._id);
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return orgName;
        }
    }
    /// 临时表配置信息
    /// <summary>
    /// 临时表配置信息
    /// </summary>
    public class MonitorParam
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    /// 查询数据库类
    /// <summary>
    /// 查询数据库类
    /// </summary>
    public class MongoDB
    {
        private static object _locker = new object();
        private static MongoDB _instance = null;
        public static MongoDB Instance
        {
            get
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new MongoDB(DataSourceForListen.CC);
                    }
                    return _instance;
                }
            }
        }

        //数据库连接字符串 测试：192.168.15.84  正式：192.168.112.206
        //不能写入配置文件，因为此连接没有用户名和密码
        private string strconn = "mongodb://192.168.112.206:27017";
        //数据库名称
        private string dbName = "monitor";

        /// 客服信息
        /// <summary>
        /// 客服信息
        /// </summary>
        public List<AgentInfo> AgentInfoList = null;
        /// 分组信息
        /// <summary>
        /// 分组信息
        /// </summary>
        public List<GroupInfo> GroupInfoList = null;

        public MongoDB(DataSourceForListen datasource)
        {
            if (LoginUser.LoginAreaType == AreaType.西安)
            {
                //正式系统
                strconn = "mongodb://192.168.112.206:27017";
            }
            else if (LoginUser.LoginAreaType == AreaType.北京)
            {
                //测试系统
                strconn = "mongodb://192.168.15.84:27017";
            }
            GetDataFromCC();
        }

        /// <summary>
        /// 从合力获取数据
        /// </summary>
        private void GetDataFromHolly()
        {
            if (AgentInfoList == null)
            {
                AgentInfoList = new List<AgentInfo>();
                //从合力库缓存数据
                AgentInfoList = QueryCommonData<AgentInfo>("CoreUser");
            }
            if (GroupInfoList == null)
            {
                GroupInfoList = new List<GroupInfo>();
                //从合力库缓存数据
                GroupInfoList = QueryCommonData<GroupInfo>("CoreOrganization");
            }
        }
        /// <summary>
        /// 从CC获取数据
        /// </summary>
        private void GetDataFromCC()
        {
            DataSet ds = AgentTimeStateHelper.Instance.GetAllEmployeeAgentAndBusinessGroup();
            if (AgentInfoList == null)
            {
                AgentInfoList = new List<AgentInfo>();
                //从CC库缓存数据
                if (ds != null && ds.Tables["EmployeeAgent"] != null)
                {
                    foreach (DataRow dr in ds.Tables["EmployeeAgent"].Rows)
                    {
                        AgentInfo ai = new AgentInfo() { UserID = CommonFunction.ObjectToInteger(dr["UserID"]), agentDn = dr["AgentNum"].ToString(), departmentsCode = new List<string>() { dr["BGID"].ToString() }, userName = dr["TrueName"].ToString() };
                        AgentInfoList.Add(ai);
                    }
                }
            }
            if (GroupInfoList == null)
            {
                GroupInfoList = new List<GroupInfo>();
                //从CC库缓存数据
                if (ds != null && ds.Tables["BusinessGroup"] != null)
                {
                    foreach (DataRow dr in ds.Tables["BusinessGroup"].Rows)
                    {
                        GroupInfo gi = new GroupInfo() { _id = dr["BGID"].ToString(), orgMarker = dr["BGID"].ToString(), orgName = dr["Name"].ToString() };
                        GroupInfoList.Add(gi);
                    }
                }
            }
        }

        /// 通用数据查询方法
        /// <summary>
        /// 通用数据查询方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection_name"></param>
        /// <returns></returns>
        public List<T> QueryCommonData<T>(string collection_name) where T : class
        {
            List<T> list = new List<T>();
            using (Mongo mongo = new Mongo(strconn))
            {
                try
                {
                    mongo.Connect();
                    MongoDatabase db = mongo.GetDatabase(dbName) as MongoDatabase;
                    list = (from item in db.GetCollection<T>(collection_name).Linq()
                            select item).ToList();
                }
                catch (Exception ex)
                {
                    Loger.Log4Net.Error("[****MongoDB****][QueryCommonData] 异常", ex);
                }
                finally
                {
                    mongo.Disconnect();
                }
            }
            return list;
        }
    }
}
