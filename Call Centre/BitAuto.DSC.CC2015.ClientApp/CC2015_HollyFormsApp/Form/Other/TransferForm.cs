using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils;

namespace CC2015_HollyFormsApp
{
    public partial class TransferForm : Form
    {
        //没有通话
        private bool NoCall = false;

        /// 输出号码
        /// <summary>
        /// 输出号码
        /// </summary>
        public string OutNum = "";
        /// 号码类型
        /// <summary>
        /// 号码类型
        /// </summary>
        public HollyContactHelper.ConDeviceType ConDeviceType = HollyContactHelper.ConDeviceType.客服工号;

        //热线
        public Dictionary<int, string> Lines = null;
        //技能组
        public DataTable SkillData = null;

        public TransferForm()
        {
            InitializeComponent();
            this.NoCall = false;
            this.FormClosed += new FormClosedEventHandler(TransferForm_FormClosed);
        }

        private void TransferForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
        }

        public TransferForm(bool notcall)
        {
            InitializeComponent();
            this.NoCall = notcall;
        }

        private void TransferForm_Load(object sender, EventArgs e)
        {
            BindData();
            timer1.Start();
            if (NoCall)
            {
                btnAgent.Enabled = false;
                btnSkill.Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            BandAgentList();
            timer1.Start();
        }

        #region 热线
        //查询技能组数据和热线数据
        private void BindData()
        {
            //获取技能组数据
            SkillData = Common.GetAllSkillInfoAndLineInfo();
            //热线
            Lines = new Dictionary<int, string>();
            foreach (DataRow dr in SkillData.Rows)
            {
                Lines[CommonFunction.ObjectToInteger(dr["CDID"])] = dr["Remark"].ToString();
            }
            //绑定热线
            cb_line.Items.Clear();
            foreach (int cdid in Lines.Keys)
            {
                cb_line.Items.Add(new KeyValue(cdid.ToString(), Lines[cdid]));
            }
            //获取当前热线
            KeyValue curtel = GetCurrentLine();
            if (curtel == null)
            {
                cb_line.SelectedIndex = 0;
            }
            else
            {
                cb_line.SelectedItem = curtel;
            }
            cb_line_SelectedIndexChanged(null, null);
            this.cb_line.SelectedIndexChanged += new System.EventHandler(this.cb_line_SelectedIndexChanged);
        }
        //切换热线时赋值
        private void cb_line_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_skill.Items.Clear();
            int cdid = GetSelectLine();
            if (cdid > 0)
            {
                cb_skill.Items.Add(new KeyValue("-1", "请选择"));
                foreach (DataRow dr in SkillData.Select("CDID=" + cdid))
                {
                    cb_skill.Items.Add(new KeyValue(dr["SGID"].ToString(), dr["ManufacturerSGID"].ToString() + "-" + dr["Name"].ToString()));
                }
                cb_skill.SelectedIndex = 0;
            }
        }

