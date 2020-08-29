using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.Entities.Examine;
using XYAuto.BUOC.IP2017.Entities.Result;
using static XYAuto.BUOC.IP2017.Entities.ENUM.ENUM;

namespace XYAuto.BUOC.IP2017.BLL.ExamineLabel
{
    public class BasicLabel
    {
        public static readonly BasicLabel Instance = new BasicLabel();
        /// <summary>
        /// zlb 2017-10-23
        /// 划分标签
        /// </summary>
        /// <param name="targetName">加载到对象名称</param>
        /// <param name="labelList">普通标签集合</param>
        /// <param name="ipLabelList">ip标签集合</param>
        /// <param name="lbStatic">标签统计类</param>
        /// <param name="IsMosaic">是否拼接字符串</param>
        public void DivideLabel(string targetName, List<BasicLabelInfo> labelList, List<IpLabelInfo> ipLabelList, LabelStatistics lbStatic, bool IsMosaic)
        {
            #region 划分普通标签
            if (labelList != null && labelList.Count > 0)
            {
                if (IsMosaic)
                {
                    lbStatic.listCategory.Add("NewInfo", "");
                    lbStatic.listDistributeScene.Add("NewInfo", "");
                    lbStatic.listMarketScene.Add("NewInfo", "");
                }
                //lbStatic.listCategory.Add(targetName, new List<Dictionary<string, object>>());
                lbStatic.listDistributeScene.Add(targetName, new List<Dictionary<string, object>>());
                lbStatic.listMarketScene.Add(targetName, new List<Dictionary<string, object>>());
                var listClass = labelList.GroupBy(t => new { t.Type }).ToList();
                foreach (var item in listClass)
                {
                    var varTagInfo = item.ToList().GroupBy(i => new { i.DictId, i.DictName }).ToList();
                    List<Dictionary<string, object>> listTag = new List<Dictionary<string, object>>();
                    StringBuilder sbNewInfo = new StringBuilder();
                    foreach (var itemTagInfo in varTagInfo)
                    {
                        Dictionary<string, object> dicTag = new Dictionary<string, object>();
                        dicTag.Add("DictId", itemTagInfo.Key.DictId);
                        dicTag.Add("DictName", itemTagInfo.Key.DictName);
                        sbNewInfo.Append(itemTagInfo.Key.DictName + ",");
                        listTag.Add(dicTag);
                    }
                    string strNewInfo = sbNewInfo.ToString();
                    switch ((EnumLabelType)item.Key.Type)
                    {
                        //case EnumLabelType.分类:
                        //    if (IsMosaic)
                        //    {
                        //        lbStatic.listCategory["NewInfo"] = strNewInfo.Substring(0, strNewInfo.Length - 1);
                        //    }
                        //    lbStatic.listCategory[targetName] = listTag;
                        //    break;
                        case EnumLabelType.分发场景:
                            if (IsMosaic)
                            {
                                lbStatic.listDistributeScene["NewInfo"] = strNewInfo.Substring(0, strNewInfo.Length - 1);
                            }
                            lbStatic.listDistributeScene[targetName] = listTag;
                            break;
                        case EnumLabelType.市场场景:
                            if (IsMosaic)
                            {
                                lbStatic.listMarketScene["NewInfo"] = strNewInfo.Substring(0, strNewInfo.Length - 1);
                            }
                            lbStatic.listMarketScene[targetName] = listTag;
                            break;
                    }
                }
            }
            #endregion
            #region 划分ip标签
            if (ipLabelList != null && ipLabelList.Count > 0)
            {
                var listClass = ipLabelList.GroupBy(t => new { t.IpId, t.IpName }).ToList();
                List<Dictionary<string, object>> listIp = new List<Dictionary<string, object>>();
                StringBuilder sbNewInfo = new StringBuilder();
                foreach (var item in listClass)
                {
                    Dictionary<string, object> dicIp2 = new Dictionary<string, object>();
                    dicIp2.Add("DictId", item.Key.IpId);
                    dicIp2.Add("DictName", item.Key.IpName);
                    sbNewInfo.Append(item.Key.IpName + ":");
                    List<IpLabelInfo> sonIpList = new List<IpLabelInfo>();
                    foreach (var itemSonIPInfo in item.ToList())
                    {
                        sonIpList.AddRange(ipLabelList.Where(t => t.IpId == itemSonIPInfo.IpId).ToList());
                    }
                    var varSopIp = sonIpList.GroupBy(t => new { t.SonIpId, t.SonIpName });
                    List<Dictionary<string, object>> listSonIp = new List<Dictionary<string, object>>();
                    foreach (var itemSonIpInfo in varSopIp)
                    {
                        Dictionary<string, object> dicSonIp = new Dictionary<string, object>();
                        dicSonIp.Add("DictId", itemSonIpInfo.Key.SonIpId);
                        dicSonIp.Add("DictName", itemSonIpInfo.Key.SonIpName);
                        sbNewInfo.Append(itemSonIpInfo.Key.SonIpName + "->");
                        var varLabel = itemSonIpInfo.GroupBy(t => new { t.LabelName });
                        List<Dictionary<string, object>> SoplabelList = new List<Dictionary<string, object>>();
                        foreach (var itemLabelInfo in varLabel)
                        {
                            Dictionary<string, object> dicLabel = new Dictionary<string, object>();
                            dicLabel.Add("DictName", itemLabelInfo.Key.LabelName);
                            sbNewInfo.Append(itemLabelInfo.Key.LabelName + " ");
                            SoplabelList.Add(dicLabel);
                        }
                        dicSonIp.Add("Label", SoplabelList);
                        listSonIp.Add(dicSonIp);
                    }
                    dicIp2.Add("SubIP", listSonIp);
                    listIp.Add(dicIp2);
                }
                if (IsMosaic)
                {
                    string strNewInfo = sbNewInfo.ToString();
                    lbStatic.listIPLabel.Add("NewInfo", strNewInfo.Substring(0, strNewInfo.Length - 1));
                }
                lbStatic.listIPLabel.Add(targetName, listIp);
            }
            #endregion
        }
        /// <summary>
        /// zlb 2017-10-23
        /// 统计某一批次的待审核和已审核标签
        /// </summary>
        /// <param name="BatchAuditID">审核批次ID</param>
        /// <param name="lbStatic">标签统计类</param>
        public bool StatisticsLabel(int BatchAuditID, LabelStatistics lbStatic)
        {
            DataSet dsPending = Dal.ExamineLabel.BasicLabel.Instance.QueryPendingAuditLabelList(BatchAuditID);
            List<BasicLabelInfo> labelOriginalList = Util.DataTableToList<BasicLabelInfo>(dsPending.Tables[0]);
            List<IpLabelInfo> ipLabelOriginalList = Util.DataTableToList<IpLabelInfo>(dsPending.Tables[1]);
            DataSet dsAudited = Dal.ExamineLabel.BasicLabel.Instance.QueryAuditedLabelList(BatchAuditID);
            List<BasicLabelInfo> labelAuditList = Util.DataTableToList<BasicLabelInfo>(dsAudited.Tables[0]);
            List<IpLabelInfo> ipLabelAuditlList = Util.DataTableToList<IpLabelInfo>(dsAudited.Tables[1]);
            DivideLabel("Original", labelOriginalList, ipLabelOriginalList, lbStatic, false);
            DivideLabel("Audit", labelAuditList, ipLabelAuditlList, lbStatic, false);
            if (ipLabelOriginalList != null && ipLabelAuditlList != null)
            {
                foreach (var item in ipLabelOriginalList)
                {
                    IpLabelInfo ipLabelInfo = ipLabelAuditlList.Where(t => t.IpId == item.IpId && t.IpName == item.IpName && t.SonIpId == item.SonIpId && t.SonIpName == item.SonIpName && t.LabelName == item.LabelName).FirstOrDefault();
                    if (ipLabelInfo != null)
                    {
                        ipLabelAuditlList.Remove(ipLabelInfo);
                    }
                    else
                    {
                        return false;
                    }
                }
                if (ipLabelAuditlList.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// zlb 2017-10-24
        /// 统计某一批次待审核或最终结果标签
        /// </summary>
        /// <param name="dsPending">标签表</param>
        /// <param name="dicAll">标签字典类</param>
        /// <param name="SelectType">查询类型（1审核 2修改）</param>
        public void DivideLabel(DataSet dsPending, Dictionary<string, object> dicAll, int SelectType)
        {

            List<BasicLabelInfo> labelList = Util.DataTableToList<BasicLabelInfo>(dsPending.Tables[0]);
            List<IpLabelInfo> ipLabelList = Util.DataTableToList<IpLabelInfo>(dsPending.Tables[1]);
            #region 划分普通标签
            if (labelList != null && labelList.Count > 0)
            {
                var listClass = labelList.GroupBy(t => new { t.Type }).ToList();
                foreach (var item in listClass)
                {
                    var varTagInfo = item.ToList().GroupBy(i => new { i.DictId, i.DictName }).ToList();
                    Dictionary<string, object> dicTagInfo = new Dictionary<string, object>();
                    List<Dictionary<string, object>> listTag = new List<Dictionary<string, object>>();
                    foreach (var itemTagInfo in varTagInfo)
                    {
                        Dictionary<string, object> dicTag = new Dictionary<string, object>();
                        dicTag.Add("DictId", itemTagInfo.Key.DictId);
                        dicTag.Add("DictName", itemTagInfo.Key.DictName);
                        List<string> listCreater = itemTagInfo.Select(t => t.Creater).Distinct().ToList();
                        dicTag.Add("Creater", listCreater);
                        dicTag.Add("SelectCount", itemTagInfo.Count());
                        listTag.Add(dicTag);
                    }
                    switch ((EnumLabelType)item.Key.Type)
                    {
                        //case EnumLabelType.分类:
                        //    LoadBasicLabel(EnumLabelType.分类, listTag);
                        //    dicAll["Category"] = listTag;
                        //    break;
                        case EnumLabelType.分发场景:
                            LoadBasicLabel(EnumLabelType.分发场景, listTag);
                            dicAll["DistributeScene"] = listTag;
                            break;
                        case EnumLabelType.市场场景:
                            LoadBasicLabel(EnumLabelType.市场场景, listTag);
                            dicAll["MarketScene"] = listTag;
                            break;
                    }
                }
            }
            if (SelectType == 2)
            {
                if ((dicAll["DistributeScene"] as List<Dictionary<string, object>>).Count <= 0)
                {
                    List<Dictionary<string, object>> listTag = new List<Dictionary<string, object>>();
                    LoadBasicLabel(EnumLabelType.分发场景, listTag);
                    dicAll["DistributeScene"] = listTag;
                }
                //if ((dicAll["Category"] as List<Dictionary<string, object>>).Count <= 0)
                //{
                //    List<Dictionary<string, object>> listTag = new List<Dictionary<string, object>>();
                //    LoadBasicLabel(EnumLabelType.分类, listTag);
                //    dicAll["Category"] = listTag;
                //}
                if ((dicAll["MarketScene"] as List<Dictionary<string, object>>).Count <= 0)
                {
                    List<Dictionary<string, object>> listTag = new List<Dictionary<string, object>>();
                    LoadBasicLabel(EnumLabelType.市场场景, listTag);
                    dicAll["MarketScene"] = listTag;
                }
            }

            #endregion

            #region 划分ip标签
            if (ipLabelList != null && ipLabelList.Count > 0)
            {
                DataTable dt = Dal.ExamineLabel.BasicLabel.Instance.SelectIpLabel();
                List<IpLabelInfo> listBasicSonIp = Util.DataTableToList<IpLabelInfo>(dt);
                var listClass = ipLabelList.GroupBy(t => new { t.IpId, t.IpName }).ToList();
                List<Dictionary<string, object>> listIp = new List<Dictionary<string, object>>();
                foreach (var item in listClass)
                {
                    Dictionary<string, object> dicIp2 = new Dictionary<string, object>();
                    dicIp2.Add("DictId", item.Key.IpId);
                    dicIp2.Add("DictName", item.Key.IpName);
                    List<string> listCreater = item.Select(t => t.IpCreater).Distinct().ToList();
                    dicIp2.Add("Creater", listCreater);
                    dicIp2.Add("SelectCount", item.GroupBy(t => t.BatchID).Count());
                    List<IpLabelInfo> sonIpList = new List<IpLabelInfo>();
                    foreach (var itemSonIPInfo in item.ToList())
                    {
                        sonIpList.AddRange(ipLabelList.Where(t => t.IpId == itemSonIPInfo.IpId).ToList());
                    }
                    var varSopIp = sonIpList.GroupBy(t => new { t.SonIpId, t.SonIpName });
                    List<Dictionary<string, object>> listSonIp = new List<Dictionary<string, object>>();
                    foreach (var itemSonIpInfo in varSopIp)
                    {
                        Dictionary<string, object> dicSonIp = new Dictionary<string, object>();
                        dicSonIp.Add("DictId", itemSonIpInfo.Key.SonIpId);
                        dicSonIp.Add("DictName", itemSonIpInfo.Key.SonIpName);
                        List<string> listSonIpCreater = itemSonIpInfo.Select(t => t.SopIpCreater).Distinct().ToList();
                        dicSonIp.Add("Creater", listSonIpCreater);
                        dicSonIp.Add("SelectCount", itemSonIpInfo.GroupBy(t => t.BatchID).Count());
                        var varLabel = itemSonIpInfo.GroupBy(t => new { t.LabelName });
                        List<Dictionary<string, object>> SoplabelList = new List<Dictionary<string, object>>();
                        foreach (var itemLabelInfo in varLabel)
                        {
                            Dictionary<string, object> dicLabel = new Dictionary<string, object>();
                            dicLabel.Add("DictName", itemLabelInfo.Key.LabelName);
                            List<string> listLabelCreater = itemLabelInfo.Select(t => t.SopIpCreater).Distinct().ToList();
                            dicLabel.Add("Creater", listLabelCreater);
                            dicLabel.Add("SelectCount", itemLabelInfo.GroupBy(t => t.BatchID).Count());
                            SoplabelList.Add(dicLabel);
                        }
                        dicSonIp.Add("Label", SoplabelList);
                        listSonIp.Add(dicSonIp);
                    }
                    LoadBasicSonIpLabel(item.Key.IpId, listBasicSonIp, listSonIp);
                    dicIp2.Add("SubIP", listSonIp);
                    listIp.Add(dicIp2);
                }
                LoadBasicLabel(EnumLabelType.IP, listIp);
                dicAll["IPLabel"] = listIp;
            }
            #endregion
        }
        /// <summary>
        /// zlb 2017-10-26
        /// 组装最终结果上一次修改的标签
        /// </summary>
        /// <param name="MediaResultID">结果ID</param>
        /// <param name="lbStatic">标签类</param>
        public void AssembleOldLabel(int MediaResultID, LabelStatistics lbStatic)
        {
            DataSet dsPending = Dal.ExamineLabel.BasicLabel.Instance.QueryResultLabelList(MediaResultID);
            List<BasicLabelInfo> labelList = Util.DataTableToList<BasicLabelInfo>(dsPending.Tables[0]);
            List<IpLabelInfo> ipLabelList = Util.DataTableToList<IpLabelInfo>(dsPending.Tables[1]);
            DivideLabel("Audit", labelList, ipLabelList, lbStatic, true);
            lbStatic.listCategory.Add("OldInfo", "");
            lbStatic.listMarketScene.Add("OldInfo", "");
            lbStatic.listDistributeScene.Add("OldInfo", "");
            lbStatic.listIPLabel.Add("OldInfo", "");
            if (labelList != null)
            {
                string optContent = Dal.Result.ResultLabel.Instance.SelectOptContent(MediaResultID);
                if (!string.IsNullOrEmpty(optContent))
                {
                    ResultLabelJson result = JsonConvert.DeserializeObject<ResultLabelJson>(optContent);
                    if (result != null)
                    {
                        //if (result.Category != null)
                        //{
                        //    StringBuilder sbOld = new StringBuilder();
                        //    foreach (var item in result.Category)
                        //    {
                        //        sbOld.Append(item.DictName + ",");
                        //    }
                        //    string strOld = sbOld.ToString();
                        //    lbStatic.listCategory["OldInfo"] = strOld.Substring(0, strOld.Length - 1);
                        //}
                        if (result.MarketScene != null)
                        {
                            StringBuilder sbOld = new StringBuilder();
                            foreach (var item in result.MarketScene)
                            {
                                sbOld.Append(item.DictName + ",");
                            }
                            string strOld = sbOld.ToString();
                            lbStatic.listMarketScene["OldInfo"] = strOld.Substring(0, strOld.Length - 1);
                        }
                        if (result.DistributeScene != null)
                        {
                            StringBuilder sbOld = new StringBuilder();
                            foreach (var item in result.DistributeScene)
                            {
                                sbOld.Append(item.DictName + ",");
                            }
                            string strOld = sbOld.ToString();
                            lbStatic.listDistributeScene["OldInfo"] = strOld.Substring(0, strOld.Length - 1);
                        }
                        if (result.IPLabel != null)
                        {
                            StringBuilder sbOld = new StringBuilder();
                            foreach (var item in result.IPLabel)
                            {
                                sbOld.Append(item.DictName + ":");
                                if (item.SubIP != null)
                                {
                                    foreach (var itemSubIP in item.SubIP)
                                    {
                                        sbOld.Append(itemSubIP.DictName + "->");
                                        if (itemSubIP.Label != null)
                                        {
                                            foreach (var itemLabel in itemSubIP.Label)
                                            {
                                                sbOld.Append(itemLabel.DictName + " ");
                                            }
                                        }
                                    }
                                }
                            }
                            string strOld = sbOld.ToString();
                            lbStatic.listIPLabel["OldInfo"] = strOld.Substring(0, strOld.Length - 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// zlb 2017-10-27
        /// 填充基础标签
        /// </summary>
        /// <param name="LabelType"></param>
        /// <param name="listTag"></param>
        private void LoadBasicLabel(EnumLabelType LabelType, List<Dictionary<string, object>> listTag)
        {
            DataTable dt = Dal.ExamineLabel.BasicLabel.Instance.SelectBasicLabel((int)LabelType);
            List<BasicLabelInfo> listBasic = Util.DataTableToList<BasicLabelInfo>(dt);
            if (listBasic != null)
            {
                foreach (var item in listBasic)
                {
                    if (listTag.Where(t => t["DictId"].ToString() == item.DictId.ToString()).Count() <= 0)
                    {
                        Dictionary<string, object> dicTag = new Dictionary<string, object>();
                        dicTag.Add("DictId", item.DictId);
                        dicTag.Add("DictName", item.DictName);
                        dicTag.Add("Creater", null);
                        dicTag.Add("SelectCount", 0);
                        listTag.Add(dicTag);
                    }
                }
            }
        }
        /// <summary>
        /// zlb 2017-10-27
        /// 填充基础子IP标签
        /// </summary>
        /// <param name="IpId"></param>
        /// <param name="listIP"></param>
        /// <param name="listTag"></param>
        private void LoadBasicSonIpLabel(int IpId, List<IpLabelInfo> listIP, List<Dictionary<string, object>> listTag)
        {
            if (listIP != null)
            {
                List<IpLabelInfo> listPartIp = listIP.Where(t => t.IpId == IpId).ToList();
                if (listPartIp != null)
                {
                    foreach (var item in listPartIp)
                    {
                        if (listTag.Where(t => t["DictId"].ToString() == item.SonIpId.ToString()).Count() <= 0)
                        {
                            Dictionary<string, object> dicTag = new Dictionary<string, object>();
                            dicTag.Add("DictId", item.SonIpId);
                            dicTag.Add("DictName", item.SonIpName);
                            dicTag.Add("Creater", null);
                            dicTag.Add("SelectCount", 0);
                            listTag.Add(dicTag);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// zlb 2017-10-28
        /// 插入基础标签
        /// </summary>
        /// <param name="ListIp"></param>
        /// <param name="UserId"></param>
        public void InsertTitleBasicInfo(List<IpLabel> ListIp, int UserId)
        {
            if (ListIp != null && ListIp.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                List<string> listCustomLable = new List<string>();
                foreach (var itemIp in ListIp)
                {
                    //item.DictId
                    foreach (var itemSonIp in itemIp.SubIP)
                    {
                        foreach (var itemCustomLable in itemSonIp.Label)
                        {
                            if (!listCustomLable.Contains(itemCustomLable.DictName))
                            {
                                listCustomLable.Add(itemCustomLable.DictName);
                                sb.Append("'" + itemCustomLable.DictName + "',");
                            }
                        }
                    }
                }
                string listLableName = sb.ToString();
                listLableName = listLableName.Substring(0, listLableName.Length - 1);
                DataTable dt = Dal.ExamineLabel.BasicLabel.Instance.SelectTitleBasicInfo(listLableName);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (listCustomLable.Contains(dt.Rows[i]["Name"].ToString()))
                        {
                            listCustomLable.Remove(dt.Rows[i]["Name"].ToString());
                        }
                    }
                }
                if (listCustomLable.Count > 0)
                {
                    Dal.ExamineLabel.BasicLabel.Instance.InsertTitleBasicInfo(listCustomLable, UserId);
                }
                foreach (var itemIp in ListIp)
                {
                    foreach (var itemSonIp in itemIp.SubIP)
                    {
                        StringBuilder sbLableName = new StringBuilder();
                        foreach (var itemCustomLable in itemSonIp.Label)
                        {
                            sbLableName.Append("'" + itemCustomLable.DictName + "',");
                        }
                        listLableName = sbLableName.ToString();
                        listLableName = listLableName.Substring(0, listLableName.Length - 1);
                        Dal.ExamineLabel.BasicLabel.Instance.DeleteIPTitleInfo(itemIp.DictId, itemSonIp.DictId, listLableName);
                        DataTable dtLableInfo = Dal.ExamineLabel.BasicLabel.Instance.SelectTitleBasicInfo(listLableName);
                        if (dtLableInfo != null && dtLableInfo.Rows.Count > 0)
                        {
                            Dal.ExamineLabel.BasicLabel.Instance.InsertIPTitleInfo(itemIp.DictId, itemSonIp.DictId, UserId, dtLableInfo);
                        }
                    }
                }
            }

        }
    }
}
