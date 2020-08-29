using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Dal.APP;
using XYAuto.ITSC.Chitunion2017.Dal.LETask;
using XYAuto.ITSC.Chitunion2017.Entities.APP;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017.BLL.APP
{
    public class GenericProfilesBll
    {
        #region 单例
        XmlDocument doc;
        XmlElement rootElem;
        public string strPath = ConfigurationManager.AppSettings["ConfigArgsPath"] + "\\GenericProfiles.xml";
        private GenericProfilesBll() { }
        public static GenericProfilesBll instance = null;
        public static readonly object padlock = new object();

        public static GenericProfilesBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new GenericProfilesBll();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        public XmlNode GetConfigurationInfo(ConfigurationModel paramQuery)
        {
            Dictionary<string, Func<ConfigurationModel, XmlNode>> _dic = new Dictionary<string, Func<ConfigurationModel, XmlNode>>{
                { "pz_hytc",m=> WelcomeSettingsInfo(m) },
                { "pz_tx", m=> WithdrawSettingsInfo(m) },
                { "pz_xytc", m=> FlauntSettingsInfo(m) },
                { "pz_qt", m=> RestsSettingsInfo(m) }
            };

            if (_dic.ContainsKey(paramQuery.NodeType))
            {
                EnumInfo enumInfo = SplitHelper.GetEnumKeyDescriptionList<NodeTypeEnum>(paramQuery.NodeType);

                return _dic[paramQuery.NodeType].Invoke(new ConfigurationModel { UserID = paramQuery.UserID, NodeType = enumInfo.Description });
            }
            return null;
        }


        /// <summary>
        /// 签到弹框配置
        /// </summary>
        /// <param name="AppSettings"></param>
        /// <returns></returns>
        public XmlNode DaySignSettingsInfo(ConfigurationModel entity)
        {
            //获取弹窗根节点
            var Nodel = GetXmlNode(entity.NodeType);
            //获取 MiddleContent节点
            var MiddleNodel = Nodel.SelectSingleNode($"MiddleContent");

            Entities.LETask.LeTaskInfo taskModel = LeTaskInfo.Instance.GetInfo(Convert.ToInt32(MiddleNodel.SelectSingleNode($"TaskID").InnerText)) ?? new Entities.LETask.LeTaskInfo();
            //获取已分享次数
            var ShareCount = FeedbackDa.Instance.GetShareCount(entity.UserID);
            //格式化并赋值
            MiddleNodel.SelectSingleNode($"MiddleTitle").InnerText = taskModel.TaskName;
            MiddleNodel.SelectSingleNode($"MediaUrl").InnerText = taskModel.ImgUrl;
            MiddleNodel.SelectSingleNode($"ContentRead").InnerText = string.Format(MiddleNodel.SelectSingleNode($"ContentRead").InnerText, taskModel.CPCPrice);
            MiddleNodel.SelectSingleNode($"ContentShare").InnerText = string.Format(MiddleNodel.SelectSingleNode($"ContentShare").InnerText, taskModel.CPCPrice);
            return Nodel;
        }


        /// <summary>
        /// 获取欢迎弹框配置
        /// </summary>
        /// <param name="AppSettings"></param>
        /// <returns></returns>
        public XmlNode WelcomeSettingsInfo(ConfigurationModel entity)
        {
            //获取弹窗根节点
            var Nodel = GetXmlNode(entity.NodeType);
            try
            {
                //获取 MiddleContent节点
                var MiddleNodel = Nodel.SelectSingleNode($"MiddleContent");
                Nodel.SelectSingleNode($"ShareContent").InnerText = string.Format(Nodel.SelectSingleNode($"ShareContent").InnerText, Nodel.SelectSingleNode($"BountyPrice").InnerText);


                #region 随机获取任务 用于测试
                //Entities.LETask.LeTaskInfo taskModel = null;
                //int taskId = FeedbackDa.Instance.GetTaskNewID();
                //taskModel = LeTaskInfo.Instance.GetInfo(taskId) ?? new Entities.LETask.LeTaskInfo();
                //MiddleNodel.SelectSingleNode($"TaskID").InnerText = taskId + string.Empty;

                #endregion

                string OrderUrl = ITSC.Chitunion2017.BLL.ShareOrderInfo.Instance.GetDomainByRandom_ShareArticle(MiddleNodel.SelectSingleNode($"MediaUrl").InnerText);
                //获取已分享次数
                int taskId = Convert.ToInt32(MiddleNodel.SelectSingleNode($"TaskID").InnerText);
                //int ShareCount = 0;
                var ShareCount = FeedbackDa.Instance.GetReadNum(taskId);
                //判断是否获取默认配置
                if (Nodel.SelectSingleNode($"IsDefault").InnerText.ToUpper() == "N")
                {
                    Entities.LETask.LeTaskInfo taskModel = LeTaskInfo.Instance.GetInfo(taskId) ?? new Entities.LETask.LeTaskInfo();
                    //格式化并赋值
                    MiddleNodel.SelectSingleNode($"MiddleTitle").InnerText = taskModel.TaskName;
                    MiddleNodel.SelectSingleNode($"MediaUrl").InnerText = OrderUrl;
                    MiddleNodel.SelectSingleNode($"Synopsis").InnerText = taskModel.Synopsis;
                    MiddleNodel.SelectSingleNode($"ImgUrl").InnerText = taskModel.ImgUrl;
                    MiddleNodel.SelectSingleNode($"ContentRead").InnerText = string.Format(MiddleNodel.SelectSingleNode($"ContentRead").InnerText, taskModel.CPCPrice);

                }
                else
                {
                    MiddleNodel.SelectSingleNode($"ContentRead").InnerText = string.Format(MiddleNodel.SelectSingleNode($"ContentRead").InnerText, MiddleNodel.SelectSingleNode($"CPCPrice").InnerText);

                }
                MiddleNodel.SelectSingleNode($"ContentShare").InnerText = string.Format(MiddleNodel.SelectSingleNode($"ContentShare").InnerText, ShareCount);

                Nodel.RemoveChild(Nodel.SelectSingleNode($"IsDefault"));

            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("欢迎弹窗配制出错：", ex);
            }
            return Nodel;
        }


        /// <summary>
        /// 提现配置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public XmlNode WithdrawSettingsInfo(ConfigurationModel entity)
        {
            return GetXmlNode(entity.NodeType);
        }
        /// <summary>
        /// 炫耀弹窗配置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public XmlNode FlauntSettingsInfo(ConfigurationModel entity)
        {
            return GetXmlNode(entity.NodeType);
        }
        /// <summary>
        /// 其他配置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public XmlNode RestsSettingsInfo(ConfigurationModel entity)
        {
            return GetXmlNode(entity.NodeType);
        }

        /// <summary>
        /// 获取欢迎签到额外奖励
        /// </summary>
        /// <returns></returns>
        public string GetWelcomeBountyPrice()
        {
            return GetXmlNode("WelcomeSettings//BountyPrice").InnerText;
        }
        /// <summary>
        /// 获取提现额外奖励
        /// </summary>
        /// <returns></returns>
        public string GetFlauntBountyPrice()
        {
            return GetXmlNode("FlauntSettings//BountyPrice").InnerText;
        }
        /// <summary>
        /// 根据节点名称获取节点文本
        /// </summary>
        /// <param name="NodelName"></param>
        /// <returns></returns>
        public XmlNode GetXmlNode(string NodelName)
        {
            doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
          
            //xmlFilePath:xml文件路径  
            XmlReader reader = XmlReader.Create(strPath, settings);
            doc.Load(reader);    //加载Xml文件  
            rootElem = doc.DocumentElement;
            reader.Dispose();
            reader.Close();
            return rootElem.SelectSingleNode($"{NodelName}");
        }
    }
}
