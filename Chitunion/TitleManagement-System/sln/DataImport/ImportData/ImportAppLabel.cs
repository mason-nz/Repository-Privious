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
using XYAuto.BUOC.IP2017.Entities.TitleBasicInfo;
using static XYAuto.BUOC.IP2017.Entities.ENUM.ENUM;

namespace DataImport.ImportData
{
    public class ImportAppLabel : Import
    {
        public readonly static ImportAppLabel Instance = new ImportAppLabel();

        private readonly int importUserID = int.Parse(ConfigurationManager.AppSettings["CarMapIP_UserID"]);
        public void ImportAppLabell(int CategoryCount, int MarkCount, int XyCount, int IpCount, int SonIpCount, string excelPath)
        {
            Loger.Log4Net.Info("[ImportAppLabel]程序开始运行开始...");
            Loger.Log4Net.Info("[ImportAppLabel]Excel位置:" + excelPath);
            Loger.Log4Net.Info("[ImportAppLabel]统一导入的UserID：" + importUserID);
            if (!File.Exists(excelPath))
            {
                Loger.Log4Net.Error($"[ImportAppLabel]导入文件[{excelPath}]不存在");
                return;
            }
            DataSet ds = ConvertExcelToDataSet(new string[] { "app资源及标签" }, excelPath);
            if (ds == null && ds.Tables.Count == 0)
            {
                Loger.Log4Net.Error($"[ImportAppLabel]导入文件[{excelPath}][app资源及标签]sheet页不存在");
                return;
            }

            DataView dataView = ds.Tables["app资源及标签"].DefaultView;
            DataTable dt = dataView.ToTable();
            if (dt.Rows.Count == 0)
            {
                Loger.Log4Net.Error($"[ImportAppLabel]导入文件[{excelPath}][app资源及标签]没有数据");
                return;
            }
            List<TitleBasicInfo> listAll = XYAuto.BUOC.IP2017.BLL.TitleBasicInfo.TitleBasicInfo.Instance.SelectAll();
            List<TitleBasicInfo> listCategory = listAll.Where(t => t.Type == (int)EnumLabelType.分类).ToList();
            List<TitleBasicInfo> listMark = listAll.Where(t => t.Type == (int)EnumLabelType.市场场景).ToList();
            List<TitleBasicInfo> listXy = listAll.Where(t => t.Type == (int)EnumLabelType.分发场景).ToList();
            List<TitleBasicInfo> listIP = listAll.Where(t => t.Type == (int)EnumLabelType.IP).ToList();
            List<TitleBasicInfo> listSonIP = listAll.Where(t => t.Type == (int)EnumLabelType.子IP).ToList();
            List<TitleBasicInfo> listLabel = listAll.Where(t => t.Type == (int)EnumLabelType.标签).ToList();
            List<string> listMediaApp = new List<string>();
            DataTable dtMediaApp = XYAuto.BUOC.IP2017.Dal.MediaBase.MediaBase.Instance.SelectMediaAPP();
            if (listMediaApp != null)
            {
                for (int i = 0; i < dtMediaApp.Rows.Count; i++)
                {
                    listMediaApp.Add(dtMediaApp.Rows[i]["Name"].ToString());
                }
            }
            int InsertError = 0;
            int InsertSuccess = 0;
            int InsertModel = 0;
            int RepeatModel = 0;
            int irow = 1;
            foreach (DataRow row in dt.Rows)
            {
                Loger.Log4Net.Info($"[ImportAppLabel]便历行：{++irow}");

                string Name = "";
                string HeadUrl = "";
                if (string.IsNullOrWhiteSpace(row["名称"].ToString()))
                {
                    continue;
                }
                else
                {
                    Name = row["名称"].ToString().Trim();
                    if (!listMediaApp.Contains(Name))
                    {
                        MediaApp mediaApp = new MediaApp();
                        mediaApp.Name = Name;
                        //mediaApp.Remark = row["媒体介绍"].ToString().Trim();
                        //string DailyLive = row["日活(万)"].ToString().Trim();
                        //if (!string.IsNullOrWhiteSpace(DailyLive))
                        //{

                        //    if (DailyLive.Contains("亿"))
                        //    {
                        //        mediaApp.DailyLive = Convert.ToDecimal(DailyLive.Replace("亿", "")) * 100000000;
                        //    }
                        //    else
                        //    {
                        //        mediaApp.DailyLive = Convert.ToDecimal(DailyLive) * 10000;
                        //    }
                        //}

                        int ResultMedia = XYAuto.BUOC.IP2017.Dal.MediaBase.MediaBase.Instance.InsertMediaApp(mediaApp, importUserID);
                        if (ResultMedia <= 0)
                        {
                            InsertError++;
                            Loger.Log4Net.Error($"[ImportAppLabel]媒体信息媒体名称：{Name}插入失败");
                            continue;
                        }
                        InsertSuccess++;
                    }
                }
                int resultID = XYAuto.BUOC.IP2017.Dal.Result.ResultLabel.Instance.GetResultID((int)EnumTaskType.媒体, -2, Name, (int)EnumMediaType.APP);
                if (resultID <= 0)
                {
                    resultID = ResultLabel.Instance.InsertResult((int)EnumTaskType.媒体, -2, -2, (int)EnumMediaType.APP, Name, Name, HeadUrl, "", importUserID);
                }
                if (resultID <= 0)
                {
                    Loger.Log4Net.Error($"[ImportAppLabel]模板文件媒体名称：{Name}插入失败");
                    continue;
                }
                InsertModel++;
                AuditLabel auditLabel = new AuditLabel();
                auditLabel.TaskType = (int)EnumTaskType.媒体;
                auditLabel.Category = new List<BasicLabelInfo>();
                auditLabel.DistributeScene = new List<BasicLabelInfo>();
                auditLabel.MarketScene = new List<BasicLabelInfo>();
                #region 分类
                for (int i = 1; i <= CategoryCount; i++)
                {
                    string dicName = row["分类" + i].ToString().Trim();
                    if (!string.IsNullOrEmpty(dicName))
                    {
                        TitleBasicInfo tbModel = listCategory.Where(x => x.Name == dicName).FirstOrDefault();
                        if (tbModel != null)
                        {
                            auditLabel.Category.Add(new BasicLabelInfo { DictId = tbModel.TitleID, DictName = dicName });
                        }
                        else
                            Loger.Log4Net.Error($"[ImportAppLabel]媒体[{Name}]的分类[{dicName}]在标签库未找到");
                    }
                }
                auditLabel.Category = auditLabel.Category.Where((x, k) => auditLabel.Category.FindIndex(z => z.DictName.ToUpper() == x.DictName.ToUpper()) == k).ToList();
                if (auditLabel.Category.Count <= 0)
                {
                    Loger.Log4Net.Error($"[ImportAppLabel]媒体[{Name}]未插入分类");
                }
                #endregion
                #region 市场场景
                for (int i = 1; i <= MarkCount; i++)
                {
                    string dicName = row["市场场景" + i].ToString().Trim();
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
                    Loger.Log4Net.Error($"[ImportAppLabel]媒体[{Name}]未插入市场场景");
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
                            Loger.Log4Net.Error($"[ImportAppLabel]媒体[{Name}]的分发场景[{dicName}]在标签库未找到");
                    }
                }
                auditLabel.DistributeScene = auditLabel.DistributeScene.Where((x, k) => auditLabel.DistributeScene.FindIndex(z => z.DictName.ToUpper() == x.DictName.ToUpper()) == k).ToList();
                if (auditLabel.DistributeScene.Count <= 0)
                {
                    Loger.Log4Net.Error($"[ImportAppLabel]媒体[{Name}]未插入分发场景");
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
                                Loger.Log4Net.Info($"[ImportAppLabel]母IP：[{ip}]插入成功");
                            }
                            else
                            {
                                Loger.Log4Net.Error($"[ImportAppLabel]母IP：[{ip}]插入失败");
                                continue;
                            }
                        }

