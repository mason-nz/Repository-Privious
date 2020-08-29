using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CC2012_CarolFormsApp
{
    public partial class SysConfig : Form
    {
        private Main _myParentForm;

        public SysConfig()
        {
            InitializeComponent();
            
        }
        public SysConfig(Main form)
        {
            this._myParentForm = form;
        }


        private void SysConfig_Load(object sender, EventArgs e)
        {
            this.skinEngine1.SkinFile = "skin/" + Common.GetSkinFileName(); 
            BingSkin();
            this.SkinComboBox.SelectedValue = Common.GetSkinFileName();
        }

        /// <summary>
        /// 绑定皮肤列表并选中
        /// </summary>
        private void BingSkin()
        {
            this.SkinComboBox.DisplayMember = "showName";
            this.SkinComboBox.ValueMember = "fileName";
            this.SkinComboBox.DataSource = ReadSkins();
        }


        private DataTable ReadSkins()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("fileName");
            dt.Columns.Add("showName");
            DataRow dr;

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo("skin");
            FileInfo[] ff = di.GetFiles("*.ssk");//只取文本文档
            foreach (FileInfo temp in ff)
            {
                dr = dt.NewRow();
                dr["showName"] = temp.Name.Replace(temp.Extension, "").ToString();
                dr["fileName"] = temp.Name;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        private void SkinComboBox_TextChanged(object sender, EventArgs e)
        {
            this.skinEngine1.SkinFile = "skin/" + this.SkinComboBox.SelectedValue;
           // this._myParentForm.skinEngine1.SkinFile = "skin/" + this.SkinComboBox.SelectedValue; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Common.SaveSkin(SkinComboBox.SelectedValue.ToString());
            this.Close();
        }

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    KeyBoard k = new KeyBoard();
        //    k.ShowDialog();
        //    MessageBox.Show(k.ReturnNum);
        //}

    }
}
