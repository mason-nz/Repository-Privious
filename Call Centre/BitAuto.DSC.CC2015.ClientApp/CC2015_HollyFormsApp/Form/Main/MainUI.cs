using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using mshtml;
using System.Web;
using System.Configuration;
using System.Threading;
using System.Collections.Generic;
using BitAuto.ISDC.CC2012.Entities;

namespace CC2015_HollyFormsApp
{
    /// UI实现
    /// <summary>
    /// UI实现
    /// </summary>
    public partial class Main : Form
    {
        #region 变量
        //TabPage每页标题显示字符数
        private int TITLE_COUNT = 8;
        //主页
        private string DefaultURL = ConfigurationManager.AppSettings["DefaultURL"].ToString();
        //关闭图片大小
        private const int CLOSE_SIZE = 12;
        //tabControl中关闭按钮的图标
        private Bitmap image = new Bitmap(System.AppDomain.CurrentDomain.BaseDirectory + "images\\closeGray.png");
        //客户端下方，电话通话时长
        private int CallRecordLength = 0;
        //是否忽略脚本错误
        private bool IsScriptErrorsSuppressed = false;
        /// 最大时间倒计时
        /// <summary>
        /// 最大时间倒计时
        /// </summary>
        private int AfterTime = 0;
        //左上角标题
        private string LabelTitle = "";
        private Color LabelColor = Color.Black;
        #endregion

        /// UI初始化
        /// <summary>
        /// UI初始化
        /// </summary>
        private void InitUI()
        {
            GC.KeepAlive(timer_jishi);
            #region SSK初始化样式文件
            this.skinEngine1.DisableTag = 9999;
            this.tabControl1.Tag = 9999;
            this.skinEngine1.SkinFile = "skin/" + Common.GetSkinFileName();
            #endregion

            #region TabControl右侧添加关闭图片
            this.tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Padding = new System.Drawing.Point(5, 5);
            this.tabControl1.DrawItem += new DrawItemEventHandler(this.MainTabControl_DrawItem);
            this.tabControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainTabControl_MouseDown);
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            #endregion