        /// 获取选择的技能组ID
        /// <summary>
        /// 获取选择的技能组ID
        /// </summary>
        /// <returns></returns>
        private int GetSelectSKill()
        {
            KeyValue kv = cb_skill.SelectedItem as KeyValue;
            if (kv != null)
            {
                return CommonFunction.ObjectToInteger(kv.Key);
            }
            else return -1;
        }
        /// 获取热线ID
        /// <summary>
        /// 获取热线ID
        /// </summary>
        /// <returns></returns>
        private int GetSelectLine()
        {
            KeyValue kv = cb_line.SelectedItem as KeyValue;
            if (kv != null)
            {
                return CommonFunction.ObjectToInteger(kv.Key);
            }
            else return -1;
        }
        /// 转接技能组
        /// <summary>
        /// 转接技能组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSkill_Click(object sender, EventArgs e)
        {
            if (NoCall) return;
            int sgid = GetSelectSKill();
            if (sgid == -1)
            {
                MessageBox.Show("请选择一个技能组");
                cb_skill.Focus();
                return;
            }
            ConDeviceType = HollyContactHelper.ConDeviceType.技能组;
            OutNum = sgid.ToString();
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        //获取当前热线号码
        private KeyValue GetCurrentLine()
        {
            //获取热线号码
            string tel = HollyContactHelper.Instance.GetLuodiNum();
            if (!string.IsNullOrEmpty(tel) && tel.Length > 8)
            {
                tel = tel.Substring(tel.Length - 8, 8);
            }
            DataRow[] drs = SkillData.Select("TelMainNum='" + tel + "'");
            if (drs.Length > 0)
            {
                return new KeyValue(drs[0]["CDID"].ToString(), drs[0]["Remark"].ToString());
            }
            return null;
        }
        #endregion

        #region 人员
        /// 查询客服数据
        /// <summary>
        /// 查询客服数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetAgentData()
        {
            DataTable dt = CreateData();
            string where = "";
            //状态条件 且 分机号前两位相同
            where += " AND t1.STATE=3 AND t1.ExtensionNum LIKE '" + LoginUser.ExtensionNum.Substring(0, 2) + "%' ";
            //热线+技能组
            int cdid = GetSelectLine();
            int sgid = GetSelectSKill();
            if (cdid > 0 && sgid <= 0)
            {
                //所在热线
                where += " AND UserID IN (SELECT UserID FROM dbo.UserSkillDataRight WHERE SGID IN (SELECT SGID FROM dbo.SkillGroup WHERE CDID=" + cdid + "))";
            }
            else if (sgid > 0)
            {
                //所在技能组
                where += " AND UserID IN (SELECT UserID FROM dbo.UserSkillDataRight WHERE SGID =" + sgid + ")";
            }
            //模糊查询条件
            string search = StringHelper.SqlFilter(te_input.Text.Trim());
            if (search != "")
            {
                //工号，姓名，分机号
                where += " AND (t2.AgentNum like '%" + search + "%' or t1.AgentName like '%" + search + "%' or t1.ExtensionNum like '%" + search + "%')";
            }
            //查询
            DataTable mydt = AgentTimeStateHelper.Instance.GetAgentStateOnLineByWhere(where);
            foreach (DataRow row in mydt.Rows)
            {
                dt.Rows.Add(new object[] { row["AgentNum"], row["AgentName"], row["ExtensionNum"], "置闲", row["BGName"], row["RegionName"] });
            }
            return dt;
        }
        /// 绑定客服
        /// <summary>
        /// 绑定客服
        /// </summary>
        private void BandAgentList()
        {
            try
            {
                //刷新
                DataTable dt = GetAgentData();

                //重置窗口表格数据
                int i = 0;
                List<int> deleteidxs = new List<int>();
                foreach (DataGridViewRow row in dgvAgentList.Rows)
                {
                    if (i == dt.Rows.Count)
                    {
                        //需要删除的内容
                        deleteidxs.Add(row.Index);
                    }
                    else
                    {
                        row.Cells["AgentNum"].Value = dt.Rows[i]["AgentNum"].ToString();
                        row.Cells["AgentName"].Value = dt.Rows[i]["AgentName"].ToString();
                        row.Cells["ExtensionNum"].Value = dt.Rows[i]["ExtensionNum"].ToString();
                        row.Cells["StateName"].Value = dt.Rows[i]["StateName"].ToString();
                        row.Cells["BGName"].Value = dt.Rows[i]["BGName"].ToString();
                        row.Cells["RegionName"].Value = dt.Rows[i]["RegionName"].ToString();
                        i++;
                    }
                }
                //删除数据
                deleteidxs.Reverse();
                foreach (int key in deleteidxs)
                {
                    dgvAgentList.Rows.RemoveAt(key);
                }
                //追加数据
                for (; i < dt.Rows.Count; i++)
                {
                    dgvAgentList.Rows.Add(dt.Rows[i].ItemArray);
                }
            }
            catch (Exception e)
            {
                Loger.Log4Net.Error("[AgentList][BandAgentList] 获取客服信息出错：", e);
            }
        }
        /// 初始化数据
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        private DataTable CreateData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("AgentNum", typeof(string));
            dt.Columns.Add("AgentName", typeof(string));
            dt.Columns.Add("ExtensionNum", typeof(string));
            dt.Columns.Add("StateName", typeof(string));
            dt.Columns.Add("BGName", typeof(string));
            dt.Columns.Add("RegionName", typeof(string));
            return dt;
        }
        /// 转接客服
        /// <summary>
        /// 转接客服
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAgentList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (NoCall) return;
            ConDeviceType = HollyContactHelper.ConDeviceType.分机号;
            OutNum = GetAgentNum(e.RowIndex);
            if (OutNum == "")
            {
                MessageBox.Show("分机号为空，不能转接");
                cb_skill.Focus();
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        /// 获取行信息
        /// <summary>
        /// 获取行信息
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetAgentNum(int index)
        {
            if (ConDeviceType == HollyContactHelper.ConDeviceType.客服工号)
            {
                return dgvAgentList.Rows[index].Cells["AgentNum"].Value.ToString();
            }
            else if (ConDeviceType == HollyContactHelper.ConDeviceType.分机号)
            {
                return dgvAgentList.Rows[index].Cells["ExtensionNum"].Value.ToString();
            }
            return "";
        }
        /// 转接客服
        /// <summary>
        /// 转接客服
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgent_Click(object sender, EventArgs e)
        {
            if (NoCall) return;
            if (dgvAgentList.SelectedRows.Count > 0)
            {
                var row = dgvAgentList.SelectedRows[0];
                ConDeviceType = HollyContactHelper.ConDeviceType.分机号;
                OutNum = GetAgentNum(row.Index);
                if (OutNum == "")
                {
                    MessageBox.Show("分机号为空，不能转接");
                    cb_skill.Focus();
                    return;
                }
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("请先选择一个客服！");
            }
        }
        #endregion
    }
}
