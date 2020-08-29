using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.UserControl
{
    public partial class WOrderBasicInfo : System.Web.UI.UserControl
    {
        /// 传值：客户分类ID
        /// <summary>
        /// 传值：客户分类ID
        /// </summary>
        public CustTypeEnum CustCategoryID { get; set; }
        /// 传值：工单实体
        /// <summary>
        /// 传值：工单实体
        /// </summary>
        public Entities.WOrderInfoInfo WOrderInfo { get; set; }

        public string WorkOrderID { get { if (WOrderInfo != null) return WOrderInfo.OrderID_Value; else return ""; } }

        private bool canseetelimg = false;
        public bool CanSeeTelImg
        {
            get { return canseetelimg; }
            set { canseetelimg = value; }
        }

        public string StrHtml = "";
        StringBuilder buiderHtml = new StringBuilder("");
        public int TdCount = 1;
        private int UserID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(WorkOrderID))
                {
                    ConcateHtml("工单类型", BLL.Util.GetEnumOptText(typeof(Entities.WOrderCategoryEnum), (int)WOrderInfo.CategoryID_Value));
                    ConcateHtml("工单状态", BLL.Util.GetEnumOptText(typeof(Entities.WorkOrderStatus), (int)WOrderInfo.WorkOrderStatus_Value));
                    ConcateHtml("工单来源", BLL.Util.GetEnumOptText(typeof(Entities.WorkOrderDataSource), (int)WOrderInfo.DataSource_Value));

                    SetBusinessTypeAndLabel();
                    //工单类型为“回访”，并且客户分类为“经销商”时才显示
                    SetJXSInfo();
                    //工单类型为“投诉”时才显示
                    SetTSInfo();

                    ToEndTr();

                    if (WOrderInfo.ContactName_Value != "" || WOrderInfo.ContactTel_Value != "")
                    {
                        ConcateHtml("联系人姓名", WOrderInfo.ContactName_Value);
                        ConcateHtml("联系人电话", AddTelImgToTels(WOrderInfo.ContactTel_Value, WOrderInfo.ContactName_Value));
                    }

                    ToEndTr();

                    ConcateHtml("提交人姓名", BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(WOrderInfo.CreateUserID_Value));
                    ConcateHtml("提交时间", WOrderInfo.CreateTime_Value.ToString("yyyy-MM-dd HH:mm:ss"));

                    ToEndTr();

                    //设置关联数据呈现
                    SetWOrderDataInfo();

                    //查询工单的附件
                    SetAttachment();



                    if (buiderHtml.ToString().StartsWith("</tr>"))
                    {
                        //去掉开始的</tr>
                        StrHtml = buiderHtml.ToString().Substring(5) + "</tr>";
                    }
                }
                else
                {
                    StrHtml = "<tr><th colspan=\"4\" style='text-align: center;'>没有找到对应的工单信息</th></tr>";
                }
            }
        }

        private void SetBusinessTypeAndLabel()
        {
            //可空
            if (WOrderInfo.BusinessType_Value > 0)
            {
                //业务类型
                DataTable businessdt = BLL.WOrderBusiType.Instance.GetAllData("1"); //在用
                if (businessdt != null && businessdt.Rows.Count > 0)
                {
                    DataRow[] rows = businessdt.Select("RecID=" + WOrderInfo.BusinessType_Value);
                    if (rows.Length > 0 && rows[0]["BusiTypeName"] != null)
                    {
                        ConcateHtml("业务类型", rows[0]["BusiTypeName"].ToString());
                    }
                }

            }
            //可空
            if (WOrderInfo.BusinessTag_Value > 0)
            {
                string labelName = BLL.WOrderTag.Instance.GetTagNamePathByRecId(WOrderInfo.BusinessTag_Value, "/");
                if (!string.IsNullOrEmpty(labelName))
                {
                    ConcateHtml("标签", labelName);
                }
            }
        }
        /// 给电话号码增加外呼图标
        /// </summary>
        /// <param name="tels"></param>
        /// <returns></returns>
        private string AddTelImgToTels(string tels, string attrcbnameVal)
        {
            string backVal = "";
            if (canseetelimg && !string.IsNullOrEmpty(tels))
            {
                tels = tels.Trim().Replace("，", ",");
                string[] telArr = tels.Split(',');
                foreach (string tel in telArr)
                {
                    backVal += tel + "<img style='margin:0px 15px 0px 5px;cursor:pointer;' name='TelImg' src='/images/phone.gif' alt='" + tel + "' tel='" + tel + "' cbname='" + attrcbnameVal + "' />";
                }
            }
            else
            {
                backVal = tels;
            }
            return backVal;
        }

        /// 设置回访经销商
        /// <summary>
        /// 设置回访经销商
        /// </summary>
        /// <param name="model"></param>
        private void SetJXSInfo()
        {
            if (WOrderInfo.CategoryID_Value == (int)BitAuto.ISDC.CC2012.Entities.WOrderCategoryEnum.W04_回访 && CustCategoryID == BitAuto.ISDC.CC2012.Entities.CustTypeEnum.T02_经销商)
            {
                //访问分类 可空
                DataTable visitdt = BitAuto.YanFa.Crm2009.BLL.DictInfo.Instance.GetDictInfoByDictType(101);
                if (visitdt != null)
                {
                    DataRow[] rows = visitdt.Select("DictID=" + WOrderInfo.VisitType_Value);
                    if (rows.Length > 0)
                    {
                        ConcateHtml("访问分类", rows[0]["DictName"].ToString());
                    }
                }
                //话务结果
                CallResult_ORIG_TaskInfo mod = BLL.CallResult_ORIG_Task.Instance.GetCallResult_ORIG_TaskInfoByBusinessID(WOrderInfo.OrderID);
                if (mod != null)
                {
                    int isEstab = mod.IsEstablish.GetValueOrDefault(-1);
                    int? notEstab = mod.NotEstablishReason;
                    if (isEstab == 1)
                    {
                        ConcateHtml("是否接通", "是");
                    }
                    else if (isEstab == 0)
                    {
                        ConcateHtml("是否接通", "否");
                        if (notEstab.HasValue && notEstab.Value > 0)
                        {
                            ConcateHtml("失败原因", BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.NotEstablishReason), (int)notEstab));
                        }
                    }
                }
                //是否同步CRM
                if (WOrderInfo.IsSyncCRM.HasValue && WOrderInfo.IsSyncCRM.Value >= 0)
                {
                    ConcateHtml("是否同步到CRM", WOrderInfo.IsSyncCRM_Value == 1 ? "是" : "否");
                }
            }
        }
        /// 设置投诉数据
        /// <summary>
        /// 设置投诉数据
        /// </summary>
        /// <param name="model"></param>
        private void SetTSInfo()
        {
            if (WOrderInfo.CategoryID_Value == (int)BitAuto.ISDC.CC2012.Entities.WOrderCategoryEnum.W03_投诉)
            {
                string strLevelName = BLL.Util.GetEnumOptText(typeof(ComplaintLevelEnum), WOrderInfo.ComplaintLevel_Value);
                ConcateHtml("投诉级别", strLevelName);
            }
        }
        /// 设置关联数据
        /// <summary>
        /// 设置关联数据
        /// </summary>
        /// <param name="model"></param>
        private void SetWOrderDataInfo()
        {
            List<WOrderDataInfo> orderDataList = BLL.WOrderData.Instance.GetCallReportByOrderID(WorkOrderID, true);
            //cc话务id
            string ccCallids = "";
            //im对话id
            string imCsids = "";
            if (orderDataList != null && orderDataList.Count > 0)
            {
                UserID = BLL.Util.GetLoginUserID();
                foreach (WOrderDataInfo orderData in orderDataList)
                {
                    if (orderData.DataType_Value == (int)BusinessTypeEnum.CC)
                    {
                        if (!string.IsNullOrEmpty(orderData.AudioURL_Value))
                        {
                            ccCallids += GetAudioLink(orderData.AudioURL_Value, orderData.DataID_Value);
                        }
                    }
                    else if (orderData.DataType_Value == (int)BusinessTypeEnum.IM)
                    {
                        imCsids += GetIMLink(orderData.DataID_Value);
                    }
                }
            }
            ConcateHtml("工单记录", "<pre  >" + WOrderInfo.Content_Value + "</pre>&nbsp;&nbsp;" + ccCallids + imCsids, true);
            ToEndTr();
        }
        /// 根据录音获取链接
        /// </summary>
        /// <param name="ccCallids"></param>
        /// <param name="orderData"></param>
        /// <returns></returns>
        private string GetAudioLink(string audioUrl, long audioId)
        {
            return " <a href=\"javascript:void(0);\" onclick=\"javascript:ADTTool.PlayRecord('"
                               + audioUrl + "','/WOrderV2/PopLayer/PlayRecord.aspx');\" title=\""
                               + audioId + "\"><img src=\"/Images/callTel.png\" alt='"
                               + audioId + "' /></a>";
        }
        /// 根据CSID获取IM会话URL
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        public string GetIMLink(long csid)
        {
            if (BLL.WOrderData.Instance.GetDailogCount(csid) > 0)
            {
                if (WOrderInfo.ModuleSource_Value == (int)ModuleSourceEnum.M05_IM个人)
                {
                    string IMUrl = ConfigurationUtil.GetAppSettingValue("PersonalIMURL");
                    string url = Server.UrlEncode(IMUrl + "/ConversationHistoryForCC.aspx");
                    return "<a href='javascript:void(0)' onclick=\"javascript:GotoConversation(this,'"
                        + url + "','"
                        + UserID + "','"
                        + csid + "')\">"
                        + "<img src=\"/Images/workorder/imCSIcon.png\" alt='"
                        + csid + "' /></a>";
                }
                else if (WOrderInfo.ModuleSource_Value == (int)ModuleSourceEnum.M06_IM经销商_新车 ||
                  WOrderInfo.ModuleSource_Value == (int)ModuleSourceEnum.M07_IM经销商_二手车)
                {
                    return "<a href='javascript:void(0)' title='经销商版IM暂时不支持查看会话详细信息' >"
                        + "<img src=\"/Images/workorder/imCSIcon.png\" /></a>";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        /// 设置附件
        /// <summary>
        /// 设置附件
        /// </summary>
        private void SetAttachment()
        {
            List<CommonAttachmentInfo> list = BLL.CommonAttachment.Instance.GetCommonAttachmentList(BLL.Util.ProjectTypePath.WorkOrder, WOrderInfo.OrderID_Value);
            if (list.Count > 0)
            {
                TdCount = 1;

                buiderHtml.Append("</tr><tr> <th style='width:150px;' style=\"vertical-align: text-top;\">附件：</th><td style='width:810px;' colspan=\"3\"><div class=\"bzh\" style='WIDTH: auto'>");
                for (int i = 0; i < list.Count; i++)
                {
                    buiderHtml.Append("<img style='width: 130px; height: 90px;' class=\"order_file_img\" "
                        + "alt='" + list[i].FileName_Value + "' "
                        + "src=\"" + list[i].SmallFilePath + "\" "
                        + "data=\"" + list[i].FilePath_Value + "\" "
                        + "title=\"" + list[i].FileName_Value + "\"/>");
                }
                buiderHtml.Append("</div></td>");
                ToEndTr();
                //需要在js中调用 BindSlider(".order_file_img", "data") ;
            }
        }
        /// table增加一组td
        /// </summary>
        /// <param name="tdName"></param>
        /// <param name="tdVal"></param>
        private void ConcateHtml(string tdName, string tdVal, bool needColspan = false)
        {
            if (TdCount % 2 != 0)
            {
                buiderHtml.Append("</tr><tr>");
            }
            if (needColspan)
            {
                buiderHtml.Append("<th style='width:150px;'>" + tdName + "：</th>");
                buiderHtml.Append("<td style='width:810px;' colspan=\"3\">" + tdVal + "&nbsp;</td>");
                TdCount = TdCount + 2;
            }
            else
            {
                buiderHtml.Append("<th style='width:150px;'>" + tdName + "：</th>");
                buiderHtml.Append("<td style='width:330px;'>" + tdVal + "&nbsp;</td>");
                TdCount++;
            }

        }
        /// 换行
        /// <summary>
        /// 换行
        /// </summary>
        private void ToEndTr()
        {
            if (TdCount % 2 == 0)
            {
                buiderHtml.Append("<th style='width:150px;'>&nbsp;</th>");
                buiderHtml.Append("<td style='width:330px;'>&nbsp;</td>");
            }
            TdCount = 1;
        }
    }
}