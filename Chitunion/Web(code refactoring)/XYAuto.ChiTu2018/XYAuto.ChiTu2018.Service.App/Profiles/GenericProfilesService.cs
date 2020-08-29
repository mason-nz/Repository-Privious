/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.App.Profiles
* 类 名 称 ：GenericProfilesService
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/23 14:35:39
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Enum.XmlProfiles;
using XYAuto.ChiTu2018.Infrastructure.LeTask;
using XYAuto.ChiTu2018.Service.App.Profiles.Dto;
using XYAuto.CTUtils.Config;
using XYAuto.CTUtils.Log;
using XYAuto.CTUtils.Sys;

namespace XYAuto.ChiTu2018.Service.App.Profiles
{

    public class GenericProfilesService
    {
        #region MyRegion
        XmlDocument doc;
        XmlElement rootElem;
        public string strPath = ConfigurationUtil.GetAppSettingValue("ConfigArgsPath") + "\\GenericProfiles.config";
        private GenericProfilesService() { }
        private static readonly Lazy<GenericProfilesService> Linstance = new Lazy<GenericProfilesService>(() => { return new GenericProfilesService(); });

        public static GenericProfilesService Instance => Linstance.Value;
        #endregion
        public XmlNode GetConfigurationInfo(ConfigurationDto paramQuery)
        {
            Dictionary<string, Func<ConfigurationDto, XmlNode>> _dic = new Dictionary<string, Func<ConfigurationDto, XmlNode>>{
                { "pz_hytc",m=> WelcomeSettingsInfo(m) },
                { "pz_tx", m=> WithdrawSettingsInfo(m) },
                { "pz_xytc", m=> FlauntSettingsInfo(m) },
                { "pz_qt", m=> RestsSettingsInfo(m) }
            };

            if (_dic.ContainsKey(paramQuery.NodeType))
            {
                var enumDescription = EnumHelper.GetEnumDesc((NodeTypeEnum)Enum.Parse(typeof(NodeTypeEnum), paramQuery.NodeType));

                return _dic[paramQuery.NodeType].Invoke(new ConfigurationDto { UserID = paramQuery.UserID, NodeType = enumDescription });
            }
            return null;
        }


        /// <summary>
        /// 签到弹框配置
        /// </summary>
        /// <param name="AppSettings"></param>
        /// <returns></returns>
        public XmlNode DaySignSettingsInfo(ConfigurationDto entity, int userId)
        {
            //获取弹窗根节点
            var Nodel = GetXmlNode(entity.NodeType);
            //获取 MiddleContent节点
            var MiddleNodel = Nodel.SelectSingleNode($"MiddleContent");

            var taskModel = new LeTaskInfoBO().GeTaskInfo(Convert.ToInt32(MiddleNodel.SelectSingleNode($"TaskID").InnerText)) ?? new LE_TaskInfo();
            //获取已分享次数
            var ShareCount = new LeShareDetailBO().GetShareDetailCount(userId);
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
        public XmlNode WelcomeSettingsInfo(ConfigurationDto entity)
        {
            //获取弹窗根节点
            var Nodel = GetXmlNode(entity.NodeType);
            try
            {
                //获取 MiddleContent节点
                var MiddleNodel = Nodel.SelectSingleNode($"MiddleContent");
                Nodel.SelectSingleNode($"ShareContent").InnerText = string.Format(Nodel.SelectSingleNode($"ShareContent").InnerText, Nodel.SelectSingleNode($"BountyPrice").InnerText);


                #region 随机获取任务 用于测试

                //taskModel = new LeTaskInfoBO().GetRandomInfo();
                //MiddleNodel.SelectSingleNode($"TaskID").InnerText = taskModel.RecID + string.Empty;

                //string OrderUrl = GetDomainByRandom_ShareArticle(taskModel.MaterialUrl);
                #endregion
                string OrderUrl = LeTaskBasicSupport.GetDomainByRandom_ShareArticle(MiddleNodel.SelectSingleNode($"MediaUrl").InnerText);
               

                int taskId = Convert.ToInt32(MiddleNodel.SelectSingleNode($"TaskID").InnerText);
                //获取已分享次数
                var ShareCount = new LeTaskInfoBO().GetReadNum(taskId);
                //判断是否获取默认配置
                if (Nodel.SelectSingleNode($"IsDefault").InnerText.ToUpper() == "N")
                {
                     var taskModel = new LeTaskInfoBO().GeTaskInfo(taskId) ?? new LE_TaskInfo();

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
                Log4NetHelper.Default().Info("欢迎弹窗配制出错：", ex);
            }
            return Nodel;
        }


        /// <summary>
        /// 提现配置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public XmlNode WithdrawSettingsInfo(ConfigurationDto entity)
        {
            return GetXmlNode(entity.NodeType);
        }
        /// <summary>
        /// 炫耀弹窗配置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public XmlNode FlauntSettingsInfo(ConfigurationDto entity)
        {
            return GetXmlNode(entity.NodeType);
        }
        /// <summary>
        /// 其他配置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public XmlNode RestsSettingsInfo(ConfigurationDto entity)
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
