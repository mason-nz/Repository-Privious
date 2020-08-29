using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data.SqlClient;
using BitAuto.Utils.Config;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TemplateManagement
{
    /// <summary>
    /// TemplateSave 的摘要说明
    /// </summary>
    public class TemplateSave : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 删除的字段IDs
        /// </summary>
        public string DelFiledIDs
        {
            get
            {
                return HttpContext.Current.Request["delFiledIds"] == null ? string.Empty :
                    HttpContext.Current.Request["delFiledIds"].ToString();
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

        int maxFieldID = -2;//最大字段ID

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = "";
            int userID = 0;
            string returnTTCode = "";

            try
            {
                if (BLL.Util.CheckButtonRight("SYS024MOD510201") && BLL.Util.CheckButtonRight("SYS024MOD510204"))
                {
                    if (msg == "")
                    {
                        userID = BitAuto.ISDC.CC2012.BLL.Util.GetLoginUserID();
                        Submit(out msg, userID, out returnTTCode);
                    }
                }
                else
                {
                    msg += "您没有添加和编辑模板的权限！";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }

            if (msg == "")
            {
                msg = "success_" + returnTTCode.ToString();
            }

            context.Response.Write(msg);
        }

        private void Submit(out string msg, int userID, out string returnTTCode)
        {
            msg = "";
            returnTTCode = "";

            string datainfoStr = DataStr;
            TemplateInfo tempInfoData = null;
            int templateStatus = -1;//模板状态

            Entities.TPage tpageModel = new Entities.TPage();
            Entities.TTable ttable = new Entities.TTable();
            List<Entities.TField> editfieldList = new List<Entities.TField>();
            List<Entities.TField> addfieldList = new List<Entities.TField>();
            Entities.TField fieldModel = new Entities.TField();

            List<Entities.TField> delFields = new List<Entities.TField>();

            tempInfoData = (TemplateInfo)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(datainfoStr, typeof(TemplateInfo));

            #region 验证数据

            CheckMsg(tempInfoData, out msg);

            if (msg != "")
            {
                return;
            }
            #endregion

            #region 准备数据

            if (tempInfoData.ttcode != "")
            {
                //编辑

                #region 模板信息

                tpageModel = BLL.TPage.Instance.GetTPageByTTCode(tempInfoData.ttcode);
                if (tpageModel != null)
                {
                    //状态
                    templateStatus = BLL.TPage.Instance.getStatus(tpageModel.RecID.ToString(), tpageModel.Status.ToString());

                    if (templateStatus != 0)
                    {
                        msg += "模板当前状态下不允许编辑";
                        return;
                    }

                    tpageModel.TPName = tempInfoData.templateName;
                    tpageModel.BGID = int.Parse(tempInfoData.BGID);
                    tpageModel.SCID = int.Parse(tempInfoData.CID);
                    tpageModel.TPContent = tempInfoData.templateDesc;
                    tpageModel.IsShowBtn = int.Parse(tempInfoData.IsShowBtn);
                    tpageModel.IsShowWorkOrderBtn = int.Parse(tempInfoData.IsShowWorkOrderBtn);
                    tpageModel.IsShowSendMsgBtn = int.Parse(tempInfoData.IsShowSendMsgBtn);
                    tpageModel.IsShowQiCheTong = int.Parse(tempInfoData.IsShowQiCheTong);
                    tpageModel.IsShowSubmitOrder = int.Parse(tempInfoData.IsShowSubmitOrder);
                }
                else
                {
                    msg += "没有找到相关模板信息";
                    return;
                }

                #endregion

                #region 自定义数据表

                ttable = BLL.TTable.Instance.GetTTableByTTCode(tempInfoData.ttcode);
                if (ttable != null)
                {
                    ttable.TTDesName = tempInfoData.templateName;
                }
                else
                {
                    msg += "没有找到相关的自定义表信息";
                    return;
                }

                #endregion

                #region 编辑字段
                editfieldList = BLL.TField.Instance.GetTFieldListByTTCode(tempInfoData.ttcode);
                List<FieldInfoData> updateFieldInfo = new List<FieldInfoData>();
                #region 新增的
                for (int i = tempInfoData.fieldListInfo.Length - 1; i >= 0; i--)
                {
                    if (tempInfoData.fieldListInfo[i].RecID == "")
                    {
                        //新增
                        fieldModel = GetOneFieldMode(userID, tempInfoData.fieldListInfo[i], tpageModel.TTCode, "");
                        addfieldList.Add(fieldModel);
                    }
                    else
                    {
                        //编辑的
                        updateFieldInfo.Add(tempInfoData.fieldListInfo[i]);
                    }
                }
                #endregion

                int existflog = 0;
                for (int i = editfieldList.Count - 1; i >= 0; i--)
                {
                    existflog = 0;
                    if (tempInfoData.fieldListInfo != null)
                    {
                        foreach (FieldInfoData field in updateFieldInfo)
                        {
                            if (field.RecID == editfieldList[i].RecID.ToString())
                            {
                                //编辑
                                editfieldList[i].TFDesName = field.TFDesName;
                                editfieldList[i].TFDes = field.TFDes;
                                editfieldList[i].TFInportIsNull = int.Parse(field.TFInportIsNull);
                                editfieldList[i].TFIsNull = int.Parse(field.TFIsNull);
                                editfieldList[i].TFSortIndex = int.Parse(field.TFSortIndex);
                                editfieldList[i].TFIsExportShow = int.Parse(field.TFIsExportShow);
                                editfieldList[i].TFIsListShow = int.Parse(field.TFIsListShow);
                                editfieldList[i].TFValue = field.TFValue;
                                editfieldList[i].TFCssName = field.TFCssName;
                                existflog = 1;
                            }

                        }

                        // //在页面传来的对象中没有，是删除
                        if (existflog == 0)
                        {
                            //删除的
                            delFields.Add(editfieldList[i]);
                            editfieldList.RemoveAt(i);
                        }
                    }
                }
                #endregion

            }
            else
            {
                //新增
                #region 自定义数据表

                int maxID = BLL.TTable.Instance.GetMaxID();

                ttable.TTCode = "t" + maxID;
                ttable.TTDesName = tempInfoData.templateName;
                ttable.TTName = "Tempt" + maxID;
                ttable.Status = 0;
                ttable.CreateTime = DateTime.Now;
                ttable.CreateUserID = userID;

                #endregion

                #region 模板表
                tpageModel.TPName = tempInfoData.templateName;
                tpageModel.BGID = int.Parse(tempInfoData.BGID);
                tpageModel.SCID = int.Parse(tempInfoData.CID);
                tpageModel.TPRef = "";
                tpageModel.TTCode = ttable.TTCode;
                tpageModel.TPContent = tempInfoData.templateDesc;
                tpageModel.GenTempletPath = "";
                tpageModel.Status = 0;
                tpageModel.Remark = "";
                tpageModel.CreateTime = DateTime.Now;
                tpageModel.CreateUserID = userID;
                tpageModel.TTName = ttable.TTName;
                tpageModel.IsShowBtn = int.Parse(tempInfoData.IsShowBtn);
                tpageModel.IsShowWorkOrderBtn = int.Parse(tempInfoData.IsShowWorkOrderBtn);
                tpageModel.IsShowSendMsgBtn = int.Parse(tempInfoData.IsShowSendMsgBtn);
                tpageModel.IsShowQiCheTong = int.Parse(tempInfoData.IsShowQiCheTong);
                tpageModel.IsShowSubmitOrder = int.Parse(tempInfoData.IsShowSubmitOrder);
                tpageModel.IsUsed = -1;
                #endregion

                #region 字段

                if (tempInfoData.fieldListInfo != null)
                {
                    foreach (FieldInfoData item in tempInfoData.fieldListInfo)
                    {
                        string ttCode = ttable.TTCode;

                        fieldModel = GetOneFieldMode(userID, item, ttCode, "");

                        addfieldList.Add(fieldModel);
                    }
                }

                #endregion
            }

            #endregion

            #region 提交事务

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");

            try
            {

                if (tempInfoData.ttcode == "")
                {
                    //新增

                    BLL.TPage.Instance.Insert(tran, tpageModel);

                    BLL.TTable.Instance.Insert(tran, ttable);

                    //新增的字段
                    foreach (Entities.TField item in addfieldList)
                    {
                        BLL.TField.Instance.Insert(tran, item);
                    }

                    returnTTCode = tpageModel.TTCode;
                }
                else
                {
                    //编辑

                    BLL.TPage.Instance.Update(tran, tpageModel);

                    BLL.TTable.Instance.Update(tran, ttable);

                    //编辑的字段
                    foreach (Entities.TField item in editfieldList)
                    {
                        BLL.TField.Instance.Update(tran, item);
                    }

                    //新增的字段
                    foreach (Entities.TField item in addfieldList)
                    {
                        BLL.TField.Instance.Insert(tran, item);

                        #region 如果已经生成模板，添加物理表的字段（修改为：模板生成了，还可以修改，⊙﹏⊙b汗）

                        if (templateStatus == 1)
                        {
                            try
                            {
                                List<string> addNames = GetRealFieldName(item);

                                foreach (string name in addNames)
                                {
                                    BLL.TField.Instance.AddColumn(ttable.TTName, name, item.TTypeID.ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                msg += "添加数据表字段失败！<br>";
                            }
                        }

                        #endregion
                    }

                    //删除的字段

                    foreach (Entities.TField item in delFields)
                    {
                        BLL.TField.Instance.Delete(item.RecID);

                        #region 如果已经生成模板，删除物理表的字段

                        if (templateStatus == 1)
                        {
                            try
                            {
                                List<string> delNames = GetRealFieldName(item);

                                foreach (string name in delNames)
                                {
                                    BLL.TField.Instance.DelColumn(ttable.TTName, name);
                                }
                            }
                            catch (Exception ex)
                            {
                                msg += "删除数据表字段失败！<br>";
                            }
                        }

                        #endregion
                    }

                    returnTTCode = tpageModel.TTCode;
                }


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

        private static List<string> GetRealFieldName(Entities.TField item)
        {
            List<string> delNames = new List<string>();

            #region 得到物理字段名字段

            if (item.TFShowCode == "100009")
            {
                //日期段
                delNames.Add(item.TFName + "_startdata");
                delNames.Add(item.TFName + "_enddata");
            }
            else if (item.TFShowCode == "100011")
            {
                //时间段
                delNames.Add(item.TFName + "_starttime");
                delNames.Add(item.TFName + "_endtime");
            }
            else if (item.TFShowCode == "100012")
            {
                //二级省市
                delNames.Add(item.TFName + "_Province");
                delNames.Add(item.TFName + "_City");

                delNames.Add(item.TFName + "_Province_name");
                delNames.Add(item.TFName + "_City_name");

            }
            else if (item.TFShowCode == "100013")
            {
                //三级省市县
                delNames.Add(item.TFName + "_Province");
                delNames.Add(item.TFName + "_City");
                delNames.Add(item.TFName + "_Country");

                delNames.Add(item.TFName + "_Province_name");
                delNames.Add(item.TFName + "_City_name");
                delNames.Add(item.TFName + "_Country_name");

            }
            else if (item.TFShowCode == "100004")//复选
            {
                delNames.Add(item.TFName + "_checkid");
                delNames.Add(item.TFName + "_checkid_name");
            }
            else if (item.TFShowCode == "100003")//单选
            {
                delNames.Add(item.TFName + "_radioid");
                delNames.Add(item.TFName + "_radioid_name");
            }
            else if (item.TFShowCode == "100005")//下拉
            {
                delNames.Add(item.TFName + "_selectid");
                delNames.Add(item.TFName + "_selectid_name");
            }
            else if (item.TFShowCode == "100014")//CRM CustID
            {
                delNames.Add(item.TFName + "_crmcustid_name");
            }
            else
            {
                delNames.Add(item.TFName);
            }

            #endregion
            return delNames;
        }

        private Entities.TField GetOneFieldMode(int userID, FieldInfoData item, string ttCode, string TFCode)
        {
            Entities.TField fieldModel = new Entities.TField();

            if (TFCode == "")
            {
                if (maxFieldID == -2)
                {
                    maxFieldID = BLL.TField.Instance.GetMaxID();
                }
                else
                {
                    maxFieldID++;
                }
                TFCode = "f" + maxFieldID;
            }
            fieldModel.TFCode = TFCode;
            fieldModel.TFDesName = item.TFDesName;

            if (item.TFShowCode == "100020")
            {
                //js中已经赋值
                fieldModel.TFName = item.TFName;
            }
            else
            {
                fieldModel.TFName = "Tempf" + maxFieldID;
            }

            fieldModel.TTCode = ttCode;

            #region 字段类型和长度

            switch (item.TFShowCode)
            {
                case "100001":
                case "100004"://复选
                case "100006":
                case "100007":
                    fieldModel.TTypeID = 1;
                    fieldModel.TFLen = 200;
                    break;
                case "100002":
                    fieldModel.TTypeID = 1;
                    fieldModel.TFLen = 2000;
                    break;
                case "100003"://单选
                    fieldModel.TTypeID = 3;
                    fieldModel.TFLen = 200;
                    break;
                case "100005"://下拉
                    fieldModel.TTypeID = 3;
                    fieldModel.TFLen = 200;
                    break;
                case "100008":
                case "100009":
                case "100010":
                case "100011":
                    fieldModel.TTypeID = 4;
                    fieldModel.TFLen = 200;
                    break;
                case "100012":
                case "100013":
                    fieldModel.TTypeID = 3;
                    fieldModel.TFLen = 200;
                    break;
                case "100014"://CRM CustID
                    fieldModel.TTypeID = 1;
                    fieldModel.TFLen = 200;
                    break;
                case "100019"://推荐活动
                    fieldModel.TTypeID = 1;
                    fieldModel.TFLen = 4000;
                    break;
                default:
                    fieldModel.TTypeID = 1;
                    fieldModel.TFLen = 200;
                    break;
            }

            #endregion

            fieldModel.TFDes = item.TFDes;
            fieldModel.TFInportIsNull = int.Parse(item.TFInportIsNull);
            fieldModel.TFIsNull = int.Parse(item.TFIsNull);
            fieldModel.TFSortIndex = int.Parse(item.TFSortIndex);
            fieldModel.TFCssName = item.TFCssName;
            fieldModel.TFIsExportShow = 1;
            fieldModel.TFIsListShow = 1;
            fieldModel.TFShowCode = item.TFShowCode;
            fieldModel.TFValue = item.TFValue;
            fieldModel.TFGenSubField = "";
            fieldModel.Status = 0;
            fieldModel.CreateTime = DateTime.Now;
            fieldModel.CreateUserID = userID;

            return fieldModel;
        }

        private void CheckMsg(TemplateInfo tempInfoData, out string msg)
        {
            msg = "";

            int intVal = 0;

            if (tempInfoData.templateName == "")
            {
                msg += "模板名称不能为空<br>";
            }
            if (tempInfoData.BGID == "" || tempInfoData.BGID == "-1")
            {
                msg += "模板所属分组不能为空<br>";
            }
            if (!int.TryParse(tempInfoData.BGID, out intVal))
            {
                msg += "模板所属业务分组ID格式不正确";
            }
            if (tempInfoData.CID == "" || tempInfoData.CID == "-1")
            {
                msg += "模板分类不能为空<br>";
            }
            if (!int.TryParse(tempInfoData.BGID, out intVal))
            {
                msg += "模板分类ID格式不正确";
            }

            if (tempInfoData.fieldListInfo != null)
            {
                for (int i = 0; i <= tempInfoData.fieldListInfo.Length - 1; i++)
                {
                    #region 验证字段

                    if (tempInfoData.fieldListInfo[i].TFInportIsNull == "")
                    {
                        msg += "字段是否导入必填不能为空<br>";
                    }
                    if (!int.TryParse(tempInfoData.fieldListInfo[i].TFInportIsNull, out intVal) || (intVal != 0 && intVal != 1))
                    {
                        msg += "字段是否导入必填格式不正确";
                    }

                    if (tempInfoData.fieldListInfo[i].TFIsNull == "")
                    {
                        msg += "字段是否必填不能为空<br>";
                    }
                    if (!int.TryParse(tempInfoData.fieldListInfo[i].TFIsNull, out intVal) || (intVal != 0 && intVal != 1))
                    {
                        msg += "字段是否必填格式不正确";
                    }
                    if (tempInfoData.fieldListInfo[i].TFSortIndex == "")
                    {
                        msg += "字段排序索引不能为空<br>";
                    }
                    if (!int.TryParse(tempInfoData.fieldListInfo[i].TFSortIndex, out intVal) || (intVal <= 0))
                    {
                        msg += "字段排序索引格式不正确<br>";
                    }


                    //添加系统内置字段名称限制,与系统字段重复，不能添加
                    if ((tempInfoData.fieldListInfo[i].TFDesName == "操作时间")
                        || (tempInfoData.fieldListInfo[i].TFDesName == "操作人")
                        || (tempInfoData.fieldListInfo[i].TFDesName == "推荐活动" && tempInfoData.fieldListInfo[i].TFShowCode != "100019")
                        || (tempInfoData.fieldListInfo[i].TFDesName == "是否成功" && tempInfoData.fieldListInfo[i].TFShowCode != "100020")
                        || (tempInfoData.fieldListInfo[i].TFDesName == "是否接通" && tempInfoData.fieldListInfo[i].TFShowCode != "100020")
                        || (tempInfoData.fieldListInfo[i].TFDesName == "失败原因" && tempInfoData.fieldListInfo[i].TFShowCode != "100020")
                        || (tempInfoData.fieldListInfo[i].TFDesName == "未接通原因" && tempInfoData.fieldListInfo[i].TFShowCode != "100020")
                        || tempInfoData.fieldListInfo[i].TFDesName == "任务ID"
                        || (tempInfoData.fieldListInfo[i].TFDesName == "客户ID" && tempInfoData.fieldListInfo[i].TFShowCode != "100014")
                        || tempInfoData.fieldListInfo[i].TFDesName == "任务状态" 
                        )
                    { 
                        msg += "【" + tempInfoData.fieldListInfo[i].TFDesName + "】字段名与系统字段重复，不能添加！<br>";
                    }
                     

                    #endregion

                    #region 判断字段名不能重复

                    for (int j = i + 1; j <= tempInfoData.fieldListInfo.Length - 1; j++)
                    {
                        if (tempInfoData.fieldListInfo[j].TFDesName == tempInfoData.fieldListInfo[i].TFDesName)
                        {
                            msg += "【" + tempInfoData.fieldListInfo[j].TFDesName + "】字段名已经重复！<br>";
                            break;
                        }
                    }

                    #endregion
                }
            }

            #region 判断模板名不能重复

            if (tempInfoData.ttcode == "")
            {
                Entities.QueryTPage query = new Entities.QueryTPage();
                query.TPName = tempInfoData.templateName;
                int totalCount = 0;
                DataTable dt = BLL.TPage.Instance.GetTPage(query, "", 1, 999, out totalCount);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["TPName"].ToString().Trim() == tempInfoData.templateName)
                        {
                            msg += "已经存在名为[" + tempInfoData.templateName + "]的模板";
                            break;
                        }
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