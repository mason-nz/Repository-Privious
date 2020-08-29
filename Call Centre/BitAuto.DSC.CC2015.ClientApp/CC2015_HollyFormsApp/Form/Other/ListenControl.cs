using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using BitAuto.ISDC.CC2012.Entities;

namespace CC2015_HollyFormsApp
{
    public partial class ListenControl : UserControl
    {
        /// 控件集合
        /// <summary>
        /// 控件集合
        /// </summary>
        public Dictionary<int, ItemUI> UIs = new Dictionary<int, ItemUI>();
        OrderBYForListen orderby = OrderBYForListen.状态;
        public int Frequency = 3;
        public int nowtime = 0;

        //10s回收一次内存
        public int Frequency_gc = 10;
        public int nowtime_gc = 10;
        //选中的工号
        private string select_agentDn = "";

        private Main mianform = null;

        public ListenControl(Main form)
        {
            mianform = form;
            InitializeComponent();
            this.Load += new EventHandler(ListenControl_Load);
        }

        private void ListenControl_Load(object sender, EventArgs e)
        {
            //定时刷新
            timer.Interval = 1 * 1000;
            timer.Start();
            lb_tip.Text = "提示：当前页面每" + Frequency + "秒自动刷新";

            //自动提示
            this.toolTip.AutomaticDelay = 500;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 500;

            //绑定查询数据
            BindData();
            ShowData();

            //注销事件
            this.Disposed += new EventHandler(ListenControl_Disposed);
        }

        #region 界面
        /// 绑定查询界面
        /// <summary>
        /// 绑定查询界面
        /// </summary>
        private void BindData()
        {
            List<KeyValue> groups = MongoDBHelper.GetManageGroupList();
            List<KeyValue> agstate = MongoDBHelper.GetAllHollyAgentState();
            BindComBox(cb_group, groups);
            BindComBox(cb_state, agstate);
            InitOrderByUI();
        }
        /// 绑定下拉控件
        /// <summary>
        /// 绑定下拉控件
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="list"></param>
        private void BindComBox(ComboBox cb, List<KeyValue> list)
        {
            cb.Items.Clear();
            foreach (KeyValue item in list)
            {
                cb.Items.Add(item);
            }
            cb.SelectedIndex = 0;
        }
        /// 排序界面初始化
        /// <summary>
        /// 排序界面初始化
        /// </summary>
        private void InitOrderByUI()
        {
            if (orderby == OrderBYForListen.状态)
            {
                lb_state.LinkColor = Color.Green;
                lb_no.LinkColor = Color.Blue;
            }
            else
            {
                lb_no.LinkColor = Color.Green;
                lb_state.LinkColor = Color.Blue;
            }
        }

        private delegate List<int> GetSelectComBoxHandler(ComboBox cb);
        /// 获取选择的数据
        /// <summary>
        /// 获取选择的数据
        /// </summary>
        /// <param name="cb"></param>
        /// <returns></returns>
        private List<int> GetSelectComBox(ComboBox cb)
        {
            if (this.InvokeRequired)
            {
                return this.Invoke(new GetSelectComBoxHandler(GetSelectComBox), cb) as List<int>;
            }
            else
            {
                List<int> list = new List<int>();

                KeyValue item = cb.SelectedItem as KeyValue;
                if (item == null || item.Key == "-1")
                {
                    foreach (object o in cb.Items)
                    {
                        KeyValue t = o as KeyValue;
                        if (t.Key != "-1")
                            list.Add(CommonFunction.ObjectToInteger(t.Key));
                    }
                }
                else
                {
                    list.Add(CommonFunction.ObjectToInteger(item.Key));
                }
                return list;
            }
        }
        private delegate string GetTextInputHandler();
        /// 获取输入的文本
        /// <summary>
        /// 获取输入的文本
        /// </summary>
        /// <returns></returns>
        private string GetTextInput()
        {
            if (this.InvokeRequired)
            {
                return this.Invoke(new GetTextInputHandler(GetTextInput)).ToString();
            }
            else
            {
                string name = tb_name.Text.Trim().Replace("'", "''");
                return name;
            }
        }
        #endregion

