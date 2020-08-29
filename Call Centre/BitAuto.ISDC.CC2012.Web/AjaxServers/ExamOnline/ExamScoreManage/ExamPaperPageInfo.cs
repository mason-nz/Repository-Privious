using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamScoreManage
{

    #region 页面传来的值

    public class ExamPaperPageInfo
    {
        public ExamPaperPage ExamPaper;
        public List<ExamBigQuestionPageinfo> ExamBigQuestioninfoList;
        public string DelBigQIDs;

        public void Validate(out Entities.ExamPaper examPaper, out List<Entities.ExamBigQuestion> bigQList, out List<Entities.ExamBSQuestionShip> shipList, out string delBigQIDs, out string msg)
        {
            examPaper = null;
            bigQList = null;
            shipList = null;
            delBigQIDs = DelBigQIDs;

            msg = "";

            #region 验证试卷信息

            if (ExamPaper != null)
            {
                examPaper = new Entities.ExamPaper();

                #region 验证

                int intVal = 0;

                if (ExamPaper.EPID != "" && !int.TryParse(ExamPaper.EPID, out intVal))
                {
                    msg += "试卷ID应该为数字！";
                }

                if (ExamPaper.Name == "")
                {
                    msg += "试卷名称不能为空！";
                }
                if (ExamPaper.ECID == "")
                {
                    msg += "试卷分类不能为空！";
                }
                if (!int.TryParse(ExamPaper.ECID, out intVal))
                {
                    msg += "试卷分类应该为数字！";
                }
                if (ExamPaper.ExamDesc == "")
                {
                    msg += "试卷说明不能为空！";
                }
                if (ExamPaper.TotalScore == "")
                {
                    msg += "总分不能为空！";
                }
                if (!int.TryParse(ExamPaper.TotalScore, out intVal))
                {
                    msg += "总分应该为数字！";
                }

                #endregion

                #region 赋值

                if (msg == "")
                {
                    if (ExamPaper.EPID != "" && int.TryParse(ExamPaper.EPID, out intVal))
                    {
                        examPaper.EPID = intVal;
                    }
                    examPaper.Name = ExamPaper.Name;
                    examPaper.ECID = int.Parse(ExamPaper.ECID);
                    examPaper.ExamDesc = ExamPaper.ExamDesc;
                    examPaper.TotalScore = int.Parse(ExamPaper.TotalScore);                    
                    examPaper.BGID = int.Parse(ExamPaper.BGID);
                }
                #endregion

            }

            #endregion

            #region 验证试卷大题信息

            if (ExamBigQuestioninfoList != null)
            {
                bigQList = new List<Entities.ExamBigQuestion>();

                #region 验证

                int intVal = 0;

                foreach (ExamBigQuestionPageinfo item in ExamBigQuestioninfoList)
                {
                    #region 验证大题数据

                    BigQPageInfo itemBigInfo = item.bigqpageinfo;
                    if (itemBigInfo != null)
                    {
                        if (itemBigInfo.BQID != "" && !int.TryParse(itemBigInfo.BQID, out intVal))
                        {
                            msg += "试卷大题ID应该为数字！";
                        }
                    }

                    if (itemBigInfo.EPID != "" && !int.TryParse(itemBigInfo.EPID, out intVal))
                    {
                        msg += "试卷大题的试卷ID应该为数字！";
                    }
                    if (itemBigInfo.Name == "")
                    {
                        msg += "试卷大题的名称不能为空！";
                    }
                    if (itemBigInfo.BQDesc == "")
                    {
                        msg += "试卷大题的描述不能为空！";
                    }
                    if (itemBigInfo.AskCategory == "")
                    {
                        msg += "试卷大题的题型不能为空！";
                    }
                    if (!int.TryParse(itemBigInfo.AskCategory, out intVal))
                    {
                        msg += "试卷大题的题型应该为数字！";
                    }
                    if (itemBigInfo.EachQuestionScore == "")
                    {
                        msg += "试卷大题的每题分值不能为空！";
                    }
                    if (!int.TryParse(itemBigInfo.EachQuestionScore, out intVal))
                    {
                        msg += "试卷大题的每题分值应该为数字！";
                    }
                    if (itemBigInfo.QuestionCount == "")
                    {
                        msg += "试卷大题的试题总量不能为空！";
                    }
                    if (!int.TryParse(itemBigInfo.QuestionCount, out intVal))
                    {
                        msg += "试卷大题的试题总量应该为数字！";
                    }

                    #endregion

                    #region 验证大小题关系数据

                    List<QuestinShipPageInfo> shiplist = item.shipList;
                    if (shiplist != null)
                    {
                        foreach (QuestinShipPageInfo shipItem in shiplist)
                        {
                            if (shipItem.BQID != "" && !int.TryParse(shipItem.BQID, out intVal))
                            {
                                msg += "大小题关系的大题ID应该为数字！";
                            }
                            if (shipItem.KLQID == "" && !int.TryParse(shipItem.KLQID, out intVal))
                            {
                                msg += "大小题关系的小题ID应该为数字！";
                            }
                        }
                    }

                    #endregion
                }

                #endregion

                #region 赋值

                if (msg == "")
                {
                    bigQList = new List<Entities.ExamBigQuestion>();
                    shipList = new List<Entities.ExamBSQuestionShip>();

                    Entities.ExamBigQuestion bigQModel;
                    Entities.ExamBSQuestionShip shipModel;
                    int rownum = 0;
                    foreach (ExamBigQuestionPageinfo item in ExamBigQuestioninfoList)
                    {
                        #region 大题赋值

                        BigQPageInfo itemBigInfo = item.bigqpageinfo;
                        bigQModel = new Entities.ExamBigQuestion();
                        if (itemBigInfo.BQID != "")
                        {
                            bigQModel.BQID = int.Parse(itemBigInfo.BQID);
                        }
                        bigQModel.Name = itemBigInfo.Name;
                        bigQModel.BQDesc = itemBigInfo.BQDesc;
                        bigQModel.AskCategory = int.Parse(itemBigInfo.AskCategory);
                        bigQModel.EachQuestionScore = int.Parse(itemBigInfo.EachQuestionScore);
                        bigQModel.QuestionCount = int.Parse(itemBigInfo.QuestionCount);
                        bigQModel.NO = rownum++;

                        bigQList.Add(bigQModel);

                        #endregion

                        #region 大小题关系赋值

                        List<QuestinShipPageInfo> shiplist = item.shipList;
                        if (shiplist != null)
                        {
                            foreach (QuestinShipPageInfo shipItem in shiplist)
                            {
                                shipModel = new Entities.ExamBSQuestionShip();

                                if (shipItem.BQID != "")
                                {
                                    shipModel.BQID = int.Parse(shipItem.BQID);
                                }
                                else if (itemBigInfo.BQID != "")
                                {
                                    shipModel.BQID = int.Parse(itemBigInfo.BQID);
                                }

                                if (shipItem.KLQID != "")
                                {
                                    shipModel.KLQID = int.Parse(shipItem.KLQID);
                                }
                                shipModel.NO = bigQModel.NO;
                                shipList.Add(shipModel);
                            }
                        }
                        #endregion
                    }
                }
                #endregion

            }


            #endregion

            #region 验证删除的大题IDs

            if (!String.IsNullOrEmpty(delBigQIDs))
            {
                long longval = 0;
                string[] idslist = delBigQIDs.Split(',');
                foreach (string item in idslist)
                {
                    if (!long.TryParse(item, out longval))
                    {
                        msg += "删除的大题ID格式不正确！";
                        break;
                    }
                }
            }

            #endregion
        }
    }


    /// <summary>
    /// 页面传来的试卷数据
    /// </summary>
    public class ExamPaperPage
    {
        public string _epid;
        public string EPID
        {
            get { return _epid; }
            set { _epid = HttpUtility.UrlDecode(value); }
        }

        public string _name;
        public string Name
        {
            get { return _name; }
            set { _name = HttpUtility.UrlDecode(value); }
        }

        public string _ecid;
        public string ECID
        {
            get { return _ecid; }
            set { _ecid = HttpUtility.UrlDecode(value); }
        }

        public string _examdesc;
        public string ExamDesc
        {
            get { return _examdesc; }
            set { _examdesc = HttpUtility.UrlDecode(value); }
        }

        public string _totalscore;
        public string TotalScore
        {
            get { return _totalscore; }
            set { _totalscore = HttpUtility.UrlDecode(value); }
        }

        private string _bgid;
        public string BGID
        {
            get { return _bgid; }
            set { _bgid = HttpUtility.UrlDecode(value); }
        }


    }

    /// <summary>
    /// 页面传来的大题数据(包括小题)
    /// </summary>
    public class ExamBigQuestionPageinfo
    {
        public BigQPageInfo bigqpageinfo;
        public List<QuestinShipPageInfo> shipList;
    }

    /// <summary>
    /// 大题数据
    /// </summary>
    public class BigQPageInfo
    {
        public string _bqid;
        public string BQID
        {
            get { return _bqid; }
            set { _bqid = HttpUtility.UrlDecode(value); }
        }

        public string _epid;
        public string EPID
        {
            get { return _epid; }
            set { _epid = HttpUtility.UrlDecode(value); }
        }

        public string _name;
        public string Name
        {
            get { return _name; }
            set { _name = HttpUtility.UrlDecode(value); }
        }

        public string _bqdesc;
        public string BQDesc
        {
            get { return _bqdesc; }
            set { _bqdesc = HttpUtility.UrlDecode(value); }
        }

        public string _askcategory;
        public string AskCategory
        {
            get { return _askcategory; }
            set { _askcategory = HttpUtility.UrlDecode(value); }
        }

        public string _eachquestionscore;
        public string EachQuestionScore
        {
            get { return _eachquestionscore; }
            set { _eachquestionscore = HttpUtility.UrlDecode(value); }
        }

        public string _questioncount;
        public string QuestionCount
        {
            get { return _questioncount; }
            set { _questioncount = HttpUtility.UrlDecode(value); }
        }

    }

    /// <summary>
    /// 大小题对应关系数据
    /// </summary>
    public class QuestinShipPageInfo
    {
        public string _bqid;
        public string BQID
        {
            get { return _bqid; }
            set { _bqid = HttpUtility.UrlDecode(value); }
        }

        public string _klqid;
        public string KLQID
        {
            get { return _klqid; }
            set { _klqid = HttpUtility.UrlDecode(value); }
        }
    }

    #endregion
}