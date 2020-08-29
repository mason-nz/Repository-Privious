using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CC2015_HollyFormsApp
{
    public class Constant
    {
        /// 本地客户端配置XML文件名（用于记录登录账号及分机信息）
        /// <summary>
        /// 本地客户端配置XML文件名（用于记录登录账号及分机信息）
        /// </summary>
        public const string ClientLocalConfigXMLName = "UserConfig.xml";
        /// 本地客户端配置XML文件中，表示域账号对应的节点
        /// <summary>
        /// 本地客户端配置XML文件中，表示域账号对应的节点
        /// </summary>
        public const string ClientLocalConfigADNameNode = "Userconfig/DomainAccount";
        /// 本地客户端配置XML文件中，表示分机号码对应的节点
        /// <summary>
        /// 本地客户端配置XML文件中，表示分机号码对应的节点
        /// </summary>
        public const string ClientLocalConfigExtensionNode = "Userconfig/Extension";
        /// 本地客户端配置XML文件中，表示分机号码对应的节点
        /// <summary>
        /// 本地客户端配置XML文件中，表示分机号码对应的节点
        /// </summary>
        public const string ClientLocalConfigPassWordNode = "Userconfig/AutoPassWord";
    }
}
