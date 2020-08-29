using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RECONCOMLibrary;
using System.Runtime.InteropServices;

namespace CC2012_CarolFormsApp
{
    public partial class AgentList : Form
    {
        private DataTable BusyReasonDT = null;//置忙原因数据
        private int RowIndex = -1;
        private bool IsLocationSelected = false;
        public string ConsultExtension = "";//转接坐席分机号码

        public AgentList()
        {
            InitializeComponent();

            #region 初始化置忙内容
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Value");

            dt.Rows.Add(new object[] { "小休", "1" });
            dt.Rows.Add(new object[] { "任务回访", "2" });
            dt.Rows.Add(new object[] { "业务处理", "3" });
            dt.Rows.Add(new object[] { "会议", "4" });
            dt.Rows.Add(new object[] { "培训", "5" });
            dt.Rows.Add(new object[] { "离席", "6" });
            BusyReasonDT = dt;
            //this.toolStripComboBox1.ComboBox.DataSource = dt;
            //toolStripComboBox1.ComboBox.DisplayMember = "Name";
            //toolStripComboBox1.ComboBox.ValueMember = "Value";
            //this.toolStripComboBox1.ComboBox.SelectedIndex = 0;
            #endregion

            BandAgentList();
        }

        private void AgentList_Load(object sender, EventArgs e)
        {
            this.skinEngine1.SkinFile = "skin/" + Common.GetSkinFileName();
            this.txtExtension.Focus();
        }

        /// <summary>
        /// 绑定坐席当前状态列表
        /// </summary>
        private void BandAgentList()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Extension", typeof(string));//分机号码
                dt.Columns.Add("StateName", typeof(string));//状态名称
                //dt.Columns.Add("State", typeof(int));//状态
                //dt.Columns.Add("Party_Number", typeof(string));//分机号码
                dt.Columns.Add("OrderNum", typeof(string));//排序字段
                for (int i = 1040; i < 1099; i++)
                {
                    AgentEvent ae = new AgentEvent();
                    int r = Program.rc.T_AgentGetState(i.ToString(), 2, out ae);
                    if (ae != null)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Extension"] = ae.AgentUsername;
                        //dr["State"] = ae.AgentState;
                        dr["StateName"] = ShowAgentState(ae.AgentState, ae.AgentAuxState);
                        //dr["Party_Number"] = ae.Party_Number;
                        if ((int)ae.AgentState == (int)enAgentState.agentStateReady)
                        {
                            dr["OrderNum"] = "1";
                        }
                        dt.Rows.Add(dr);
                        Marshal.ReleaseComObject(ae);
                    }
                }
                dt.DefaultView.Sort = "OrderNum Desc";
                dgvAgentList.DataSource = dt.DefaultView.ToTable();

                foreach (DataGridViewRow item in dgvAgentList.Rows)
                {
                    item.Selected = false;
                }

                if (IsLocationSelected)
                {
                    if (RowIndex >= 0)
                    {
                        dgvAgentList.Rows[RowIndex].Selected = true;
                    }
                }
            }
            catch (Exception e)
            {
                Loger.Log4Net.Error("获取坐席状态出错", e);
            }
        }

        #region 获取坐席状态名称
        /// <summary>
        /// 获取坐席状态名称
        /// </summary>
        /// <param name="agentState">坐席状态</param>
        /// <param name="agentAuxState">坐席附加状态</param>
        /// <returns></returns>
        private string ShowAgentState(int agentState, int agentAuxState)
        {
            string AgentState = string.Empty;
            switch (agentState)
            {
                case (int)enAgentState.agentStateAfterCallWork: AgentState = "话后";
                    break;
                case (int)enAgentState.agentStateIntiated: AgentState = "状态初始化";
                    break;
                case (int)enAgentState.agentStateLoggedOff: AgentState = "签出";
                    break;
                case (int)enAgentState.agentStateLoggedOn: AgentState = "签入";
                    break;
                case (int)enAgentState.agentStateNotReady:
                    if (BusyReasonDT != null)
                    {
                        BusyReasonDT.DefaultView.RowFilter = "Value=" + agentAuxState;
                        DataTable dt = BusyReasonDT.DefaultView.ToTable();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            AgentState = "置忙(" + dt.Rows[0]["Name"].ToString() + ")";
                        }
                        else
                        {
                            AgentState = "置忙(" + agentAuxState + ")";
                        }
                    }
                    break;
                case (int)enAgentState.agentStateReady: AgentState = "置闲";
                    break;
                case (int)enAgentState.agentStateRinging: AgentState = "振铃";
                    break;
                case (int)enAgentState.agentStateUnknown: AgentState = "未知";
                    break;
                case (int)enAgentState.agentStateWorking: AgentState = "工作中";
                    break;
                default: AgentState = "错误";
                    break;
            }
            return AgentState;
        }
        #endregion

        private void timer1_Elapsed(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            BandAgentList();
            this.timer1.Enabled = true;
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose(true);
        }

        /// <summary>
        /// 确定操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtExtension.Text))
            {
                MessageBox.Show("转接分机号码不能为空！");
            }
            else
            {
                ConsultExtension = txtExtension.Text;
                this.Close();
            }
        }

        private void dgvAgentList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RowIndex = ((DataGridView)sender).CurrentRow.Index;
            IsLocationSelected = true;
        }

        private void dgvAgentList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ConsultExtension = dgvAgentList.Rows[e.RowIndex].Cells["Column1"].Value.ToString();
            //MessageBox.Show(ConsultExtension);
            txtExtension.Text = ConsultExtension;
            btnOK_Click(null, null);
        }
    }
}
