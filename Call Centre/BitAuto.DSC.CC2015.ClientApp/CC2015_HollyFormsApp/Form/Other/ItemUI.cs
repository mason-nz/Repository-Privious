using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BitAuto.ISDC.CC2012.Entities;

namespace CC2015_HollyFormsApp
{
    public partial class ItemUI : UserControl
    {
        /// 数据源
        /// <summary>
        /// 数据源
        /// </summary>
        public AgentInfo data = null;
        /// 是否选中
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelect = false;

        public ItemUI()
        {
            InitializeComponent();
            this.Load += new EventHandler(Item_Load);
        }

        private void Item_Load(object sender, EventArgs e)
        {
            lb_name.MaximumSize = new Size(panel.Size.Width, 0);
            lb_name.MinimumSize = new Size(panel.Size.Width, 0);

            lb_date.MaximumSize = new Size(panel.Size.Width, 0);
            lb_date.MinimumSize = new Size(panel.Size.Width, 0);
        }
        /// 设置信息
        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="data"></param>
        public void SetInfo(AgentInfo data)
        {
            this.data = data;
            //客服名称
            lb_name.Text = data.userName;
            //持续时间
            lb_date.Text = CalcDuration();
            //设置背景图
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemUI));
            this.picbox.Image = MongoDBHelper.ImageName(data.CurrStatus);
        }

        /// 设置选中
        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="b"></param>
        public void SetSelect(bool b)
        {
            IsSelect = b;
            this.BackColor = b ? Color.FromArgb(255, 246, 152) : Color.White;
        }
        /// 获取所有控件
        /// <summary>
        /// 获取所有控件
        /// </summary>
        /// <returns></returns>
        private Control[] GetAllControl()
        {
            return new Control[] { this, this.picbox, this.lb_name, this.lb_date };
        }
        /// 增加点击事件
        /// <summary>
        /// 增加点击事件
        /// </summary>
        /// <param name="click"></param>
        public void AddClick(EventHandler click)
        {
            foreach (Control c in GetAllControl())
            {
                c.Click += click;
            }
        }
        /// 添加鼠标抬起事件
        /// <summary>
        /// 添加鼠标抬起事件
        /// </summary>
        /// <param name="mouseup"></param>
        public void AddMouseUp(MouseEventHandler mouseup)
        {
            foreach (Control c in GetAllControl())
            {
                c.MouseUp += mouseup;
            }
        }

        /// 设置提示
        /// <summary>
        /// 设置提示
        /// </summary>
        /// <param name="tooltip"></param>
        public void SetToolTip(ToolTip tooltip)
        {
            foreach (Control c in GetAllControl())
            {
                tooltip.SetToolTip(c, CalcToolTip());
            }
        }
        /// 空格
        /// <summary>
        /// 空格
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        private string KongGe(int k)
        {
            string a = "";
            for (int i = 0; i < k; i++)
            {
                a += " ";
            }
            return a;
        }

        /// 计算持续时间
        /// <summary>
        /// 计算持续时间
        /// </summary>
        /// <returns></returns>
        private string CalcDuration()
        {
            if (data.AgentLive != null)
            {
                DateTime st = data.AgentLive.StartTime_Date;
                DateTime et = Common.GetCurrentTime();
                TimeSpan ts = et - st;
                if (ts.TotalMinutes >= 10)
                {
                    //超过10分钟的标红处理
                    lb_date.ForeColor = Color.Red;
                }
                else
                {
                    lb_date.ForeColor = Color.Black;
                }
                int miao = (int)(ts.TotalSeconds - ((int)ts.TotalMinutes) * 60);
                if (miao < 0) miao = 0;
                string s = ((int)ts.TotalMinutes != 0 ? (int)ts.TotalMinutes + "分" : "") + miao + "秒";
                return s;
            }
            else
            {
                return "";
            }
        }
        /// 计算提示信息
        /// <summary>
        /// 计算提示信息
        /// </summary>
        /// <returns></returns>
        private string CalcToolTip()
        {
            int k = 0;
            string info = "";
            info += KongGe(k) + "工号：" + data.agentDn + "\r\n";
            if (data.CurrStatus != AgentStateForListen.离线)
            {
                info += KongGe(k) + "分机号：" + data.ExtensionNum + "\r\n";
            }
            if (data.CurrStatus == AgentStateForListen.通话中 || data.CurrStatus == AgentStateForListen.振铃)
            {
                info += KongGe(k) + "话务类型：" + data.AgentLive.CallDirectionDesc + "\r\n";
                info += KongGe(k) + "用户号码：" + data.AgentLive.UserPhone + "\r\n";
            }
            info += KongGe(k) + "客服状态：" + data.CurrStatus.ToString() + "\r\n";
            return info;
        }
    }
}
