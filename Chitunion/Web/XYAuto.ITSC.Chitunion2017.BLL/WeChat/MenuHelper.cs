using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Menu;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat.Dto.WxMenu;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat
{
    /// <summary>
    /// 注释：MenuHelper
    /// 作者：masj
    /// 日期：2018/5/29 8:58:48
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class MenuHelper
    {
        public static readonly MenuHelper Instance = new MenuHelper();
        private string WeChatMenuClickDataPath_User = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WeChatMenuClickDataPath_User", true);
        private string WeChatMenuClickDataPath_Pwd = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WeChatMenuClickDataPath_Pwd", true);

        /// <summary>
        /// 获取微信菜单信息
        /// </summary>
        /// <param name="appId">AppID</param>
        /// <returns>若找不到则返回null</returns>
        public GetMenuResult GetMenu(string appId)
        {
            return CommonApi.GetMenu(appId);
        }

        /// <summary>
        /// 清空微信公众号菜单
        /// </summary>
        /// <param name="appId">AppID</param>
        public bool ClearMenu(string appId)
        {
            var result = GetAppNameAndSecretByAppId(appId);
            if (result != null)
            {
                AccessTokenContainer.Register(result.Item1, result.Item2);
                CommonApi.DeleteMenu(appId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 创建微信公众号菜单
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="bg">ButtonGroup对象</param>
        /// <param name="msg">若调用出错，则记录错误信息</param>
        /// <returns>创建成功返回true，否则返回false</returns>
        public bool AddMenu(string appId, ButtonGroup bg, ref string msg)
        {
            if (bg != null)
            {
                var result = CommonApi.CreateMenu(appId, bg);
                if (result != null)
                {
                    msg = result.errmsg;
                    if (result.errmsg.ToLower() == "ok")
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        /// <summary>
        /// 获取微信模板信息数据
        /// 若filePath不为空，可以支持共享文件夹，如：\\192.168.3.19\wwwroot\XYAuto.POBU.Chitunion2018.AdminWebAPI\ConfigFile\test.config
        /// </summary>
        /// <returns></returns>
        public List<WxMenuDataDto> GetWxMenuConfigData(string filePath = null)
        {
            string content = string.Empty;
            string wxTempDataFilePath = string.Empty;
            if (string.IsNullOrEmpty(filePath))
            {
                wxTempDataFilePath = System.AppDomain.CurrentDomain.BaseDirectory +
                                     XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WxMenuDataInfo");
                content = XYAuto.Utils.FileHelper.ReadFile(wxTempDataFilePath, Encoding.UTF8);
            }
            else
            {
                if (filePath.StartsWith(@"\\"))
                {
                    string ip = filePath.Replace(@"\\", "").Substring(0, filePath.Replace(@"\\", "").IndexOf(@"\", StringComparison.Ordinal));
                    using (FileShareHelper tool = new FileShareHelper(WeChatMenuClickDataPath_User, WeChatMenuClickDataPath_Pwd, ip))
                    {
                        content = BLL.FileShareHelper.ReadFiles(filePath);
                    }
                    //bool flag = BLL.FileShareHelper.ConnectState(filePath.Substring(0, filePath.LastIndexOf(@"\")),
                    //     WeChatMenuClickDataPath_User, WeChatMenuClickDataPath_Pwd);
                    //if (flag)
                    //{
                    //    content = BLL.FileShareHelper.ReadFiles(filePath);
                    //}
                }
                else
                {
                    wxTempDataFilePath = filePath;
                    content = XYAuto.Utils.FileHelper.ReadFile(wxTempDataFilePath, Encoding.UTF8);
                }
            }
            return JsonConvert.DeserializeObject<List<WxMenuDataDto>>(content);
        }

        public string GetMediaIdByAppIdAndKey(string appId, string key)
        {
            List<WxMenuDataDto> list = GetWxMenuConfigData();
            var result = list.FirstOrDefault(s => s.AppId == appId);
            if (result != null && result.MenuClickList != null)
            {
                var res2 = result.MenuClickList.FirstOrDefault(s => s.EventKey == key);
                if (res2 != null)
                {
                    return res2.MediaId;
                }

            }
            return null;
        }

        /// <summary>
        /// 根据配置文件物理路径及AppID，获取菜单点击集合
        /// </summary>
        /// <param name="filePath">配置文件物理路径</param>
        /// <param name="appId">AppID</param>
        /// <returns>返回菜单点击集合</returns>
        public List<WxMenuClick> GetMenuClickDataByAppId(string filePath, string appId)
        {
            List<WxMenuDataDto> list = GetWxMenuConfigData(filePath);
            var result = list.FirstOrDefault(s => s.AppId == appId);
            if (result != null && result.MenuClickList != null)
            {
                return result.MenuClickList;
            }
            return null;
        }

        public WxMenuDataDto GetMenuDataByAppId(string filePath, string appId)
        {
            List<WxMenuDataDto> list = GetWxMenuConfigData(filePath);
            var result = list.FirstOrDefault(s => s.AppId == appId);
            if (result != null)
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// 根据AppID，从配置文件（ConfigFile/WxMenuData.config）中获取AppName
        /// </summary>
        /// <param name="appId">AppID</param>
        /// <returns>第一个属性为：微信名称；第二个属性为密钥</returns>
        public Tuple<string, string> GetAppNameAndSecretByAppId(string appId)
        {
            List<WxMenuDataDto> list = GetWxMenuConfigData();
            var result = list.FirstOrDefault(s => s.AppId == appId);
            if (result != null)
            {
                return Tuple.Create<string, string>(result.WxNum, result.AppSecret);
            }
            return null;
        }

        /// <summary>
        /// 验证提交保存自定义回复信息及关注回复信息逻辑
        /// </summary>
        /// <param name="req">请求对象</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool VerifyRequestBySaveCustomMsgData(WxMenuDataDto req, out string msg)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(req.AppId))
            {
                msg = "缺少AppId参数";
            }
            else
            {
                if (req.CustomMsgList != null)
                {
                    var listResult = req.CustomMsgList.Where((x, i) => req.CustomMsgList.FindIndex(z => z.Key == x.Key) == i);
                    if (listResult != null && req.CustomMsgList.Count != listResult.Count())
                    {
                        msg = "页面中输入的key有重复项";
                    }
                    else
                    {
                        msg = "";
                        flag = true;
                    }
                }
                else
                {
                    msg = "";
                    flag = true;
                }
            }

            return flag;
        }

        public bool VerifyRequestBySaveMenuData(SaveWxMenuDataDto req, out string msg)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(req.AppId))
            {
                msg = "缺少AppId参数";
            }
            else if (req.Buttons == null)
            {
                msg = "缺少Buttons参数";
            }
            else if (req.Buttons.Count > 3)
            {
                msg = "微信公众号一级菜单最多只能有3个";
            }
            else
            {
                foreach (var btn in req.Buttons)
                {
                    if (btn.Name.Length > 6)
                    {
                        msg = $"一级菜单[{btn.Name}]，长度不能超过6";
                        return false;
                    }
                    else if (string.IsNullOrEmpty(btn.Type))
                    {
                        msg = $"一级菜单[{btn.Name}]，Type内容不能为空";
                        return false;
                    }
                    else if (btn.Type.ToLower() == "view" && !BLL.Util.IsUrl(btn.Value))
                    {
                        msg = $"一级菜单[{btn.Name}]，中的Value必须是url格式";
                        return false;
                    }
                    else if (btn.Type.ToLower() == "click")
                    {
                        //MemoryStream memStream = new MemoryStream();
                        //Senparc.Weixin.MP.AdvancedAPIs.MediaApi.GetForeverMedia(req.AppId, btn.Value, memStream);
                        //if (memStream.Length < 0)
                        //{
                        //    msg = $"一级菜单[{btn.Name}]，中的Value必须是图片格式";
                        //    return false;
                        //}
                    }
                    else if (btn.SubButtons != null && btn.SubButtons.Count > 5)
                    {
                        msg = "微信公众号二级菜单最多只能有5个";
                        return false;
                    }
                    if (btn.SubButtons != null)
                    {
                        foreach (var subButton in btn.SubButtons)
                        {
                            if (subButton.Name.Length > 20)
                            {
                                msg = $"二级菜单[{subButton.Name}]，长度不能超过20";
                                return false;
                            }
                            else if (string.IsNullOrEmpty(subButton.Type))
                            {
                                msg = $"二级菜单[{subButton.Name}]的Type内容不能为空";
                                return false;
                            }
                            else if (subButton.Type.ToLower() == "view" && !BLL.Util.IsUrl(subButton.Value))
                            {
                                msg = $"二级菜单[{subButton.Name}]，中的Value必须是url格式";
                                return false;
                            }
                            else if (subButton.Type.ToLower() == "click")
                            {
                                //MemoryStream memStream = new MemoryStream();
                                //Senparc.Weixin.MP.AdvancedAPIs.MediaApi.GetForeverMedia(req.AppId, subButton.Value, memStream);
                                //if (memStream.Length < 0)
                                //{
                                //    msg = $"二级菜单[{subButton.Name}]，中的Value必须是图片格式";
                                //    return false;
                                //}
                            }
                        }
                    }
                }
                flag = true;
                msg = "";
            }
            return flag;
        }

        /// <summary>
        /// 根据请求的参数，写入到指定的配置文件中
        /// </summary>
        /// <param name="req">请求的参数对象</param>
        /// <returns>写入成功返回True，否则返回false</returns>
        private bool SaveMenuConfigFile(SaveWxMenuDataDto req)
        {
            try
            {
                var result = GetAppNameAndSecretByAppId(req.AppId);
                if (result != null)
                {
                    string wxTempDataFilePath = System.AppDomain.CurrentDomain.BaseDirectory + XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WxMenuDataInfo");
                    WxMenuDataDto menuData = GetMenuDataByAppId(wxTempDataFilePath, req.AppId);

                    WxMenuDataDto md = new WxMenuDataDto();
                    md.AppId = req.AppId;
                    md.WxNum = result.Item1;
                    md.AppSecret = result.Item2;
                    md.MenuClickList = new List<WxMenuClick>();
                    if (menuData != null)
                    {
                        md.SubscribeMsg = menuData.SubscribeMsg;
                        md.CustomMsgList = menuData.CustomMsgList;
                        md.DefaultCustomMsg = menuData.DefaultCustomMsg;
                    }
                    if (req != null && req.Buttons != null)
                    {
                        foreach (var button in req.Buttons)
                        {
                            if (button.Type.ToLower() == "click")
                            {
                                md.MenuClickList.Add(new WxMenuClick()
                                {
                                    EventKey = button.Value,
                                    MediaId = button.MediaId,
                                    Desc = $"[{button.Level}]{button.Name}"
                                });
                            }
                            if (button.SubButtons != null)
                            {
                                foreach (var subButton in button.SubButtons)
                                {
                                    if (subButton.Type.ToLower() == "click")
                                    {
                                        md.MenuClickList.Add(new WxMenuClick()
                                        {
                                            EventKey = subButton.Value,
                                            MediaId = subButton.MediaId,
                                            Desc = $"[{subButton.Level}]{subButton.Name}"
                                        });
                                    }
                                }
                            }
                        }
                        var list = GetWxMenuConfigData();
                        list[list.FindIndex(s => s.AppId == req.AppId)] = md;
                        string fileContent = JsonConvert.SerializeObject(list);

                        //XYAuto.Utils.FileHelper.RemoveFile( wxTempDataFilePath);
                        XYAuto.Utils.FileHelper.UpdateFile(fileContent, wxTempDataFilePath, Encoding.UTF8);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[SaveMenuConfigFile]根据请求的参数，写入到指定的配置文件中出错", ex);
                return false;
            }

            return false;
        }


        /// <summary>
        /// 保存微信菜单
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <param name="msg"></param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool SaveMenuData(SaveWxMenuDataDto req, ref string msg)
        {
            msg = "";
            WxMenuDataDto md = new WxMenuDataDto();
            md.AppId = req.AppId;
            //md.WxNum=req.

            ButtonGroup bg = new ButtonGroup();
            if (req.Buttons != null)
            {
                foreach (var wxMenuButton in req.Buttons)
                {
                    #region 初始化一级、二级菜单
                    if (wxMenuButton.Type == "view" || wxMenuButton.Type == "click")//一级菜单
                    {
                        switch (wxMenuButton.Type.ToLower())
                        {
                            case "click":
                                var scbutton = new SingleClickButton();
                                scbutton.type = wxMenuButton.Type.ToLower();
                                scbutton.name = wxMenuButton.Name;
                                scbutton.key = wxMenuButton.Value;
                                bg.button.Add(scbutton);
                                break;
                            case "view":
                                var svbutton = new SingleViewButton();
                                svbutton.type = wxMenuButton.Type.ToLower();
                                svbutton.name = wxMenuButton.Name;
                                svbutton.url = wxMenuButton.Value;
                                bg.button.Add(svbutton);
                                break;
                            default:
                                break;
                        }
                    }
                    else if (wxMenuButton.SubButtons != null)//二级菜单
                    {
                        var sButton = new SubButton()
                        {
                            name = wxMenuButton.Name
                        };
                        foreach (var subButton in wxMenuButton.SubButtons)
                        {
                            switch (subButton.Type.ToLower())
                            {
                                case "click":
                                    var scbutton = new SingleClickButton();
                                    scbutton.type = subButton.Type.ToLower();
                                    scbutton.name = subButton.Name;
                                    scbutton.key = subButton.Value;
                                    sButton.sub_button.Add(scbutton);
                                    break;
                                case "view":
                                    var svbutton = new SingleViewButton();
                                    svbutton.type = subButton.Type.ToLower();
                                    svbutton.name = subButton.Name;
                                    svbutton.url = subButton.Value;
                                    sButton.sub_button.Add(svbutton);
                                    break;
                                default:
                                    break;
                            }
                        }
                        bg.button.Add(sButton);
                    }
                    #endregion
                }
                ClearMenu(req.AppId);
                return AddMenu(req.AppId, bg, ref msg) && SaveMenuConfigFile(req);
            }
            return false;
        }

        /// <summary>
        /// 清除公众号下关注回复，以及自定义回复信息
        /// </summary>
        /// <param name="appId">公众号的AppId</param>
        /// <returns>成功返回true，否则返回fasle</returns>
        public bool ClearCustomMsg(string appId)
        {
            try
            {
                string wxTempDataFilePath = System.AppDomain.CurrentDomain.BaseDirectory + XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WxMenuDataInfo");
                WxMenuDataDto menuData = GetMenuDataByAppId(wxTempDataFilePath, appId);
                if (menuData != null)
                {
                    menuData.SubscribeMsg = null;
                    menuData.CustomMsgList = null;

                    var list = GetWxMenuConfigData();
                    list[list.FindIndex(s => s.AppId == appId)] = menuData;
                    string fileContent = JsonConvert.SerializeObject(list);
                    XYAuto.Utils.FileHelper.UpdateFile(fileContent, wxTempDataFilePath, Encoding.UTF8);
                    return true;
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[ClearCustomMsg]清除公众号下关注回复，以及自定义回复信息出错", ex);
                return false;
            }
            return false;
        }

        /// <summary>
        /// 保存自定义回复及关注回复信息
        /// </summary>
        /// <param name="req"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SaveCustomMsgData(WxMenuDataDto req, ref string msg)
        {
            try
            {
                string wxTempDataFilePath = System.AppDomain.CurrentDomain.BaseDirectory + XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WxMenuDataInfo");
                WxMenuDataDto menuData = GetMenuDataByAppId(wxTempDataFilePath, req.AppId);
                if (menuData != null)
                {
                    var list = GetWxMenuConfigData();
                    var currentMenu = list[list.FindIndex(s => s.AppId == req.AppId)];
                    currentMenu = menuData;
                    currentMenu.SubscribeMsg = req.SubscribeMsg;
                    currentMenu.CustomMsgList = req.CustomMsgList;
                    currentMenu.DefaultCustomMsg = req.DefaultCustomMsg;
                    currentMenu.SubArticleInfo = req.SubArticleInfo;
                    list[list.FindIndex(s => s.AppId == req.AppId)] = currentMenu;
                    string fileContent = JsonConvert.SerializeObject(list);
                    XYAuto.Utils.FileHelper.UpdateFile(fileContent, wxTempDataFilePath, Encoding.UTF8);
                    return true;
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[SaveCustomMsgData]保存自定义回复及关注回复信息-出错", ex);
                return false;
            }
            return false;
        }
    }
}
