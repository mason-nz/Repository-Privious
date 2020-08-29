using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Web.WebServices;
using BitAuto.ISDC.CC2012.Web.YiJiKeService;
using BitAuto.ISDC.CC2012.WebService.YTG;
using BitAuto.ISDC.CC2012.Web.CallRecordService;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web
{
    public partial class OracleTest : System.Web.UI.Page
    {
        private string key = "2D55A797-6386-4D35-A6B6-B340CF425B2E";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int total = 0;
                DataTable dt = ReportOracleBLL.Instance.GetDataTable("select * from gen_cfg.cfg_person", 1, 10, out total);
                foreach (DataRow dr in dt.Rows)
                {
                    string a = dr["first_name"].ToString();
                }

                MessageBoxShow(dt.Rows.Count.ToString());
                return;
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ReportOracleBLL.Instance.GetDataTable("select time_key,object_id,round(pc_t_calls,4) as pc_t_calls from v_Agent_Day order by object_id");
                MessageBoxShow(dt.Rows.Count.ToString());
                return;
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                int total = 0;
                DataTable dt = ReportOracleBLL.Instance.GetDataTable("select time_key,object_id,round(pc_t_calls,4) as pc_t_calls from v_Agent_Day where 1=2");
                DataTable dt2 = ReportOracleBLL.Instance.GetDataTable("select time_key,object_id,round(pc_t_calls,4) as pc_t_calls from v_Agent_Day where 1=2", 521, 10, out total);
                MessageBoxShow((dt.Rows.Count + dt2.Rows.Count).ToString());
                return;
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
        }

        private void MessageBoxShow(string mes)
        {
            Response.Write("<script>alert('" + mes + "')</script>");
        }

        protected void Button4_Click(object sender, EventArgs e)
        {

        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            string sql = @"select
                                    t.begin_time,
                                    t.hour_hh24,
                                    t.week_n_in_year,
                                    t.month_n_in_year,
                                    t.year,
                                    o.object_name,
                                    decode(o.object_name,'81400@SIPSwitch',1,'81401@SIPSwitch',2,null) as object_type,
                                    round(av_t_abandoned,5) as av_t_abandoned,
                                    round(av_t_answered,5) as av_t_answered,
                                    round(av_t_distributed,5) as av_t_distributed,
                                    n_abandoned,
                                    n_answered,
                                    n_distributed,
                                    n_entered,
                                    n_abandoned_in_tr,
                                    n_distrib_in_tr,
                                    t_abandoned,
                                    t_answered,
                                    t_distributed
                                    from v_routepoint_hour v, o_routepoint_hour o , t_routepoint_hour t
                                    where v.time_key=t.time_key and v.object_id=o.object_id
                                    and o.delete_time is null
                                    and o.object_name in ('81400@SIPSwitch','81401@SIPSwitch')
                                    order by t.begin_time,o.object_name";


            int total = ReportOracleBLL.Instance.GetDataTableCount(sql);
            //分页查询，每页100条
            int page = total / 100 + ((total % 100 == 0) ? 0 : 1);
            for (int p = 1; p <= page; p++)
            {
                DataTable dt = ReportOracleBLL.Instance.GetDataTable(sql, p, 100, out total);
                int rowlen = dt.Rows.Count;
                byte[] b = BitAuto.ISDC.CC2012.BLL.Util.GetBinaryFormatData(dt.DataSet);
                double mlen = (double)b.Length / 1024.0 / 1024.0;
                string msg = "";

                CallReportService service = new CallReportService();
                bool a = service.SyncRoutepointHourData(key, b, ref msg);
            }
        }

        protected void Button6_Click(object sender, EventArgs e)
        {

        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            CallReportService service = new CallReportService();
            DateTime? a = service.GetMaxDateTimeFromHourData(key);
            DateTime? b = service.GetMaxDateTimeFrom15MinData(key);

            service.ClearVendor15MinDataFormBeginToEnd(key, DateTime.Today.AddDays(-1), DateTime.Today, Entities.Vender.Holly);
        }

        protected void Button8_Click(object sender, EventArgs e)
        {
            string id = TextBox1.Text;
            int bc = int.Parse(TextBox2.Text);
            CCYiJiKeService a = new CCYiJiKeService();
            string r = a.PullCustNotify("2D55A797-6386-4D35-A6B6-B340CF425B2E", id, bc);
            Response.Write("<script>alert('" + r + "');</script>");
        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            string id = TextBox1.Text;
            CCYiJiKeService a = new CCYiJiKeService();
            string r = a.EndCCProject("2D55A797-6386-4D35-A6B6-B340CF425B2E", id);
            Response.Write("<script>alert('" + r + "');</script>");
        }

        protected void Button10_Click(object sender, EventArgs e)
        {
            string id = TextBox1.Text;
            int bc = int.Parse(TextBox2.Text);
            CCYiJiKeService a = new CCYiJiKeService();
            a.PullCJKLeadTask("2D55A797-6386-4D35-A6B6-B340CF425B2E", id, bc, 100);
        }

        protected void Button11_Click(object sender, EventArgs e)
        {
            string id = TextBox1.Text;
            CCYiJiKeService a = new CCYiJiKeService();
            a.EndCJKProject("2D55A797-6386-4D35-A6B6-B340CF425B2E", id);
        }







        protected void Button12_Click(object sender, EventArgs e)
        {
            TestYTGActivityTest.InsertYTGActivity();
        }
        protected void Button14_Click(object sender, EventArgs e)
        {
            TestYTGActivityTest.UpdateYTGActivity(TextBox3.Text);
        }

        protected void Button13_Click(object sender, EventArgs e)
        {
            //TestYTGActivityTest.EndYTGActivity();
            //BLL.Util.isHuiMaiCHE("1", "1");
        }

        protected void Button15_Click(object sender, EventArgs e)
        {
            DictionaryDataCacheService a = new DictionaryDataCacheService();
            bool b = a.ResetDictionaryDataCache("2D55A797-6386-4D35-A6B6-B340CF425B2E");
        }

        protected void Button16_Click(object sender, EventArgs e)
        {
            CCDataInterfaceService service = new CCDataInterfaceService();
            service.GetEmployeeAgentInfo("2D55A797-6386-4D35-A6B6-B340CF425B2E", 10899);
        }

        protected void Button17_Click(object sender, EventArgs e)
        {
            CCDataInterfaceService service = new CCDataInterfaceService();
            service.GetCustBasicInfo("2D55A797-6386-4D35-A6B6-B340CF425B2E", "15012354879", "舒女士");
        }

        protected void Button18_Click(object sender, EventArgs e)
        {

        }
        public const string KeyCode = "autocallsyncdata_!@QW#E";
        private AutoCallSyncDataService autocall = new AutoCallSyncDataService();
        //查询北京自动外呼项目表
        protected void Button19_Click(object sender, EventArgs e)
        {
            string msg = "";
            byte[] data = autocall.GetAutoCallProjectFormBJToXiAn(KeyCode, 0, out msg);
            if (data != null)
            {
                DataTable dt = BLL.Util.BinaryToDataTable(data);
            }
        }
        //查询北京自动外呼任务表
        protected void Button20_Click(object sender, EventArgs e)
        {
            string msg = "";
            autocall.GetAutoCallTaskFormBJToXiAn(KeyCode, 0, out msg);
        }
        //获取统计表最大时间戳
        protected void Button21_Click(object sender, EventArgs e)
        {
            string msg = "";
            autocall.GetAutoCallStatMaxTimeStamp(KeyCode, out msg);
        }
        //获取明细表最大时间戳
        protected void Button22_Click(object sender, EventArgs e)
        {
            string msg = "";
            autocall.GetAutoCallDetailMaxTimeStamp(KeyCode, out msg);
        }

        protected void Button23_Click(object sender, EventArgs e)
        {
            BitAuto.ISDC.CC2012.Web.CallRecordService.AgentTimeState server = new BitAuto.ISDC.CC2012.Web.CallRecordService.AgentTimeState();
            var dt = server.GetManageBusinessGroups("2D55A797-6386-4D35-A6B6-B340CF425B2E", 10291);
        }

        protected void Button24_Click(object sender, EventArgs e)
        {
            BitAuto.ISDC.CC2012.Web.CallRecordService.AgentTimeState server = new BitAuto.ISDC.CC2012.Web.CallRecordService.AgentTimeState();
            var dt = server.GetAllEmployeeAgentAndBusinessGroup("2D55A797-6386-4D35-A6B6-B340CF425B2E");
        }

        protected void Button25_Click(object sender, EventArgs e)
        {
            string logpath = Server.MapPath("/log") + "\\SendMail.Log";
            byte[] data = BLL.Util.FileToBinary(logpath);

            string copypath = Server.MapPath("/ClientLog") + "\\SendMail.Log";
            BLL.Util.BinaryToFile(copypath, data);
        }

        protected void Button26_Click(object sender, EventArgs e)
        {
            string logpath = Server.MapPath("/log") + "\\SendMail.Log";
            byte[] data = BLL.Util.FileToBinary(logpath);
            string key = "yiche-ClineLog-!@#$#@!";
            BitAuto.ISDC.CC2012.Web.CallRecordService.ClientAssistantService server = new CallRecordService.ClientAssistantService();
            bool a = server.PushClientLogForAgent(key, data, DateTime.Today, 1, Entities.Vender.Holly);
        }

        protected void Button27_Click(object sender, EventArgs e)
        {
            //string key = "yiche-ClineLog-!@#$#@!";
            //BitAuto.ISDC.CC2012.Web.CallRecordService.ClientAssistantService server = new CallRecordService.ClientAssistantService();
            //byte[] data = server.GetClientLogForManage(key, DateTime.Today, 1, Entities.Vender.Holly);

            //string copypath = Server.MapPath("/log/test") + "\\SendMail.Log";
            //BLL.Util.BinaryToFile(copypath, data);
        }

        protected void Button28_Click(object sender, EventArgs e)
        {
            string key = "yiche-ClineLog-!@#$#@!";
            BitAuto.ISDC.CC2012.Web.CallRecordService.ClientAssistantService server = new CallRecordService.ClientAssistantService();
            string a = server.GetClientServerVersion(key, "Versions_Holly");
            string b = server.GetClientServerVersion(key, "Versions_Genesys");
        }

        protected void Button29_Click(object sender, EventArgs e)
        {
            //DataTable dt = BLL.ClientLogRequire.Instance.GetAllEmployeeAgent(new DateTime(2015, 1, 1), DateTime.Today, null);
        }

        protected void Button30_Click(object sender, EventArgs e)
        {
            string msg = "";
            BlackWhiteService server = new BlackWhiteService();
            byte[] b = server.GetBlackListDataForOutCall(key, 2, 4, out msg);
            DataTable dt = BLL.Util.BinaryToDataTable(b);

        }

        protected void Button31_Click(object sender, EventArgs e)
        {
            string msg = "";
            string ccCustID = "";
            bool a = BLL.CustBasicInfo.Instance.InsertCustInfo("强斐测试-1", new string[] { "13811510109", "13811510108" }, 1, -1, 1, null, null, out msg, out ccCustID);
        }

        protected void Button32_Click(object sender, EventArgs e)
        {
            string msg = "";
            WebServices.EmployeeAgent service = new WebServices.EmployeeAgent();
            DataTable dt = service.GetEmployeeAgentNewData(key, 0, out msg);

            Response.Write("<script>alert(" + dt.Rows.Count + ")</script>");


            BitAuto.ISDC.CC2012.Web.CallRecordService.AgentTimeState server = new CallRecordService.AgentTimeState();
            server.SendLogToServer(key, "", "测试数据");

        }

        protected void Button33_Click(object sender, EventArgs e)
        {
            string phone = TextBox4.Text;
            VerifyPhoneFormat server = new VerifyPhoneFormat();
            string outnum = "";
            string msg = "";
            var a = server.VerifyFormat("E0F3C0C3-5317-4D5E-9548-7E31A506EC37", phone, out outnum, out msg);
            var b = server.VerifyFormatXiAn("E0F3C0C3-5317-4D5E-9548-7E31A506EC37", phone, out outnum, out msg);
        }

        protected void Button34_Click(object sender, EventArgs e)
        {
            BlackWhiteService server = new BlackWhiteService();
            string msg = "";
            var b = server.GetChangedCallDisplayFromBJ(key, 0, out msg);
        }

        protected void Button35_Click(object sender, EventArgs e)
        {
            WOrderRequest q = WOrderRequest.AddWOrderComeIn_CustPool("18629257531");
            HyperLink1.Text = "http://ncc.sys1.bitauto.com/WOrderV2/AddWOrderInfo.aspx?" + q.ToString();
            HyperLink1.NavigateUrl = HyperLink1.Text;
        }

        protected void Button36_Click(object sender, EventArgs e)
        {
            WOrderRequest q = WOrderRequest.AddWOrderComeIn_NoPhone(ModuleSourceEnum.M02_工单);
            HyperLink1.Text = "http://ncc.sys1.bitauto.com/WOrderV2/AddWOrderInfo.aspx?" + q.ToString();
            HyperLink1.NavigateUrl = HyperLink1.Text;
        }

        protected void Button37_Click(object sender, EventArgs e)
        {
            string srt = TextBox5.Text;
            TextBox6.Text = BLL.Util.EncryptString(srt);
        }
        protected void Button38_Click1(object sender, EventArgs e)
        {
            WOrderRequest q = WOrderRequest.AddWOrderComeIn_CallIn(WorkOrderDataSource.HMCLine, "18629257531");
            q.CustType = CustTypeEnum.T02_经销商;
            HyperLink1.Text = "http://ncc.sys1.bitauto.com/WOrderV2/AddWOrderInfo.aspx?" + q.ToString();
            HyperLink1.NavigateUrl = HyperLink1.Text;
        }

        protected void Button39_Click(object sender, EventArgs e)
        {
            WOrderRequest q = WOrderRequest.AddWOrderComeIn_CallOut("18629257531", "12345678", "实际联系人", 1, "");
            HyperLink1.Text = "http://ncc.sys1.bitauto.com/WOrderV2/AddWOrderInfo.aspx?" + q.ToString();
            HyperLink1.NavigateUrl = HyperLink1.Text;
        }

        protected void Button40_Click(object sender, EventArgs e)
        {
            WOrderRequest q = WOrderRequest.AddWOrderComeIn_CRMCustID("12345678");
            HyperLink1.Text = "http://ncc.sys1.bitauto.com/WOrderV2/AddWOrderInfo.aspx?" + q.ToString();
            HyperLink1.NavigateUrl = HyperLink1.Text;
        }

        protected void Button41_Click(object sender, EventArgs e)
        {
            WOrderRequest q = WOrderRequest.AddWOrderComeIn_IMGR(CallSourceEnum.C03_IM对话, "18629257531", 10000000001, "IM-强斐", 1, -1, -1, -1, -1, -1);
            HyperLink1.Text = "http://ncc.sys1.bitauto.com/WOrderV2/AddWOrderInfo.aspx?" + q.ToString();
            HyperLink1.NavigateUrl = HyperLink1.Text;
        }

        protected void Button42_Click(object sender, EventArgs e)
        {
            WOrderRequest q = WOrderRequest.AddWOrderComeIn_IMJXS(CallSourceEnum.C03_IM对话, ModuleSourceEnum.M06_IM经销商_新车, "18629257531", 10000000002, "IM-qf", 1, -1, -1, -1, "112233445566");
            HyperLink1.Text = "http://ncc.sys1.bitauto.com/WOrderV2/AddWOrderInfo.aspx?" + q.ToString();
            HyperLink1.NavigateUrl = HyperLink1.Text;
        }

        protected void Button43_Click(object sender, EventArgs e)
        {
            HyperLink2.Text = "http://ncc.sys1.bitauto.com/WOrderV2/WOrderProcess.aspx?OrderID=" + TextBox7.Text; ;
            HyperLink2.NavigateUrl = HyperLink2.Text;

            WorkOrderDataSource a = (WorkOrderDataSource)102;
            WorkOrderDataSource b = (WorkOrderDataSource)102222;
            bool c = Enum.IsDefined(typeof(WorkOrderDataSource), 102);

        }

        protected void Button44_Click(object sender, EventArgs e)
        {
            string json = TextBox6.Text;
            GetCallRecordList server = new GetCallRecordList();
            server.InsertCustData("2D55A797-6386-4D35-A6B6-B340CF425B2E", "{CustName:'masj-yangyh234',Sex:'2',Tels:[13146611863,13146611864],CallID:'351951471501580514',BusinessID:'323',BGID:'1',SCID:'2',MemberCode:''}");
        }
    }
}