                        IpLabel IpInfo = new IpLabel();
                        IpInfo.DictId = tbModel.TitleID;
                        IpInfo.DictName = ip;
                        IpInfo.SubIP = new List<SonIpLabel>();
                        for (int j = 1; j <= SonIpCount; j++)
                        {
                            string columnName = i + "子IP" + j;
                            if (row.Table.Columns.Contains(columnName))
                            {
                                string subip = row[columnName].ToString().Trim();
                                if (!string.IsNullOrWhiteSpace(subip))
                                {
                                    string label = row[i + "标签" + j].ToString().Trim();
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
                                            Loger.Log4Net.Info($"[ImportAppLabel]子IP：[{subip}]插入成功");
                                        }
                                        else
                                        {
                                            Loger.Log4Net.Error($"[ImportAppLabel]子IP：[{subip}]插入失败");
                                            continue;
                                        }
                                    }
                                    if (!string.IsNullOrWhiteSpace(label))
                                    {
                                        SonIpLabel sonIpLabel = new SonIpLabel();
                                        sonIpLabel.DictId = tbModelSonIp.TitleID;
                                        sonIpLabel.DictName = subip;
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
                                                        Loger.Log4Net.Error($"[ImportAppLabel]标签：[{varLabel.Trim()}]插入失败");
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
                                    else
                                    {
                                        Loger.Log4Net.Error($"[ImportAppLabel]媒体[{Name}]下子IP[{ tbModelSonIp.Name}]无标签");
                                        continue;
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        if (IpInfo.SubIP.Count > 0)
                        {
                            auditLabel.IPLabel.Add(IpInfo);
                        }
                        else
                        {
                            Loger.Log4Net.Error($"[ImportAppLabel]媒体[{Name}]下IP标签[{IpInfo.DictName}]无子IP");
                        }
                    }
                }
                if (auditLabel.IPLabel.Count <= 0)
                {
                    Loger.Log4Net.Error($"[ImportAppLabel]媒体[{Name}]未插入IP");
                }
                #endregion
                Loger.Log4Net.Info(Name);
                RepeatMedia(auditLabel, resultID);
                if (auditLabel.Category.Count == 0 && auditLabel.MarketScene.Count == 0 && auditLabel.DistributeScene.Count == 0 && auditLabel.IPLabel.Count == 0)
                {
                    RepeatModel++;
                    Loger.Log4Net.Info($"[ImportAppLabel]媒体[{Name}]完全重复");
                    continue;
                }
                XYAuto.BUOC.IP2017.Dal.Result.ResultLabel.Instance.InsertResultLabel(auditLabel, resultID, importUserID, false);
            }

