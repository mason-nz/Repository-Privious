using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CC2012_CarolFormsApp
{
    public partial class KeyBoard : Form
    {
        /// <summary>
        /// 返回的电话号码
        /// </summary>
        public string ReturnNum = "";

        /// <summary>
        /// 是否呼出
        /// </summary>
        public bool IsCallOut = false;

        public KeyBoard()
        {
            InitializeComponent();
        }

        private void KeyBoard_Load(object sender, EventArgs e)
        {
            this.skinEngine1.SkinFile = "skin/" + Common.GetSkinFileName();
            this.txtTelNum.Focus();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string telNum = this.txtTelNum.Text.Trim();
            if (!IsNumber(telNum))
            {
                MessageBox.Show("电话号码只能是数字");
            }
            else if (!(rdoNum1.Checked || rdoNum2.Checked || rdoNum3.Checked))
            {
                MessageBox.Show("请选择一个外呼号码！");
            }
            else
            {
                //if (System.Text.RegularExpressions.Regex.IsMatch(telNum, @"^[1]+[3,5,8]+\d{9}$"))
                //{
                    //if (!IsLocalPhoneNum(telNum))//是否为外地（非北京）手机号码
                    //{
                    //    telNum = "0" + telNum;
                    //}
                //}
                string outNumber="";string errorMsg="";
                bitauto.sys.ncc.VerifyPhoneFormat vpf = new bitauto.sys.ncc.VerifyPhoneFormat();
                vpf.VerifyFormat("E0F3C0C3-5317-4D5E-9548-7E31A506EC37", telNum, out outNumber, out errorMsg);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    MessageBox.Show(errorMsg);
                }
                else
                {
                    //ReturnNum = (rdoNum1.Checked ? rdoNum1.Tag.ToString() : rdoNum2.Tag.ToString()) + outNumber;
                    string tag = "";
                    if (rdoNum1.Checked)
                    {
                        tag = rdoNum1.Tag.ToString();
                    }
                    else if (rdoNum2.Checked)
                    {
                        tag = rdoNum2.Tag.ToString();
                    }
                    else
                    {
                        tag = rdoNum3.Tag.ToString();
                    }

                    ReturnNum = tag + outNumber;
                    IsCallOut = true;
                    this.Close();
                }
            }

        }

        //private bool IsLocalPhoneNum(string telNum)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "LocalPhoneNumDict.xml");
        //        ds.Tables[0].DefaultView.RowFilter = "PhonePrefix=" + telNum.Substring(0, 7);
        //        DataTable dtNew = ds.Tables[0].DefaultView.ToTable();
        //        if (dtNew != null && dtNew.Rows.Count > 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        return false;
        //    }
        //}

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
            this.Close();
        }

        private void txtTelNum_TextChanged(object sender, EventArgs e)
        {
            string telNum = this.txtTelNum.Text.Trim();
            if (!IsNumber(telNum))
            {
                this.labInfo.ForeColor = Color.Red;
                this.labInfo.Text = "电话号码只能是数字";
            }
            else
            {
                this.labInfo.Text = "";
            }
        }

        private void rdoNum1_CheckedChanged(object sender, EventArgs e)
        {
            this.txtTelNum.Focus();
        }

        private void rdoNum2_CheckedChanged(object sender, EventArgs e)
        {
            this.txtTelNum.Focus();
        }
    }
}
