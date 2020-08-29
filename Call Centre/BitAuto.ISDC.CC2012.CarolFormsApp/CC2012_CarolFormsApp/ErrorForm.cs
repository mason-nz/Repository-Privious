using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CC2012_CarolFormsApp
{
    public partial class ErrorForm : Form
    {
        public string Msg
        {
            set { lblMsg.Text = value; }
            get { return lblMsg.Text; }
        }
        public ErrorForm()
        {
            InitializeComponent();
            btnOK.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
        }
    }
}
