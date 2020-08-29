using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CC2015_HollyFormsApp
{
    public partial class UpdateTip : Form
    {
        /// 是否已经显示
        /// <summary>
        /// 是否已经显示
        /// </summary>
        public static bool HasShow = false;
        /// 显示的时间
        /// <summary>
        /// 显示的时间
        /// </summary>
        public static DateTime ShowStart = new DateTime();
        /// 单例
        /// <summary>
        /// 单例
        /// </summary>
        public static UpdateTip Instance = null;

        //System.Threading.Timer timer = null;

        public UpdateTip()
        {
            Instance = this;
            HasShow = true;
            InitializeComponent();
            this.Load += new EventHandler(UpdateTip_Load);
            this.FormClosing += new FormClosingEventHandler(UpdateTip_FormClosing);

            //自动关闭机制
            //timer = new System.Threading.Timer((t) => { DefineClose(); }, null, 1000 * 10, 1000 * 12);
        }

        /// 自定义关闭
        /// <summary>
        /// 自定义关闭
        /// </summary>
        private void DefineClose()
        {
            if (HasShow == false)
            {
                return;
            }
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(DefineClose));
            }
            else
            {
                //只显示一次
                //HasShow = false;
                //timer.Dispose();
                this.Close();
                GC.Collect();
            }
        }
        /// 自定义重启
        /// <summary>
        /// 自定义重启
        /// </summary>
        private void DefineReStart()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(DefineReStart));
            }
            else
            {
                Common.Restart();
            }
        }

        private void UpdateTip_Load(object sender, EventArgs e)
        {
            if (this.StartPosition == FormStartPosition.Manual)
            {
                int x = Screen.PrimaryScreen.WorkingArea.Right - this.Width;
                int y = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height;
                this.Location = new Point(x, y);//设置窗体在屏幕右下角显示  
                AnimateWindow(this.Handle, 1000, AW_SLIDE | AW_ACTIVE | AW_VER_NEGATIVE);
            }
        }

        private void UpdateTip_FormClosing(object sender, FormClosingEventArgs e)
        {
            //AnimateWindow(this.Handle, 1000, AW_BLEND | AW_HIDE);
            UpdateTip.Instance = null;
        }

        private void panelclose_Click(object sender, EventArgs e)
        {
            DefineClose();
        }

        #region 动画
        /// 窗体动画函数    注意：要引用System.Runtime.InteropServices;  
        /// <summary>  
        /// 窗体动画函数    注意：要引用System.Runtime.InteropServices;  
        /// </summary>  
        /// <param name="hwnd">指定产生动画的窗口的句柄</param>  
        /// <param name="dwTime">指定动画持续的时间</param>  
        /// <param name="dwFlags">指定动画类型，可以是一个或多个标志的组合。</param>  
        /// <returns></returns>
        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        //下面是可用的常量，根据不同的动画效果声明自己需要的  
        private const int AW_HOR_POSITIVE = 0x0001;//自左向右显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志  
        private const int AW_HOR_NEGATIVE = 0x0002;//自右向左显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志  
        private const int AW_VER_POSITIVE = 0x0004;//自顶向下显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志  
        private const int AW_VER_NEGATIVE = 0x0008;//自下向上显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志该标志  
        private const int AW_CENTER = 0x0010;//若使用了AW_HIDE标志，则使窗口向内重叠；否则向外扩展  
        private const int AW_HIDE = 0x10000;//隐藏窗口  
        private const int AW_ACTIVE = 0x20000;//激活窗口，在使用了AW_HIDE标志后不要使用这个标志  
        private const int AW_SLIDE = 0x40000;//使用滑动类型动画效果，默认为滚动动画类型，当使用AW_CENTER标志时，这个标志就被忽略  
        private const int AW_BLEND = 0x80000;//使用淡入淡出效果
        #endregion

        #region 标题可拖动
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF012, 0);
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF012, 0);
        }
        #endregion

        private void BtnRe_Click(object sender, EventArgs e)
        {
            DefineReStart();
        }

        private void BtnTc_Click(object sender, EventArgs e)
        {
            DefineClose();
        }
    }
}
