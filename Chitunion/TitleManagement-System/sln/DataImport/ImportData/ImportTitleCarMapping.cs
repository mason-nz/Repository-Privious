using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.Entities;
using XYAuto.BUOC.IP2017.BLL;
using XYAuto.BUOC.IP2017.Entities.DTO;
using static XYAuto.BUOC.IP2017.Entities.ENUM.ENUM;
using XYAuto.BUOC.IP2017.Entities.TitleBasicInfo;
using XYAuto.BUOC.IP2017;
using XYAuto.BUOC.IP2017.BLL.ResultLabel;
using XYAuto.BUOC.IP2017.Entities.Examine;
using XYAuto.BUOC.IP2017.Entities.MediaBase;
using XYAuto.BUOC.IP2017.Entities.Result;

namespace DataImport.ImportData
{
    public class ImportTitleCarMapping : Import
    {
        public readonly static ImportTitleCarMapping Instance = new ImportTitleCarMapping();

        private readonly int importUserID = int.Parse(ConfigurationManager.AppSettings["CarMapIP_UserID"]);
        public void ImportCarLabel(int IpCount, string excelPath)
        {
            Loger.Log4Net.Info("[ImportTitleCarMapping]程序开始运行开始...");
            Loger.Log4Net.Info("[ImportTitleCarMapping]Excel位置:" + excelPath);
            Loger.Log4Net.Info("[ImportTitleCarMapping]统一导入的UserID：" + importUserID);
            if (!File.Exists(excelPath))
            {
                Loger.Log4Net.Error($"[ImportTitleCarMapping]导入文件[{excelPath}]不存在");
                return;
            }
            DataSet ds = ConvertExcelToDataSet(new string[] { "车型标签" }, excelPath);
            if (ds == null && ds.Tables.Count == 0)
            {
                Loger.Log4Net.Error($"[ImportTitleCarMapping]导入文件[{excelPath}][车型标签]sheet页不存在");
                return;
            }

            DataView dataView = ds.Tables["车型标签"].DefaultView;
            DataTable dt = dataView.ToTable();
            if (dt.Rows.Count == 0)
            {
                Loger.Log4Net.Error($"[ImportTitleCarMapping]导入文件[{excelPath}][车型标签]没有数据");
                return;
            }
            List<TitleBasicInfo> listALL = XYAuto.BUOC.IP2017.BLL.TitleBasicInfo.TitleBasicInfo.Instance.SelectAll();
            List<TitleBasicInfo> listCategory = listALL.Where(t => t.Type == (int)EnumLabelType.分类).ToList();
            List<TitleBasicInfo> listMark = listALL.Where(t => t.Type == (int)EnumLabelType.市场场景).ToList();
            List<TitleBasicInfo> listXy = listALL.Where(t => t.Type == (int)EnumLabelType.分发场景).ToList();
            List<TitleBasicInfo> listIP = listALL.Where(t => t.Type == (int)EnumLabelType.IP).ToList();
            List<TitleBasicInfo> listSonIP = listALL.Where(t => t.Type == (int)EnumLabelType.子IP).ToList();
            List<TitleBasicInfo> listLabel = listALL.Where(t => t.Type == (int)EnumLabelType.标签).ToList();
            var listCarSerial = XYAuto.BUOC.IP2017.BLL.CarSerialInfo.CarSerial.Instance.GetCarSerialList();
            int irow = 1;
            foreach (DataRow row in dt.Rows)
            {
                Loger.Log4Net.Info($"[ImportTitleCarMapping]便历行：{++irow}");
                int serialID = -2;
                int itmp = -2;
                if (!int.TryParse(row["车型ID"].ToString().Trim(), out itmp))
                {
                    serialID = XYAuto.BUOC.IP2017.Dal.Result.ResultLabel.Instance.SelectSerialId(row["车型"].ToString().Trim());
                    if (serialID <= 0)
                    {
                        Loger.Log4Net.Error($"[ImportTitleCarMapping]模板文件车型名称[{row["车型"].ToString().Trim()}]未找到");
                        continue;
                    }

                }
                else
                    serialID = itmp;
                RespCarSerialDto carSerial = listCarSerial.Find(x => x.CarSerialId == serialID);
                if (carSerial == null)
                {
                    Loger.Log4Net.Error($"[ImportTitleCarMapping]模板文件车型ID值[{serialID}]在车型表不存在");
                    continue;
                }
                int resultID = XYAuto.BUOC.IP2017.Dal.Result.ResultLabel.Instance.GetResultID((int)EnumTaskType.车型, serialID, "", 0);
                if (resultID <= 0)
                {
                    resultID = ResultLabel.Instance.InsertResult((int)EnumTaskType.车型, carSerial.BrandId, serialID, -2, "", "", "", "", importUserID);
                }
                if (resultID <= 0)
                {
                    Loger.Log4Net.Error($"[ImportTitleCarMapping]模板文件车型ID值[{serialID}]品牌ID值[{carSerial.BrandId}]插入失败");
                    continue;
                }
                XYAuto.BUOC.IP2017.Dal.Result.ResultLabel.Instance.DeleteResultLabel(resultID);
                #region IP1
                List<IpLabel> IPLabel = new List<IpLabel>();
                for (int i = 1; i <= IpCount; i++)
                {
                    string ip = row["IP" + i.ToString()].ToString().Trim();
                    string subip1 = row["子IP" + i.ToString()].ToString().Trim();
                    string label = row["标签"].ToString().Trim();
                    TitleBasicInfo tbModel = new TitleBasicInfo();
                    if (!string.IsNullOrWhiteSpace(ip))
                    {
                        tbModel = listIP.Where(x => x.Name == ip).FirstOrDefault();
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
                                Loger.Log4Net.Info($"[ImportTitleCarMapping]母IP：[{ip}]插入成功");
                            }
                            else
                            {
                                Loger.Log4Net.Error($"[ImportTitleCarMapping]母IP：[{ip}]插入失败");
                                continue;
                            }
                        }
                        IpLabel IpInfo = new IpLabel();
                        IpInfo.DictId = tbModel.TitleID;
                        IpInfo.DictName = ip;
                        IpInfo.SubIP = new List<SonIpLabel>();
                        if (!string.IsNullOrWhiteSpace(subip1))
                        {
                            foreach (var subIp in subip1.Split(new string[] { "、" }, StringSplitOptions.None))
                            {
                                if (!string.IsNullOrWhiteSpace(subIp))
                                {
                                    TitleBasicInfo tbModelSonIp = tbModelSonIp = listSonIP.Where(x => x.Name == subIp.Trim()).FirstOrDefault();
                                    int newSonIpID = 0;
                                    if (tbModelSonIp == null)
                                    {
                                        newSonIpID = XYAuto.BUOC.IP2017.Dal.IPTitleInfo.IPTitleInfo.Instance.InsertIpInfo(subIp.Trim(), (int)EnumLabelType.子IP, importUserID);
                                        if (newSonIpID > 0)
                                        {
                                            tbModelSonIp = new TitleBasicInfo();
                                            tbModelSonIp.TitleID = newSonIpID;
                                            tbModelSonIp.Name = subIp.Trim();
                                            listSonIP.Add(tbModelSonIp);
                                            Loger.Log4Net.Info($"[ImportTitleCarMapping]子IP：[{ subIp.Trim()}]插入成功");
                                        }
                                        else
                                        {
                                            Loger.Log4Net.Error($"[ImportTitleCarMapping]子IP：[{ subIp.Trim()}]插入失败");
                                            continue;
                                        }
                                    }
                                    SonIpLabel sonIpInfo = new SonIpLabel();
                                    sonIpInfo.DictId = tbModelSonIp.TitleID;
                                    sonIpInfo.DictName = subIp.Trim();
                                    int newLabelID = 0;
                                    if (string.IsNullOrWhiteSpace(label))
                                    {
                                        int LabelID = listLabel.Where(t => t.Name.ToUpper() == label.Trim().ToUpper()).Select(t => t.TitleID).FirstOrDefault();
                                        if (LabelID <= 0)
                                        {
                                            newLabelID = XYAuto.BUOC.IP2017.Dal.IPTitleInfo.IPTitleInfo.Instance.InsertIpInfo(label.Trim(), (int)EnumLabelType.标签, importUserID);
                                            if (newLabelID <= 0)
                                            {
                                                Loger.Log4Net.Error($"[ImportTitleCarMapping]标签：[{label.Trim()}]插入失败");
                                                continue;
                                            }
                                            listLabel.Add(new TitleBasicInfo() { TitleID = newLabelID, Name = label.Trim() });
                                            LabelID = newLabelID;
                                        }
                                        if (newIpID > 0 || newSonIpID > 0 || newLabelID > 0)
                                        {
                                            XYAuto.BUOC.IP2017.Dal.IPTitleInfo.IPTitleInfo.Instance.InsertIPTitleInfo(tbModel.TitleID, tbModelSonIp.TitleID, LabelID, importUserID);
                                        }
                                    }
                                    sonIpInfo.Label = new List<BasicLabelInfo>();
                                    sonIpInfo.Label.Add(new BasicLabelInfo { DictName = label.Trim() });
                                    IpInfo.SubIP.Add(sonIpInfo);
                                }
                            }
                            IPLabel.Add(IpInfo);
                        }
                    }
                }
                #endregion
                string strResult = InsertResult(IPLabel, resultID);
                if (strResult == "无可用标签")
                {
                    Loger.Log4Net.Error($"车型：{serialID}" + strResult);
                    ReqResultIdDto dto = new ReqResultIdDto() { MediaResultID = resultID };
                    XYAuto.BUOC.IP2017.BLL.ResultLabel.ResultLabel.Instance.DeleteMediaLabel(dto);
                }
                else
                {
                    Loger.Log4Net.Info($"车型：{serialID}" + strResult);
                }
            }
            Loger.Log4Net.Info("[ImportTitleCarMapping]程序开始运行结束...");
        }
        private string InsertResult(List<IpLabel> IPLabel, int resultID)
        {
            #region IP标签去重
            if (IPLabel.Count > 0)
            {
                DataSet dsDistict = XYAuto.BUOC.IP2017.Dal.ExamineLabel.BasicLabel.Instance.QueryResultLabelList(resultID);

                List<IpLabelInfo> ipLabelList = Util.DataTableToList<IpLabelInfo>(dsDistict.Tables[1]);
                if (ipLabelList != null)
                {
                    var varIpList = ipLabelList.GroupBy(t => new { t.PIPID, t.IpName });
                    foreach (var item in varIpList)
                    {
                        IpLabel infoIPLabel = IPLabel.Where(t => t.DictName.ToUpper() == item.Key.IpName.ToUpper()).FirstOrDefault();
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
                                IPLabel.Remove(infoIPLabel);
                            }
                            else
                            {
                                infoIPLabel.PIPID = item.Key.PIPID;
                            }
                        }
                    }
                }
                if (IPLabel.Count > 0)
                {
                    XYAuto.BUOC.IP2017.Dal.Result.ResultLabel.Instance.InserResultLableIpInfo(IPLabel, resultID, importUserID);
                    return "添加标签成功";
                }
                else
                {
                    return "标签完全重复";
                }


            }
            return "无可用标签";
            #endregion
        }



    }
}