            Loger.Log4Net.Info($"[ImportAppLabel]插入媒体成功个数：{InsertSuccess}");
            Loger.Log4Net.Info($"[ImportAppLabel]插入媒体失败个数{InsertError}");
            Loger.Log4Net.Info($"[ImportAppLabel]插入模板个数:{InsertModel}");
            Loger.Log4Net.Info($"[ImportAppLabel]插入模板重复个数:{RepeatModel}");
            Loger.Log4Net.Info("[ImportAppLabel]程序开始运行结束...");
        }
        void RepeatMedia(AuditLabel auditLabel, int resultID)
        {

            DataSet ds = XYAuto.BUOC.IP2017.Dal.ExamineLabel.BasicLabel.Instance.QueryResultLabelList(resultID);

            List<IpLabelInfo> ipLabelList = Util.DataTableToList<IpLabelInfo>(ds.Tables[1]);
            if (!(auditLabel.Category == null || auditLabel.MarketScene == null || auditLabel.DistributeScene == null))
            {
                List<BasicLabelInfo> labelList = Util.DataTableToList<BasicLabelInfo>(ds.Tables[0]);
                if (labelList != null)
                {
                    foreach (var item in labelList)
                    {
                        BasicLabelInfo infoCategory = auditLabel.Category.Where(t => item.Type == (int)EnumLabelType.分类 && t.DictName.ToUpper() == item.DictName.ToUpper()).FirstOrDefault();
                        if (infoCategory != null)
                        {
                            auditLabel.Category.Remove(infoCategory);
                        }
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