            #region 绑定异常事件处理
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionFunction);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            #endregion
        }
        /// load方法
        /// <summary>
        /// load方法
        /// </summary>
        private void InitUI_Load()
        {
            this.Text = string.Format("易车客服中心管理系统（Holly）" + Common.GetVersion() + "                            " +
                "当前登录客服分机：{0}    工号：{3}    登录姓名：{1}    所在部门：{2}    登录区域：{4}",
                LoginUser.ExtensionNum,
                LoginUser.TrueName,
                LoginUser.Department == null ? string.Empty : LoginUser.Department.Name,
                LoginUser.AgentNum,
                LoginUser.LoginAreaType.ToString()
                );
            InitMainForm();
            ChangeBtnStyle();
        }
        #region 程序关闭按钮处理事件
        private int WM_SYSCOMMAND = 0x112;
        private long SC_CLOSE = 0xF060;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                if (m.WParam.ToInt64() == SC_CLOSE)
                {
                    if (LoginHelper.Instance.ExitLogin())
                    {
                        System.Environment.Exit(0);
                    }
                    return;
                }
            }
            base.WndProc(ref m);
        }
        #endregion

        #region 浏览器事件
        /// 创建浏览器
        /// <summary>
        /// 创建浏览器
        /// </summary>
        /// <returns></returns>
        private WebBrowser CreateWebBrower()
        {
            WebBrowser tempBrowser = new WebBrowser();
            tempBrowser.Dock = DockStyle.Fill;
            tempBrowser.ObjectForScripting = this;
            tempBrowser.ScriptErrorsSuppressed = IsScriptErrorsSuppressed;

            //为临时浏览器关联NewWindow等事件
            tempBrowser.NewWindow += new CancelEventHandler(tempBrowser_NewWindow);
            tempBrowser.Navigated += new WebBrowserNavigatedEventHandler(tempBrowser_Navigated);
            tempBrowser.ProgressChanged += new WebBrowserProgressChangedEventHandler(tempBrowser_ProgressChanged);
            tempBrowser.StatusTextChanged += new EventHandler(tempBrowser_StatusTextChanged);
            tempBrowser.Navigating += new WebBrowserNavigatingEventHandler(tempBrowser_Navigating);
            tempBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(tempBrowser_DocumentCompleted);

            return tempBrowser;
        }
        /// 导航前事件
        /// <summary>
        /// 导航前事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tempBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            WebBrowser mybrowser = (WebBrowser)sender;
            TabPage mypage = (TabPage)mybrowser.Parent;
            string url = e.Url.ToString();
            //排除非页面和导出页面 《不确定》 强斐 2015-11-18
            if (IsNewWin && url.StartsWith("http://"))
            {
                string lasturl = "";
                if (e.Url.Segments.Length > 0)
                {
                    lasturl = e.Url.Segments[e.Url.Segments.Length - 1].ToLower();
                    if (!lasturl.Contains("export") && lasturl.EndsWith(".aspx"))
                    {
                        NewPageForURL(url);
                        e.Cancel = true;
                    }
                }
            }
        }

        /// 临时浏览器进度变化事件
        /// <summary>
        /// 临时浏览器进度变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tempBrowser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            //toolStripProgressBar1.Maximum = 100;
            //int max = (int)e.MaximumProgress;
            //int c = (int)e.CurrentProgress;

            //if (max == c)
            //{
            //    toolStripProgressBar1.Value = 100;
            //}
            //else
            //{
            //    int s = max / 100;
            //    toolStripProgressBar1.Value = c / s;
            //}
        }
        /// 临时浏览器状态变化事件
        /// <summary>
        /// 临时浏览器状态变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tempBrowser_StatusTextChanged(object sender, EventArgs e)
        {
            WebBrowser myBrowser = (WebBrowser)sender;
            TabPage mypage = (TabPage)myBrowser.Parent;
            //设置临时浏览器所在tab标题
            mypage.Text = newstring(myBrowser.DocumentTitle);
            mypage.ToolTipText = mypage.Text;
            if (myBrowser != GetCurrentBrowser())
            {
                return;
            }
            else
            {
                toolStripStatusUrl.Text = myBrowser.StatusText;
                if (string.IsNullOrEmpty(toolStripStatusUrl.Text) || toolStripStatusUrl.Text == "完成")
                {
                    toolStripStatusUrl.Text = mypage.Name;
                }
            }
        }
        /// 临时浏览器产生新窗体事件
        /// <summary>
        /// 临时浏览器产生新窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tempBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            //获取触发tempBrowser_NewWindow事件的浏览器
            WebBrowser myBrowser = (WebBrowser)sender;
            //获取触发tempBrowser_NewWindow事件的浏览器所在TabPage
            TabPage mypage = (TabPage)myBrowser.Parent;
            //通过StatusText属性获得新的url
            string NewURL = ((WebBrowser)sender).StatusText;

            if (NewURL == "完成")
            {
                IHTMLWindow2 win = (IHTMLWindow2)myBrowser.Document.Window.DomWindow;
                win.execScript("try{ document.getElementById('" + myBrowser.Document.ActiveElement.Id + "').click();}catch(e){}", "Javascript");
            }
            else
            {
                NewPageForURL(NewURL);
                //使外部无法捕获此事件
                e.Cancel = true;
            }
        }
        /// 临时浏览器定向完毕
        /// <summary>
        /// 临时浏览器定向完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tempBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            WebBrowser mybrowser = (WebBrowser)sender;
            TabPage mypage = (TabPage)mybrowser.Parent;
            //设置临时浏览器所在tab标题
            mypage.Text = newstring(mybrowser.DocumentTitle);
            mypage.ToolTipText = mypage.Text;
        }
        /// 文档加载完成
        /// <summary>
        /// 文档加载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tempBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser mybrowser = (WebBrowser)sender;
            if (mybrowser.ReadyState < WebBrowserReadyState.Complete)
            {
                return;
            }

            TabPage mypage = (TabPage)mybrowser.Parent;
            //通知网页设置window.name
            IHTMLWindow2 win = (IHTMLWindow2)mybrowser.Document.Window.DomWindow;
            if (win != null)
            {
                SendToWeb(new NoticeData("SaveWindowsName", mypage.Name).ToString(), mypage.Name);
            }
        }

        private delegate IHTMLWindow2 GetCurrentIHTMLWindow2Handler(string webpageid);
        /// 根据ID获取指定的窗口
        /// <summary>
        /// 根据ID获取指定的窗口
        /// </summary>
        /// <returns>当前浏览器</returns>
        private IHTMLWindow2 GetIHTMLWindow2ByWebPageID(string webpageid)
        {
            if (InvokeRequired)
            {
                return (IHTMLWindow2)Invoke(new GetCurrentIHTMLWindow2Handler(GetIHTMLWindow2ByWebPageID));
            }
            else
            {
                IHTMLWindow2 win;
                if (string.IsNullOrEmpty(webpageid))
                {
                    win = (IHTMLWindow2)GetCurrentBrowser().Document.Window.DomWindow;
                    Loger.Log4Net.Info("[MainUI] [SendMesToWeb] 获取当前默认标签页");
                    return win;
                }
                else
                {
                    WebBrowser a = GetWebBrowserByWebPageID(webpageid);
                    //Loger.Log4Net.Info("[MainUI] [SendMesToWeb] GetWebBrowserByWebPageID 值是否为空 " + (a == null));
                    if (a != null)
                    {
                        //Loger.Log4Net.Info("[MainUI] [SendMesToWeb] Document 值是否为空 " + (a.Document == null));
                        if (a.Document != null)
                        {
                            //Loger.Log4Net.Info("[MainUI] [SendMesToWeb] Window 值是否为空 " + (a.Document.Window == null));
                            if (a.Document.Window != null)
                            {
                                //Loger.Log4Net.Info("[MainUI] [SendMesToWeb] DomWindow 值是否为空 " + (a.Document.Window.DomWindow == null));
                                if (a.Document.Window.DomWindow != null)
                                {
                                    win = (IHTMLWindow2)a.Document.Window.DomWindow;
                                    Loger.Log4Net.Info("[MainUI] [SendMesToWeb] 获取到了当前指定webPageID的页面：" + webpageid);
                                    return win;
                                }
                            }
                        }
                    }
                    win = (IHTMLWindow2)GetCurrentBrowser().Document.Window.DomWindow;
                    Loger.Log4Net.Info("[MainUI] [SendMesToWeb] 获取不到当前指定webPageID的页面：" + webpageid + "异常**错误**，使用默认页面代替！");
                    return win;
                }
            }
        }
        /// 根据ID获取指定的浏览器
        /// <summary>
        /// 根据ID获取指定的浏览器
        /// </summary>
        /// <returns>当前浏览器</returns>
        private WebBrowser GetWebBrowserByWebPageID(string webpageid)
        {
            TabPage outBoundTP = GetTabPageByWebPageID(webpageid);
            if (outBoundTP == null)
            {
                Loger.Log4Net.Info("[MainUI] [getCurrentOutBoundBrowser] TabPages中找不到 " + webpageid);
                return null;
            }
            if (outBoundTP.Controls.Count == 0)
            {
                Loger.Log4Net.Info("[MainUI] [getCurrentOutBoundBrowser] TabPage.Controls.Count == 0 " + webpageid);
                return null;
            }
            WebBrowser currentBrowser = (WebBrowser)outBoundTP.Controls[0];
            return currentBrowser;
        }
        /// 根据ID获取指定的页签控件
        /// <summary>
        /// 根据ID获取指定的页签控件
        /// </summary>
        /// <param name="webpageid"></param>
        /// <returns></returns>
        private TabPage GetTabPageByWebPageID(string webpageid)
        {
            TabPage outBoundTP = null;
            foreach (TabPage tp in tabControl1.TabPages)
            {
                if (tp.Name.Equals(webpageid))
                {
                    outBoundTP = tp;
                    break;
                }
            }
            return outBoundTP;
        }

        /// 获取当前浏览器
        /// <summary>
        /// 获取当前浏览器
        /// </summary>
        /// <returns>当前浏览器</returns>
        private WebBrowser GetCurrentBrowser()
        {
            WebBrowser currentBrowser = null;
            try
            {
                //先取当前页
                currentBrowser = tabControl1.SelectedTab.Controls[0] as WebBrowser;
                if (currentBrowser == null)
                {
                    //没有的话取最后一页
                    //只有一页的时候取第一页
                    TabPage lastp = null;
                    foreach (TabPage t in tabControl1.TabPages)
                    {
                        if (!WinformPages.Contains(t.Name))
                        {
                            lastp = t;
                        }
                    }
                    currentBrowser = lastp.Controls[0] as WebBrowser;
                }
            }
            catch { }
            return currentBrowser;
        }
        /// 新建一页并定向到指定url
        /// <summary>
        /// 新建一页并定向到指定url
        /// </summary>
        /// <param name="address">新一页的浏览器重新定向到的url</param>
        /// <param name="isCalling">是否通话中</param>
        private void NewPageForReq(string address, bool isCalling)
        {
            Loger.Log4Net.Info("[MainUI] [NewPageForReq] address is:" + address);
            TabPage mypage = new TabPage();
            mypage.Name = BusinessProcess.GetWebPageID();
            if (isCalling)
            {
                CurrentOutBoundTabPageName = mypage.Name;
                Loger.Log4Net.Info("[MainUI] [CurrentOutBoundTabPageName] 呼入通话网页名称=新建：" + CurrentOutBoundTabPageName);
            }
            mypage.Tag = GetParentName();
            WebBrowser tempBrowser = CreateWebBrower();
            mypage.Controls.Add(tempBrowser);
            tabControl1.TabPages.Add(mypage);
            ShowTabPagesLog();
            tabControl1.SelectedTab = mypage;
            Navigate(tempBrowser, address);
        }
        /// 取当前页面名称
        /// <summary>
        /// 取当前页面名称
        /// </summary>
        /// <returns></returns>
        public string GetParentName()
        {
            WebBrowser currentBrowser = GetCurrentBrowser();
            if (currentBrowser != null && currentBrowser.Parent != null)
            {
                TabPage tp = currentBrowser.Parent as TabPage;
                if (tp != null)
                {
                    return tp.Name;
                }
            }
            return "";
        }
        /// 显示TabPages日志
        /// <summary>
        /// 显示TabPages日志
        /// </summary>
        private void ShowTabPagesLog()
        {
            string str = "";
            foreach (TabPage tp in tabControl1.TabPages)
            {
                str += (tp.Text.Trim() == "" ? "新增页" : tp.Text.Trim()) + "=" + tp.Name + "(" + CommonFunction.ObjectToString(tp.Tag) + "); ";
            }
            Loger.Log4Net.Info("[MainUI] [ShowTabPagesLog] 全部页面 " + str);
        }
        /// 点击链接打开新窗口
        /// <summary>
        /// 点击链接打开新窗口
        /// </summary>
        /// <param name="NewURL"></param>
        private void NewPageForURL(string NewURL)
        {
            //生成新的一页
            TabPage TabPageTemp = new TabPage();
            TabPageTemp.Name = BusinessProcess.GetWebPageID();
            TabPageTemp.Tag = GetParentName();
            //生成新的tempBrowser
            WebBrowser tempBrowser = CreateWebBrower();
            //将临时浏览器添加到临时TabPage中
            TabPageTemp.Controls.Add(tempBrowser);
            //将临时TabPage添加到主窗体中
            this.tabControl1.TabPages.Add(TabPageTemp);
            ShowTabPagesLog();
            tabControl1.SelectedTab = TabPageTemp;
            Application.DoEvents();

            ThreadPool.QueueUserWorkItem(state =>
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<WebBrowser, string>(Navigate), tempBrowser, NewURL);
                }
                else
                {
                    Navigate(tempBrowser, NewURL);
                }
            });
        }
        /// 导航
        /// <summary>
        /// 导航
        /// </summary>
        /// <param name="tempBrowser"></param>
        /// <param name="address"></param>
        private void Navigate(WebBrowser tempBrowser, string address)
        {
            //代码导航时，不触发Navigating
            tempBrowser.Navigating -= new WebBrowserNavigatingEventHandler(tempBrowser_Navigating);
            //取消代理
            RefreshIESettings("");
            if (address.StartsWith("http://"))
            {
                tempBrowser.Navigate(Common.getUrl(address));
            }
            else
            {
                Uri u = new Uri(DefaultURL);
                tempBrowser.Navigate(Common.getUrl("http://" + u.Host + address));
            }
            tempBrowser.Navigating += new WebBrowserNavigatingEventHandler(tempBrowser_Navigating);
        }
        /// 截取字符串为指定长度
        /// <summary>
        /// 截取字符串为指定长度
        /// </summary>
        /// <param name="oldstring"></param>
        /// <returns></returns>
        private string newstring(string oldstring)
        {
            string temp;
            if (oldstring.Length < TITLE_COUNT)
            {
                temp = oldstring;
            }
            else
            {
                temp = oldstring.Substring(0, TITLE_COUNT);
            }
            return temp + "   ";
        }
        #endregion

        #region TabControl事件
        /// 切换tab
        /// <summary>
        /// 切换tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Controls[0] is WebBrowser)
            {
                WebBrowser mybor = (WebBrowser)tabControl1.SelectedTab.Controls[0];
                if (mybor.Url != null)
                {
                    //地址输入框
                    tabControl1.SelectedTab.Text = newstring(mybor.DocumentTitle);
                    Loger.Log4Net.Info("[MainUI] [SelectedIndexChanged] [切换页面] " + this.tabControl1.SelectedTab.Name);
                    toolStripStatusUrl.Text = tabControl1.SelectedTab.Name;
                }
                else
                {
                    tabControl1.SelectedTab.Text = "";
                }
            }
            else
            {
                toolStripStatusUrl.Text = tabControl1.SelectedTab.Name;
            }
        }
        private void MainTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                Font t = null;
                SolidBrush bruFont = null;
                //设置通话样式
                TabPage tp = tabControl1.TabPages[e.Index];
                if (tp.Name == CurrentOutBoundTabPageName)
                {
                    t = new System.Drawing.Font(new FontFamily("微软雅黑"), 9, FontStyle.Bold);
                    bruFont = new SolidBrush(Color.Blue);
                }
                else
                {
                    t = new System.Drawing.Font(new FontFamily("微软雅黑"), 9, FontStyle.Regular);
                    bruFont = new SolidBrush(Color.Black);
                }

                Rectangle myTabRect = this.tabControl1.GetTabRect(e.Index);
                //先添加TabPage属性
                e.Graphics.DrawString(this.tabControl1.TabPages[e.Index].Text, t, bruFont, myTabRect.X + 2, myTabRect.Y + 2);
                //再画一个矩形框   
                using (Pen p = new Pen(Color.Transparent))
                {
                    myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                    myTabRect.Width = CLOSE_SIZE;
                    myTabRect.Height = CLOSE_SIZE;
                    e.Graphics.DrawRectangle(p, myTabRect);
                }
                //填充矩形框
                Color recColor = e.State == DrawItemState.Selected ? Color.Transparent : Color.Transparent;
                using (Brush b = new SolidBrush(recColor))
                {
                    e.Graphics.FillRectangle(b, myTabRect);
                }
                //画关闭符号
                using (Pen objpen = new Pen(Color.Gray))
                {
                    //使用图片  
                    Bitmap bt = new Bitmap(image);
                    Point p5 = new Point(myTabRect.X, 4);
                    e.Graphics.DrawImage(bt, p5);
                }
                e.Graphics.Dispose();
            }
            catch
            {
            }
        }
        /// 手动关闭当前页面
        /// <summary>
        /// 手动关闭当前页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainTabControl_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    int x = e.X, y = e.Y;
                    //计算关闭区域      
                    Rectangle myTabRect = this.tabControl1.GetTabRect(this.tabControl1.SelectedIndex);
                    myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                    myTabRect.Width = CLOSE_SIZE;
                    myTabRect.Height = CLOSE_SIZE;
                    //如果鼠标在区域内就关闭选项卡      
                    bool isClose = x > myTabRect.X && x < myTabRect.Right
                     && y > myTabRect.Y && y < myTabRect.Bottom;

                    if (isClose == true && !IsCurrentWindowIsCallingTel())
                    {
                        //仅仅剩下一个tab时返回
                        if (GetTabControlTabPagesCount() <= 1)
                        {
                            //取消代理
                            RefreshIESettings("");
                            GetCurrentBrowser().Navigate(DefaultURL);
                        }
                        else
                        {
                            if (WinformPages.Contains(this.tabControl1.SelectedTab.Name))
                            {
                                //关闭winform页面
                            }
                            else
                            {
                                WebBrowser mybor = GetCurrentBrowser();
                                //释放资源
                                mybor.Dispose();
                                mybor.Controls.Clear();
                            }
                            TabPage page = this.tabControl1.SelectedTab;
                            this.tabControl1.TabPages.Remove(page);
                            bool flag = alowReceiveCallMsg_WebpageIDList.Remove(page.Name);
                            Loger.Log4Net.Info("[MainUI] 手动关闭页面=" + page.Name + "，并且移除alowReceiveCallMsg_WebpageIDList[允许给iframe子页面推送话务消息列表]中的PageID：" + page.Name + ",结果=" + flag);
                            ShowTabPagesLog();
                            page.Dispose();
                            GC.Collect();
                        }
                    }
                    this.GetCurrentBrowser().Document.Window.Focus();
                    IHTMLWindow2 win = (IHTMLWindow2)this.GetCurrentBrowser().Document.Window.DomWindow;
                    win.focus();
                }
            }
            catch
            {
            }
        }
        /// web端关闭当前tab
        /// <summary>
        /// web端关闭当前tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebClosePageEvent(string webpageid)
        {
            if (webpageid == CurrentOutBoundTabPageName)
            {
                Loger.Log4Net.Info("[MainUI] [WebClosePageEvent] 当前窗口有电话正在通话，无法关闭=" + webpageid);
                return;
            }
            //仅仅剩下一个tab时返回
            if (GetTabControlTabPagesCount() <= 1)
            {
                //取消代理
                RefreshIESettings("");
                GetCurrentBrowser().Navigate(DefaultURL);
            }
            else
            {
                if (WinformPages.Contains(webpageid))
                {
                    //关闭winform页面
                    //无操作
                }
                else
                {
                    WebBrowser mybor = GetWebBrowserByWebPageID(webpageid);
                    //释放资源
                    mybor.Dispose();
                    mybor.Controls.Clear();
                }

                TabPage page = GetTabPageByWebPageID(webpageid);
                this.tabControl1.TabPages.Remove(page);
                page.Dispose();
                bool flag = alowReceiveCallMsg_WebpageIDList.Remove(page.Name);
                Loger.Log4Net.Info("[MainUI] [WebClosePageEvent] 网站关闭页面=" + webpageid + "，并且移除alowReceiveCallMsg_WebpageIDList[允许给iframe子页面推送话务消息列表]中的PageID：" + page.Name + ",结果=" + flag);
                ShowTabPagesLog();
            }
        }
        /// 判断是否有电话正在通话
        /// <summary>
        /// 判断是否有电话正在通话
        /// </summary>
        /// <returns>有，返回True，否决返回False</returns>
        private bool IsCurrentWindowIsCallingTel()
        {
            if (!string.IsNullOrEmpty(CurrentOutBoundTabPageName))
            {
                Loger.Log4Net.Info("[MainUI] [IsCurrentWindowIsCallingTel] 手动关闭前判断是否可以关闭 当前=" + tabControl1.SelectedTab.Name + " 通话=" + CurrentOutBoundTabPageName);
                if (tabControl1.SelectedTab.Name.Equals(CurrentOutBoundTabPageName))
                {
                    if ((int)Main_PhoneStatus <= (int)PhoneStatus.PS06_话后)
                    {
                        //合力状态和客户端状态不一致
                        //提示用户是否可以关闭
                        if (MessageBox.Show("如果在通话中，请不要关闭当前窗口！是否确认关闭？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == System.Windows.Forms.DialogResult.Yes)
                        {
                            //坐席确认，没有通话，可以关闭
                            CurrentOutBoundTabPageName = "";
                            Loger.Log4Net.Info("[MainUI] [IsCurrentWindowIsCallingTel] 坐席确认，没有通话，可以关闭，手动清空CurrentOutBoundTabPageName");
                            return false;
                        }
                        else
                        {
                            //坐席确认，有通话，不能关闭
                            return true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("当前窗口有电话正在通话，请勿关闭！");
                        Loger.Log4Net.Info("[MainUI] [IsCurrentWindowIsCallingTel] 当前窗口有电话正在通话，无法关闭=" + CurrentOutBoundTabPageName);
                        return true;
                    }
                }
            }
            return false;
        }
        private int GetTabControlTabPagesCount()
        {
            int a = tabControl1.TabPages.Count;
            //排除winform页面
            foreach (TabPage t in tabControl1.TabPages)
            {
                if (WinformPages.Contains(t.Name))
                {
                    //当前页面中包含监控页面，所以可用数量-1
                    a = a - 1;
                }
            }
            Loger.Log4Net.Info("[MainUI] [GetTabControlTabPagesCount] 可用页面数量=" + a);
            return a;
        }
        #endregion

        #region 异常处理
        static void UnhandledExceptionFunction(Object sender, UnhandledExceptionEventArgs args)
        {
            Loger.Log4Net.Info("[@@@MainUI@@@] UnhandledExceptionFunction begin...");
            Exception e = (Exception)args.ExceptionObject;
            LoginHelper.Instance.ExceptionUpdateAgentState(e);
            Loger.Log4Net.Info("[@@@MainUI@@@] UnhandledExceptionFunction end");
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs args)
        {
            Loger.Log4Net.Info("[@@@MainUI@@@] Application_ThreadException begin...");
            LoginHelper.Instance.ExceptionUpdateAgentState(args.Exception);
            Loger.Log4Net.Info("[@@@MainUI@@@] Application_ThreadException end");
        }
        #endregion

        #region 其他
        /// 修改工具栏按钮样式
        /// <summary>
        /// 修改工具栏按钮样式
        /// </summary>
        private void ChangeBtnStyle()
        {
            int btnCount = this.toolStripAgentTool.Items.Count;
            for (int i = 0; i < btnCount; i++)
            {
                if (this.toolStripAgentTool.Items[i].GetType() == typeof(ToolStripButton) ||
                     this.toolStripAgentTool.Items[i].GetType() == typeof(System.Windows.Forms.ToolStripSplitButton))
                {
                    this.toolStripAgentTool.Items[i].AutoSize = false;
                    this.toolStripAgentTool.Items[i].Size = new Size(60, 60);
                    this.toolStripAgentTool.Items[i].Font = new Font("微软雅黑", (float)10.5);
                    this.toolStripAgentTool.Items[i].ForeColor = Color.FromArgb(11, 107, 178);
                    this.toolStripAgentTool.Items[i].Margin = new Padding(0, 10, 0, 2);
                }
            }
        }
        /// 初始化浏览器
        /// <summary>
        /// 初始化浏览器
        /// </summary>
        private void InitMainForm()
        {
            TabPage mypage = new TabPage();
            mypage.Name = BusinessProcess.GetWebPageID();
            WebBrowser tempBrowser = CreateWebBrower();
            //取消代理
            RefreshIESettings("");
            string url = "/?DomainAccount=" + LoginUser.DomainAccount + "&Password=" + HttpUtility.UrlEncode(LoginUser.Password);
            tempBrowser.Navigate(DefaultURL + url);
            mypage.Controls.Add(tempBrowser);
            tabControl1.TabPages.Add(mypage);
            CheckHasListen();
        }
        /// 错误弹出窗口
        /// <summary>
        /// 错误弹出窗口
        /// </summary>
        /// <param name="msg"></param>
        private void PopErrorLayer(string msg)
        {
            ErrorForm ef = new ErrorForm();
            ef.Msg = msg + "\r\n\r\n您确定要退出系统吗？";
            if (ef.ShowDialog(this) == DialogResult.OK)
            {
                //退出系统               
                if (LoginHelper.Instance.ExitLogin())
                {
                    System.Environment.Exit(0);
                }
            }
            else
            {
            }
            ef.Dispose();
        }
        #endregion

        #region 状态栏设置接口
        /// 开始计时
        /// <summary>
        /// 开始计时
        /// </summary>
        public void StartTimeForCall()
        {
            CallRecordLength = 0;
            toolStripStatusTime.Text = CallRecordLength.ToString() + " 秒";
            timer_jishi.Enabled = true;
        }
        /// 停止计时
        /// <summary>
        /// 停止计时
        /// </summary>
        public void EndTimeForCall()
        {
            timer_jishi.Enabled = false;
        }
        ///  获取计时器状态
        /// <summary>
        ///  获取计时器状态
        /// </summary>
        /// <returns></returns>
        public bool GetTimeForCall()
        {
            return timer_jishi.Enabled;
        }
        /// 重置通话时长
        /// <summary>
        /// 重置通话时长
        /// </summary>
        public void RestTimeForCall()
        {
            toolStripStatusTime.Text = "0 秒";
        }
        /// 设置主叫号码
        /// <summary>
        /// 设置主叫号码
        /// </summary>
        /// <param name="tel"></param>
        public void SetZhujiaoDn(string tel)
        {
            toolStripStatusZhujiao.Text = tel;
        }
        /// 设置被叫号码
        /// <summary>
        /// 设置被叫号码
        /// </summary>
        /// <param name="tel"></param>
        public void SetBeijiaoDn(string tel)
        {
            toolStripStatusBeijiao.Text = tel;
        }
        /// 设置厂家状态
        /// <summary>
        /// 设置厂家状态
        /// </summary>
        /// <param name="cti"></param>
        public void SetCTIStatus(string cti)
        {
            toolStripStatusText.Text = cti;
        }
        /// 设置落地号码
        /// <summary>
        /// 设置落地号码
        /// </summary>
        /// <param name="num"></param>
        public void SetLuodiNum(string num, string skillname)
        {
            if (skillname != "")
            {
                this.toolStripStatusLDH.Text = num + "（" + skillname + "）";
            }
            else
            {
                this.toolStripStatusLDH.Text = num;
            }
        }
        #endregion

        #region 监控
        /// 校验监控权限
        /// <summary>
        /// 校验监控权限
        /// </summary>
        private void CheckHasListen()
        {
            try
            {
                Loger.Log4Net.Info("[InitMainForm] 开始验证监控权限");
                string sysID = ConfigurationManager.AppSettings.Get("ThisSysID");
                if (!AgentTimeStateHelper.Instance.IsCanListenAgent(Convert.ToInt32(LoginUser.UserID), sysID))
                {
                    Loger.Log4Net.Info("[InitMainForm] 无 监控权限");
                    this.toolSbtnAgentListen.Visible = false;
                }
                else
                {
                    Loger.Log4Net.Info("[InitMainForm] 有 监控权限");
                    this.toolSbtnAgentListen.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[InitMainForm] 验证监控权限出错：" + ex.StackTrace);
            }
        }
        /// 监控窗口
        /// <summary>
        /// 监控窗口
        /// </summary>
        private void AddListenTabPage()
        {
            bool hasopen = false;
            TabPage p = null;
            foreach (TabPage t in tabControl1.TabPages)
            {
                if (t.Name == "ListenTab")
                {
                    hasopen = true;
                    p = t;
                    break;
                }
            }
            if (hasopen == false)
            {
                TabPage mypage = new TabPage();
                mypage.AutoScroll = true;
                mypage.Text = newstring("监控页面");
                mypage.ToolTipText = mypage.Text;
                mypage.Name = "ListenTab";
                ListenControl control = new ListenControl(this);
                control.Dock = DockStyle.Fill;
                mypage.Controls.Add(control);
                tabControl1.TabPages.Add(mypage);
                p = mypage;
            }
            tabControl1.SelectedTab = p;
        }

        public List<string> WinformPages = new List<string>() { "ListenTab" };
        #endregion
    }
}