        #region 事件
        /// 清空查询条件
        /// <summary>
        /// 清空查询条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lb_clear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cb_group.SelectedIndex = 0;
            cb_state.SelectedIndex = 0;
            tb_name.Text = "";
        }
        /// 排序切换
        /// <summary>
        /// 排序切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lb_no_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            orderby = OrderBYForListen.工号;
            InitOrderByUI();
            ShowData();
        }
        /// 排序切换
        /// <summary>
        /// 排序切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lb_state_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            orderby = OrderBYForListen.状态;
            InitOrderByUI();
            ShowData();
        }
        /// 查询数据
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            ShowData();
        }
        #endregion

        #region 显示数据
        /// 异步显示数据
        /// <summary>
        /// 异步显示数据
        /// </summary>
        public void ShowData()
        {
            ThreadPool.QueueUserWorkItem(obj =>
            {
                SwitchTimer(false);
                //查询数据
                List<AgentInfo> data = GetHollyData();
                //显示数据
                RefreshUI(data);
                SwitchTimer(true);
            });
        }
        /// 查询数据
        /// <summary>
        /// 查询数据
        /// </summary>
        public List<AgentInfo> GetHollyData()
        {
            List<AgentInfo> allagent = MongoDBHelper.GetAllAgentInfo();
            //获取查询条件
            List<int> select_group = GetSelectComBox(cb_group);
            List<int> select_state = GetSelectComBox(cb_state);
            string name = GetTextInput();
            //查询有效的数据
            List<AgentInfo> data = allagent.Where(x =>
                //所在分组
                select_group.Contains(x.BGID) &&
                    //状态
                select_state.Contains((int)x.CurrStatus) &&
                    //文本框
                (x.agentDn == name || x.userName.Contains(name) || x.ExtensionNum == name) &&
                    //非自己
                x.agentDn != LoginUser.AgentNum).ToList();
            //排序
            if (orderby == OrderBYForListen.工号)
            {
                //客服工号排序
                data = data.OrderBy(x => CommonFunction.ObjectToInteger(x.agentDn) * -1).ToList();
            }
            else if (orderby == OrderBYForListen.状态)
            {
                //状态 工号排序
                data = data.OrderBy(x => MongoDBHelper.SortNumber(x.CurrStatus)).ThenBy(x => CommonFunction.ObjectToInteger(x.agentDn) * -1).ToList();
            }
            return data;
        }
        /// 刷新控件
        /// <summary>
        /// 刷新控件
        /// </summary>
        private void RefreshUI(List<AgentInfo> data)
        {
            if (InvokeRequired)
            {
                this.Invoke(new System.Action<List<AgentInfo>>(RefreshUI), data);
            }
            else
            {
                lb_count.Text = "共" + data.Count + "条";
                int count = Math.Max(UIs.Count, data.Count);
                int start = 999999;
                for (int i = 0; i < count; i++)
                {
                    if (!UIs.ContainsKey(i))
                    {
                        //没有控件，生成控件，加入到集合中
                        ItemUI ui = new ItemUI();
                        UIs[i] = ui;
                        //添加到控件
                        this.flowPanel.Controls.Add(UIs[i]);
                        //增加点击事件
                        ui.AddClick(ui_Click);
                        //增加鼠标事件
                        ui.AddMouseUp(ui_MouseUp);
                    }
                    //判断是否有对应的数据
                    if (i < data.Count)
                    {
                        UIs[i].SetInfo(data[i]);
                        //显示
                        UIs[i].Visible = true;
                        //设置提示
                        UIs[i].SetToolTip(toolTip);
                        //设置选中
                        UIs[i].SetSelect(data[i].agentDn == select_agentDn);
                        //刷新界面
                        Application.DoEvents();
                    }
                    else
                    {
                        start = i;
                        break;
                    }
                }

                for (int k = count - 1; k >= start; k--)
                {
                    //不显示
                    UIs[k].Visible = false;
                    UIs[k].data = null;
                    //刷新界面
                    Application.DoEvents();
                }
            }
        }
        /// 点击事件
        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ui_Click(object sender, EventArgs e)
        {
            ItemUI ui = GetUI(sender);
            if (ui != null)
            {
                foreach (ItemUI item in UIs.Values)
                {
                    item.SetSelect(false);
                }
                ui.SetSelect(true);
                ui.Focus();
                select_agentDn = ui.data.agentDn;
            }
        }
        /// 菜单显示事件
        /// <summary>
        /// 菜单显示事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ui_MouseUp(object sender, MouseEventArgs e)
        {
            ItemUI ui = GetUI(sender);
            if (ui != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    Mi_zhimang.Visible = false;
                    Mi_zhixian.Visible = false;
                    Mi_qianchu.Visible = false;
                    Mi_jianting.Visible = false;
                    Mi_qiangchai.Visible = false;
                    Mi_qiangcha.Visible = false;
                    Mi_jieguan.Visible = false;

                    //---存储数据
                    Mi_zhimang.Tag = ui;
                    Mi_zhixian.Tag = ui;
                    Mi_qianchu.Tag = ui;
                    Mi_jianting.Tag = ui;
                    Mi_qiangchai.Tag = ui;
                    Mi_qiangcha.Tag = ui;
                    Mi_jieguan.Tag = ui;
                    //---存储数据

                    if (ui.data.ExtensionNum == "")
                    {
                        //没有分机号，无法监控
                        return;
                    }
                    if (!ui.data.ExtensionNum.StartsWith(LoginUser.ExtensionNum.Substring(0, 2)))
                    {
                        //不是同号段，无法监控
                        return;
                    }


                    switch (ui.data.CurrStatus)
                    {
                        case AgentStateForListen.置忙:
                            Mi_zhixian.Visible = true;
                            Mi_qianchu.Visible = true;
                            break;
                        case AgentStateForListen.置闲:
                            Mi_zhimang.Visible = true;
                            Mi_qianchu.Visible = true;
                            break;
                        case AgentStateForListen.通话中:
                            Mi_jianting.Visible = true;
                            Mi_qiangchai.Visible = true;
                            Mi_qiangcha.Visible = true;
                            Mi_jieguan.Visible = true;
                            if (Main.Main_PhoneStatus == PhoneStatus.PS04_置忙 || Main.Main_PhoneStatus == PhoneStatus.PS03_置闲)
                            {
                                Mi_jianting.Enabled = true;
                                Mi_qiangchai.Enabled = true;
                                Mi_qiangcha.Enabled = true;
                                Mi_jieguan.Enabled = true;
                            }
                            else
                            {
                                Mi_jianting.Enabled = false;
                                Mi_qiangchai.Enabled = false;
                                Mi_qiangcha.Enabled = false;
                                Mi_jieguan.Enabled = false;
                            }
                            break;
                        case AgentStateForListen.话后:
                            Mi_zhixian.Visible = true;
                            Mi_zhimang.Visible = true;
                            Mi_qianchu.Visible = true;
                            break;
                        case AgentStateForListen.振铃:
                        case AgentStateForListen.离线:
                        default:
                            return;
                    }
                    contextMenuStrip.Show(sender as Control, e.Location);
                }
            }
        }
        /// 获取当前触发事件的控件
        /// <summary>
        /// 获取当前触发事件的控件
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private ItemUI GetUI(object sender)
        {
            if (sender is ItemUI)
            {
                return sender as ItemUI;
            }
            else if (sender is PictureBox)
            {
                return (sender as PictureBox).Parent.Parent as ItemUI;
            }
            else if (sender is Label)
            {
                return (sender as Label).Parent.Parent as ItemUI;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 定时器
        private void ListenControl_Disposed(object sender, EventArgs e)
        {
            SwitchTimer(false);
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (nowtime == 0)
            {
                ShowData();
                nowtime = Frequency;
            }
            else
            {
                nowtime--;
            }
            SetLabelTip();

            if (nowtime_gc == 0)
            {
                nowtime_gc = Frequency_gc;
                GC.Collect();
            }
            else
            {
                nowtime_gc--;
            }
        }
        private void SwitchTimer(bool b)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new System.Action<bool>(SwitchTimer), b);
            }
            else
            {
                if (b)
                {
                    this.timer.Start();
                    nowtime = Frequency;
                    SetLabelTip();
                }
                else
                {
                    this.timer.Stop();
                    nowtime = 0;
                    SetLabelTip();
                }
            }
        }
        private void SetLabelTip()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new System.Action(SetLabelTip));
            }
            else
            {
                if (nowtime == 0)
                {
                    lb_tip.Text = "正在查询数据...";
                }
                else
                {
                    lb_tip.Text = "提示：当前页面" + nowtime + "秒后自动刷新";
                }
                Application.DoEvents();
            }
        }
        #endregion

        #region 菜单
        /// 强制置忙
        /// <summary>
        /// 强制置忙
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mi_zhimang_Click(object sender, EventArgs e)
        {
            ListenOper(sender, HollyContactHelper.Instance.ListenToQZZM, OperForListen.强制置忙);
        }
        /// 强制置闲
        /// <summary>
        /// 强制置闲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mi_zhixian_Click(object sender, EventArgs e)
        {
            ListenOper(sender, HollyContactHelper.Instance.ListenToQZZX, OperForListen.强制置闲);
        }
        /// 强制签出
        /// <summary>
        /// 强制签出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mi_qianchu_Click(object sender, EventArgs e)
        {
            ListenOper(sender, HollyContactHelper.Instance.ListenToQZQC, OperForListen.强制签出);
        }
        /// 监听
        /// <summary>
        /// 监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mi_jianting_Click(object sender, EventArgs e)
        {
            ListenOper(sender, HollyContactHelper.Instance.ListenToJT, OperForListen.监听);
        }
        /// 强拆
        /// <summary>
        /// 强拆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mi_qiangchai_Click(object sender, EventArgs e)
        {
            ListenOper(sender, HollyContactHelper.Instance.ListenToQCai, OperForListen.强拆);
        }
        /// 强插
        /// <summary>
        /// 强插
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mi_qiangcha_Click(object sender, EventArgs e)
        {
            ListenOper(sender, HollyContactHelper.Instance.ListenToQC, OperForListen.强插);
        }
        /// 接管
        /// <summary>
        /// 接管
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mi_jieguan_Click(object sender, EventArgs e)
        {
            ListenOper(sender, HollyContactHelper.Instance.ListenToLJ, OperForListen.接管);
        }

        private delegate bool ListenOperHandler(CC2015_HollyFormsApp.HollyContactHelper.ConDeviceType devicetype, string DestNo);
        /// 具体操作
        /// <summary>
        /// 具体操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="action"></param>
        /// <param name="oper"></param>
        private void ListenOper(object sender, ListenOperHandler action, OperForListen oper)
        {
            ItemUI ui = (sender as ToolStripMenuItem).Tag as ItemUI;
            if (ui != null && ui.data != null && !string.IsNullOrEmpty(ui.data.ExtensionNum) && ui.data.UserID > 0)
            {
                if (action(HollyContactHelper.ConDeviceType.客服工号, ui.data.agentDn))
                {
                    AgentTimeStateHelper.Instance.InsertListenAgentLog(oper, ui.data.UserID, ui.data.userName, ui.data.agentDn, ui.data.ExtensionNum, ui.data.CurrStatus);
                    switch (oper)
                    {
                        case OperForListen.强制签出:
                        case OperForListen.强制置忙:
                        case OperForListen.强制置闲:
                        case OperForListen.强拆:
                            {
                                ui.data.LockAgentLive(oper);
                                ShowData();
                            }
                            break;
                        case OperForListen.强插:
                            mianform.SetMainStatus(PhoneStatus.PS21_强插振铃);
                            break;
                        case OperForListen.监听:
                            mianform.SetMainStatus(PhoneStatus.PS19_监听振铃);
                            break;
                        case OperForListen.接管:
                            Main.Main_ConMonitor_Calltype = ui.data.AgentLive.CallDirection_Calltype;
                            break;
                    }
                }
                else
                {
                    MessageBox.Show(oper.ToString() + "失败！");
                }
            }
        }
        #endregion
    }
}
