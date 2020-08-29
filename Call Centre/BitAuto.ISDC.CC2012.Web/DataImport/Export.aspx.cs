using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.DataImport
{
    public partial class Export : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {

        public string ExportStr
        {
            get { return BLL.Util.GetCurrentRequestStr("hidData"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(ExportStr))
                {
                    //char[] splitchar = new char[] { '&', '*' };
                    string[] Datastr = ExportStr.Substring(0,ExportStr.Length-1).Split('&');
                    DataTable ExportDataTable = new DataTable();
                    ExportDataTable.Columns.Add("username", typeof(System.String));
                    ExportDataTable.Columns.Add("sex", typeof(System.String));
                    ExportDataTable.Columns.Add("tel1", typeof(System.String));
                    ExportDataTable.Columns.Add("custcategory", typeof(System.String));
                    ExportDataTable.Columns.Add("tel2", typeof(System.String));
                    ExportDataTable.Columns.Add("email", typeof(System.String));
                    ExportDataTable.Columns.Add("province", typeof(System.String));
                    ExportDataTable.Columns.Add("city", typeof(System.String));
                    ExportDataTable.Columns.Add("county", typeof(System.String));
                    ExportDataTable.Columns.Add("address", typeof(System.String));
                    ExportDataTable.Columns.Add("datasource", typeof(System.String));
                    ExportDataTable.Columns.Add("EnquiryType", typeof(System.String));
                    ExportDataTable.Columns.Add("brand", typeof(System.String));
                    ExportDataTable.Columns.Add("carmodel", typeof(System.String));
                    ExportDataTable.Columns.Add("Dealer", typeof(System.String));
                    ExportDataTable.Columns.Add("callrecord", typeof(System.String));
                    ExportDataTable.Columns.Add("failinfo", typeof(System.String));
                    for (int i = 0; i < Datastr.Length; i++)
                    {
                        string[] RowData = Datastr[i].Split('|');

                        DataRow newRow = ExportDataTable.NewRow();
                        newRow["username"] = RowData[0];
                        newRow["sex"] = RowData[1];
                        newRow["tel1"] = RowData[2];
                        newRow["custcategory"] = RowData[3];
                        newRow["tel2"] = RowData[4];
                        newRow["email"] = RowData[5];

                        newRow["province"] = RowData[6];
                        newRow["city"] = RowData[7];
                        newRow["county"] = RowData[8];

                        newRow["address"] = RowData[9];
                        newRow["datasource"] = RowData[10];

                        newRow["EnquiryType"] = RowData[11];
                        newRow["brand"] = RowData[12];

                        newRow["carmodel"] = RowData[13];
                        newRow["Dealer"] = RowData[14];
                        newRow["callrecord"] = RowData[15];
                        newRow["failinfo"] = RowData[16];
                        ExportDataTable.Rows.Add(newRow);
                    }

                    ExprotExcel(ExportDataTable);
                }
            }
        }
        private void ExprotExcel(DataTable dtCost)
        {


            //BitAuto.Utils.ExportDataHelper.ExportDataSetToExcelWithTitle("会员信息导出",
            //new string[] { "省份", "城市", "区县", "经销商名称", "品牌", "类别", "会员ID号", "类型", "合作名称", "销售类型", "执行周期", "创建时间", "备注" }, dtCost.DataSet, new string[] { "provincename", "cityname", "countyname", "membername", "branname", "membertype", "membercode", "kind", "collaboratename", "saletype", "executeTimespan", "createtime", "note" }, "memberinfo");


            //---csv
            StringWriter sw = new StringWriter();
            sw.WriteLine("姓名*\t性别*\t电话1*\t客户分类*\t电话2\t邮箱\t地区（省）\t地区（市）\t地区（区县）\t地址\t数据来源\t咨询类型\t品牌\t车型\t推荐经销商\t来电记录\t导入错误信息");
            foreach (DataRow dr in dtCost.Rows)
            {
                sw.WriteLine(dr["username"] + "\t" + dr["sex"] + "\t" + dr["tel1"].ToString() + "\t" + dr["custcategory"] + "\t" + dr["tel2"] + "\t" + dr["email"] + "\t" + dr["province"].ToString() + "\t" + dr["city"] + "\t" + dr["county"] + "\t" + dr["address"] + "\t" + dr["datasource"].ToString() + "\t" + dr["EnquiryType"] + "\t" + dr["brand"] + "\t" + dr["carmodel"] + "\t" + dr["Dealer"].ToString() + "\t" + dr["callrecord"] + "\t" + dr["failinfo"]);
            }
            sw.Close();
            Response.AddHeader("Content-Disposition", "attachment; filename=FailRecord.xls");
            Response.ContentType = "application/vnd.xls";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.Write(sw);
            Response.End();
            //---


            //StringWriter sw = new StringWriter();
            //sw.WriteLine("省份\t城市\t区县\t经销商名称\t品牌\t类别\t会员ID号\t类型\t合作名称\t销售类型\t执行周期\t创建时间\t备注");
            //foreach (DataRow dr in dtCost.Rows)
            //{
            //    sw.WriteLine(dr["provincename"] + "\t" + dr["cityname"] + "\t" + dr["countyname"].ToString() + "\t" + dr["membername"] + "\t" + dr["branname"] + "\t" + dr["membertype"] + "\t" + dr["membercode"].ToString() + "\t" + dr["kind"] + "\t" + dr["collaboratename"] + "\t" + dr["saletype"] + "\t" + dr["executeTimespan"].ToString() + "\t" + dr["createtime"] + "\t" + dr["note"]);
            //}
            //sw.Close();
            //Response.ContentType = "application/vnd.xls";
            //dtCost.TableName = "memberinfo";
            //Response.AddHeader("Content-Disposition", "attachment;filename=\"" + System.Web.HttpUtility.UrlEncode(dtCost.TableName, System.Text.Encoding.UTF8) + ".xls\"");
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            //Response.Write(sw);
            //Response.End();
        }
    }
}