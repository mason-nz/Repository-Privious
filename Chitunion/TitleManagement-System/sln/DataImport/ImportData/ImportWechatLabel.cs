using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.BLL;
using XYAuto.BUOC.IP2017.BLL.ResultLabel;
using XYAuto.BUOC.IP2017.Entities.Examine;
using XYAuto.BUOC.IP2017.Entities.MediaBase;
using static XYAuto.BUOC.IP2017.Entities.ENUM.ENUM;
using XYAuto.BUOC.IP2017.Entities.TitleBasicInfo;

namespace DataImport.ImportData
{
    public class ImportWechatLabel : Import
    {
        public readonly static ImportWechatLabel Instance = new ImportWechatLabel();

        private readonly int importUserID = int.Parse(ConfigurationManager.AppSettings["CarMapIP_UserID"]);
        public void ImportWechatLabell(int CategoryCount, int MarkCount, int XyCount, int IpCount, string excelPath)
        {
            Loger.Log4Net.Info("[ImportWechatLabel]程序开始运行开始...");
            Loger.Log4Net.Info("[ImportWechatLabel]Excel位置:" + excelPath);
            Loger.Log4Net.Info("[ImportWechatLabel]统一导入的UserID：" + importUserID);
            if (!File.Exists(excelPath))
            {
                Loger.Log4Net.Error($"[ImportWechatLabel]导入文件[{excelPath}]不存在");
                return;
            }
            DataSet ds = ConvertExcelToDataSet(new string[] { "汽车垂直资源" }, excelPath);
            if (ds == null && ds.Tables.Count == 0)
            {
                Loger.Log4Net.Error($"[ImportWechatLabel]导入文件[{excelPath}][汽车垂直资源]sheet页不存在");
                return;
            }

            DataView dataView = ds.Tables["汽车垂直资源"].DefaultView;
            DataTable dt = dataView.ToTable();
            if (dt.Rows.Count == 0)
            {
                Loger.Log4Net.Error($"[ImportWechatLabel]导入文件[{excelPath}][汽车垂直资源]没有数据");
                return;
            }
            DataTable dtCategory = XYAuto.BUOC.IP2017.Dal.MediaBase.MediaBase.Instance.SelectMediaCategory(47);
            List<DicInfo> listCategory = Util.DataTableToList<DicInfo>(dtCategory);
            List<TitleBasicInfo> listAll = XYAuto.BUOC.IP2017.BLL.TitleBasicInfo.TitleBasicInfo.Instance.SelectAll();
            List<TitleBasicInfo> listMark = listAll.Where(t => t.Type == (int)EnumLabelType.市场场景).ToList();
            List<TitleBasicInfo> listXy = listAll.Where(t => t.Type == (int)EnumLabelType.分发场景).ToList();
            List<TitleBasicInfo> listIP = listAll.Where(t => t.Type == (int)EnumLabelType.IP).ToList();
            List<TitleBasicInfo> listSonIP = listAll.Where(t => t.Type == (int)EnumLabelType.子IP).ToList();
            List<TitleBasicInfo> listLabel = listAll.Where(t => t.Type == (int)EnumLabelType.标签).ToList();
            List<string> listMediaApp = new List<string>();
            DataTable dtMediaWechat = XYAuto.BUOC.IP2017.Dal.MediaBase.MediaBase.Instance.SelectMediaWechat();
            if (listMediaApp != null)
            {
                for (int i = 0; i < dtMediaWechat.Rows.Count; i++)
                {
                    listMediaApp.Add(dtMediaWechat.Rows[i]["WxNumber"].ToString());
                }
            }
            int InsertError = 0;
            int InsertSuccess = 0;
            int InsertModel = 0;
            int RepeatModel = 0;
            int irow = 1;
            foreach (DataRow row in dt.Rows)
            {
                Loger.Log4Net.Info($"[ImportWechatLabel]便历行：{++irow}");
                string Number = "";
                string Name = "";
                string HeadUrl = "";
                if (string.IsNullOrWhiteSpace(row["媒体账号"].ToString()))
                {
                    continue;
                }
                else
                {
                    Number = row["媒体账号"].ToString().Trim();
                    Name = row["名称"].ToString().Trim();
                    if (!listMediaApp.Contains(Number))
                    {
                        MediaWechat mediaWechat = new MediaWechat();
                        mediaWechat.WxNumber = Number;
                        mediaWechat.NickName = Name;
                        string DailyLive = row["粉丝数"].ToString().Trim();
                        if (!string.IsNullOrWhiteSpace(DailyLive))
                        {
                            int fansCount = 0;
                            int.TryParse(DailyLive, out fansCount);
                            mediaWechat.FansCount = fansCount;
                        }
                        mediaWechat.IsAreaMedia = row["区域媒体"].ToString().Trim() == "是" ? 1 : 0;
                        mediaWechat.LevelType = row["媒体级别"].ToString().Trim() == "意见领袖" ? 4001 : 4002;
                        mediaWechat.IsVerify = row["微信认证"].ToString().Trim() == "Y" ? 1 : 0;
                        mediaWechat.Sign = row["描述"].ToString().Trim() == "#N/A" ? "" : row["描述"].ToString().Trim();
                        int ResultMedia = XYAuto.BUOC.IP2017.Dal.MediaBase.MediaBase.Instance.InsertMediaWechat(mediaWechat, importUserID);
                        if (ResultMedia <= 0)
                        {
                            InsertError++;
                            Loger.Log4Net.Error($"[ImportWechatLabel]媒体信息媒体账号：{Number}插入失败");
                            continue;
                        }
                        for (int i = 1; i <= CategoryCount; i++)
                        {
                            string dicName = row["分类" + i].ToString().Trim();
                            if (!string.IsNullOrEmpty(dicName))
                            {
                                DicInfo tbModel = listCategory.Where(x => x.DictName == dicName).FirstOrDefault();
                                if (tbModel != null)
                                {
                                    MediaCategory m = new MediaCategory();
                                    m.MediaType = 14001;
                                    m.WxID = ResultMedia;
                                    m.SortNumber = i == 1 ? 1 : 0;
                                    m.CategoryID = tbModel.DictId;
                                    if (XYAuto.BUOC.IP2017.Dal.MediaBase.MediaBase.Instance.InsertMediaCategory(m) <= 0)
                                    {
                                        Loger.Log4Net.Error($"[ImportWechatLabel]媒体[{Number}]的分类[{dicName}]在插入失败");
                                    }
                                }
                                else
                                    Loger.Log4Net.Error($"[ImportWechatLabel]媒体[{Number}]的分类[{dicName}]在分类中未找到");
                            }
                        }
                        InsertSuccess++;
                    }
                }
                int resultID = XYAuto.BUOC.IP2017.Dal.Result.ResultLabel.Instance.GetResultID((int)EnumTaskType.媒体, -2, Number, (int)EnumMediaType.微信);
                if (resultID <= 0)
                {
                    resultID = ResultLabel.Instance.InsertResult((int)EnumTaskType.媒体, -2, -2, (int)EnumMediaType.微信, Number, Name, HeadUrl,"", importUserID);
                }
                if (resultID <= 0)
                {
                    Loger.Log4Net.Error($"[ImportWechatLabel]模板文件媒体名称：{Name}插入失败");
                    continue;
                }
                InsertModel++;
                AuditLabel auditLabel = new AuditLabel();
                auditLabel.TaskType = (int)EnumTaskType.媒体;
                auditLabel.DistributeScene = new List<BasicLabelInfo>();
                auditLabel.MarketScene = new List<BasicLabelInfo>();
                #region 市场场景
                for (int i = 1; i <= MarkCount; i++)
                {
                    string dicName = row["场景" + i].ToString().Trim();
                    if (!string.IsNullOrEmpty(dicName))
                    {
                        TitleBasicInfo tbModel = listMark.Where(x => x.Name == dicName).FirstOrDefault();

                        if (tbModel != null)
                        {
                            auditLabel.MarketScene.Add(new BasicLabelInfo { DictId = tbModel.TitleID, DictName = dicName });
                        }
                        else
                            auditLabel.MarketScene.Add(new BasicLabelInfo { DictId = -2, DictName = dicName });
                    }
                }
                auditLabel.MarketScene = auditLabel.MarketScene.Where((x, k) => auditLabel.MarketScene.FindIndex(z => z.DictName.ToUpper() == x.DictName.ToUpper()) == k).ToList();
                if (auditLabel.MarketScene.Count <= 0)
                {
                    Loger.Log4Net.Error($"[ImportWechatLabel]媒体[{Name}]未插入市场场景");
                }
                #endregion
                #region 分发场景
                for (int i = 1; i <= XyCount; i++)
                {
                    string dicName = row["行圆场景" + i].ToString().Trim();
                    if (!string.IsNullOrEmpty(dicName))
                    {
                        if (dicName.Contains("场景") && dicName != "IP场景")
                        {
                            dicName = dicName.Replace("场景", "");
                        }
                        TitleBasicInfo tbModel = listXy.Where(x => x.Name == dicName).FirstOrDefault();
                        if (tbModel != null)
                        {
                            auditLabel.DistributeScene.Add(new BasicLabelInfo { DictId = tbModel.TitleID, DictName = dicName });
                        }
                        else
                            Loger.Log4Net.Error($"[ImportWechatLabel]媒体[{Name}]的分发场景[{dicName}]在标签库未找到");
                    }
                }
                auditLabel.DistributeScene = auditLabel.DistributeScene.Where((x, k) => auditLabel.DistributeScene.FindIndex(z => z.DictName.ToUpper() == x.DictName.ToUpper()) == k).ToList();
                if (auditLabel.DistributeScene.Count <= 0)
                {
                    Loger.Log4Net.Error($"[ImportWechatLabel]媒体[{Name}]未插入分发场景");
                }
                #endregion
                #region IP
                auditLabel.IPLabel = new List<IpLabel>();
                for (int i = 1; i <= IpCount; i++)
                {
                    string ip = row["IP" + i.ToString()].ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(ip))
                    {
                        TitleBasicInfo tbModel = listIP.Where(x => x.Name.ToUpper() == ip.ToUpper()).FirstOrDefault();
                        int newIpID = 0;
                        if (tbModel == null)
                        {
                            newIpID = XYAuto.BUOC.IP2017.Dal.IPTitleInfo.IPTitleInfo.Instance.InsertIpInfo(ip, (int)EnumLabelType.IP, importUserID);
                            if (newIpID > 0)
                            {
                                tbModel = new TitleBasicInfo();
                                tbModel.TitleID = newIpID;
                                tbModel.Name = ip;
                                listIP.Add(tbModel);
                                Loger.Log4Net.Info($"[ImportWechatLabel]母IP：[{ip}]插入成功");
                            }
                            else
                            {
                                Loger.Log4Net.Error($"[ImportWechatLabel]母IP：[{ip}]插入失败");
                                continue;
                            }
                        }

                        IpLabel IpInfo = new IpLabel();
                        IpInfo.DictId = tbModel.TitleID;
                        IpInfo.DictName = ip;
                        IpInfo.SubIP = new List<SonIpLabel>();
                        string columnName = "子IP" + i;
                        if (row.Table.Columns.Contains(columnName))
                        {
                            string subipSplit = row[columnName].ToString().Trim();

                            if (!string.IsNullOrWhiteSpace(subipSplit))
                            {
                                foreach (var subips in subipSplit.Split(new string[] { "、" }, StringSplitOptions.None))
                                {
                                    if (!string.IsNullOrWhiteSpace(subips))
                                    {
                                        string subip = subips.Trim();
                                        TitleBasicInfo tbModelSonIp = new TitleBasicInfo();
                                        tbModelSonIp = listSonIP.Where(x => x.Name.ToUpper() == subip.ToUpper()).FirstOrDefault();
                                        int newSonIpID = 0;
                                        if (tbModelSonIp == null)
                                        {
                                            newSonIpID = XYAuto.BUOC.IP2017.Dal.IPTitleInfo.IPTitleInfo.Instance.InsertIpInfo(subip, (int)EnumLabelType.子IP, importUserID);
                                            if (newSonIpID > 0)
                                            {
                                                tbModelSonIp = new TitleBasicInfo();
                                                tbModelSonIp.TitleID = newSonIpID;
                                                tbModelSonIp.Name = subip;
                                                listSonIP.Add(tbModelSonIp);
                                                Loger.Log4Net.Info($"[ImportWechatLabel]子IP：[{subip}]插入成功");
                                            }
                                            else
                                            {
                                                Loger.Log4Net.Error($"[ImportWechatLabel]子IP：[{subip}]插入失败");
                                                continue;
                                            }
                                        }
                                        SonIpLabel sonIpLabel = new SonIpLabel();
                                        sonIpLabel.DictId = tbModelSonIp.TitleID;
                                        sonIpLabel.DictName = subip;
                                        string label = row["标签"].ToString().Trim();
                                        sonIpLabel.Label = new List<BasicLabelInfo>();
                                        foreach (var varLabel in label.Split(new string[] { "、" }, StringSplitOptions.None))
                                        {
                                            if (!string.IsNullOrWhiteSpace(varLabel))
                                            {
                                                int newLabelID = 0;
                                                int LabelID = listLabel.Where(t => t.Name.ToUpper() == varLabel.Trim().ToUpper()).Select(t => t.TitleID).FirstOrDefault();
                                                if (LabelID <= 0)
                                                {
                                                    newLabelID = XYAuto.BUOC.IP2017.Dal.IPTitleInfo.IPTitleInfo.Instance.InsertIpInfo(varLabel.Trim(), (int)EnumLabelType.标签, importUserID);
                                                    if (newLabelID <= 0)
                                                    {
                                                        Loger.Log4Net.Error($"[ImportWechatLabel]标签：[{varLabel.Trim()}]插入失败");
                                                        continue;
                                                    }
                                                    listLabel.Add(new TitleBasicInfo() { TitleID = newLabelID, Name = varLabel.Trim() });
                                                    LabelID = newLabelID;
                                                }
                                                if (newIpID > 0 || newSonIpID > 0 || newLabelID > 0)
                                                {
                                                    XYAuto.BUOC.IP2017.Dal.IPTitleInfo.IPTitleInfo.Instance.InsertIPTitleInfo(tbModel.TitleID, tbModelSonIp.TitleID, LabelID, importUserID);
                                                }
                                                sonIpLabel.Label.Add(new BasicLabelInfo() { DictName = varLabel.Trim() });
                                            }
                                        }
                                        IpInfo.SubIP.Add(sonIpLabel);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        auditLabel.IPLabel.Add(IpInfo);
                    }
                }
                #endregion
                Loger.Log4Net.Info(Name);
                RepeatMedia(auditLabel, resultID);
                if (auditLabel.MarketScene.Count == 0 && auditLabel.DistributeScene.Count == 0 && auditLabel.IPLabel.Count == 0)
                {
                    RepeatModel++;
                    Loger.Log4Net.Info($"[ImportWechatLabel]媒体[{Name}]完全重复");
                    continue;
                }
                XYAuto.BUOC.IP2017.Dal.Result.ResultLabel.Instance.InsertResultLabel(auditLabel, resultID, importUserID, false);
            }
            Loger.Log4Net.Info($"[ImportWechatLabel]插入媒体成功个数：{InsertSuccess}");
            Loger.Log4Net.Info($"[ImportWechatLabel]插入媒体失败个数{InsertError}");
            Loger.Log4Net.Info($"[ImportWechatLabel]插入模板个数:{InsertModel}");
            Loger.Log4Net.Info($"[ImportWechatLabel]插入模板重复个数:{RepeatModel}");
            Loger.Log4Net.Info("[ImportWechatLabel]程序开始运行结束...");
        }
        void RepeatMedia(AuditLabel auditLabel, int resultID)
        {

            DataSet ds = XYAuto.BUOC.IP2017.Dal.ExamineLabel.BasicLabel.Instance.QueryResultLabelList(resultID);

            List<IpLabelInfo> ipLabelList = Util.DataTableToList<IpLabelInfo>(ds.Tables[1]);
            if (!(auditLabel.MarketScene == null || auditLabel.DistributeScene == null))
            {
                List<BasicLabelInfo> labelList = Util.DataTableToList<BasicLabelInfo>(ds.Tables[0]);
                if (labelList != null)
                {
                    foreach (var item in labelList)
                    {
                        BasicLabelInfo infoMarket = auditLabel.MarketScene.Where(t => item.Type == (int)EnumLabelType.市场场景 && t.DictName.ToUpper() == item.DictName.ToUpper()).FirstOrDefault();
                        if (infoMarket != null)
                        {
                            auditLabel.MarketScene.Remove(infoMarket);
                        }
                        BasicLabelInfo infoDistribute = auditLabel.DistributeScene.Where(t => item.Type == (int)EnumLabelType.分发场景 && t.DictName.ToUpper() == item.DictName.ToUpper()).FirstOrDefault();
                        if (infoDistribute != null)
                        {
                            auditLabel.DistributeScene.Remove(infoDistribute);
                        }
                    }
                }
            }
            if (ipLabelList != null)
            {
                var varIpList = ipLabelList.GroupBy(t => new { t.PIPID, t.IpName });
                foreach (var item in varIpList)
                {
                    IpLabel infoIPLabel = auditLabel.IPLabel.Where(t => t.DictName.ToUpper() == item.Key.IpName.ToUpper()).FirstOrDefault();
                    if (infoIPLabel != null)
                    {
                        var varSonIpList = item.GroupBy(t => new { t.SubIPID, t.SonIpName });
                        foreach (var itemSonIp in varSonIpList)
                        {
                            SonIpLabel InfoSonIp = infoIPLabel.SubIP.Where(t => t.DictName.ToUpper() == itemSonIp.Key.SonIpName.ToUpper()).FirstOrDefault();
                            if (InfoSonIp != null)
                            {
                                foreach (var itemLabel in itemSonIp)
                                {
                                    BasicLabelInfo labelInfo = InfoSonIp.Label.Where(t => t.DictName.ToUpper() == itemLabel.LabelName.ToUpper()).FirstOrDefault();
                                    if (labelInfo != null)
                                    {
                                        InfoSonIp.Label.Remove(labelInfo);
                                    }
                                }
                                if (InfoSonIp.Label.Count <= 0)
                                {
                                    infoIPLabel.SubIP.Remove(InfoSonIp);
                                }
                                else
                                {
                                    InfoSonIp.SubIPID = itemSonIp.Key.SubIPID;
                                }
                            }
                        }
                        if (infoIPLabel.SubIP.Count <= 0)
                        {
                            auditLabel.IPLabel.Remove(infoIPLabel);
                        }
                        else
                        {
                            infoIPLabel.PIPID = item.Key.PIPID;
                        }
                    }
                }
            }
        }
    }
}
