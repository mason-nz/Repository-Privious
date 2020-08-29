using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CC2015_HollyFormsApp
{
    public partial class Test : Form
    {
        Main main = null;

        public Test(Main main)
        {
            InitializeComponent();
            this.main = main;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            main.SetMainStatus((PhoneStatus)Enum.Parse(typeof(PhoneStatus), (sender as Button).Text));
        }
    }
}
