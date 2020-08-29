using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Web.QualityStandard.ScoreTableManage;
using System.Data.SqlClient;
using BitAuto.Utils.Config;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ScoreTableManage
{
    /// <summary>
    /// ScoreTableSave 的摘要说明
    /// </summary>
    public class ScoreTableSave : IHttpHandler, IRequiresSessionState
    {
        public string ActionType
        {
            get
            {

                return HttpContext.Current.Request["ActionType"] == null ? string.Empty :
                   HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ActionType"].ToString());
            }
        }

        public string DataStr
        {
            get
            {

                return HttpContext.Current.Request["DataStr"] == null ? string.Empty :
                   HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["DataStr"].ToString());
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";

            string msg = "";
            int userID = 0;
            string returnRTID = "";

            try
            {
                if (BLL.Util.CheckButtonRight("SYS024BUT600201"))
                {
                    if (msg == "")
                    {
                        userID = BitAuto.ISDC.CC2012.BLL.Util.GetLoginUserID();
                        Submit(out msg, userID, out returnRTID);
                    }
                }
                else
                {
                    msg += "您没有添加和编辑评分表的权限！";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString().Replace("\r", "").Replace("\n","");
            }

            string retJson = "";
            string successFlog = "";
            if (msg == "")
            {
                successFlog = "success";
            }
            retJson += "{\"result\":\"" + successFlog + "\",";
            retJson += "\"QS_RTID\":\"" + returnRTID + "\",";
            retJson += "\"msg\":\"" + msg + "\"";
            retJson += "}";

            context.Response.Write(retJson);
        }



        private void Submit(out string msg, int userID, out string returnRTID)
        {
            msg = "";
            returnRTID = "";

            #region 定义变量

            string datainfoStr = DataStr;
            ScoreTableInfo tempInfoData;

            Entities.QS_RulesTable QsTable;
            Entities.QS_Category QsCategory;
            Entities.QS_Item QsItem;
            Entities.QS_Standard QsStandard;
            Entities.QS_Marking QsMarking;
            Entities.QS_DeadOrAppraisal Qsdead;


            List<Entities.QS_Category> QsCategoryList = new List<Entities.QS_Category>();
            List<Entities.QS_Item> QsItemList = new List<Entities.QS_Item>();
            List<Entities.QS_Standard> QsStandardList = new List<Entities.QS_Standard>();
            List<Entities.QS_Marking> QsMarkingList = new List<Entities.QS_Marking>();
            List<Entities.QS_DeadOrAppraisal> QsDeadList = new List<Entities.QS_DeadOrAppraisal>();

            #endregion

            tempInfoData = (ScoreTableInfo)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(datainfoStr, typeof(ScoreTableInfo));

            #region 验证数据
            //判断是否是提交，提交才验证，add by qizq
            if (ActionType == "Submit")
            {
                CheckMsg(tempInfoData, out msg);

                if (msg != "")
                {
                    return;
                }
            }
            //
            #endregion

            #region 准备数据

            #region 评分表

            QsTable = new Entities.QS_RulesTable();
            QsTable.QS_RTID = int.Parse(tempInfoData.RTID);
            QsTable.Name = tempInfoData.Name;
            QsTable.ScoreType = int.Parse(tempInfoData.ScoreType);
            QsTable.Description = tempInfoData.Description;

            QsTable.DeadItemNum = int.Parse(tempInfoData.DeadItemNum);
            QsTable.NoDeadItemNum = int.Parse(tempInfoData.NoDeadItemNum);
            QsTable.HaveQAppraisal = int.Parse(tempInfoData.Appraisal);
            QsTable.LastModifyTime = DateTime.Now;
            QsTable.LastModifyUserID = userID.ToString();
            QsTable.RegionID = tempInfoData.RegionID;

            if (QsTable.QS_RTID < 0)
            {
                if (BLL.QS_RulesTable.Instance.IsRuleTableNameExist(QsTable.Name, QsTable.ScoreType))
                {
                    msg = "评分表名称[" + QsTable.Name + "]已经存在，不能重复添加！";
                    return;
                }
            }

            //add by qizq 添加日志2013-5-8
            StringBuilder sbStr = new StringBuilder();

            if (ActionType == "Submit")
            {
                sbStr.Append("提交评分表 " + QsTable.Name);
                QsTable.Status = (int)Entities.QSRulesTableStatus.Audit;
            }
            else
            {
                sbStr.Append("保存评分表 " + QsTable.Name);
                QsTable.Status = (int)Entities.QSRulesTableStatus.Unfinished;
            }

            #endregion

            #region 评分分类

            if (tempInfoData.Catage != null)
            {
                foreach (Catage catageInfo in tempInfoData.Catage)
                {
                    QsCategory = new Entities.QS_Category();
                    QsCategory.QS_CID = int.Parse(catageInfo.CID);
                    QsCategory.QS_RTID = int.Parse(tempInfoData.RTID);//评分表的值
                    QsCategory.Name = catageInfo.Name;
                    QsCategory.Score = int.Parse(catageInfo.Score);
                    QsCategory.Status = int.Parse(catageInfo.Status);
                    QsCategory.ScoreType = int.Parse(tempInfoData.ScoreType); //评分表的值
                    QsCategory.LastModifyTime = DateTime.Now;
                    QsCategory.LastModifyUserID = userID;

                    QsCategoryList.Add(QsCategory);

                    #region 质检项目

                    if (catageInfo.Item != null)
                    {
                        foreach (Item itemInfo in catageInfo.Item)
                        {
                            QsItem = new Entities.QS_Item();
                            QsItem.QS_IID = int.Parse(itemInfo.IID);
                            QsItem.QS_CID = int.Parse(catageInfo.CID);
                            QsItem.QS_RTID = int.Parse(tempInfoData.RTID);
                            QsItem.ItemName = itemInfo.ItemName;
                            QsItem.ScoreType = int.Parse(tempInfoData.ScoreType);
                            QsItem.Score = decimal.Parse(itemInfo.Score);
                            QsItem.Status = int.Parse(itemInfo.Status);
                            QsItem.LastModifyTime = DateTime.Now;
                            QsItem.LastModifyUserID = userID;

                            QsItemList.Add(QsItem);

                            #region 质检标准

                            if (itemInfo.Standard != null)
                            {           
                                foreach (Standard standardInfo in itemInfo.Standard)
                                {                                    
                                    if (standardInfo.Score.Trim() != "" && standardInfo.SName.Trim() != "") //&& standardInfo.Status.Trim() == "0"
                                    {                                       
                                        QsStandard = new Entities.QS_Standard();
                                        QsStandard.QS_SID = int.Parse(standardInfo.SID);
                                        QsStandard.QS_IID = int.Parse(itemInfo.IID);
                                        QsStandard.QS_CID = int.Parse(catageInfo.CID);
                                        QsStandard.QS_RTID = int.Parse(tempInfoData.RTID);
                                        QsStandard.ScoringStandardName = standardInfo.SName;
                                        QsStandard.ScoreType = int.Parse(tempInfoData.ScoreType);
                                        QsStandard.Score = decimal.Parse(standardInfo.Score);
                                        QsStandard.IsIsDead = string.IsNullOrEmpty(standardInfo.IsIsDead) ? -2 : int.Parse(standardInfo.IsIsDead);
                                        QsStandard.Status = int.Parse(standardInfo.Status);
                                        QsStandard.LastModifyTime = DateTime.Now;
                                        QsStandard.LastModifyUserID = userID;

                                        QsStandard.QS_SExplanation = standardInfo.SExplanation;
                                        QsStandard.QS_SkillLevel = standardInfo.SkillLevel == null ? Constant.INT_INVALID_VALUE: int.Parse(standardInfo.SkillLevel);

                                        QsStandardList.Add(QsStandard);
                                    }

                                    #region 扣分项

                                    if (standardInfo.Marking != null && tempInfoData.ScoreType.Trim() != "3")
                                    {
                                        foreach (Marking markInfo in standardInfo.Marking)
                                        {
                                            QsMarking = new Entities.QS_Marking();

                                            QsMarking.QS_MID = int.Parse(markInfo.MID);
                                            QsMarking.QS_RTID = int.Parse(tempInfoData.RTID);
                                            QsMarking.QS_CID = int.Parse(catageInfo.CID);
                                            QsMarking.QS_IID = int.Parse(itemInfo.IID);
                                            QsMarking.QS_SID = int.Parse(standardInfo.SID);
                                            QsMarking.ScoreType = int.Parse(tempInfoData.ScoreType);
                                            QsMarking.MarkingItemName = markInfo.MName;
                                            QsMarking.Score = int.Parse(markInfo.Score);
                                            QsMarking.Status = int.Parse(markInfo.Status);

                                            QsMarkingList.Add(QsMarking);
                                        }
                                    }

                                    #endregion

                                }                                 
                            }


                            #endregion

                        }
                    }

                    #endregion
                }
            }

            #endregion

            #region 致命项

            if (tempInfoData.Dead != null)
            {
                foreach (Dead deadInfo in tempInfoData.Dead)
                {
                    Qsdead = new Entities.QS_DeadOrAppraisal();
                    Qsdead.QS_DAID = int.Parse(deadInfo.DID);
                    Qsdead.QS_RTID = int.Parse(tempInfoData.RTID);
                    Qsdead.DeadItemName = deadInfo.DName;
                    Qsdead.Status = int.Parse(deadInfo.Status);
                    Qsdead.LastModifyTime = DateTime.Now;
                    Qsdead.LastModifyUserID = userID;

                    QsDeadList.Add(Qsdead);
                }
            }

            #endregion

            #endregion

            #region 提交到数据库

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
            //  SqlTransaction tran = connection.BeginTransaction("SampleTransaction");

            try
            {
                #region 保存评分表

                if (QsTable.QS_RTID < 0)
                {
                    //新增

                    QsTable.CreateTime = DateTime.Now;
                    QsTable.CreateUserID = userID;
                    int retID = BLL.QS_RulesTable.Instance.Insert(tran, QsTable);
                    returnRTID = retID.ToString();

                    #region 修改各个表的评分表ID

                    foreach (Entities.QS_Category cItem in QsCategoryList)
                    {
                        if (cItem.QS_RTID == QsTable.QS_RTID)
                        {
                            cItem.QS_RTID = int.Parse(returnRTID);
                        }
                    }

                    foreach (Entities.QS_Item iItem in QsItemList)
                    {
                        if (iItem.QS_RTID == QsTable.QS_RTID)
                        {
                            iItem.QS_RTID = int.Parse(returnRTID);
                        }
                    }

                    foreach (Entities.QS_Standard sItem in QsStandardList)
                    {
                        if (sItem.QS_RTID == QsTable.QS_RTID)
                        {
                            sItem.QS_RTID = int.Parse(returnRTID);
                        }
                    }

                    foreach (Entities.QS_Marking mItem in QsMarkingList)
                    {
                        if (mItem.QS_RTID == QsTable.QS_RTID)
                        {
                            mItem.QS_RTID = int.Parse(returnRTID);
                        }
                    }

                    foreach (Entities.QS_DeadOrAppraisal dItem in QsDeadList)
                    {
                        if (dItem.QS_RTID == QsTable.QS_RTID)
                        {
                            dItem.QS_RTID = int.Parse(returnRTID);
                        }
                    }

                    #endregion

                }
                else
                {
                    //编辑
                    BLL.QS_RulesTable.Instance.Update(tran, QsTable);
                    returnRTID = QsTable.QS_RTID.ToString();
                }

                #endregion

                #region 保存分类

                foreach (Entities.QS_Category cItem in QsCategoryList)
                {
                    if (cItem.Status == -1)
                    {
                        //删除的
                        BLL.QS_Category.Instance.Delete(tran, cItem.QS_CID);
                        continue;
                    }

                    if (cItem.QS_CID < 0)
                    {
                        //新增
                        cItem.CreateTime = DateTime.Now;
                        cItem.CreateUserID = userID;
                        int retCID = BLL.QS_Category.Instance.Insert(tran, cItem);

                        #region 修改各个表的分类ID

                        foreach (Entities.QS_Item iItem in QsItemList)
                        {
                            if (iItem.QS_CID == cItem.QS_CID)
                            {
                                iItem.QS_CID = retCID;
                            }
                        }

                        foreach (Entities.QS_Standard sItem in QsStandardList)
                        {
                            if (sItem.QS_CID == cItem.QS_CID)
                            {
                                sItem.QS_CID = retCID;
                            }
                        }

                        foreach (Entities.QS_Marking mItem in QsMarkingList)
                        {
                            if (mItem.QS_CID == cItem.QS_CID)
                            {
                                mItem.QS_CID = retCID;
                            }
                        }

                        #endregion

                    }
                    else
                    {
                        //编辑
                        BLL.QS_Category.Instance.Update(tran, cItem);
                    }
                }

                #endregion

                #region 保存项目

                foreach (Entities.QS_Item iItem in QsItemList)
                {
                    if (iItem.Status == -1)
                    {
                        //删除的
                        BLL.QS_Item.Instance.Delete(tran, iItem.QS_IID);
                        continue;
                    }

                    if (iItem.QS_IID < 0)
                    {
                        //新增
                        iItem.CreateTime = DateTime.Now;
                        iItem.CreateUserID = userID;
                        int retIID = BLL.QS_Item.Instance.Insert(tran, iItem);

                        #region 修改各个表的项目ID

                        foreach (Entities.QS_Standard sItem in QsStandardList)
                        {
                            if (sItem.QS_IID == iItem.QS_IID)
                            {
                                sItem.QS_IID = retIID;
                            }
                        }

                        foreach (Entities.QS_Marking mItem in QsMarkingList)
                        {
                            if (mItem.QS_IID == iItem.QS_IID)
                            {
                                mItem.QS_IID = retIID;
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        //编辑
                        BLL.QS_Item.Instance.Update(tran, iItem);
                    }
                }

                #endregion

                #region 保存质检标准

                foreach (Entities.QS_Standard sItem in QsStandardList)
                {
                    if (sItem.Status == -1)
                    {
                        //删除的
                        BLL.QS_Standard.Instance.Delete(tran, sItem.QS_SID);
                        continue;
                    }

                    if (sItem.QS_SID < 0 || (sItem.ScoreType == 3 && sItem.QS_SID<=5))
                    {
                        //新增
                        sItem.CreateTime = DateTime.Now;
                        sItem.CreateUserID = userID;
                        int retSID = BLL.QS_Standard.Instance.Insert(tran, sItem);

                        #region 修改各个表的项目ID
                        if (sItem.ScoreType != 3)
                        {
                            foreach (Entities.QS_Marking mItem in QsMarkingList)
                            {
                                if (mItem.QS_SID == sItem.QS_SID)
                                {
                                    mItem.QS_SID = retSID;
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        //编辑
                        BLL.QS_Standard.Instance.Update(tran, sItem);
                    }
                }

                #endregion

                #region 保存评分扣分项

                foreach (Entities.QS_Marking mItem in QsMarkingList)
                {
                    if (mItem.Status == -1)
                    {
                        //删除的
                        BLL.QS_Marking.Instance.Delete(tran, mItem.QS_MID);
                        continue;
                    }

                    if (mItem.QS_MID < 0)
                    {
                        //新增
                        mItem.CreateTime = DateTime.Now;
                        mItem.CreateUserID = userID;
                        BLL.QS_Marking.Instance.Insert(tran, mItem);
                    }
                    else
                    {
                        //编辑
                        BLL.QS_Marking.Instance.Update(tran, mItem);
                    }
                }

                #endregion

                #region 保存致命项

                foreach (Entities.QS_DeadOrAppraisal dItem in QsDeadList)
                {
                    if (dItem.Status == -1)
                    {
                        //删除的
                        BLL.QS_DeadOrAppraisal.Instance.Delete(tran, dItem.QS_DAID);
                        continue;
                    }

                    if (dItem.QS_DAID < 0)
                    {
                        //新增
                        dItem.CreateTime = DateTime.Now;
                        dItem.CreateUserID = userID;
                        BLL.QS_DeadOrAppraisal.Instance.Insert(tran, dItem);
                    }
                    else
                    {
                        //编辑
                        BLL.QS_DeadOrAppraisal.Instance.Update(tran, dItem);
                    }
                }

                #endregion

                //add by qizq 保存日志 2013-5-8
                BLL.Util.InsertUserLog(tran, sbStr.ToString());

                tran.Commit();
            }
            catch (Exception ex)
            {
                if (tran.Connection != null)
                {
                    tran.Rollback();
                }
                msg = ex.Message.ToString();
            }
            finally
            {
                connection.Close();
            }


            #endregion

        }

        private void CheckMsg(ScoreTableInfo tempInfoData, out string msg)
        {
            msg = "";

            int intVal = 0;
            decimal decimalVal = 0;

            if (ActionType != "PreView" && ActionType != "Submit" && ActionType != "Save")
            {
                msg += "参数不正确<br/>";
            }

            if (tempInfoData.Name == "")
            {
                msg += "评分表名称不能为空<br/>";
            }
            //if (tempInfoData.RegionID != "1" && tempInfoData.RegionID != "2" && tempInfoData.RegionID != "1,2")
            //{
            //    msg += "请选择评分表适用区域<br/>";
            //}
            if (tempInfoData.Description == "")
            {
                msg += "评分表说明不能为空<br/>";
            }
            if (tempInfoData.ScoreType != "1" && tempInfoData.ScoreType != "2" && tempInfoData.ScoreType != "3") 
            {
                msg += "评分表类型不正确<br/>";
            }

            if (tempInfoData.ScoreType == "2")
            {
                if (!int.TryParse(tempInfoData.DeadItemNum, out intVal))
                {
                    msg += "评分表致命项个数应该为数字<br/>";
                }
                if (!int.TryParse(tempInfoData.NoDeadItemNum, out intVal))
                {
                    msg += "评分表非致命项个数应该为数字<br/>";
                }
                if (int.Parse(tempInfoData.DeadItemNum) == 0 && int.Parse(tempInfoData.NoDeadItemNum) == 0)
                {
                    msg += "评分表致命项个数和非致命项个数不能同时为0<br/>";
                }
            }

            #region 验证

            if (tempInfoData.Catage != null)
            {
                foreach (Catage cInfo in tempInfoData.Catage)
                {

                    #region 验证分类

                    if (cInfo.Name == "")
                    {
                        msg += "分类名称不能为空<br/>";
                    }
                    if (tempInfoData.ScoreType == "1" || tempInfoData.ScoreType == "3")
                    {
                        if (cInfo.Score == "")
                        {
                            msg += "请输入分类的分数值<br/>";
                        }
                        else
                        {
                            if (!int.TryParse(cInfo.Score, out intVal))
                            {
                                msg += "分类的分数值应该是数字";
                            }
                        }
                    }
                    
                    #endregion

                    #region 验证项目

                    if (cInfo.Item != null && cInfo.Item.Length > 0)
                    {
                        decimal sumItemScore = 0;
                        foreach (Item iinfo in cInfo.Item)
                        {
                            #region 项目

                            if (iinfo.ItemName == "")
                            {
                                msg += "[" + cInfo.Name + "]分类下的质检项目名称不能为空<br/>";
                            }

                            if (tempInfoData.ScoreType == "1")
                            {
                                if (iinfo.Score == "")
                                {
                                    msg += "请输入[" + cInfo.Name + "]分类下的质检项目的分数值<br/>";
                                }
                                else
                                {
                                    if (!decimal.TryParse(iinfo.Score, out decimalVal))
                                    {
                                        msg += "[" + cInfo.Name + "]分类下的质检项目的分数值应该是数字";
                                    }
                                    else
                                    {
                                        sumItemScore += decimal.Parse(iinfo.Score);
                                    }
                                }
                            }
                            else if (tempInfoData.ScoreType == "3")
                            {
                                if (iinfo.Score == "")
                                {
                                    msg += "请输入[" + cInfo.Name + "]分类下的质检项目的分数值<br/>";
                                }
                                else
                                {
                                    if (!decimal.TryParse(iinfo.Score, out decimalVal))
                                    {
                                        msg += "[" + cInfo.Name + "]分类下的质检项目的分数值应该是数字";
                                    }
                                    else
                                    {
                                        sumItemScore += decimal.Parse(iinfo.Score);
                                    }
                                }
                            }
                            #endregion

                            #region 验证标准

                            decimal sumStandScore = 0;
                            decimal maxStandardScore = 0;
                            if (iinfo.Standard != null && iinfo.Standard.Length > 0)
                            {
                                foreach (Standard sInfo in iinfo.Standard)
                                {
                                    if (tempInfoData.ScoreType == "3" && sInfo.Status != "0")
                                    {
                                        continue;
                                    }
                                  
                                    #region 标准

                                    if (sInfo.SName == "")
                                    {
                                        msg += "[" + cInfo.Name + "]分类下的[" + iinfo.ItemName + "]质检项目下质检标准名称不能为空<br/>";
                                    }

                                    if (tempInfoData.ScoreType == "1")
                                    {
                                        if (sInfo.Score == "")
                                        {
                                            msg += "请输入[" + cInfo.Name + "]分类下的[" + iinfo.ItemName + "]质检项目下质检标准的分数值<br/>";
                                        }
                                        else
                                        {
                                            if (!decimal.TryParse(sInfo.Score, out decimalVal))
                                            {
                                                msg += "[" + cInfo.Name + "]分类下的[" + iinfo.ItemName + "]质检项目下质检标准分数值应该是数字<br/>";
                                            }
                                            else
                                            {
                                                sumStandScore += decimal.Parse(sInfo.Score);
                                            }
                                        }
                                    }
                                    else if (tempInfoData.ScoreType == "3")
                                    {
                                        if (sInfo.Score == "")
                                        {
                                            msg += "请输入[" + cInfo.Name + "]分类下的[" + iinfo.ItemName + "]质检项目下质检标准的分数值<br/>";
                                        }
                                        else
                                        {
                                            if (!decimal.TryParse(sInfo.Score, out decimalVal))
                                            {
                                                msg += "[" + cInfo.Name + "]分类下的[" + iinfo.ItemName + "]质检项目下质检标准分数值应该是数字<br/>";
                                            }
                                            else
                                            {
                                                sumStandScore += decimal.Parse(sInfo.Score);
                                                maxStandardScore = maxStandardScore > decimal.Parse(sInfo.Score) ? maxStandardScore : decimal.Parse(sInfo.Score);
                                            }
                                        }
                                    }
                                    #endregion

                                    #region 验证扣分项

                                    if (sInfo.Marking != null && tempInfoData.ScoreType != "3")
                                    {
                                        foreach (Marking mInfo in sInfo.Marking)
                                        {
                                            if (tempInfoData.ScoreType == "1")
                                            {
                                                if (mInfo.Score == "")
                                                {
                                                    msg += "请输入[" + cInfo.Name + "]--[" + iinfo.ItemName + "]--[" + sInfo.SName + "]的分数值<br/>";
                                                }
                                                else
                                                {
                                                    if (!int.TryParse(mInfo.Score, out intVal))
                                                    {
                                                        msg += "[" + cInfo.Name + "]--[" + iinfo.ItemName + "]--[" + sInfo.SName + "]质检标准的分数值应该是数字<br/>";
                                                    }
                                                    else
                                                    {
                                                        if (int.Parse(mInfo.Score) >= 0)
                                                        {
                                                            msg += "[" + cInfo.Name + "]--[" + iinfo.ItemName + "]--[" + sInfo.SName + "]下扣分项应该为负数<br/>";
                                                        }
                                                        else
                                                        {
                                                            if (int.TryParse(sInfo.Score, out intVal))
                                                            {
                                                                if (int.Parse(mInfo.Score) * -1 > int.Parse(sInfo.Score))
                                                                {
                                                                    msg += "[" + cInfo.Name + "]--[" + iinfo.ItemName + "]--[" + sInfo.SName + "]下扣分项的分数不能超过扣分标准的分数值<br/>";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //只有一个扣分项的评分型
                                        if (tempInfoData.ScoreType == "1" && sInfo.Marking.Length == 1)
                                        {
                                            if (int.TryParse(sInfo.Score, out intVal) && int.TryParse(sInfo.Marking[0].Score, out intVal))
                                            {
                                                if (int.Parse(sInfo.Marking[0].Score) < 0)
                                                {
                                                    if (int.Parse(sInfo.Marking[0].Score) * -1 != int.Parse(sInfo.Score))
                                                    {
                                                        msg += "[" + cInfo.Name + "]--[" + iinfo.ItemName + "]--[" + sInfo.SName + "]下扣分项的分数应该等于扣分标准的分数值<br/>";
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    #endregion
                                }
                                if (decimal.TryParse(iinfo.Score, out decimalVal))
                                {
                                    if (tempInfoData.ScoreType == "3" && maxStandardScore != decimalVal)
                                    {
                                        msg += "【" + cInfo.Name + "】分类下的[" + iinfo.ItemName + "]质检项目下质检标准分数的最大值应该等于当前项目的分数<br/>";
                                    }
                                    else if (tempInfoData.ScoreType != "3" && sumStandScore != decimal.Parse(iinfo.Score))
                                    {
                                        msg += "【" + cInfo.Name + "】分类下的[" + iinfo.ItemName + "]质检项目下质检标准分数的和应该等于当前项目的分数<br/>";
                                    }
                                }


                            }
                            //add by qizq添加验证，项目下不能没有质检标准
                            else if (iinfo.Standard == null || iinfo.Standard.Length == 0)
                            {
                                msg += "【" + cInfo.Name + "】分类下的[" + iinfo.ItemName + "]质检项目下不能没有质检标准<br/>";
                            }

                            #endregion
                        }
                        if (decimal.TryParse(cInfo.Score, out decimalVal))
                        {
                            if (sumItemScore != decimal.Parse(cInfo.Score))
                            {
                                msg += "【" + cInfo.Name + "】分类下项目分数的和应该等于当前分类的分数<br/>";
                            }
                        }
                    }
                    //add by qizq添加验证，分类下不能没有项目
                    else if (cInfo.Item == null || cInfo.Item.Length == 0)
                    {
                        msg += "【" + cInfo.Name + "】分类下不能没有项目<br/>";
                    }


                    #endregion
                }
            }

            #endregion

            #region 验证致命项

            if (tempInfoData.Dead != null )
            {
                foreach (Dead dInfo in tempInfoData.Dead)
                {
                    if (dInfo.DName == "")
                    {
                        msg += "致命项内容不能为空<br/>";
                    }
                }
            }

            #endregion

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}