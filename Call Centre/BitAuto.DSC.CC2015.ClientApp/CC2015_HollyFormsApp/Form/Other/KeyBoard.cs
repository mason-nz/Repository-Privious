using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CC2015_HollyFormsApp
{
    public partial class KeyBoard : Form
    {
        /// <summary>
        /// 返回的电话号码
        /// </summary>
        public string ReturnNum = "";
        public CC2015_HollyFormsApp.HollyContactHelper.ConDeviceType ConDeviceType = HollyContactHelper.ConDeviceType.未定义;

        public KeyBoard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 判断数字
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumber(string strNumber)
        {
            Regex regex = new Regex("[^0-9]");
            return !regex.IsMatch(strNumber);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (rgtest_fenji.Checked || rgtest_waixian.Checked)
            {
                if (TestPhone())
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            else
            {
                if (Phone())
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }

        private bool Phone()
        {
            string telNum = this.txtTelNum.Text.Trim();
            if (string.IsNullOrEmpty(telNum))
            {
                MessageBox.Show("外呼号码不能为空！");
                txtTelNum.Focus();
                return false;
            }
            if (!IsNumber(telNum))
            {
                MessageBox.Show("电话号码只能是数字");
                txtTelNum.Focus();
                return false;
            }
            if (CallRecordHelper.Instance.CheckPhoneAndTelIsInBlackList(telNum))
            {
                MessageBox.Show("该号码已加入免打扰，禁止进行外呼！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTelNum.Focus();
                return false;
            }
            if (!(rdoNum1.Checked || rdoNum2.Checked || rdoNum3.Checked || rdoNum4.Checked || rdoNum5.Checked))
            {
                MessageBox.Show("请选择一条热线进行外呼！");
                txtTelNum.Focus();
                return false;
            }

            string outNumber = ""; string errorMsg = "";
            if (LoginUser.LoginAreaType == AreaType.西安)
            {
                //本地区域属于西安
                VerifyPhoneFormatHelper.Instance.VerifyFormatXiAn(telNum, out outNumber, out errorMsg);
            }
            else if (LoginUser.LoginAreaType == AreaType.北京)
            {
                //本地区域属于北京
                VerifyPhoneFormatHelper.Instance.VerifyFormat(telNum, out outNumber, out errorMsg);
            }

            if (!string.IsNullOrEmpty(errorMsg))
            {
                MessageBox.Show(errorMsg);
                txtTelNum.Focus();
                return false;
            }

            string tag = "";
            if (rdoNum1.Checked)
            {
                tag = rdoNum1.Tag.ToString();
            }
            else if (rdoNum2.Checked)
            {
                tag = rdoNum2.Tag.ToString();
            }
            else if (rdoNum3.Checked)
            {
                tag = rdoNum3.Tag.ToString();
            }
            else if (rdoNum4.Checked)
            {
                tag = rdoNum4.Tag.ToString();
            }
            else if (rdoNum5.Checked)
            {
                tag = rdoNum5.Tag.ToString();
            }

            ReturnNum = tag + outNumber;
            ConDeviceType = HollyContactHelper.ConDeviceType.外线号码;
            return true;
        }

        private bool TestPhone()
        {
            string telNum = this.txtTelNum.Text.Trim();
            if (string.IsNullOrEmpty(telNum))
            {
                MessageBox.Show("外呼号码不能为空！");
                txtTelNum.Focus();
                return false;
            }
            if (!IsNumber(telNum))
            {
                MessageBox.Show("电话号码只能是数字");
                txtTelNum.Focus();
                return false;
            }

            if (rgtest_fenji.Checked)
            {
                ConDeviceType = HollyContactHelper.ConDeviceType.分机号;
                ReturnNum = telNum;
            }
            else if (rgtest_waixian.Checked)
            {
                ConDeviceType = HollyContactHelper.ConDeviceType.外线号码;
                ReturnNum = rgtest_waixian.Tag.ToString() + telNum;
            }
            return true;
        }

        private void KeyBoard_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }
    }
}
